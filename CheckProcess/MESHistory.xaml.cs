using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckProcess
{
    /// <summary>
    /// Interaction logic for MESHistory.xaml
    /// </summary>
    public partial class MESHistory : Window
    {
        public MESHistory(DataTable dt)
        {
            InitializeComponent();
            DgMesData.ItemsSource = dt.DefaultView;
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
