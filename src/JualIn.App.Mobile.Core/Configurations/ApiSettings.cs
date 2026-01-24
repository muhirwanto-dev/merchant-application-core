namespace JualIn.App.Mobile.Core.Configurations
{
    public sealed record ApiSettings(
        string BackendUrl
        )
    {
        public const string Section = "Apis";


        //#if DEBUG
        //        public static string ServerAddress => "http://192.168.50.251:5022/api";
        //#if ANDROID
        //        public static string ServerAddressWithEmulator => "http://10.0.2.2:5022/api";
        //#else
        //        public static string ServerAddressWithEmulator => "http://10.0.2.2:5022/api";
        //#endif // ANDROID
        //#else
        //        public static string ServerAddress => "https://localhost:7083/api";
        //#endif // DEBUG
    }
}
