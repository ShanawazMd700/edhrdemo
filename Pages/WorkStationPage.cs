using Microsoft.Playwright;
using PlaywrightDemo.Locators;

namespace PlaywrightDemo.Pages
{
    public class WorkStationPage : BasePage
    {
        public WorkStationPage(IPage page) : base(page)
        {
        }

        public async Task ValidateWorkstations(string id, string name)
        {
            var expectedText = $"{id}: {name}";

            Console.WriteLine($"Expected workstation: {expectedText}");

            Console.WriteLine($"Page URL: {Page.Url}");

            var bodyText = await Page.Locator("body").InnerTextAsync();

            Console.WriteLine("========== PAGE TEXT ==========");
            Console.WriteLine(bodyText);
            Console.WriteLine("================================");

            await Assertions.Expect(Page.Locator("body"))
                .ToContainTextAsync(expectedText);
        }
    }
}