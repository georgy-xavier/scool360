using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data;
using WC.PDF.Document;
using System.Xml.Serialization;
using WC.PdfDocumentClass;
using MigraDoc.DocumentObjectModel;
using System.Xml;
using System.Data.Odbc;


namespace WinBase
{
   public class CCEUtility
    {
        private SchoolClass m_objSchool = null;
        private Dictionary<string, DataSet> m_Dataset_Dic;
        private KnowinUser m_MyUser;
        private int m_StudentId = 0;

    

        public string m_PdfPysicalPath;
        public int BatchId;
        public string BatchName;
        public int ClassId;
        public string ClassName;
        public int TermId;
        public string TermName;
        public string xmlstring;
        public string m_DefaultImgpath;
        public string m_TempImgpath;

        public string m_PerformanceChartURI;

        public CCEUtility(KnowinUser MyUser,SchoolClass objSchool,Dictionary<string, DataSet> Dataset_Dic, int StudentId)
        {
            m_MyUser = MyUser;
            m_objSchool = objSchool;
            m_Dataset_Dic = Dataset_Dic;
            m_StudentId = StudentId;
        }

        public CCEUtility()
        {
        }

        #region Function

        private string GetSchooladdress(int Id)
        {
            string Schname = "";
            string sql = "SELECT tblschooldetails.Address from tblschooldetails";
            DataSet Ds = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count == 0)
                Schname = "Not founded";
            else
                Schname = Ds.Tables[0].Rows[0][0].ToString();
            return Schname;
        }

        public bool Exporttopdfdocument(out Document PDFdocument, out string Errmsg)
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

                Dictionary<string, string> Img_Dic = new Dictionary<string, string>();
                if (m_Dataset_Dic.Count == 0)
                    Errmsg = "Academic Performance are not found!";
                else
                {
                    if (!Directory.Exists(m_DefaultImgpath))
                        Directory.CreateDirectory(m_DefaultImgpath);
                    try
                    {
                        docObj.DatasetDictionary = m_Dataset_Dic;
                        docObj.dataDictionary = LoadStudentinformation();
                        Img_Dic.Add("@@reportcardtittlebaricon@@", m_DefaultImgpath + "\\reportcard.png");
                        Img_Dic.Add("@@studentstampicon@@", m_DefaultImgpath + "\\studentstampicon.png");
                        loadImageDictionary(m_StudentId, ref Img_Dic);
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

            string Stuaddress = "", Studob = "", Stuphono = "", mothername = "", fathername = "", StudentName = "", ADMISSIONNO = "", Rollno="";
            GetStudentInformation(out Stuaddress, out Studob, out Stuphono, out mothername, out fathername, out StudentName, out ADMISSIONNO, out  Rollno);
            dataDictionary.Add("@@Line@@", "_");
            dataDictionary.Add("@@and@@", " & ");
            dataDictionary.Add("@@date@@", DateTime.Today.Date.ToString("dd/MM/yyyy"));
            dataDictionary.Add("@@studentname@@", StudentName);
            dataDictionary.Add("@@schooladdress@@", GetSchooladdress(m_objSchool.SchoolId));
            dataDictionary.Add("@@schoolname@@",m_objSchool.SchoolName);
            dataDictionary.Add("@@termname@@", TermName);
            dataDictionary.Add("@@Year@@", BatchName);
            dataDictionary.Add("@@class@@", ClassName);
            dataDictionary.Add("@@admissionno@@", ADMISSIONNO);
            dataDictionary.Add("@@dateofbirth@@", Studob);
            dataDictionary.Add("@@rollno@@", Rollno);
            dataDictionary.Add("@@mothername@@", mothername);
            dataDictionary.Add("@@fathername@@", fathername);
            dataDictionary.Add("@@address@@", Stuaddress);
            dataDictionary.Add("@@telephoneno@@", Stuphono);
            string _TotalPercentdayTerm1 = "", _TotalPercentdayTerm2 = "", _TotalPercentdayTerm3 = "", _TotalWorkingdaysTerm1 = "", _TotalWorkingdaysTerm2 = "", _TotalWorkingdaysTerm3 = "";
            GetAttendanceDetails(m_StudentId, out _TotalPercentdayTerm1, out _TotalPercentdayTerm2, out _TotalPercentdayTerm3, out _TotalWorkingdaysTerm1, out _TotalWorkingdaysTerm2, out _TotalWorkingdaysTerm3);
            dataDictionary.Add("@@TotalPercentdayTerm1@@", _TotalPercentdayTerm1);
            dataDictionary.Add("@@TotalPercentdayTerm2@@", _TotalPercentdayTerm2);
            dataDictionary.Add("@@TotalPercentdayTerm3@@", _TotalPercentdayTerm3);
            dataDictionary.Add("@@TotalWorkingdaysTerm1@@", _TotalWorkingdaysTerm1);
            dataDictionary.Add("@@TotalWorkingdaysTerm2@@", _TotalWorkingdaysTerm2);
            dataDictionary.Add("@@TotalWorkingdaysTerm3@@", _TotalWorkingdaysTerm3);
            dataDictionary.Add("@@Term1Remark@@", GetRemark("Term1"));
            dataDictionary.Add("@@Term2Remark@@", GetRemark("Term2"));
            dataDictionary.Add("@@HEALTH INFORMATION@@", "Height______________________________Weight______________________________BloodGroup______________________________/nbspVision(R)______________________________(L)______________________________Dental Hygiene_____________________________");

            return dataDictionary;
        }

        private string GetRemark(string _Term)
        {
            bool valid = false;
            string remark = "";
            string Tablename = "tblcce_descriptive";
            int Classid = ClassId;
            int Batchid = BatchId;

            string sql = "SELECT tblcce_parts.Id,tblcce_parts.Description,tblcce_parts.FooterDesc from tblcce_parts";
            DataSet Ds_Part = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds_Part.Tables[0].Rows.Count == 0)
                valid = false;
            else
            {
                foreach (DataRow drpart in Ds_Part.Tables[0].Rows)
                {
                    sql = "SELECT DISTINCT tblsubjects.Id,tblsubjects.subject_name from tblsubjects INNER JOIN tblcce_subjectskillmap  on tblsubjects.Id=tblcce_subjectskillmap.SubjectId WHERE tblcce_subjectskillmap.PartId=" + int.Parse(drpart["Id"].ToString()) + " AND tblcce_subjectskillmap.ClassId=" + Classid + "";
                    DataSet Ds_Subject = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Ds_Subject.Tables[0].Rows.Count == 0)
                        valid = false;
                    else
                    {                        
                        foreach (DataRow drsubject in Ds_Subject.Tables[0].Rows)
                        {
                            sql = "SELECT DISTINCT(tblcce_subjectskills.SkillName) as Temp,tblcce_subjectskills.SkillName," + Tablename + ".DescriptiveIndicator," + Tablename + ".Term1," + Tablename + ".Term2," + Tablename + ".Term3 from " + Tablename + " inner JOIN tblcce_subjectskills on " + Tablename + ".SkillId=tblcce_subjectskills.Id inner JOIN tblcce_subjectskillmap on tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where " + Tablename + ".SubjectId=" + int.Parse(drsubject["Id"].ToString()) + " AND " + Tablename + ".StudentId=" + m_StudentId + " AND tblcce_subjectskills.Id =100 order by tblcce_subjectskillmap.SkillOrder";
                            OdbcDataReader Dr_Remark = m_MyUser.m_MysqlDb.ExecuteQuery(sql);
                            while (Dr_Remark.Read())
                            {
                                remark = Dr_Remark[_Term].ToString();
                            }
                            
                        }
                        valid = true;
                    }

                }
            }
            return remark;
            throw new NotImplementedException();
        }
        
        private void GetStudentInformation(out string Stuaddress, out string Studob, out string Stuphono, out string mothername, out string fathername, out string StudentName, out string ADMISSIONNO, out string Rollno)
        {
            Stuaddress = "";
            Studob = "";
            Stuphono = "";
            mothername = "";
            fathername = "";
            StudentName = "";
            ADMISSIONNO = "";
            Rollno = "";
            string sql = "SELECT tblstudent.Addresspresent,tblstudent.DOB,tblstudent.OfficePhNo,tblstudent.MothersName,tblstudent.GardianName,tblstudent.AdmitionNo,tblstudent.RollNo,tblstudent.StudentName from tblstudent where tblstudent.Id=" + m_StudentId;
            DataSet ds = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Stuaddress = dr[0].ToString();
                    Stuphono = dr[2].ToString();
                    mothername = dr[3].ToString();
                    fathername = dr[4].ToString();
                    ADMISSIONNO = dr[5].ToString();
                    Rollno = dr[6].ToString();
                    StudentName = dr[7].ToString();
                  

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
            Attendance _objattendance = new Attendance(m_MyUser.m_MysqlDb);

            int _no_workingdays = 0, _no_fulldays = 0, _no_absentdays = 0, _no_holidays = 0, _no_halfdays = 0;
            double _attendencepersent = 0.0;
            DateTime _startDate;
            DateTime _endingDate;
            string sql = "SELECT tblcce_term.StartindDate,tblcce_term.EndingDate FROM  tblcce_term WHERE tblcce_term.Id in (1,2,3)";
            DataSet returnDataset = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            int i = 0;
            if (returnDataset != null || returnDataset.Tables[0] != null || returnDataset.Tables[0].Rows.Count != 0)
            {

                foreach (DataRow dr in returnDataset.Tables[0].Rows)
                {
                    _startDate = Convert.ToDateTime(dr[0].ToString());
                    _startDate = Convert.ToDateTime(_startDate.ToString("s"));
                    _endingDate = Convert.ToDateTime(dr[1].ToString());
                    _endingDate = Convert.ToDateTime(_endingDate.ToString("s"));
                    if (_objattendance.GetCurrentBatchNewattendanceDetailsWithDate(StudentId, out _no_workingdays, out _no_fulldays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, BatchId, _startDate, _endingDate))
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

            if (!Directory.Exists(m_DefaultImgpath))
                Directory.CreateDirectory(m_DefaultImgpath);

            if (!Directory.Exists(m_TempImgpath))
                Directory.CreateDirectory(m_TempImgpath);

            #region student image

            bool stuimg = false;
            sql = "select tblfileurl.FileBytes from tblfileurl where tblfileurl.UserId=" + m_StudentId;
            Keywords = "@@studentimgurl@@";
            Ds = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                #region history student

                Ds = null;
                sql = "select tblfileurl_history.FileBytes from tblfileurl_history where tblfileurl_history.UserId=" +m_StudentId;
                Ds = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (Ds.Tables[0].Rows.Count == 0)
                    Img_Dic.Add(Keywords, m_DefaultImgpath + "\\stdnt.png");
                else
                    stuimg = true;

                #endregion
            }
            else
                stuimg = true;

            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                if (dr[0].ToString() == "" || dr[0].ToString() == null)
                    Img_Dic.Add(Keywords, m_DefaultImgpath + "\\stdnt.png");
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
                        Img_Dic.Add(Keywords, m_DefaultImgpath + "\\stdnt.png");
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
            bool schimg = true;
            sql = "SELECT tblschooldetails.Logo from tblschooldetails";
            Keywords = "@@schoollogourl@@";
            Ds = m_MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
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
                            if (Image == "")
                                Img_Dic.Add(Keywords, m_DefaultImgpath + "\\school.png");
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
                Img_Dic.Add(Keywords, m_DefaultImgpath + "\\school.png");
            }
            #endregion

        }

        private string GetImageLinkForBytes(byte[] _Imagebytes, int Id)
        {
            string IMagepath = "";
            if (_Imagebytes != null)
            {
                if (!Directory.Exists(m_TempImgpath))
                    Directory.CreateDirectory(m_TempImgpath);

                IMagepath = @m_TempImgpath + "\\" + GetDateString(DateTime.Now) + "_" + Id + ".jpeg";
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
                    IMagepath = "";
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

        public bool Exporttopdfdocument(ref Document PDFdocument, PageSetup pgSetup, out string Errmsg)
        {
            bool run = false;
            Errmsg = "";

                #region XML CCE Report Creation

                var ser = new XmlSerializer(typeof(WCpdfDocument));
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlstring)))
                {
                    WCpdfDocument wrapper = (WCpdfDocument)ser.Deserialize(reader);
                    DocumentCreator docObj = null;
                    docObj = new DocumentCreator();
                    docObj.DocDefinition = wrapper;
                    Dictionary<string, string> Img_Dic = new Dictionary<string, string>();
                    if (m_Dataset_Dic.Count == 0)
                        Errmsg = "Academic Performance are not found!";
                    else
                    {
                        if (!Directory.Exists(m_PdfPysicalPath))
                            Directory.CreateDirectory(m_PdfPysicalPath);
                        try
                        {
                            docObj.DatasetDictionary = m_Dataset_Dic;
                            docObj.dataDictionary = LoadStudentinformation();
                            if (!string.IsNullOrEmpty(m_PerformanceChartURI))
                                Img_Dic.Add("@@studentperformancechart@@", m_PerformanceChartURI);
                            Img_Dic.Add("@@reportcardtittlebaricon@@", m_DefaultImgpath + "\\reportcard.png");
                            Img_Dic.Add("@@studentstampicon@@", m_DefaultImgpath + "\\studentstampicon.png");
                            loadImageDictionary(m_StudentId, ref Img_Dic);
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
                #endregion
            
            if (Errmsg != "")
                run = false;

            return run;
        }

        
    }
}
