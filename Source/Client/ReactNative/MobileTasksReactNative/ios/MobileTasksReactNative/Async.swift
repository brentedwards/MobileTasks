//
//  Async.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

typealias ServiceResponse = (Array<String>?) -> Void
typealias TaskResponse = (Array<MobileTask>?, Error?) -> Void
typealias TaskSaveResponse = (MobileTask?, Error?) -> Void
typealias PreviousAuthenticationResponse = (Array<String>?) -> Void
