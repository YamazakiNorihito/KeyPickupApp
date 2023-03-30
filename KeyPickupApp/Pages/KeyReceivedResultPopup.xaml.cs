using CommunityToolkit.Maui.Views;
namespace KeyPickupApp.Pages;

public partial class KeyReceivedResultPopup : Popup 
{

    public KeyReceivedResultPopup()
	{
        InitializeComponent();
	}
    void OnOKButtonClicked(object? sender, EventArgs e) => Close("yamazaki");
}