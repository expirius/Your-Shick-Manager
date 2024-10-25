using CommunityToolkit.Maui.Views;
using MFASeekerApp.ViewModel;

namespace MFASeekerApp.View;

public partial class NewPinPopup : Popup
{
    public NewPinPopup()
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

    private void OnAddImageClicked(object sender, EventArgs e)
    {

    }
}