import React, { Component } from "react";
import { StyleSheet, View, Dimensions, Keyboard } from "react-native";
import { Input, Button, ButtonGroup, Text } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";
import PickupIcon from "./PickupIcon";
import { connect } from "react-redux";
import base_url from "../constants/constants";

class Pickup extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedIndex: 0,
      buttonwidth: Math.round(Dimensions.get("window").width) - 20,
      placeholder: "Enter meter ID",
      meter: "",
      meterOne: "",
      check: true,
      error: "",
      errorOne: "",
      cameraSelected: false,
      cameraOneSelected: false,
      quantity: 0,
      meters: []
    };
  }
  static navigationOptions = {
    headerRight: () => (
      <PickupIcon
        onPress={() => {
          this.props.navigation.navigate("CartScreen");
        }}
      />
    )
  };
  async validateMetersBeforeAddingToCart(meter) {
    let response = await fetch(
      base_url + "api/meters/status/" + meter
    );
    let json = await response.json();
    if (json.Error[0] != "Meter is Ready to Pickup") {
      alert(json.Error[0] + " Meter id: " + meter);
      return false;
    }
    if (this.props.cartItemsDB.includes(meter)) {
      alert("Meter already added in cart.Meter id: " + meter);
      return false;
    }
    return true;
  }
  async addToCart() {
    try {
      var meters = [];
      var isValidated = true;
      if (this.state.selectedIndex == 0) {
        isValidated = await this.validateMetersBeforeAddingToCart(
          this.state.meter
        );
      } else {
        for (let i = this.state.meter; i <= this.state.meterOne; i++) {
          isValidated = await this.validateMetersBeforeAddingToCart(
            i.toString()
          );
          if (isValidated == false) {
            break;
          }
          meters.push(i.toString());
        }
      }
      if (isValidated) {
        Keyboard.dismiss();
        if (this.state.selectedIndex == 0) {
          this.props.addItemToCart(this.state.meter, 1, this.state.meter);
        } else {
          var meter = this.state.meter + " - " + this.state.meterOne;
          this.props.addItemToCart(meter, this.state.quantity, meters);
        }
        await this.setState({
          meter: "",
          meterOne: "",
          quantity: 0
        });
        this.meterInput.clear();
        if (this.state.selectedIndex == 1) {
          this.meterInputOne.clear();
        }
      }
    } catch (error) {
      console.log(error);
    }
  }
  handleSingleIndexSelect = async index => {
    await this.setState({
      selectedIndex: index
    });
    if (this.state.selectedIndex == 0) {
      await this.setState({
        placeholder: "Enter meter ID"
      });
    } else {
      await this.setState({
        placeholder: "Enter starting ID of range"
      });
    }
  };
  validateForm() {
    if (this.state.selectedIndex == 0) {
      if (this.state.meter == "" || this.state.error != "") {
        return false;
      } else {
        return true;
      }
    } else {
      if (
        this.state.meterOne == "" ||
        this.state.errorOne != "" ||
        this.state.meter == "" ||
        this.state.error != ""
      ) {
        return false;
      } else {
        return true;
      }
    }
  }
  handleChange = async value => {
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
    if (this.state.selectedIndex == 1) {
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
    }
    await this.setState({
      meter: value,
      check: false,
      cameraOneSelected: false,
      cameraSelected: false
    });
  };
  handleChangeOne = async value => {
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
      check: false,
      cameraOneSelected: false,
      cameraSelected: false
    });
  };
  render() {
    if (this.state.check) {
      const { navigation } = this.props;
      const meter = navigation.getParam("meter");
      if (this.state.cameraSelected) {
        this.state.meter = meter;
      }
      if (this.state.cameraOneSelected) {
        this.state.meterOne = meter;
      }
    }
    if (
      this.state.meterOne != "" &&
      this.state.errorOne == "" &&
      this.state.meter != "" &&
      this.state.error == ""
    ) {
      var quantityOne = parseInt(this.state.meterOne.match(/(\d+)/));
      var quantity = parseInt(this.state.meter.match(/(\d+)/));
      if (quantityOne > quantity)
        this.state.quantity = quantityOne - quantity + 1;
      else {
        this.state.quantity = 0;
      }
    } else {
      this.state.quantity = 0;
    }

    return (
      <View style={styles.container}>
        <ButtonGroup
          selectedBackgroundColor="blue"
          onPress={this.handleSingleIndexSelect}
          selectedIndex={this.state.index}
          buttons={["Pick up single", "Pick up bulk"]}
          containerStyle={{ height: 30 }}
        />
        <Text
          style={{ padding: 0, fontSize: 15, marginTop: 10, marginLeft: 0 }}
        ></Text>
        <Input
          keyboardType="numeric"
          editable={true}
          name="meterInput"
          ref={input => {
            this.meterInput = input;
          }}
          errorMessage={this.state.error}
          placeholder={this.state.placeholder}
          value={this.state.meter}
          onChangeText={value => {
            this.handleChange(value);
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
                  check: true,
                  cameraSelected: true,
                  cameraOneSelected: false
                });
                this.props.navigation.navigate("Barcode", {
                  screen: "Pickup"
                });
              }}
            />
          }
        />
        <Text></Text>
        {this.state.selectedIndex == 1 && (
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
              this.handleChangeOne(value);
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
                    check: true,
                    cameraOneSelected: true,
                    cameraSelected: false
                  });
                  this.props.navigation.navigate("Barcode", {
                    screen: "Pickup"
                  });
                }}
              />
            }
          />
        )}
        {this.state.selectedIndex == 1 && (
          <View style={{ marginLeft: 10, marginTop: 20, marginBottom: 10 }}>
            <Text style={{ fontSize: 20 }}>
              Quantity: {this.state.quantity}
            </Text>
          </View>
        )}
        <Button
          title="Add to list"
          buttonStyle={{borderRadius: 100, marginBottom: 20, alignSelf:'center',width: this.state.buttonwidth,height: 40,marginTop: 5}}
          disabled={!this.validateForm()}
          onPress={this.addToCart.bind(this)}
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
const mapDispatchToProps = dispatch => {
  return {
    addItemToCart: (meter, count, metersDB) =>
      dispatch({
        type: "ADD_TO_CART",
        payload: { meter: meter, count: count, metersDB: metersDB }
      })
  };
};
const mapStateToProps = state => {
  return {
    cartItems: state.cartItems,
    count: state.count,
    cartItemsDB: state.cartItemsDB
  };
};
export default connect(mapStateToProps, mapDispatchToProps)(Pickup);
