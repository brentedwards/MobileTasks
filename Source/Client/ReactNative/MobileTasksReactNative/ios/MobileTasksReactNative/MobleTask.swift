//
//  MobleTask.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 4/4/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class MobileTask : NSObject {
    var id : Int = 0
    var sid : String = ""
    var dateCreated : Date? = nil
    var dateCompleted : Date? = nil
    var taskDescription: String = ""
    var isCompleted : Bool = false
    var dateDue : Date? = nil
}


