import { Injectable } from '@angular/core';
import { Task } from '../models/task';

declare var WindowsAzure: any;
declare var plugins : any;

@Injectable()
export class AzureAppService {

    private azureService : any;
    private azureUrl : string = "https://mobiletasks.azurewebsites.net";
    private lastUsedProvider : string = "LastUsedProvider";
    private userId :string = "|UserId";
    private token :string = "|Token";

    private getAzureService() : any {
        if (this.azureService == null) {
            this.azureService = new WindowsAzure.MobileServiceClient(this.azureUrl);
        }
        return this.azureService;
    }
    
    public login(provider: string): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this.getAzureService().login(provider).done((results) => {

                var prefs = plugins.appPreferences;
                prefs.store(() => {
                    prefs.store(() => {
                        prefs.store(()  => {
                            resolve(true);
                        }, () => { resolve(false); }, provider + this.token, this.getAzureService().currentUser.mobileServiceAuthenticationToken);
                    }, (error) => { resolve(false); }, provider + this.userId, results.userId);
                }, (error) => { resolve(false); }, this.lastUsedProvider, provider);
            }, (err) => {
                reject(err.message);
            });
        });
    }

    public hasPreviousAuthentication() : Promise<boolean> {
        var prefs = plugins.appPreferences;

        return new Promise<boolean>((resolve, reject) => {
            prefs.fetch((provider) => {
                if (provider != null) {
                    prefs.fetch((uid) => {
                        prefs.fetch((uToken) => {
                            this.getAzureService().currentUser = { "userId": uid, "mobileServiceAuthenticationToken": uToken };

                            resolve(true);
                        }, () => { resolve(false); }, provider + this.token);
                    }, (error) => { resolve(false); }, provider + this.userId);
                } else { resolve(false); }
            }, (error) => { resolve(false); }, this.lastUsedProvider);
        });
    }

    public getTasks(): Promise<Task[]> {
        return new Promise<boolean>((resolve, reject) => {
            this.getAzureService().invokeApi("task", { method: "Get" }).done((results) => {
                alert(results.response);
                resolve(JSON.parse(results.response));
            }, (err) => {
                reject(err.message);
            });
        });
    }

    public logout() : Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this.getAzureService().logout().done((error, result) => {
                var prefs = plugins.appPreferences;
                prefs.remove(() => {
                    resolve(true);
                }, () => { resolve(true); }, this.lastUsedProvider);
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