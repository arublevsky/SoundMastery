import * as React from "react";
import { useState } from "react";
import { Button, Form, Card } from "react-bootstrap";
import { useHistory } from "react-router-dom";

export const LoginCardForm = (props: Props) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const history = useHistory();

    const handleEmailChange = (event: React.SyntheticEvent) => {
        let target = event.target as HTMLInputElement;
        setEmail(target.value);
    };

    const handlePasswordChange = (event: React.SyntheticEvent) => {
        let target = event.target as HTMLInputElement;
        setPassword(target.value);
    };

    const handleLogin = () => {
        props.handleLogin(email, password);
        history.push("/");
    }

    return (<Card className="text-center">
        <Card.Header>Login</Card.Header>
        <Card.Body>
            <Card.Title>Provide your credentials</Card.Title>
            <Card.Text>
                <Form>
                    <Form.Group controlId="formBasicEmail">
                        <Form.Label>Email address</Form.Label>
                        <Form.Control type="email" placeholder="Enter email" onChange={handleEmailChange} value={email} />
                        <Form.Text className="text-muted">
                            We'll never share your email with anyone else.
                            </Form.Text>
                    </Form.Group>

                    <Form.Group controlId="formBasicPassword">
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password" onChange={handlePasswordChange} value={password} />
                    </Form.Group>
                    <Form.Group controlId="formBasicChecbox">
                        <Form.Check type="checkbox" label="Check me out" />
                    </Form.Group>
                </Form>
            </Card.Text>
            <Button variant="primary" onClick={handleLogin}>Log In</Button>
        </Card.Body>
        <Card.Footer className="text-muted">2 days ago</Card.Footer>
    </Card>);
}

interface Props {
    handleLogin: (email: string, password: string) => void;
}

interface State {
    email: string;
    password: string;
}