using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.View.Controls
{
    public class ImageBase64 : Image
    {
        public static readonly BindableProperty Base64SourceProperty =
             BindableProperty.Create(
                 nameof(Base64Source),
                 typeof(string),
                 typeof(ImageBase64),
                 string.Empty,
                 propertyChanged: OnBase64SourceChanged);
        public string Base64Source
        {   set { SetValue(Base64SourceProperty, value); }
            get { return (string)GetValue(Base64SourceProperty); }
        }

        private static void OnBase64SourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is string base64String && !string.IsNullOrEmpty(base64String))
            {
                ((Image)bindable).Source = ImageSource.FromStream(() =>
                {
                    var imageBytes = Convert.FromBase64String(base64String);
                    return new MemoryStream(imageBytes); // Создаем новый поток каждый раз
                });
            }
        }
    }
}
