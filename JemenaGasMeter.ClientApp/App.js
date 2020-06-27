import { createAppContainer, NavigationContainer } from "react-navigation";
import { createStackNavigator } from "react-navigation-stack";
import Login from "./components/Login";
import Options from "./components/Options";
import Pickup from "./components/Pickup";
import Barcode from "./components/Barcode";
import CartScreen from "./components/CartScreen";
import ChangeStatus from "./components/ChangeStatus";
import Transfer from "./components/Transfer";
import AddressForm from "./components/AddressForm";
import TransfereeForm from "./components/TransfereeForm";
import Filter from "./components/Filter";
import React, { Component } from "react";
import { Provider } from "react-redux";
import store from "./store";
import { ActionSheetProvider } from '@expo/react-native-action-sheet'

const RootStack = createStackNavigator(
  {
    Home: Login,
    Options: Options,
    Barcode: Barcode,
    Pickup: Pickup,
    CartScreen: CartScreen,
    ChangeStatus: ChangeStatus,
    Filter: Filter,
    AddressForm: AddressForm,
    Transfer: Transfer,
    TransfereeForm: TransfereeForm
  },
  {
    initialRouteName: "Home"
  }
);

const AppContainer = createAppContainer(RootStack);

export default class App extends Component {
  render() {
    console.disableYellowBox = true; 
    return (
      <Provider store={store}>
        <ActionSheetProvider>
          <AppContainer />
        </ActionSheetProvider>
      </Provider>
    );
  }
}
