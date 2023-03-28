using Microsoft.Maui.Controls;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{

    public static int QR_GIRD_MAX_ROW = 100;


	public KeyReceivedPage()
	{
		InitializeComponent();


        for (var i = 0; i < QR_GIRD_MAX_ROW; i++)
        {
            AddQrGridRow(qrGrid);
        }
    }

    private void AddRowButton_Clicked(object sender, EventArgs e)
    {
        AddQrGridRow(qrGrid);
    }

    private void ClearQRValuesInRowButtonClicked(object sender, EventArgs args)
    {
        // もしクリアに時間がかかるようであれば、Entryをキャッシュするしかない
        var qrEntries = qrGrid.Children
            .Where(o => o.GetType().Equals(typeof(Entry)))
            .Select(q => (Entry)q);

        foreach(var qrEntry in qrEntries)
        {
            qrEntry.Text = string.Empty;
        }

    }

    private void AddQrGridRow(Grid qrGrid)
    {
        // 新しい行を作成
        var row = new RowDefinition { Height = new GridLength(50) };

        // Gridに行を追加
        qrGrid.RowDefinitions.Add(row);

        // 行に追加するコントロールを作成
        var label = new Label { Text = "QRコード", Style = (Style)Resources["LabelFontStyle"] };
        var entry = new Entry { Text = $"{qrGrid.RowDefinitions.Count}行2列目" };

        // Gridにコントロールを追加
        qrGrid.Add(label, 0, qrGrid.RowDefinitions.Count - 1);
        qrGrid.Add(entry, 1, qrGrid.RowDefinitions.Count - 1);
    }
}