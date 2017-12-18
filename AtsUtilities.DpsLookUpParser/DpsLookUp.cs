
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
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
            //http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS

            /*
              the email is: jboesch@abctitle.com the password is: notary123
            the VIN Number you can test with is: 2GCEC13T861244730
            The license plate number you can test with is: YLL248
            Please also test failed conditions. Thanks. VM LM
             */
            //vimnumber / licensenumber

            String initialAddress = "http://www.autotitleservice.com/DPS/Account/Login?ReturnUrl=%2fDPS";

            /*
             new WebDriverWait(driver, MyDefaultTimeout).Until(
    d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));
             */

            //Instantiate web browser simulator
            var webDriver = new PhantomJSDriver();
            //Navigate to the initial login page
            webDriver.Navigate().GoToUrl(initialAddress);
            //Wait until page is loaded and login button is available
            var buttonLogin =
                 (new WebDriverWait(webDriver, DpsLookUp.Timeout))
                 .Until(ExpectedConditions.ElementExists(By.Id("btnLogin")));
            //Get login and password fields
            var fieldUserName = webDriver.FindElement(By.Name("UserName"));
            var fieldPassWord = webDriver.FindElement(By.Name("Password"));
            //Populate these fields
            fieldUserName.SendKeys(userName);
            fieldPassWord.SendKeys(passWord);
            //Submit login
            buttonLogin.Submit();
            //Wait until the page loads
            DpsLookUp.WaitUntilPageIsLoaded(webDriver);
            //Check if login was correct
            if(webDriver.Url == initialAddress)
            {
                //If URL remains the same, that does mean login failure
                throw new Exception("Incorrect credentials");
            }
            else
            {
                switch(searchBy)
                {
                    case "vimnumber":
                        break;
                    case "licensenumber":
                        break;
                    default:
                        throw new Exception(@"[searchBy] must be equal to 'vimnumber' or 'licensenumber'");
                }
            }

            //errorBar

            Console.WriteLine("\nSUBMITTED LOGIN\n");

           

            IWebElement element =
                (new WebDriverWait(webDriver, new TimeSpan(0, 0, 10)))
                .Until(ExpectedConditions.ElementExists(By.Name("searchData")));

            element.SendKeys("2GCEC13T861244730");

            Console.WriteLine("\nSUBMITTED DATA\n");

            var checkboxes = webDriver.FindElements(By.Name("searchtype"));
            checkboxes[0].Click();



            webDriver.FindElement(By.Name("search_now")).Submit();

            Console.WriteLine("\nSUBMITTED SEARCH\n");

            //table lookup-table

            
            IWebElement tableElement = 
                (new WebDriverWait(webDriver, new TimeSpan(0, 0, 10)))
                .Until(ExpectedConditions.ElementExists(By.TagName("table")));


            Console.WriteLine("\n\n\n\nElement: " + tableElement.Text);
            



            /*
             WebDriver driver = new FirefoxDriver();
driver.get("http://somedomain/url_that_delays_loading");
WebElement myDynamicElement = (new WebDriverWait(driver, 10))
  .until(ExpectedConditions.presenceOfElementLocated(By.Name("searchData")));
             */

            /*
            Assert.AreEqual(
              "Andy",
              _driver.FindElement(By.ClassName("sidebar-text5")).Text
            );*/


        }
    }
}
