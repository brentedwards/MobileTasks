import React, { Component, PropTypes } from 'react';
import { StackNavigator } from 'react-navigation';
import Login from './views/login';
import Tasks from './views/tasks';
import Task from './views/task';

export const Root = StackNavigator({
    Login: { screen: Login },
    Tasks: {screen: Tasks},
    Task: {screen: Task},
  }, {
    mode: 'card',
    headerMode: 'float',
  });

export default class Index extends Component {
  render() {
    return <Root />;
  }
}