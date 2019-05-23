import * as React from "react";
import * as ReactDOM from "react-dom";
import { App } from "./components/app";

ReactDOM.render(<Provider store={store}><App /></Provider>, document.getElementById("app"));
ReactDOM.render(<App />, document.getElementById("app"));