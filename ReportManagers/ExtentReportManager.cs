using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using System.Diagnostics;

namespace PlaywrightDemo.ReportManagers
{
    public static class ExtentReportManager
    {
        private static ExtentReports? _extent;

        private static ExtentTest? _scenario;

        // Stores Features
        private static readonly Dictionary<
            string,
            ExtentTest> _features = new();

        private static string? _reportDirectory;
        private static string? _reportPath;
        private static string? _screenshotDirectory;


        // =========================================================
        // START REPORT
        // =========================================================

        public static void StartReport()
        {
            string timestamp =
                DateTime.Now.ToString(
                    "yyyy-MM-dd_HH-mm-ss");

            _reportDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Reports",
                timestamp);

            _screenshotDirectory = Path.Combine(
                _reportDirectory,
                "Screenshots");

            Directory.CreateDirectory(
                _reportDirectory);

            Directory.CreateDirectory(
                _screenshotDirectory);

            _reportPath = Path.Combine(
                _reportDirectory,
                "ExtentReport.html");

            var htmlReporter =
                new ExtentSparkReporter(
                    _reportPath);

            htmlReporter.Config.DocumentTitle =
                "Playwright Automation Report";

            htmlReporter.Config.ReportName =
                "Playwright Automation Execution";

            htmlReporter.Config.Theme =
                AventStack.ExtentReports
                    .Reporter.Config.Theme.Standard;

            _extent =
                new ExtentReports();

            _extent.AttachReporter(
                htmlReporter);

            _extent.AddSystemInfo(
                "Framework",
                "Playwright");

            _extent.AddSystemInfo(
                "Language",
                "C#");

            _extent.AddSystemInfo(
                "Browser",
                "Microsoft Edge");

            _extent.AddSystemInfo(
                "Execution Time",
                DateTime.Now.ToString(
                    "yyyy-MM-dd HH:mm:ss"));
        }


        // =========================================================
        // CREATE FEATURE -> SCENARIO
        // =========================================================

        public static void CreateScenario(
            string featureName,
            string scenarioName)
        {
            if (_extent == null)
            {
                throw new InvalidOperationException(
                    "Extent report has not been started.");
            }

            // -----------------------------------------------------
            // Get existing Feature
            // -----------------------------------------------------

            if (!_features.TryGetValue(
                    featureName,
                    out ExtentTest? feature))
            {
                // -------------------------------------------------
                // Feature does not exist.
                // Create a new top-level Feature.
                // -------------------------------------------------

                feature =
                    _extent.CreateTest(
                        featureName);

                _features.Add(
                    featureName,
                    feature);
            }

            // -----------------------------------------------------
            // Create Scenario under Feature
            // -----------------------------------------------------

            _scenario =
                feature.CreateNode(
                    scenarioName);

            _scenario.Info(
                $"Scenario started at " +
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }


        // =========================================================
        // PASS
        // =========================================================

        public static void Pass(
            string message)
        {
            _scenario?.Pass(message);
        }


        // =========================================================
        // FAIL
        // =========================================================

        public static void Fail(
            string message)
        {
            _scenario?.Fail(message);
        }


        public static void Fail(
            Exception exception)
        {
            _scenario?.Fail(exception);
        }


        // =========================================================
        // STEP PASS
        // =========================================================

        public static void PassStep(
            string step)
        {
            _scenario?.Pass(step);
        }


        // =========================================================
        // STEP FAIL
        // =========================================================

        public static void FailStep(
            string step,
            Exception exception)
        {
            _scenario?
                .Fail(step)
                .Fail(exception);
        }


        // =========================================================
        // LOG STEP
        // =========================================================

        public static void LogStep(
            string message)
        {
            _scenario?.Info(message);
        }


        // =========================================================
        // SCREENSHOT
        // =========================================================

        public static async Task AddScreenshotAsync(
            IPage page,
            string scenarioName)
        {
            if (_screenshotDirectory == null ||
                _scenario == null)
            {
                return;
            }

            string safeScenarioName =
                SanitizeFileName(
                    scenarioName);

            string screenshotPath =
                Path.Combine(
                    _screenshotDirectory,
                    $"{safeScenarioName}.png");

            await page.ScreenshotAsync(
                new PageScreenshotOptions
                {
                    Path = screenshotPath,
                    FullPage = true
                });

            _scenario.AddScreenCaptureFromPath(
                screenshotPath);
        }


        // =========================================================
        // FLUSH
        // =========================================================

        public static void Flush()
        {
            _extent?.Flush();
        }


        // =========================================================
        // OPEN REPORT
        // =========================================================

        public static void OpenReport()
        {
            if (string.IsNullOrWhiteSpace(
                    _reportPath))
            {
                return;
            }

            if (!File.Exists(
                    _reportPath))
            {
                return;
            }

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = _reportPath,
                    UseShellExecute = true
                });
        }


        // =========================================================
        // SANITIZE FILE NAME
        // =========================================================

        private static string SanitizeFileName(
            string fileName)
        {
            foreach (char invalidChar in
                     Path.GetInvalidFileNameChars())
            {
                fileName =
                    fileName.Replace(
                        invalidChar,
                        '_');
            }

            return fileName;
        }


        // =========================================================
        // CLEAR
        // =========================================================

        public static void Clear()
        {
            _features.Clear();

            _scenario = null;

            _extent = null;
        }
    }
}
