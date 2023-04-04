using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace KeyPickupApp.Services;

public class ReceivingSytemService : IReceivingSytemService
{
    private readonly HttpClient _httpClient;
    private readonly string _secret;

    public ReceivingSytemService(HttpClient httpClient, string secret)
    {
        _httpClient = httpClient;
        _secret = secret;
    }

    public async Task<ReceivingSytemResult> TakeReturnKeyAsync(string barcodeValue)
    {
        var requestBody = new { secret = _secret, barcode_value = barcodeValue };
        try
        {
            var httpResponseMessage = await _httpClient.PatchAsync("api/v1/receivingsytem/key/"
                , new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return new ReceivingSytemResult(ReceivingSytemStatus.Success, null);
            }

            var responseBody = await httpResponseMessage.Content.ReadFromJsonAsync<ReceivingSytemResponse>();
            return new ReceivingSytemResult(ReceivingSytemStatus.Failure, responseBody.message);

        }catch(Exception ex)
        {
            return new ReceivingSytemResult(ReceivingSytemStatus.Exception, ex.Message);
        }
    }

    private class ReceivingSytemResponse
    {
        public string message { get; set; }
    }
}
