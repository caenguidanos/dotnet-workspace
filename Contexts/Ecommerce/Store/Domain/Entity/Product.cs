namespace Ecommerce.Store.Domain.Entity;

using Common.Domain.Entity;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.ValueObject;

public class Product : TimeStamp
{
    private readonly ProductId _id;
    private readonly ProductTitle _title;
    private readonly ProductDescription _description;
    private readonly ProductStatus _status;
    private readonly ProductPrice _price;

    public Product(ProductId id, ProductTitle title, ProductDescription description, ProductStatus status, ProductPrice price)
    {
        _id = id;
        _title = title;
        _description = description;
        _status = status;
        _price = price;
    }

    public Guid Id
    {
        get
        {
            return _id.GetValue();
        }
    }

    public int Price
    {
        get
        {
            return _price.GetValue();
        }
    }

    public string Title
    {
        get
        {
            return _title.GetValue();
        }
    }

    public string Description
    {
        get
        {
            return _description.GetValue();
        }
    }

    public ProductStatusValue Status
    {
        get
        {
            return _status.GetValue();
        }
    }

    public static Guid NewID()
    {
        return Guid.NewGuid();
    }
}
