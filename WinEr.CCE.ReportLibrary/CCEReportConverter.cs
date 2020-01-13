using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WinCCE_Convertion
{
    public class CCEReportConverter
    {
        public DataSet ConvertReport(DataSet _DataSet_OldFormat, DataTable _CCE_Config, DataTable _GradeTable, int ConvertionType)
        {
            switch (ConvertionType)
            {
                case 1:
                    return ConvertType1(_DataSet_OldFormat, _CCE_Config, _GradeTable);
                default:
                    return ConvertType1(_DataSet_OldFormat, _CCE_Config, _GradeTable);
            }
        }
         
        private DataSet ConvertType1(DataSet _DataSet_OldFormat, DataTable _CCE_Config, DataTable _GradeTable)
        {
            #region Createing Dataset
            DataSet _DataSet_NewFormat = new DataSet("CCE_Report_New");
            DataTable _DataTable_NewFormat = new DataTable("MarkTanle");
            _DataTable_NewFormat.Columns.Add("Sl.No");
            _DataTable_NewFormat.Columns.Add("Subject");
            _DataTable_NewFormat.Columns.Add("FA1");
            _DataTable_NewFormat.Columns.Add("FA2");
            _DataTable_NewFormat.Columns.Add("SA1");
            _DataTable_NewFormat.Columns.Add("Term1Total");
            _DataTable_NewFormat.Columns.Add("FA3");
            _DataTable_NewFormat.Columns.Add("FA4");
            _DataTable_NewFormat.Columns.Add("SA2");
            _DataTable_NewFormat.Columns.Add("Term2Total");
            _DataTable_NewFormat.Columns.Add("FATotal");
            _DataTable_NewFormat.Columns.Add("SATotal");
            _DataTable_NewFormat.Columns.Add("Over all");
            _DataTable_NewFormat.Columns.Add("Grade");
            #endregion

            #region Calulation and data Inserting
            try
            {
                if (_DataSet_OldFormat != null)
                {
                    DataTable dt = new DataTable();
                    dt = _DataSet_OldFormat.Tables["Mark"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        double FA1, FA2, FA3, FA4, SA1, SA2;
                        int i = 1;
                        GetFields(dr, _CCE_Config, out FA1, out FA2, out FA3, out FA4, out SA1, out SA2);
                        double FA_Total, SA_Total, Total;
                        string Grade;
                        FA_Total = FA1 + FA2 + FA3 + FA4; SA_Total = SA1 + SA2; Total = FA_Total + SA_Total;
                        Grade = GetGrade(_GradeTable, Total);
                        _DataTable_NewFormat.Rows.Add(i, 2, FA1, FA2, SA1, FA1 + FA2 + SA1, FA3, FA4, SA2, FA3 + FA4 + SA2, FA_Total, SA_Total, FA1 + FA2 + FA3 + FA4 + SA1 + SA2, Grade);
                        i++;
                    }
                    _DataSet_NewFormat.Tables.Add(_DataTable_NewFormat);
                }
            }
            catch
            {
                _DataSet_NewFormat = null;
            }
            return _DataSet_NewFormat;
            #endregion
        }        

        public void GetFields(DataRow _TableRow,DataTable _Config, out double FA1, out double FA2, out double FA3, out double FA4, out double SA1, out double SA2)
        {
            FA1 = 0; FA2 = 0; FA3 = 0; FA4 = 0; SA1 = 0; SA2 = 0;
            double _TotalMark, _MaxMark;
            foreach (DataRow dr in _Config.Rows)
            {
                switch (dr["ExamName"].ToString())
                {
                    case "FA1":
                        _TotalMark = double.Parse(_TableRow[dr["ColName"].ToString()].ToString());
                        _MaxMark = double.Parse(dr["ExamMaxMark"].ToString());
                        FA1=GetMark(_TotalMark,_MaxMark,10);
                        break;
                    case "FA2":
                        _TotalMark = double.Parse(_TableRow[dr["ColName"].ToString()].ToString());
                        _MaxMark = double.Parse(dr["ExamMaxMark"].ToString());
                        FA2 = GetMark(_TotalMark, _MaxMark, 10);
                        break;
                    case "FA3":
                        _TotalMark = double.Parse(_TableRow[dr["ColName"].ToString()].ToString());
                        _MaxMark = double.Parse(dr["ExamMaxMark"].ToString());
                        FA3 = GetMark(_TotalMark, _MaxMark, 10);
                        break;
                    case "FA4":
                        _TotalMark = double.Parse(_TableRow[dr["ColName"].ToString()].ToString());
                        _MaxMark = double.Parse(dr["ExamMaxMark"].ToString());
                        FA4 = GetMark(_TotalMark, _MaxMark, 10);
                        break;
                    case "SA1":
                        _TotalMark = double.Parse(_TableRow[dr["ColName"].ToString()].ToString());
                        _MaxMark = double.Parse(dr["ExamMaxMark"].ToString());
                        SA1 = GetMark(_TotalMark, _MaxMark, 30);
                        
                        break;
                    case "SA2":
                        _TotalMark = double.Parse(_TableRow[dr["ColName"].ToString()].ToString());
                        _MaxMark = double.Parse(dr["ExamMaxMark"].ToString());
                        SA2 = GetMark(_TotalMark, _MaxMark, 30);                        
                        break;
                }                
            }
        }

        double GetMark(double TotalMark, double MaxMark, int OutPutPersentage)
        {
            double Mark = 0.0;
            Mark = (TotalMark / MaxMark) * OutPutPersentage;
            Mark = Math.Round((double)Mark, 2);
            return Mark;
        }

        private string GetGrade(DataTable _GradeTable, double TotalMark)
        {
            string Grade="NA";
            foreach (DataRow dr in _GradeTable.Rows)
            {
                if (double.Parse(dr["LowerLimit"].ToString()) <= TotalMark)
                {
                    Grade = dr["Grade"].ToString();
                    break;
                }
            }
            return Grade;
        }
    }
}