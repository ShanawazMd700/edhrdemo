using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightDemo.Locators;

namespace PlaywrightDemo.Pages
{
    public class AdministrationPage : BasePage
    {
        private const string AdministrationUrl = "https://app-order-tracker-eus-tst.azurewebsites.net/administration";

        public AdministrationPage(IPage page) : base(page)
        {
        }

        public async Task GoToAsync()
        {
            await Page.GotoAsync(AdministrationUrl);
            await Page.AddButton(0).WaitForAsync(new() { State = WaitForSelectorState.Visible });
        }

        public async Task ClickAddProcessAsync(string processName, string displayName)
        {
            await Page.AddButton(0).ClickAsync();
            await Page.ProcessEditorField(0).FillAsync(processName);
            await Page.ProcessEditorField(1).FillAsync(displayName);
            await Page.SaveOrCheckButton().ClickAsync();
            await ScrollToProcessAsync(processName);
            await OpenProcessActionsAndSaveAsync(processName);
            await OpenProcessActionsAndPublishAsync(processName);
        }

        private async Task OpenProcessActionsAndSaveAsync(string processName)
        {
            await Page.GetProcessRowActionButton(processName).ClickAsync();
            await Page.GetElementByText("Save").ClickAsync();
            await WaitAsync();
        }

        private async Task OpenProcessActionsAndPublishAsync(string processName)
        {
            await Page.GetProcessRowActionButton(processName).ClickAsync();
            await Page.ClickElementWithTextAsync("Publish");
        }

        
    }
}
