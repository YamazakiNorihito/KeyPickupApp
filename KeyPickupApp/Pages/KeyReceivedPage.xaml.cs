using Microsoft.Maui.Controls;

namespace KeyPickupApp.Pages;

public partial class KeyReceivedPage : ContentPage
{
    private static int QR_GIRD_MAX_ROW = 100;

    private readonly GridLength RowDefinition_Height;

    public KeyReceivedPage()
	{
		InitializeComponent();

        // Style�ł��������ɂ����@���킩��Ȃ������̂ŃR�[�h�Ŋ撣����
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
        // �����N���A�Ɏ��Ԃ�������悤�ł���΁AEntry���L���b�V�����邵���Ȃ�
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
        // �V�����s���쐬
        var row = new RowDefinition { Height = RowDefinition_Height };

        // Grid�ɍs��ǉ�
        qrGrid.RowDefinitions.Add(row);

        // �s�ɒǉ�����R���g���[�����쐬
        var label = new Label { Text = "QR�R�[�h", Style = (Style)Resources["LabelFontStyle"] };
        var entry = new Entry { Text = $"{qrGrid.RowDefinitions.Count}�s2���" };

        // Grid�ɃR���g���[����ǉ�
        qrGrid.Add(label, 0, qrGrid.RowDefinitions.Count - 1);
        qrGrid.Add(entry, 1, qrGrid.RowDefinitions.Count - 1);
    }
}