using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MultiSwitchSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiToogleSwitch : ContentView, INotifyPropertyChanged
    {
        public new Color BackgroundColor { get; set; } = Color.Gray;

        public List<ToogleCustomButton> ToogleButtons { get; set; } = new List<ToogleCustomButton>();

        public IToogled SelectedToogle { get; set; }

        public Color SelectedColor { get; set; } = Color.Green;
        public Color DefaultColor { get; set; } = Color.Gray;

        public Color SelectedBorderColor { get; set; } = Color.LightGreen;
        public Color DefaultBorderColor { get; set; } = Color.LightGray;

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



        public string Test
        {
            get { return (string)GetValue(TestProperty); }
            set { SetValue(TestProperty, 1); }
        }

        public static readonly BindableProperty TestProperty =
    BindableProperty.Create("Test", typeof(string), typeof(MultiToogleSwitch),
                "dsd",
        BindingMode.TwoWay);

        public int SelectedId
        {
            get { return (int)GetValue(SelectedIdProperty); }
            set { SetValue(SelectedIdProperty, value); }
        }

        public static readonly BindableProperty SelectedIdProperty =
    BindableProperty.Create("SelectedId", typeof(int), typeof(MultiToogleSwitch),
                0, BindingMode.TwoWay, propertyChanged: SelectedIdPropertyChanged);

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

        public MultiToogleSwitch()
        {
            InitializeComponent();
        }
        public void Init(string[] tooglesNames)
        {
            MainStack.Children.Clear();
            for (int i = 0; i < tooglesNames.Length; i++)
            {
                var toogleButton = new ToogleCustomButton(i);
                ToogleButtons.Add(toogleButton);
                toogleButton.Command = ClickCommand;
                toogleButton.CommandParameter = toogleButton;

                toogleButton.SelectedColor = SelectedColor;
                toogleButton.SelectedBorderColor = SelectedBorderColor;

                toogleButton.DefaultColor = DefaultColor;
                toogleButton.DefaultBorderColor = DefaultBorderColor;

                toogleButton.Text = tooglesNames[i];

                MainStack.Children.Add(toogleButton);
                MainStack.BackgroundColor = BackgroundColor;
            }
        }

        public void Select(int id)
        {
            foreach (var item in ToogleButtons)
            {
                if (item.NumberId == id)
                    item.Toogle(false);
                else
                    item.UnToogle(false);
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

    public interface IToogled
    {
        int NumberId { get; }
        void Toogle(bool animate = true);
        void UnToogle(bool animate = true);
    }

    public class ToogleButton : Button, IToogled
    {
        public int NumberId { get; set; }
        public bool IsClicked { get; set; }

        public Color SelectedColor { get; set; }
        public Color DefaultColor { get; set; }

        public Color SelectedBorderColor { get; set; }
        public Color DefaultBorderColor { get; set; }

        public ToogleButton(int id)
        {
            NumberId = id;
        }
        public void Toogle(bool animate = true)
        {
            BackgroundColor = SelectedColor;
            BorderColor = SelectedBorderColor;
        }

        public void UnToogle(bool animate = true)
        {
            BackgroundColor = DefaultColor;
            BorderColor = DefaultBorderColor;
        }
    }

    public class ToogleCustomButton : StackLayout, IToogled
    {
        public int NumberId { get; set; }
        public Color SelectedColor { get; set; }
        public Color DefaultColor { get; set; }

        public Color SelectedBorderColor { get; set; }
        public Color DefaultBorderColor { get; set; }

        public string Text
        {
            get
            {
                return label.Text;
            }
            set
            {
                label.Text = value;
            }
        }
        Label label;

        public bool IsClicked { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

        public Color Color { get; set; }

        private StackLayout border;

        public ToogleCustomButton(int id)
        {
            Padding = 2;

            NumberId = id;
            border = new StackLayout();
            border.Children.Add(label = new Label
            {
                Margin = 10
            });
            Children.Add(border);

            border.GestureRecognizers.Add(new TapGestureRecognizer(view =>
            {
                Command?.Execute(CommandParameter);
            }));
        }

        public void SetBackColor(Color Color)
        {
            var stack = (StackLayout)Children[0];
            stack.BackgroundColor = Color;
        }

        public void SetBorderColor(Color Color)
        {
            BackgroundColor = Color;
        }

        public void Toogle(bool animate = true)
        {
            animate = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!this.AnimationIsRunning("FadeTo") && animate)
                {
                    await this.FadeTo(0.5, 150, Easing.Linear);
                    border.BackgroundColor = SelectedColor;
                    BackgroundColor = SelectedBorderColor;
                    await this.FadeTo(1, 150, Easing.Linear);
                }
                else
                {
                    border.BackgroundColor = SelectedColor;
                    BackgroundColor = SelectedBorderColor;
                }
            });
        }

        public void UnToogle(bool animate = true)
        {
            animate = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!this.AnimationIsRunning("FadeTo") && animate)
                {
                    border.BackgroundColor = DefaultColor;
                    BackgroundColor = DefaultBorderColor;
                    await this.FadeTo(1, 150, Easing.Linear);
                }
                else
                {
                    await this.FadeTo(0.7, 150, Easing.Linear);
                    border.BackgroundColor = DefaultColor;
                    BackgroundColor = DefaultBorderColor;
                }
            });
        }
    }
}