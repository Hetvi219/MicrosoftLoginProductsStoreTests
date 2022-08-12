using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicrosoftLoginProductsStore.Controllers;
using MicrosoftLoginProductsStore.Data;
using MicrosoftLoginProductsStore.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftLoginProductsStore.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private ApplicationDbContext _dbContext;
       [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase("ProductStoreDb")
                   .Options;

            _dbContext = new ApplicationDbContext(options);
            //_dbContext.Database.Migrate();
            _dbContext.Products.AddRange(new[]{
                new Product
            {
                Id = 1,
                Name = "Product 1",
                Price = "10",
                Description = "desc 1"
            },
                new Product
            {
                Id = 2,
                Name = "Product 2",
                Price = "11",
                Description = "desc 2"
            },new Product
            {
                Id = 3,
                Name = "Product 3",
                Price = "12",
                Description = "desc 3"
            }
            });
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task Edit_WithNoId_ShouldReturnNotFound()
        {
            var controller = new ProductsController(_dbContext);
            var result = await controller.Edit(null);
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task Edit_WithInvalidId_ShouldReturnNotFound()
        {
            var controller = new ProductsController(_dbContext);
            var result = await controller.Edit(5);
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task Edit_WithValidId_ShouldReturnViewSuccessfully()
        {
            var controller = new ProductsController(_dbContext);
            var result = (await controller.Edit(2)) as ViewResult;
            var product = result.ViewData.Model as Product;
            Assert.AreEqual(product.Id, 2);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
