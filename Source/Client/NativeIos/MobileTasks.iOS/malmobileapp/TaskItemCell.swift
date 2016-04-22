//
//  TaskItemCell.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 4/7/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class TaskItemCell : UITableViewCell {
    
    @IBOutlet weak var lblTaskTitle: UILabel!
    @IBOutlet weak var swTaskComplete: UISwitch!
    
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        
    }
    

}