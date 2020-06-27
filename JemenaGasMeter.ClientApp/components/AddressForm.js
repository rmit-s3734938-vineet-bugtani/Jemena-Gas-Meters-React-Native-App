import React, { Component } from "react";
import { StyleSheet, View, Dimensions,AsyncStorage } from "react-native";
import { Input, Text, Button } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";
import base_url from "../constants/constants";

export default class AddressForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      buttonwidth: Math.round(Dimensions.get("window").width) - 20,
      error: "",
      errorOne: "",
      meter: this.props.navigation.getParam("meters"),
      streetNumber: "",
      streetName: "",
      suburb: "",
      postcode: "",
      stateName: "NSW",
      comments: ""
    };
  }
  async componentDidMount() {
    this.props.navigation.setParams({ toggleSearch: this.toggleSearch });
  }
  static navigationOptions = ({ navigation }) => {
    return {
      title: "Enter meter installation address",
      headerLeft: (
        <Icon
          name="times"
          size={20}
          color="black"
          onPress={() => {
            navigation.navigate("ChangeStatus", {});
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
    await fetch(base_url + "api/meterhistories/install", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        MIRN: this.state.meter.mirn,
        PayRollID: username,
        MeterStatus: 5,
        StreetNo: this.state.streetNumber,
        StreetName: this.state.streetName,
        Suburb: this.state.suburb,
        State: this.state.stateName,
        PostCode: parseInt(this.state.postcode),
        Comment: this.state.comments,
        TransactionDate: "2020-04-27T12:16:41.928Z"
      })
    });
    this.props.navigation.navigate("ChangeStatus", {});
    alert("Status changed successfully");
  }
  validateForm() {
    if (
      this.state.streetNumber == "" ||
      this.state.suburb == "" ||
      this.state.streetName == "" ||
      this.state.postcode == "" ||
      this.state.stateName == ""
    ) {
      return false;
    } else {
      return true;
    }
  }
  handleChangeInSuburb = async value => {
    await this.setState({
      suburb: value
    });
  };
  handleChangeInPostCode = async value => {
    await this.setState({
      postcode: value
    });
  };
  handleChangeInStateName = async value => {
    await this.setState({
      stateName: value
    });
  };
  handleChangeInStreetName = async value => {
    await this.setState({
      streetName: value
    });
  };
  handleChangeInStreetNumber = async value => {
    await this.setState({
      streetNumber: value
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
          placeholder="Enter street number"
          value={this.state.streetNumber}
          onChangeText={value => {
            this.handleChangeInStreetNumber(value);
          }}
        />
        <Text></Text>
        <Input
          editable={true}
          errorMessage={this.state.error}
          placeholder="Enter street name"
          value={this.state.streetName}
          onChangeText={value => {
            this.handleChangeInStreetName(value);
          }}
        />
        <Text></Text>
        <Input
          editable={true}
          errorMessage={this.state.errorOne}
          placeholder="Enter suburb name"
          value={this.state.suburb}
          onChangeText={value => {
            this.handleChangeInSuburb(value);
          }}
        />
        <Text></Text>
        <Input
          keyboardType="numeric"
          editable={true}
          errorMessage={this.state.error}
          maxLength={4}
          placeholder="Enter postcode"
          value={this.state.postcode}
          onChangeText={value => {
            this.handleChangeInPostCode(value);
          }}
        />
        <Text></Text>
        <Input
          editable={true}
          errorMessage={this.state.error}
          placeholder="Enter state"
          value={this.state.stateName}
          onChangeText={value => {
            this.handleChangeInStateName(value);
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

        <Text></Text>
        <Button
          title="Submit"
          buttonStyle={{borderRadius: 100, marginBottom: 20, alignSelf:'center',width: this.state.buttonwidth,height: 40}}
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
