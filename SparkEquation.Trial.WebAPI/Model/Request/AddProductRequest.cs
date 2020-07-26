using SparkEquation.Trial.WebAPI.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SparkEquation.Trial.WebAPI.Model.Request
{
    public class AddProductRequest
    {
        [Required(ErrorMessage = "Name required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [ExpirationDateValidation]
        public DateTime? ExpirationDate { get; set; }

        public int ItemsInStock { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime? ReceiptDate { get; set; }

        [Required(ErrorMessage = "Rating required")]
        [Range(0, 10, ErrorMessage = "Rating is out of range")]
        public double Rating { get; set; }

        [Range(1, 9, ErrorMessage = "Invalid Brand")]
        public int BrandId { get; set; }

        [CategoryValidation]
        public int[] CategoryIds { get; set; }

        public bool Featured
        {
            get => Rating > 8;
        }
    }
}
