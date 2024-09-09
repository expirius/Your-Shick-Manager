namespace MFASeeker.View.Controls.Buttons;

public partial class ImageCheckBox : ContentView
{
	public ImageCheckBox()
	{
        InitializeComponent();
    }
    // �������� IsChecked � ���������� ��������
    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ImageCheckBox), false, BindingMode.TwoWay);
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    
    // �������� ��� ����������� ��������� ���������
    public ImageSource ImageActive
    {
        get => (ImageSource)GetValue(ImageActiveProperty);
        set => SetValue(ImageActiveProperty, value);
    }
    public static readonly BindableProperty ImageActiveProperty =
        BindableProperty.Create(nameof(ImageActive), typeof(ImageSource), typeof(CustomCheckBox), default(ImageSource), BindingMode.TwoWay);

    // �������� ��� ����������� ����������� ���������
    public ImageSource ImageInactive
    {
        get => (ImageSource)GetValue(ImageInactiveProperty);
        set => SetValue(ImageInactiveProperty, value);
    }
    public static readonly BindableProperty ImageInactiveProperty =
        BindableProperty.Create(nameof(ImageInactive), typeof(ImageSource), typeof(CustomCheckBox));

    private void UpdateImage()
    {
        // ��������� ����������� � ����������� �� ��������� ��������
        Image.Source = IsChecked ? ImageActive : ImageInactive;
    }

    public event EventHandler<CheckedChangedEventArgs?> IsCheckedChanged = delegate { };
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // ��������� ����������� �������� IsChecked
        IsChecked = e.Value;
        // ��������� ����������� ��� ��������� ��������� ��������
        UpdateImage();
        // �������� ������� IsCheckedChanged
        IsCheckedChanged?.Invoke(this, e);
    }
    // ��������������� ����� ������ ��������� ���� ����� ��������, ����� ����������
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        UpdateImage();
    }
}