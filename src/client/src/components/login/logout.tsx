import * as React from "react";
import { useEffect } from "react";
import { Redirect } from "react-router-dom";

interface Props {
    handleLogout: () => void;
}

export const Logout = (props: Props) => {
    useEffect(() => {
        props.handleLogout();
    })

    return <Redirect to="/index" />;
}
