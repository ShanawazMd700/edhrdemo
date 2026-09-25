using Reqnroll;

using MobileAppium.Drivers;

namespace PlaywrightDemo.Hooks
{
    [Binding]
    public class MobileHooks
    {
        public static AppiumDriverManager DriverManager { get; } =
            new AppiumDriverManager();

        [BeforeScenario]
        public void StartMobileDriver()
        {
            DriverManager.StartDriver();
        }

        [AfterScenario]
        public void StopMobileDriver()
        {
            DriverManager.StopDriver();
        }
    }
}