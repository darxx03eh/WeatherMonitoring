namespace WeatherMonitoring.Tests.Captures;

public static class ConsoleCapture
{
    private static readonly object ConsoleLock = new();
    public static void Capture(Action<StringWriter> testBody)
    {
        lock (ConsoleLock)
        {
            var @out = Console.Out;
            var writer = new StringWriter();
            Console.SetOut(writer);
            try
            {
                testBody(writer);
            }
            finally
            {
                Console.SetOut(@out);
            }
        }
    }
}