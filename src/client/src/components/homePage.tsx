import * as React from "react";
import { Container, Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useAuthorization } from "../modules/authorization/context";

export const HomePage = () => {
    const { isAuthorized } = useAuthorization();

    return (
        <Container>
            <Row>Welcome, Home page</Row>
            <Row>
                {isAuthorized() && <Link to="/admin">Admin Page</Link>}
            </Row>
        </Container>);
}
