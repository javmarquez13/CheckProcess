using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalletLinkDLL;

namespace CheckProcess
{
    class StaticFunctions
    {
        /// <summary>
        /// Function to Verify CheckPoint in MES, and return a DataTable with the Status for each Panel Array
        /// </summary>
        /// <param name="SerialNumber">Unit Serial Number Variable</param>
        /// <param name="ProcessName">State the name of the process to verify</param>
        /// <returns name="DateTable">Return a DataTable with all serial numbers in panel and their information</returns>
        public static DataTable VerifyCheckPoint(string SerialNumber, string ProcessName)
        {
            DataTable _dtResult = new DataTable();
            DataTable _dtPanel = new DataTable();      
            
            DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);
            int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
            int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);       
            
            if(SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
            if(SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

            _dtResult = _dtPanel.Copy();
            _dtResult.Columns.Add("History");
            _dtResult.Columns.Add("Status");

            foreach (DataRow _dr in _dtPanel.Rows)
            {
                _dsQuery = new MES.Service().BoardHistoryReport(_dr[3].ToString(), _CustomerID);

                foreach (DataRow _drHistory in _dsQuery.Tables[0].Rows)
                {
                    string _TestType = _drHistory[0].ToString();
                    string _TempStep = _drHistory[9].ToString();
                    string _TempStatus = _drHistory[10].ToString();

                    if (_TestType == "TEST")
                    {
                        if (_TempStep.Contains(ProcessName))
                        {
                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                                .Select(b => b["History"] = ProcessName)
                                                                .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                                .Select(b => b["Status"] = _TempStatus)
                                                                .ToList();
                            break;
                        }

                        if (_TempStep.Contains("MRB"))
                        {
                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                                .Select(b => b["History"] = "MRB")
                                                                .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                    .Select(b => b["Status"] = _TempStatus)
                                                    .ToList();
                            break;
                        }
                    }
                }

            }

            return _dtResult;
        }

        public static DataTable VerifyCheckPointPanel(string SerialNumber)
        {
            DataTable _dtResult = new DataTable();
            DataTable _dtPanel = new DataTable();

            DataTable EventsByLoop_2 = new DataTable();
            bool _Had2Loops = false;

            DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);
            int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
            int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

            if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
            if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

            _dtResult = _dtPanel.Copy();
            _dtResult.Columns.Add("History");
            _dtResult.Columns.Add("Status");
            _dtResult.Columns.Add("DoubleLoop");

            DataTable EventsByStepMatrix = new DataTable();

            foreach (DataRow _dr in _dtPanel.Rows)
            {
                _dsQuery = new MES.Service().BoardHistoryReport(_dr[3].ToString(), _CustomerID);

                foreach (string _Key in Globals.DATA_MATRIX)
                {
                    string StepToCheck = ConfigFiles.reader("DATA_MATRIX", _Key, Globals.CONFIG_FILE);

                    try
                    {
                        EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                     .Where(r => r.Field<string>("Test_Process") == "QC / MRB")
                                     .CopyToDataTable();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                              .Select(b => b["History"] = "QC / MRB")
                                             .ToList();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                 .Select(b => b["Status"] = "QC / MRB")
                                .ToList();

                        break;
                    }
                    catch (Exception) { }


                    try
                    {
                        EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                                               .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                           r.Field<string>("Test_Process") == Globals.STEP_TO_CHECK &&
                                                                           r.Field<string>("TestStatus") == "Pass")
                                                               .CopyToDataTable();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                              .Select(b => b["History"] = Globals.STEP_TO_CHECK)
                                             .ToList();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                 .Select(b => b["Status"] = "Pass")
                                .ToList();

                        break;
                    }
                    catch (Exception) { }

                    try
                    {
                        EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                                                  .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                              r.Field<string>("Test_Process") == StepToCheck &&
                                                                              r.Field<string>("TestStatus") == "Fail")
                                                                  .CopyToDataTable();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                     .Select(b => b["History"] = StepToCheck)
                                                    .ToList();




                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                 .Select(b => b["Status"] = "Fail")
                                .ToList();


                        EventsByLoop_2 = _dsQuery.Tables[0].AsEnumerable()
                                                 .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                             r.Field<string>("Test_Process") == "FVT / PBTS" &&
                                                             r.Field<string>("TestStatus") == "Fail" &&
                                                             r.Field<string>("EquipmentRouteName") == "Loop 2")
                                                 .CopyToDataTable();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                  .Select(b => b["DoubleLoop"] = "Loop 2")
                                                 .ToList();

                        break;
                    }
                    catch (Exception) { }
                   
                    try 
                    {
                        EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                                                  .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                              r.Field<string>("Test_Process") == StepToCheck &&
                                                                              r.Field<string>("TestStatus") == "Pass")
                                                                  .CopyToDataTable();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                     .Select(b => b["History"] = StepToCheck)
                                                    .ToList();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                 .Select(b => b["Status"] = "Pass")
                                .ToList();
                    }
                    catch (Exception) 
                    {
                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                                               .Select(b => b["History"] = StepToCheck)
                                                              .ToList();

                        _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[3].ToString())
                                            .Select(b => b["Status"] = "Missing Step")
                                           .ToList();

                        break;
                    }                                       
                }
            }

            return _dtResult;
        }


        public static DataTable VerifyCheckPointSingle(string SerialNumber)
        {
            DataTable _dtResult = new DataTable();
            DataTable _dtPanel = new DataTable();

            DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);
            int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
            int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

            if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
            if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

            _dtResult = _dtPanel.Copy();
            _dtResult.Columns.Add("History");
            _dtResult.Columns.Add("Status");

            DataTable EventsByStepMatrix = new DataTable();

            _dsQuery = new MES.Service().BoardHistoryReport(SerialNumber, _CustomerID);

            foreach (string _Key in Globals.DATA_MATRIX)
            {
                string StepToCheck = ConfigFiles.reader("DATA_MATRIX", _Key, Globals.CONFIG_FILE);

                try
                {
                    EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                 .Where(r => r.Field<string>("Test_Process") == "QC / MRB")
                                 .CopyToDataTable();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                                          .Select(b => b["History"] = "QC / MRB")
                                         .ToList();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                             .Select(b => b["Status"] = "QC / MRB")
                            .ToList();

                    break;
                }
                catch (Exception) { }


                try
                {
                    EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                                           .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                       r.Field<string>("Test_Process") == Globals.STEP_TO_CHECK &&
                                                                       r.Field<string>("TestStatus") == "Pass")
                                                           .CopyToDataTable();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                                          .Select(b => b["History"] = Globals.STEP_TO_CHECK)
                                         .ToList();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                             .Select(b => b["Status"] = "Pass")
                            .ToList();

                    break;
                }
                catch (Exception) { }

                try
                {
                    EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                                              .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                          r.Field<string>("Test_Process") == StepToCheck &&
                                                                          r.Field<string>("TestStatus") == "Fail")
                                                              .CopyToDataTable();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                                                 .Select(b => b["History"] = StepToCheck)
                                                .ToList();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                             .Select(b => b["Status"] = "Fail")
                            .ToList();
                    break;
                }
                catch (Exception) { }

                try
                {
                    EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                                              .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                          r.Field<string>("Test_Process") == StepToCheck &&
                                                                          r.Field<string>("TestStatus") == "Pass")
                                                              .CopyToDataTable();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                                                 .Select(b => b["History"] = StepToCheck)
                                                .ToList();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                             .Select(b => b["Status"] = "Pass")
                            .ToList();
                }
                catch (Exception)
                {
                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                                                           .Select(b => b["History"] = StepToCheck)
                                                          .ToList();

                    _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == SerialNumber.ToString())
                                        .Select(b => b["Status"] = "Missing Step")
                                       .ToList();

                    break;
                }
            }

            return _dtResult;
        }


        public static void SendStepToMES(string SerialNumber, string StepToSend) 
        {
            string _result = string.Empty;
            string horaInicial = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
            string horaFinal = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            string archivoMES = string.Format("S{0}\r\nC{1}\r\nN{2}\r\nOoperador\r\np{3}\r\nP{4}\r\nTP\r\n[{5}\r\n]{6}\r\n", SerialNumber, "DEXCOM", "WM-AQST200-09", 12, StepToSend, horaInicial, horaFinal);
        
            _result = new PalletLinkDLL.PalletLinkSN().fnSendToMES(archivoMES, SerialNumber);  
        }


        public static DataTable CombineDataTables(List<DataTable> dataTables) 
        {
            DataTable _dtResult = new DataTable();
            _dtResult.Columns.Add("SerialNumber");
            _dtResult.Columns.Add("Mapping");
            _dtResult.Columns.Add("History");
            _dtResult.Columns.Add("Status");
            _dtResult.Columns.Add("DoubleLoop");

            int _Mapping = 0;

            foreach (DataTable table in dataTables) 
            {
                foreach (DataRow dr in table.Rows)
                {
                    _Mapping++;
                    _dtResult.Rows.Add(dr[0], _Mapping, dr[2], dr[3], dr[4]);
                }
            }

            return _dtResult;
        }





        public static void NUsed() 
        {

            DataSet _dsQuery = new MES.Service().SelectBySerialNumber(Globals.SERIAL_NUMBER1);

            Globals.CUSTOMER_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
            Globals.WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

            _dsQuery = new MES.Service().BoardHistoryReport(Globals.SERIAL_NUMBER1, Globals.CUSTOMER_ID);


            foreach (DataRow _dr in _dsQuery.Tables[0].Rows)
            {
                string _TempProces = _dr[9].ToString();
                string _TempStatus = _dr[10].ToString();

                if (string.IsNullOrEmpty(_TempProces)) return;


                foreach (string _StepMatrix in Globals.DATA_MATRIX)
                {
                    string StepToCheck = ConfigFiles.reader("DATA_MATRIX", _StepMatrix, Globals.CONFIG_FILE);

                    DataTable EventsByStepMatrix = new DataTable();

                    try
                    {
                        EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                        .Where(r => r.Field<string>("Test_Process") == StepToCheck)
                        .CopyToDataTable();
                    }
                    catch (Exception) { }

                    if (EventsByStepMatrix.Rows.Count >= 1)
                        Globals.COUNT_MATRIX1++;

                    else
                        Globals.STEPS_MISSING1 += StepToCheck + ", ";

                    if (!string.IsNullOrEmpty(Globals.STEPS_MISSING1))
                    {

                    }
                }
            }
        }

    }
}
