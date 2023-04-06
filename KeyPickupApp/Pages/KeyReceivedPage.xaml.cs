using CommunityToolkit.Maui.Views;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using KeyPickupApp.Services;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{
    private static int QR_GIRD_MAX_ROW = 100;

    private readonly IReceivingSytemService _receivingSytemService;

    private List<Entry> _qrEntries = new List<Entry>();

    public KeyReceivedPage()
	{
        InitializeComponent();

        //SetEnableDeveloperMode();

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

            int failCount = 0;
            for (int i = 0; i < qrCodes.Count; i++)
            {
                var result = await _receivingSytemService.TakeReturnKeyAsync(qrCodes[i]);

                if (result.ReceivingSytemRequestStatus != ReceivingSytemStatus.Success)
                    failCount++;
            }

            ClearQRValuesInRowButtonClicked(null, null);

            SetQrCodes(qrCodes.Skip(qrCodes.Count - failCount).ToList());

            await this.ShowPopupAsync(new KeyReceivedResultPopup(failCount, (qrCodes.Count - failCount)));
        }
        finally
        {
            activityIndicator.IsRunning = !activityIndicator.IsRunning;
            verticalStackLayout.IsEnabled = !verticalStackLayout.IsEnabled;
        }
    }


    private async void CsvImportClicked(object sender, EventArgs args)
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                    { DevicePlatform.WinUI, new[] { ".csv" } },
            });

        PickOptions options = new()
        {
            PickerTitle = "Please select a csv file",
            FileTypes = customFileType,
        };

        var result = await FilePicker.Default.PickAsync(options);

        if (result is null)
            return;

        using var stream = await result.OpenReadAsync();
        using var reader = new StreamReader(stream);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Read();
        csv.ReadHeader();

        var qrCodes = new List<string>();
        while (csv.Read())
        {
            var record = csv.GetRecord<KeyBuilding>();

            qrCodes.Add(record.BarcodeID);

            if (QR_GIRD_MAX_ROW <= qrCodes.Count)
                break;
        }


        ClearQRValuesInRowButtonClicked(null, null);

        SetQrCodes(qrCodes);
    }
    private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
        DeveloperStackLayout.IsVisible = !DeveloperStackLayout.IsVisible;
        DeveloperStackLayout.IsEnabled = !DeveloperStackLayout.IsEnabled;
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


public class KeyBuilding
{
    [Index(0)]
    public string NumFloors { get; set; }
    [Index(1)]
    public string MgmtNum { get; set; }
    [Index(2)]
    public string MkCode { get; set; }
    [Index(3)]
    public string RoomName { get; set; }
    [Index(4)]
    public string KeyNum { get; set; }
    [Index(5)]
    public string NumKeys { get; set; }
    [Index(6)]
    public string Remarks { get; set; }
    [Index(7)]
    public string Contractor { get; set; }
    [Index(8)]
    public string DoorSymbol { get; set; }
    [Index(9)]
    public string Progress { get; set; }
    [Index(10)]
    public string defects { get; set; }
    [Index(11)]
    public string DefectUpdateDate { get; set; }
    [Index(12)]
    public string Remarks2 { get; set; }
    [Index(13)]
    public string RemarksLastUpdateDate { get; set; }
    [Index(14)]
    public string RemarksLastUpdateBy { get; set; }
    [Index(15)]
    public string CorrectionConfirmationRequest { get; set; }
    [Index(16)]
    public string KeyOrgLastUpdateDate { get; set; }
    [Index(17)]
    public string KeyOrgLastUpdateBy { get; set; }
    [Index(18)]
    public string SubKeyComboLastUpdateDate { get; set; }
    [Index(19)]
    public string SubKeyComboLastUpdateBy { get; set; }
    [Index(20)]
    public string MkComboLastUpdateDate { get; set; }
    [Index(21)]
    public string MkComboLastUpdateBy { get; set; }
    [Index(22)]
    public string BarcodeID { get; set; }
    [Index(23)]
    public string VisualKeyBoxPlacement { get; set; }
    [Index(24)]
    public string LastEditor { get; set; }
    [Index(25)]
    public string KeyComboPreparation { get; set; }
    [Index(26)]
    public string DocURL { get; set; }
    [Index(27)]
    public string IntegrationID { get; set; }
}