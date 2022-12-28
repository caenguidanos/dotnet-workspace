import { Outlet } from "@solidjs/router";
import type { Component } from "solid-js";

const EcommerceLayout: Component = () => {
    return (
        <div class="py-2">
            <Outlet />
        </div>
    );
};

export default EcommerceLayout;
