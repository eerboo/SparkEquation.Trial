using System;

namespace SparkEquation.Trial.WebAPI.Model
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Featured { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int ItemsInStock { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public double Rating { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public CategoryDto[] Categories { get; set; }
    }
}
