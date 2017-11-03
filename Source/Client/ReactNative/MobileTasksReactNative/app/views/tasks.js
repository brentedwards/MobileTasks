import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  Text,
  Button,
  Alert,
  StyleSheet,
  View, 
  FlatList,
  TouchableOpacity,
  Platform
} from 'react-native'
import { StackNavigator, NavigationActions } from 'react-navigation'
import { NativeModules } from 'react-native'
import FlexImage from 'react-native-flex-image'
import TaskRow from './taskRow';
import apiService from '../Services/apiServices'

var NetworkService = NativeModules.NetworkService;

export default class Tasks extends Component {
    constructor(props) {
        super(props);
        this.state = {taskList: [], listRefreshing: false}
    }

    static navigationOptions = ({ navigation }) => {
        const params = navigation.state.params || {};
        const {dispatch} = navigation;
        return {
            headerStyle: styles.headerArea,
            title: 'Task List',
            headerTintColor: '#FFF',
            headerRight: 
            <TouchableOpacity style={{flex: .8, flexDirection: 'row', paddingRight: 10, alignItems: 'center'}} onPress={() => {        
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
            })}}>
                <Text title="Logout" style={{color: '#FFF', fontSize: 18}}>Logout</Text>
            </TouchableOpacity>,
            headerLeft: (Platform.OS === 'ios') ? <TouchableOpacity style={{flex: .8, flexDirection: 'row', paddingLeft: 10, alignItems: 'center'}} onPress={params.addTask}> 
                <Text style={{color: '#FFF', fontSize: 18}}>+</Text>
            </TouchableOpacity> : null
        }
    }

    setNavigationParams() {
        let addTask = this.addTask
      
        this.props.navigation.setParams({ 
          addTask
        })
    }

    componentWillMount() {
        this.setNavigationParams();
    }

    addTask = () => {
        console.log('Add task')
        var navAction = NavigationActions.navigate({
          routeName: 'Task',
          params: { task: null, navigation: this.props.navigation }
        })
        this.props.navigation.dispatch(navAction)
    }

    keyExtractor = (item, index) => {
        return item.id
    }

    refreshTaskList = () => {
        this.setState(
        {
            listRefreshing: true
        },
        function() {
            apiService.getTasksAsync()
            .then((response) => {
                console.log(response)
                this.setState(
                {
                    listRefreshing: false,
                    taskList: response
                })
            })
            .catch((error) => {
                console.log(error)
                if (error.httpError === 401) {
                    NetworkService.logout((error) => {
                        if (error == "") {
                        // Token expired, need to log in again
                        let resetAction = NavigationActions.reset({
                                index: 0,
                                actions: [
                                    NavigationActions.navigate({ routeName: 'Login'})
                                ]
                            })
                        this.props.navigation.dispatch(resetAction);
                    }
                    })
                } else if (error.httpError != 0) {
                    Alert.alert('Error status in http request ' + error.httpError)
                } else {
                    Alert.alert('Error in http request ' + error.otherError)
                }
                
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
              renderItem={({item}) => (<TaskRow task={item} navigation={this.props.navigation} />)}
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