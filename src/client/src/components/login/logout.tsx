import * as React from "react";
import { useEffect } from "react";
import { Redirect } from "react-router-dom";
import { useAuthorization } from "../../modules/authorization/context";

interface Props {
    handleLogout: () => void;
}

export const Logout = (props: Props) => {
    const context = useAuthorization();

    useEffect(() => {
        context.onLogout();
    }, []);

    return <Redirect to="/home" />;
}
