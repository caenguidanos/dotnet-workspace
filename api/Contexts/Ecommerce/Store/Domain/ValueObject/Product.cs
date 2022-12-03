using api.Contexts.Shared;
using api.Contexts.Ecommerce.Store.Domain.Model;

namespace api.Contexts.Ecommerce.Store.Domain.ValueObject
{
    public class ProductId : ValueObject<string>
    {
        public ProductId(string value) : base(value) { }

        public override string validate(string value)
        {
            try
            {
                Guid.Parse(value);
            }
            catch (System.Exception)
            {
                throw new ProductIdInvalidException();
            }

            return value;
        }
    }

    public class ProductPrice : ValueObject<int>
    {
        public ProductPrice(int value) : base(value) { }

        public override int validate(int value)
        {
            int MIN = 100;
            int MAX = 100_000;

            if (value < MIN || value > MAX)
            {
                throw new ProductPriceInvalidException(value.ToString());
            }

            return value;
        }
    }

    public class ProductDescription : ValueObject<string>
    {
        public ProductDescription(string value) : base(value) { }

        public override string validate(string value)
        {
            int MIN_LENGTH = 5;
            int MAX_LENGTH = 600;

            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            {
                throw new ProductDescriptionInvalidException(value);
            }

            return value;
        }
    }

    public class ProductStatus : ValueObject<ProductStatusValue>
    {
        public ProductStatus(ProductStatusValue value) : base(value) { }

        public override ProductStatusValue validate(ProductStatusValue value)
        {
            if (!Enum.IsDefined<ProductStatusValue>(value))
            {
                throw new ProductStatusInvalidException(value.ToString());
            }

            return value;
        }
    }

    public class ProductTitle : ValueObject<string>
    {
        public ProductTitle(string value) : base(value) { }

        public override string validate(string value)
        {
            int MIN_LENGTH = 5;
            int MAX_LENGTH = 256;

            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            {
                throw new ProductTitleInvalidException(value);
            }

            return value;
        }
    }
}

