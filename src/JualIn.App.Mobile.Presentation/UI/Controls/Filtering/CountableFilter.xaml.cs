using CommunityToolkit.Diagnostics;
using HorusStudio.Maui.MaterialDesignControls;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Filtering;

public partial class CountableFilter : MaterialChip
{
    private MaterialBadge? _badge;

    public CountableFilter()
    {
        var card = (MaterialCard)Content;
        var stack = (HorizontalStackLayout)card.Content!;
        var badge = new MaterialBadge
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = (Thickness)Application.Current!.Resources["CommonSpaceSmallLeft"]
        };

        stack.Children.Add(badge);

        _badge = badge;

        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        Guard.IsAssignableToType<CountableFilterViewModel>(BindingContext);

        base.OnBindingContextChanged();

        if (_badge != null)
        {
            _badge.BindingContext = BindingContext;
            _badge.SetBinding(
                MaterialBadge.IsVisibleProperty,
                nameof(CountableFilterViewModel.ShowCounter));
            _badge.SetBinding(
                MaterialBadge.TextProperty,
                nameof(CountableFilterViewModel.Counter));
        }

        this.SetBinding(
            MaterialChip.TextProperty,
            nameof(CountableFilterViewModel.Key));
        this.SetBinding(
            MaterialChip.IsSelectedProperty,
            nameof(CountableFilterViewModel.IsSelected));
    }
}