using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CSASPNETBackgroundWorker;
using System.Threading;
using System.Data.Odbc;
using WinBase;

namespace SalesTracker
{
    public partial class test1 : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private FeeManage MyFeeMang;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }

            else
            {
                if (!IsPostBack)
                {

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new BackgroundWorker.DoWorkEventHandler(worker_DoWork);
                    worker.RunWorker("Doing Work");

                    // It needs Session Mode is "InProc"
                    // to keep the Background Worker working.
                    Session["worker"] = worker;

                    // Enable the timer to update the status of the operation.
                    Timer1.Enabled = true;
                    // private WinBase.AsyncModule.MyAsyncResult _new;
                    // DoAsyncWork(_new);

                }
            }

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Show the progress of current operation.
           
            BackgroundWorker worker = (BackgroundWorker)Session["worker"];
            
            if (worker != null)
            {
                // Display the progress of the operation.
                if(lbProgress.Text == "please wait.")
                    lbProgress.Text = "please wait...";
                else if (lbProgress.Text == "please wait...")
                    lbProgress.Text = "please wait.....";
                else
                {
                    lbProgress.Text = "please wait.";
                }

                //btnStart.Enabled = !worker.IsRunning;
                Timer1.Enabled = worker.IsRunning;

                // Display the result when the operation completed.
                if (worker.Progress >= 100)
                {
                    lbProgress.Text = (string)worker.Result;
                    Response.Redirect("AbsDlyFeeReport.aspx");
                }
            }
        }

        /// <summary>
        /// Create a Background Worker to run the operation when button clicked.
        /// </summary>
        //protected void btnStart_Click(object sender, EventArgs e)
        //{
            
        //}

        /// <summary>
        /// This method is the operation that needs long time to complete.
        /// </summary>
        void worker_DoWork(ref int progress,
            ref object result, params object[] arguments)
        {
            // Get the value which passed to this operation.
            string input = string.Empty;
            if (arguments.Length > 0)
            {
                input = arguments[0].ToString();
            }

            // Need 10 seconds to complete this operation.
            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(100);

            //    progress += 1;
            //}

            try
            {
                if (Session["Session_ToDate"] != null && Session["Session_FromDate"] != null)
                {
                    string _ToDate = Session["Session_ToDate"].ToString();
                    string _FromDate = Session["Session_FromDate"].ToString();
                    if (_ToDate != "" && _ToDate != "")
                    {
                        DataSet MyClass = MyUser.MyAssociatedClass();
                        MydataSet = MyFeeMang.GetDailyAbsFeeReport(_FromDate, _ToDate, MyClass);
                        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                        {
                            Session["Session_DataSet"] = MydataSet;
                            result = "Data loaded successfully";
                        }

                    }
                }
                progress = 100;
            }
            catch(Exception ex)
            {
                result = "No Data Present For Exporting To Excel-"+ ex.Message;
                progress = 100;
            }
            // The operation is completed.
            
        }

     
    }
}
