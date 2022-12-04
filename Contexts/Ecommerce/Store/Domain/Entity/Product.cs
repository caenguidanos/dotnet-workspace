// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Domain.Entity;

using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.ValueObject;

public class Product
{
    private readonly ProductId id;
    private readonly ProductTitle title;
    private readonly ProductDescription description;
    private readonly ProductStatus status;
    private readonly ProductPrice price;

    public Product(
        ProductId id,
        ProductTitle title,
        ProductDescription description,
        ProductStatus status,
        ProductPrice price)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.status = status;
        this.price = price;
    }

    public Guid Id
    {
        get { return this.id.GetValue(); }
    }

    public int Price
    {
        get { return this.price.GetValue(); }
    }

    public string Title
    {
        get { return this.title.GetValue(); }
    }

    public string Description
    {
        get { return this.description.GetValue(); }
    }

    public ProductStatusValue Status
    {
        get { return this.status.GetValue(); }
    }

    public static Guid NewID()
    {
        return Guid.NewGuid();
    }
}
