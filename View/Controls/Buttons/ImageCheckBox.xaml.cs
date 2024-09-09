namespace MFASeeker.View.Controls.Buttons;

public partial class ImageCheckBox : ContentView
{
	public ImageCheckBox()
	{
        InitializeComponent();
    }
    // Свойство IsChecked с поддержкой привязки
    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ImageCheckBox), false, BindingMode.TwoWay);
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    
    // Свойства для изображения активного состояния
    public ImageSource ImageActive
    {
        get => (ImageSource)GetValue(ImageActiveProperty);
        set => SetValue(ImageActiveProperty, value);
    }
    public static readonly BindableProperty ImageActiveProperty =
        BindableProperty.Create(nameof(ImageActive), typeof(ImageSource), typeof(CustomCheckBox), default(ImageSource), BindingMode.TwoWay);

    // Свойства для изображения неактивного состояния
    public ImageSource ImageInactive
    {
        get => (ImageSource)GetValue(ImageInactiveProperty);
        set => SetValue(ImageInactiveProperty, value);
    }
    public static readonly BindableProperty ImageInactiveProperty =
        BindableProperty.Create(nameof(ImageInactive), typeof(ImageSource), typeof(CustomCheckBox));

    private void UpdateImage()
    {
        // Обновляем изображение в зависимости от состояния чекбокса
        Image.Source = IsChecked ? ImageActive : ImageInactive;
    }

    public event EventHandler<CheckedChangedEventArgs?> IsCheckedChanged = delegate { };
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Обновляем привязанное свойство IsChecked
        IsChecked = e.Value;
        // Обновляем изображение при изменении состояния чекбокса
        UpdateImage();
        // Вызываем событие IsCheckedChanged
        IsCheckedChanged?.Invoke(this, e);
    }
    // ПЕРЕОПРЕДЕЛЕНИЕ ЭТОГО МЕТОДА ПОЗВОЛИЛО ЖИТЬ ЭТОМУ ЧЕКБОКСУ, ХВАЛА ВСЕВЫШНЕМУ
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        UpdateImage();
    }
}