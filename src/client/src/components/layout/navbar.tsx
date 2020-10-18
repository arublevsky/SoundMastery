import * as React from "react";
import { Navbar, Nav } from "react-bootstrap";
import { useAuthorization } from "../../modules/authorization/context";

export const TopNavBar = () => {
    const { isAuthorized } = useAuthorization();

    return (
        <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
            <Navbar.Brand href={"/home"}>Sound Mastery</Navbar.Brand>
            <Navbar.Toggle aria-controls="responsive-navbar-nav" />
            <Navbar.Collapse id="responsive-navbar-nav">
                <Nav className="mr-auto">
                    {/* placeholder to keep the following buttons aligned to the right side */}
                    <Nav.Link></Nav.Link>
                </Nav>
                <Nav>
                    {!isAuthorized() && <Nav.Link href={"/login"}>Login</Nav.Link>}
                    {isAuthorized() && <Nav.Link href={"/logout"}>Logout</Nav.Link>}
                </Nav>
            </Navbar.Collapse>
        </Navbar>);
}