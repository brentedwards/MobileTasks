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
    
    lazy var TasksViewController: NSFetchedResultsController = { //() -> <error type> in
        let fetchRequest = NSFetchRequest(entityName: "TodoItem")
        let managedObjectContext = (UIApplication.shared.delegate as! AppDelegate).managedObjectContext!
        
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
        
        UIApplication.shared.isNetworkActivityIndicatorVisible = true
        networkService?.getTasks({ (results: [MobileTask]?, error: NSError?) -> Void in
            UIApplication.shared.isNetworkActivityIndicatorVisible = false
            if (error == nil) {
                self.Tasks = results!
                self.tableView.reloadData()
            } else {
                self.handleNetworkCallError(error!)
            }
        })
    }
    
    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        let appDelegate = UIApplication.shared.delegate as! AppDelegate
        let navigationController = appDelegate.window?.rootViewController as! UINavigationController
        
        if (navigationController.viewControllers.count > 1 ) {
            for i in (0...navigationController.viewControllers.count - 1).reversed() {
                let controller = navigationController.viewControllers[i] as? TaskProtocol
                if controller == nil {
                    navigationController.viewControllers.remove(at: i)
                }
            }
        }
        
        navigationController.isNavigationBarHidden = false
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
    // MARK: Table Controls
    
    override func tableView(_ tableView: UITableView, canEditRowAt indexPath: IndexPath) -> Bool {
        return true
    }
    
    override func tableView(_ tableView: UITableView, editingStyleForRowAt indexPath: IndexPath) -> UITableViewCellEditingStyle {
        return UITableViewCellEditingStyle.delete
    }
    
    override func tableView(_ tableView: UITableView, titleForDeleteConfirmationButtonForRowAt indexPath: IndexPath) -> String? {
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
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int
    {
        return Tasks.count;
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let CellIdentifier = "TaskItemCell"
        
        var cell = tableView.dequeueReusableCell(withIdentifier: CellIdentifier, for: indexPath)
        
        let taskCell : TaskItemCell = cell as! TaskItemCell
        taskCell.btnStatus.addTarget(self, action: #selector (self.switchIsChanged(_:)), for: UIControlEvents.touchUpInside)
        cell = configureCell(cell, indexPath: indexPath)
        
        
        return cell
    }
    
    func switchIsChanged(_ myButton: UIButton) {
        let task : MobileTask = Tasks[myButton.tag]
        UIApplication.shared.isNetworkActivityIndicatorVisible = true
        
        if (!task.isCompleted) {
            task.isCompleted = true
            networkService?.upsertTask(task, completion: { (mobileTask :MobileTask?, error: NSError?) in
                UIApplication.shared.isNetworkActivityIndicatorVisible = false
                if (error == nil) {
                    myButton.setImage(self.getImage(true, dateDue: task.dateDue), for: UIControlState())
                } else {
                    task.isCompleted = false
                    self.handleNetworkCallError(error!)
                }
            })
        }
    }
    
    func configureCell(_ cell: UITableViewCell, indexPath: IndexPath) -> UITableViewCell {
        let item : MobileTask = Tasks[(indexPath as NSIndexPath).item]
        
        // Set the label on the cell and make sure the label color is black (in case this cell
        // has been reused and was previously greyed out
        
        let taskCell = cell as! TaskItemCell
        taskCell.lblTaskTitle.text = item.taskDescription
        taskCell.lblDateDue.text = getDueDate(item.dateDue as Date?)
        taskCell.btnStatus.tag = (indexPath as NSIndexPath).item
        taskCell.btnStatus.setImage(getImage(item.isCompleted, dateDue: item.dateDue as Date?), for: UIControlState())
        return cell
    }
    
    func getDueDate(_ dateInfo : Date?) -> String {
        if (dateInfo == nil) {
            return "No Due Date";
        } else {
            let dateFormatter = DateFormatter()
            
            let theDateFormat = DateFormatter.Style.medium
            let theTimeFormat = DateFormatter.Style.short
            
            dateFormatter.dateStyle = theDateFormat
            dateFormatter.timeStyle = theTimeFormat
            
            return dateFormatter.string(from: dateInfo!)
        }
    }
    
    func getImage(_ completed: Bool, dateDue: Date?) -> UIImage {
        if (dateDue == nil) {
            return UIImage(named: "Incomplete")!
        } else if (completed) {
            return UIImage(named: "Completed")!
        } else if (dateDue!.timeIntervalSinceNow.sign == .minus) {
            return UIImage(named: "PastDue")!
        } else {
            return UIImage(named: "Incomplete")!
        }
    }
    
    @IBAction func addItem(_ sender : AnyObject) {
        self.performSegue(withIdentifier: "sguToItemDetail", sender: self)
    }
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any!)
    {
        if segue.identifier == "sguToItemDetail" {
            let taskController = segue.destination as! TaskViewController
            taskController.delegate = self
        }
    }
    
    func didSaveItem(_ task : MobileTask) {
        UIApplication.shared.isNetworkActivityIndicatorVisible = true
        networkService?.upsertTask(task, completion: { (mobileTask :MobileTask?, error: NSError?) in
            UIApplication.shared.isNetworkActivityIndicatorVisible = false
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
