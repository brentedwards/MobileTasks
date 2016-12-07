//
//  BaseTableViewController.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 4/4/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class BaseTableViewController: UITableViewController {
    
    lazy var networkService : NetworkProtocol? = NetworkService()
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
    }
    
    func handleNetworkCallError(_ error : Error) -> Void {
        ViewControllerShared.handleNetworkCallError(error, networkService: networkService!, viewController: self)
    }
}
