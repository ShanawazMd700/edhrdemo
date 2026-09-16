using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;

namespace PlaywrightDemo.Hooks
{
    [Binding]
    public sealed class Hooks1
    {
        public static Hooks1 Instance { get; private set; }
        private readonly IObjectContainer _container;
        private IPlaywright _playwright;
        private IBrowserContext _context;
        public IPage Page { get; private set; }

        public Hooks1(IObjectContainer container)
        {
            _container = container;
        }
        [BeforeScenario]
        public async Task Setup()
        {
            Instance = this;

            _playwright = await Playwright.CreateAsync();

            string userDataDir = Path.Combine(
                Directory.GetCurrentDirectory(),
                "EdgeProfile");

            _context = await _playwright.Chromium.LaunchPersistentContextAsync(
            userDataDir,
            new BrowserTypeLaunchPersistentContextOptions
            {
                Headless = false,
                Channel = "msedge", // Tells Playwright to locate the local Edge binary automatically
                ViewportSize = null,
                // Suppress Playwright's auto-injected --no-sandbox flag (causes Edge warning banner)
                IgnoreDefaultArgs = new[] { "--no-sandbox" },
                Args = new[]
                {
                    "--start-maximized"  // Ensure content fills the full window on launch
                }
            });



            Page = _context.Pages.Count > 0
                ? _context.Pages[0]
                : await _context.NewPageAsync();

            // Explicitly maximize the Edge window
            var cdp = await _context.NewCDPSessionAsync(Page);

            var windowInfo = await cdp.SendAsync(
                "Browser.getWindowForTarget");

            int windowId = windowInfo.Value.GetProperty("windowId").GetInt32();

            await cdp.SendAsync(
                "Browser.setWindowBounds",
                new Dictionary<string, object>
                {
                    ["windowId"] = windowId,
                    ["bounds"] = new Dictionary<string, object>
                    {
                        ["windowState"] = "maximized"
                    }
                });
            _container.RegisterInstanceAs<IPage>(Page);
        }


        [AfterScenario]
        public async Task AfterScenario()
        {
            //await _context.CloseAsync();
            _playwright?.Dispose();
        }
    }
}
