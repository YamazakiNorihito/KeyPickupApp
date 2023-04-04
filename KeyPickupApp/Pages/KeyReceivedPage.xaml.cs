using CommunityToolkit.Maui.Views;
using KeyPickupApp.Services;
using Microsoft.Extensions.Configuration;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{
    private static int QR_GIRD_MAX_ROW = 100;

    private readonly IReceivingSytemService _receivingSytemService;

    private List<Entry> _qrEntries = new List<Entry>();

    public KeyReceivedPage()
	{
        InitializeComponent();

        _receivingSytemService = Services.ServiceProvider.GetService<IReceivingSytemService>();

        // Styleでいい感じにやる方法がわからなかったのでコードで頑張った
        var heightStyle = GetRowDefinitionStyle();
        var rowDefinition_Height = new GridLength(heightStyle is null ? 40 : Convert.ToInt32(heightStyle.Value));

        for (var i = 0; i < QR_GIRD_MAX_ROW; i++)
        {
            qrGrid.RowDefinitions.Add(new RowDefinition { Height = rowDefinition_Height });

            var label = new Label { Text = "QRコード", Style = (Style)Resources["LabelFontStyle"] };
            var entry = new Entry();

            qrGrid.Add(label, 0, qrGrid.RowDefinitions.Count - 1);
            qrGrid.Add(entry, 1, qrGrid.RowDefinitions.Count - 1);

            _qrEntries.Add(entry);
        }
    }

    private void ClearQRValuesInRowButtonClicked(object sender, EventArgs args)
    {
        foreach(var qrEntry in _qrEntries)
            qrEntry.Text = string.Empty;
    }

    private async void SendQRValuesInRowButtonClicked(object sender, EventArgs args)
    {
        try
        {
            verticalStackLayout.IsEnabled = false;
            activityIndicator.IsRunning = true;

            var qrCodes = GetQrCodes();

            var failQrCodes = new List<string>();
            foreach (var qrCode in qrCodes)
            {
                var result = await _receivingSytemService.TakeReturnKeyAsync(qrCode);

                if (result.ReceivingSytemRequestStatus != ReceivingSytemStatus.Success)
                    failQrCodes.Add(qrCode);
            }

            ClearQRValuesInRowButtonClicked(null, null);

            SetQrCodes(failQrCodes);

            await this.ShowPopupAsync(new KeyReceivedResultPopup(failQrCodes.Count, (qrCodes.Count - failQrCodes.Count)));

        }finally
        {
            activityIndicator.IsRunning = !activityIndicator.IsRunning;
            verticalStackLayout.IsEnabled = !verticalStackLayout.IsEnabled;
        }
    }

    private List<string> GetQrCodes() => _qrEntries.Where(o => !string.IsNullOrWhiteSpace(o.Text)).Select(o => o.Text).Distinct().ToList();

    private void SetQrCodes(List<string> qrCodes)
    {
        for (var i = 0; i < qrCodes.Count; i++)
        {
            _qrEntries[i].Text = qrCodes[i];
        }
    }

    private Setter GetRowDefinitionStyle()
    {
        var rowDefinitionStyle = (Style)Resources["RowDefinitionStyle"];
        return rowDefinitionStyle.Setters.Where(o => o.Property.Equals(nameof(RowDefinition.Height))).SingleOrDefault();
    }
}