using CommunityToolkit.Mvvm.Input;

namespace MFASeeker.View.Controls.Buttons;

public partial class StarSelector : ContentView
{
    private const string GoldStar = "gold_startoilet";
    private const string WhiteStar = "white_startoilet";
    public StarSelector()
	{
		InitializeComponent();
        UpdateStarImages();
    }
    public int SelectedStar
    {
        get => (int)GetValue(SelectedStarProperty);
        set => SetValue(SelectedStarProperty, value);
    }
    private static readonly BindableProperty SelectedStarProperty =
           BindableProperty.Create(nameof(SelectedStar), typeof(int), typeof(StarSelector), 0, propertyChanged: OnSelectedStarChanged);

    // Привязка события для обработки нажатия
    public delegate void StarClickedEventHandler(object sender, int star);
    public StarClickedEventHandler OnStarClicked
    {
        get => (StarClickedEventHandler)GetValue(OnStarClickedProperty);
        set => SetValue(OnStarClickedProperty, value);
    }
    private static readonly BindableProperty OnStarClickedProperty =
       BindableProperty.Create(nameof(OnStarClicked), typeof(EventHandler<int>), typeof(StarSelector));

    [RelayCommand]
    private void StarSelected(int star)
    {
        SelectedStar = star;
        OnStarClicked?.Invoke(this, star); // Вызов события
    }
    private static void OnSelectedStarChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (StarSelector)bindable;
        control.UpdateStarImages();
    }
    private void UpdateStarImages()
    {
        for (int i = 1; i <= 5; i++)
        {
            var starImage = i <= SelectedStar ? GoldStar : WhiteStar;
            SetStarImage(i, starImage);
        }
    }
    private void SetStarImage(int starNumber, string imageSource)
    {
        var imageButton = (ImageButton)StarSelectorLayout.Children[starNumber - 1];
        imageButton.Source = imageSource;
    }
}