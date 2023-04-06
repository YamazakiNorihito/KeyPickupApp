using CommunityToolkit.Maui.Views;
namespace KeyPickupApp.Pages;

public partial class KeyReceivedResultPopup : Popup 
{

    /* xamlのTextに設定すると以下のエラーが出るので、csファイルで管理する
     * エラー	MSB4018	"XamlCTask" タスクが予期せずに失敗しました。
     * System.Xml.XmlException: 指定されたエンコードに無効な文字があります。 行 20、位置 57。
     */
    private static string ALERT_MESSAGE_TEMPLATE = "{0}件送信失敗しました。失敗しているものは残留しているので確認して再送信してください";
    private static string OK_MESSAGE_TEMPLATE = "{0}件送信成功しました。";



    public KeyReceivedResultPopup(int updateFailureCount, int updateSuccessCount)
	{
        InitializeComponent();

        AlertMessage.Text = string.Format(ALERT_MESSAGE_TEMPLATE, updateFailureCount);
        OkMessage.Text = string.Format(OK_MESSAGE_TEMPLATE, updateSuccessCount);
    }

    void ClosePopup(object sender, EventArgs e) => Close();
}