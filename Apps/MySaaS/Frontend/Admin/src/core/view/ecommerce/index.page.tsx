import { Component } from "solid-js";

import { ProductTable } from "~/ecommerce";

const EcommercePage: Component = () => {
    return (
        <div class="grid gap-5">
            <div class="grid gap-2">
                <h1 class="font-bold text-xl">Ecommerce</h1>

                <p class="text-sm">
                    Lorem ipsum, dolor sit amet consectetur adipisicing elit. Sit, quisquam ex. Odio dolorem eaque at reiciendis
                    ipsum quae sint fuga doloremque, praesentium numquam dolores voluptatem ipsam voluptatibus doloribus
                    repudiandae maxime!
                </p>
            </div>

            <ProductTable />
        </div>
    );
};

export default EcommercePage;
