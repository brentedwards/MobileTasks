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
    
    @IBAction func cancelPressed(_ sender : UIBarButtonItem) {
        self.navigationController!.popViewController(animated: true)
    }
    
    @IBAction func savePressed(_ sender : UIBarButtonItem) {
        saveItem()
        self.navigationController!.popViewController(animated: true)
    }
    
    @IBAction func showDatePickerPressed(_ sender: UIButton) {
        if (self.swDueDate.isOn) {
            let datePickerView  : UIDatePicker = UIDatePicker()
            datePickerView.datePickerMode = UIDatePickerMode.date
            self.lblDateDue.inputView = datePickerView
            datePickerView.addTarget(self, action: #selector(TaskViewController.handleDatePicker(_:)), for: UIControlEvents.valueChanged)
        }
    }

    @IBAction func startDateEditing(_ sender: AnyObject) {
        if (self.swDueDate.isOn) {
            let datePickerView  : UIDatePicker = UIDatePicker()
            datePickerView.datePickerMode = UIDatePickerMode.date
            self.lblDateDue.inputView = datePickerView
            datePickerView.addTarget(self, action: #selector(TaskViewController.handleDatePicker(_:)), for: UIControlEvents.valueChanged)
        } else {
            self.lblDateDue.resignFirstResponder()
        }
    }
    
    func textFieldDidEndEditing(_ textField: UITextField)
    {
        self.navigationController!.popViewController(animated: true);
    }
    
    func textFieldShouldEndEditing(_ textField: UITextField) -> Bool
    {
        return true
    }
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool
    {
        saveItem()
        
        textField.resignFirstResponder()
        return true
    }
    
    func handleDatePicker(_ sender: UIDatePicker) {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "dd MMM yyyy"
        self.lblDateDue.text = dateFormatter.string(from: sender.date)
    }
    
    // Delegate
    
    func saveItem()
    {
        if let text = self.lblTaskDetails.text {
            let task : MobileTask = MobileTask()
            task.taskDescription = text
            task.isCompleted = swCompleted.isOn
            if (self.swDueDate.isOn) {
                let dateFormatter = DateFormatter()
                dateFormatter.dateFormat = "dd MMM yyyy"
                task.dateDue = dateFormatter.date(from: self.lblDateDue.text!)
            } else {
                task.dateDue = nil
            }
            self.delegate?.didSaveItem(task)
        }
    }
}
