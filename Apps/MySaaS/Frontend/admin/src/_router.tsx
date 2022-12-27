import { lazy } from "solid-js";
import type { RouteDefinition } from "@solidjs/router";

export const routeDefinitions: RouteDefinition[] = [
    {
        path: "/",
        component: lazy(() => import("./core/view/index.layout")),
        children: [
            {
                path: "/",
                component: lazy(() => import("./core/view/index.page")),
            },
            {
                path: "/ecommerce",
                component: lazy(() => import("./core/view/ecommerce/index.page")),
            },
        ],
    },
];
