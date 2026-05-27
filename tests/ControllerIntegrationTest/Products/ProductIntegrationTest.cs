using ControllerIntegrationTest.Helper;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Contract.Products;
using Napoleon.Shared.Contract.ApiResponse;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ControllerIntegrationTest.Products;

public class ProductIntegrationTest(CustomWebApplicationFactory<Program> factory) : BaseIntegrationTest(factory)
{

    [Fact]
    public async Task GetAllProduct_Should_ReturnSuccess()
    {
        // Arrange
        var token = await SetUpTokenAsync();

        // Act
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/v1/products");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var httpClient = _appFactory.CreateClient();

        var response = await httpClient.SendAsync(requestMessage);
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<List<ProductDto>>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotNull(actual.Data);
        Assert.NotEmpty(actual.Data);
    }
    
    [Fact]
    public async Task GetAllProduct_Should_ReturnSuccess_WithFiltering()
    {
        // Arrange
        var token = await SetUpTokenAsync();
        var filter = ProductList.First().Name.Substring(0,2);

        // Act
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/v1/products?name={filter}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var httpClient = _appFactory.CreateClient();

        var response = await httpClient.SendAsync(requestMessage);
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<List<ProductDto>>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotNull(actual.Data);
        Assert.NotEmpty(actual.Data);
    }
    
    [Fact]
    public async Task AddSingleAsync_Should_ReturnSuccess()
    {
        // Arrange
        var token = await SetUpTokenAsync();
        var productDto = new ProductDto(null, "string 11", "decript 1", "image URL 1");

        // Act
        var httpClient = _appFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.PostAsJsonAsync("api/v1/products", productDto);
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<ProductDto>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotNull(actual.Data);
        Assert.Equal(productDto.Name, actual.Data.Name);
    }
    
    [Fact]
    public async Task UpdateSingleAsync_Should_ReturnSuccess()
    {
        // Arrange
        var token = await SetUpTokenAsync();
        var product= ProductList.First();
        product.Name = "testing 1111";

        var productDto = product.ToDto();

        // Act
        var httpClient = _appFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.PutAsJsonAsync("api/v1/products", productDto);
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<ProductDto>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotNull(actual.Data);
        Assert.Equal(productDto.Name, actual.Data.Name);
    }
    
    [Fact]
    public async Task DeleteSingleAsync_Should_ReturnSuccess()
    {
        // Arrange
        var token = await SetUpTokenAsync();
        var productId = ProductList.First().Id;

        // Act
        var httpClient = _appFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.DeleteAsync($"api/v1/products?productId={productId}");
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<bool>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.True(actual.Data);
    }


    private async ValueTask<string> SetUpTokenAsync()
    {
        // Arrange
        var customer = CustomerList.FirstOrDefault();
        var registerModel = new CustomerAuthDto(customer.Email, "12345678");
        await PrepareDataAsync();

        var loginResponse = await _appFactory.CreateClient().PostAsJsonAsync("api/v1/auth/login", registerModel);
        var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
        var loginData = JsonConvert.DeserializeObject<ApiResult<string>>(loginResponseContent);

        return loginData.Data;
    }
}
