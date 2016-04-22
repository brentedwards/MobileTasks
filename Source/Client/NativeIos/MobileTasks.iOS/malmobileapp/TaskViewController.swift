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

protocol TaskDelegate {
    func didSaveItem(task : MobileTask)
}

class TaskViewController: UIViewController,  UIBarPositioningDelegate, UITextFieldDelegate {
    
    @IBOutlet weak var lblTaskDescription: UITextField!
    @IBOutlet weak var pkDateDue: UIDatePicker!
    @IBOutlet weak var swTaskComplete: UISwitch!

    var delegate : TaskDelegate?
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
        self.lblTaskDescription.delegate = self
        self.lblTaskDescription.becomeFirstResponder()
    }
    
    @IBAction func cancelPressed(sender : UIBarButtonItem) {
        self.lblTaskDescription.resignFirstResponder()
    }
    
    @IBAction func savePressed(sender : UIBarButtonItem) {
        saveItem()
        self.lblTaskDescription.resignFirstResponder()
    }
    
    // Textfield
    
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
    
    // Delegate
    
    func saveItem()
    {
        if let text = self.lblTaskDescription.text {
            let task : MobileTask = MobileTask()
            task.taskDescription = text
            task.isCompleted = swTaskComplete.on
            task.dateDue = self.pkDateDue.date
            self.delegate?.didSaveItem(task)
        }
    }
}