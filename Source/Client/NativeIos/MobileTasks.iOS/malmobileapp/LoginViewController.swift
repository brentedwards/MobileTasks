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
        
        setBackgroundColor()
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
                let appDelegate = UIApplication.sharedApplication().delegate as! AppDelegate
                let navigationController = appDelegate.window?.rootViewController as! UINavigationController
                navigationController.navigationBarHidden = false
                self.performSegueWithIdentifier("sguToList", sender: self)
            }
        })
    }
    
    func setBackgroundColor() {
        let startColor = UIColor(red: (180 / 255), green: (236 / 255), blue: (81 / 255), alpha: 1)  //"#B4EC51"
        let endColor = UIColor(red: (66 / 255), green: (147 / 255), blue: (33 / 255), alpha: 1) //"#429321"
        
        let gl = CAGradientLayer()
        gl.colors = [startColor.CGColor, endColor.CGColor]
        gl.locations = [0.0, 1.0]
        
        self.view.backgroundColor = UIColor.clearColor()
        gl.frame = self.view.bounds
        self.view.layer.insertSublayer(gl, atIndex: 0)
    }
    
    override func viewDidAppear(animated: Bool) {
        super.viewDidAppear(false)
        let appDelegate = UIApplication.sharedApplication().delegate as! AppDelegate
        let navigationController = appDelegate.window?.rootViewController as! UINavigationController
        
        if (navigationController.viewControllers.count > 1) {
            for i in (0...navigationController.viewControllers.count - 1).reverse() {
                let controller = navigationController.viewControllers[i] as? LoginViewController
                if controller == nil {
                    navigationController.viewControllers.removeAtIndex(i)
                }
            }
        }
    }
}