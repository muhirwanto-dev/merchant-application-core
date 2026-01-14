namespace JualIn.App.Mobile.Presentation.Shared.Constants
{
    public static class Preprocessor
    {
#if USE_VERSION_0
        public const bool USE_VERSION_0 = true;
#else
        public const bool USE_VERSION_0 = false;
#endif // USE_VERSION_0
    }
}
