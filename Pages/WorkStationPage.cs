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
            await Assertions.Expect(Page.WorkstationDisplay())
                .ToHaveTextAsync($"{id}: {name}");
        }
    }
}