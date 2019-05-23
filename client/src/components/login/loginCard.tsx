import * as React from "react";
import { connect } from "react-redux";
import { loginUser } from "../../state/actions";
import { Dispatch } from "redux";
import { RootAction } from "../../state/types";
import { LoginCardForm } from "./loginForm";

interface Props {
    handleLogin: (email: string, password: string) => void;
}

class LoginCardComponent extends React.Component<Props> {
    render() {
        return (<LoginCardForm handleLogin={this.props.handleLogin} />);
    }
}

const mapDispatchToProps = (dispatch: Dispatch<RootAction>) => {
    return {
        handleLogin: (email: string, password: string) => {
            dispatch(loginUser(email, password))
        }
    }
}

export const LoginCard = connect(() => { }, mapDispatchToProps)(LoginCardComponent)