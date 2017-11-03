import { NativeModules, NetInfo } from 'react-native'

var NetworkService = NativeModules.NetworkService;

export default class ApiService {
    static getTasksAsync () {
        return new Promise((resolve, reject) => {
            try {
                NetInfo.isConnected.fetch().then(isConnected => {
                    if (isConnected = 'online') {
                        NetworkService.getUserInfo(async (token, userId) => {
                            console.log(token)
                            let response = await fetch(
                                'https://mobiletasks.azurewebsites.net/api/task/', {
                                    method: 'GET',
                                    headers: {
                                    'x-zumo-auth': token,
                                    'Content-Type': 'application/json',
                                    'ZUMO-API-VERSION': '2.0.0'
                                    }
                                }
                            )
                            if (response.status != 200) {
                                console.log('about to reject')
                                reject({httpError: response.status, otherError: ''})
                                return
                            }
                            response.json()
                            .then((responseJson) => {
                                resolve(responseJson)
                            })       
                        })
                    } else {
                        console.log('Device not connected')
                        reject({httpError: response.status, otherError: 'Your device does not have a connection. Please try again when a connection is established'})
                    }
                })
            } catch(error) {
                console.error(error)
                reject({httpError: 0, otherError: error})
            }
        })
    }

    static upsertTasksAsync (task) {
        return new Promise((resolve, reject) => {
            try {
                NetInfo.isConnected.fetch().then(isConnected => {
                    if (isConnected = 'online') {
                        NetworkService.getUserInfo(async (token, userId) => {
                            console.log(token)
                            var taskBody = JSON.stringify(task)
                            console.log(taskBody)
                            let response = await fetch(
                                'https://mobiletasks.azurewebsites.net/api/task/', {
                                    method: 'POST',
                                    headers: {
                                    'x-zumo-auth': token,
                                    'Content-Type': 'application/json',
                                    'ZUMO-API-VERSION': '2.0.0'
                                    },
                                    body: taskBody
                                }
                            )
                            if (response.status != 200) {
                                console.log('about to reject')
                                reject({httpError: response.status, otherError: ''})
                                return
                            }
                            console.log('saveified')
                            response.json()
                            .then((responseJson) => {
                                resolve(responseJson)
                            })       
                        })
                    } else {
                        console.log('Device not connected')
                        reject({httpError: response.status, otherError: 'Your device does not have a connection. Please try again when a connection is established'})
                    }
                })
            } catch(error) {
                console.log('we gots an error')
                console.error(error)
                reject({httpError: 0, otherError: error})
            }
        })
    }
}
