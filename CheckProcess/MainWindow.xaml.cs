using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Deployment.Application;
using System.Data;
using System.Windows.Threading;
using Notification.Wpf;

namespace CheckProcess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitDataGrid();
            GetVersion();
            DockMenu.Width = 0;
        }


        public struct MESData
        {
            public string SERIAL_NUMBER { set; get; }
            public string MAPPING { set; get; }
            public string HISTORY { set; get; }
            public string STATUS { set; get; }
        }

        void InitDataGrid()
        {
            DataGridTextColumn SERIAL_NUMBER = new DataGridTextColumn();
            SERIAL_NUMBER.Header = "SERIAL NUMBER";
            SERIAL_NUMBER.Binding = new Binding("SERIAL_NUMBER");
            SERIAL_NUMBER.Width = 125;
            SERIAL_NUMBER.IsReadOnly = true;

            DataGridTextColumn MAPPING = new DataGridTextColumn();
            MAPPING.Header = "MAPPING";
            MAPPING.Binding = new Binding("MAPPING");
            MAPPING.Width = 30;
            MAPPING.IsReadOnly = true;

            DataGridTextColumn HISTORY = new DataGridTextColumn();
            HISTORY.Header = "HISTORY";
            HISTORY.Binding = new Binding("HISTORY");
            HISTORY.Width = 280;
            HISTORY.IsReadOnly = true;

            DataGridTextColumn STATUS = new DataGridTextColumn();
            STATUS.Header = "STATUS";
            STATUS.Binding = new Binding("STATUS");
            STATUS.Width = 80;
            STATUS.IsReadOnly = true;

            DgPanelInfo.Columns.Add(SERIAL_NUMBER);
            DgPanelInfo.Columns.Add(MAPPING);
            DgPanelInfo.Columns.Add(HISTORY);
            DgPanelInfo.Columns.Add(STATUS);
        }

        void GetVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                lblVersion.Content = "PRD v" + myVersion;
            }
            else
            {
                lblVersion.Content = "UAT v" + "1.0.0.0";
            }       
        }

        void WriteDgv(string _SERIAL_NUMBER, string _MAPPING, string _HISTROY, string _STATUS)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    DgPanelInfo.Items.Add(new MESData { SERIAL_NUMBER = _SERIAL_NUMBER, MAPPING = _MAPPING, HISTORY = _HISTROY, STATUS = _STATUS });

                    if (DgPanelInfo.Items.Count > 0)
                    {
                        var border = VisualTreeHelper.GetChild(DgPanelInfo, 0) as Decorator;
                        if (border != null)
                        {
                            var scroll = border.Child as ScrollViewer;
                            if (scroll != null) scroll.ScrollToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteDgv("APP ERROR", "0", ex.Message, "FAIL");
                }
            }));
        }
        private void txtScanning_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                if (txtScanning.Text.Length == 10 || txtScanning.Text.Length == 16) 
                {
                    txtScanning.IsEnabled = false;
                    DgPanelInfo.Items.Clear();
                    DgPanelInfo.DataContext = null;

                    Globals.SERIAL_NUMBER1 = txtScanning.Text;
                    Task.Factory.StartNew(() => MainFunction());
                }      
            }
        }

        void CleanUp() 
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN01.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN02.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN03.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN04.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN05.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN06.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN07.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN08.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN09.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN10.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN11.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN12.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN13.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN14.Background = new SolidColorBrush(Colors.Transparent); }));               
                this.Dispatcher.BeginInvoke(new Action(() => { lblSN15.Background = new SolidColorBrush(Colors.Transparent); }));

                this.Dispatcher.BeginInvoke(new Action(() => { ((MainWindow)System.Windows.Application.Current.MainWindow).UpdateLayout(); }));
            }));
        }



        void UpdateLayout(string _TempSN, string _TempMapping, string _TempStep, string _TempStatus) 
        {
            if (_TempMapping == "0")
            {
                lblSN.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "1")
            {
                lblSN01.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN01.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN01.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN01.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }


            if (_TempMapping == "2")
            {
                lblSN02.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN02.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN02.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN02.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "3")
            {
                lblSN03.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN03.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN03.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN03.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "4")
            {
                lblSN04.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN04.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN04.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN04.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "5")
            {
                lblSN05.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN05.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN05.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN05.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "6")
            {
                lblSN06.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN06.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN06.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN06.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "7")
            {
                lblSN07.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN07.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN07.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN07.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "8")
            {
                lblSN08.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN08.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN08.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN08.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "9")
            {
                lblSN09.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN09.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN09.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN09.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "10")
            {
                lblSN10.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "11")
            {
                lblSN11.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "12")
            {
                lblSN12.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "13")
            {
                lblSN13.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "14")
            {
                lblSN14.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }

            if (_TempMapping == "15")
            {
                lblSN15.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
            }
        }

        void MainFunction()
        {          
            CleanUp();
           

            if(Globals.MODE == "TabPanelMode") Globals.DT_LANE1 = StaticFunctions.VerifyCheckPointPanel(Globals.SERIAL_NUMBER1);
            if(Globals.MODE == "TabSingleMode") Globals.DT_LANE1 = StaticFunctions.VerifyCheckPointSingle(Globals.SERIAL_NUMBER1);

            foreach (DataRow _dr in Globals.DT_LANE1.Rows)
            {
                string _TempSN = _dr[3].ToString();
                string _TempMapping = _dr[4].ToString();
                string _TempStep = _dr[7].ToString();
                string _TempStatus = _dr[8].ToString();

                if (Globals.MODE == "TabSingleMode")
                    if (Globals.SERIAL_NUMBER1 == _TempSN) _TempMapping = "0";

                this.Dispatcher.BeginInvoke(new Action(() => { UpdateLayout(_TempSN, _TempMapping, _TempStep, _TempStatus); }));                   
            }


            if (Globals.DT_LANE1 == null) return;

            foreach (DataRow _dr in Globals.DT_LANE1.Rows)
            {
                string SERIAL_NUMBER = _dr[3].ToString();
                string MAPPING = _dr[4].ToString();
                string HISTORY = _dr[7].ToString();
                string STATUS = _dr[8].ToString();

                WriteDgv(SERIAL_NUMBER, MAPPING, HISTORY, STATUS);
            }



            this.Dispatcher.BeginInvoke(new Action(() => { txtScanning.IsEnabled = true;}));
            this.Dispatcher.BeginInvoke(new Action(() => { txtScanning.Clear();}));
            this.Dispatcher.BeginInvoke(new Action(() => { txtScanning.Focus();}));
        }

        #region Eventos de aplicacion

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DOCK_MENU)
            {
                //Margin="10,120,10,440"
                //Thickness _temp = lblEventosLane1.Margin;
                //_temp.Left = 200f;

                //btnOnOff_Lane1.Visibility = Visibility.Visible;
                //btnOnOff_Lane2.Visibility = Visibility.Visible;
                //btnDebug.Visibility = Visibility.Visible;
                //btnPassThru.Visibility = Visibility.Visible;
                //btnMenu.Content = "MENU";
                //btnMenu.Width = 168;
                //btnMenu.Height = 75;
                Globals.DOCK_MENU = false;
                DockMenu.Width = 180;
                return;
            }

            if (!Globals.DOCK_MENU)
            {
                //Thickness _temp = lblEventosLane1.Margin;
                //_temp.Left = 10f;

                //btnOnOff_Lane1.Visibility = Visibility.Hidden;
                //btnOnOff_Lane2.Visibility = Visibility.Hidden;
                //btnDebug.Visibility = Visibility.Hidden;
                //btnPassThru.Visibility = Visibility.Hidden;
                //btnMenu.Content = "M";
                //btnMenu.Width = 45;
                //btnMenu.Height = 45;
                Globals.DOCK_MENU = true;
                DockMenu.Width = 0;
                return;
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void DgPanelInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var row = e.Row;
            MESData _myData = new MESData();
            _myData = (MESData)row.DataContext;

            if (_myData.STATUS == "Pass")
            {
                row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20")); //DEEP GREEN 900
                row.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            }

            if (_myData.STATUS == "Fail" || _myData.STATUS == "QC / MRB")
            {
                row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C")); //DEEP RED 900
                row.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            }

            if (_myData.STATUS == "Missing Step")
            {
                row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100")); //DEEP ORANGE 900
                row.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void txtScanning_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveText(sender, e);
        }

        private void txtScanning_LostFocus(object sender, RoutedEventArgs e)
        {
            AddText(sender, e);
        }

        void RemoveText(object sender, EventArgs e)
        {
            if (txtScanning.Text == "Scan board here...")
            {
                txtScanning.Text = "";
                txtScanning.Text = "";
            }

        }

        void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtScanning.Text))
            {
                txtScanning.Text = "Scan board here...";
                txtScanning.Text = "Scan board here...";
            }
        }

        #endregion

        private void lblSN_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;
            Clipboard.SetText(lbl.Content.ToString());


            var _notificationManager = new NotificationManager();
            _notificationManager.Show("Copyied:" + lbl.Content, null, TimeSpan.FromSeconds(5));
        }

        private void TabMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = (TabControl)sender;

            if (tab != null)
            {
                TabItem item = (TabItem)tab.SelectedItem;
                Globals.MODE = item.Name;
            }
        }
    }
}
