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
    
    func login(_ serviceProvider: String, controller: UIViewController, completion: @escaping ServiceResponse) {
        NetworkService.mobileClient?.login(withProvider: serviceProvider,  controller: controller, animated: false, completion: { (user : MSUser?, error : Error?) -> Void in
            if (user != nil) {
                let currentToken : String = (user?.mobileServiceAuthenticationToken!)!
                let currentUserId : String = (user?.userId)!
                
                UserDefaults.standard.set(serviceProvider, forKey: LastUsedProvider)
                UserDefaults.standard.set(currentUserId, forKey: serviceProvider + UserId)
                UserDefaults.standard.set(currentToken, forKey: serviceProvider + Token)
                
                completion(nil)
                return
            }
            completion((error as? NSError)!)
            return
        })
    }
    
    func hasPreviousAuthentication() -> Bool {
        
        let userDefaults : UserDefaults = UserDefaults.standard
        
        
        
        if (userDefaults.object(forKey: LastUsedProvider) != nil) {
            let serviceProvider : String = userDefaults.object(forKey: LastUsedProvider) as! String
            let currenttoken : String = userDefaults.object(forKey: serviceProvider + Token) as! String
            if (userDefaults.object(forKey: serviceProvider + Token) != nil) {
                let currentUserId : String = userDefaults.object(forKey: serviceProvider + UserId) as! String
                
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
    
    func getTasks(_ completion: @escaping TaskResponse) -> Void {
        NetworkService.mobileClient?.invokeAPI("task", body: nil, httpMethod: "GET", parameters: nil, headers: nil, completion: {(result : Any?,
            response : HTTPURLResponse?,
            error : Error?) in
            if (error == nil) {
                let returnValue : Array<MobileTask> = self.decodeJsonList((result as AnyObject))
                completion(returnValue, nil)
            } else {
                completion(nil, error!)
            }
            
        })
    }
    

    func decodeJsonList(_ target: AnyObject) -> Array<MobileTask> {
        var returnValue : Array<MobileTask> = Array<MobileTask>()
        
        let dictionary : [NSDictionary] = target as! [NSDictionary]
        
        for task in dictionary {
            let mobileTask = decodeJson(task)
            returnValue.append(mobileTask)
        }
        return returnValue;
    }
    
    func decodeJson(_ task: NSDictionary) -> MobileTask {
        let mobileTask : MobileTask = MobileTask()
        mobileTask.id = task["id"] as! Int
        mobileTask.sid = task["sid"] as! String
        mobileTask.taskDescription = task["description"] as! String
        mobileTask.dateCreated = task["dateCreated"] as? Date
        mobileTask.dateDue = task["dateDue"] as? Date
        mobileTask.dateCompleted = task["dateCompleted"] as? Date
        mobileTask.isCompleted = task["isCompleted"] as! Bool
        return mobileTask
    }
    
    func encodeJson(_ source: MobileTask) -> NSMutableDictionary {
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
    
    func logout(_ completion: @escaping ServiceResponse) -> Void {
        NetworkService.mobileClient?.logout(completion: { (error: Error?) in
            if (error == nil) {
                UserDefaults.standard.removeObject(forKey: LastUsedProvider)
            }
            completion((error as? NSError)!)
        })
    }
    
    func upsertTask(_ task: MobileTask, completion: @escaping TaskSaveResponse) -> Void {
        
        let jsonTask : NSMutableDictionary = self.encodeJson(task) as NSMutableDictionary
        NetworkService.mobileClient?.invokeAPI("task", body: jsonTask, httpMethod: "POST", parameters: nil, headers: nil, completion: { (result : Any?, response: HTTPURLResponse?, error : Error?) in
            if (error == nil) {
                completion(self.decodeJson(result! as! NSDictionary), nil)
            } else {
                completion(nil, (error as? NSError)!)
            }
        })
    }
}
