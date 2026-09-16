using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightDemo.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; }

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

        protected async Task WaitAsync(int seconds = 3)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }
    }
}
