import { useNavigate } from "@solidjs/router";
import type { Component } from "solid-js";

const IndexPage: Component = () => {
    const navigate = useNavigate();

    return (
        <div class="grid md:grid-cols-2">
            <article class="bg-white rounded border border-slate-200 hover:border-slate-300">
                <header class="p-3">
                    <span class="font-tight font-semibold">Ecommerce</span>
                </header>

                <hr />

                <div class="p-3 text-sm">
                    <p>Manage products, orders and billing.</p>
                </div>

                <footer class="p-3">
                    <button
                        class="bg-slate-800 text-white px-4 py-1 text-sm rounded-sm hover:bg-slate-700 active:bg-slate-900"
                        onClick={() => navigate("/ecommerce")}
                    >
                        Go now
                    </button>
                </footer>
            </article>
        </div>
    );
};

export default IndexPage;
