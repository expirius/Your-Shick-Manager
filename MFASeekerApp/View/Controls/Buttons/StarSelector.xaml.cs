namespace MFASeeker.View.Controls.Buttons;

/*
 * Необходимо в будущем сделать стили звездочек для разных целей
 */
public partial class StarSelector : ContentView
{
    private const string GoldStarImage = "gold_startoilet";
    private const string WhiteStarImage = "white_startoilet";

    public StarSelector()
    {
        InitializeComponent();
        UpdateStarImages();
    }

    // Привязываемые свойства, как у label.Text 
    public int SelectedStar
    {
        get => (int)GetValue(SelectedStarProperty);
        set => SetValue(SelectedStarProperty, value);
    }
    public static readonly BindableProperty SelectedStarProperty =
        BindableProperty.Create(
            nameof(SelectedStar),
            typeof(int),
            typeof(StarSelector),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnSelectedStarChanged);

    private static void OnSelectedStarChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is StarSelector control)
        {
            control.UpdateStarImages();
        }
    }
    private void UpdateStarImages()
    {
        for (int i = 1; i <= 5; i++)
        {
            var starImage = i <= SelectedStar ? GoldStarImage : WhiteStarImage;
            SetStarImage(i, starImage);
        }
    }
    private void SetStarImage(int starIndex, string imageSource)
    {
        var imageButton = (ImageButton)StarSelectorLayout.Children[starIndex - 1];
        imageButton.Source = imageSource;
    }
    private void OnStarClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button &&
            button.CommandParameter is string starString &&
            int.TryParse(starString, out int star))
        {
            SelectedStar = star;
        }
    }
}
