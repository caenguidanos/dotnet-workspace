import { HttpDataAccessResult } from "~/common";
import { Product } from "~/ecommerce/model";

class ProductHttpDataAccess {
    private readonly apiVersion: string = "1.0";

    public async getAllProducts(options: { abortController: AbortController }): Promise<HttpDataAccessResult<Product[]>> {
        let url = new URL("http://localhost:5000");

        url.pathname = "/product";

        let responseStatus = null;
        let responsePayload = null;

        try {
            let response = await fetch(url.toString(), {
                method: "GET",
                signal: options.abortController.signal,
                headers: {
                    "x-api-version": this.apiVersion,
                },
            });

            if (response.ok) {
                responseStatus = response.status;
                responsePayload = await response.json();
            }
        } catch (e) {
            console.error(e);
        }

        return { payload: responsePayload, status: responseStatus };
    }
}

export const productHttpDataAccess = new ProductHttpDataAccess();
