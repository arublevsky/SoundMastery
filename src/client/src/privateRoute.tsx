import React from "react";
import { Route, Redirect, RouteProps } from "react-router-dom";
import { useAuthorization } from "./modules/authorization/context";

export const PrivateRoute = ({ component: Component, ...rest }: RouteProps) => {
    const { isAuthorized } = useAuthorization();

    return (
        <Route
            {...rest}
            render={(props) => isAuthorized()
                ? (<Component {...props} />)
                : (<Redirect
                    to={{ pathname: "/login", state: { referer: props.location } }}
                />)
            }
        />
    );
}