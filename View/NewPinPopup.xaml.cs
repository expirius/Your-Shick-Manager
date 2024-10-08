using CommunityToolkit.Maui.Views;
using MFASeeker.ViewModel;

namespace MFASeeker.View;

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
}