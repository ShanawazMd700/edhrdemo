using Microsoft.Playwright;
using PlaywrightDemo.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace PlaywrightDemo.Locators
{
    public static class Locators
    {
        // Locates the entire row container so you can click the Process Name text
        public static ILocator GetProcessRow(this IPage page, string processName) =>
            page.Locator("div.optionbox-option.optionbox-content")
                .Filter(new() { HasText = processName });

        public static ILocator GetProcessRow(this IPage page, string processName, string sectionText)
        {
            ArgumentNullException.ThrowIfNull(page);
            ArgumentException.ThrowIfNullOrWhiteSpace(processName);
            ArgumentException.ThrowIfNullOrWhiteSpace(sectionText);

            return page.Locator("div.flex-column, div.optionbox-group")
                .Filter(new() { Has = page.Locator(".optionbox-header").GetByText(sectionText, new() { Exact = true }) })
                .Locator(".nested-middle.optionlist.optionbox, .optionbox-content.optionlist")
                .Locator("div.optionbox-option.optionbox-content")
                .Filter(new() { Has = page.GetByText(processName, new() { Exact = true }) });
        }
        public static ILocator ClickProcess(this IPage page, string processName) =>
            (ILocator)page.GetProcessRow(processName).ClickAsync();
        // Locates the specific action button (usually the triple-dot/ellipsis) inside that row
        public static ILocator GetProcessRowActionButton(this IPage page, string processName) =>
            page.GetProcessRow(processName)
                .Locator("button");

        public static ILocator GetProcessRowActionButton(this IPage page, string processName, string sectionText) =>
            page.GetProcessRow(processName, sectionText)
                .Locator("button");
        


        // Locates a menu option (Duplicate / Download / Archive) after the dropdown opens
        public static ILocator GetContextMenuOption(this IPage page, string optionText) =>
            page.Locator("div.option-context-menu")
                .Locator("button.menu-action")
                .GetByText(optionText, new() { Exact = false }); 
        // Exact = false allows it to match despite extra spaces like " Duplicate "

        // Example usage:
        //await page.AdminConfigEllipsisButton("ProcessName").ClickAsync();
        //await page.GetContextMenuOption("Duplicate").ClickAsync();
        //Or use the below locators for Add, Edit, and Delete buttons based on section text
        public static ILocator AdminConfigMenuOption1(this IPage page, string sectionText) =>
           page.Locator("menu-action").Filter(new() { HasText = sectionText });

        // Passing an integer index directly (0 for the first plus button, 1 for the second, etc.)
        public static ILocator AddButton(this IPage page, int index) =>
            page.Locator("button:has(i.fa-plus-square-o)").Nth(index);
        public static ILocator GetPlusButtonByUserGroup(this IPage page, string groupName) =>
    page.Locator("div.optionbox-group")
        .Filter(new() { Has = page.Locator("div.optionbox-header", new() { HasText = groupName }) })
        .Locator("i.fa-plus-square-o");

       


        public static ILocator AddButton1(this IPage page, string sectionText) =>
    page.Locator("div.optionbox-group, div.flex-column")
        .Filter(new() { Has = page.Locator(".optionbox-header", new() { HasText = sectionText }) })
        .Locator("button:has(i.fa-plus-square-o)");

        public static ILocator EditButton1(this IPage page, string sectionText) =>
            page.Locator("div.optionbox-group, div.flex-column")
                .Filter(new() { Has = page.Locator(".optionbox-header", new() { HasText = sectionText }) })
                .Locator("button:has(i.fa-pencil-square-o)");

        public static ILocator DeleteButton1(this IPage page, string sectionText) =>
            page.Locator("div.optionbox-group, div.flex-column")
                .Filter(new() { Has = page.Locator(".optionbox-header", new() { HasText = sectionText }) })
                .Locator("button:has(i.fa-trash-can)");

        // Example usage:
        //await page.AddButton("Processes").ClickAsync();
        //await page.EditButton("Processes").ClickAsync();
        //await page.DeleteButton("Processes").ClickAsync();
        //*************************************************************************************************************************************

        // Locates the first button (Save/Check) in the footer button group under the header
        // Finds a button containing an action indicator or standard text within footer spaces
        // 1. FIXED: Added the dot and removed the space between the classes
        public static ILocator SaveOrCheckButton(this IPage page, int index) =>
            page.Locator(".action-button.button-default").Nth(index);

        // 2. FIXED: Added the missing dot and removed the space here as well
        public static ILocator SaveOrCheckButton(this IPage page) =>
            page.Locator(".fa-solid.fa-check-circle");
        public static ILocator SaveOrCheckButton(this IPage page, string sectionText) =>
        page.Locator("div.optionbox-group, div.flex-column")
        .Filter(new() { Has = page.Locator(".optionbox-header").GetByText(sectionText, new() { Exact = true }) })
        .Locator(".fa-solid.fa-check-circle")
        .First;
        public static async Task ClickSaveOrCheckButtonByAsync(this IPage page, string groupName)
        {
            var clicked = await page.EvaluateAsync<bool>(@"(groupName) => {
        const isVisible = (el) => {
            const rect = el.getBoundingClientRect();
            const style = window.getComputedStyle(el);
            return rect.width > 0 && rect.height > 0 && style.display !== 'none' && style.visibility !== 'hidden';
        };

        const getRootId = (el) => {
            let cur = el;
            while (cur) {
                if (cur.id) return cur.id;
                cur = cur.parentElement;
            }
            return null;
        };

        const topLevelTabIds = ['processes', 'userGroups', 'lines', 'validation'];

        const headers = Array.from(document.querySelectorAll('div.optionbox-header'))
            .filter(h => h.textContent.trim() === groupName)
            .filter(isVisible)
            .filter(h => !(topLevelTabIds.includes(getRootId(h)) && getRootId(h).toLowerCase() === groupName.replace(/\s+/g,'').toLowerCase()));

        if (headers.length === 0) return false;
        const header = headers[headers.length - 1];

        // Search forward through siblings for the check-circle save button
        let sibling = header.parentElement.nextElementSibling;
        while (sibling) {
            const check = sibling.querySelector('.fa-solid.fa-check-circle');
            if (check && isVisible(check)) {
                check.closest('button')?.click() ?? check.click();
                return true;
            }
            sibling = sibling.nextElementSibling;
        }
        return false;
    }", groupName);

            if (!clicked)
            {
                throw new InvalidOperationException($"Could not find a VISIBLE nested Save button for section '{groupName}'");
            }
        }
        public static ILocator CancelButton(this IPage page, string sectionText) =>
            page.Locator("div.btn-group button, button.action-button").Nth(1);

        //Editor form input fields
        public static ILocator ProcessEditorField(this IPage page, int index) =>
            page.Locator(".editor-input").Nth(index);
        //// 1. Fill Process Name
        //await page.GetFormFieldInput("Editor", "Process Name").FillAsync("My New Process");
        //// 2. Fill Process Display Name
        //await page.GetFormFieldInput("Editor", "Process Display Name").FillAsync("Display Process")

        public static ILocator GetElementByText(this IPage page, string text, bool exact = false) =>
            page.GetByText(text, new() { Exact = exact });

        public static async Task ClickElementWithTextAsync(this IPage page, string text, bool exact = false)
        {
            await page.GetElementByText(text, exact).ClickAsync();
        }

        //fa-solid fa-publish
        public static ILocator Sync(this IPage page, string sectionId) =>
            page.Locator($"#{sectionId} i.fa-solid.fa-rotate");
        public static ILocator Sync1(this IPage page, string sectionText) =>
            page.Locator("div.optionbox-group, div.flex-column")
            .Filter(new() { Has = page.Locator(".optionbox-header", new() { HasText = sectionText }) })
            .Locator("button:has(i.fa-solid.fa-rotate)");

        public static ILocator GetButtonTab(this IPage page, string tabName) =>
            page.Locator("button.action-button.button-tab")
            .Filter(new() { HasText = tabName });


        public static ILocator GetAddButtonBy(this IPage page, string groupName) =>
            page.Locator("div.optionbox-group")
            .Filter(new() { Has = page.Locator("div.optionbox-header", new() { HasText = groupName }) })
            .Locator("i.fa-plus-square-o");
        public static async Task ClickAddButtonByAsync(this IPage page, string groupName)
        {
            var clicked = await page.EvaluateAsync<bool>(@"(groupName) => {
        const isVisible = (el) => {
            const rect = el.getBoundingClientRect();
            const style = window.getComputedStyle(el);
            return rect.width > 0 && rect.height > 0 && style.display !== 'none' && style.visibility !== 'hidden';
        };

        const getRootId = (el) => {
            let cur = el;
            while (cur) {
                if (cur.id) return cur.id;
                cur = cur.parentElement;
            }
            return null;
        };

        const headers = Array.from(document.querySelectorAll('div.optionbox-header'))
            .filter(h => h.textContent.trim() === groupName)
            .filter(isVisible)
            // Exclude the top-level tab sections (processes/userGroups/lines/validation) —
            // we want the NESTED panel under the currently selected item, which lives
            // inside a different top-level tab (e.g. 'lines') than its own name suggests.
            .filter(h => getRootId(h) !== groupName.replace(/\s+/g, '').replace(/^./, c => c.toLowerCase()));

        if (headers.length === 0) return false;
        const header = headers[headers.length - 1];

        let sibling = header.parentElement.nextElementSibling;
        while (sibling) {
            const icon = sibling.querySelector('i.fa-plus-square-o');
            if (icon && isVisible(icon)) {
                icon.closest('button')?.click() ?? icon.click();
                return true;
            }
            sibling = sibling.nextElementSibling;
        }
        return false;
    }", groupName);

            if (!clicked)
            {
                throw new InvalidOperationException($"Could not find a VISIBLE nested Add button for section '{groupName}'");
            }
        }

        public static ILocator GetOptionListBySection(this IPage page, string sectionText) =>
        page.Locator("div.optionbox.backgroundcolor-orange")
        .Filter(new() { Has = page.Locator("div.optionbox-header").GetByText(sectionText, new() { Exact = true }) })
        .Locator("xpath=following-sibling::div[contains(@class,'nested-middle') and contains(@class,'optionlist')][1]");

        public static ILocator GetOptionRow(this IPage page, string sectionText, string optionText) =>
            page.GetOptionListBySection(sectionText)
                .Locator(".optionbox-option")
                .GetByText(optionText, new() { Exact = true });

        public static async Task ClickOptionRowAsync(this IPage page, string sectionText, string optionText)
        {
            
            await page.GetOptionRow(sectionText, optionText).ClickAsync();

        }
    }
}
