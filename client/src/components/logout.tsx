import * as React from "react";
import authStore from "../stores/authorizationStore";
import { Redirect } from "react-router";

export class Logout extends React.Component {
    public render() {
        this.handleLogout();
        return <Redirect to="/login" />;
    }

    private handleLogout = () => {
        authStore.logout();
    }
}