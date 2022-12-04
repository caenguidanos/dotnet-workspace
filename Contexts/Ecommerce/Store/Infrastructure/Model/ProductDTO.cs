// <copyright file="ProductDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Infrastructure.Model;

public class CreateProductRequestBodyDTO
{
    required public int Price { get; set; }

    required public string Title { get; set; }

    required public string Description { get; set; }

    required public int Status { get; set; }
}
