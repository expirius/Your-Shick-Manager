using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.View.Controls.Buttons
{
    public class CustomCheckBox : ContentView
    {
        private readonly Image _image; // Элемент для отображения изображения
        private readonly CheckBox _checkBox; // Сам чекбокс

        public event EventHandler<CheckedChangedEventArgs> IsCheckedChanged = delegate { };

        // Свойство IsChecked с поддержкой привязки
        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CustomCheckBox), false, BindingMode.TwoWay);
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }


        public CustomCheckBox()
        {
            // Создаем изображение
            _image = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Создаем сам чекбокс
            _checkBox = new CheckBox
            {
                Opacity = 0, // Скрываем стандартный вид чекбокса
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Подписываемся на событие изменения состояния чекбокса
            _checkBox.CheckedChanged += OnCheckedChanged;

            // Создаем Grid для размещения чекбокса и изображения
            var grid = new Grid();
            grid.Children.Add(_image); // Добавляем изображение
            grid.Children.Add(_checkBox); // Добавляем чекбокс

            // Устанавливаем Grid как содержимое ContentView
            Content = grid;

            // Инициализация изображения на основе текущего состояния
            UpdateImage();
        }

        // Свойства для изображения активного состояния
        public ImageSource ImageActive
        {
            get => (ImageSource)GetValue(ImageActiveProperty);
            set => SetValue(ImageActiveProperty, value);
        }
        public static readonly BindableProperty ImageActiveProperty =
            BindableProperty.Create(nameof(ImageActive), typeof(ImageSource), typeof(CustomCheckBox));

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
            _image.Source = _checkBox.IsChecked ? ImageActive : ImageInactive;
        }

        private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
            // Обновляем изображение при изменении состояния чекбокса
            UpdateImage();
            // Обновляем привязанное свойство IsChecked
            IsChecked = e.Value;
            // Вызываем событие IsCheckedChanged
            IsCheckedChanged?.Invoke(this, e);
        }
        /*
        private Image _image; // Элемент для отображения изображения
        public CustomCheckBox()
        {
            // Поодписка на изменения чекбокса
            CheckedChanged += OnCheckedChanged;

            _image = new();

            this.Opacity = 0;

            UpdateImage();

        }
        // Свойство для Corner Radius
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CustomCheckBox), 0f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // Свойства для изображения активного состояния
        public ImageSource ImageActive
        {
            get => (ImageSource)GetValue(ImageActiveProperty);
            set => SetValue(ImageActiveProperty, value);
        }
        public static readonly BindableProperty ImageActiveProperty =
            BindableProperty.Create(nameof(ImageActive), typeof(ImageSource), typeof(CustomCheckBox));

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
            if (IsChecked)
            {
                _image.Source = ImageActive; // Устанавливаем изображение для активного состояния
            }
            else
            {
                _image.Source = ImageInactive; // Устанавливаем изображение для неактивного состояния
            }
        }

        private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            UpdateImage();
        }

        // Переопределяем метод OnParentSet, чтобы добавить изображение в визуальное дерево при добавлении CustomCheckBox в интерфейс
        protected override void OnParentSet()
        {
            base.OnParentSet();

            // Добавляем изображение в визуальное дерево (предполагается, что это элемент типа ContentView или аналогичный)
            if (Parent is Layout layout)
            {
                layout.Children.Add(_image);
            }
        }
        */
    }
}
