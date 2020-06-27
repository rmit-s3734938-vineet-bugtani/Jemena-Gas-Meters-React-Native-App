import React, { Component } from "react";
import { StyleSheet, View, Dimensions,AsyncStorage } from "react-native";
import { Input, Text, Button } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";
import base_url from "../constants/constants";

export default class TransfereeForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      buttonwidth: Math.round(Dimensions.get("window").width) - 20,
      error: "",
      errorOne: "",
      meter: this.props.navigation.getParam("meters"),
      name: "",
      companyName: "",
      comments: ""
    };
  }
  async componentDidMount() {
    this.props.navigation.setParams({ toggleSearch: this.toggleSearch });
  }
  static navigationOptions = ({ navigation }) => {
    return {
      title: "Enter transferree details",
      headerLeft: () => (
        <Icon
          name="times"
          size={20}
          color="black"
          onPress={() => {
            navigation.navigate("Transfer", {});
          }}
          style={{ marginLeft: 20 }}
        />
      )
    };
  };
  async _getStorageValue(){
    var value = await AsyncStorage.getItem('user')
    return value
  }
  async submit() {
    var username = await this._getStorageValue();
    var date = new Date();
    await fetch(base_url + "api/meterhistories/transfer", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        MIRN: this.state.meter,
        PayRollID: username,
        MeterStatus: 4,
        Name: this.state.name,
        Company: this.state.companyName,
        Comment: this.state.comments,
        TransactionDate: "2020-04-27T12:16:41.928Z"
      })
    });
    this.props.navigation.navigate("Transfer", {});
    alert("Meters transfered successfully");
  }
  validateForm() {
    if (this.state.name == "" || this.state.companyName == "") {
      return false;
    } else {
      return true;
    }
  }
  handleChangeInName = async value => {
    await this.setState({
      name: value
    });
  };
  handleChangeInCompanyName = async value => {
    await this.setState({
      companyName: value
    });
  };
  handleChangeInComments = async value => {
    await this.setState({
      comments: value
    });
  };

  render() {
    return (
      <View style={styles.container}>
        <Input
          editable={true}
          errorMessage={this.state.error}
          placeholder="Enter transferee name"
          value={this.state.name}
          onChangeText={value => {
            this.handleChangeInName(value);
          }}
        />
        <Text></Text>
        <Input
          editable={true}
          errorMessage={this.state.error}
          placeholder="Enter company name"
          value={this.state.companyName}
          onChangeText={value => {
            this.handleChangeInCompanyName(value);
          }}
        />
        <Text></Text>
        <Input
          editable={true}
          errorMessage={this.state.error}
          placeholder="Enter comments(optional)"
          value={this.state.comments}
          onChangeText={value => {
            this.handleChangeInComments(value);
          }}
        />
        <Button
          title="Submit"
          buttonStyle={{borderRadius: 100, marginBottom: 20, alignSelf:'center',width: this.state.buttonwidth,height: 40,marginTop: 10}}
          disabled={!this.validateForm()}
          onPress={this.submit.bind(this)}
        />
      </View>
    );
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "flex-start",
    backgroundColor: "white",
    padding: 10
  }
});
