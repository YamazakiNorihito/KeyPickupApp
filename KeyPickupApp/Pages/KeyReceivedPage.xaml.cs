using CommunityToolkit.Maui.Views;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{
    private static int QR_GIRD_MAX_ROW = 100;
    private List<Entry> _qrEntries = new List<Entry>();

    public KeyReceivedPage()
	{
		InitializeComponent();

        // Styleでいい感じにやる方法がわからなかったのでコードで頑張った
        var heightStyle = GetRowDefinitionStyle();
        var rowDefinition_Height = new GridLength(heightStyle is null ? 40 : Convert.ToInt32(heightStyle.Value));

        for (var i = 0; i < QR_GIRD_MAX_ROW; i++)
        {
            qrGrid.RowDefinitions.Add(new RowDefinition { Height = rowDefinition_Height });

            var label = new Label { Text = "QRコード", Style = (Style)Resources["LabelFontStyle"] };
            var entry = new Entry { Text = $"{qrGrid.RowDefinitions.Count}行2列目" };

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

            var qrCodes = _qrEntries.Select(o => o.Text).Distinct();
            var failQrCode = qrCodes.ToList();
            ClearQRValuesInRowButtonClicked(null, null);
            
            for(var i = 0; i < failQrCode.Count;i++)
            {
                _qrEntries[i].Text = failQrCode[i];
            }
            await this.ShowPopupAsync(new KeyReceivedResultPopup(10, 20));
        }finally
        {
            activityIndicator.IsRunning = !activityIndicator.IsRunning;
            verticalStackLayout.IsEnabled = !verticalStackLayout.IsEnabled;
        }
    }

    private Setter GetRowDefinitionStyle()
    {
        var rowDefinitionStyle = (Style)Resources["RowDefinitionStyle"];
        var height = rowDefinitionStyle.Setters.Where(o => o.Property.Equals(nameof(RowDefinition.Height))).SingleOrDefault();
        return height;
    }
}