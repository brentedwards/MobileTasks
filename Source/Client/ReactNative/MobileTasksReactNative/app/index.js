import React, { Component, PropTypes } from 'react';
import { NavigatorIOS, Text } from 'react-native';
import { StackNavigator } from 'react-navigation';
import Login from './views/login';

export const Root = StackNavigator({
    Login: {
      screen: Login,
    },
  }, {
    mode: 'modal',
    headerMode: 'none',
  });

export default class Index extends Component {
  render() {
    return <Root />;
  }
}