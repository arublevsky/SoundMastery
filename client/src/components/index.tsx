import * as React from "react";
import { Container, Row } from "react-bootstrap";
import { connect } from "react-redux";
import { AppState } from "../state/store";

interface Props {
    isLoggedIn: boolean;
    username: string;
}
export class IndexComponent extends React.Component<Props> {
    render() {
        return (
            <Container>
                <Row>
                    Welcome, {this.props.username}. Status: {this.props.isLoggedIn.toString()}
                </Row>
            </Container>
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    isLoggedIn: state.user.isLoggedIn,
    username: state.user.login,
});

export const Index = connect(mapStateToProps)(IndexComponent);