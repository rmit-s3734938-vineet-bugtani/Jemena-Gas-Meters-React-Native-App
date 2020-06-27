import React, { Component } from "react";
import { StyleSheet, View, Dimensions, AsyncStorage } from "react-native";
import { Input, Text, Button } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";
import { NavigationEvents } from "react-navigation";

export default class Filter extends Component {
  constructor(props) {
    super(props);
    this.state = {
      buttonwidth: Math.round(Dimensions.get("window").width) - 20,
      meter: "",
      meterOne: "",
      checkForValueFromBarcodeScanner: true,
      error: "",
      errorOne: "",
      cameraSelected: false,
      cameraOneSelected: false,
      data: this.props.navigation.getParam("data"),
      screen: this.props.navigation.getParam("screen")
    };
  }
  clearAsyncStorage = async () => {
    const asyncStorageKeys = await AsyncStorage.getAllKeys();
    if (asyncStorageKeys.length > 0) {
      AsyncStorage.clear();
    }
  };
  async toggleSearch() {
    const asyncStorageKeys = await AsyncStorage.getAllKeys();
    if (asyncStorageKeys.length > 0) {
      AsyncStorage.clear();
    }
    navigation.navigate("ChangeStatus", {});
  }
  async componentDidMount() {
    this.props.navigation.setParams({ toggleSearch: this.toggleSearch, screen: this.state.screen });
  }
  static navigationOptions = ({ navigation }) => {
    return {
      title: "Filter within a range",
      headerLeft: (
        <Icon
          name="times"
          size={20}
          color="black"
          onPress={() => {
            navigation.navigate(navigation.getParam('screen'), {});
          }}
          style={{ marginLeft: 20 }}
        />
      ),
      headerRight: (
        <Icon
          name="eraser"
          size={20}
          color="black"
          onPress={() => {
            AsyncStorage.clear();
            navigation.navigate(navigation.getParam('screen'), {});
          }}
          style={{ marginRight: 20 }}
        />
      )
    };
  };
  applyFilter() {
    var send = true;
    for (
      var i = parseInt(this.state.meter);
      i <= parseInt(this.state.meterOne);
      i++
    ) {
      if (!this.state.data.includes(i)) {
        send = false;
        alert("Meter id " + i + " does not exist in pick up list");
        break;
      }
    }
    if (send) {
      this.storeFilterValues("startRange", this.state.meter);
      this.storeFilterValues("endRange", this.state.meterOne);
      this.props.navigation.navigate(this.props.navigation.getParam('screen'), {});
    }
  }
  storeFilterValues = async (key, value) => {
    try {
      await AsyncStorage.setItem(key, value);
    } catch (error) {}
  };
  validateForm() {
    if (this.state.errorOne != "" || this.state.error != "") {
      return false;
    } else {
      return true;
    }
  }
  handleChangeInFirstTextField = async value => {
    if (value.length == 0) {
      this.setState({
        error: ""
      });
    } else if (value.length > 0 && value.length != 6) {
      this.setState({
        error: "Meter id should be a six digit number."
      });
    } else {
      this.setState({
        error: ""
      });
    }
    if (
      value >= this.state.meterOne &&
      this.state.meter != "" &&
      this.state.meterOne != ""
    ) {
      this.setState({
        errorOne: "Ending range id should be greater than starting range id."
      });
    } else {
      this.setState({
        errorOne: ""
      });
    }
    await this.setState({
      meter: value,
      checkForValueFromBarcodeScanner: false,
      cameraOneSelected: false,
      cameraSelected: false
    });
  };
  handleChangeInSecondTextField= async value => {
    if (value.length == 0) {
      this.setState({
        errorOne: ""
      });
    } else if (value.length > 0 && value.length != 6) {
      this.setState({
        errorOne: "Meter id should be a six digit number."
      });
    } else if (value <= this.state.meter) {
      this.setState({
        errorOne: "Ending range id should be greater than starting range id."
      });
    } else {
      this.setState({
        errorOne: ""
      });
    }
    await this.setState({
      meterOne: value,
      checkForValueFromBarcodeScanner: false,
      cameraOneSelected: false,
      cameraSelected: false
    });
  };
  async showPreviouslySetFiltersOnScreen() {
    const value = await AsyncStorage.getItem("startRange");
    const valueOne = await AsyncStorage.getItem("endRange");
    if (value !== null && valueOne != null) {
      this.setState({ meter: value });
      this.setState({ meterOne: valueOne });
    }
  }
  render() {
    if (this.state.checkForValueFromBarcodeScanner) {
      const { navigation } = this.props;
      const meter = navigation.getParam("meter");
      if (this.state.cameraSelected) {
        this.state.meter = meter;
      }
      if (this.state.cameraOneSelected) {
        this.state.meterOne = meter;
      }
    }
    return (
      <View style={styles.container}>
        <NavigationEvents
          onWillFocus={() => {
            this.showPreviouslySetFiltersOnScreen();
          }}
        />
        <Input
          keyboardType="numeric"
          editable={true}
          name="meterInput"
          ref={input => {
            this.meterInput = input;
          }}
          errorMessage={this.state.error}
          placeholder="Enter starting ID of range"
          value={this.state.meter}
          onChangeText={value => {
            this.handleChangeInFirstTextField(value);
          }}
          rightIcon={
            <Icon
              ref={input => {
                this.trial = input;
              }}
              name="camera"
              type="font-awesome"
              color="lightgrey"
              size={30}
              onPress={() => {
                this.setState({
                  checkForValueFromBarcodeScanner: true,
                  cameraSelected: true,
                  cameraOneSelected: false
                });
                this.props.navigation.navigate("Barcode", {
                  screen: "Filter"
                });
              }}
            />
          }
        />
        <Text></Text>
        <Input
          keyboardType="numeric"
          editable={true}
          name="meterInputOne"
          ref={input => {
            this.meterInputOne = input;
          }}
          errorMessage={this.state.errorOne}
          placeholder="Enter ending ID of range"
          value={this.state.meterOne}
          onChangeText={value => {
            this.handleChangeInSecondTextField(value);
          }}
          rightIcon={
            <Icon
              ref={input => {
                this.trialOne = input;
              }}
              name="camera"
              type="font-awesome"
              color="lightgrey"
              size={30}
              onPress={() => {
                this.setState({
                  checkForValueFromBarcodeScanner: true,
                  cameraOneSelected: true,
                  cameraSelected: false
                });
                this.props.navigation.navigate("Barcode", {
                  screen: "Filter"
                });
              }}
            />
          }
        />
        <Button
          title="Apply filter"
          buttonStyle={{borderRadius: 100, marginBottom: 20, alignSelf:'center',width: this.state.buttonwidth,height: 40,marginTop: 10}}
          disabled={!this.validateForm()}
          onPress={this.applyFilter.bind(this)}
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
