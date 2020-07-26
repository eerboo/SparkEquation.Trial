using System.Collections.Generic;
using System.Threading.Tasks;
using SparkEquation.Trial.WebAPI.Data.Models;
using SparkEquation.Trial.WebAPI.Model.Request;

namespace SparkEquation.Trial.WebAPI.Services
{
    public interface IProductsService
    {
        List<Product> GetAllProductsData();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(AddProductRequest request);
        Task<Product> UpdateProductAsync(UpdateProductRequest request);
        Task<bool> RemoveProductAsync(int id);
    }
}