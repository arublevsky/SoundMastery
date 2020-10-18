import * as React from "react";
import { useEffect } from "react";
import { Redirect } from "react-router-dom";
import { useAuthorization } from "../../modules/authorization/context";

export const Logout = () => {
    const context = useAuthorization();

    useEffect(() => {
        context.onLogout();
    }, []);

    return <Redirect to="/home" />;
}
