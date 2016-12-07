//
//  BaseViewController.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 3/30/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class BaseViewController: UIViewController {
    
    lazy var networkService : NetworkProtocol? = NetworkService()
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
    }
    
    override var preferredStatusBarStyle : UIStatusBarStyle {
        return UIStatusBarStyle.lightContent
    }
    
    func handleNetworkCallError(_ error : NSError) -> Void {
        ViewControllerShared.handleNetworkCallError(error, networkService: networkService!, viewController: self)
    }
}
