using CommunityToolkit.Maui.Views;

namespace MFASeeker.View;

public partial class NewPinPopup : Popup
{
    public event EventHandler<bool>? OnResult;

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