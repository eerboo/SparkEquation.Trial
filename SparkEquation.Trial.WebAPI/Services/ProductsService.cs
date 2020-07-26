using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SparkEquation.Trial.WebAPI.Data.Factory;
using SparkEquation.Trial.WebAPI.Data.Models;
using SparkEquation.Trial.WebAPI.Model.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkEquation.Trial.WebAPI.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IContextFactory _factory;
        private readonly IMapper _mapper;

        public ProductsService(IContextFactory contextFactory, IMapper mapper)
        {
            _factory = contextFactory;
            _mapper = mapper;
        }

        public List<Product> GetAllProductsData()
        {
            using (var context = _factory.GetContext())
            {
                return context.Products.ToList();
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            using (var context = _factory.GetContext())
            {
                return await context.Products
                    .Include(pr => pr.CategoryProducts).ThenInclude(c => c.Category)
                    .Include(pr => pr.Brand)
                    .FirstOrDefaultAsync(pr => pr.Id == id);
            }
        }

        public async Task<Product> AddProductAsync(AddProductRequest request)
        {
            using (var context = _factory.GetContext())
            {
                Product product = _mapper.Map<Product>(request);

                await context.Products.AddAsync(product);

                if (request.CategoryIds != null)
                {
                    foreach (var id in request.CategoryIds)
                    {
                        CategoryProduct categoryProduct = new CategoryProduct { CategoryId = id, ProductId = product.Id };
                        context.CategoryProducts.Add(categoryProduct);
                    }
                }

                context.SaveChanges();

                return await GetProductByIdAsync(product.Id);
            }
        }

        public async Task<Product> UpdateProductAsync(UpdateProductRequest request)
        {
            using (var context = _factory.GetContext())
            {
                Product product = context.Products.Find(request.Id);

                if (product == null)
                {
                    return null;
                }

                Product productReq = _mapper.Map<Product>(request);
                context.Entry(product).CurrentValues.SetValues(productReq);

                var categoryProducts = context.CategoryProducts.Where(c => c.ProductId == request.Id).ToList();

                context.RemoveRange(categoryProducts);

                foreach (var id in request.CategoryIds)
                {
                    CategoryProduct categoryProduct = new CategoryProduct { CategoryId = id, ProductId = product.Id };
                    context.CategoryProducts.Add(categoryProduct);
                }
                context.SaveChanges();

                return await GetProductByIdAsync(product.Id);
            }
        }

        public async Task<bool> RemoveProductAsync(int id)
        {
            using (var context = _factory.GetContext())
            {
                Product product = context.Products.Find(id);

                if (product == null)
                {
                    return false;
                }

                context.Remove(product);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}