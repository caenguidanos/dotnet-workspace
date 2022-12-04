// <copyright file="ProductCreated.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Event;

using Ecommerce.Store.Domain.Event;
using Ecommerce.Store.Domain.Service;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IProductService productService;

    public ProductCreatedEventHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"Product created with id={notification.Id}");

        return Task.CompletedTask;
    }
}
