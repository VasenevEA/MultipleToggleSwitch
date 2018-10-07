using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MultiSwitchSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiToogleSwitch : ContentView, INotifyPropertyChanged
    {
        public new Color BackgroundColor { get; set; } = Color.FromHex("#14B774");
        public List<ToogleCustomButton> ToogleButtons { get; set; } = new List<ToogleCustomButton>();
        public IToogled SelectedToogle { get; set; }
        public Color SelectedColor { get; set; } = Color.FromRgba(50, 249, 3, 137);
        public Color DefaultColor { get; set; } = Color.FromHex("#14B774");
        public Color SelectedBorderColor { get; set; } = Color.FromHex("#0CB90C");
        public Color DefaultBorderColor { get; set; } = Color.FromHex("#14B774");

        public float CornerRadius { get; set; } = 10;
        public double FontSize { get; set; }
        public string[] Toggles
        {
            get { return (string[])GetValue(TogglesProperty); }
            set { SetValue(TogglesProperty, value); }
        }

        public static readonly BindableProperty TogglesProperty =
    BindableProperty.Create("Toggles", typeof(string[]), typeof(MultiToogleSwitch),
                default(string[]), propertyChanged: TogglesPropertyChanged);

        private static void TogglesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _switch = bindable as MultiToogleSwitch;
            if (_switch != null)
            {
                if (newValue is string[])
                {
                    _switch.Init((string[])newValue);
                }
            }
        }

        public int SelectedId
        {
            get { return (int)GetValue(SelectedIdProperty); }
            set { SetValue(SelectedIdProperty, value); }
        }

        public static readonly BindableProperty SelectedIdProperty =
    BindableProperty.Create("SelectedId", typeof(int), typeof(MultiToogleSwitch),
                default(int), BindingMode.TwoWay, propertyChanged: SelectedIdPropertyChanged);

        private static void SelectedIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _switch = bindable as MultiToogleSwitch;
            if (_switch != null)
            {
                if (newValue is int)
                {
                    _switch.Select((int)newValue);
                }
            }
        }

        public string SelectedImageSource
        {
            get { return (string)GetValue(SelectedImageSourceProperty); }
            set { SetValue(SelectedImageSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedImageSourceProperty =
    BindableProperty.Create("SelectedImageSource", typeof(string), typeof(MultiToogleSwitch),
                default(string), BindingMode.OneWay, propertyChanged: SelectedImageSourcePropertyPropertyChanged);

        private static void SelectedImageSourcePropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = bindable as MultiToogleSwitch;
            button.Select(button.SelectedId);
        }

        #region Image specific
        public double SelectedImageWidth { get; set; }
        public double SelectedImageHeight { get; set; }
        public Aspect SelectedImageAspect { get; set; }
        public LayoutOptions SelectedImageVerticalOptions { get; set; }
        public LayoutOptions SelectedImageHorizontalOptions { get; set; }
        #endregion


        public bool EvenWidth { get; set; }

        public MultiToogleSwitch()
        {
            InitializeComponent();
            this.SizeChanged += MultiToogleSwitch_SizeChanged;
        }
        public void Init(string[] tooglesNames)
        {
            MainStack.Children.Clear();
            for (int i = 0; i < tooglesNames.Length; i++)
            {
                var toogleButton = new ToogleCustomButton(i, CornerRadius, FontSize)
                {
                    Command = ClickCommand,
                    SelectedColor = SelectedColor,
                    SelectedBorderColor = SelectedBorderColor,
                    DefaultColor = DefaultColor,
                    DefaultBorderColor = DefaultBorderColor,
                    Text = tooglesNames[i],
                    CornerRadius = CornerRadius
                };
                toogleButton.CommandParameter = toogleButton;
                ToogleButtons.Add(toogleButton);
                MainStack.Children.Add(toogleButton);
                MainStack.BackgroundColor = BackgroundColor;
            }
        }

        private void MultiToogleSwitch_SizeChanged(object sender, EventArgs e)
        {   //Set even width
            if (EvenWidth)
            {
                var maxSize = ToogleButtons.Max(x => x.Width);
                foreach (var toogle in ToogleButtons)
                    toogle.WidthRequest = maxSize;
            }
            else
            {
                foreach (var toogle in ToogleButtons)
                    toogle.WidthRequest = 0;
            }
        }

        public void Select(int id)
        {
            foreach (var item in ToogleButtons)
            {
                if (item.NumberId == id)
                {
                    item.SetImage(SelectedImageSource, SelectedImageWidth,
                        SelectedImageHeight,
                        SelectedImageAspect,
                        SelectedImageVerticalOptions,
                        SelectedImageHorizontalOptions);
                    item.Toogle(false);
                }
                else
                {
                    item.SetImage(null, SelectedImageWidth,
                         SelectedImageHeight,
                         SelectedImageAspect,
                         SelectedImageVerticalOptions,
                         SelectedImageHorizontalOptions);
                    item.UnToogle(false);
                }
            }
        }

        ICommand ClickCommand => new Xamarin.Forms.Command((obj) =>
        {
            var button = (IToogled)obj;


            foreach (var item in ToogleButtons)
            {
                if (item == button)
                    button.Toogle();
                else if (item == SelectedToogle)
                    SelectedToogle?.UnToogle();
                else
                    item.UnToogle(false);
            }
            SelectedToogle = button;
            SelectedId = button.NumberId;
        });
    }
}