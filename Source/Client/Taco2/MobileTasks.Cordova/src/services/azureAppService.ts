import { Injectable } from '@angular/core';
import { Task } from '../models/task';
import { Storage } from '@ionic/storage';

declare var WindowsAzure: any;

@Injectable()
export class AzureAppService {

    private azureService : any;
    private azureUrl : string = "https://mobiletasks.azurewebsites.net";
    private lastUsedProvider : string = "LastUsedProvider";
    private userId :string = "|UserId";
    private token: string = "|Token";

    constructor(private storage: Storage) {
    }

    private getAzureService() : any {
        if (this.azureService == null) {
            this.azureService = new WindowsAzure.MobileServiceClient(this.azureUrl);
        }
        return this.azureService;
    }
    
    public login(provider: string): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this.getAzureService().login(provider).done((results) => {

                this.storage.set(this.lastUsedProvider, provider).then((provider) => {
                    this.storage.set(provider + this.userId, results.userId).then((provider) => {
                        this.storage.set(provider + this.token, this.getAzureService().currentUser.mobileServiceAuthenticationToken).then((provider) => {
                            resolve(true);
                        }).catch(error => { resolve(false); });
                    }).catch(error => { resolve(false); });
                }).catch(error => { resolve(false); });
            }, (err) => {
                reject(err.message);
            });
        });
    }

    public hasPreviousAuthentication() : Promise<boolean> {

        return new Promise<boolean>((resolve, reject) => {
            this.storage.get(this.lastUsedProvider).then((provider) => {
                if (provider != null) {
                    this.storage.get(provider + this.userId).then((uid) => {
                        this.storage.get(provider + this.token).then((uToken) => {
                            this.getAzureService().currentUser = { "userId": uid, "mobileServiceAuthenticationToken": uToken };
                            resolve(false);
                        }).catch(error => { resolve(false); });
                    }).catch(error => { resolve(false); });
                }
            }).catch(error => { resolve(false); });
        });
    }

    public getTasks(): Promise<Task[]> {
        return new Promise<boolean>((resolve, reject) => {
            this.getAzureService().invokeApi("task", { method: "Get" }).done((results) => {
                resolve(JSON.parse(results.response));
            }, (err) => {
                reject(err.message);
            });
        });
    }

    public logout() : Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this.getAzureService().logout().done((error, result) => {
                this.storage.remove(this.lastUsedProvider).then((provider) => {
                    resolve(true);
                }).catch(error => { resolve(false); });
            }, (err) => {
                reject(err.message);
            });
        });
    }

    public upsertTask(task : Task) : Promise<Task> {
        return new Promise<Task>((resolve, reject) => {
        var stringTask = JSON.stringify(task);
        this.getAzureService().invokeApi("task", { body: stringTask }).done((results) => {
                resolve(JSON.parse(results.response));
            }, (err) => {
                reject(err.message);
            });
        });
    }
}