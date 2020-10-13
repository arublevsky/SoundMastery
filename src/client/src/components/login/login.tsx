import * as React from "react";
import { Container, Col } from "react-bootstrap";
import { Row } from "react-bootstrap";
import { LoginCard } from "./loginCard";
import './login.css';

export class Login extends React.Component {
    render() {
        return (
            <>
                <div className="loginPage">
                    <Container>
                        <Row>
                            <Col></Col>
                            <Col xs={6}>
                                <LoginCard />
                            </Col>
                            <Col></Col>
                        </Row>
                    </Container>
                </div>
            </>);
    }
}
