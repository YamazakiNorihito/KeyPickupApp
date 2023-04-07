namespace KeyPickupApp.Services;

public interface IReceivingSytemService
{
    Task<ReceivingSytemResult> TakeReturnKeyAsync(string barcodeValue);
}

public class ReceivingSytemResult
{
    public ReceivingSytemStatus ReceivingSytemRequestStatus { get; }
    public string Message { get; }

    public ReceivingSytemResult(ReceivingSytemStatus receivingSytemRequestStatus, string message)
        => (ReceivingSytemRequestStatus, Message) = (receivingSytemRequestStatus, message);
}

public enum ReceivingSytemStatus
{
    Success,
    Failure,
    Exception
}
