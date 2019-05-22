import * as React from "react";
import { Navbar, Nav } from "react-bootstrap";

interface Props {
    isLoggedIn: boolean;
}

export class NavBarComponent extends React.Component<Props> {
    render() {
        return (<Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
            <Navbar.Brand href={"/login"}>Sound Mastery</Navbar.Brand>
            <Navbar.Toggle aria-controls="responsive-navbar-nav" />
            <Navbar.Collapse id="responsive-navbar-nav">
                <Nav className="mr-auto">
                    {/* placeholder to keep the following buttons aligned to the right side */}
                    <Nav.Link></Nav.Link> 
                </Nav>
                <Nav>
                    {!this.props.isLoggedIn && <Nav.Link href={"/login"}>Login</Nav.Link>}
                    {this.props.isLoggedIn && <Nav.Link href={"/logout"}>Logout</Nav.Link>}
                </Nav>
            </Navbar.Collapse>
        </Navbar>);
    }
}