// <copyright file="GetAll.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Query;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;

public class GetAllQuery : IRequest<IEnumerable<Product>>
{
}

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<Product>>
{
    private readonly IProductRepository productRepository;

    public GetAllQueryHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetAll(cancellationToken);

        return products;
    }
}
