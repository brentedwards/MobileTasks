//
//  NetworkService.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class NetworkService : NetworkProtocol {
    static var mobileClient : MSClient?
    
    init () {
        if (NetworkService.mobileClient == nil) {
            NetworkService.mobileClient = MSClient(applicationURLString: ServiceUrl)
        }
    }
    
    func login(serviceProvider: String, controller: UIViewController, completion: ServiceResponse) {
            NetworkService.mobileClient?.loginWithProvider(serviceProvider,  controller: controller, animated: false, completion: { (user : MSUser?, error : NSError?) -> Void in
            if (user != nil) {
                let currentToken : String = (user?.mobileServiceAuthenticationToken!)!
                let currentUserId : String = (user?.userId)!
                
                NSUserDefaults.standardUserDefaults().setObject(serviceProvider, forKey: LastUsedProvider)
                NSUserDefaults.standardUserDefaults().setObject(currentUserId, forKey: serviceProvider + UserId)
                NSUserDefaults.standardUserDefaults().setObject(currentToken, forKey: serviceProvider + Token)
                
                completion(nil)
                return
            }
            completion(error)
            return
        })
    }
    
    func hasPreviousAuthentication() -> Bool {
        
        let userDefaults : NSUserDefaults = NSUserDefaults.standardUserDefaults()
        
        
        
        if (userDefaults.objectForKey(LastUsedProvider) != nil) {
            let serviceProvider : String = userDefaults.objectForKey(LastUsedProvider) as! String
            let currenttoken : String = userDefaults.objectForKey(serviceProvider + Token) as! String
            if (userDefaults.objectForKey(serviceProvider + Token) != nil) {
                let currentUserId : String = userDefaults.objectForKey(serviceProvider + UserId) as! String
                
                NetworkService.mobileClient?.currentUser = MSUser(userId: currentUserId)
                NetworkService.mobileClient?.currentUser!.mobileServiceAuthenticationToken = currenttoken
                return true
            }
        }
        return false
    }
    
    func getClient() -> MSClient {
        return NetworkService.mobileClient!
    }
    
    func getTasks(completion: TaskResponse) -> Void {
        NetworkService.mobileClient?.invokeAPI("task", body: nil, HTTPMethod: "GET", parameters: nil, headers: nil, completion: {(result : AnyObject?,
            response : NSHTTPURLResponse?,
            error : NSError?) -> Void in
            if (error == nil) {
                let returnValue : Array<MobileTask> = self.decodeJson(result!)
                completion(returnValue, nil)
            } else {
                completion(nil, error!)
            }
            
        })
    }
    
    func decodeJson(target: AnyObject) -> Array<MobileTask> {
        var returnValue : Array<MobileTask> = Array<MobileTask>()

        let dictionary : [NSDictionary] = target as! [NSDictionary]
        
        for task in dictionary {
            let mobileTask : MobileTask = MobileTask()
            mobileTask.id = task["id"] as! Int
            mobileTask.sid = task["sid"] as! String
            mobileTask.taskDescription = task["description"] as! String
            mobileTask.dateCreated = task["dateCreated"] as? NSDate
            mobileTask.dateDue = task["dateDue"] as? NSDate
            mobileTask.dateCompleted = task["dateCompleted"] as? NSDate
            mobileTask.isCompleted = task["isCompleted"] as! Bool
            returnValue.append(mobileTask)
        }
        return returnValue;
    }
    
    func logout(completion: ServiceResponse) -> Void {
        NetworkService.mobileClient?.logoutWithCompletion({ (error: NSError?) in
            if (error == nil) {
                NSUserDefaults.standardUserDefaults().removeObjectForKey(LastUsedProvider)
                //.setObject(serviceProvider, forKey: LastUsedProvider)
            }
            completion(error)
        })
    }
}