using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Playwright;
using PlaywrightDemo.Locators;

namespace PlaywrightDemo.Pages
{
    public class Process : BasePage
    {
        public Process(IPage page) : base(page)
        {
        }
        private async Task AddProcessStepsAsync(params string[] stepNames)
        {
            await WaitAsync();
            foreach (var stepName in stepNames)
            {
                var addButton = Page.AddButton(1); // Second Add button is for Process Steps
                await addButton.ClickAsync();

                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(stepName);

                var stepDisplayNameInput = Page.ProcessEditorField(1);
                await stepDisplayNameInput.FillAsync(stepName); // same value for name/display name

                var optionalDescriptionInput = Page.ProcessEditorField(2);
                await optionalDescriptionInput.FillAsync("true");

                var dhrStepInput = Page.ProcessEditorField(3);
                await dhrStepInput.FillAsync("true");
                
                var watsProcessInput = Page.ProcessEditorField(4);
                await watsProcessInput.FillAsync("45");

                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();

            }        
        }
        private async Task AddLineDetailsAsync(params string[] linenames)
        {
            await WaitAsync();
            foreach (var line in linenames)
            {
                var addButton = Page.GetAddButtonBy("Lines"); // Second Add button is for Process Steps
                await addButton.ClickAsync();

                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(line);

                var stepNameInput1 = Page.ProcessEditorField(1);
                await stepNameInput1.FillAsync(line);

                var stepNameInput2 = Page.ProcessEditorField(2);
                await stepNameInput2.FillAsync(line);

                var stepNameInput3 = Page.ProcessEditorField(3);
                await stepNameInput3.FillAsync(line);

                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();

            }
        }
        private async Task SaveProcess(string processName)
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
            await Page.Sync("processes").ClickAsync();
        }
        private async Task SaveUserGroups(string processName)
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
            await Page.Sync1("User Groups").ClickAsync();
        }
        private async Task SaveLines(string processName)
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

        private async Task AddUserEmailAsync(params string[] emailAddresses)
        {
            await WaitAsync();
            foreach (var email in emailAddresses)
            {
                var addButton = Page.GetAddButtonBy("User Email ID"); 
                await addButton.ClickAsync();

                var stepNameInput = Page.ProcessEditorField(0);
                await stepNameInput.FillAsync(email);

                var saveButton = Page.SaveOrCheckButton();
                await saveButton.ClickAsync();

            }
        }

        private async Task SelectProcessAsync(string processName, string sectionText = "Processes")
        {
            await WaitAsync();
            await ScrollToProcessAsync(processName, sectionText);
            var processRow = Page.GetProcessRow(processName);
            await processRow.ClickAsync();
        }
        public async Task AddingEmailID(string emailID1, string emailID2, string emailID3, string UserGroupName)
        {
            await SelectProcessAsync(UserGroupName, sectionText: "User Groups");
            await AddUserEmailAsync(emailID1, emailID2, emailID3);
            await SaveUserGroups(UserGroupName);
        }

        public async Task CreateProcessSteps(string step1, string step2, string step3, string processName)
        {
            await SelectProcessAsync(processName);
            await AddProcessStepsAsync(step1, step2, step3);
            await SaveProcess(processName);
        }
       
        //private async Task AddLinesName(string tabname)
        //{
        //    await Page.GetPlusButtonByUserGroup("Lines").ClickAsync();
        //    await Page.ProcessEditorField(0).FillAsync(tabname);
        //    await Page.SaveOrCheckButton().ClickAsync();
        //    await ScrollToUserGroupsAsync1(tabname);
        //    await OpenProcessActionsAndSaveAsync(tabname);
        //    await OpenProcessActionsAndPublishAsync(tabname);
        //    await WaitAsync();
        //}
        public async Task AddToLines(string line1, string line2, string line3, string linename)
        {
            await SelectProcessAsync(linename, sectionText: "Lines");
            await AddLineDetailsAsync(line1, line2, line3);
            await SaveLines(linename);
        }
        


    }
}
