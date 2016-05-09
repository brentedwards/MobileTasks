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

class TaskViewController: UIViewController,  UIBarPositioningDelegate, UITextFieldDelegate {
    
    @IBOutlet weak var lblTaskDetails: UITextField!
    @IBOutlet weak var swDueDate: UISwitch!
    @IBOutlet weak var btnShowDueDate: UIButton!
    @IBOutlet weak var swCompleted: UISwitch!
    @IBOutlet weak var lblDateDue: UITextField!
    
    var delegate : TaskProtocol?
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
        self.lblTaskDetails.becomeFirstResponder()
    }
    
    @IBAction func cancelPressed(sender : UIBarButtonItem) {
        self.navigationController?.popViewControllerAnimated(true)
    }
    
    @IBAction func savePressed(sender : UIBarButtonItem) {
        saveItem()
        self.navigationController?.popViewControllerAnimated(true)
    }
    
    @IBAction func showDatePickerPressed(sender: UIButton) {
        if (self.swDueDate.on) {
            let datePickerView  : UIDatePicker = UIDatePicker()
            datePickerView.datePickerMode = UIDatePickerMode.Date
            self.lblDateDue.inputView = datePickerView
            datePickerView.addTarget(self, action: #selector(TaskViewController.handleDatePicker(_:)), forControlEvents: UIControlEvents.ValueChanged)
        }
    }

    @IBAction func startDateEditing(sender: AnyObject) {
        if (self.swDueDate.on) {
            let datePickerView  : UIDatePicker = UIDatePicker()
            datePickerView.datePickerMode = UIDatePickerMode.Date
            self.lblDateDue.inputView = datePickerView
            datePickerView.addTarget(self, action: #selector(TaskViewController.handleDatePicker(_:)), forControlEvents: UIControlEvents.ValueChanged)
        } else {
            self.lblDateDue.resignFirstResponder()
        }
    }
    
    func textFieldDidEndEditing(textField: UITextField)
    {
        self.navigationController?.popViewControllerAnimated(true);
    }
    
    func textFieldShouldEndEditing(textField: UITextField) -> Bool
    {
        return true
    }
    
    func textFieldShouldReturn(textField: UITextField) -> Bool
    {
        saveItem()
        
        textField.resignFirstResponder()
        return true
    }
    
    func handleDatePicker(sender: UIDatePicker) {
        let dateFormatter = NSDateFormatter()
        dateFormatter.dateFormat = "dd MMM yyyy"
        self.lblDateDue.text = dateFormatter.stringFromDate(sender.date)
    }
    
    // Delegate
    
    func saveItem()
    {
        if let text = self.lblTaskDetails.text {
            let task : MobileTask = MobileTask()
            task.taskDescription = text
            task.isCompleted = swCompleted.on
            if (self.swDueDate.on) {
                let dateFormatter = NSDateFormatter()
                dateFormatter.dateFormat = "dd MMM yyyy"
                task.dateDue = dateFormatter.dateFromString(self.lblDateDue.text!)
            } else {
                task.dateDue = nil
            }
            self.delegate?.didSaveItem(task)
        }
    }
}