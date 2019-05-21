import * as React from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { Index } from ".";
import { About } from "./about";

export class App extends React.Component {
    render() {
        return (<Router>
            <div>
                <nav>
                    <ul>
                        <li>
                            <Link to="/">Home</Link>
                        </li>
                        <li>
                            <Link to="/about/">About</Link>
                        </li>
                    </ul>
                </nav>

                <Route path="/" exact component={Index} />
                <Route path="/about/" component={About} />
            </div>
        </Router>);
    }
}