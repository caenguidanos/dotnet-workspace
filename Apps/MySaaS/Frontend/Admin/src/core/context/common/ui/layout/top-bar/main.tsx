import { Component, For } from "solid-js";
import { useLocation, useNavigate, A as Link } from "@solidjs/router";

import { AvatarMenu } from "../../user/avatar-menu/main";

const literals = {
    brand: "My SaaS",
};

export const LayoutTopBar: Component = () => {
    let navElementRef: HTMLElement;

    let navigate = useNavigate();
    let location = useLocation();

    let avatarDropdownMenuItems = [
        {
            title: "Portal",
            description: "Access to the portal and manage your system.",
            path: "/",
        },
        {
            title: "Ecommerce",
            description: "Manage products, orders and billing.",
            path: "/ecommerce",
        },
    ];

    return (
        <>
            <nav ref={navElementRef} class="sticky top-0 bg-white border-b border-slate-200">
                <div class="px-4 py-3 flex justify-between items-center">
                    <div class="flex justify-center items-center gap-3">
                        <Link href="/">
                            <span class="font-tight font-bold">{literals.brand}</span>
                        </Link>

                        <div class="text-sm rounded-sm bg-sky-100 text-sky-900 px-2 py-0.5">admin</div>
                    </div>

                    <AvatarMenu
                        size={30}
                        colors={["#2dd4bf", "#facc15", "#fb923c", "#ec4899", "#6366f1"]}
                        header={
                            <div class="p-2">
                                <span>
                                    Signed in as <span class="font-semibold">admin</span>
                                </span>
                            </div>
                        }
                        body={
                            <div class="py-2">
                                <For each={avatarDropdownMenuItems}>
                                    {(item) => (
                                        <Link href={item.path}>
                                            <div
                                                class="border-transparent w-full border-l-[3px] p-2 hover:bg-slate-100 active:bg-slate-200 text-left grid data-[active=true]:border-blue-500 data-[active=true]:bg-blue-50 data-[active=true]:hover:bg-blue-50"
                                                data-active={location.pathname === item.path}
                                                onClick={() => navigate(item.path)}
                                            >
                                                <span class="font-medium">{item.title}</span>
                                                <span class="text-xs text-slate-500">{item.description}</span>
                                            </div>
                                        </Link>
                                    )}
                                </For>
                            </div>
                        }
                        footer={
                            <div class="py-2">
                                <button class="w-full p-2 hover:bg-red-50 active:bg-red-100 text-red-500 text-left">
                                    Sign out
                                </button>
                            </div>
                        }
                    />
                </div>
            </nav>
        </>
    );
};
