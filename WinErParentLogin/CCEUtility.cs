using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WinBase;
using MigraDoc.DocumentObjectModel;
using System.Xml;
using System.IO;
using WC.PDF.Document;
using System.Xml.Serialization;
using WC.PdfDocumentClass;
using System.Drawing;

namespace WinErParentLogin
{
    public class CCEUtility
    {
        private ParentInfoClass m_MyParentInfo;
        private MysqlClass m_MysqlDb;
        private Dictionary<string, DataSet> m_Dataset_Dic;
 

        public string m_PdfPysicalPath;
        public int BatchId;
        public string BatchName;
        public int ClassId;
        public string ClassName;
        public int TermId;
        public string TermName;
        public string xmlstring;
        public string Rollno;


        public CCEUtility(MysqlClass MyStudMang, ParentInfoClass MyParentInfo,Dictionary<string, DataSet> Dataset_Dic)
        {
            m_MysqlDb = MyStudMang;
            m_MyParentInfo = MyParentInfo;
            m_Dataset_Dic = Dataset_Dic;
        }

        #region Function

        internal bool Exporttopdfdocument(out Document PDFdocument,out string Errmsg)
        {
            bool run = false;
            Errmsg = "";
            PDFdocument = new Document();
            var ser = new XmlSerializer(typeof(WCpdfDocument));
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlstring)))
            {
                WCpdfDocument wrapper = (WCpdfDocument)ser.Deserialize(reader);
                DocumentCreator docObj = null;
                PageSetup pgSetup = PDFdocument.DefaultPageSetup;
                docObj = new DocumentCreator();
                docObj.DocDefinition = wrapper;

                Dictionary<string, string> Img_Dic= new Dictionary<string, string>();
                if (m_Dataset_Dic .Count== 0)
                    Errmsg = "Academic Performance are not found!";
                else
                {
                    if (!Directory.Exists(m_PdfPysicalPath))
                        Directory.CreateDirectory(m_PdfPysicalPath);
                    try
                    {
                        docObj.DatasetDictionary = m_Dataset_Dic;
                        docObj.dataDictionary = LoadStudentinformation();
                        Img_Dic.Add("@@reportcardtittlebaricon@@", m_PdfPysicalPath + "UpImage\\reportcard.png");
                        Img_Dic.Add("@@studentstampicon@@", m_PdfPysicalPath + "UpImage\\studentstampicon.png");
                        loadImageDictionary(m_MyParentInfo.StudentId, ref Img_Dic);
                        docObj.imageUrlDictionary = Img_Dic;
                    }
                    catch (Exception ex)
                    {
                        Errmsg = ex.Message;
                    }

                    if (Errmsg == "")
                    {
                        if (!docObj.CreateDocument(ref PDFdocument, pgSetup))
                            Errmsg = "Pdf is not created Sucessfully!";
                        else
                            run = true;
                    }
                }

            }
            if (Errmsg != "")
                run = false;

            return run;
        }

        private Dictionary<string, string> LoadStudentinformation()
        {
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

            string Stuaddress = "", Studob = "", Stuphono = "",mothername="";
            GetStudentInformation(out Stuaddress, out Studob, out Stuphono, out mothername);
            dataDictionary.Add("@@Line@@", "_");
            dataDictionary.Add("@@and@@", " & ");
            dataDictionary.Add("@@studentname@@", m_MyParentInfo.StudentName);
            dataDictionary.Add("@@schooladdress@@",GetSchooladdress(m_MyParentInfo.SchoolObject.SchoolId));
            dataDictionary.Add("@@Year@@",BatchName);
            dataDictionary.Add("@@class@@", ClassName);
            dataDictionary.Add("@@admissionno@@", m_MyParentInfo.ADMISSIONNO);
            dataDictionary.Add("@@dateofbirth@@", Studob);
            dataDictionary.Add("@@rollno@@", Rollno);
            dataDictionary.Add("@@mothername@@", mothername);
            dataDictionary.Add("@@fathername@@", m_MyParentInfo.ParentName);
            dataDictionary.Add("@@address@@", Stuaddress);
            dataDictionary.Add("@@telephoneno@@", Stuphono);
            string _TotalPercentdayTerm1 = "", _TotalPercentdayTerm2 = "", _TotalPercentdayTerm3 = "", _TotalWorkingdaysTerm1 = "", _TotalWorkingdaysTerm2 = "", _TotalWorkingdaysTerm3 = "";
            GetAttendanceDetails(m_MyParentInfo.StudentId, out _TotalPercentdayTerm1, out _TotalPercentdayTerm2, out _TotalPercentdayTerm3, out _TotalWorkingdaysTerm1, out _TotalWorkingdaysTerm2, out _TotalWorkingdaysTerm3);
            dataDictionary.Add("@@TotalPercentdayTerm1@@", _TotalPercentdayTerm1);
            dataDictionary.Add("@@TotalPercentdayTerm2@@", _TotalPercentdayTerm2);
            dataDictionary.Add("@@TotalPercentdayTerm3@@", _TotalPercentdayTerm3);
            dataDictionary.Add("@@TotalWorkingdaysTerm1@@", _TotalWorkingdaysTerm1);
            dataDictionary.Add("@@TotalWorkingdaysTerm2@@", _TotalWorkingdaysTerm2);
            dataDictionary.Add("@@TotalWorkingdaysTerm3@@", _TotalWorkingdaysTerm3);
            dataDictionary.Add("@@HEALTH INFORMATION@@", "Height______________________________Weight______________________________BloodGroup______________________________/nbspVision(R)______________________________(L)______________________________Dental Hygiene_____________________________");

            return dataDictionary;
        }

        private string GetSchooladdress(int Id)
        {
            string Schname = "";
            string sql = "SELECT tblschooldetails.Address from tblschooldetails";
            DataSet Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count == 0)
                Schname = "Not founded";
            else
                Schname= Ds.Tables[0].Rows[0].ToString();
            return Schname;
        }

        private void GetStudentInformation(out string Stuaddress, out string Studob, out string Stuphono, out string mothername)
        {
            Stuaddress="";
            Studob="";
            Stuphono="";
            mothername="";
            string sql = "SELECT tblstudent.Addresspresent,tblstudent.DOB,tblstudent.OfficePhNo,tblstudent.MothersName,tblstudent.GardianName,tblstudent.AdmitionNo,tblstudent.RollNo from tblstudent where tblstudent.Id=" + m_MyParentInfo.StudentId;
            DataSet ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Stuaddress = dr[0].ToString();
                    Stuphono = dr[2].ToString();
                    mothername = dr[3].ToString();

                    #region DOB
                    string[] datestr = dr[1].ToString().Split('/');
                    for (int i = 0; i < datestr.Count(); i++)
                    {
                        if (i == datestr.Count() - 1)
                        {
                            string[] _str = datestr[i].ToString().Split(' ');

                            Studob += _str[0];
                        }
                        else
                            Studob += datestr[i].ToString() + "/";
                    }
                   #endregion

                }
            }

        }

        private void GetAttendanceDetails(int StudentId, out string _TotalPercentdayTerm1, out string _TotalPercentdayTerm2, out string _TotalPercentdayTerm3, out string _TotalWorkingdaysTerm1, out string _TotalWorkingdaysTerm2, out string _TotalWorkingdaysTerm3)
        {
            _TotalPercentdayTerm1 = "Error";
            _TotalPercentdayTerm2 = "Error";
            _TotalPercentdayTerm3 = "Error";
            _TotalWorkingdaysTerm1 = "Error";
            _TotalWorkingdaysTerm2 = "Error";
            _TotalWorkingdaysTerm3 = "Error";
            Attendance _objattendance = new Attendance(m_MysqlDb);

            int _no_workingdays = 0, _no_fulldays = 0, _no_absentdays = 0, _no_holidays = 0, _no_halfdays = 0;
            double _attendencepersent = 0.0;
            DateTime _startDate;
            DateTime _endingDate;
            string sql = "SELECT tblcce_term.StartindDate,tblcce_term.EndingDate FROM  tblcce_term WHERE tblcce_term.Id in (1,2,3)";
            DataSet returnDataset = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            int i = 0;
            if (returnDataset != null || returnDataset.Tables[0] != null || returnDataset.Tables[0].Rows.Count != 0)
            {

                foreach (DataRow dr in returnDataset.Tables[0].Rows)
                {
                    _startDate = Convert.ToDateTime(dr[0].ToString());
                    _startDate = Convert.ToDateTime(_startDate.ToString("s"));
                    _endingDate = Convert.ToDateTime(dr[1].ToString());
                    _endingDate = Convert.ToDateTime(_endingDate.ToString("s"));
                    if (_objattendance.GetCurrentBatchNewattendanceDetailsWithDate(StudentId, out _no_workingdays, out _no_fulldays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent,BatchId, _startDate, _endingDate))
                    {
                        if (i == 0)
                        {
                            _TotalPercentdayTerm1 = Convert.ToString(_no_fulldays + (_no_halfdays / 2));
                            _TotalWorkingdaysTerm1 = Convert.ToString(_no_workingdays);
                        }
                        else if (i == 1)
                        {
                            _TotalPercentdayTerm2 = Convert.ToString(_no_fulldays + (_no_halfdays / 2)); ;
                            _TotalWorkingdaysTerm2 = Convert.ToString(_no_workingdays);

                        }
                        else
                        {

                            _TotalPercentdayTerm3 = Convert.ToString(_no_fulldays + (_no_halfdays / 2)); ;
                            _TotalWorkingdaysTerm3 = Convert.ToString(_no_workingdays);
                        }

                    }
                    i++;
                }

            }
        }

        private void loadImageDictionary(int StuId, ref Dictionary<string, string> Img_Dic)
        {
            string Image = "";
            string sql = "";
            string Keywords = "";
            DataSet Ds = null;
            string Path = m_PdfPysicalPath;
            if (!Directory.Exists(Path + "Image\\"))
                Directory.CreateDirectory(Path + "Image\\");
            if (!Directory.Exists(Path + "TempImg\\"))
                Directory.CreateDirectory(Path + "TempImg\\");

            #region student image

            bool stuimg = false;
            sql = "select tblfileurl.FileBytes from tblfileurl where tblfileurl.UserId="+m_MyParentInfo.StudentId;
            Keywords = "@@studentimgurl@@";
            Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                #region history student

                Ds = null;
                sql = "select tblfileurl_history.FileBytes from tblfileurl_history where tblfileurl_history.UserId=" + m_MyParentInfo.StudentId;
                Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (Ds.Tables[0].Rows.Count == 0)
                    Img_Dic.Add(Keywords, Path + "Image\\stdnt.png");
                else
                    stuimg = true;

                #endregion
            }
            else
                stuimg = true;

            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                if (dr[0].ToString() == "" || dr[0].ToString() == null)
                    Img_Dic.Add(Keywords, Path + "Image\\stdnt.png");
                else
                    stuimg = true;
            }

            if (stuimg)
            {
                try
                {
                    DataRow dr = Ds.Tables[0].Rows[0];
                    byte[] image = (byte[])dr[0];
                    Image = GetImageLinkForBytes(image, 0);
                    if (Image == "")
                        Img_Dic.Add(Keywords, Path + "Image\\stdnt.png");
                    else
                        Img_Dic.Add(Keywords, Image);
                }
                catch
                {
                    stuimg = false;
                }
            }

            #endregion

            #region school image
            Ds = null;
            bool schimg=true;
            sql = "SELECT tblschooldetails.Logo from tblschooldetails";
            Keywords = "@@schoollogourl@@";
            Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count == 0)
                schimg = false;
            else
            {
                try
                {
                      foreach (DataRow dr in Ds.Tables[0].Rows)
                      {
                          if (dr[0].ToString() == "" || dr[0].ToString() == null)
                              schimg = false;
                          else
                          {
                              byte[] image = (byte[])dr[0];
                              Image = GetImageLinkForBytes(image, 0);
                              if (Image=="")
                                  Img_Dic.Add(Keywords, Path + "Image\\school.png");
                              else
                                  Img_Dic.Add(Keywords, Image);
                          }
                      }
                }
                catch
                {
                    schimg = false;
                }
            }

            if (!schimg)
            {
                Img_Dic.Add(Keywords, Path + "Image\\school.png");
            }
            #endregion

        }

        private string GetImageLinkForBytes(byte[] _Imagebytes, int Id)
        {
            string IMagepath = "";
            if (_Imagebytes != null)
            {
                string path = m_PdfPysicalPath + "TempImg";
                IMagepath = @path + "\\" + GetDateString(DateTime.Now) + "_" + Id + ".jpeg";
                try
                {
                    System.Drawing.Image image;
                    var inputStream = new MemoryStream(_Imagebytes);
                    image = System.Drawing.Image.FromStream(inputStream);
                    using (System.Drawing.Image Img = image)
                    {
                        Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 100);
                        using (System.Drawing.Image ImgThnail =
                        new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                        {
                            ImgThnail.Save(IMagepath, Img.RawFormat);
                            ImgThnail.Dispose();
                        }
                        Img.Dispose();
                    }
                }
                catch (Exception _Exception)
                {
                    // Error
                    IMagepath="";
                }
            }
            return IMagepath;
        }

        public static String GetDateString(DateTime _dt)
        {
            return _dt.Year + "_" + _dt.Month + "_" + _dt.Day + "_" + _dt.Hour + "_" + _dt.Minute + "_" + _dt.Second;
        }

        private static Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
        {
            Size NewSize;
            double tempval;

            if (OriginalHeight > FormatSize && OriginalWidth > FormatSize)
            {
                if (OriginalHeight > OriginalWidth)
                    tempval = FormatSize / Convert.ToDouble(OriginalHeight);
                else
                    tempval = FormatSize / Convert.ToDouble(OriginalWidth);

                NewSize = new Size(Convert.ToInt32(tempval * OriginalWidth), Convert.ToInt32(tempval * OriginalHeight));
            }
            else
                NewSize = new Size(OriginalWidth, OriginalHeight); return NewSize;
        }

        #endregion

    }
}
