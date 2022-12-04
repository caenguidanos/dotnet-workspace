// <copyright file="ProductRemoved.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Event;

using Ecommerce.Store.Domain.Event;
using Ecommerce.Store.Domain.Service;

public class ProductRemovedEventHandler : INotificationHandler<ProductRemovedEvent>
{
    private readonly IProductService productService;

    public ProductRemovedEventHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public Task Handle(ProductRemovedEvent notification, CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"Product removed with id={notification.Id}");

        return Task.CompletedTask;
    }
}
