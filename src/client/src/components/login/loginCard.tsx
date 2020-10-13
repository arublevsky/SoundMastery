import * as React from "react";
import { connect } from "react-redux";
import { loginUserAsync } from "../../state/actions";
import { Dispatch } from "redux";
import { RootAction, LoginAction } from "../../state/types";
import { LoginCardForm } from "./loginForm";

interface Props {
    handleLogin: (email: string, password: string) => void;
}

class LoginCardComponent extends React.Component<Props> {
    render() {
        return (<LoginCardForm handleLogin={this.props.handleLogin} />);
    }
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        handleLogin: (email: string, password: string) => {
            dispatch(loginUserAsync(email, password))
        }
    }
}

const mapStateToProps = () => ({});

export const LoginCard = connect(mapStateToProps, mapDispatchToProps)(LoginCardComponent)