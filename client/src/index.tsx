import * as React from "react";
import * as ReactDOM from "react-dom";

import { Hello } from "./components/home";

declare var module: any;
module.hot.accept();
ReactDOM.render(<Hello compiler="XXX" framework="YYY" />, document.getElementById("app"));