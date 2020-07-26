using System;
using System.ComponentModel.DataAnnotations;

namespace SparkEquation.Trial.WebAPI.Model.Request
{
    public class UpdateProductRequest : AddProductRequest
    {
        [Required(ErrorMessage = "Id required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid product id")]
        public int Id { get; set; }
    }
}
