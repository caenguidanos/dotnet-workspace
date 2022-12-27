import { Component, Show, createSignal, onCleanup } from "solid-js";

interface AvatarMenuProps {
    size: number;
    colors: string[];
}

export const AvatarMenu: Component<AvatarMenuProps> = (props) => {
    let elementRef: HTMLDivElement;

    let processedColors = props.colors.join(",").replace(/#/g, "");
    let url = new URL("https://source.boringavatars.com");
    url.pathname = `/pixel/${props.size}/${Date.now()}`;
    url.searchParams.set("colors", processedColors);

    const [showAvatarDropdownMenu, setShowAvatarDrodownMenu] = createSignal<boolean>(false);

    function handleOutSideClickEvent(ev: Event): void {
        if (!elementRef.contains(ev.target as Node)) {
            if (showAvatarDropdownMenu()) {
                setShowAvatarDrodownMenu(false);
            }
        }
    }

    document.body.addEventListener("click", handleOutSideClickEvent);
    onCleanup(() => document.body.removeEventListener("click", handleOutSideClickEvent));

    return (
        <div ref={elementRef} class="relative grid place-content-center">
            <button
                onClick={() => setShowAvatarDrodownMenu(true)}
                class="active:ring-pink-500 rounded-full ring ring-transparent hover:ring-teal-500 hover:ring-offset-2 transition-all duration-150 ease-in-out motion-reduce:transition-none"
            >
                <img width={props.size} height={props.size} src={url.toString()} />
            </button>

            <Show when={showAvatarDropdownMenu()}>
                <div class="absolute bg-white w-80 h-auto right-4 top-4 rounded shadow-lg text-sm border border-slate-200">
                    <div class="grid">
                        <div class="p-2">
                            <span>
                                Signed in as <span class="font-semibold">admin</span>
                            </span>
                        </div>

                        <hr />

                        <div class="py-2">
                            <button class="w-full p-2 hover:bg-slate-100 active:bg-slate-200 text-left">Ecommerce</button>
                        </div>

                        <hr />

                        <div class="py-2">
                            <button class="w-full p-2 hover:bg-red-100 active:bg-red-200 text-red-500 text-left">Sign out</button>
                        </div>
                    </div>
                </div>
            </Show>
        </div>
    );
};
