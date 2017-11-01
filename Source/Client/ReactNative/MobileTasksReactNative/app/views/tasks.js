import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  Text,
  Button,
  Alert,
  StyleSheet,
  View, 
  FlatList,
  TouchableOpacity
} from 'react-native'
import { StackNavigator, NavigationActions } from 'react-navigation'
import { NativeModules } from 'react-native'
import FlexImage from 'react-native-flex-image'
import TaskRow from './taskRow';

var NetworkService = NativeModules.NetworkService;

export default class Tasks extends Component {
    constructor(props) {
        super(props);
        this.state = {taskList: [], userToken: '', listRefreshing: false}
    }

    static navigationOptions = ({ navigation }) => {
        const {dispatch} = navigation;
        return {
            headerStyle: styles.headerArea,
            title: 'Task List',
            headerTintColor: '#FFF',
            headerRight: <Button title="Logout" color='#FFF' onPress={() => {        
                NetworkService.logout((error) => {
                if (error == "") {
                    let resetAction = NavigationActions.reset({
                        index: 0,
                        actions: [
                            NavigationActions.navigate({ routeName: 'Login'})
                        ]
                    });
                    dispatch(resetAction)
                }
            })}}/>,
            headerLeft: <Button title="+" color='#FFF' onPress={() => {        
                NetworkService.logout((error) => {
                if (error == "") {
                    let resetAction = NavigationActions.reset({
                        index: 0,
                        actions: [
                            NavigationActions.navigate({ routeName: 'Login'})
                        ]
                    });
                    dispatch(resetAction)
                }
            })}}/>
        };
    }

    keyExtractor = (item, index) => {
        return item.id
    }

    refreshTaskList = () => {
        NetworkService.getUserInfo((token, userId) => {
            this.setState(
                {
                    userToken: token,
                    listRefreshing: true
                },
                function() {
                    fetch(
                        'https://mobiletasks.azurewebsites.net/api/' + 'task/', {
                            method: 'GET',
                            headers: {
                            'x-zumo-auth': this.state.userToken,
                            'Content-Type': 'application/json',
                            'ZUMO-API-VERSION': '2.0.0'
                            }
                        }
                    )
                    .then(response => response.json())
                    .then(responseJson => {
                        console.log(responseJson)
                      this.setState(
                        {
                            listRefreshing: false,
                            taskList: responseJson
                        }
                    )})
                    .catch(error => {
                      console.error('Error ' + error)
                      this.setState(
                        {
                            listRefreshing: false
                        })
                    })
                })
        })
    }

    componentDidMount() {
        this.refreshTaskList()
    }

    render() {
        return (
        <View style={styles.container}>
            <FlatList
              keyExtractor={this.keyExtractor}
              data = {this.state.taskList}
              extraData={this.state}
              renderItem={({item}) => (<TaskRow task={item} />)}
              ItemSeparatorComponent={(sectionId, rowId) => <View key={rowId} style={styles.separator} />}
              refreshing={this.state.listRefreshing}
              onRefresh={this.refreshTaskList}
            />

        </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
     flex: 1,
     paddingTop: 22,
     backgroundColor: '#FFF'
    },
    headerArea: {
        backgroundColor: '#45B1FF',
    },
    item: {
      padding: 10,
      fontSize: 18,
      height: 44,
    },
    separator: {
        flex: 1,
        height: StyleSheet.hairlineWidth,
        backgroundColor: '#8E8E8E',
      },
  })