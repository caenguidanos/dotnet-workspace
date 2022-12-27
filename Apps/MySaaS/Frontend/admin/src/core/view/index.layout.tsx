import { Outlet } from "@solidjs/router";
import type { Component } from "solid-js";

import { LayoutTopBar } from "../context/common";

const IndexLayout: Component = () => {
    return (
        <>
            <LayoutTopBar />

            <main class="p-3 min-h-[90vh]">
                <Outlet />
            </main>
        </>
    );
};

export default IndexLayout;
