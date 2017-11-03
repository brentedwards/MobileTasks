import React, { Component } from 'react'
import {
  DatePickerIOS,
  DatePickerAndroid,
  Platform,
  StyleSheet,
  Text,
  TextInput,
  View,
  Switch,
  TouchableNativeFeedback,
  TouchableOpacity,
  Alert
} from 'react-native'

import { StackNavigator, NavigationActions } from 'react-navigation'

import apiService from '../Services/apiServices'


export default class Task extends Component {
  constructor(props) {
    super(props)
    console.log(this.props.navigation)
    console.log('loading task ' + this.props.navigation.state.params.task)
    var today = new Date()
    this.state = {
      specifyDueDate: this.isNew() ? false: this.props.navigation.state.params.task.dateDue === null ? false: true,
      task: this.isNew() ? {id: 0, sid: '', description: '', isCompleted: null, dateCreated: new Date(), dateDue: null, dateCompleted: null} : 
      this.getTaskWithRealDates(this.props.navigation.state.params.task),
      isNew: this.isNew()
    }
  }

  getTaskWithRealDates(task) {
    return {id: task.id, sid: task.sid, 
      description: task.description, 
      isCompleted: task.isCompleted, 
      dateCreated: task.dateCreated === null ? null : new Date(task.dateCreated), 
      dateDue: task.dateDue === null ? null : new Date(task.dateDue), 
      dateCompleted: task.dateCompleted === null ? null : new Date(task.dateCompleted)}
  }

  isNew() {
    return this.props.navigation.state.params.task === null
  }

  static navigationOptions = ({ navigation }) => {
    const params = navigation.state.params || { };
    const {goBack} = navigation;
    return {
      headerStyle: styles.headerArea,
      title: params.title,
      headerTintColor: '#FFF',
      headerRight: 
      <TouchableOpacity style={{flex: .8, flexDirection: 'row', paddingRight: 10, alignItems: 'center'}} onPress={params.saveTask}>
          <Text title="Logout" style={{color: '#FFF', fontSize: 18}}>Save</Text>
      </TouchableOpacity>,
      headerLeft: (Platform.OS === 'ios') ? 
      <TouchableOpacity style={{flex: .8, flexDirection: 'row', paddingLeft: 10, alignItems: 'center'}} onPress={params.goBack}> 
          <Text style={{color: '#FFF', fontSize: 18}}>Cancel</Text>
      </TouchableOpacity> : null
    }
  }

  setNavigationParams() {
    let title = this.isNew() ? 'Add Task' : 'Edit Task'
    let goBack = this.goBack
    let saveTask = this.saveTask
  
    this.props.navigation.setParams({ 
      title,
      goBack,
      saveTask
    })
  }

  saveTask = () => {
    console.log('Save task')
    console.log(this.state.task)
    apiService.upsertTasksAsync(this.state.task)
    .then((response) => {
        console.log('done!')
        console.log(response)
        this.setState(
        {
            task: this.getTaskWithRealDates(response)
        },
        function() {
            Alert.alert('Task saved')
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
  }

  componentWillMount() {
    this.setNavigationParams();
  }

  goBack = () => {
    console.log('go back')
    this.props.navigation.goBack(null)
  }

  dateChanged = newDate => {
    this.setState({
      task: { ...this.state.task, dateDue: newDate }
    })
  }

  itemDescriptionChanged = newItemDescription => {
    this.setState({
      task: { ...this.state.task, description: newItemDescription }
    })
  }

  specifyDueDateChanged = newSpecifyDueDate => {
    this.setState({
      specifyDueDate: newSpecifyDueDate,
      task: { ...this.state.task, dateDue:  newSpecifyDueDate ? new Date() : null}
    })
  }

  isCompletedChanged = newIsCompletedChanged => {
    this.setState({
      task: { ...this.state.task, isCompleted: newIsCompletedChanged, dateCompleted:  newIsCompletedChanged? new Date() : null}
    })
  }

render() {
    return (
        <View style={styles.container}>
            <TextInput value={this.state.task.description} placeholder='Task Details' onChangeText={this.itemDescriptionChanged}/>
            <View style={styles.separator} />
            <View style={{flexDirection:'row', paddingTop: 5, paddingBottom: 5, alignItems: 'center'}}>
              <Text style={{flex: .8 }}>Specify Due Date:</Text>
              <Switch style={{flex: .2}} value={this.state.specifyDueDate} onValueChange={this.specifyDueDateChanged} />
            </View>
            <View style={styles.separator} />
            {Platform.OS === 'ios' && this.state.specifyDueDate && (
                    <DatePickerIOS
                      date={this.state.task.dateDue}
                      mode="date"
                      onDateChange={this.dateChanged}
                    />
            )}
            {Platform.OS === 'android' && this.state.specifyDueDate === true && (
                    <TouchableNativeFeedback onPress={() => this.setDate()}>
                      <Text>
                        {this.state.task.dateDue.toDateString()}
                      </Text>
                    </TouchableNativeFeedback>
            )}
            <View style={styles.separator} /> 
            <View style={{flexDirection:'row', paddingTop: 5, paddingBottom: 5, alignItems: 'center'}}>
              <Text style={{flex: .8 }}>Mark as Completed:</Text>
              <Switch style={{flex: .2}} value={this.state.task.isCompleted} onValueChange={this.isCompletedChanged} />
            </View>
        </View>

    )
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'column',
    backgroundColor: '#FFF',
    paddingTop: 18,
    paddingLeft: 5
  },
  headerArea: {
    backgroundColor: '#45B1FF',
  },
  separator: {
    height: StyleSheet.hairlineWidth,
    backgroundColor: '#8E8E8E',
  },
})