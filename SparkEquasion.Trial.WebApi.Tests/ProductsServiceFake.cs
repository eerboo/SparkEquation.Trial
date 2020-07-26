using SparkEquation.Trial.WebAPI.Data.Models;
using SparkEquation.Trial.WebAPI.Model;
using SparkEquation.Trial.WebAPI.Model.Request;
using SparkEquation.Trial.WebAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkEquasion.Trial.WebApi.Tests
{
    class ProductsServiceFake : IProductsService
    {
        private readonly List<Product> _products;
        private readonly List<ProductDto> _productsDto;

        public ProductsServiceFake()
        {
            _products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    BrandId = 1,
                    Featured = false,
                    Rating = 5,
                    Name = "La Vieille Ferme Rouge",
                    ItemsInStock = 0
                },
                new Product()
                {
                    Id = 2,
                    BrandId = 2,
                    Featured = false,
                    Rating = 4.9,
                    Name = "J.P. Chenet Pays D'Oc Cabernet - Syrah",
                    ItemsInStock = 0
                },
                new Product()
                {
                    Id = 3,
                    BrandId = 5,
                    Featured = true,
                    Rating = 8.1,
                    Name = "Essentuki 4",
                    ItemsInStock = 0
                },
            };
        }
        public async Task<Product> AddProductAsync(AddProductRequest request)
        {
            var id = _products.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            var newProduct = new Product()
            {
                Id = id,
                BrandId = request.BrandId,
                Featured = request.Featured,
                Rating = request.Rating,
                Name = request.Name,
                ItemsInStock = request.ItemsInStock
            };
            _products.Add(newProduct);
            return newProduct;
        }

        public List<Product> GetAllProductsData()
        {
            return _products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> RemoveProductAsync(int id)
        {
            var existing = _products.FirstOrDefault(x => x.Id == id);
            if (existing == null) return false;
            _products.Remove(existing);
            return true;
        }

        public async Task<Product> UpdateProductAsync(UpdateProductRequest request)
        {
            var product = _products.FirstOrDefault(x => x.Id == request.Id);

            if (product == null) return null;

            product.BrandId = request.BrandId;
            product.Featured = request.Featured;
            product.Rating = request.Rating;
            product.Name = request.Name;
            product.ItemsInStock = request.ItemsInStock;

            return product;

        }
    }
}
