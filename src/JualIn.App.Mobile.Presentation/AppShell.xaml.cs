using JualIn.App.Mobile.Presentation.Modules.Dashboard.Views;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Views;

namespace JualIn.App.Mobile.Presentation
{
    public partial class AppShell : Shell
    {
        private static readonly Dictionary<string, (ImageSource Unselected, ImageSource Selected)> _tabIconMap = new()
        {
            { nameof(DashboardPage), (
                new FontImageSource
                {
                    FontFamily = UraniumUI.FontAliases.MaterialRounded,
                    Glyph = UraniumUI.Icons.MaterialSymbols.MaterialRounded.Home,
                },
                new FontImageSource
                {
                    FontFamily = UraniumUI.FontAliases.MaterialRoundedFilled,
                    Glyph = UraniumUI.Icons.MaterialSymbols.MaterialRounded.Home,
                })
            },
            { nameof(InventoryManagementPage), (
                new FontImageSource
                {
                    FontFamily = UraniumUI.FontAliases.MaterialRounded,
                    Glyph = UraniumUI.Icons.MaterialSymbols.MaterialRounded.Package_2,
                },
                new FontImageSource
                {
                    FontFamily = UraniumUI.FontAliases.MaterialRoundedFilled,
                    Glyph = UraniumUI.Icons.MaterialSymbols.MaterialRounded.Package_2,
                })
            }
        };

        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (IsPopup(args.Current))
            {
                return;
            }

            if (BottomTab?.CurrentItem?.CurrentItem is ShellContent tab)
            {
                if (_tabIconMap.TryGetValue(tab.Route, out var icons))
                {
                    tab.Icon = icons.Unselected;
                }
            }
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            if (IsPopup(args.Current))
            {
                return;
            }

            if (BottomTab?.CurrentItem?.CurrentItem is ShellContent tab)
            {
                if (_tabIconMap.TryGetValue(tab.Route, out var icons))
                {
                    tab.Icon = icons.Selected;
                }
            }
        }

        private static bool IsPopup(ShellNavigationState state)
            => state != null && state.Location.OriginalString.Contains("PopupPage", StringComparison.InvariantCultureIgnoreCase);
    }
}
