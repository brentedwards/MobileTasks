//
//  NetworkService.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

typealias ServiceResponse = (Array<Any>?) -> Void

@objc(NetworkService)
class NetworkService : NSObject {
    static var mobileClient : MSClient?
  
    func getMobileClient() -> MSClient {
        if (NetworkService.mobileClient == nil) {
            NetworkService.mobileClient = MSClient(applicationURLString: ServiceUrl)
        }
      return NetworkService.mobileClient!
    }
  
    @objc(login:completion:)
    func login(_ serviceProvider: String, completion: @escaping ServiceResponse) {
      let controller = getViewController();
    
        getMobileClient().login(withProvider: serviceProvider,  controller: controller, animated: false, completion: { (user : MSUser?, error : Error?) -> Void in
            if (user != nil) {
                let currentToken : String = (user?.mobileServiceAuthenticationToken!)!
                let currentUserId : String = (user?.userId)!
                
                UserDefaults.standard.set(serviceProvider, forKey: LastUsedProvider)
                UserDefaults.standard.set(currentUserId, forKey: serviceProvider + UserId)
                UserDefaults.standard.set(currentToken, forKey: serviceProvider + Token)
                
              completion([""])
                return
            }
            completion([error.debugDescription])
            return
        })
    }
  
    func getViewController() -> UIViewController {
      let window = UIApplication.shared.delegate!.window
        var viewController = window!!.rootViewController
        if(viewController is UINavigationController){
          viewController = (viewController as! UINavigationController).visibleViewController
        }
        return viewController!
    }
  
  @objc(hasPreviousAuthentication:)
  func hasPreviousAuthentication(_ completion: @escaping ServiceResponse) {
        
        let userDefaults : UserDefaults = UserDefaults.standard
        
        
        
        if (userDefaults.object(forKey: LastUsedProvider) != nil) {
            let serviceProvider : String = userDefaults.object(forKey: LastUsedProvider) as! String
            let currenttoken : String = userDefaults.object(forKey: serviceProvider + Token) as! String
            if (userDefaults.object(forKey: serviceProvider + Token) != nil) {
                let currentUserId : String = userDefaults.object(forKey: serviceProvider + UserId) as! String
                
                getMobileClient().currentUser = MSUser(userId: currentUserId)
                getMobileClient().currentUser!.mobileServiceAuthenticationToken = currenttoken
                completion(["", "true"])
                return
            }
        }
        completion(["", "false"])
    }
  
    @objc(getTasks:)
    func getTasks(_ completion: @escaping ServiceResponse) -> Void {
        getMobileClient().invokeAPI("task", body: nil, httpMethod: "GET", parameters: nil, headers: nil, completion: {(result : Any?,
            response : HTTPURLResponse?,
            error : Error?) in
            if (error == nil) {
                completion(["", result as! [NSDictionary]])
            } else {
                completion([error.debugDescription])
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
  
    @objc(logout:)
    func logout(_ completion: @escaping ServiceResponse) -> Void {
        getMobileClient().logout(completion: { (error: Error?) in
            if (error == nil) {
                UserDefaults.standard.removeObject(forKey: LastUsedProvider)
              completion([""])
              return
            }
            completion([error.debugDescription])
        })
    }
  
  @objc(upsertTask:completion:)
    func upsertTask(_ task: String, completion: @escaping ServiceResponse) -> Void {
        
        //let jsonTask : NSMutableDictionary = self.encodeJson(task) as NSMutableDictionary
        getMobileClient().invokeAPI("task", body: task, httpMethod: "POST", parameters: nil, headers: nil, completion: { (result : Any?, response: HTTPURLResponse?, error : Error?) in
            if (error == nil) {
                completion(["", result! as! NSDictionary])
            } else {
                completion([error.debugDescription])
            }
        })
    }
}
