using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Playwright;
using PlaywrightDemo.Locators;
using PlaywrightDemo.Support;

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

        private async Task OpenProcessActionsAndSaveAsync(string processName, string sectionText = "Processes")
        {
            await Page.GetProcessRowActionButton(processName, sectionText).ClickAsync();
            await Page.GetElementByText("Save").ClickAsync();
            await WaitAsync();
        }

        private async Task OpenProcessActionsAndPublishAsync(string processName, string sectionText = "Processes")
        {
            await Page.GetProcessRowActionButton(processName, sectionText).ClickAsync();
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
            await OpenProcessActionsAndSaveAsync(tabname, "User Groups");
            await OpenProcessActionsAndPublishAsync(tabname, "User Groups");
            await WaitAsync();
        }
        public async Task NavigateToTab(string tabName)
        {
            await Page.GetButtonTab(tabName).ClickAsync();
        }
        private async Task SaveLinesProcess(string processName)
        {
            var actionButton = Page.GetProcessRowActionButton(processName, "Lines");
            await actionButton.ClickAsync();
            var savebutton = Page.GetElementByText("Save");
            await savebutton.ClickAsync();
            await WaitAsync();
            var actionButton1 = Page.GetProcessRowActionButton(processName, "Lines");
            await actionButton1.ClickAsync();
            await WaitAsync();
            Page.ClickElementWithTextAsync("Publish").Wait();
            await Page.Sync1("Lines").ClickAsync();
        }
        public Task AddLinesAsync(string lineName) =>
            AddLinesAsync(lineName, lineName, 88, "1100");

        public async Task AddLinesAsync(string lineName, string displayName, int loginTimeoutInSeconds, string watsLocation)
        {
            await WaitAsync();

                var addButton = Page.GetAddButtonBy("Lines");
                await addButton.ClickAsync();

                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(lineName);

                var stepNameInput1 = Page.ProcessEditorField(1);
                await stepNameInput1.FillAsync(displayName);

                var stepNameInput2 = Page.ProcessEditorField(2);
                await stepNameInput2.FillAsync(loginTimeoutInSeconds.ToString());

                var stepNameInput3 = Page.ProcessEditorField(3);
                await stepNameInput3.FillAsync(watsLocation);

                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();
                await ScrollToLinesAsync(lineName);
                await SaveLinesProcess(lineName);

        }

        public async Task AddWorkStations(string lineName, params string[] workstationNames)
        {
            await AddWorkStations(
                lineName,
                workstationNames.Select(workstationName => new WorkstationDefinition
                {
                    Id = workstationName,
                    Name = workstationName
                }));
        }

        public async Task AddWorkStations(string lineName, IEnumerable<WorkstationDefinition> workstations)
        {
            await WaitAsync();
            await ScrollToLinesAsync(lineName);
            var lineRow = Page.GetProcessRow(lineName, "Lines"); 
            await lineRow.ClickAsync();
            await AddWorkStationsAsync(workstations);
            await SaveLinesProcess(lineName);
        }

        private async Task AddWorkStationsAsync(IEnumerable<WorkstationDefinition> workstations)
        {
            await WaitAsync();
            foreach (var workstation in workstations)
            {
                await Page.ClickAddButtonByAsync("Line Workstations");
                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(workstation.Id);
                var stepNameInput1 = Page.ProcessEditorField(1);
                await stepNameInput1.FillAsync(workstation.Name);
                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();

            }
        }
        public async Task SelectWorkStationAsync(string linename, string workstationName) // public
        {
            await ScrollToLinesAsync(linename);
            await Page.GetProcessRow(linename, "Lines").ClickAsync();
            await WaitAsync();
            await Page.GetProcessRow(linename, "Lines").ClickAsync();
            await WaitAsync();
            await Page.GetProcessRow(linename, "Lines").ClickAsync();
            await WaitAsync();
            await Page.ClickOptionRowAsync("Line Workstations", workstationName);
            await WaitAsync();
        }

        public async Task AddProcessStepsAsync(string linename, params string[] stepNames)
        {
            foreach (var stepName in stepNames)
            {
                await Page.ClickAddButtonByAsync("Process Steps");
                await Page.ProcessEditorField(0).FillAsync(stepName);
                await Page.SaveOrCheckButton().ClickAsync();
                
            }
            await SaveLinesProcess(linename); // Save/Publish after each step
        }

        //public async Task AddUserGroupAsync(string linename, string userGroupName)
        //{
        //    await Page.ClickAddButtonByAsync("User Groups");
        //    await Page.ProcessEditorField(0).FillAsync(userGroupName);
        //    await Page.SaveOrCheckButton("User Groups").ClickAsync();
        //    await SaveLinesProcess(linename); // Save/Publish after this addition
        //}
        public async Task AddUserGroupAsync(string linename, string userGroupName)
        {
            await Page.ClickAddButtonByAsync("User Groups");
            await Page.ProcessEditorField(0).FillAsync(userGroupName);
            await Page.SaveOrCheckButton().ClickAsync();
            await SaveLinesProcess(linename);
        }

        public async Task AddAssetAsync(
            string linename,
            string assetName,
            string assetType,
            string assetDisplayName,
            string serialNumber,
            bool displayAtWorkstation,
            DateTime expirationDate)
        {
            await Page.ClickAddButtonByAsync("Assets");

            await Page.ProcessEditorField(0).FillAsync(assetName);
            await Page.ProcessEditorField(1).FillAsync(assetType);
            await Page.ProcessEditorField(2).FillAsync(assetDisplayName);
            await Page.ProcessEditorField(3).FillAsync(serialNumber);
            await Page.ProcessEditorField(4).FillAsync(displayAtWorkstation.ToString().ToLower());
            await Page.ProcessEditorField(5).FillAsync(expirationDate.ToString("dd-MM-yyyy"));

            await Page.SaveOrCheckButton().ClickAsync();
            await SaveLinesProcess(linename); // Save/Publish after this addition
        }

        public async Task CreateConfigurationAsync(string configurationName)
        {
            var configuration = AdminAccessConfigurationLoader.LoadProcessConfiguration(configurationName);
            var process = configuration.ProcessCreation;
            var line = configuration.LineCreation;
            var processPage = new Process(Page);

            await ClickAddProcessAsync(process.Name, process.DisplayName);
            await processPage.CreateProcessStepsAsync(process.Name, process.Steps);

            foreach (var userGroup in AdminAccessConfigurationLoader.LoadUserGroups())
            {
                await AddUsergroup(userGroup.Name);
                await processPage.AddUsersToUserGroupAsync(userGroup.Name, userGroup.Users);
            }

            await NavigateToTab("Lines");
            await AddLinesAsync(
                line.Name,
                line.DisplayName,
                line.LoginTimeoutInSeconds,
                line.WatsLocation);
            await AddWorkStations(line.Name, line.Workstations);

            foreach (var workstation in line.Workstations)
            {
                if (workstation.ProcessStepNames.Count > 0)
                {
                    await SelectWorkStationAsync(line.Name, workstation.Name);
                    await AddProcessStepsAsync(line.Name, workstation.ProcessStepNames.ToArray());
                }

                foreach (var userGroup in workstation.UserGroups)
                {
                    await SelectWorkStationAsync(line.Name, workstation.Name);
                    await AddUserGroupAsync(line.Name, userGroup);
                }

                foreach (var asset in workstation.Assets)
                {
                    await SelectWorkStationAsync(line.Name, workstation.Name);
                    await AddAssetAsync(
                        line.Name,
                        asset.Name,
                        asset.Type,
                        asset.DisplayName,
                        asset.SerialNumber,
                        asset.DisplayAtWorkstation,
                        DateTime.ParseExact(asset.ExpirationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                }
            }
        }
    }
}
