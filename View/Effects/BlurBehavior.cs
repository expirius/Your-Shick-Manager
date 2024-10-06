#if ANDROID
using Android.Graphics;
using Android.Widget;

namespace MFASeeker.View.Effects;

public partial class BlurBehavior : PlatformBehavior<Image, ImageView>
{
    public static readonly BindableProperty RadiusProperty = BindableProperty.Create(
        nameof(Radius), 
        typeof(float), 
        typeof(BlurBehavior), 
        5f, 
        propertyChanged: OnRadiusChanged);

    public float Radius
    {
        get => (float)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }
    static void OnRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var behavior = (BlurBehavior)bindable;
        if (behavior.imageView is null)
        {
            return;
        }
        behavior.SetRendererEffect(behavior.imageView, Convert.ToSingle(newValue));
    }
    ImageView? imageView;
    protected override void OnAttachedTo(Image bindable, ImageView platformView)
    {
        imageView = platformView;
        SetRendererEffect(platformView, Radius);
    }
    protected override void OnDetachedFrom(Image bindable, ImageView platformView)
    {
        SetRendererEffect(platformView, 0);
    }
    void SetRendererEffect(ImageView imageView, float radius)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(31))
        {
            var renderEffect = radius > 0 ? GetEffect(radius) : null;
            imageView.SetRenderEffect(renderEffect);
        }
    }
    static RenderEffect? GetEffect(float radius)
    {
        return OperatingSystem.IsAndroidVersionAtLeast(31) ?
            RenderEffect.CreateBlurEffect(radius, radius, Shader.TileMode.Decal!) :
            null;
    }
}
#endif