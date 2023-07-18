using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckProcess.Clases
{
    class StaticMESFuntions
    {
        public static List<string> ListByPanel(string SerialNumber)
        {
            List<string> _ListByPanel = new List<string>();
            DataTable _dtPanel = new DataTable();

            DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);
            int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
            int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

            if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
            if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

            foreach(DataRow dr in _dtPanel.Rows)
                _ListByPanel.Add(dr[3].ToString());

            return _ListByPanel;
        }


        public static async Task<DataTable> VerifyCheckPoint(List<string> SerialNumbers)
        {
            DataTable _dtResult = new DataTable();
            DataTable _dtPanel = new DataTable();

            _dtPanel.Columns.Add("SerialNumber");
            _dtPanel.Columns.Add("Mapping");
            _dtPanel.Columns.Add("History");
            _dtPanel.Columns.Add("Status");
            _dtPanel.Columns.Add("DoubleLoop");
            foreach (string SerialNumber in SerialNumbers) _dtPanel.Rows.Add(SerialNumber);

            _dtResult = _dtPanel.Copy();


            DataTable EventsByLoop_2 = new DataTable();
            bool _Had2Loops = false;

            foreach (string SerialNumber in SerialNumbers) 
            {
                DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);
               
                int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
                int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

                //if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
                //if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];
                    
                DataTable EventsByStepMatrix = new DataTable();

                foreach (DataRow _dr in _dtPanel.Rows)
                {
                    _dsQuery = new MES.Service().BoardHistoryReport(_dr[0].ToString(), _CustomerID);

                    foreach (string _Key in Globals.DATA_MATRIX)
                    {
                        string StepToCheck = ConfigFiles.reader("DATA_MATRIX", _Key, Globals.CONFIG_FILE);

                        try
                        {
                            EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                         .Where(r => r.Field<string>("Test_Process") == "QC / MRB")
                                         .CopyToDataTable();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                  .Select(b => b["History"] = "QC / MRB")
                                                 .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                  .Select(b => b["History"] = Globals.STEP_TO_CHECK)
                                                 .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                         .Select(b => b["History"] = StepToCheck)
                                                        .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                     .Select(b => b["Status"] = "Fail")
                                    .ToList();


                            EventsByLoop_2 = _dsQuery.Tables[0].AsEnumerable()
                                                     .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                 r.Field<string>("Test_Process") == "FVT / PBTS" &&
                                                                 r.Field<string>("TestStatus") == "Fail" &&
                                                                 r.Field<string>("EquipmentRouteName") == "Loop 2")
                                                     .CopyToDataTable();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                         .Select(b => b["History"] = StepToCheck)
                                                        .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                     .Select(b => b["Status"] = "Pass")
                                    .ToList();
                        }
                        catch (Exception)
                        {
                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                                   .Select(b => b["History"] = StepToCheck)
                                                                  .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                .Select(b => b["Status"] = "Missing Step")
                                               .ToList();

                            break;
                        }
                    }
                }

            }

            return _dtResult;
        }


        public static async Task<DataTable> VerifyCheckPoint_2(List<string> SerialNumbers)
        {
            DataTable _dtResult = new DataTable();
            DataTable _dtPanel = new DataTable();

            _dtPanel.Columns.Add("SerialNumber");
            _dtPanel.Columns.Add("Mapping");
            _dtPanel.Columns.Add("History");
            _dtPanel.Columns.Add("Status");
            _dtPanel.Columns.Add("DoubleLoop");
            foreach (string SerialNumber in SerialNumbers) _dtPanel.Rows.Add(SerialNumber);

            _dtResult = _dtPanel.Copy();


            DataTable EventsByLoop_2 = new DataTable();
            bool _Had2Loops = false;

            foreach (string SerialNumber in SerialNumbers)
            {
                DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);

                int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
                int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

                //if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
                //if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

                DataTable EventsByStepMatrix = new DataTable();

                foreach (DataRow _dr in _dtPanel.Rows)
                {
                    _dsQuery = new MES.Service().BoardHistoryReport(_dr[0].ToString(), _CustomerID);

                    foreach (string _Key in Globals.DATA_MATRIX)
                    {
                        string StepToCheck = ConfigFiles.reader("DATA_MATRIX", _Key, Globals.CONFIG_FILE);

                        try
                        {
                            EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                         .Where(r => r.Field<string>("Test_Process") == "QC / MRB")
                                         .CopyToDataTable();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                  .Select(b => b["History"] = "QC / MRB")
                                                 .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                  .Select(b => b["History"] = Globals.STEP_TO_CHECK)
                                                 .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                         .Select(b => b["History"] = StepToCheck)
                                                        .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                     .Select(b => b["Status"] = "Fail")
                                    .ToList();


                            EventsByLoop_2 = _dsQuery.Tables[0].AsEnumerable()
                                                     .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                 r.Field<string>("Test_Process") == "FVT / PBTS" &&
                                                                 r.Field<string>("TestStatus") == "Fail" &&
                                                                 r.Field<string>("EquipmentRouteName") == "Loop 2")
                                                     .CopyToDataTable();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                         .Select(b => b["History"] = StepToCheck)
                                                        .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                     .Select(b => b["Status"] = "Pass")
                                    .ToList();
                        }
                        catch (Exception)
                        {
                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                                   .Select(b => b["History"] = StepToCheck)
                                                                  .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                .Select(b => b["Status"] = "Missing Step")
                                               .ToList();

                            break;
                        }
                    }
                }

            }

            return _dtResult;
        }

        public static async Task<DataTable> VerifyCheckPoint_3(List<string> SerialNumbers)
        {
            DataTable _dtResult = new DataTable();
            DataTable _dtPanel = new DataTable();

            _dtPanel.Columns.Add("SerialNumber");
            _dtPanel.Columns.Add("Mapping");
            _dtPanel.Columns.Add("History");
            _dtPanel.Columns.Add("Status");
            _dtPanel.Columns.Add("DoubleLoop");
            foreach (string SerialNumber in SerialNumbers) _dtPanel.Rows.Add(SerialNumber);

            _dtResult = _dtPanel.Copy();


            DataTable EventsByLoop_2 = new DataTable();
            bool _Had2Loops = false;

            foreach (string SerialNumber in SerialNumbers)
            {
                DataSet _dsQuery = new MES.Service().SelectBySerialNumber(SerialNumber);

                int _CustomerID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][2]);
                int _WIP_ID = Convert.ToInt32(_dsQuery.Tables[0].Rows[0][0]);

                //if (SerialNumber.Length == Globals.SMOTHER_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID + 1).Tables[0];
                //if (SerialNumber.Length == Globals.SN_LENGH) _dtPanel = new MES.Service().ListByBoard(_WIP_ID).Tables[0];

                DataTable EventsByStepMatrix = new DataTable();

                foreach (DataRow _dr in _dtPanel.Rows)
                {
                    _dsQuery = new MES.Service().BoardHistoryReport(_dr[0].ToString(), _CustomerID);

                    foreach (string _Key in Globals.DATA_MATRIX)
                    {
                        string StepToCheck = ConfigFiles.reader("DATA_MATRIX", _Key, Globals.CONFIG_FILE);

                        try
                        {
                            EventsByStepMatrix = _dsQuery.Tables[0].AsEnumerable()
                                         .Where(r => r.Field<string>("Test_Process") == "QC / MRB")
                                         .CopyToDataTable();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                  .Select(b => b["History"] = "QC / MRB")
                                                 .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                  .Select(b => b["History"] = Globals.STEP_TO_CHECK)
                                                 .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                         .Select(b => b["History"] = StepToCheck)
                                                        .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                     .Select(b => b["Status"] = "Fail")
                                    .ToList();


                            EventsByLoop_2 = _dsQuery.Tables[0].AsEnumerable()
                                                     .Where(r => r.Field<string>("TestType") == "TEST" &&
                                                                 r.Field<string>("Test_Process") == "FVT / PBTS" &&
                                                                 r.Field<string>("TestStatus") == "Fail" &&
                                                                 r.Field<string>("EquipmentRouteName") == "Loop 2")
                                                     .CopyToDataTable();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
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

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                         .Select(b => b["History"] = StepToCheck)
                                                        .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                     .Select(b => b["Status"] = "Pass")
                                    .ToList();
                        }
                        catch (Exception)
                        {
                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                                   .Select(b => b["History"] = StepToCheck)
                                                                  .ToList();

                            _dtResult.AsEnumerable().Where(row => row.Field<string>("SerialNumber") == _dr[0].ToString())
                                                .Select(b => b["Status"] = "Missing Step")
                                               .ToList();

                            break;
                        }
                    }
                }

            }
            return _dtResult;
        }



    }
}
