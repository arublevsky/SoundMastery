import { createStore, combineReducers, applyMiddleware } from "redux";
import thunkMiddleware from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";
import { userReducer } from "../reducers";

const rootReducer = combineReducers({
    user: userReducer,
});

export type AppState = ReturnType<typeof rootReducer>;

export default function configureStore() {
    const middleWareEnhancer = applyMiddleware(thunkMiddleware);
    return createStore(rootReducer, composeWithDevTools(middleWareEnhancer));
}