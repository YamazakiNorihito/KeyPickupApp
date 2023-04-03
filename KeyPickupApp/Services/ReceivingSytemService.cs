using System.Net.Http.Json;

namespace KeyPickupApp.Services;

public class ReceivingSytemService : IReceivingSytemService
{
    private readonly HttpClient _httpClient;

    public ReceivingSytemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReceivingSytemResult> SendBarcodeAsync(string secret, string barcodeValue)
    {
        var requestBody = new { secret, barcode_value = barcodeValue };

        var httpResponseMessage = await _httpClient.PutAsJsonAsync("api/v1/receivingsytem/key/", requestBody);

        if(httpResponseMessage.IsSuccessStatusCode)
        {
            return new ReceivingSytemResult(ReceivingSytemRequestStatus.Success, null);
        }

        var responseBody = await httpResponseMessage.Content.ReadFromJsonAsync<ReceivingSytemResponse>();
        return new ReceivingSytemResult(ReceivingSytemRequestStatus.Failure, responseBody.message);
    }

    private class ReceivingSytemResponse
    {
        public string message { get; set; }
    }
}
