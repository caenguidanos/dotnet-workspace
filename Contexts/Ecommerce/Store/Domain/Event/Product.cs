// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Domain.Event;

public class ProductCreatedEvent : INotification
{
    required public Guid Id { get; set; }
}

public class ProductRemovedEvent : INotification
{
    required public Guid Id { get; set; }
}
