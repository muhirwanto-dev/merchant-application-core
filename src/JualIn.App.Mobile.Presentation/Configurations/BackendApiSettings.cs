namespace JualIn.App.Mobile.Presentation.Configurations
{
    public sealed class BackendApiSettings
    {
#if DEBUG
        public string ServerAddress => "http://192.168.50.251:5022/api";
#if ANDROID
        public string ServerAddressWithEmulator => "http://10.0.2.2:5022/api";
#else
        public string ServerAddressWithEmulator => "http://10.0.2.2:5022/api";
#endif // ANDROID
#else
        public string ServerAddress => "https://localhost:7083/api";
#endif // DEBUG
    }
}
