import { useParams } from "@solidjs/router";
import type { Component } from "solid-js";

const EcommerceProductByIdPage: Component = () => {
    const params = useParams();

    return <p>Product: {params.id}</p>;
};

export default EcommerceProductByIdPage;
