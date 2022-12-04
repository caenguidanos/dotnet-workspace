// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Infrastructure.Model;

public class ProductPrimitives
{
    required public Guid Id { get; set; }

    required public string Title { get; set; }

    required public string Description { get; set; }

    required public int Price { get; set; }

    required public int Status { get; set; }
}
