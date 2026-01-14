namespace JualIn.App.Mobile.Presentation.UI.Controls;

public partial class ItemDetailBottom : ContentView
{
    public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item), typeof(object), typeof(object), default);

    public object Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public ItemDetailBottom()
    {
        InitializeComponent();
    }
}