import * as React from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { Index } from ".";
import { NavBarComponent } from "./layout/navbar";
import authStore from "../stores/authorizationStore";
import { Login } from "./login";
import { Logout } from "./logout";

export class App extends React.Component {
    render() {
        return (
        <Router>
            <NavBarComponent isLoggedIn={authStore.isLoggedIn}/>
            <Route path="/" exact component={Index} />
            <Route path="/index" component={Index} />
            <Route path="/login" component={Login} />
            <Route path="/logout" component={Logout} />
        </Router>);
    }
}