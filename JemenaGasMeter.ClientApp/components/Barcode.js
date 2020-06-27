import React, { Component } from "react";
import { Text, View, Dimensions } from "react-native";
import * as Permissions from "expo-permissions";
import { BarCodeScanner } from "expo-barcode-scanner";
import { Button } from "react-native-elements";

export default class BarcodeScannerExample extends Component {
  constructor(props) {
    super(props);
  }

  state = {
    hasCameraPermission: null,
    scanned: false,
    screen: this.props.navigation.getParam("screen")
  };
  async componentDidMount() {
    this.getPermissionsAsync();
  }
  static navigationOptions = {
    headerLeft: () => null,
    title: "Scan barcode on meter"
  };

  getPermissionsAsync = async () => {
    const { status } = await Permissions.askAsync(Permissions.CAMERA);
    this.setState({ hasCameraPermission: status === "granted" });
  };

  render() {
    const { hasCameraPermission, scanned } = this.state;
    if (hasCameraPermission === null) {
      return <Text>Requesting for camera permission</Text>;
    }
    if (hasCameraPermission === false) {
      return <Text>No access to camera</Text>;
    }
    return (
      <View
        style={{
          flex: 1,
          flexDirection: "column",
          alignItems: "center"
        }}
      >
        <BarCodeScanner
          onBarCodeScanned={scanned ? undefined : this.handleBarCodeScanned}
          style={{
            width: Math.round(Dimensions.get("window").width - 20),
            height: 300
          }}
        />
        {scanned && (
          <Button
            title={"Tap to Scan Again"}
            onPress={() => this.setState({ scanned: false })}
            style={{
              width: Math.round(Dimensions.get("window").width) - 20,
              height: 50,
              marginTop: 30
            }}
          />
        )}

        {!scanned && (
          <Button
            title={"Back to form"}
            buttonStyle={{borderRadius: 100, marginBottom: 20, alignSelf:'center',width: Math.round(Dimensions.get("window").width) - 20,height: 40,marginTop: 10}}
            onPress={() => {
              this.props.navigation.navigate(this.state.screen, {
                meter: ""
              });
            }}
          />
        )}
      </View>
    );
  }

  handleBarCodeScanned = async ({ type, data }) => {
    this.setState({ scanned: true });
    this.props.navigation.navigate(this.state.screen, {
      meter: data
    });
  };
}
