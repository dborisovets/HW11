using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Xml.Linq;

namespace HW11
{
    public class Tests
    {
        private IWebDriver _driver;
        private Actions _driverActions; //skip the add
        private WebDriverWait _driverWait;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driverActions = new Actions(_driver);
            _driverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));
        }

        [Test]
        public void Buttons()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/buttons");

            const string doubleClickmeButtonMessage = "You have done a double click";
            var doubleClickmeButton = _driver.FindElement(By.Id("doubleClickBtn"));
            _driverActions.MoveToElement(doubleClickmeButton).DoubleClick().Perform();

            const string rightClickmeButtonMessage = "You have done a right click";
            var rightClickmeButton = _driver.FindElement(By.Id("rightClickBtn"));
            _driverActions.MoveToElement(rightClickmeButton).ContextClick().Perform();

            const string clickmeButtonMessage = "You have done a dynamic click";
            var clickmeButton = _driver.FindElement(By.XPath("//button[@id='rightClickBtn']/parent::div/following-sibling::div/button"));
            clickmeButton.Click();

            var doubleClickmeButtonResult = _driver.FindElement(By.XPath("//p[@id='doubleClickMessage']"));
            Assert.AreEqual(doubleClickmeButtonResult.Text, doubleClickmeButtonMessage);

            var rightClickmeButtonResult = _driver.FindElement(By.Id("rightClickMessage"));
            Assert.AreEqual(rightClickmeButtonResult.Text, rightClickmeButtonMessage);

            var clickmeButtonResult = _driver.FindElement(By.Id("dynamicClickMessage"));
            Assert.AreEqual(clickmeButtonResult.Text, clickmeButtonMessage);
        }

        [Test]
        public void Checkbox()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/checkbox");
            var homeCheckbox = _driver.FindElement(By.XPath("//input[@id='tree-node-home']/following-sibling::span[@class='rct-checkbox']"));
         
            Assert.IsTrue(homeCheckbox.Displayed);
            
            var arrowCheckbox = _driver.FindElement(By.XPath("//button[@title='Toggle']"));

            arrowCheckbox.Click(); //Expand Home
            By byDesktopCheckbox = By.XPath("//label[@for='tree-node-desktop']//span[@class='rct-checkbox']");
            By byDocumentsCheckbox = By.XPath("//label[@for='tree-node-documents']//span[@class='rct-checkbox']");
            By byDownloadsCheckbox = By.XPath("//label[@for='tree-node-downloads']//span[@class='rct-checkbox']");
            By byHomeCheckbox = By.XPath("//input[@id='tree-node-home']/following-sibling::span[@class='rct-checkbox']");
            var desktopCheckbox = _driver.FindElement(byDesktopCheckbox);
            var documentsCheckbox = _driver.FindElement(byDocumentsCheckbox);
            var downloadsCheckbox = _driver.FindElement(byDownloadsCheckbox);
            Assert.IsTrue(desktopCheckbox.Displayed);
            Assert.IsTrue(documentsCheckbox.Displayed);
            Assert.IsTrue(downloadsCheckbox.Displayed);

            arrowCheckbox.Click(); //close home checkbox

            Assert.IsTrue(IsElementPresent(byHomeCheckbox));
            Assert.IsFalse(IsElementPresent(byDesktopCheckbox));
            Assert.IsFalse(IsElementPresent(byDocumentsCheckbox));
            Assert.IsFalse(IsElementPresent(byDownloadsCheckbox));

            homeCheckbox.Click();

            IWebElement selectedText = _driver.FindElement(By.XPath("//*[@id='result']"));
            string expectedResult = "You have selected : home desktop notes commands documents workspace react angular veu office public private classified general downloads wordFile excelFile";
            string textFromElement = selectedText.Text.Replace("\r\n", " ");
            Assert.AreEqual(expectedResult, textFromElement);
        }               

        [Test]
        public void RadioButton()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/radio-button");

            var yesRadioButton = _driver.FindElement(By.XPath("//label[@class='custom-control-label' and @for='yesRadio']"));
            var noRadioButton = _driver.FindElement(By.Id("noRadio"));
            var impressiveRadioButton = _driver.FindElement(By.XPath("//label[@class='custom-control-label' and @for='impressiveRadio']"));
            Assert.IsTrue(yesRadioButton.Enabled);
            Assert.IsFalse(noRadioButton.Enabled);
            Assert.IsTrue(impressiveRadioButton.Enabled);

            yesRadioButton.Click();
            var selectedYes = _driver.FindElement(By.XPath("//*[@id='yesRadio']"));
            Assert.IsTrue(selectedYes.Selected);

            var selectedYesResult = _driver.FindElement(By.CssSelector("[class='mt-3']")).Text;
            Assert.AreEqual("You have selected Yes", selectedYesResult);

            impressiveRadioButton.Click();
            var selectedImpresive = _driver.FindElement(By.XPath("//*[@id='impressiveRadio']"));
            Assert.IsTrue(selectedImpresive.Selected);
            Assert.IsFalse(selectedYes.Selected);

            var selectedImpresiveResult = _driver.FindElement(By.CssSelector("[class='mt-3']")).Text;
            Assert.AreEqual("You have selected Impressive", selectedImpresiveResult);
        }

        [Test]
        public void WebTables()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/webtables");

            const string Error_Border_Color = "rgb(220, 53, 69)";
            const string Success_Border_Color = "rgb(40, 167, 69)";
            var table = _driver.FindElement(By.XPath("//*[contains(@class, 'ReactTable')]"));
            var searchBox = _driver.FindElement(By.Id("searchBox"));
            var addButton = _driver.FindElement(By.Id("addNewRecordButton"));

            Assert.IsTrue(table.Displayed); //expected result of 1 
            Assert.IsTrue(searchBox.Displayed); //expected result of 1 
            Assert.IsTrue(addButton.Displayed); //expected result of 1 

            addButton.Click(); // step 2

            var regForm = _driver.FindElement(By.Id("registration-form-modal"));

            WaitUntilDisplayed(regForm);

            Assert.IsTrue(regForm.Displayed); // exp result 2

            var firstNameField = _driver.FindElement(By.Id("firstName"));
            var lastNameField = _driver.FindElement(By.Id("lastName"));
            var emailField = _driver.FindElement(By.Id("userEmail"));
            var ageField = _driver.FindElement(By.Id("age"));
            var salaryField = _driver.FindElement(By.Id("salary"));
            var departmentField = _driver.FindElement(By.Id("department"));

            Assert.IsTrue(firstNameField.Displayed); // exp result 2
            Assert.IsTrue(lastNameField.Displayed); // exp result 2
            Assert.IsTrue(emailField.Displayed); // exp result 2
            Assert.IsTrue(ageField.Displayed); // exp result 2
            Assert.IsTrue(salaryField.Displayed); // exp result 2
            Assert.IsTrue(departmentField.Displayed); // exp result 2

            var submitButton = _driver.FindElement(By.Id("submit"));
            submitButton.Click(); // step 2

            IWebElement firstNameFieldError = _driver.FindElement(By.XPath("//input[@id='firstName']"));

            _driverWait.Until(drv => firstNameFieldError.GetCssValue("border-color").Contains(Error_Border_Color));

            string firstNameborderColor = firstNameFieldError.GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, firstNameborderColor);

            IWebElement lastNameFieldError = _driver.FindElement(By.XPath("//input[@id='lastName']"));
            string lastNameborderColor = lastNameFieldError.GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, lastNameborderColor);

            IWebElement emailFieldError = _driver.FindElement(By.XPath("//input[@id='userEmail']"));
            string emailborderColor = emailFieldError.GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, emailborderColor);

            IWebElement ageFieldError = _driver.FindElement(By.XPath("//input[@id='age']"));
            string ageborderColor = ageFieldError.GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, ageborderColor);

            IWebElement salaryFieldError = _driver.FindElement(By.XPath("//input[@id='salary']"));
            string salaryborderColor = salaryFieldError.GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, salaryborderColor);

            IWebElement departmentFieldError = _driver.FindElement(By.XPath("//input[@id='department']"));
            string departmentborderColor = departmentFieldError.GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, departmentborderColor);

            var closeButton = _driver.FindElement(By.XPath("//button[@class='close']"));
            closeButton.Click();
            
            addButton.Click();

           _driverWait.Until(drv => drv.FindElements(By.Id("registration-form-modal")).Count > 0);

            _driver.FindElement(By.Id("firstName")).SendKeys("Adam");
            _driver.FindElement(By.Id("lastName")).SendKeys("Smith");
            _driver.FindElement(By.Id("userEmail")).SendKeys("test");
            _driver.FindElement(By.Id("age")).SendKeys("29");
            _driver.FindElement(By.Id("salary")).SendKeys("3000");
            _driver.FindElement(By.Id("department")).SendKeys("test");
            _driver.FindElement(By.Id("submit")).Click();

            IWebElement firstNameFieldSuccess = _driver.FindElement(By.XPath("//input[@id='firstName']"));

            _driverWait.Until(drv => firstNameFieldSuccess.GetCssValue("border-color").Contains(Success_Border_Color));

            string firstNameborderColorSuccess = firstNameFieldSuccess.GetCssValue("border-color");
            Assert.AreEqual(Success_Border_Color, firstNameborderColorSuccess);

            IWebElement lastNameFieldSuccess = _driver.FindElement(By.XPath("//input[@id='lastName']"));
            string lastNameborderSuccess = lastNameFieldSuccess.GetCssValue("border-color");
            Assert.AreEqual(Success_Border_Color, lastNameborderSuccess);

            _driver.FindElement(By.XPath("//input[@id='userEmail']")).GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, emailborderColor);

            IWebElement ageFieldSuccess = _driver.FindElement(By.XPath("//input[@id='age']"));
            string ageborderColorSuccess = ageFieldSuccess.GetCssValue("border-color");
            Assert.AreEqual(Success_Border_Color, ageborderColorSuccess);

            IWebElement salaryFieldSuccess = _driver.FindElement(By.XPath("//input[@id='salary']"));
            string salaryborderColorSuccess = salaryFieldSuccess.GetCssValue("border-color");
            Assert.AreEqual(Success_Border_Color, salaryborderColorSuccess);

            IWebElement departmentFieldSuccess = _driver.FindElement(By.XPath("//input[@id='department']"));
            string departmentborderColorSuccess = departmentFieldSuccess.GetCssValue("border-color");
            Assert.AreEqual(Success_Border_Color, departmentborderColorSuccess);

            _driver.FindElement(By.Id("userEmail")).SendKeys("test@test.com");
            _driver.FindElement(By.Id("age")).SendKeys("-2");

            IWebElement emailFieldSuccess = _driver.FindElement(By.XPath("//input[@id='userEmail']"));

            _driverWait.Until(drv => emailFieldSuccess.GetCssValue("border-color").Contains(Success_Border_Color));

            string emailborderColorSuccess = emailFieldSuccess.GetCssValue("border-color");
            Assert.AreEqual(Success_Border_Color, emailborderColorSuccess);

            _driver.FindElement(By.XPath("//input[@id='age']")).GetCssValue("border-color");
            Assert.AreEqual(Error_Border_Color, emailborderColor);

            _driver.FindElement(By.Id("age")).SendKeys("29");
            _driver.FindElement(By.Id("submit")).Click();

            _driver.FindElement(By.Id("searchBox")).SendKeys("Adam");

            List<IWebElement> gridRows = _driver.FindElements(By.XPath("//div[@class='rt-tbody']")).ToList();

            List<IWebElement> filteredRows = gridRows
                .Where(row => row.FindElement(By.XPath("//div[@class='rt-td']")).Text == "Adam")
                .ToList();

            Assert.AreEqual(1, filteredRows.Count);
        }

        [Test]
        public void Links()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/links");
            _driverActions.SendKeys(Keys.PageDown).Perform();

            IWebElement homeLink = _driver.FindElement(By.Id("simpleLink"));
            IWebElement homehZvluLink = _driver.FindElement(By.XPath("//*[@id='dynamicLink']"));
            IWebElement createdLink = _driver.FindElement(By.Id("created"));
            IWebElement noContentLink = _driver.FindElement(By.Id("no-content"));
            IWebElement  movedLink = _driver.FindElement(By.Id("moved"));
            IWebElement badRequestLink = _driver.FindElement(By.XPath("//*[@id='bad-request']"));
            IWebElement unauthorizedLink = _driver.FindElement(By.Id("unauthorized"));
            IWebElement forbiddenLink = _driver.FindElement(By.Id("forbidden"));
            IWebElement notFoundLink = _driver.FindElement(By.Id("invalid-url"));

            Assert.IsTrue(homeLink.Displayed);
            Assert.IsTrue(homehZvluLink.Displayed);
            Assert.IsTrue(createdLink.Displayed);
            Assert.IsTrue(noContentLink.Displayed);
            Assert.IsTrue(movedLink.Displayed);
            Assert.IsTrue(badRequestLink.Displayed);
            Assert.IsTrue(unauthorizedLink.Displayed);
            Assert.IsTrue(forbiddenLink.Displayed);
            Assert.IsTrue(notFoundLink.Displayed);

            homeLink.Click();
            IReadOnlyCollection<string> windowHandles = _driver.WindowHandles;
            _driver.SwitchTo().Window(windowHandles.Last());
            
            const string demoQaUrl = "https://demoqa.com/";
            Assert.AreEqual(demoQaUrl, _driver.Url);

            _driver.SwitchTo().Window(windowHandles.First());

            homehZvluLink.Click();
            windowHandles = _driver.WindowHandles;
            _driver.SwitchTo().Window(windowHandles.Last());
            Assert.AreEqual(demoQaUrl, _driver.Url);

            _driver.SwitchTo().Window(windowHandles.First());

            createdLink.Click();

            _driverWait.Until(drv => drv.FindElements(By.XPath("//*[@id='linkResponse']")).Count > 0);
            
            var createdLinkStatusCode = _driver.FindElement(LocatorByXPath("201"));
            var createdLinkStatusText = _driver.FindElement(LocatorByXPath("Created"));

            Assert.AreEqual("201", createdLinkStatusCode.Text);
            Assert.AreEqual("Created", createdLinkStatusText.Text);

            noContentLink.Click();

            _driverWait.Until(drv => drv.FindElements(LocatorByXPath("204")).Count > 0);

            var noContentLinkStatusCode = _driver.FindElement(LocatorByXPath("204"));
            var noContentLinkStatusText = _driver.FindElement(LocatorByXPath("No Content"));

            Assert.AreEqual("204", noContentLinkStatusCode.Text);
            Assert.AreEqual("No Content", noContentLinkStatusText.Text);

            movedLink.Click();

            _driverWait.Until(drv => drv.FindElements(LocatorByXPath("301")).Count > 0);

            var movedLinkStatusCode = _driver.FindElement(LocatorByXPath("301"));
            var movedLinkStatusText = _driver.FindElement(LocatorByXPath("Moved Permanently")); 

            Assert.AreEqual("301", movedLinkStatusCode.Text);
            Assert.AreEqual("Moved Permanently", movedLinkStatusText.Text);

            badRequestLink.Click();

            _driverWait.Until(drv => drv.FindElements(LocatorByXPath("400")).Count > 0);

            var badRequestLinkStatusCode = _driver.FindElement(LocatorByXPath("400"));
            var badRequestLinkStatusText = _driver.FindElement(LocatorByXPath("Bad Request"));

            Assert.AreEqual("400", badRequestLinkStatusCode.Text);
            Assert.AreEqual("Bad Request", badRequestLinkStatusText.Text);

            unauthorizedLink.Click();

            _driverWait.Until(drv => drv.FindElements(LocatorByXPath("401")).Count > 0);

            var unauthorizedLinkStatusCode = _driver.FindElement(LocatorByXPath("401"));
            var unauthorizedLinkStatusText = _driver.FindElement(LocatorByXPath("Unauthorized"));

            Assert.AreEqual("401", unauthorizedLinkStatusCode.Text);
            Assert.AreEqual("Unauthorized", unauthorizedLinkStatusText.Text);

            forbiddenLink.Click();

            _driverWait.Until(drv => drv.FindElements(LocatorByXPath("403")).Count > 0);

            var forbiddenLinkStatusCode = _driver.FindElement(LocatorByXPath("403"));
            var forbiddenLinkStatusText = _driver.FindElement(LocatorByXPath("Forbidden"));

            Assert.AreEqual("403", forbiddenLinkStatusCode.Text);
            Assert.AreEqual("Forbidden", forbiddenLinkStatusText.Text);

            notFoundLink.Click();

            _driverWait.Until(drv => drv.FindElements(LocatorByXPath("404")).Count > 0);

            var notFoundLinkStatusCode = _driver.FindElement(LocatorByXPath("404"));
            var notFoundLinkStatusText = _driver.FindElement(LocatorByXPath("Not Found"));

            Assert.AreEqual("404", notFoundLinkStatusCode.Text);
            Assert.AreEqual("Not Found", notFoundLinkStatusText.Text);
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static By LocatorByXPath(string text)
        {
            return By.XPath($"//*[@id='linkResponse']/b[.='{text}']");
        }

        private void WaitUntilDisplayed(IWebElement element)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));
            wait.Until(driver => element.Displayed);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
} 