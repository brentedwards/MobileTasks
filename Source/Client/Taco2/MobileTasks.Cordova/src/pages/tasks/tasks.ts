import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { AzureAppService } from '../../services/azureAppService';
import { Task } from '../../models/task'
import { LoginPage } from '../login/login';
import { TaskPage } from '../task/task';

@Component({
    templateUrl: 'tasks.html'
})

export class TasksPage {

    constructor(public navCtrl: NavController, private azureAppService: AzureAppService) {
    }

    ionViewWillEnter() {
        var navigation: NavController = this.navCtrl;

        var screenCount: number = this.navCtrl.length() - 1;
        for (var i: number = 0; i < screenCount; i++) {
            if (this.navCtrl.getByIndex(i) instanceof LoginPage) {
                this.navCtrl.remove(i);
            }
        }
    }

    ionViewDidEnter() {;
        this.azureAppService.getTasks().then((result) => {
            if (result) {
                this.tasks = result;
            }
        }).catch((error) => {
            if (error === "Unauthorized") {
                this.azureAppService.logout().then((result) => {
                    this.navCtrl.push(LoginPage, {});
                });
            } else {
                alert("not good " + error);
            }
        });
    }

    public getIconForTask(taskComplete : boolean, taskDateDue : Date) : string {
        if (taskComplete == null && taskDateDue == null) {
            return "images/icon-incomplete.png";
        } else if (taskComplete) {
            return "images/icon-completed.png";
        } else if (new Date(taskDateDue) < new Date()) {
            return "images/icon-pastdue.png";
        } else {
            return "images/icon-incomplete.png";
        }
    }

    public switchIsChanged(task: Task): void {
        if (!task.isCompleted) {
            task.isCompleted = true;
            task.dateCompleted = new Date();
            this.azureAppService.upsertTask(task).then((result) => {
                if (result) {
                    task = result;
                }
            }).catch((error) => {
                alert("not good");
            });
        }
    }

    public addTask() : void {
        this.navCtrl.push(TaskPage, {});
    }

    public tasks: Task[] = [];
}