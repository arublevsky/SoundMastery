import * as React from "react";
import { useEffect, useState } from "react";
import { Container, Row } from "react-bootstrap";
import { loadValues } from "../modules/values/valuesApi";

export const AdminPage = () => {
    const [values, setValues] = useState<string[]>([]);

    useEffect(() => {
        async function fetch() {
            const data = await loadValues();
            setValues(data);
        }

        fetch();
    }, []);

    return (
        <Container>
            <Row>
                Welcome, Admin page
            </Row>
            {values && values.map(x => <Row key={x}>{x}</Row>)}
        </Container>);
}
