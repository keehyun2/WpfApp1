using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Turbo.turbo
{
    /// <summary>
    /// InsertWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertWindow : Window
    {
        private MainWindow _parent;
        public InsertWindow(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        // 저장 
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("저장하시겠습니까?", "저장 확인", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) return;

            PayVO vo = new PayVO();
            vo.Worker = this.Worker.Text;
            vo.CompNum = this.CompNum.Text;
            vo.Amt = Convert.ToInt64(this.Amt.Text);
            vo.Company = this.Company.Text;
            //vo.PayDt = String.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd}", this.PayDt.SelectedDate.Value);
            if (this.PayDt.SelectedDate != null)
            {
                vo.PayDt = String.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd}", this.PayDt.SelectedDate.Value);
            }
            else
            {
                vo.PayDt = String.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd}", DateTime.Now);
            }
            vo.WriteDt = String.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd}", DateTime.Now);

            var collection = ControllDB.db.GetCollection<PayVO>("turbo");

            collection.InsertOne(vo);

            this.Worker.Text = String.Empty;
            this.CompNum.Text = String.Empty;
            this.Amt.Text = String.Empty;
            this.Company.Text = String.Empty;

            _parent.Focus();
            _parent.Button_Click(null,null);
        }

        private void Amt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
