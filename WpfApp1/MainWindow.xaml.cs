using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using WpfApp1.khphub;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        EventFiringWebDriver blindDriver;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DriverControl dc = new DriverControl();
            EventFiringWebDriver eventDriver = dc.GetDriver(); // 드라이버 객체 생성 및 클릭 이벤트 세팅

            eventDriver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=CMLGN0001");
            Thread.Sleep(1000);
            //dc.Click(eventDriver, By.CssSelector(".pr_log_2"));
            Thread.Sleep(1000);
            BringToFront();
            MessageBox.Show("로그인 해주세요.");
            Thread.Sleep(1000);

            // 로그인 될때까지 대기
            while (true)
            {
                try
                {
                    if (eventDriver.FindElement(By.CssSelector(".login-name")).Displayed)
                    {
                        System.Console.WriteLine("로그인 성공");
                        break;
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine($": '{e}'");
                }
                Thread.Sleep(2000);
            }

            //EventFiringWebDriver blindDriver = dc.GetDriver(new String[] { "--headless", "window-size=1920x1080", "disable-gpu" });
            blindDriver = dc.GetDriver();

            blindDriver.Navigate().GoToUrl("https://www.wooribank.com/");

            ReadOnlyCollection<Cookie> _cookies = eventDriver.Manage().Cookies.AllCookies;

            //WebDriverWait wait = new WebDriverWait(blindDriver, TimeSpan.FromSeconds(60));
            //wait.Until(ExpectedConditions.AlertIsPresent());
            //blindDriver.SwitchTo().Alert().Dismiss();
            //wait = new WebDriverWait(blindDriver, TimeSpan.FromSeconds(60));
            //wait.Until(ExpectedConditions.AlertIsPresent());
            //blindDriver.SwitchTo().Alert().Dismiss();

            // blindDriver.Manage().Cookies.DeleteAllCookies();
            foreach (Cookie item in _cookies)
            {
                blindDriver.Manage().Cookies.AddCookie(item);
            }
            Thread.Sleep(1000);
            blindDriver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=PSINQ0013"); // 게좌 화면 
            Thread.Sleep(1000);
            eventDriver.Quit();

            AccountList.ItemsSource = dc.SetAccountList(blindDriver);

            BringToFront();
        }

        // 창 앞으로 땡기는 함수 
        public void BringToFront() {
            if (!IsVisible)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }

            Activate();
            Topmost = true;  // important
            Topmost = false; // important
            Focus();         // important
        }

        // 계좌 선택 
        private void AccountList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            blindDriver.Navigate().GoToUrl("https://spib.wooribank.com/pib/Dream?withyou=PSINQ0028"); // 게좌 화면 
            
        }

    } // MainWindow class 끝 
} // namespace 끝
