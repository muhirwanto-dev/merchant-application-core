namespace JualIn.SharedLib
{
    /// <summary>
    /// Provides predefined tasks that represent common waiting intervals for use in asynchronous operations.
    /// </summary>
    /// <remarks>These static tasks can be used to introduce delays of varying lengths without repeatedly
    /// allocating new delay tasks. The intervals are suitable for scenarios such as throttling, retry backoff, or
    /// simulating latency in tests. All tasks are completed after their respective timeouts and can be awaited multiple
    /// times. The class is thread-safe.</remarks>
    public static class Waiting
    {
        public static readonly Task Moment = Task.Delay(100);
        public static readonly Task Half = Task.Delay(500);
        public static readonly Task One = Task.Delay(1000);
        public static readonly Task Long = Task.Delay(3000);
    }
}
