import * as React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Index } from "./components";
import { TopNavBar } from "./components/layout/navbar";
import { Login } from "./components/login/login";
import { Logout } from "./components/login/logout";

export class App extends React.Component {
    render() {
        return (
            <Router>
                <TopNavBar />
                <Route path="/" exact component={Index} />
                <Route path="/index" component={Index} />
                <Route path="/login" component={Login} />
                <Route path="/logout" component={Logout} />
            </Router>);
    }
}