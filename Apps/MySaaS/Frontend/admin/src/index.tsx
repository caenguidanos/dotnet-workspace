/* @refresh reload */
import { render } from "solid-js/web";
import { Router } from "@solidjs/router";

import App from "./_app";

import "./index.scss";

render(
    () => (
        <Router>
            <App />
        </Router>
    ),
    document.getElementById("root") as HTMLElement
);
