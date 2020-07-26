using Microsoft.AspNetCore.Mvc;
using SparkEquation.Trial.WebAPI.Controllers;
using SparkEquation.Trial.WebAPI.Data.Models;
using SparkEquation.Trial.WebAPI.Model;
using SparkEquation.Trial.WebAPI.Model.Mapper;
using SparkEquation.Trial.WebAPI.Model.Request;
using SparkEquation.Trial.WebAPI.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace SparkEquasion.Trial.WebApi.Tests
{
    public class ProductControllerTest
    {
        ProductController _productController;
        IProductsService _productsService;
        public ProductControllerTest()
        {
            _productsService = new ProductsServiceFake();
            _productController = new ProductController(_productsService, DtoProfile.CreateMapper());
        }

        #region Get
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var response = _productController.Get().Result;

            // Assert
            Assert.IsType<JsonResult>(response);
            var okResult = response as JsonResult;
            Assert.NotNull(okResult);
            var items = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _productController.GetById(10);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsOkResult()
        {
            // Act
            var okResult = _productController.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            var id = 1;

            // Act
            var okResult = _productController.GetById(id).Result as OkObjectResult;

            // Assert
            Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(id, (okResult.Value as ProductDto).Id);
        }

        #endregion

        #region Add
        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest1()
        {
            // Arrange
            var request = new AddProductRequest()
            {
                ExpirationDate = DateTime.Now,
                CategoryIds = new int[] { 10 },
            };
            _productController.ModelState.AddModelError("Name", "Name required");
            _productController.ModelState.AddModelError("Rating", "Rating required");
            _productController.ModelState.AddModelError("BrandId", "Invalid Brand");
            _productController.ModelState.AddModelError("CategoryIds", "Invalid category");
            _productController.ModelState.AddModelError("ExpirationDate", "Expiration date should expire not less than 30 days since now");


            // Act
            var badResponse = _productController.Add(request).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
            var response = (badResponse as BadRequestObjectResult).Value as SerializableError;
            Assert.Equal(5, response.Count);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest2()
        {
            // Arrange
            var request = new AddProductRequest()
            {
                ExpirationDate = DateTime.Now.AddDays(30),
                CategoryIds = new int[] { 1, 2, 3 },
                Rating = 11,
                Name = "Test",
                BrandId = 1,

            };
            _productController.ModelState.AddModelError("Rating", "Rating is out of range");


            // Act
            var badResponse = _productController.Add(request).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
            var response = (badResponse as BadRequestObjectResult).Value as SerializableError;
            Assert.Equal(1, response.Count);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var request = new AddProductRequest()
            {
                BrandId = 1,
                Name = "TestName",
                Rating = 7
            };

            // Act
            var createdResponse = _productController.Add(request).Result;

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var request = new AddProductRequest()
            {
                BrandId = 2,
                Name = "Guinness Original Pack",
                Rating = 7.5
            };

            // Act
            var createdResponse = _productController.Add(request).Result as CreatedAtActionResult;
            var item = createdResponse.Value as ProductDto;

            // Assert
            Assert.IsType<ProductDto>(item);
            Assert.Equal("Guinness Original Pack", item.Name);
            Assert.Equal(2, item.BrandId);
            Assert.Equal(7.5, item.Rating);
            Assert.False(item.Featured);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItemFeatured()
        {
            // Arrange
            var request = new AddProductRequest()
            {
                BrandId = 1,
                Name = "IPhone 11 XL",
                Rating = 8.1
            };

            // Act
            var createdResponse = _productController.Add(request).Result as CreatedAtActionResult;
            var item = createdResponse.Value as ProductDto;

            // Assert
            Assert.IsType<ProductDto>(item);
            Assert.Equal("IPhone 11 XL", item.Name);
            Assert.Equal(1, item.BrandId);
            Assert.Equal(8.1, item.Rating);
            Assert.True(item.Featured);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_InvalidObjectPassed_ReturnsBadRequest1()
        {
            // Arrange
            var request = new UpdateProductRequest()
            {
                Id = 0,
                ExpirationDate = DateTime.Now,
                CategoryIds = new int[] { 10 },
            };
            _productController.ModelState.AddModelError("Id", "Id required");
            _productController.ModelState.AddModelError("Name", "Name required");
            _productController.ModelState.AddModelError("Rating", "Rating required");
            _productController.ModelState.AddModelError("BrandId", "Invalid Brand");
            _productController.ModelState.AddModelError("CategoryIds", "Invalid category");
            _productController.ModelState.AddModelError("ExpirationDate", "Expiration date should expire not less than 30 days since now");


            // Act
            var badResponse = _productController.Update(request).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
            var response = (badResponse as BadRequestObjectResult).Value as SerializableError;
            Assert.Equal(6, response.Count);
        }

        [Fact]
        public void Update_InvalidObjectPassed_ReturnsBadRequest2()
        {
            // Arrange
            var request = new UpdateProductRequest()
            {
                Id = 1,
                ExpirationDate = DateTime.Now.AddDays(30),
                CategoryIds = new int[] { 1, 2, 3 },
                Rating = 11,
                Name = "Test",
                BrandId = 1,

            };
            _productController.ModelState.AddModelError("Rating", "Rating is out of range");


            // Act
            var badResponse = _productController.Update(request).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
            var response = (badResponse as BadRequestObjectResult).Value as SerializableError;
            Assert.Equal(1, response.Count);
        }

        [Fact]
        public void Update_ValidObjectPassed__ReturnsNotFoundResult()
        {
            // Arrange
            var request = new UpdateProductRequest()
            {
                Id = 100,
                BrandId = 4,
                Name = "TestName",
                Rating = 7
            };

            // Act
            var notFoundResult = _productController.Update(request).Result;

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }


        [Fact]
        public void Update_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var request = new UpdateProductRequest()
            {
                Id = 1,
                BrandId = 1,
                Name = "TestName",
                Rating = 7
            };

            // Act
            var response = _productController.Update(request).Result;

            // Assert
            Assert.IsType<OkObjectResult>(response);
        }


        [Fact]
        public void Update_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var request = new UpdateProductRequest()
            {
                Id = 3,
                BrandId = 2,
                Name = "Xiaomi",
                Rating = 7.5
            };

            // Act
            var response = _productController.Update(request).Result as OkObjectResult;
            var item = response.Value as ProductDto;

            // Assert
            Assert.IsType<ProductDto>(item);
            Assert.Equal(3, item.Id);
            Assert.Equal("Xiaomi", item.Name);
            Assert.Equal(2, item.BrandId);
            Assert.Equal(7.5, item.Rating);
            Assert.False(item.Featured);
        }

        [Fact]
        public void Update_ValidObjectPassed_ReturnedResponseHasCreatedItemFeatured()
        {
            // Arrange
            var request = new UpdateProductRequest()
            {
                Id = 2,
                BrandId = 1,
                Name = "IPhone 11 XL",
                Rating = 8.1
            };

            // Act
            var response = _productController.Update(request).Result as OkObjectResult;
            var item = response.Value as ProductDto;

            // Assert
            Assert.IsType<ProductDto>(item);
            Assert.Equal(2, item.Id);
            Assert.Equal("IPhone 11 XL", item.Name);
            Assert.Equal(1, item.BrandId);
            Assert.Equal(8.1, item.Rating);
            Assert.True(item.Featured);
        }
        #endregion

        #region Delete

        [Fact]
        public void Remove_NotExistingIddPassed_ReturnsNotFoundResponse()
        {
            // Act
            var badResponse = _productController.Remove(30).Result;

            // Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public void Remove_ExistingIddPassed_ReturnsOkResult()
        {
            // Act
            var okResponse = _productController.Remove(1).Result;

            // Assert
            Assert.IsType<OkResult>(okResponse);
        }
        [Fact]
        public void Remove_ExistingIdPassed_RemovesOneItem()
        {
            // Act
            var okResponse = _productController.Remove(1).Result;

            // Assert
            var response = _productController.Get().Result as JsonResult;
            var items = response.Value as List<Product>;
            Assert.Equal(2, items.Count);
        }
        #endregion
    }
}
