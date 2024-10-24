using MFASeeker.ViewModel;

namespace MFASeeker.View;
public partial class PinManagerPage : ContentPage
{
	//private PinManagerViewModel _pinManagerVM;
    private SwipeView? _previousSwipeView;
    public PinManagerPage(PinManagerViewModel pinManagerVM)
	{
		InitializeComponent();
        BindingContext = pinManagerVM;


        //CarouselCards.IsVisible = true;
        //ListCards.IsVisible = false;
    }
    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
    //    await _pinManagerVM.RefreshToiletsCommand.ExecuteAsync(null);
    //}
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

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        //if (CarouselCards.IsVisible)
        //{
        //    CarouselCards.IsVisible = false;
        //    ListCards.IsVisible = true;
        //}
        //else
        //{
        //    CarouselCards.IsVisible = true;
        //    ListCards.IsVisible = false;
        //}
    }
}