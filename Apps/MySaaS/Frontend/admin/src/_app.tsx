import { useRoutes } from "@solidjs/router";
import type { Component } from "solid-js";

import { routeDefinitions } from "./_router";

const App: Component = () => {
    return useRoutes(routeDefinitions);
};

export default App;
