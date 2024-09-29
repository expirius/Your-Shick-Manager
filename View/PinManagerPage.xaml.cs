using MFASeeker.ViewModel;

namespace MFASeeker.View;

public partial class PinManagerPage : ContentPage
{
	private PinManagerViewModel _pinManagerVM;
    private SwipeView? _previousSwipeView;
    public PinManagerPage(PinManagerViewModel pinManagerVM)
	{
		InitializeComponent();
        BindingContext = pinManagerVM;
        _pinManagerVM = pinManagerVM;

    }

    private void OnSwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        if (sender as SwipeView is var currentSwipeView)
        {
            if (_previousSwipeView != null && _previousSwipeView != currentSwipeView)
            {
                _previousSwipeView.Close();
            }
            _previousSwipeView = currentSwipeView;
        }
    }
}