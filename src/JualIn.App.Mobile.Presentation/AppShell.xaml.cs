#if ANDROID
using Android.OS;
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
#endif // ANDROID
using JualIn.App.Mobile.Presentation.Modules.Dashboard.Views;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Views;
using SingleScope.Mvvm.Maui;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation
{
    public partial class AppShell : Shell
    {
        private readonly IReportingService _reporting;

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
            _reporting = MauiServiceProvider.Current.GetRequiredService<IReportingService>();

            InitializeComponent();
            SetStatusBarColor();

            Application.Current?.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            SetStatusBarColor();
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            try
            {
                base.OnNavigating(args);

                if (BottomTab?.CurrentItem?.CurrentItem is ShellContent tab && args.Target is not null)
                {
                    if (_tabIconMap.TryGetValue(args.Target.Location.OriginalString.AsSpan(2).ToString(), out var icons))
                    {
                        tab.Icon = icons.Unselected;
                    }
                }
            }
            catch (Exception ex)
            {
                _reporting.Report(ex);
            }
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            try
            {
                base.OnNavigated(args);

                if (BottomTab?.CurrentItem?.CurrentItem is ShellContent tab && args.Current is not null)
                {
                    if (_tabIconMap.TryGetValue(args.Current.Location.OriginalString.AsSpan(2).ToString(), out var icons))
                    {
                        tab.Icon = icons.Selected;
                    }
                }
            }
            catch (Exception ex)
            {
                _reporting.Report(ex);
            }
        }

        private static bool IsPopup(ShellNavigationState state)
            => state != null && state.Location.OriginalString.Contains("PopupPage", StringComparison.InvariantCultureIgnoreCase);

        private static void SetStatusBarColor()
        {
#if ANDROID
            if (Application.Current?.Resources["Background"] is Color light)
            {
                bool isLight = Application.Current.RequestedTheme == AppTheme.Light;
                var targetColor = isLight ? light : (Application.Current?.Resources["DarkBackground"] is Color dark ? dark : light);
                var targetStyle = isLight ?
                    CommunityToolkit.Maui.Core.StatusBarStyle.LightContent :
                    CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent;

                if (Build.VERSION.SdkInt < BuildVersionCodes.VanillaIceCream)
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(targetColor);
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(targetStyle);
#pragma warning restore CA1416 // Validate platform compatibility
                }
                else
                {
                    var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
                    var window = activity?.Window;

                    if (window != null)
                    {
                        const string StatusBarOverlayTag = "StatusBarOverlay";

                        var decorGroup = (Android.Views.ViewGroup)window.DecorView;
                        var statusBarOverlay = decorGroup.FindViewWithTag(StatusBarOverlayTag);

                        if (statusBarOverlay == null)
                        {
                            int statusBarHeight = activity?.Resources?.GetIdentifier("status_bar_height", "dimen", "android") ?? -1;
                            int height = statusBarHeight > 0 ? activity?.Resources?.GetDimensionPixelSize(statusBarHeight) ?? -1 : 0;

                            statusBarOverlay = new(activity)
                            {
                                LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, height + 3) { Gravity = Android.Views.GravityFlags.Top }
                            };

                            decorGroup.AddView(statusBarOverlay);
                            statusBarOverlay.SetZ(0);
                        }

                        statusBarOverlay.SetBackgroundColor(targetColor.ToPlatform());

                        if (WindowCompat.GetInsetsController(window, decorGroup) is WindowInsetsControllerCompat controller)
                        {
                            controller.AppearanceLightStatusBars = isLight;
                        }
                    }
                }
            }
#endif // ANDROID
        }
    }
}
