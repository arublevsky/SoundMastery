import * as React from "react";
import { Button, Form, Card } from "react-bootstrap";
import { RouteComponentProps, withRouter } from "react-router";

class LoginCardFormComponent extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            email: "",
            password: ""
        };
    }
    render() {
        return (<Card className="text-center">
            <Card.Header>Login</Card.Header>
            <Card.Body>
                <Card.Title>Provide your credentials</Card.Title>
                <Card.Text>
                    <Form>
                        <Form.Group controlId="formBasicEmail">
                            <Form.Label>Email address</Form.Label>
                            <Form.Control type="email" placeholder="Enter email" onChange={this.handleEmailChange} value={this.state.email} />
                            <Form.Text className="text-muted">
                                We'll never share your email with anyone else.
                            </Form.Text>
                        </Form.Group>

                        <Form.Group controlId="formBasicPassword">
                            <Form.Label>Password</Form.Label>
                            <Form.Control type="password" placeholder="Password" onChange={this.handlePasswordChange} value={this.state.password} />
                        </Form.Group>
                        <Form.Group controlId="formBasicChecbox">
                            <Form.Check type="checkbox" label="Check me out" />
                        </Form.Group>
                    </Form>
                </Card.Text>
                <Button variant="primary" onClick={this.handleLogin}>Log In</Button>
            </Card.Body>
            <Card.Footer className="text-muted">2 days ago</Card.Footer>
        </Card>);
    }

    private handleEmailChange = (event: React.SyntheticEvent) => {
        let target = event.target as HTMLInputElement;
        this.setState({
            email: target.value,
        })
    };

    private handlePasswordChange = (event: React.SyntheticEvent) => {
        let target = event.target as HTMLInputElement;
        this.setState({
            password: target.value,
        })
    };

    private handleLogin = () => {
        this.props.handleLogin(this.state.email, this.state.password);
        this.props.history.push("/");
    }
}

export const LoginCardForm = withRouter(LoginCardFormComponent);

interface Props extends RouteComponentProps {
    handleLogin: (email: string, password: string) => void;
}

interface State {
    email: string;
    password: string;
}