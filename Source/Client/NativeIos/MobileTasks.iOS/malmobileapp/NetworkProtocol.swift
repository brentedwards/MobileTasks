//
//  NetworkProtocol.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

protocol NetworkProtocol {
    func login(serviceProvider : String, controller : UIViewController, completion : ServiceResponse) -> Void
    func hasPreviousAuthentication() -> Bool
    func getClient() -> MSClient
    func getTasks(completion: TaskResponse) -> Void
    func logout(completion: ServiceResponse) -> Void
    func upsertTask(task: MobileTask, completion: TaskSaveResponse) -> Void
}
