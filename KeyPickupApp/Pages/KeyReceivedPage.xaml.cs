using CommunityToolkit.Maui.Views;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{
    private static int QR_GIRD_MAX_ROW = 100;

    private readonly GridLength RowDefinition_Height;
    private List<Entry> _qrEntries = new List<Entry>();

    public KeyReceivedPage()
	{
		InitializeComponent();

        // Style�ł��������ɂ����@���킩��Ȃ������̂ŃR�[�h�Ŋ撣����
        var rowDefinitionStyle = (Style)Resources["RowDefinitionStyle"];
        var height = rowDefinitionStyle.Setters.Where(o => o.Property.Equals(nameof(RowDefinition.Height))).SingleOrDefault();
        RowDefinition_Height = new GridLength(height is null ? 40 : Convert.ToInt32(height.Value));

        for (var i = 0; i < QR_GIRD_MAX_ROW; i++)
        {
            qrGrid.RowDefinitions.Add(new RowDefinition { Height = RowDefinition_Height });

            var label = new Label { Text = "QR�R�[�h", Style = (Style)Resources["LabelFontStyle"] };
            var entry = new Entry { Text = $"{qrGrid.RowDefinitions.Count}�s2���" };

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

            var qrCodes = _qrEntries.Select(o => o.Text);

            await Task.Delay(5000);
            await this.ShowPopupAsync(new KeyReceivedResultPopup(10, 20));
        }finally
        {
            activityIndicator.IsRunning = !activityIndicator.IsRunning;
            verticalStackLayout.IsEnabled = !verticalStackLayout.IsEnabled;
        }
    }
}