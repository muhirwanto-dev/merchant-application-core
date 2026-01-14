using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace JualIn.App.Mobile.Presentation.Core.Behaviors
{
    public class NoStatusBarBehavior : StatusBarBehavior
    {
        private static readonly StatusBarBehavior? _originalShellBehavior =
            Shell.Current.Behaviors.FirstOrDefault(x => x is StatusBarBehavior) as StatusBarBehavior;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public NoStatusBarBehavior()
        {
            StatusBarColor = Colors.Transparent;
            StatusBarStyle = StatusBarStyle.Default;
        }

        public void Apply()
        {

            Shell.Current.Behaviors.Remove(_originalShellBehavior);
            Shell.Current.Behaviors.Add(this);
        }

        public void Revert()
        {
            Shell.Current.Behaviors.Remove(this);

            if (_originalShellBehavior != null)
            {
                Shell.Current.Behaviors.Add(_originalShellBehavior);
            }
        }
    }
}
