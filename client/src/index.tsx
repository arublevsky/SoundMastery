import * as React from "react";
import * as ReactDOM from "react-dom";
import { App } from "./components/app";

declare var module: any;
module.hot.accept();
ReactDOM.render(<App />, document.getElementById("app"));