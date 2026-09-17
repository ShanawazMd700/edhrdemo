using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightDemo.Locators;

namespace PlaywrightDemo.Pages
{
    public class AdministrationPage : BasePage
    {
        private const string AdministrationUrl = "https://app-order-tracker-eus-tst.azurewebsites.net/administration";
        //private readonly Process _process;
        public AdministrationPage(IPage page) : base(page)
        {
            //_process = new Process(page);
        }
        private async Task ScrollAndSelectAsync(string processName)
        {
            await WaitAsync();
            await ScrollToProcessAsync(processName);
            var processRow = Page.GetProcessRow(processName);
            await processRow.ClickAsync();
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
            await Page.ProcessEditorField(1).FillAsync(displayName); ////Process Creation
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
            await WaitAsync();
            await Page.ClickElementWithTextAsync("Publish");
        }
        public async Task AddUsergroup(string userGroupName)
        {
            await Page.GetButtonTab("User Groups").ClickAsync();
            await AddUserGroupName(userGroupName);
        }
        private async Task AddUserGroupName(string tabname)
        {
            await Page.GetPlusButtonByUserGroup("User Groups").ClickAsync();
            await Page.ProcessEditorField(0).FillAsync(tabname);
            await Page.SaveOrCheckButton().ClickAsync();
            await ScrollToUserGroupsAsync1(tabname);
            await OpenProcessActionsAndSaveAsync(tabname);
            await OpenProcessActionsAndPublishAsync(tabname);
            await WaitAsync();
        }
        public async Task NavigateToTab(string tabName)
        {
            await Page.GetButtonTab(tabName).ClickAsync();
        }
        private async Task SaveLinesProcess(string processName)
        {
            var actionButton = Page.GetProcessRowActionButton(processName);
            await actionButton.ClickAsync();
            var savebutton = Page.GetElementByText("Save");
            await savebutton.ClickAsync();
            await WaitAsync();
            var actionButton1 = Page.GetProcessRowActionButton(processName);
            await actionButton1.ClickAsync();
            await WaitAsync();
            Page.ClickElementWithTextAsync("Publish").Wait();
            await Page.Sync1("Lines").ClickAsync();
        }
        public async Task AddLinesAsync( string linename)
        {
            await WaitAsync();

                var addButton = Page.GetAddButtonBy("Lines");
                await addButton.ClickAsync();

                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(linename);

                var stepNameInput1 = Page.ProcessEditorField(1);
                await stepNameInput1.FillAsync(linename);

                var stepNameInput2 = Page.ProcessEditorField(2);
                await stepNameInput2.FillAsync("88");

                var stepNameInput3 = Page.ProcessEditorField(3);
                await stepNameInput3.FillAsync("1100");

                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();
                await ScrollToLinesAsync(linename);
                await SaveLinesProcess(linename);

        }

        public async Task AddWorkStations(string ws1, string ws2, string linename)
        {
            await WaitAsync();
            await ScrollToLinesAsync(linename);
            var lineRow = Page.GetProcessRow(linename); 
            await lineRow.ClickAsync();
            await Page.GetAddButtonBy1("Line Workstations").ClickAsync();
            await AddWorkStationsAsync(ws1, ws2);
            await SaveLinesProcess(linename);
        }

        private async Task AddWorkStationsAsync(params string[] linenames)
        {
            await WaitAsync();
            foreach (var line in linenames)
            {
                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(line);
                var stepNameInput1 = Page.ProcessEditorField(1);
                await stepNameInput1.FillAsync(line);
                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();

            }
        }
    }
}
