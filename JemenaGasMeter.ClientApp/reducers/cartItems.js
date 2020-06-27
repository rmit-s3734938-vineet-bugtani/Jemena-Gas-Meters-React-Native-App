const initialState = {
  count: 0,
  cartItems: [],
  cartItemsDB: []
};

const cartItems = (state = initialState, action) => {
  switch (action.type) {
    case "ADD_TO_CART":
      var meters = action.payload.metersDB;
      if (Array.isArray(meters)) {
        for (let i = 0; i <= meters.length - 1; i++) {
          state.cartItemsDB.push(meters[i]);
        }
      } else {
        state.cartItemsDB.push(meters);
      }
      return Object.assign({}, state, {
        cartItems: [...state.cartItems, action.payload.meter],
        cartItemsDB: [...state.cartItemsDB],
        count: state.count + action.payload.count
      });
    case "REMOVE_FROM_CART":
      var meters = action.payload.metersDB;
      return Object.assign({}, state, {
        cartItems: state.cartItems.filter(
          cartItem => cartItem !== action.payload.meter
        ),
        cartItemsDB: state.cartItemsDB.filter(item => !meters.includes(item)),
        count: state.count - action.payload.count
      });
    case "CLEAR_CART":
      return Object.assign({}, state, {
        cartItems: [],
        cartItemsDB: [],
        count: 0
      });
  }
  return state;
};

export default cartItems;
