import React from "react";
import { View, Text } from "react-native";

import { connect } from "react-redux";
import Icon from "react-native-vector-icons/FontAwesome";
import { withNavigation } from "react-navigation";

const PickupIcon = props => (
  <View style={{ padding: 5 }}>
    <View
      style={{
        position: "absolute",
        height: 25,
        width: 25,
        borderRadius: 15,
        backgroundColor: "rgba(95,197,123,0.8)",
        left: 15,
        bottom: 20,
        alignItems: "center",
        justifyContent: "center",
        zIndex: 2000
      }}
    >
      <Text style={{ color: "white", fontWeight: "bold" }}>{props.count}</Text>
    </View>
    <Icon
      onPress={() => props.navigation.navigate("CartScreen")}
      name="truck"
      size={30}
    />
  </View>
);

const mapStateToProps = state => {
  return {
    cartItems: state.cartItems,
    count: state.count
  };
};

export default connect(mapStateToProps)(withNavigation(PickupIcon));
