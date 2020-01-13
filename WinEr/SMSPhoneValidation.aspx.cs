using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class SMSPhoneValidation : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(463))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    //some initlization
                    Load_DuplicateEntry();

                }
            }


        }

        # region DuplicatePhoneList


        protected void Btn_ReloadDuplicate_Click(object sender, EventArgs e)
        {
            Load_DuplicateEntry();
        }

        private void Load_DuplicateEntry()
        {
            lbl_duplicate_error.Text = "";
            string sql = "SELECT tbluser.Id,0 as _type,tbluser.UserName as Name,tblsmsstafflist.PhoneNo,tblsmsstafflist.Enabled FROM tbluser INNER JOIN tblsmsstafflist ON tbluser.Id=tblsmsstafflist.Id WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 ";

            sql = sql + " UNION " + "SELECT tblstudentclassmap.StudentId as Id,1 as _type,tblstudent.GardianName as Name,tblsmsparentlist.PhoneNo,tblsmsparentlist.Enabled FROM tblstudentclassmap INNER JOIN tblsmsparentlist ON tblstudentclassmap.StudentId=tblsmsparentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId ";

            sql = sql + " UNION " + "SELECT tblstudentclassmap.StudentId as Id,2 as _type,tblstudent.StudentName as Name,tblsmsstudentlist.PhoneNo,tblsmsstudentlist.Enabled FROM tblstudentclassmap INNER JOIN tblsmsstudentlist ON tblstudentclassmap.StudentId=tblsmsstudentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.`Status`=1  ORDER BY PhoneNo";
            
            MydataSet = GetDuplicateDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_DuplicateEntries.Columns[0].Visible = true;
                Grd_DuplicateEntries.DataSource = MydataSet;
                Grd_DuplicateEntries.DataBind();
                Grd_DuplicateEntries.Columns[0].Visible = false;
                Load_RepresentDuplicateColor();
            }
            else
            {
                Grd_DuplicateEntries.DataSource = null;
                Grd_DuplicateEntries.DataBind();
                lbl_duplicate_error.Text = "No duplicate number found";
            }
        }

        private void Load_RepresentDuplicateColor()
        {
            string phoneNumber = "";
            int color = 150000;
            foreach (GridViewRow gv in Grd_DuplicateEntries.Rows)
            {
                if (gv.Cells[2].Text == phoneNumber)
                {
                    gv.Cells[2].ForeColor = HexStringToColor("#" + color.ToString());
                }
                else
                {
                    color = color + 166666;
                    phoneNumber=gv.Cells[2].Text;
                    gv.Cells[2].ForeColor = HexStringToColor("#" + color.ToString());
                }
            }
        }

        private System.Drawing.Color HexStringToColor(string hex)
        {
            hex = hex.Replace("#", "");

            //if (hex.Length != 6)
            //    throw new Exception(hex +
            //        " is not a valid 6-place hexadecimal color code.");

            if (hex.Length > 6)
            {
                hex = hex.Substring(0, 6);
            }

            string r, g, b;

            r = hex.Substring(0, 2);
            g = hex.Substring(2, 2);
            b = hex.Substring(4, 2);

            return System.Drawing.Color.FromArgb(HexStringToBase10Int(r), HexStringToBase10Int(g),
                                                HexStringToBase10Int(b));
        }

        private int HexStringToBase10Int(string hex)
        {
            int base10value = 0;

            try { base10value = System.Convert.ToInt32(hex, 16); }
            catch { base10value = 0; }

            return base10value;

        }



        private DataSet GetDuplicateDataSet(string sql)
        {
            DataSet _Phdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Phdataset.Tables.Add(new DataTable("DuplicatePhlist"));
            dt = _Phdataset.Tables["DuplicatePhlist"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("_type");
            dt.Columns.Add("PhoneNo");
            dt.Columns.Add("Enabled");
            DataSet MySqldataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            DataSet NewCopyDataSet = MySqldataSet;
            if (MySqldataSet != null && MySqldataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow DTrows in MySqldataSet.Tables[0].Rows)
                {
                    if (IsDuplicate(DTrows[0].ToString(), DTrows[1].ToString(), DTrows[3].ToString(), NewCopyDataSet))
                    {
                        dr = _Phdataset.Tables["DuplicatePhlist"].NewRow();
                        dr["Id"] = DTrows[0].ToString();
                        string Str_Type = DTrows[1].ToString();
                        string _type = "Student";
                        if (Str_Type == "0")
                        {
                            _type = "Staff";
                        }
                        else if (Str_Type == "1")
                        {
                            _type = "Parent";
                        }

                        dr["_type"] = _type;
                        dr["Name"] = DTrows[2].ToString();
                        dr["PhoneNo"] = DTrows[3].ToString();
                        string Enabled = "Yes";
                        if (DTrows[4].ToString() == "0")
                        {
                            Enabled = "No";
                        }
                        dr["Enabled"] = Enabled;
                        _Phdataset.Tables["DuplicatePhlist"].Rows.Add(dr);

                    }
                }
            }
            


            return _Phdataset;
        }

        private bool IsDuplicate(string Id, string _type, string phoneno, DataSet NewCopyDataSet)
        {

            bool valid = false;
            foreach (DataRow DTrows in NewCopyDataSet.Tables[0].Rows)
            {
                if (DTrows[3].ToString() == phoneno && !(DTrows[0].ToString() == Id && DTrows[1].ToString()==_type))
                {
                    valid = true;
                }
            }
            return valid;
        }

        protected void Grd_DuplicateEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Id = Grd_DuplicateEntries.SelectedRow.Cells[0].Text;
            string _type = Grd_DuplicateEntries.SelectedRow.Cells[3].Text;

            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScript", "window.open('SMSConfig.aspx?Id=" + Id + "&&Type=" + _type + "','Info','status=1, width=1050, height=550,resizable = 1,scrollbars=1')", true);
        }

        # endregion


        # region Search PhoneList

        protected void Btn_SearchNo_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (IsSearchPossible(out msg))
            {
                Load_SerachData();
            }
            else
            {
                lbl_error.Text = msg;
            }
        }

        private bool IsSearchPossible(out string msg)
        {
            bool _valid = true;
            msg = "";

            if (Txt_SearchNo.Text.Trim() == "")
            {
                msg = "Enter phone number";
                _valid = false;
            }

            return _valid;
        }

        private void Load_SerachData()
        {
            lbl_error.Text = "";
            string sql = "SELECT tbluser.Id,0 as _type,tbluser.UserName as Name,tblsmsstafflist.PhoneNo,tblsmsstafflist.Enabled FROM tbluser INNER JOIN tblsmsstafflist ON tbluser.Id=tblsmsstafflist.Id WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 AND tblsmsstafflist.PhoneNo=" + Txt_SearchNo.Text;

            sql = sql + " UNION " + "SELECT tblstudentclassmap.StudentId as Id,1 as _type,tblstudent.GardianName as Name,tblsmsparentlist.PhoneNo,tblsmsparentlist.Enabled FROM tblstudentclassmap INNER JOIN tblsmsparentlist ON tblstudentclassmap.StudentId=tblsmsparentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblsmsparentlist.PhoneNo=" + Txt_SearchNo.Text;

            sql = sql + " UNION " + "SELECT tblstudentclassmap.StudentId as Id,2 as _type,tblstudent.StudentName as Name,tblsmsstudentlist.PhoneNo,tblsmsstudentlist.Enabled FROM tblstudentclassmap INNER JOIN tblsmsstudentlist ON tblstudentclassmap.StudentId=tblsmsstudentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.`Status`=1 AND tblsmsstudentlist.PhoneNo=" + Txt_SearchNo.Text;
            MydataSet = GetPhoneListDataset(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_PhoneNo.Columns[0].Visible = true;
                Grd_PhoneNo.DataSource = MydataSet;
                Grd_PhoneNo.DataBind();
                Grd_PhoneNo.Columns[0].Visible = false;
            }
            else
            {
                lbl_error.Text = "No match found";
                Grd_PhoneNo.DataSource = null;
                Grd_PhoneNo.DataBind();
            }
        }

        private DataSet GetPhoneListDataset(string sql)
        {
            DataSet _Phdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Phdataset.Tables.Add(new DataTable("Phlist"));
            dt = _Phdataset.Tables["Phlist"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("_type");
            dt.Columns.Add("PhoneNo");
            dt.Columns.Add("Enabled");
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = _Phdataset.Tables["Phlist"].NewRow();
                    dr["Id"] = MyReader.GetValue(0).ToString();
                    string Str_Type=MyReader.GetValue(1).ToString();
                    string _type = "Student";
                    if (Str_Type == "0")
                    {
                        _type = "Staff";
                    }
                    else if (Str_Type == "1")
                    {
                        _type = "Parent";
                    }

                    dr["_type"] = _type;
                    dr["Name"] = MyReader.GetValue(2).ToString();
                    dr["PhoneNo"] = MyReader.GetValue(3).ToString();
                    string Enabled = "Yes";
                    if (MyReader.GetValue(4).ToString() == "0")
                    {
                        Enabled = "No";
                    }
                    dr["Enabled"] = Enabled;
                    _Phdataset.Tables["Phlist"].Rows.Add(dr);
                }
            }

            return _Phdataset;
        }

        protected void Grd_PhoneNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Id = Grd_PhoneNo.SelectedRow.Cells[0].Text;
            string _type = Grd_PhoneNo.SelectedRow.Cells[3].Text;//
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScript", "window.open('SMSConfig.aspx?Id=" + Id + "&&Type=" + _type + "','Info','status=1, width=1050, height=550,resizable = 1,scrollbars=1')", true);
        }


        protected void Grd_DuplicateEntries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_DuplicateEntries.PageIndex = e.NewPageIndex;
            Load_DuplicateEntry();
        }

        #endregion

       

      
    }
}
