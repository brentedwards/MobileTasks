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
    @IBOutlet weak var lblDateDue: UILabel!
    @IBOutlet weak var btnDetails: UIImageView!
    @IBOutlet weak var btnStatus: UIButton!
    
    var onButtonTapped : (() -> Void)? = nil
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        
    }
    
    @IBAction func favoriteClicked(_ sender: UIButton) {
        if let onButtonTapped = self.onButtonTapped {
            onButtonTapped()
        }
    }
    

}
