//
//  NetworkProtocol.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

protocol NetworkProtocol {
    func login(_ serviceProvider : String, controller : UIViewController, completion : ServiceResponse) -> Void
    func hasPreviousAuthentication() -> Bool
    func getClient() -> MSClient
    func getTasks(_ completion: TaskResponse) -> Void
    func logout(_ completion: ServiceResponse) -> Void
    func upsertTask(_ task: MobileTask, completion: TaskSaveResponse) -> Void
}
