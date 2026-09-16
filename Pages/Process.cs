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
        public async Task SelectProcessAsync(string processName)
        {
            await WaitAsync();
            await ScrollToProcessAsync(processName);
            var processRow = Page.GetProcessRow(processName);
            await processRow.ClickAsync();
        }

        public async Task AddProcessStepsAsync(params string[] stepNames)
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

        public async Task SaveProcess(string processName)
        {
            var actionButton = Page.GetProcessRowActionButton(processName);
            await actionButton.ClickAsync();
            //Thread.Sleep(7000);
            var savebutton = Page.GetElementByText("Save");
            await savebutton.ClickAsync();
            await WaitAsync();
            var actionButton1 = Page.GetProcessRowActionButton(processName);
            await actionButton1.ClickAsync();
            await WaitAsync();
            Page.ClickElementWithTextAsync("Publish").Wait();
            //await Page.Publish().ClickAsync();
            await Page.Sync("processes").ClickAsync();
        }
    }
}
