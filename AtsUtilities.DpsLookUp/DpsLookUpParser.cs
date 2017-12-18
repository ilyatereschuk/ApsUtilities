using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsUtilities.DpsLookUp
{
    public class DpsLookUpParser
    {
        private static readonly String InitialUrl = "http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS";

        private static readonly TimeSpan Timeout = new TimeSpan(0, 0, 30);

        private static void WaitUntilPageIsLoaded(IWebDriver webDriver)
        {
            (new WebDriverWait(webDriver, DpsLookUpParser.Timeout))
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
            Action<String,Int32> onStepChanged,
            Action<String> onCompleted)
        { 
            //Instantiate web browser simulator
            onStepChanged("Instantiating PhantomJS...", 0);
            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var webDriver = new PhantomJSDriver(driverService);
            try
            {
                //Navigate to the initial login page
                onStepChanged("Navigating to initial page...", 25);
                webDriver.Navigate().GoToUrl(DpsLookUpParser.InitialUrl);
                //Wait until page is loaded and login button is available
                var buttonLogin = webDriver.FindElement(By.Id("btnLogin"));
                //Populate login and password fields
                webDriver.FindElement(By.Name("UserName")).SendKeys(userName);
                webDriver.FindElement(By.Name("Password")).SendKeys(passWord);
                //Submit login
                onStepChanged("Submitting credentials...", 50);
                buttonLogin.Submit();
                //Wait until the page loads
                DpsLookUpParser.WaitUntilPageIsLoaded(webDriver);
                //Check if login was correct
                if (webDriver.Url == DpsLookUpParser.InitialUrl)
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
                    onStepChanged("Loading data...", 75);
                    (new WebDriverWait(webDriver, DpsLookUpParser.Timeout))
                    .Until(ExpectedConditions.ElementExists(By.TagName("table")));
                    //Check if the query was correct
                    if (webDriver.FindElements(By.ClassName("alert-error")).Count != 0)
                    {
                        //If there is HTML element with 'alert-error' class, that means data was wrong
                        throw new Exception("[query] does not represent some existing [" + searchBy + "]");
                    }
                    else
                    {
                        onStepChanged("Data has been successfully loaded!", 100);
                        onCompleted(webDriver.PageSource);
                        return webDriver.PageSource;
                    }
                }
            }
            catch (Exception exception)
            {
                driverService.Dispose();
                webDriver.Quit();
                throw exception;
            }
            finally
            {
                driverService.Dispose();
                webDriver.Quit();
            }
        }

        public static String GetDpsLookupHtmlResultSimplified(
            String userName,
            String passWord,
            String searchBy,
            String query)
        {
            return DpsLookUpParser.GetDpsLookupHtmlResult(
                userName,
                passWord,
                searchBy,
                query,
                (String stepChangedMessage, Int32 progressChangedValue) => { },
                (String htmlResult) => { });
        }
    }
}
