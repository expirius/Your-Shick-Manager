using MFASeeker.Model;

namespace MFASeeker.View.Controls;

public partial class ToiletCard : ContentView
{
	public ToiletCard()
	{
		InitializeComponent();
	}
	public Toilet Toilet
	{
		get => (Toilet)GetValue(ToiletProperty);
		set => SetValue(ToiletProperty, value);
	}
	public static readonly BindableProperty ToiletProperty =
		BindableProperty.Create(
			nameof(ToiletProperty),
			typeof(Toilet),
			typeof(ToiletCard),
			null,
			propertyChanged: OnToiletChanged);
	public static void OnToiletChanged(BindableObject bindable, object oldValue, object newValue)
	{
        if (bindable is ToiletCard card)
        {
            card.SetValue(ToiletProperty, newValue);
        }
    }
}