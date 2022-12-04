using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Domain.Repository;
using api.Contexts.Ecommerce.Store.Domain.ValueObject;
using api.Contexts.Ecommerce.Store.Infrastructure.Model;
using api.Contexts.Ecommerce.Store.Infrastructure.Persistence;
using FaunaDB.Client;
using FaunaDB.Types;

using static FaunaDB.Query.Language;

namespace api.Contexts.Ecommerce.Store.Infrastructure.Repository
{

    public class ProductRepository : IProductRepository
    {
        private readonly FaunaClient _db;

        public ProductRepository(DatabaseClient database)
        {
            _db = database.client;
        }

        public async Task<string> GenerateID(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new HostAbortedException();
            }

            var result = await _db.Query(NewId(), TimeSpan.FromMilliseconds(500));

            IResult<string> id = result.To<string>();

            return id.Value;
        }

        public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new HostAbortedException();
            }

            var result = await _db.Query(
                Map(
                    Paginate(
                        Match(
                            Index("product_sort_by_ts_desc")
                        )
                    ),
                    Lambda(
                        Arr("ts", "ref"),
                        Merge(
                            Obj("id", Select(Arr("id"), Var("ref"))),
                            Select(Arr("data"), Get(Var("ref")))
                        )
                    )
                ),
                TimeSpan.FromMilliseconds(500)
            );

            IEnumerable<GetProductQueryBindModel> products = Decoder.Decode<IEnumerable<GetProductQueryBindModel>>(result.At("data"));

            Func<GetProductQueryBindModel, Product> productsSelector = product =>
            {
                return new Product(
                    new ProductId(product.Id),
                    new ProductTitle(product.Title),
                    new ProductDescription(product.Description),
                    new ProductStatus((ProductStatusValue)product.Status),
                    new ProductPrice(product.Price)
                );
            };

            return products.Select(productsSelector);
        }


        public async Task<Product> GetById(string id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new HostAbortedException();
            }

            try
            {
                var result = await _db.Query(
                    Merge(
                        Obj("id", id),
                        Get(Ref(Collection("Product"), id))
                    ),
                    TimeSpan.FromMilliseconds(500)
                );

                GetProductQueryBindModel product = Decoder.Decode<GetProductQueryBindModel>(result.At("data"));

                return new Product(
                    new ProductId(product.Id),
                    new ProductTitle(product.Title),
                    new ProductDescription(product.Description),
                    new ProductStatus((ProductStatusValue)product.Status),
                    new ProductPrice(product.Price)
                );
            }
            catch (System.Exception exception)
            {
                throw new ProductNotFoundException(exception.ToString());
            }

        }

        public async Task Save(Product product, CancellationToken cancellationToken)
        {
            var bind = new SaveProductQueryBindModel(
                title: product.Title,
                description: product.Description,
                price: product.Price,
                status: (int)product.Status
            );

            if (cancellationToken.IsCancellationRequested)
            {
                throw new HostAbortedException();
            }

            await _db.Query(
                Create(
                    Ref(Collection("Product"), product.Id),
                    Obj("data", Encoder.Encode(bind))
                ),
                TimeSpan.FromMilliseconds(500)
            );

        }

        public async Task DeleteById(string id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new HostAbortedException();
            }

            await _db.Query(
                Delete(
                    Ref(Collection("Product"), id)
                ),
                TimeSpan.FromMilliseconds(500)
            );
        }
    }
}
