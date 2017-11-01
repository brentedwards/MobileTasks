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
    
        getMobileClient().login(withProvider: serviceProvider, urlScheme: "commagenicmobiletasks", controller: controller, animated: false, completion: { (user : MSUser?, error : Error?) -> Void in
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
  
    @objc(getUserInfo:)
    func getUserInfo(_ completion: @escaping ServiceResponse) {
        if (getMobileClient().currentUser != nil) {
          let user = getMobileClient().currentUser
            let returnValue = user?.mobileServiceAuthenticationToken!
          completion([returnValue!, user?.userId])
        } else {
            completion(["", ""])
        }
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
}
