using System.Windows.Input;

namespace MFASeeker.View.Controls.Buttons;

public partial class ImageCheckBox : ContentView
{
    public ImageCheckBox()
    {
        InitializeComponent();
    }

    // �������� IsChecked � ���������� ��������
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(
            nameof(IsChecked), 
            typeof(bool), 
            typeof(ImageCheckBox), 
            false, 
            BindingMode.TwoWay, 
            propertyChanged: OnIsCheckedChanged);
    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ImageCheckBox control)
        {
            control.UpdateImage();
        }
    }


    // �������� ��� ����������� ��������� ���������
    public ImageSource ImageActive
    {
        get => (ImageSource)GetValue(ImageActiveProperty);
        set => SetValue(ImageActiveProperty, value);
    }
    public static readonly BindableProperty ImageActiveProperty =
        BindableProperty.Create(nameof(ImageActive), typeof(ImageSource), typeof(ImageCheckBox), default(ImageSource), BindingMode.TwoWay);
    // �������� ��� ����������� ����������� ���������
    public ImageSource ImageInactive
    {
        get => (ImageSource)GetValue(ImageInactiveProperty);
        set => SetValue(ImageInactiveProperty, value);
    }
    public static readonly BindableProperty ImageInactiveProperty =
        BindableProperty.Create(nameof(ImageInactive), typeof(ImageSource), typeof(ImageCheckBox));
    // ����� ���������� �������� ��������
    private void UpdateImage()
    {
        // ��������� ����������� � ����������� �� ��������� ��������
        ImageBox.Source = IsChecked ? ImageActive : ImageInactive;
    }

    // �������� � ��������
    public static readonly BindableProperty CheckedCommandProperty =
        BindableProperty.Create(nameof(CheckedCommand), typeof(ICommand), typeof(ImageCheckBox));
    public ICommand CheckedCommand
    {
        get => (ICommand)GetValue(CheckedCommandProperty);
        set => SetValue(CheckedCommandProperty, value);
    }
    // Events
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckedCommand?.CanExecute(e.Value) == true)
        {
            UpdateImage();
            CheckedCommand.Execute(e.Value);
        }
    }
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        UpdateImage();
    }
}
