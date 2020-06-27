import React, { Component } from "react";
import { View, Text, StyleSheet, Dimensions, AsyncStorage } from "react-native";
import { connect } from "react-redux";
import { ListItem, Button } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";
import base_url from "../constants/constants";

class CartScreen extends Component {
  validateForm() {
    if (this.props.cartItems.length > 0) {
      return true;
    } else {
      return false;
    }
  }
  async _getStorageValue(){
    var value = await AsyncStorage.getItem('user')
    return value
  }
  async saveToDB() {
    try {
      var username = await this._getStorageValue();
      var date = new Date();
      fetch(base_url + "api/meterhistories/pickup", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          MIRN: this.props.cartItemsDB,
          PayRollID: username,
          MeterStatus: 2,
          Location: "1",
          TransfereeID: "",
          TransactionDate: date
        })
      });
      this.props.clearAll();
    } catch (error) {}
  }
  render() {
    return (
      <View>
        {this.props.cartItems.length > 0 ? (
          <View>
            {this.props.cartItems.map((l, i) => (
              <View>
                <ListItem
                  key={i}
                  title={l}
                  rightIcon={
                    <Icon
                      ref={input => {
                        this.trialOne = input;
                      }}
                      name="trash"
                      type="font-awesome"
                      color="red"
                      size={20}
                      onPress={() => {
                        if (l.includes("-")) {
                          var startId = l.split(" - ");
                          var meters = [];
                          for (var i = startId[0]; i <= startId[1]; i++) {
                            meters.push(i.toString());
                          }
                          for (var i = 0; i <= meters.length - 1; i++) {
                            meters[i];
                          }
                          this.props.removeItem(l, meters.length, meters);
                        } else {
                          this.props.removeItem(l, 1, l);
                        }
                      }}
                    />
                  }
                  bottomDivider
                />
              </View>
            ))}
          </View>
        ) : (
          <View style={{ alignItems: "center", marginTop: 10 }}>
            <Text>No items in your cart</Text>
          </View>
        )}
        <Button
          title="Submit pick up list"
          buttonStyle={{borderRadius: 100, marginTop: 10, alignSelf:'center',width: Math.round(Dimensions.get("window").width) - 20, height: 40}}
          disabled={!this.validateForm()}
          onPress={this.saveToDB.bind(this)}
        />
      </View>
    );
  }
}
const mapStateToProps = state => {
  return {
    cartItems: state.cartItems,
    count: state.count,
    cartItemsDB: state.cartItemsDB
  };
};

const mapDispatchToProps = dispatch => {
  return {
    removeItem: (meter, count, metersDB) =>
      dispatch({
        type: "REMOVE_FROM_CART",
        payload: { meter: meter, count: count, metersDB: metersDB }
      }),
    clearAll: product => dispatch({ type: "CLEAR_CART" })
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(CartScreen);
