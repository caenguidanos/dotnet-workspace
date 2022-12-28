export type ProductId = string;

export type ProductTitle = string;

export type ProductDescription = string;

export type ProductPrice = number;

export const enum ProductStatus {
    Published = "published",
    Draft = "draft",
}

export interface Product {
    id: ProductId;
    title: ProductTitle;
    description: ProductDescription;
    price: ProductPrice;
    status: ProductStatus;
}
