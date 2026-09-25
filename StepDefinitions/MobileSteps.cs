using System;
using System.Collections.Generic;
using System.Text;
using MobileAppium.Actions;
using MobileAppium.Drivers;

namespace PlaywrightDemo.StepDefinitions
{
    [Binding]
    public class MobileSteps
    {
        private readonly AppiumDriverManager _driverManager;
        private readonly MobileActions _mobileActions;

        public MobileSteps()
        {
            _driverManager = new AppiumDriverManager();

            _driverManager.StartDriver();

            _mobileActions = new MobileActions(_driverManager);
        }

        [When("I open the QR code {string}")]
        public void WhenIOpenTheQRCode(string qrcode)
        {
            _mobileActions.SelectQrCode(qrcode);
        }

        [When("I open the QR code {string} of {string}")]
        public void WhenIOpenTheQRCodeOf(string qrcode, string folderName)
        {
            _mobileActions.SelectQrCode1(qrcode, folderName);
        }


    }
}
