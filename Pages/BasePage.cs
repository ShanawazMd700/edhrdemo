using Microsoft.Playwright;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaywrightDemo.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; private set; }

        protected BasePage(IPage page)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
        }

        public async Task ScrollToProcessAsync(string processName, string sectionText = "Processes", int maxScrollAttempts = 8)
        {
            // 1. Target the scroll container strictly inside the specific section column wrapper
            var scrollContainer = Page.Locator("div.flex-column, div.optionbox-group")
                .Filter(new() { Has = Page.Locator(".optionbox-header", new() { HasText = sectionText }) })
                .Locator(".nested-middle.optionlist.optionbox, .optionbox-content.optionlist");

            // 2. Target the specific row inside that isolated column
            var targetRow = scrollContainer.Locator(".optionbox.optionbox-option.optionbox-content")
                                .GetByText(processName, new() { Exact = true });

            int attempt = 0;

            // Loop until the row is visible or we hit the maximum threshold
            while (attempt < maxScrollAttempts)
            {
                // First check if the targeted container itself is visible/ready
                if (await scrollContainer.CountAsync() == 1 && await targetRow.IsVisibleAsync())
                {
                    await targetRow.ScrollIntoViewIfNeededAsync();
                    return;
                }

                // Slowly scroll down by 100 pixels inside the correctly isolated container panel
                if (await scrollContainer.CountAsync() == 1)
                {
                    await scrollContainer.EvaluateAsync("el => el.scrollTop += 100");
                }

                await Page.WaitForTimeoutAsync(150);
                attempt++;
            }

            throw new System.TimeoutException($"Could not find process '{processName}' inside the '{sectionText}' column after scrolling.");
        }

        public async Task ScrollToProcessStepsAsync(string processName, string sectionText = "Process Steps", int maxScrollAttempts = 8)
        {
            // 1. Target the scroll container strictly inside the specific section column wrapper
            var scrollContainer = Page.Locator("div.flex-column, div.optionbox-group")
                .Filter(new() { Has = Page.Locator(".optionbox-header", new() { HasText = sectionText }) })
                .Locator(".nested-middle.optionlist.optionbox, .optionbox-content.optionlist");

            // 2. Target the specific row inside that isolated column
            var targetRow = scrollContainer.Locator(".optionbox.optionbox-option.optionbox-content")
                                .GetByText(processName, new() { Exact = true });

            int attempt = 0;

            // Loop until the row is visible or we hit the maximum threshold
            while (attempt < maxScrollAttempts)
            {
                // First check if the targeted container itself is visible/ready
                if (await scrollContainer.CountAsync() == 1 && await targetRow.IsVisibleAsync())
                {
                    await targetRow.ScrollIntoViewIfNeededAsync();
                    return;
                }

                // Slowly scroll down by 100 pixels inside the correctly isolated container panel
                if (await scrollContainer.CountAsync() == 1)
                {
                    await scrollContainer.EvaluateAsync("el => el.scrollTop += 100");
                }

                await Page.WaitForTimeoutAsync(150);
                attempt++;
            }

            throw new System.TimeoutException($"Could not find process '{processName}' inside the '{sectionText}' column after scrolling.");
        }


        public async Task ScrollToUserGroupsAsync1(string usergroupName, string sectionText = "User Groups", int maxScrollAttempts = 8)
        {
            // 1. Target the scroll container strictly inside the specific section column wrapper
            var scrollContainer = Page.Locator("div.flex-column, div.optionbox-group")
                .Filter(new() { Has = Page.Locator(".optionbox-header", new() { HasText = sectionText }) })
                .Locator(".nested-middle.optionlist.optionbox, .optionbox-content.optionlist");

            // 2. Target the specific row inside that isolated column
            var targetRow = scrollContainer.Locator(".optionbox.optionbox-option.optionbox-content")
                                .GetByText(usergroupName, new() { Exact = true });

            int attempt = 0;

            // Loop until the row is visible or we hit the maximum threshold
            while (attempt < maxScrollAttempts)
            {
                // First check if the targeted container itself is visible/ready
                if (await scrollContainer.CountAsync() == 1 && await targetRow.IsVisibleAsync())
                {
                    await targetRow.ScrollIntoViewIfNeededAsync();
                    return;
                }

                // Slowly scroll down by 100 pixels inside the correctly isolated container panel
                if (await scrollContainer.CountAsync() == 1)
                {
                    await scrollContainer.EvaluateAsync("el => el.scrollTop += 100");
                }

                await Page.WaitForTimeoutAsync(150);
                attempt++;
            }

            throw new System.TimeoutException($"Could not find User Group '{usergroupName}' inside the '{sectionText}' column after scrolling.");
        }

        public async Task ScrollToLinesAsync(string lineName, string sectionText = "Lines", int maxScrollAttempts = 8)
        {
            // 1. Target the scroll container strictly inside the specific section column wrapper
            var scrollContainer = Page.Locator("div.flex-column, div.optionbox-group")
                .Filter(new() { Has = Page.Locator(".optionbox-header", new() { HasText = sectionText }) })
                .Locator(".nested-middle.optionlist.optionbox, .optionbox-content.optionlist");

            // 2. Target the specific row inside that isolated column
            var targetRow = scrollContainer.Locator(".optionbox.optionbox-option.optionbox-content")
                                .GetByText(lineName, new() { Exact = true });

            int attempt = 0;

            // Loop until the row is visible or we hit the maximum threshold
            while (attempt < maxScrollAttempts)
            {
                // First check if the targeted container itself is visible/ready
                if (await scrollContainer.CountAsync() == 1 && await targetRow.IsVisibleAsync())
                {
                    await targetRow.ScrollIntoViewIfNeededAsync();
                    return;
                }

                // Slowly scroll down by 100 pixels inside the correctly isolated container panel
                if (await scrollContainer.CountAsync() == 1)
                {
                    await scrollContainer.EvaluateAsync("el => el.scrollTop += 100");
                }

                await Page.WaitForTimeoutAsync(150);
                attempt++;
            }

            throw new System.TimeoutException($"Could not find Line '{lineName}' inside the '{sectionText}' column after scrolling.");
        }

        protected async Task WaitAsync(int seconds = 3)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }


        protected async Task<IPage> NavigateToAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be empty.", nameof(url));

            if (!Uri.TryCreate(url, UriKind.Absolute, out var targetUri))
                throw new ArgumentException(
                    $"Invalid URL: {url}", nameof(url));

            // Check all currently open Edge tabs
            foreach (var page in Page.Context.Pages)
            {
                if (page.IsClosed)
                    continue;

                var currentUrl = page.Url;

                if (string.IsNullOrWhiteSpace(currentUrl))
                    continue;

                if (!Uri.TryCreate(currentUrl, UriKind.Absolute, out var currentUri))
                    continue;

                bool sameUrl = string.Equals(
                    currentUri.ToString().TrimEnd('/'),
                    targetUri.ToString().TrimEnd('/'),
                    StringComparison.OrdinalIgnoreCase);

                if (sameUrl)
                {
                    // Website already open → switch to that tab
                    await page.BringToFrontAsync();

                    // Make sure Edge window is maximized
                    await MaximizeEdgeWindowAsync(page);

                    Page = page;

                    return page;
                }
            }

            // URL is not open → create a new tab
            var newPage = await Page.Context.NewPageAsync();

            await newPage.GotoAsync(url);

            await newPage.BringToFrontAsync();

            // IMPORTANT:
            // Maximize the Edge window after opening the new tab
            await MaximizeEdgeWindowAsync(newPage);

            Page = newPage;

            return newPage;
        }
        private async Task MaximizeEdgeWindowAsync(IPage page)
        {
            var cdp = await page.Context.NewCDPSessionAsync(page);

            var windowInfo = await cdp.SendAsync("Browser.getWindowForTarget");

            if (windowInfo is not { ValueKind: JsonValueKind.Object } windowInfoValue ||
                !windowInfoValue.TryGetProperty("windowId", out var windowIdElement))
            {
                throw new InvalidOperationException(
                    "Could not determine the Edge window ID.");
            }

            int windowId = windowIdElement.GetInt32();

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
        }

    }
}