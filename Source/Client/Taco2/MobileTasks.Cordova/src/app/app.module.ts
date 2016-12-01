import { NgModule } from '@angular/core';
import { IonicApp, IonicModule } from 'ionic-angular';
import { MyApp } from './app.component';
import { LoginPage } from '../pages/login/login';
import { TasksPage } from '../pages/tasks/tasks';
import { TaskPage } from '../pages/task/task';
import { AzureAppService } from '../services/azureAppService';
import { Storage } from '@ionic/storage';

@
NgModule({
    declarations: [
        MyApp,
        LoginPage,
        TasksPage,
        TaskPage
    ],
    imports: [
        IonicModule.forRoot(MyApp)
    ],
    bootstrap: [IonicApp],
    entryComponents: [
        MyApp,
        LoginPage,
        TasksPage,
        TaskPage
    ],
    providers: [
        Storage, { provide: AzureAppService, useClass: AzureAppService }]
})
export class AppModule {}
