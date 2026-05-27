using Azure;
using ControllerIntegrationTest.Helper;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Shared.Contract.ApiResponse;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ControllerIntegrationTest.Auth;

public class AuthIntegrationTest(CustomWebApplicationFactory<Program> factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task RegisterCustomer_Success()
    {
        var registerModel = new CustomerAuthDto("testtest@gmail.com", "12345678");
        var url = "api/v1/auth/register";

        var response = await _appFactory.CreateClient().PostAsJsonAsync(url, registerModel);

        var responseContent = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<bool>>(responseContent);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.True(actual.Data);
    }

    [Fact]
    public async Task Login_Success()
    {
        var customer = CustomerList.FirstOrDefault();
        var registerModel = new CustomerAuthDto(customer.Email, "12345678");
        var url = "api/v1/auth/login";
        await PrepareDataAsync();

        var response = await _appFactory.CreateClient().PostAsJsonAsync(url, registerModel);

        var responseContent = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<string>>(responseContent);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotEmpty(actual.Data);
    }
    
    [Fact]
    public async Task Logout_Success()
    {
        var customer = CustomerList.FirstOrDefault();
        var registerModel = new CustomerAuthDto(customer.Email, "12345678");
        await PrepareDataAsync();

        var loginResponse = await _appFactory.CreateClient().PostAsJsonAsync("api/v1/auth/login", registerModel);
        var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
        var loginData = JsonConvert.DeserializeObject<ApiResult<string>>(loginResponseContent);

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "api/v1/auth/logout");
        var httpClient = _appFactory.CreateClient();
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Data);
        var response = await httpClient.SendAsync(requestMessage);
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<string>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotEmpty(actual.Data);
    }
}
