import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  Text,
  Button,
  Alert,
  StyleSheet,
  View, 
  FlatList,
} from 'react-native';
import { StackNavigator, NavigationActions } from 'react-navigation';
import { NativeModules } from 'react-native';
var NetworkService = NativeModules.NetworkService;

export default class Tasks extends Component {
    constructor(props) {
        super(props);
        this.state = {taskList: []};

        NetworkService.getTasks((error, tasks) => {

            if (error == "") {
                this.setState({ taskList: tasks}, function() {
                });
            } else {
                Alert.alert(error);
            }
        })
    }

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

    _keyExtractor = (item, index) => {
        console.log('id in key extractor', item.id)
        return item.id;
    }

    _convertDate = (date) => {
        if (date === null) {
            return 'null date';
        } else {
            return date.toString();
        }
    }

    _renderItem = ({item}) => (
        <View style={{flexDirection: 'column'}}>
            <Text style={styles.item}>{item.description}</Text>
            <Text>{this._convertDate(item.dateDue)}</Text>
        </View>
      );

    render() {
        return (
            <View style={styles.container}>
            <FlatList
              keyExtractor={this._keyExtractor}
              data = {this.state.taskList}
              extraData={this.state}
              renderItem={this._renderItem}
              ItemSeparatorComponent={(sectionId, rowId) => <View key={rowId} style={styles.separator} />}
            />
          </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
     flex: 1,
     paddingTop: 22
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