import * as React from "react";
import { Container, Row, Button } from "react-bootstrap";
import { refreshToken, TokenAuthorizationResult } from "../modules/authorization/authorizationApi";

interface Props {
    isLoggedIn: boolean;
    username: string;
    handleRefresh: (result: TokenAuthorizationResult) => void;
}

export const Index = (props: Props) => {
    const refreshTokenAsync = async () => {
        const result = await refreshToken();
        props.handleRefresh(result);
    }

    return (
        <Container>
            <Row>
                Welcome, {props.username}. Status: {props.isLoggedIn?.toString()}
            </Row>
            <Row>
                <Button onClick={refreshTokenAsync}>Refresh token</Button>
            </Row>
        </Container>
    );
}
