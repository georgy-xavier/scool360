using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class BusFeeManagr : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();
            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(849))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadDestinationToGrid();

                }

            }
        }
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gv in Grd_BusFeeMangr.Rows)
            {
                TextBox _Cost = (TextBox)gv.FindControl("Txt_Cost");
                _Cost.Text = "";
            }
        }


        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            int count = 0;
            int save = 0;
            int select = 0;
            foreach (GridViewRow gv in Grd_BusFeeMangr.Rows)
            {
                TextBox _Cost = (TextBox)gv.FindControl("Txt_Cost");
                CheckBox cb = (CheckBox)gv.FindControl("ChkFee");
                if (cb.Checked)
                {
                    if (_Cost.Text == "")
                    {
                        select = 1;
                     
                    }

                }
            }
            if (select != 1)
            {
                foreach (GridViewRow gv in Grd_BusFeeMangr.Rows)
                {
                    TextBox _Cost = (TextBox)gv.FindControl("Txt_Cost");
                    CheckBox cb = (CheckBox)gv.FindControl("ChkFee");
                    if (cb.Checked)
                    {
                        count = 1;
                        string Destination = gv.Cells[2].Text;
                        int Id = int.Parse(gv.Cells[1].Text.ToString());
                        double cost = double.Parse(_Cost.Text);
                        save = 1;
                        MyTransMang.SaveData(Id, cost);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add Bus Fee", "An amount of Rs." + cost + "  is added as Bus Fee to the route " + Destination + "", 1);
                    }
                }
            }
            if (select == 1)
            {
                WC_MessageBox.ShowMssage("Please enter cost for selected destination !");
            }
            else
            {
                if (count == 0)
                {
                    WC_MessageBox.ShowMssage("Select any destination!");
                }

                if (save == 1)
                {
                    LoadDestinationToGrid();
                    WC_MessageBox.ShowMssage("Data saved successfully!");
                }
            }
        }
        protected void Grd_BusFeeMangr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_BusFeeMangr.PageIndex = e.NewPageIndex;
            LoadDestinationToGrid();
        }

        #region Methods

        private void LoadDestinationToGrid()
        {
            Lbl_Err.Text = "";            
            DataSet Destination_Ds = new DataSet();
            Destination_Ds = MyTransMang.GetDestinationDetails();
            if (Destination_Ds != null & Destination_Ds.Tables[0].Rows.Count > 0)
            {

                Pnl_ListOfDestination.Visible = true;
                Grd_BusFeeMangr.Columns[1].Visible = true;
                Grd_BusFeeMangr.DataSource = Destination_Ds;
                Grd_BusFeeMangr.DataBind();
                Grd_BusFeeMangr.Columns[1].Visible = false;                
            }
            else
            {
                Lbl_Err.Text = "No destination found";
                Pnl_ListOfDestination.Visible = false;
                Grd_BusFeeMangr.DataSource = null;
                Grd_BusFeeMangr.DataBind();                
            }
            FillTextBox();
        }

        private void FillTextBox()
        {
            foreach (GridViewRow gv in Grd_BusFeeMangr.Rows)
            {
                TextBox _Cost = (TextBox)gv.FindControl("Txt_Cost");
                int Id = int.Parse(gv.Cells[1].Text.ToString());
                double Cost = MyTransMang.GetCost(Id);
                _Cost.Text = Cost.ToString();
            }
        }

        

        #endregion     

        
    }
}
