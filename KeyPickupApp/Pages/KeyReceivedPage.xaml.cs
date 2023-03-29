using Microsoft.Maui.Controls;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{
    private static int QR_GIRD_MAX_ROW = 100;

    private readonly GridLength RowDefinition_Height;

    public KeyReceivedPage()
	{
		InitializeComponent();

        // Styleでいい感じにやる方法がわからなかったのでコードで頑張った
        var rowDefinitionStyle = (Style)Resources["RowDefinitionStyle"];
        var height = rowDefinitionStyle.Setters.Where(o => o.Property.Equals(nameof(RowDefinition.Height))).SingleOrDefault();
        RowDefinition_Height = new GridLength(height is null ? 40 : Convert.ToInt32(height.Value));

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
        var row = new RowDefinition { Height = RowDefinition_Height };

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