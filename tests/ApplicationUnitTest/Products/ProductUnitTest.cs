using ApplicationUnitTest.Helper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Moq;
using Napoleon.Fos.Application.Customers.Auth.Commands;
using Napoleon.Fos.Application.Products.Commands;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Application.Products.Queries;
using Napoleon.Fos.Contract.ProductDetails;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Common;

namespace ApplicationUnitTest.Products;

public class ProductUnitTest
{
    private readonly Mock<IAppUnitOfWork> _appUnitOfWork = new();

    [Fact]
    public async Task AddSingleProductCommand_Return_Success()
    {
        // Arrange
        var products = GetSeedProductData(1);
        var expected = products.FirstOrDefault()!;
        _appUnitOfWork.Setup(x => x.ProductRepository.AddAndSaveAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var handler = new AddSingleProductCommandHandler(_appUnitOfWork.Object);
        var actual = await handler.Handle(new AddSingleProductCommand(expected.ToDto()), CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
    }

    [Fact]
    public async Task DeleteSingleProductCommand_Return_Success()
    {
        // Arrange
        _appUnitOfWork.Setup(x => x.ProductRepository.GetSingleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(GetSeedProductData(1).FirstOrDefault());
        _appUnitOfWork.Setup(x => x.ProductRepository.SoftDeleteByIdAndSaveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var handler = new DeleteSingleProductCommandHandler(_appUnitOfWork.Object);
        var actual = await handler.Handle(new DeleteSingleProductCommand(Ulid.NewUlid().ToGuid()), CancellationToken.None);
        // Assert

        Assert.True(actual);
    }
    
    [Fact]
    public async Task DeleteSingleProductCommand_Throw_Exception()
    {
        // Arrange
        _appUnitOfWork.Setup(x => x.ProductRepository.GetSingleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Product);
        _appUnitOfWork.Setup(x => x.ProductRepository.SoftDeleteByIdAndSaveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var handler = new DeleteSingleProductCommandHandler(_appUnitOfWork.Object);
        var actual = async () => await handler.Handle(new DeleteSingleProductCommand(Ulid.NewUlid().ToGuid()), CancellationToken.None);
        // Assert

        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }
    

    [Fact]
    public async Task UpdateSingleProductCommand_ThrowException_If_IdIsNull()
    {
        // Arrange
        // Act
        var handler = new UpdateSingleProductCommandHandler(_appUnitOfWork.Object);
        var productId = Ulid.NewUlid().ToGuid();
        var actual = async () => await handler.Handle(new UpdateSingleProductCommand(new ProductDto(productId, "name", "description", "imageURL", [new ProductDetailDto(null, "1", "2", "3", 5, productId, 5)])), CancellationToken.None);
        // Assert

        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact]
    public async Task UpdateSingleProductCommand_ThrowException_If_EntityIsNotFound()
    {
        // Arrange
        _appUnitOfWork.Setup(x => x.ProductRepository.GetSingleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Product);
        _appUnitOfWork.Setup(x => x.ProductRepository.SoftDeleteByIdAndSaveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var handler = new UpdateSingleProductCommandHandler(_appUnitOfWork.Object);
        var productId = Ulid.NewUlid().ToGuid();
        var actual = async () => await handler.Handle(new UpdateSingleProductCommand(new ProductDto(productId, "name", "description", "imageURL", [new ProductDetailDto(Ulid.NewUlid().ToGuid(), "1", "2", "3", 5, productId, 5)])), CancellationToken.None);
        // Assert

        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact]
    public async Task UpdateSingleProductCommand_Return_Success()
    {
        // Arrange
        var dbEntity = GetSeedProductData(1).FirstOrDefault();

        var expected = dbEntity.Clone();
        expected!.Description = "12345678";
        var newDto = expected.ToDto();


        _appUnitOfWork.Setup(x => x.ProductRepository.GetSingleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(dbEntity);
        _appUnitOfWork.Setup(x => x.ProductRepository.SoftDeleteByIdAndSaveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var handler = new UpdateSingleProductCommandHandler(_appUnitOfWork.Object);
        var actual = await handler.Handle(new UpdateSingleProductCommand(newDto), CancellationToken.None);
        
        // Assert
        Assert.Equal(actual.Id, expected.Id);
        Assert.Equal(actual.Description, expected.Description);
    }
    
    [Fact]
    public async Task GetAllProductQuery_Return_Success()
    {
        // Arrange
        var dbEntity = GetSeedProductData(5);
        var queryable = dbEntity.AsAsyncQueryable();
        var expected = dbEntity.Select(x => x.ToDto()).ToList();

        _appUnitOfWork.Setup(x => x.ProductRepository.GetQueryable(It.IsAny<QueryTrackingBehavior>())).Returns(queryable);
        _appUnitOfWork.Setup(x => x.ProductRepository.GetManyTypeAsync(It.IsAny<IQueryable<ProductDto>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var handler = new GetAllProductQueryHandler(_appUnitOfWork.Object);
        var actual = await handler.Handle(new GetAllProductQuery(null), CancellationToken.None);
        
        // Assert
        Assert.Equal(expected.Count, actual.Count);
    }

    private static IList<Product> GetSeedProductData(int numberOfEntity)
    {
        var faker = new Faker();
        var ProductFaker = new Faker<Product>()
               .RuleFor(x => x.IsDeleted, f => false)
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl());
        var Products = ProductFaker.Generate(numberOfEntity);

        return Products;
    }
}
