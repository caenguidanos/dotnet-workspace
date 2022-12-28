import { Component, JSX, Show, children, createSignal, onCleanup } from "solid-js";

interface AvatarMenuProps {
    size: number;
    colors: string[];
    header: JSX.Element;
    body: JSX.Element;
    footer: JSX.Element;
}

const enum AvatarMenuDropdownStatus {
    CLOSED,
    OPENED,
}

export const AvatarMenu: Component<AvatarMenuProps> = (props) => {
    let elementRef: HTMLDivElement;

    let AvatarMenuDropdownHeader = children(() => props.header);
    let AvatarMenuDropdownBody = children(() => props.body);
    let AvatarMenuDropdownFooter = children(() => props.footer);

    const [avatarDropdownMenuStatus, setAvatarDrodownMenuStatus] = createSignal<AvatarMenuDropdownStatus>(
        AvatarMenuDropdownStatus.CLOSED
    );

    function handleOutSideClickEvent(ev: Event): void {
        if (!elementRef.contains(ev.target as Node)) {
            if (avatarDropdownMenuStatus() === AvatarMenuDropdownStatus.OPENED) {
                setAvatarDrodownMenuStatus(AvatarMenuDropdownStatus.CLOSED);
            }
        }
    }

    document.body.addEventListener("click", handleOutSideClickEvent);

    onCleanup(() => document.body.removeEventListener("click", handleOutSideClickEvent));

    return (
        <div ref={elementRef} class="relative grid place-content-center">
            <button
                onClick={() => setAvatarDrodownMenuStatus(AvatarMenuDropdownStatus.OPENED)}
                class="active:ring-pink-500 rounded-full ring ring-transparent hover:ring-teal-500 hover:ring-offset-2 transition-all duration-150 ease-in-out motion-reduce:transition-none"
            >
                <img width={props.size} height={props.size} src={composeAvatarImageUrl(props.size, props.colors)} />
            </button>

            <Show when={avatarDropdownMenuStatus() === AvatarMenuDropdownStatus.OPENED}>
                <div class="absolute bg-white w-80 h-auto right-4 top-4 rounded shadow-lg text-sm border border-slate-200">
                    <div class="grid">
                        <AvatarMenuDropdownHeader />
                        <hr />
                        <AvatarMenuDropdownBody />
                        <hr />
                        <AvatarMenuDropdownFooter />
                    </div>
                </div>
            </Show>
        </div>
    );
};

function composeAvatarImageUrl(size: number, colors: string[]): string {
    let processedColors = colors.join(",").replace(/#/g, "");
    let url = new URL("https://source.boringavatars.com");
    url.pathname = `/pixel/${size}/${Date.now()}`;
    url.searchParams.set("colors", processedColors);
    return url.toString();
}
