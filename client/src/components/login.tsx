import * as React from "react";
import { Button, Form, Card, Container, Col } from "react-bootstrap";
import authStore from "../stores/authorizationStore";
import { RouteComponentProps } from "react-router";
import { Row } from "react-bootstrap";
import './login.css';

export class Login extends React.Component<RouteComponentProps> {
    render() {
        return (
            <>
                <div className="loginPage">
                    <Container>
                        <Row>
                            <Col></Col>
                            <Col xs={6}>
                                <LoginCard {...this.props} />
                            </Col>
                            <Col></Col>
                        </Row>
                    </Container>
                </div>
            </>);
    }
}
export class LoginCard extends React.Component<RouteComponentProps> {
    render() {
        return (<Card className="text-center">
            <Card.Header>Login</Card.Header>
            <Card.Body>
                <Card.Title>Provide your credentials</Card.Title>
                <Card.Text>
                    <LoginForm {...this.props} />
                </Card.Text>
                <Button variant="primary" onClick={this.handleLogin}>Log In</Button>
            </Card.Body>
            <Card.Footer className="text-muted">2 days ago</Card.Footer>
        </Card>);
    }

    private handleLogin = () => {
        authStore.login();
        this.props.history.push("/");
    }
}

class LoginForm extends React.Component<RouteComponentProps> {
    render() {
        return (<Form>
            <Form.Group controlId="formBasicEmail">
                <Form.Label>Email address</Form.Label>
                <Form.Control type="email" placeholder="Enter email" />
                <Form.Text className="text-muted">
                    We'll never share your email with anyone else.
              </Form.Text>
            </Form.Group>

            <Form.Group controlId="formBasicPassword">
                <Form.Label>Password</Form.Label>
                <Form.Control type="password" placeholder="Password" />
            </Form.Group>
            <Form.Group controlId="formBasicChecbox">
                <Form.Check type="checkbox" label="Check me out" />
            </Form.Group>
        </Form>);
    }  
}
