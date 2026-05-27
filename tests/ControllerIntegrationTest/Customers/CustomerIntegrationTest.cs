using ControllerIntegrationTest.Helper;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Shared.Contract.ApiResponse;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ControllerIntegrationTest.Customers;

public class CustomerIntegrationTest(CustomWebApplicationFactory<Program> factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetPersonalInformation_Success()
    {
        // Arrange
        var customer = CustomerList.FirstOrDefault();
        var registerModel = new CustomerAuthDto(customer.Email, "12345678");
        var url = "api/v1/customers/personal";
        await PrepareDataAsync();

        var loginResponse = await _appFactory.CreateClient().PostAsJsonAsync("api/v1/auth/login", registerModel);
        var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
        var loginData = JsonConvert.DeserializeObject<ApiResult<string>>(loginResponseContent);

        // Act
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Data);
        var httpClient = _appFactory.CreateClient();

        var response = await httpClient.SendAsync(requestMessage);
        var actualData = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<ApiResult<CustomerDto>>(actualData);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotNull(actual.Data);
    }
}
