//
//  BaseTableViewController.swift
//  MobileTasks.iOS
//
//  Created by kevin Ford on 4/4/16.
//  Copyright © 2016 Microsoft. All rights reserved.
//

import Foundation

class BaseTableViewController: UITableViewController {
    
    var networkService : NetworkProtocol?
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
        networkService = NetworkService()
    }
    
    func handleNetworkCallError(_ error : Error) -> Void {
        ViewControllerShared.handleNetworkCallError(error, networkService: networkService!, viewController: self)
    }
}
