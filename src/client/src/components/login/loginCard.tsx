import * as React from "react";
import { LoginCardForm } from "./loginForm";

interface Props {
    handleLogin: (email: string, password: string) => void;
}

export const LoginCard = (props: Props) => {
    return (<LoginCardForm handleLogin={props.handleLogin} />);
}