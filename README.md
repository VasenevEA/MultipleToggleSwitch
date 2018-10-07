# MultipleToogleSwitch
Multiple position Toogle Switch control for Xamarin Forms

This is a ContentView based Control. 
Requeires - None;

How to use:
- add files .xaml and .xaml.cs to your pcl/.net Standart project
- custom file (if need)
- paste code on your page/view\
```
<local:MultiToogleSwitch Toggles="{Binding Toggles}" 
                                 SelectedId="{Binding SelectedId, Mode=TwoWay}"
                                 HorizontalOptions="Center"
                                 VerticalOptions="Center"
                                 EvenWidth="True"
                                 FontSize="14"/>
```
- In you ViewModel add property ( array of buttons text) 
```
public string[] Toggles { get; set; } = new string[] { "On", "Auto", "Off", "Some" };
```
- Bind SelectedId property
- Profit!

```
public string[] Toggles { get; set; } = new string[] { "On", "Auto", "Off"};
```
![](https://github.com/VasenevEA/MultipleToogleSwitch/blob/master/Res/3toogleSwitch.gif)

```
public string[] Toggles { get; set; } = new string[] { "On", "Auto", "Off", "Some" };
```
![](https://github.com/VasenevEA/MultipleToogleSwitch/blob/master/Res/4toogleSwitch.gif)

- You cane change some parameters
``` 
public new Color BackgroundColor { get; set; }
public Color SelectedColor { get; set; }
public Color DefaultColor { get; set; }

public Color SelectedBorderColor { get; set; }
public Color DefaultBorderColor { get; set; }
public float CornerRadius { get; set; }
public double FontSize { get; set; }
```
