import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  Text,
  Button,
  Alert,
} from 'react-native';
import { StackNavigator, NavigationActions } from 'react-navigation';
import { NativeModules } from 'react-native';
var NetworkService = NativeModules.NetworkService;

export default class Tasks extends Component {
    static navigationOptions = ({ navigation }) => {
        const {dispatch} = navigation;
        return {
            title: 'Task List',
            headerRight: <Button title="Logout" onPress={() => {        
                NetworkService.logout((error) => {
                if (error == "") {
                    let resetAction = NavigationActions.reset({
                        index: 0,
                        actions: [
                            NavigationActions.navigate({ routeName: 'Login'})
                        ]
                    });
                    dispatch(resetAction);
                }
            })}}/>,
        };
    };

    _onLogoutTapped() {
        Alert.alert('dd');

    }

    render() {
        return (
            <Text>Tasks</Text>
        );
    }
}