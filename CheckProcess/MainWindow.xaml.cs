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
using PalletLinkDLL;
using System.IO;
using CheckProcess.Clases;

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


            //List<string> _ListByPanel = StaticMESFuntions.ListByPanel("DC14523001120369");

            //List<string> _Worker1 = _ListByPanel.Skip(0).Take(5).ToList();
            //List<string> _Worker2 = _ListByPanel.Skip(5).Take(5).ToList();
            //List<string> _Worker3 = _ListByPanel.Skip(10).Take(5).ToList();
                   
            //Task<DataTable> task1 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker1).Result);
            //Task<DataTable> task2 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker2).Result);
            //Task<DataTable> task3 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker3).Result);

            //Task.WaitAll(new[] { task1, task2, task3 });

            //List<DataTable> tasks = new List<DataTable>();
            //tasks.Add(task1.Result);
            //tasks.Add(task2.Result);
            //tasks.Add(task3.Result);
            
            //DataTable _dtResult = StaticFunctions.CombineDataTables(tasks);
        }


        public struct MESData
        {
            public string SERIAL_NUMBER { set; get; }
            public string MAPPING { set; get; }
            public string HISTORY { set; get; }
            public string STATUS { set; get; }
            public string DOUBLE_LOOP { set; get; }
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

            DataGridTextColumn DOUBLE_LOOP = new DataGridTextColumn();
            DOUBLE_LOOP.Header = "DOUBLE_LOOP";
            DOUBLE_LOOP.Binding = new Binding("DOUBLE_LOOP");
            DOUBLE_LOOP.Width = 80;
            DOUBLE_LOOP.IsReadOnly = true;

            DgPanelInfo.Columns.Add(SERIAL_NUMBER);
            DgPanelInfo.Columns.Add(MAPPING);
            DgPanelInfo.Columns.Add(HISTORY);
            DgPanelInfo.Columns.Add(STATUS);
            DgPanelInfo.Columns.Add(DOUBLE_LOOP);
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

        void WriteDgv(string _SERIAL_NUMBER, string _MAPPING, string _HISTROY, string _STATUS, string _DOUBLE_LOOP)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    DgPanelInfo.Items.Add(new MESData { SERIAL_NUMBER = _SERIAL_NUMBER, MAPPING = _MAPPING, HISTORY = _HISTROY, STATUS = _STATUS, DOUBLE_LOOP = _DOUBLE_LOOP});

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
                    WriteDgv("APP ERROR", "0", ex.Message, "FAIL" , "");
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
                    //Task.Factory.StartNew(() => MainFunctionMultiTask());
                  
                }      
            }
        }

        void CleanDg() 
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



        void UpdateLayout(string _TempSN, string _TempMapping, string _TempStep, string _TempStatus, string _TempDoubleLoop) 
        {
            if (_TempMapping == "0")
            {
                lblSN.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
                
            }

            if (_TempMapping == "1")
            {
                lblSN01.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN01.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN01.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN01.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }


            if (_TempMapping == "2")
            {
                lblSN02.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN02.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN02.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN02.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "3")
            {
                lblSN03.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN03.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN03.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN03.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "4")
            {
                lblSN04.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN04.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN04.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN04.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "5")
            {
                lblSN05.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN05.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN05.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN05.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "6")
            {
                lblSN06.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN06.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN06.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN06.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "7")
            {
                lblSN07.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN07.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN07.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN07.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "8")
            {
                lblSN08.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN08.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN08.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN08.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "9")
            {
                lblSN09.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN09.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN09.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN09.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "10")
            {
                lblSN10.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "11")
            {
                lblSN11.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "12")
            {
                lblSN12.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "13")
            {
                lblSN13.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "14")
            {
                lblSN14.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }

            if (_TempMapping == "15")
            {
                lblSN15.Content = _TempSN;

                if (_TempStatus == "Pass") lblSN15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"));
                if (_TempStatus == "Fail" || _TempStatus == "QC / MRB") lblSN15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B71C1C"));
                if (_TempStatus == "Missing Step") lblSN15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E65100"));
                if (_TempDoubleLoop == "Loop 2") lblSN.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F"));
            }
        }



        async void MultiMES() 
        {
            string[] All90Seriales = File.ReadAllLines(@"\\mxchim0rel02\Dexcom\TEApplications\CheckProcess\CheckProcess\bin\Debug\90Seriales.txt");

            string[] _15Seriales = All90Seriales.Skip(0).Take(15).ToArray();
            string[] _30Seriales = All90Seriales.Skip(15).Take(15).ToArray();
            string[] _45Seriales = All90Seriales.Skip(30).Take(15).ToArray();
            string[] _60Seriales = All90Seriales.Skip(45).Take(15).ToArray();
            string[] _75Seriales = All90Seriales.Skip(60).Take(15).ToArray();
            string[] _90Seriales = All90Seriales.Skip(75).Take(15).ToArray();

            Task task1 = Task.Factory.StartNew(() => MultiTaskingMES.SendStepToMES_15(_15Seriales, "Aquastorm")); 
            Task task2 = Task.Factory.StartNew(() => MultiTaskingMES.SendStepToMES_30(_30Seriales, "Aquastorm"));
            Task task3 = Task.Factory.StartNew(() => MultiTaskingMES.SendStepToMES_45(_45Seriales, "Aquastorm"));
            Task task4 = Task.Factory.StartNew(() => MultiTaskingMES.SendStepToMES_60(_60Seriales, "Aquastorm"));
            Task task5 = Task.Factory.StartNew(() => MultiTaskingMES.SendStepToMES_75(_75Seriales, "Aquastorm"));
            Task task6 = Task.Factory.StartNew(() => MultiTaskingMES.SendStepToMES_90(_90Seriales, "Aquastorm"));

            Task.WaitAll(new[] { task1, task2, task3, task4, task5, task6 });
        }


        void MainFunctionMultiTask()
        {
            CleanDg();

            List<string> _ListByPanel = StaticMESFuntions.ListByPanel(Globals.SERIAL_NUMBER1);

            List<string> _Worker1 = _ListByPanel.Skip(0).Take(3).ToList();
            List<string> _Worker2 = _ListByPanel.Skip(3).Take(3).ToList();
            List<string> _Worker3 = _ListByPanel.Skip(6).Take(3).ToList();
            List<string> _Worker4 = _ListByPanel.Skip(9).Take(3).ToList();
            List<string> _Worker5 = _ListByPanel.Skip(12).Take(3).ToList();
      

            DataTable _dt1 = new DataTable();
            DataTable _dt2 = new DataTable();
            DataTable _dt3 = new DataTable();
            DataTable _dt4 = new DataTable();
            DataTable _dt5 = new DataTable();

            if (Globals.MODE == "TabPanelMode")
            {
                Task<DataTable> task1 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker1).Result);
                Task<DataTable> task2 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker2).Result);
                Task<DataTable> task3 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker3).Result);
                Task<DataTable> task4 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker4).Result);
                Task<DataTable> task5 = Task.Factory.StartNew(() => StaticMESFuntions.VerifyCheckPoint(_Worker5).Result);
        
                Task.WaitAll(new[] { task1, task2, task3, task4, task5});

                List<DataTable> tasks = new List<DataTable>();
                tasks.Add(task1.Result);
                tasks.Add(task2.Result);
                tasks.Add(task3.Result);
                tasks.Add(task4.Result);
                tasks.Add(task5.Result);

                _dt1 = task1.Result;
                _dt2 = task2.Result;
                _dt3 = task3.Result;
                _dt4 = task4.Result;
                _dt5 = task5.Result;
   
                Globals.DT_LANE1 = StaticFunctions.CombineDataTables(tasks);
            }

            if (Globals.MODE == "TabSingleMode") Globals.DT_LANE1 = StaticFunctions.VerifyCheckPointSingle(Globals.SERIAL_NUMBER1);

            Task tsk1 = Task.Factory.StartNew(() => SendToMES(_dt1));
            Task tsk2 = Task.Factory.StartNew(() => SendToMES(_dt2));
            Task tsk3 = Task.Factory.StartNew(() => SendToMES(_dt3));
            Task tsk4 = Task.Factory.StartNew(() => SendToMES(_dt4));
            Task tsk5 = Task.Factory.StartNew(() => SendToMES(_dt5));

            Task.WaitAll(new[] { tsk1, tsk2, tsk3, tsk4, tsk5 });

            #region Original
            //foreach (DataRow _dr in Globals.DT_LANE1.Rows)
            //{
            //    string _TempSN = _dr[0].ToString();
            //    string _TempMapping = _dr[1].ToString();
            //    string _TempStep = _dr[2].ToString();
            //    string _TempStatus = _dr[3].ToString();
            //    string _TempDoubleLoop = _dr[4].ToString();

            //    if (_TempStep == Globals.STEP_TO_CHECK && _TempStatus == "Pass") 
            //        Task.Factory.StartNew(() => StaticFunctions.SendStepToMES(_TempSN, Globals.STEP_TO_SEND));

            //    if (Globals.MODE == "TabSingleMode")
            //        if (Globals.SERIAL_NUMBER1 == _TempSN) _TempMapping = "0";

            //    this.Dispatcher.BeginInvoke(new Action(() => { UpdateLayout(_TempSN, _TempMapping, _TempStep, _TempStatus, _TempDoubleLoop); }));
            //}
            #endregion

            if (Globals.DT_LANE1 == null) return;

            foreach (DataRow _dr in Globals.DT_LANE1.Rows)
            {
                string SERIAL_NUMBER = _dr[0].ToString();
                string MAPPING = _dr[1].ToString();
                string HISTORY = _dr[2].ToString();
                string STATUS = _dr[3].ToString();
                string DOUBLE_LOOP = _dr[4].ToString();

                WriteDgv(SERIAL_NUMBER, MAPPING, HISTORY, STATUS, DOUBLE_LOOP);
                this.Dispatcher.BeginInvoke(new Action(() => { UpdateLayout(SERIAL_NUMBER, MAPPING, HISTORY, STATUS, DOUBLE_LOOP); }));
            }



            this.Dispatcher.BeginInvoke(new Action(() => { txtScanning.IsEnabled = true; }));
            this.Dispatcher.BeginInvoke(new Action(() => { txtScanning.Clear(); }));
            this.Dispatcher.BeginInvoke(new Action(() => { txtScanning.Focus(); }));
        }


        async void SendToMES(DataTable dt)
        {
            foreach (DataRow _dr in dt.Rows)
            {
                string _TempSN = _dr[0].ToString();
                string _TempMapping = _dr[1].ToString();
                string _TempStep = _dr[2].ToString();
                string _TempStatus = _dr[3].ToString();
                string _TempDoubleLoop = _dr[4].ToString();

                if (_TempStep == Globals.STEP_TO_CHECK && _TempStatus == "Pass") StaticFunctions.SendStepToMES(_TempSN, Globals.STEP_TO_SEND);

                if (Globals.MODE == "TabSingleMode")
                    if (Globals.SERIAL_NUMBER1 == _TempSN) _TempMapping = "0";             
            }
        }


        void MainFunction()
        {          
            CleanDg();
           
            if(Globals.MODE == "TabPanelMode") Globals.DT_LANE1 = StaticFunctions.VerifyCheckPointPanel(Globals.SERIAL_NUMBER1);
            if(Globals.MODE == "TabSingleMode") Globals.DT_LANE1 = StaticFunctions.VerifyCheckPointSingle(Globals.SERIAL_NUMBER1);

            foreach (DataRow _dr in Globals.DT_LANE1.Rows)
            {
                string _TempSN = _dr[3].ToString();
                string _TempMapping = _dr[4].ToString();
                string _TempStep = _dr[7].ToString();
                string _TempStatus = _dr[8].ToString();
                string _TempDoubleLoop = _dr[9].ToString();
               
                if (Globals.MODE == "TabSingleMode")
                    if (Globals.SERIAL_NUMBER1 == _TempSN) _TempMapping = "0";

                this.Dispatcher.BeginInvoke(new Action(() => { UpdateLayout(_TempSN, _TempMapping, _TempStep, _TempStatus, _TempDoubleLoop); }));                   
            }


            if (Globals.DT_LANE1 == null) return;

            foreach (DataRow _dr in Globals.DT_LANE1.Rows)
            {
                string SERIAL_NUMBER = _dr[3].ToString();
                string MAPPING = _dr[4].ToString();
                string HISTORY = _dr[7].ToString();
                string STATUS = _dr[8].ToString();
                string DOUBLE_LOOP = _dr[9].ToString();

                WriteDgv(SERIAL_NUMBER, MAPPING, HISTORY, STATUS, DOUBLE_LOOP);
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
                if (_myData.DOUBLE_LOOP == "Loop 2")
                    row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880E4F")); //DEEP RED 900

                else 
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
            string SerialNumber = lbl.Content.ToString();

            if (Globals.USING_BY == "DEBUG") 
            {
                var _notificationManager = new NotificationManager();
                _notificationManager.Show("Copyied:" + lbl.Content, null, TimeSpan.FromSeconds(5));     
            }

            if (Globals.USING_BY == "MFG") 
            {
                DataTable _dtResult = new DataTable();
                DataTable _dtPanel = new DataTable();

                DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);
                int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
                int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

                if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
                if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

                _dsQuery = new MES.Service().BoardHistoryReport(SerialNumber, _CustomerID);

                new MESHistory(_dsQuery.Tables[0]).Show();
            }
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

        private void MenuItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private static Label FindClickedItem(object sender)
        {
            var mi = sender as MenuItem;
            if (mi == null)
            {
                return null;
            }

            var cm = mi.CommandParameter as ContextMenu;
            if (cm == null)
            {
                return null;
            }

            return cm.PlacementTarget as Label;
        }    
    }
}
