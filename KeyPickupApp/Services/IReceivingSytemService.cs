namespace KeyPickupApp.Services;

public interface IReceivingSytemService
{
    Task<ReceivingSytemResult> SendBarcodeAsync(string secret, string barcodeValue);
}

public class ReceivingSytemResult
{
    public ReceivingSytemRequestStatus ReceivingSytemRequestStatus { get; }
    public string Message { get; }

    public ReceivingSytemResult(ReceivingSytemRequestStatus receivingSytemRequestStatus, string message)
        => (ReceivingSytemRequestStatus, Message) = (receivingSytemRequestStatus, message);
}


public enum ReceivingSytemRequestStatus
{
    Success,
    Failure
}
