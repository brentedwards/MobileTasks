//
//  LoginViewController.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class LoginViewController: BaseViewController {
    
    @IBOutlet weak var btnMicrosoft: UIButton!
    @IBOutlet weak var btnFacebook: UIButton!
    @IBOutlet weak var btnGoogle: UIButton!
    @IBOutlet weak var btnTwitter: UIButton!
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
        if (networkService!.hasPreviousAuthentication()) {
            self.performSegueWithIdentifier("sguToList", sender: self)
        }
        
        let appDelegate = UIApplication.sharedApplication().delegate as! AppDelegate
        let navigationController = appDelegate.window?.rootViewController as! UINavigationController
        navigationController.navigationBarHidden = true
    }
    
    @IBAction func btnMicrosoft_TouchDown(sender: AnyObject) {
        processLogin(MicrosoftProvider)
    }
    
    @IBAction func btnFacebook_TouchDown(sender: AnyObject) {
        processLogin(FacebookProvider)
    }
    
    @IBAction func btnGoogle_TouchDown(sender: AnyObject) {
        processLogin(GoogleProvider)
    }
    
    @IBAction func btnTwitter_TouchDown(sender: AnyObject) {
        processLogin(TwitterProvider)
    }
    
    func processLogin(provider: String) {

        networkService!.login(provider, controller: self, completion: { (error: NSError?) -> Void in
            if (error == nil) {
                self.performSegueWithIdentifier("sguToList", sender: self)
            }
        })
    }
    
    override func viewDidAppear(animated: Bool) {
        let appDelegate = UIApplication.sharedApplication().delegate as! AppDelegate
        let navigationController = appDelegate.window?.rootViewController as! UINavigationController
        
        if (navigationController.viewControllers.count > 1) {
            navigationController.viewControllers.removeAll()
            navigationController.viewControllers.append(self)
        }
    }
}