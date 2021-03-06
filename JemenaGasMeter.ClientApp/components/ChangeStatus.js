import React, { Component } from "react";
import {
  View,
  Dimensions,
  ScrollView,
  Text,
  AsyncStorage
} from "react-native";
import { ListItem, Input, CheckBox, Button } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";
import { NavigationEvents } from "react-navigation";
import base_url from "../constants/constants";
import { connectActionSheet } from '@expo/react-native-action-sheet'

class ChangeStatus extends Component {
  constructor(props) {
    super(props);
    this.state = {
      meters: [],
      checked: [],
      filteredMeters: [],
      value: false,
      buttonwidth: Math.round(Dimensions.get("window").width) - 20
    };
  }
  toggleSearch = () => {
    this.setState({ filteredMeters: this.state.meters });
    if (this.state.value) this.setState({ value: false });
    else this.setState({ value: true });
  };
  validate() {
    for (var i = 0; i <= this.state.filteredMeters.length - 1; i++) {
      if (this.state.filteredMeters[i].checked) return true;
    }
    return false;
  }
  getMeters() {
    var meterIds = [];
    for (var i = 0; i <= this.state.meters.length - 1; i++) {
      meterIds.push(parseInt(this.state.meters[i].mirn));
    }
    return meterIds;
  }
  async toogleCheck(i) {
    var index = parseInt(i);
    var items = [];
    for (var i = 0; i <= this.state.filteredMeters.length - 1; i++) {
      items[i] = this.state.filteredMeters[i];
    }
    if (index == 0) {
      items[0].checked = !items[0].checked;
      for (var i = 0; i <= items.length - 1; i++) {
        items[i].checked = await items[0].checked;
      }
    } else {
      items[index].checked = !items[index].checked;
    }
    this.setState({ filteredMeters: items });
  }
  static navigationOptions = ({ navigation }) => {
    return {
      title: "Change Meter Status",
      headerRight: () => (
        <View>
          <Icon
            name="search"
            size={20}
            style={{
              position: "absolute",
              right: 35
            }}
            onPress={navigation.getParam("toggleSearch")}
          />
          <Icon
            name="filter"
            size={20}
            style={{ right: 10 }}
            onPress={() => {
              navigation.navigate("Filter", {
                data: navigation.getParam("data"),
                screen: "ChangeStatus"
              });
            }}
          />
        </View>
      )
    };
  };
  async sendRequestToDbForReturning(meters, condition) {
    var date = new Date();
    var username = await this._getStorageValue();
    await fetch(base_url + "api/meterhistories/return", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        MIRN: meters,
        PayRollID: username,
        MeterStatus: 3,
        MeterCondition: condition,
        Location: "1",
        TransactionDate: date
      })
    });
    await this.fetchMeters();
    await this.filterByRange();
  }
  async sendRequestToDb(meters, status, condition) {
    await fetch(base_url + "api/meters/change-status", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        MIRN: meters,
        MeterStatus: status,
        MeterCondition: condition
      })
    });
    await this.fetchMeters();
    await this.filterByRange();
  }
  async submit() {
    var meters = this.state.filteredMeters.filter(cartItem => {
      return cartItem.checked && cartItem.mirn !== "";
    });
    var meterIds = [];
    for (var i = 0; i <= meters.length - 1; i++) {
      meterIds.push(meters[i].mirn);
    }
    var meterConditions = [];
    for (var i = 0; i <= meters.length - 1; i++) {
      if (meters[i].meterCondition == CONDITION.ACTIVE.name) {
        meterConditions.push(CONDITION.ACTIVE.value);
      }
      if (meters[i].meterCondition == CONDITION.FAULTY.name) {
        meterConditions.push(CONDITION.FAULTY.value);
      }
      if (meters[i].meterCondition == CONDITION.EXPIRED.name) {
        meterConditions.push(CONDITION.EXPIRED.value);
      }
    }
    const options = ['Faulty', 'Expired', 'Installed', 'Returned', 'Cancel'];
    const cancelButtonIndex = 4;
  
    this.props.showActionSheetWithOptions(
      {
        options,
        cancelButtonIndex
      },
      async buttonIndex => {
        if (buttonIndex === 0) {
          this.sendRequestToDb(
            meterIds,
            STATUS.PICKUP.value,
            CONDITION.FAULTY.value
          );
          alert("Status changed successfully");
          let newArray = [...this.state.filteredMeters];
          for (var i = 1; i <= this.state.filteredMeters.length - 1; i++) {
            if (this.state.filteredMeters[i].checked) {
              newArray[i].meterCondition = CONDITION.FAULTY.name;
            }
          }
          this.setState({ meters: newArray });
        } else if (buttonIndex === 1) {
          this.sendRequestToDb(
            meterIds,
            STATUS.PICKUP.value,
            CONDITION.EXPIRED.value
          );
          alert("Status changed successfully");
          let newArray = [...this.state.filteredMeters];
          for (var i = 1; i <= this.state.filteredMeters.length - 1; i++) {
            if (this.state.filteredMeters[i].checked) {
              newArray[i].meterCondition = CONDITION.EXPIRED.name;
            }
          }
          this.setState({ meters: newArray });
        } else if (buttonIndex === 2) {
          var send = true;
          for (var i = 0; i <= meters.length - 1; i++) {
            if (meters[i].meterCondition !== "Active") {
              alert("Cannot install faulty or expired meter!!");
              send = false;
              break;
            }
          }
          if (send) {
            var newArray = this.state.filteredMeters.filter(cartItem => {
              return !cartItem.checked;
            });
            await this.setState({ meters: newArray });
            await this.setState({ filteredMeters: newArray });
            this.props.navigation.navigate("AddressForm", {
              meters: meters[0]
            });
          }
        } else if (buttonIndex === 3) {
          var newArray = this.state.filteredMeters.filter(cartItem => {
            return !cartItem.checked;
          });
          await this.setState({ meters: newArray });
          await this.setState({ filteredMeters: newArray });
          await this.forceUpdate();
          this.sendRequestToDbForReturning(meterIds, meterConditions);
          alert("Status changed successfully");
        } else {
        }
      }
    );
  }
  async _getStorageValue(){
    var value = await AsyncStorage.getItem('user')
    return value
  }
  async fetchMeters() {
    try {
      var username = await this._getStorageValue();
      let response = await fetch(
        base_url + "api/search/view-meter",
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            PayRollID: username,
            MeterStatus: 2
          })
        }
      );
      let json = await response.json();
      json.splice(0, 0, {
        mirn: "",
        meterType: null,
        meterStatus: "",
        meterCondition: null,
        expriyDate: ""
      });
      for (var i = 1; i <= json.length - 1; i++) {
        json[i].checked = false;
        if (json[i].meterCondition == CONDITION.ACTIVE.value) {
          json[i].meterCondition = CONDITION.ACTIVE.name;
        }
        if (json[i].meterCondition == CONDITION.FAULTY.value) {
          json[i].meterCondition = CONDITION.FAULTY.name;
        }
        if (json[i].meterCondition == CONDITION.EXPIRED.value) {
          json[i].meterCondition = CONDITION.EXPIRED.name;
        }
      }
      this.setState({
        meters: json,
        filteredMeters: json
      });
    } catch (error) {}
  }
  async filterSearch(text) {
    await this.setState({ filteredMeters: this.state.meters });
    const newData = this.state.filteredMeters.filter(cartItem => {
      return cartItem.mirn.indexOf(text) > -1;
    });
    this.setState({ filteredMeters: newData });
  }
  filterByRange = async () => {
    try {
      const value = await AsyncStorage.getItem("startRange");
      const valueOne = await AsyncStorage.getItem("endRange");
      if (value !== null && valueOne != null) {
        const newData = [];
        for (var i = value; i <= valueOne; i++) {
          this.state.filteredMeters.filter(cartItem => {
            if (cartItem.mirn.indexOf(i) > -1) newData.push(cartItem);
          });
        }
        newData.splice(0, 0, {
          mirn: "",
          meterType: null,
          meterStatus: "",
          meterCondition: null,
          expriyDate: ""
        });
        this.setState({ filteredMeters: newData });
      } else {
        this.setState({ filteredMeters: this.state.meters });
      }
    } catch (error) {}
  };
  async fetchAndFilter() {
    await this.fetchMeters();
    await this.filterByRange();
    await this.props.navigation.setParams({
      toggleSearch: this.toggleSearch,
      value: this.state.value,
      data: this.getMeters()
    });
  }
  render() {
    const { navigation } = this.props;
    return (
      <>
        <View>
          <NavigationEvents
            onWillFocus={() => {
              this.fetchAndFilter();
            }}
          />
          {this.state.value == true && (
            <Input
              placeholder="Search meter"
              onChangeText={text => {
                this.filterSearch(text);
              }}
              leftIcon={
                <Icon
                  name=""
                  size={15}
                  color="black"
                  style={{ marginRight: 20 }}
                />
              }
              rightIcon={
                <Icon
                  name="times"
                  size={15}
                  color="black"
                  onPress={this.toggleSearch}
                  style={{ marginRight: 20 }}
                />
              }
            />
          )}
        </View>
        <ScrollView>
          {this.state.meters.length > 1 ? (
            <View>
              {this.state.filteredMeters.map((l, i) => (
                <ListItem
                  title={l.mirn}
                  key={i}
                  subtitle={
                    l.meterStatus ? "Condition: " + l.meterCondition : ""
                  }
                  titleStyle={{ marginLeft: -20 }}
                  subtitleStyle={{ marginTop: 10, marginLeft: -20 }}
                  leftIcon={
                    <View style={{ marginLeft: -20 }}>
                      <CheckBox
                        checked={this.state.filteredMeters[i].checked}
                        disabled={false}
                        onPress={() => this.toogleCheck(i)}
                        size={20}
                      />
                    </View>
                  }
                  bottomDivider
                />
              ))}
            </View>
          ) : (
            <View style={{ alignItems: "center", marginTop: 10 }}>
              <Text>No items in your pick up list</Text>
            </View>
          )}
        </ScrollView>
        <Button
          title="Change the status"
          buttonStyle={{borderRadius: 100, marginBottom: 20, alignSelf:'center',width: this.state.buttonwidth,height: 40}}
          onPress={this.submit.bind(this)}
          disabled={!this.validate()}
        />
      </>
    );
  }
}
export default connectActionSheet(ChangeStatus);
const CONDITION = {
  ACTIVE: { value: 1, name: "Active" },
  FAULTY: { value: 2, name: "Faulty" },
  EXPIRED: { value: 3, name: "Expired" }
};
const STATUS = {
  INHOUSE: { value: 1, name: "Inhouse" },
  PICKUP: { value: 2, name: "Pickup" },
  RETURN: { value: 3, name: "Return" },
  TRANSFER: { value: 4, name: "Transfer" },
  INSTALL: { value: 5, name: "Install" }
};
