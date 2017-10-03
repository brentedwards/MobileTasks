/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 * @flow
 */

import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  AppRegistry,
  StyleSheet,
  Text,
  View,
  Button,
  Image,
  Alert,
  TouchableOpacity, 
  WebView
} from 'react-native';
import LinearGradient from 'react-native-linear-gradient';
import FlexImage from 'react-native-flex-image';
import { StackNavigator, NavigationActions } from 'react-navigation';
import { NativeModules } from 'react-native';
var NetworkService = NativeModules.NetworkService;

export default class Login extends Component {
    static navigationOptions = {
        header: null,
      };
    
    constructor(props) {
        super(props);
        NetworkService.hasPreviousAuthentication((error, hasLogin) => {
            if (error == "" && hasLogin == 'true') {
                let resetAction = NavigationActions.reset({
                    index: 0,
                    actions: [
                      NavigationActions.navigate({ routeName: 'Tasks'})
                    ]
                })
                this.props.navigation.dispatch(resetAction)
            }
        })
      }

    _onMicrosoftTapped = () =>  {

        NetworkService.login("MICROSOFTACCOUNT", (error, loginInfo) => {
            if (error) {
              console.error(error);
            } else {
              console.debug(loginInfo);
            }
          })
        //console.debug(this._image.Text);
        //Alert.alert('You tapped the button!')
        //var auth0 = new Auth0({ domain: 'vslivemobiletasks.auth0.com', clientId: '9Z4fFNtg66Z1Yy2KWOOgMlLIRP60lIEn' });
        //auth0
        //.webAuth
        //.authorize({scope: 'openid email', audience: 'https://vslivemobiletasks.auth0.com/userinfo'})
        //.then(credentials => {
            //require('../../shim');
            //var WindowsAzure = require('azure-mobile-apps-client');
            //var clientRef = new WindowsAzure.MobileServiceClient('https://mobiletasks.azurewebsites.net'); 
            //clientRef.currentUser = { "mobileServiceAuthenticationToken": credentials.accessToken };  
            //clientRef.invokeApi("task", { method: "Get" }).done((results) => {
            //    Alert.alert(JSON.parse(results.response));
            //}, (err) => {
            //    Alert.alert(err.message);
            //}); 
            //var request = new Request('https://mobiletasks.azurewebsites.net/api/task');

            //request.method = 'Get';
            //Fetch(request)
            //.then(function(response) {
            //    console.debug(response);
                // ...
            //})
            //.catch((error) => {
            //  console.error(error);
            //});
        //})
        //.catch(error => Alert.alert(error.error)
        //);
      }
      _onGoogleTapped = () =>  {
        
                NetworkService.login("GOOGLE", (error, loginInfo) => {
                    if (error) {
                      console.error(error);
                    } else {
                      console.debug(loginInfo);
                    }
                  })
              }

              _onFacebookTapped = () =>  {
                
                        NetworkService.login("FACEBOOK", (error) => {
                            if (error == "") {
                                NetworkService.hasPreviousAuthentication((hasLogin) => {
                                    this.props.navigation.navigate('Tasks');
                                })
                            } else {
                              console.debug(error);
                            }
                          })
                      }

  render() {
    return (
        <LinearGradient colors={['#B4EC51', '#429321']} style={{flex: 1, flexDirection: 'column', paddingTop: 18}} >
            <View style={{flex: .3}} />
            <View style={{flex: .75, flexDirection: 'row', justifyContent: 'center', alignItems: 'center', paddingLeft: 20}} >
                <FlexImage source={require('../images/Logo.png')} style={{flex: .75}} />
            </View>
            <View style={{flex: .05}} />
           <View style={{flex: .33, flexDirection: 'column', justifyContent: 'center', alignItems: 'center'}}>
               <Text style={styles.loginLabel}>Login</Text>
           </View>
           <View style={{flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
                <View style={{width: 0}} />
                <TouchableOpacity style={{flex: .2}} onPress={this._onMicrosoftTapped}>
                    <FlexImage source={require('../images/IconTwitter.png')} />
                </TouchableOpacity>
                <TouchableOpacity style={{flex: .2}} onPress={this._onFacebookTapped}>
                    <FlexImage source={require('../images/IconFacebook.png')} />
                </TouchableOpacity>
                <TouchableOpacity style={{flex: .2}} onPress={this._onMicrosoftTapped}>
                    <FlexImage source={require('../images/IconMicrosoft.png')} />
                </TouchableOpacity>
                <TouchableOpacity style={{flex: .2}} onPress={this._onGoogleTapped}>
                    <FlexImage ref={component => this._image = component} source={require('../images/IconGoogle.png')} />
                </TouchableOpacity>
                <View style={{width: 0}} />
           </View>
           <View style={{flexDirection: 'row', justifyContent: 'space-between'}}>
                <View style={{width: 0}} />
                <Text style={styles.buttonLabel}>Twitter</Text>
                <Text style={styles.buttonLabel}>Facebook</Text>
                <Text style={styles.buttonLabel}>Microsoft</Text>
                <Text style={styles.buttonLabel}>Google</Text>
                <View style={{width: 0}} />
           </View>
           <View style={{flex: .2}} />
      </LinearGradient>
    );
  }
}

const styles = StyleSheet.create({
    loginLabel: {
      color: 'white',
      fontSize: 20,
      backgroundColor: 'transparent',
      textAlign: 'center',
    },
    buttonLabel: {
        flex: .2,
        color: 'white',
        fontSize: 12,
        backgroundColor: 'transparent',
        textAlign: 'center',
    },
  });