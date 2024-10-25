using MFASeekerApp.Model;
using MFASeekerApp.ViewModel;
using System.Runtime.CompilerServices;

namespace MFASeekerApp.View.Controls;

public partial class ToiletCard : ContentView
{
    public static readonly BindableProperty ToiletVMProperty =
        BindableProperty.Create(
            nameof(ToiletVM),
            typeof(ToiletViewModel),  // Замените на правильный тип ViewModel
            typeof(ToiletCard),
            default(ToiletViewModel),
            propertyChanged: OnToiletVMChanged);

    public ToiletViewModel ToiletVM
    {
        get => (ToiletViewModel)GetValue(ToiletVMProperty);
        set => SetValue(ToiletVMProperty, value);
    }
    public ToiletCard()
	{
		InitializeComponent();
	}
    private static void OnToiletVMChanged(BindableObject bindable, object oldValue, object newValue)
    {

    }
}