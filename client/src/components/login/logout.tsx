import * as React from "react";
import { Redirect } from "react-router";
import { RootAction } from "../../state/types";
import { logoutUser } from "../../state/actions";
import { connect } from "react-redux";

interface Props {
    handleLogout: () => void;
}

export class LogoutComponent extends React.Component<Props> {
    public render() {
        this.handleLogout();
        return <Redirect to="/index" />;
    }

    private handleLogout = () => {
        this.props.handleLogout();
    }
}

const mapDispatchToProps = (dispatch: React.Dispatch<RootAction>) => {
    return {
        handleLogout: () => { dispatch(logoutUser()) }
    }
}

export const Logout = connect(() => { }, mapDispatchToProps)(LogoutComponent)