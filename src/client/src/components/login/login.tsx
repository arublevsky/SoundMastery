import * as React from "react";
import { Container, Col } from "react-bootstrap";
import { Row } from "react-bootstrap";
import { LoginCard } from "./loginCard";
import './login.css';
import { login } from "../../modules/authorization/authorizationApi";
import { useAuthorization } from "../../modules/authorization/context";
import { Redirect, RouteProps, useHistory } from "react-router-dom";

interface LoginRouteState {
    referer: string;
}

export const Login = (props: RouteProps) => {
    const history = useHistory();
    const { isAuthorized, onLogin } = useAuthorization();

    const handleLogin = async (email: string, password: string) => {
        const result = await login(email, password);
        onLogin(result);
        history.push("/home");
    }

    if (isAuthorized()) {
        const referer = (props.location.state as LoginRouteState)?.referer || '/';
        return <Redirect to={referer} />;
    }

    return (
        <>
            <div className="loginPage">
                <Container>
                    <Row>
                        <Col></Col>
                        <Col xs={6}>
                            <LoginCard handleLogin={handleLogin} />
                        </Col>
                        <Col></Col>
                    </Row>
                </Container>
            </div>
        </>);
}
