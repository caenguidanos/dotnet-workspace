import path from "node:path";

import { defineConfig } from "vite";
import solidPlugin from "vite-plugin-solid";

const root = (...paths: string[]) => path.resolve(process.cwd(), ...paths);

export default defineConfig({
    plugins: [solidPlugin()],
    server: {
        port: 5173,
    },
    build: {
        target: "esnext",
    },
    resolve: {
        alias: {
            "~": root("src", "core", "context"),
        },
    },
});
