import { Component } from "solid-js";

import { AvatarMenu } from "../../user/avatar-menu/main";

const literals = {
    brand: "My SaaS",
};

export const LayoutTopBar: Component = () => {
    let navElementRef: HTMLElement;

    return (
        <>
            <nav ref={navElementRef} class="sticky top-0 bg-white shadow">
                <div class="px-4 py-3 flex justify-between items-center">
                    <div class="flex justify-center items-center gap-3">
                        <span class="font-tight font-bold">{literals.brand}</span>

                        <div class="text-sm rounded-sm bg-sky-100 text-sky-900 px-2 py-0.5">admin</div>
                    </div>

                    <AvatarMenu size={30} colors={["#2dd4bf", "#facc15", "#fb923c", "#ec4899", "#6366f1"]} />
                </div>
            </nav>
        </>
    );
};
