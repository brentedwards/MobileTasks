import React, { Component, PropTypes } from 'react';
import { StackNavigator } from 'react-navigation';
import Login from './views/login';
import Tasks from './views/tasks';

export const Root = StackNavigator({
    Login: { screen: Login },
    Tasks: {screen: Tasks},
  }, {
    mode: 'modal',
    headerMode: 'float',
  });

export default class Index extends Component {
  render() {
    return <Root />;
  }
}