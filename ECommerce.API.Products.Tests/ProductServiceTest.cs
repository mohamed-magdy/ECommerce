using ECommerce.API.Products.Db;
using ECommerce.API.Products.Profiles;
using ECommerce.API.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace ECommerce.API.Products.Tests
{
    public class ProductServiceTest
    {
        [Fact]
        public async void GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                            .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                            .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new AutoMapper.Mapper(configuration);
            var productsProvider = new ProductProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync();

            Assert.True(product.IsSucess);
            Assert.True(product.products.Any());
            Assert.Null(product.errorMessage);

        }

        [Fact]
        public async void GetProductReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                            .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingValidId))
                            .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new AutoMapper.Mapper(configuration);
            var productsProvider = new ProductProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(1);

            Assert.True(product.IsSucess);
            Assert.NotNull(product.product);
            Assert.True(product.product.Id == 1);
            Assert.Null(product.errorMessage);

        }

        [Fact]
        public async void GetProductReturnsProductUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                            .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingInvalidId))
                            .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new AutoMapper.Mapper(configuration);
            var productsProvider = new ProductProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(-1);

            Assert.False(product.IsSucess);
            Assert.Null(product.product);
            Assert.NotNull(product.errorMessage);

        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
