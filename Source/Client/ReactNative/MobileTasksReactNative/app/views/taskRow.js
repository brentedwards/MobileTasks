import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  Text,
  Button,
  Alert,
  StyleSheet,
  View, 
  TouchableOpacity
} from 'react-native'
import { StackNavigator, NavigationActions } from 'react-navigation'
import { NativeModules } from 'react-native'
import FlexImage from 'react-native-flex-image'
import Moment from 'moment';
var NetworkService = NativeModules.NetworkService;


export default class TaskRow extends Component {
    constructor(props) {
        super(props);
    }

    convertDate = (date) => {
        if (date === null) {
            return 'No Due Date'
        } else {
            return Moment(date).format('MMM DD, YYYY') + ' at ' + Moment(date).format('hh:mm A') 
        }
    }

    getIconForTask(taskComplete, taskDateDue) {
        console.log('complete ' + taskComplete)
        console.log('due ' + taskDateDue)
        if (taskComplete === null && taskDateDue === null) {
            return require("../images/icon-incomplete.png")
        } else if (taskComplete) {
            return require("../images/icon-completed.png")
        } else if (taskDateDue != null && new Date(taskDateDue) < new Date()) {
            return require("../images/icon-pastdue.png")
        } else {
            return require("../images/icon-incomplete.png")
        }
    }

    switchIsChanged() {
        if (!this.props.task.isCompleted) {
            this.props.task.isCompleted = true;
            this.props.task.dateCompleted = new Date();
            /*this.azureAppService.upsertTask(task).then((result) => {
                if (result) {
                    task = result;
                }
            }).catch((error) => {
                alert("not good");
            })*/
        }
    }

    render() {
        return (
        <View style={{flex: 1, flexDirection: 'row'}}>
            <TouchableOpacity style={{flex: .1, justifyContent: 'center', marginLeft: 15}} onPress={this.switchIsChanged()}>
                <FlexImage source={this.getIconForTask(this.props.task.isCompleted, this.props.task.dateDue)} />
            </TouchableOpacity>
            <View style={{flex: .8, flexDirection: 'column'}}>
                <Text style={styles.headingItem}>{this.props.task.description}</Text>
                <Text style={styles.detailItem}>{this.convertDate(this.props.task.dateDue)}</Text>
            </View>
            <TouchableOpacity style={{flex: .1, justifyContent: 'center', marginRight: 15}}>
                <FlexImage source={require("../images/icon-detail.png")} />
            </TouchableOpacity>
        </View>
        )
    }
}

const styles = StyleSheet.create({
    headingItem: {
      marginTop: 10,
      marginLeft: 10,
      marginBottom: 5,
      fontSize: 18,
    },
    detailItem: {
        marginLeft: 10,
        marginBottom: 10,
        fontSize: 14,
        color: 'grey'
      },
  })