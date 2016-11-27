import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AzureAppService } from '../../services/azureAppService';
import { Task } from '../../models/task'

@Component({
    templateUrl: 'task.html'
})

export class TaskPage {
    constructor(public navCtrl: NavController, private azureAppService: AzureAppService) {
        this.task = new Task();
    }

    public save() {
        this.azureAppService.upsertTask(this.task).then((result) => {
            if (result) {
                this.navCtrl.pop();
            }
        }).catch(function (error) {
            alert("not good");
        });
    }

    public task: Task;
    public specifyDueDate : boolean;
}