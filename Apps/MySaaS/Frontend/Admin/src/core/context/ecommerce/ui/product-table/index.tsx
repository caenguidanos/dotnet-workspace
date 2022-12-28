import { Component, For, Show, createEffect, createSignal } from "solid-js";
import { A as Link } from "@solidjs/router";
import { flexRender, getCoreRowModel, ColumnDef, createSolidTable } from "@tanstack/solid-table";

import { Product, ProductPrice, ProductStatus, ProductTitle } from "~/ecommerce/model";
import { useEcommerceProducts, ProductsResourceStatus } from "~/ecommerce/hook";

let currencyFormatEngine = new Intl.NumberFormat(navigator.languages as string[], { style: "currency", currency: "EUR" });

export const ProductTable: Component = () => {
    let [products, productsResourceStatus] = useEcommerceProducts();

    let [tableElementRef, setTableElementRef] = createSignal<HTMLTableElement>();
    let [hoveredColumn, setHoveredColumn] = createSignal<string>();

    createEffect(function handleHoveredColumnAndApplyStylesOnDemand(): void {
        if (!tableElementRef()) return;

        if (hoveredColumn()) {
            if (hoveredColumn() === "actions") return;

            let elements = tableElementRef().querySelectorAll(`td[data-column=${hoveredColumn()}]`);

            elements.forEach((element) => {
                if (element.getAttribute("data-column-hovered") === "false") {
                    element.setAttribute("data-column-hovered", "true");
                }
            });
        } else {
            let elements = tableElementRef().querySelectorAll(`td`);

            elements.forEach((element) => {
                if (element.getAttribute("data-column-hovered") === "true") {
                    element.setAttribute("data-column-hovered", "false");
                }
            });
        }
    });

    const columns: ColumnDef<Product>[] = [
        {
            accessorKey: "title",
            cell: (info) => <span>{info.getValue() as ProductTitle}</span>,
            header: () => <span class="font-bold">Title</span>,
        },
        {
            accessorKey: "price",
            cell: (info) => (
                <span class="font-mono font-light">{currencyFormatEngine.format((info.getValue() as ProductPrice) / 100)}</span>
            ),
            header: () => <span class="font-bold">Price</span>,
        },
        {
            accessorKey: "status",
            cell: (info) => {
                let value = info.getValue() as ProductStatus;

                switch (value) {
                    case ProductStatus.Draft:
                        return <span class="text-xs bg-emerald-100 rounded text-emerald-900 py-1 px-2">{value}</span>;

                    case ProductStatus.Published:
                        return <span class="text-xs bg-violet-100 rounded text-violet-900 py-1 px-2">{value}</span>;
                    default:
                        return null;
                }
            },
            header: () => <span class="font-bold">Status</span>,
        },
        {
            id: "actions",
            cell: ({ row }) => {
                return (
                    <div class="w-full grid">
                        <Link
                            href={`/ecommerce/product/${row.original.id}`}
                            class="bg-slate-800 text-white rounded-sm px-2 py-1 text-xs w-full hover:bg-slate-700 active:bg-slate-900 text-center"
                        >
                            Detail
                        </Link>
                    </div>
                );
            },
        },
    ];

    const table = createSolidTable({
        columns,
        getCoreRowModel: getCoreRowModel(),
        get data() {
            return products();
        },
    });

    return (
        <Show when={productsResourceStatus() === ProductsResourceStatus.ACTIVE}>
            <div class="w-full grid bg-white shadow text-sm">
                <table ref={setTableElementRef} class="border-collapse">
                    <thead>
                        <For each={table.getHeaderGroups()}>
                            {(headerGroup) => (
                                <tr>
                                    <For each={headerGroup.headers}>
                                        {(header) => (
                                            <th
                                                class="p-2 border border-slate-400 text-left bg-slate-200"
                                                classList={{ "hover:bg-slate-300": header.id !== "actions" }}
                                                onMouseEnter={() => setHoveredColumn(header.column.id)}
                                                onMouseLeave={() => setHoveredColumn(undefined)}
                                            >
                                                {header.isPlaceholder
                                                    ? null
                                                    : flexRender(header.column.columnDef.header, header.getContext())}
                                            </th>
                                        )}
                                    </For>
                                </tr>
                            )}
                        </For>
                    </thead>

                    <tbody>
                        <For each={table.getRowModel().rows}>
                            {(row) => (
                                <tr class="hover:bg-yellow-100">
                                    <For each={row.getVisibleCells()}>
                                        {(cell) => (
                                            <td
                                                data-column={cell.column.id}
                                                data-column-hovered={false}
                                                class="p-2 border border-slate-400 data-[column-hovered=true]:bg-yellow-100"
                                                classList={{
                                                    "text-right": cell.column.id === "price",
                                                    "text-center": cell.column.id === "status",
                                                    "w-20": cell.column.id === "actions",
                                                }}
                                            >
                                                {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                            </td>
                                        )}
                                    </For>
                                </tr>
                            )}
                        </For>
                    </tbody>
                </table>
            </div>
        </Show>
    );
};
