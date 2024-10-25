using CommunityToolkit.Maui.Views;

namespace MFASeekerApp.View;

public partial class ToiletQRpopup : Popup
{
	public ToiletQRpopup()
	{
		InitializeComponent();
	}
    void OnYesButtonClicked(object sender, EventArgs e)
    {
        Close(true);
    }

    void OnCancelButtonClicked(object sender, EventArgs e)
    {
        Close(false);
    }
}