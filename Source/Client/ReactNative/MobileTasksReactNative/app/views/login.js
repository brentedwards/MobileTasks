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
  TouchableOpacity
} from 'react-native';
import LinearGradient from 'react-native-linear-gradient';
import FlexImage from 'react-native-flex-image';
import Auth0 from 'react-native-auth0';

export default class Login extends Component {
    _onMicrosoftTapped() {
        //Alert.alert('You tapped the button!')
        var auth0 = new Auth0({ domain: 'vslivemobiletasks.auth0.com', clientId: '9Z4fFNtg66Z1Yy2KWOOgMlLIRP60lIEn' });
        auth0
        .webAuth
        .authorize({scope: 'openid email', audience: 'https://vslivemobiletasks.auth0.com/userinfo'})
        .then(credentials => Alert.alert(credentials.accessToken))
        .catch(error => Alert.alert(error.error));
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
                <TouchableOpacity style={{flex: .2}} onPress={this._onMicrosoftTapped}>
                    <FlexImage source={require('../images/IconFacebook.png')} />
                </TouchableOpacity>
                <TouchableOpacity style={{flex: .2}} onPress={this._onMicrosoftTapped}>
                    <FlexImage source={require('../images/IconMicrosoft.png')} />
                </TouchableOpacity>
                <TouchableOpacity style={{flex: .2}} onPress={this._onMicrosoftTapped}>
                    <FlexImage source={require('../images/IconGoogle.png')} />
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