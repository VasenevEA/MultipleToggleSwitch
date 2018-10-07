using System.Windows.Input;
using Xamarin.Forms;

namespace MultiSwitchSample
{
    public interface IToogled
    {
        int NumberId { get; }
        void Toogle(bool animate = true);
        void UnToogle(bool animate = true);

        void SetImage(string source, double width, double height, Aspect aspect, LayoutOptions verticalOption, LayoutOptions horisontalOption);
    }

    public class ToogleCustomButton : Frame, IToogled
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
        private Frame body;
        private Image image;

        public ToogleCustomButton(int id, float cornerRadius, double fontSize)
        {
            SetBorderColor(DefaultBorderColor);
            HasShadow = false;
            Padding = 4;
            Margin = new Thickness(-3, 0);
            CornerRadius = cornerRadius;

            NumberId = id;
            body = new Frame
            {
                CornerRadius = this.CornerRadius,
                Padding = 0,
                Margin = 0,
                HasShadow = false
            };
            label = new Label
            {
                Margin = 10,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = fontSize
            };

            var imageLay = new Grid();
            image = new Image();
            image.WidthRequest = this.WidthRequest;
            imageLay.Children.Add(image);
            imageLay.Children.Add(label);
            body.Content = imageLay;
            Content = body;

            body.GestureRecognizers.Add(new TapGestureRecognizer(view =>
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
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!this.AnimationIsRunning("FadeTo") && animate)
                {
                    body.BackgroundColor = SelectedColor;
                    SetBorderColor(SelectedBorderColor);
                    await this.FadeTo(1, 50, Easing.Linear);
                }
                else
                {
                    body.BackgroundColor = SelectedColor;
                    BackgroundColor = SelectedBorderColor;
                }
            });
        }

        public void UnToogle(bool animate = true)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!this.AnimationIsRunning("FadeTo") && animate)
                {
                    await this.FadeTo(0.7, 150, Easing.Linear);
                    SetBorderColor(DefaultColor);
                    BackgroundColor = DefaultBorderColor;
                }
                else
                {
                    body.BackgroundColor = DefaultColor;
                    BackgroundColor = DefaultBorderColor;
                }
            });
        }

        public void SetImage(string source, double width, double height, Aspect aspect, LayoutOptions verticalOption, LayoutOptions horizontalOptions)
        {
            image.Source = source;
            image.Aspect = aspect;
            image.WidthRequest = width;
            image.HeightRequest = height;
            image.VerticalOptions = verticalOption;
            image.HorizontalOptions = horizontalOptions;
        }
    }
}