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
                let returnValue : Array<MobileTask> = self.decodeJsonList(result!)
                completion(returnValue, nil)
            } else {
                completion(nil, error!)
            }
            
        })
    }
    

    func decodeJsonList(target: AnyObject) -> Array<MobileTask> {
        var returnValue : Array<MobileTask> = Array<MobileTask>()
        
        let dictionary : [NSDictionary] = target as! [NSDictionary]
        
        for task in dictionary {
            let mobileTask = decodeJson(task)
            returnValue.append(mobileTask)
        }
        return returnValue;
    }
    
    func decodeJson(task: NSDictionary) -> MobileTask {
        let mobileTask : MobileTask = MobileTask()
        mobileTask.id = task["id"] as! Int
        mobileTask.sid = task["sid"] as! String
        mobileTask.taskDescription = task["description"] as! String
        mobileTask.dateCreated = task["dateCreated"] as? NSDate
        mobileTask.dateDue = task["dateDue"] as? NSDate
        mobileTask.dateCompleted = task["dateCompleted"] as? NSDate
        mobileTask.isCompleted = task["isCompleted"] as! Bool
        return mobileTask
    }
    
    func encodeJson(source: MobileTask) -> NSMutableDictionary {
        let jsonObject: NSMutableDictionary = [
            "id" : source.id,
            "sid" : source.sid,
            "description" : source.taskDescription,
            "isCompleted": source.isCompleted
        ]
        if (source.dateCreated != nil) {
            jsonObject.setValue(source.dateCreated!, forKey: "dateCreated")
        }
        if (source.dateDue != nil) {
            jsonObject["dateDue"] = source.dateDue!
        }
        if (source.dateCompleted != nil) {
            jsonObject["dateCompleted"] = source.dateCompleted!
        }

        return jsonObject
    }
    
    func logout(completion: ServiceResponse) -> Void {
        NetworkService.mobileClient?.logoutWithCompletion({ (error: NSError?) in
            if (error == nil) {
                NSUserDefaults.standardUserDefaults().removeObjectForKey(LastUsedProvider)
            }
            completion(error)
        })
    }
    
    func upsertTask(task: MobileTask, completion: TaskSaveResponse) -> Void {
        
        let jsonTask : NSMutableDictionary = self.encodeJson(task) as NSMutableDictionary
        NetworkService.mobileClient?.invokeAPI("task", body: jsonTask, HTTPMethod: "POST", parameters: nil, headers: nil, completion: { (result : AnyObject?, response: NSHTTPURLResponse?, error : NSError?) in
            if (error == nil) {
                completion(self.decodeJson(result! as! NSDictionary), nil)
            } else {
                completion(nil, error)
            }
        })
    }
}