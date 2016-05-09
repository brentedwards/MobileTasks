// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
import Foundation
import UIKit
import CoreData

class TasksViewController: BaseTableViewController, TaskProtocol {
    
    var Tasks : [MobileTask] = []
    
    lazy var TasksViewController: NSFetchedResultsController = {
        let fetchRequest = NSFetchRequest(entityName: "TodoItem")
        let managedObjectContext = (UIApplication.sharedApplication().delegate as! AppDelegate).managedObjectContext!
        
        // show only non-completed items
        fetchRequest.predicate = NSPredicate(format: "complete != true")
        
        // sort by item text
        fetchRequest.sortDescriptors = [NSSortDescriptor(key: "createdAt", ascending: true)]
        
        // Note: if storing a lot of data, you should specify a cache for the last parameter
        // for more information, see Apple's documentation: http://go.microsoft.com/fwlink/?LinkId=524591&clcid=0x409
        let resultsController = NSFetchedResultsController(fetchRequest: fetchRequest, managedObjectContext: managedObjectContext, sectionNameKeyPath: nil, cacheName: nil)
        
        return resultsController
    }()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        UIApplication.sharedApplication().networkActivityIndicatorVisible = true
        networkService?.getTasks({ (results: [MobileTask]?, error: NSError?) -> Void in
            UIApplication.sharedApplication().networkActivityIndicatorVisible = false
            if (error == nil) {
                self.Tasks = results!
                self.tableView.reloadData()
            } else {
                self.handleNetworkCallError(error!)
            }
        })
    }
    
    override func viewDidAppear(animated: Bool) {
        super.viewDidAppear(animated)
        let appDelegate = UIApplication.sharedApplication().delegate as! AppDelegate
        let navigationController = appDelegate.window?.rootViewController as! UINavigationController
        
        if (navigationController.viewControllers.count > 1 ) {
            for i in (0...navigationController.viewControllers.count - 1).reverse() {
                let controller = navigationController.viewControllers[i] as? TaskProtocol
                if controller == nil {
                    navigationController.viewControllers.removeAtIndex(i)
                }
            }
        }
        
        navigationController.navigationBarHidden = false
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
    // MARK: Table Controls
    
    override func tableView(tableView: UITableView, canEditRowAtIndexPath indexPath: NSIndexPath) -> Bool {
        return true
    }
    
    override func tableView(tableView: UITableView, editingStyleForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCellEditingStyle {
        return UITableViewCellEditingStyle.Delete
    }
    
    override func tableView(tableView: UITableView, titleForDeleteConfirmationButtonForRowAtIndexPath indexPath: NSIndexPath) -> String? {
        return "Complete"
    }
    
    //    override func tableView(tableView: UITableView, commitEditingStyle editingStyle: UITableViewCellEditingStyle, forRowAtIndexPath indexPath: NSIndexPath)
    //    {
    //        let record = self.fetchedResultController.objectAtIndexPath(indexPath) as! NSManagedObject
    //        var item = self.store!.tableItemFromManagedObject(record)
    //        item["complete"] = true
    //
    //        UIApplication.sharedApplication().networkActivityIndicatorVisible = true
    //
    //        self.table!.update(item) { (error) -> Void in
    //            UIApplication.sharedApplication().networkActivityIndicatorVisible = false
    //            if error != nil {
    //                print("Error: \(error!.description)")
    //                return
    //            }
    //        }
    //    }
    
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int
    {
        return Tasks.count;
    }
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        let CellIdentifier = "TaskItemCell"
        
        var cell = tableView.dequeueReusableCellWithIdentifier(CellIdentifier, forIndexPath: indexPath)
        
        let taskCell : TaskItemCell = cell as! TaskItemCell
        taskCell.btnStatus.addTarget(self, action: #selector (self.switchIsChanged(_:)), forControlEvents: UIControlEvents.TouchUpInside)
        cell = configureCell(cell, indexPath: indexPath)
        
        
        return cell
    }
    
    func switchIsChanged(myButton: UIButton) {
        let task : MobileTask = Tasks[myButton.tag]
        UIApplication.sharedApplication().networkActivityIndicatorVisible = true
        
        if (!task.isCompleted) {
            task.isCompleted = true
            networkService?.upsertTask(task, completion: { (mobileTask :MobileTask?, error: NSError?) in
                UIApplication.sharedApplication().networkActivityIndicatorVisible = false
                if (error == nil) {
                    myButton.setImage(self.getImage(true, dateDue: task.dateDue), forState: UIControlState.Normal)
                } else {
                    task.isCompleted = false
                    self.handleNetworkCallError(error!)
                }
            })
        }
    }
    
    func configureCell(cell: UITableViewCell, indexPath: NSIndexPath) -> UITableViewCell {
        let item : MobileTask = Tasks[indexPath.item]
        
        // Set the label on the cell and make sure the label color is black (in case this cell
        // has been reused and was previously greyed out
        
        let taskCell = cell as! TaskItemCell
        taskCell.lblTaskTitle.text = item.taskDescription
        taskCell.lblDateDue.text = getDueDate(item.dateDue)
        taskCell.btnStatus.tag = indexPath.item
        taskCell.btnStatus.setImage(getImage(item.isCompleted, dateDue: item.dateDue), forState: UIControlState.Normal)
        return cell
    }
    
    func getDueDate(dateInfo : NSDate?) -> String {
        if (dateInfo == nil) {
            return "No Due Date";
        } else {
            let dateFormatter = NSDateFormatter()
            
            let theDateFormat = NSDateFormatterStyle.MediumStyle
            let theTimeFormat = NSDateFormatterStyle.ShortStyle
            
            dateFormatter.dateStyle = theDateFormat
            dateFormatter.timeStyle = theTimeFormat
            
            return dateFormatter.stringFromDate(dateInfo!)
        }
    }
    
    func getImage(completed: Bool, dateDue: NSDate?) -> UIImage {
        if (dateDue == nil) {
            return UIImage(named: "Incomplete")!
        } else if (completed) {
            return UIImage(named: "Completed")!
        } else if (dateDue!.timeIntervalSinceNow.isSignMinus) {
            return UIImage(named: "PastDue")!
        } else {
            return UIImage(named: "Incomplete")!
        }
    }
    
    @IBAction func addItem(sender : AnyObject) {
        self.performSegueWithIdentifier("sguToItemDetail", sender: self)
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject!)
    {
        if segue.identifier == "sguToItemDetail" {
            let taskController = segue.destinationViewController as! TaskViewController
            taskController.delegate = self
        }
    }
    
    func didSaveItem(task : MobileTask) {
        UIApplication.sharedApplication().networkActivityIndicatorVisible = true
        networkService?.upsertTask(task, completion: { (mobileTask :MobileTask?, error: NSError?) in
            UIApplication.sharedApplication().networkActivityIndicatorVisible = false
            if (error == nil) {
                self.Tasks.append(task)
                self.tableView.reloadData()
            } else {
                task.isCompleted = false
                self.handleNetworkCallError(error!)
            }
        })
    }
}
