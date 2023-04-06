using CommunityToolkit.Maui.Views;
namespace KeyPickupApp.Pages;

public partial class KeyReceivedResultPopup : Popup 
{

    /* xaml��Text�ɐݒ肷��ƈȉ��̃G���[���o��̂ŁAcs�t�@�C���ŊǗ�����
     * �G���[	MSB4018	"XamlCTask" �^�X�N���\�������Ɏ��s���܂����B
     * System.Xml.XmlException: �w�肳�ꂽ�G���R�[�h�ɖ����ȕ���������܂��B �s 20�A�ʒu 57�B
     */
    private static string ALERT_MESSAGE_TEMPLATE = "{0}�����M���s���܂����B���s���Ă�����͎̂c�����Ă���̂Ŋm�F���čđ��M���Ă�������";
    private static string OK_MESSAGE_TEMPLATE = "{0}�����M�������܂����B";



    public KeyReceivedResultPopup(int updateFailureCount, int updateSuccessCount)
	{
        InitializeComponent();

        AlertMessage.Text = string.Format(ALERT_MESSAGE_TEMPLATE, updateFailureCount);
        OkMessage.Text = string.Format(OK_MESSAGE_TEMPLATE, updateSuccessCount);
    }

    void ClosePopup(object sender, EventArgs e) => Close();
}