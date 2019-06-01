import * as React from "react";
import { Container, Row, Button } from "react-bootstrap";
import { connect } from "react-redux";
import { AppState } from "../state/store";
import { refreshToken, TokenAuthorizationResult } from "../modules/authorization/authorizationApi";
import { loginUser } from "../state/actions";

interface Props {
    isLoggedIn: boolean;
    username: string;
    handleRefresh: (result: TokenAuthorizationResult) => void;
}

export class IndexComponent extends React.Component<Props> {
    render() {
        return (
            <Container>
                <Row>
                    Welcome, {this.props.username}. Status: {this.props.isLoggedIn.toString()}
                </Row>
                <Row>
                    <Button onClick={this.refreshToken}>Refresh token</Button>
                </Row>
            </Container>
        );
    }

    private refreshToken = async () => {
        const result = await refreshToken();
        this.props.handleRefresh(result);
    }
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        handleRefresh: (result: TokenAuthorizationResult) => {
            dispatch(loginUser(result))
        }
    }
}

const mapStateToProps = (state: AppState) => ({
    isLoggedIn: state.user.isLoggedIn(),
});

export const Index = connect(mapStateToProps, mapDispatchToProps)(IndexComponent);