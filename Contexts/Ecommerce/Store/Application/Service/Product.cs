// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Service;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Event;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Domain.ValueObject;

public class ProductService : IProductService
{
    private readonly IPublisher publisher;
    private readonly IProductRepository productRepository;

    public ProductService(IPublisher publisher, IProductRepository productRepository)
    {
        this.publisher = publisher;
        this.productRepository = productRepository;
    }

    public async Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        var id = Product.NewID();

        var newProduct = new Product(
            new ProductId(id),
            new ProductTitle(title),
            new ProductDescription(description),
            new ProductStatus((ProductStatusValue)status),
            new ProductPrice(price));

        await this.productRepository.Save(newProduct, cancellationToken);

        var productCreatedEvent = new ProductCreatedEvent { Id = id };
        await this.publisher.Publish<ProductCreatedEvent>(productCreatedEvent);

        return newProduct.Id;
    }

    public async Task DeleteProductById(Guid id, CancellationToken cancellationToken)
    {
        await this.productRepository.DeleteById(id, cancellationToken);

        var productRemovedEvent = new ProductRemovedEvent { Id = id };
        await this.publisher.Publish<ProductRemovedEvent>(productRemovedEvent);
    }
}
