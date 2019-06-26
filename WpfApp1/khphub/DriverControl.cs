using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1.khphub
{
    class DriverControl
    {

        IWebDriver driver;
        EventFiringWebDriver firingDriver;

        // 드라이버 설정 
        public EventFiringWebDriver GetDriver(String[] optionsParam = null)
        {

            // 크롬 드라이버 설정
            ChromeOptions option = new ChromeOptions();
            // option.AddArgument("--headless");
            if (optionsParam != null)
            {
                option.AddArguments(optionsParam);
            }
            driver = new ChromeDriver(option);

            // 드라이버 이벤트 설정 
            firingDriver = new EventFiringWebDriver(driver);
            firingDriver.ElementClicked += new EventHandler<WebElementEventArgs>(delegate (object sender, WebElementEventArgs e)
            {
                try
                {
                    System.Console.Write("Clicked on: " + e.Element.ToString());
                    System.Console.WriteLine("Page title: " + e.Driver.Title);
                }
                catch (Exception)
                {
                    //System.Console.WriteLine(e.StackTrace);
                    //throw;
                }

            });

            firingDriver.Navigated += new EventHandler<WebDriverNavigationEventArgs>(delegate (object sender, WebDriverNavigationEventArgs e)
            {
                try
                {
                    System.Console.Write("Navigated to: " + e.Driver.Url);
                    System.Console.WriteLine("Page title: " + e.Driver.Title);
                }
                catch (Exception)
                {
                    //throw;
                }
            });

            // driver = firingDriver;
            return firingDriver;
        }

        // 경고창과 클릭 예외 처리가 된 클릭 이벤트
        public void Click(EventFiringWebDriver pDriver, By by)
        {

            while (true)
            {
                try
                {
                    pDriver.FindElement(by).Click();
                    break;
                }
                catch (UnhandledAlertException e)
                {
                    Console.WriteLine($": '{e}'");
                    try
                    {
                        IAlert alert = pDriver.SwitchTo().Alert();
                        Console.WriteLine(alert.Text);
                        alert.Dismiss();
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($": '{e}'");
                    }
                }
                catch (ElementClickInterceptedException e)
                {
                    Console.WriteLine($": '{e}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($": '{e}'");
                }
                Thread.Sleep(1000);
            }
        }

        public List<BankAccount> SetAccountList(EventFiringWebDriver pDriver)
        {
            List<BankAccount> list = new List<BankAccount>();

            IReadOnlyCollection<IWebElement> accList = pDriver.FindElements(By.CssSelector("#tabDep_sub_table1 tbody tr"));

            foreach (IWebElement el in accList)
            {
                BankAccount bankAccount = new BankAccount
                {
                    Num = el.FindElement(By.CssSelector("td:nth-child(2)")).Text,
                    Balance = el.FindElement(By.CssSelector("td:nth-child(4)")).Text
                };
                list.Add(bankAccount);
            }

            return list;
        }
    }
}
