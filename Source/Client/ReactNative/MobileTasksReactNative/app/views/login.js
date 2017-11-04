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
  WebView,
  ActivityIndicator
} from 'react-native';
import LinearGradient from 'react-native-linear-gradient'
import FlexImage from 'react-native-flex-image'
import { StackNavigator, NavigationActions } from 'react-navigation'
import { NativeModules } from 'react-native'
var NetworkService = NativeModules.NetworkService

export default class Login extends Component {
    static navigationOptions = {
        header: null,
      };
    
    constructor(props) {
        super(props);
        this.state = {showProgress: false}

    }

    componentDidMount() {
        this.setState({
            showProgress: true
        },
        function() {
            NetworkService.hasPreviousAuthentication((error, hasLogin) => {
                this.setState({
                    showProgress: false
                },
                function() {
                    if (error == "" && hasLogin == 'true') {
                        this.navigateToTaskList();
                    }
                })
            })
        })
    }

    navigateToTaskList() {
        let resetAction = NavigationActions.reset({
            index: 0,
            actions: [
              NavigationActions.navigate({ routeName: 'Tasks'})
            ]
        })
        this.props.navigation.dispatch(resetAction);
    }

    processLogin = (provider) => {
        NetworkService.login(provider, (error) => {
            if (error == "") {
                this.navigateToTaskList();
            } else {
              Alert.alert(error);
            }
        })
    }

    _onMicrosoftTapped = () =>  {
        this.processLogin('MICROSOFTACCOUNT');
    }

    _onGoogleTapped = () =>  {
        this.processLogin('GOOGLE');
    }

    _onFacebookTapped = () =>  {
        this.processLogin('FACEBOOK');
    }

    _onTwitterTapped = () =>  {
        this.processLogin('TWITTER');
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
                <TouchableOpacity style={{flex: .2}} onPress={this._onTwitterTapped}>
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
           {this.state.showProgress &&
            <View style={styles.loading}>
                <ActivityIndicator animating={this.state.showProgress} size="large"/>
            </View>
            }
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
    loading: {
        position: 'absolute',
        left: 0,
        right: 0,
        top: 0,
        bottom: 0,
        alignItems: 'center',
        justifyContent: 'center'
      }
  });