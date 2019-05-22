import * as React from "react";
import { Container, Row } from "react-bootstrap";
import authStore from "../stores/authorizationStore";

export class Index extends React.Component {
    render() {
        return (
            <Container>
                <Row>
                    Welcome. Status: {authStore.isLoggedIn.toString()}
                </Row>
            </Container >
        );
    }
}