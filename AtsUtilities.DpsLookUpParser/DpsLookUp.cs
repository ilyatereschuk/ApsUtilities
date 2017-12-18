
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AtsUtilities.DpsLookUpParser
{
    public class DpsLookUp
    {
        private static readonly String InitialUrl = "http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS";

        private static readonly TimeSpan Timeout = new TimeSpan(0, 0, 10);

        private static void WaitUntilPageIsLoaded(IWebDriver webDriver)
        {
            (new WebDriverWait(webDriver, DpsLookUp.Timeout))
            .Until(
                jsExecutor => ((IJavaScriptExecutor)jsExecutor)
                .ExecuteScript("return document.readyState")
                .Equals("complete")
            );
        }
        public static String GetDpsLookupHtmlResult(
            String userName,
            String passWord,
            String searchBy, 
            String query,
            Action<String> onStepChanged,
            Action<Int32> onProgressPercentageChanged)
        {
            //Instantiate web browser simulator
            onStepChanged("Instantiating PhantomJS...");
            onProgressPercentageChanged(0);
            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var webDriver = new PhantomJSDriver(driverService);
            //Navigate to the initial login page
            onStepChanged("Navigating to initial page...");
            onProgressPercentageChanged(25);
            webDriver.Navigate().GoToUrl(DpsLookUp.InitialUrl);
            //Wait until page is loaded and login button is available
            var buttonLogin =
                 (new WebDriverWait(webDriver, DpsLookUp.Timeout))
                 .Until(ExpectedConditions.ElementExists(By.Id("btnLogin")));
            //Populate login and password fields
            webDriver.FindElement(By.Name("UserName")).SendKeys(userName);
            webDriver.FindElement(By.Name("Password")).SendKeys(passWord);
            //Submit login
            onStepChanged("Submitting credentials...");
            onProgressPercentageChanged(50);
            buttonLogin.Submit();
            //Wait until the page loads
            DpsLookUp.WaitUntilPageIsLoaded(webDriver);
            //Check if login was correct
            if(webDriver.Url == DpsLookUp.InitialUrl)
            {
                //If URL remains the same, that does mean login failure
                throw new Exception("Incorrect credentials");
            }
            else
            {
                //Browser is on a search page. Get the search category radiobuttons
                var checkboxesSearchType = webDriver.FindElements(By.Name("searchtype"));
                switch (searchBy)
                {
                    case "vinnumber":
                        checkboxesSearchType[0].Click();
                        break;
                    case "licensenumber":
                        checkboxesSearchType[1].Click();
                        break;
                    default:
                        throw new Exception("[searchBy] must be equal to 'vinnumber' or 'licensenumber'");
                }
                //Populate input with the given VIM or license plate number
                webDriver.FindElement(By.Name("searchData")).SendKeys(query);
                //Submit input
                webDriver.FindElement(By.Name("search_now")).Submit();
                //Wait until AJAX performs to the end so the table appears
                onStepChanged("Loading data...");
                onProgressPercentageChanged(75);
                (new WebDriverWait(webDriver, DpsLookUp.Timeout))
                .Until(ExpectedConditions.ElementExists(By.TagName("table")));
                //Check if the query was correct
                if (webDriver.FindElements(By.ClassName("alert-error")).Count != 0)
                {
                    //If there is HTML element with 'alert-error' class, that means data was wrong
                    throw new Exception("[query] does not represent some existing [" + searchBy + "]");
                }
                else
                {
                    onStepChanged("Data was successfully loaded!");
                    onProgressPercentageChanged(100);
                    return webDriver.PageSource;
                }
            }
        }
    }
}
