import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AzureAppService } from '../../services/azureAppService';
import {TasksPage} from '../tasks/tasks'

@Component({
    selector: 'page-home',
    templateUrl: 'login.html'
})

export class LoginPage {

    constructor(public navCtrl: NavController, private azureAppService : AzureAppService) {
    }

    ionViewWillEnter() {

        this.azureAppService.hasPreviousAuthentication().then((result) => {
            if (result) {
                this.navCtrl.push(TasksPage, {});
            }
        });
    }

    performLogin(provider : string): void {
        this.azureAppService.login(provider).then((result) => {
             this.navCtrl.push(TasksPage, {});
        } );
    }
}