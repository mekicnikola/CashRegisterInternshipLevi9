using AutoMapper;
using CashRegister.Application.Services.Dto;
using CashRegister.Application.Services;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Repositories;
using Moq;

namespace CashRegister.Application.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository>? _productRepositoryMock;
        private Mock<IMapper>? _mapperMock;
        private ProductService? _productService;

        [TestInitialize]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();

            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ReturnsProductDtoList()
        {
            var products = new List<Product> { new() { Id = 1, Name = "Test Product", Price = 100 } };
            var productDtos = new List<ProductDto> { new() { Id = 1, Name = "Test Product", Price = 100 } };

            _productRepositoryMock!.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(products);
            _mapperMock!.Setup(mapper => mapper.Map<List<ProductDto>>(It.IsAny<List<Product>>())).Returns(productDtos);

            var result = await _productService!.GetAllProductsAsync();

            Assert.AreEqual(productDtos.Count, result.Count);
            Assert.AreEqual(productDtos[0].Id, result[0].Id);
            Assert.AreEqual(productDtos[0].Name, result[0].Name);
            Assert.AreEqual(productDtos[0].Price, result[0].Price);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ReturnsProductDto()
        {
            var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
            var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 100 };

            _productRepositoryMock!.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(product);
            _mapperMock!.Setup(mapper => mapper.Map<ProductDto>(It.IsAny<Product>())).Returns(productDto);

            var result = await _productService!.GetProductByIdAsync(1);

            Assert.AreEqual(productDto, result);
        }

        [TestMethod]
        public async Task CreateProductAsync_ReturnsCreatedProductDto()
        {
            var createProductDto = new CreateUpdateProductRequest { Name = "New Product", Price = 200 };
            var createdProduct = new Product { Id = 2, Name = "New Product", Price = 200 };
            var productDto = new ProductDto { Id = 2, Name = "New Product", Price = 200 };

            _mapperMock!.Setup(mapper => mapper.Map<Product>(It.IsAny<CreateUpdateProductRequest>())).Returns(createdProduct);
            _productRepositoryMock!.Setup(repo => repo.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);
            _mapperMock.Setup(mapper => mapper.Map<ProductDto>(It.IsAny<Product>())).Returns(productDto);

            var result = await _productService!.CreateProductAsync(createProductDto);

            Assert.AreEqual(productDto, result);
        }

        [TestMethod]
        public async Task UpdateProductAsync_UpdatesExistingProduct()
        {
            var productId = 1;
            var updateProductDto = new CreateUpdateProductRequest { Name = "Updated Product", Price = 300 };
            var existingProduct = new Product { Id = productId, Name = "Test Product", Price = 100 };

            _productRepositoryMock!.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(repo => repo.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            await _productService!.UpdateProductAsync(productId, updateProductDto);

            _productRepositoryMock.Verify(repo => repo.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [TestMethod]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            var productId = 1;

            _productRepositoryMock!.Setup(repo => repo.DeleteProductAsync(productId)).Returns(Task.CompletedTask);

            await _productService!.DeleteProductAsync(productId);

            _productRepositoryMock.Verify(repo => repo.DeleteProductAsync(productId), Times.Once);
        }
    }
}
