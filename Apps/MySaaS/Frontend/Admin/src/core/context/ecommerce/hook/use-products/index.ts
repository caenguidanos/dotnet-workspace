import { Accessor, createEffect, createResource, createSignal, onCleanup } from "solid-js";

import { HttpStatus } from "~/common";
import { Product } from "~/ecommerce/model";
import { productHttpDataAccess } from "~/ecommerce/data-access";

export const enum ProductsResourceStatus {
    IDLE,
    LOADING,
    ERROR,
    EMPTY,
    ACTIVE,
}

export function useEcommerceProducts(): [Accessor<Product[]>, Accessor<ProductsResourceStatus>] {
    let abortController = new AbortController();

    let [products, setProducts] = createSignal<Product[]>([]);
    let [productsResource] = createResource(() => productHttpDataAccess.getAllProducts({ abortController }));
    let [productsResourceStatus, setProductsResourceStatus] = createSignal<ProductsResourceStatus>(ProductsResourceStatus.IDLE);

    createEffect(() => {
        if (productsResource.loading) {
            setProductsResourceStatus(ProductsResourceStatus.LOADING);
            return;
        }

        switch (productsResource().status) {
            case HttpStatus.OK:
                if (productsResource().payload.length) {
                    setProductsResourceStatus(ProductsResourceStatus.ACTIVE);
                } else {
                    setProductsResourceStatus(ProductsResourceStatus.EMPTY);
                }

                setProducts(productsResource().payload);
                break;

            default:
                setProductsResourceStatus(ProductsResourceStatus.ERROR);
        }
    });

    onCleanup(() => abortController.abort());

    return [products, productsResourceStatus];
}
