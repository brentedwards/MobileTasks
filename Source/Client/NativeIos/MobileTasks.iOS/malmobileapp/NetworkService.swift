//
//  NetworkService.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class NetworkService : NetworkProtocol {
    var mobileClient : MSClient?
    
    init () {
        mobileClient = MSClient(applicationURLString: ServiceUrl)
    }
    
    func login(serviceProvider: String, controller: UIViewController, completion: ServiceResponse) {
 
        
            mobileClient?.loginWithProvider(serviceProvider,  controller: controller, animated: false, completion: { (user : MSUser?, error : NSError?) -> Void in
            if (user != nil) {
                let currentToken : String = (user?.mobileServiceAuthenticationToken!)!
                let currentUserId : String = (user?.userId)!
                
                NSUserDefaults.standardUserDefaults().setObject(serviceProvider, forKey: LastUsedProvider)
                NSUserDefaults.standardUserDefaults().setObject(currentUserId, forKey: serviceProvider + Token)
                NSUserDefaults.standardUserDefaults().setObject(currentToken, forKey: serviceProvider + UserId)
                
                completion(true, nil, nil)
                return
            }
            completion(false, nil, error)
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
                
                mobileClient?.currentUser = MSUser(userId: currentUserId)
                mobileClient?.currentUser!.mobileServiceAuthenticationToken = currenttoken
                return true
            }
        }
        return false
    }
    
    func getClient() -> MSClient {
        return mobileClient!
    }
}