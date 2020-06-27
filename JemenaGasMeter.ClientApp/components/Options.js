import React, { Component } from "react";
import { View, AsyncStorage, Dimensions, StyleSheet, Text } from "react-native";
import Icon from "react-native-vector-icons/FontAwesome";
import base_url from "../constants/constants";

export default class Options extends Component {
  constructor(props) {
    super(props);
    this.state = {
      buttonwidth: Math.round(Dimensions.get("window").width) - 30
    };
  }
  static navigationOptions = {
    headerLeft: () => null
  };
  async clearAsyncStorage() {
    const asyncStorageKeys = await AsyncStorage.getAllKeys();
    if (asyncStorageKeys.length > 0) {
      AsyncStorage.clear();
    }
  }
  async resetDB() {
    try {
      fetch(base_url + "api/meterhistories/delete", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        }
      });
      fetch(base_url + "api/meters/reset", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        }
      });
      fetch(base_url + "api/transfers/delete", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        }
      });
      fetch(base_url + "api/installations/delete", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        }
      });
    } catch (error) {}
  }
  render() {
    const { navigation } = this.props;
    return (
      <View style={styles.container}>
        <View style={styles.bottom}>
          <View style={styles.bottomItem}>
            <View style={styles.bottomItemInner}>
              <Icon
                style={styles.icon}
                onPress={() => {
                  this.props.navigation.navigate("Pickup");
                }}
                name="truck"
                size={70}
                color="white"
              />
              <Text style={styles.textOption}>Pick Up Meters</Text>
            </View>
          </View>
          <View style={styles.bottomItem}>
            <View style={styles.bottomItemInner}>
              <Icon
                style={styles.icon}
                onPress={() => {
                  this.props.navigation.navigate("ChangeStatus");
                }}
                name="list"
                size={70}
                color="white"
              />
              <Text style={styles.textOption}>Change status</Text>
            </View>
          </View>
          <View style={styles.bottomItem}>
            <View style={styles.bottomItemInner}>
              <Icon
                style={styles.icon}
                onPress={() => {
                  this.props.navigation.navigate("Transfer");
                }}
                name="users"
                size={70}
                color="white"
              />
              <Text style={styles.textOption}>Transfer Meters</Text>
            </View>
          </View>
          <View style={styles.bottomItem}>
            <View style={styles.bottomItemInner}>
              <Icon
                style={styles.icon}
                onPress={() => {
                  this.props.navigation.navigate("Home");
                }}
                name="user"
                size={70}
                color="white"
              />
              <Text style={styles.textOption}>Logout</Text>
            </View>
          </View>
          <View style={styles.bottomItem}>
            <View style={styles.bottomItemInner}>
              <Icon
                style={styles.icon}
                onPress={() => {
                  this.resetDB();
                }}
                name="user"
                size={70}
                color="white"
              />
              <Text style={styles.textOption}>Reset database</Text>
            </View>
          </View>
        </View>
      </View>
    );
  }
}
const styles = StyleSheet.create({
  bottom: {
    flexDirection: "row",
    flexWrap: "wrap",
    padding: 10,
    height: "50%"
  },
  bottomItem: {
    width: "50%",
    height: "50%",
    padding: 10
  },
  bottomItemInner: {
    flex: 1,
    backgroundColor: "#306ccf",
    alignItems: "center"
  },
  container: {
    flex: 1
  },
  textOption: {
    color: "white",
    fontSize: 20,
    marginTop: 20
  },
  icon: {
    marginTop: 5
  }
});
