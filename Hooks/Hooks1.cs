using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;
using System.Text.Json;
using PlaywrightDemo.ReportManagers;

namespace PlaywrightDemo.Hooks
{
    [Binding]
    public sealed class Hooks1
    {
        public static Hooks1? Instance { get; private set; }

        private readonly IObjectContainer _container;

        private IPlaywright? _playwright;
        private IBrowserContext? _context;

        public IPage? Page { get; private set; }

        public Hooks1(IObjectContainer container)
        {
            _container = container;
        }

        // =========================================================
        // BEFORE TEST RUN
        // =========================================================

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ExtentReportManager.StartReport();
        }


        // =========================================================
        // BEFORE SCENARIO
        // =========================================================

        [BeforeScenario]
        public async Task Setup(
            ScenarioContext scenarioContext,
            FeatureContext featureContext)
        {
            Instance = this;

            // -----------------------------------------------------
            // Get Feature and Scenario names
            // -----------------------------------------------------

            string featureName =
                featureContext.FeatureInfo.Title;

            string scenarioName =
                scenarioContext.ScenarioInfo.Title;

            // -----------------------------------------------------
            // Create Scenario under Feature
            // -----------------------------------------------------

            ExtentReportManager.CreateScenario(
                featureName,
                scenarioName);

            // ------------------------------------------------
            // Playwright setup
            // ------------------------------------------------

            _playwright = await Playwright.CreateAsync()
                ?? throw new InvalidOperationException(
                    "Playwright could not be initialized.");

            string userDataDir = Path.Combine(
                Directory.GetCurrentDirectory(),
                "EdgeProfile");

            _context =
                await _playwright.Chromium
                    .LaunchPersistentContextAsync(
                        userDataDir,
                        new BrowserTypeLaunchPersistentContextOptions
                        {
                            Headless = false,
                            Channel = "msedge",
                            ViewportSize = null,

                            IgnoreDefaultArgs = new[]
                            {
                                "--no-sandbox"
                            },

                            Args = new[]
                            {
                                "--start-maximized"
                            }
                        })
                ?? throw new InvalidOperationException(
                    "Browser context could not be created.");

            var page = _context.Pages.Count > 0
                ? _context.Pages[0]
                : await _context.NewPageAsync();

            Page = page
                ?? throw new InvalidOperationException(
                    "Browser page could not be created.");

            // ------------------------------------------------
            // Maximize Edge
            // ------------------------------------------------

            var cdp =
                await _context.NewCDPSessionAsync(Page);

            var windowInfo =
                await cdp.SendAsync(
                    "Browser.getWindowForTarget");

            if (windowInfo is not
                {
                    ValueKind: JsonValueKind.Object
                } windowInfoValue ||
                !windowInfoValue.TryGetProperty(
                    "windowId",
                    out var windowIdElement))
            {
                throw new InvalidOperationException(
                    "Could not determine the Edge window ID.");
            }

            int windowId =
                windowIdElement.GetInt32();

            await cdp.SendAsync(
                "Browser.setWindowBounds",
                new Dictionary<string, object>
                {
                    ["windowId"] = windowId,

                    ["bounds"] =
                        new Dictionary<string, object>
                        {
                            ["windowState"] = "maximized"
                        }
                });

            // ------------------------------------------------
            // Register Page with Reqnroll
            // ------------------------------------------------

            _container.RegisterInstanceAs<IPage>(Page);

            ExtentReportManager.Pass(
                "Playwright browser initialized.");
        }
        [AfterStep]
        public void AfterStep(
            ScenarioContext scenarioContext)
        {
            var step =
                scenarioContext.StepContext.StepInfo;

            string stepName =
                $"{step.StepDefinitionType} {step.Text}";

            if (scenarioContext.TestError != null)
            {
                ExtentReportManager.FailStep(
                    stepName,
                    scenarioContext.TestError);
            }
            else
            {
                ExtentReportManager.PassStep(
                    stepName);
            }
        }

        [AfterScenario]
        public async Task AfterScenario(
            ScenarioContext scenarioContext)
        {
            try
            {
                if (scenarioContext.TestError != null)
                {
                    ExtentReportManager.Fail(
                        "Scenario Failed");

                    ExtentReportManager.Fail(
                        scenarioContext.TestError);
                }
                else
                {
                    ExtentReportManager.Pass(
                        "Scenario Passed");
                }
            }
            finally
            {
                _playwright?.Dispose();

                _playwright = null;
                _context = null;
                Page = null;
                Instance = null;
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ExtentReportManager.Flush();

            ExtentReportManager.OpenReport();
        }
    }
}