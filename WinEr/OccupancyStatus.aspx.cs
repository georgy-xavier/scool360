using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class OccupancyStatus : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private FeeManage MyFeeMang;
        private DataSet MydataSet;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(851))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadFloorWiseDetails();
                }

            }
        }

        private void LoadFloorWiseDetails()
        {
            DataSet Route_Ds = new DataSet();
            string strFloor = "", FloorDetails = "", FloorProgreeBar = "";
            Route_Ds = MyTransMang.GetRouteId();
            if (Route_Ds != null && Route_Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Route_Ds.Tables[0].Rows)
                {
                    FloorDetails = GetFloorDetails(int.Parse(dr["Id"].ToString()),dr["RouteName"].ToString());
                    FloorProgreeBar = GetFloorProgressBar(int.Parse(dr["Id"].ToString()));
                    strFloor = strFloor + "<h2 class=\"acc_trigger\">  <table width=\"100%\" cellspacing=\"0\"> <tr> <td align=\"left\"  valign=\"middle\">     <a  style=\"color:Gray; font-size:large;\"  href=\"#\" >" + dr["RouteName"].ToString() + "</a>    </td> <td align=\"right\" valign=\"middle\"> " + FloorProgreeBar + "     </td>    </tr>   </table>     </h2><div class=\"acc_container\"><div class=\"block\">			" + FloorDetails + "		 </div></div>";
                }
            }
           FloorDiv.InnerHtml = strFloor;
        }


        private string GetFloorProgressBar(int RoutId)
        {
            OdbcDataReader capacityreader = null;
            string FloorProgressBar = "";           
            int Occupancy = 0;
            int Capacity = 0;
            int Id = 1;
            capacityreader= MyTransMang.getTripCapacity(RoutId,Id);
            if (capacityreader.HasRows)
            {
                int.TryParse(capacityreader.GetValue(0).ToString(), out Capacity);
                //Capacity =int.Parse(capacityreader.GetValue(0).ToString());
                Occupancy = MyTransMang.getFloorOccupancy(RoutId,Id);
            }

            
            string  TdValue = "";
            string ProgressStr = "<div class=\"ProgressManyStyle\" > <table width=\"100%\"  cellspacing=\"0\">  <tr>   ";
            if (Capacity > 0)
            {
                TdValue = " <td class=\"EmptyStyle\"  style=\"width:100%; font-size:small; font-weight:bold\" align=\"center\">Allotted:" + Occupancy + ", Capacity:"+Capacity+" </td>";

               
            }
            ProgressStr = ProgressStr + TdValue + " </tr>    </table></div>";

            FloorProgressBar = ProgressStr;

            return FloorProgressBar;
        }

        private string GetFloorDetails(int RoutId,string routename)
        {
            int Count = 1;
            string RoomNo = "", RoomType = "", RoomStr = "", RoomStatus = "", StatusColor = "", RoomStyle = "";
            int TripId = 0, Capacity = 0, Occupancy = 0;
            string FloorDetails = "";
           int Id=0;
            //string sql = "SELECT tblroom.Id,tblroom.RoomNo,tblroomtype.`Type`,tblroom.Capacity,tblroom.Occupied FROM tblroom INNER JOIN tblroomtype ON tblroomtype.Id=tblroom.RoomTypeId WHERE tblroom.`Status`=1 AND tblroom.FloorId=" + FloorId;
            OdbcDataReader MyFloorReader = MyTransMang.getTripCapacity(RoutId,Id);           
            if (MyFloorReader.HasRows)
            {
                while (MyFloorReader.Read())
                {
                    RoomStatus = ""; StatusColor = "";
                    RoomNo = ""; RoomType = "";
                    TripId = 0; Capacity = 0; Occupancy = 0;
                    int.TryParse(MyFloorReader.GetValue(0).ToString(), out TripId);
                    //RoomNo = MyFloorReader.GetValue(1).ToString();
                    //RoomType = MyFloorReader.GetValue(2).ToString();
                    int.TryParse(MyFloorReader.GetValue(1).ToString(), out Capacity);
                    string tripname="";
                    tripname = MyFloorReader.GetValue(2).ToString();
                    Occupancy = MyTransMang.getFloorOccupancy(TripId, Id);
                   // int.TryParse(MyFloorReader.GetValue().ToString(), out Occupancy);
                    if (Occupancy == 0)
                    {
                        RoomStyle = "RoomStyleRed";
                        RoomStatus = "EMPTY";
                        StatusColor = "Red";
                    }
                    else if (Capacity == Occupancy)
                    {
                        RoomStyle = "RoomStyleGreen";
                        RoomStatus = "FULL";
                        StatusColor = "Green";
                    }
                    else
                    {
                        RoomStyle = "RoomStyleWhite";
                        RoomStatus = "HAVE VACANCY";
                        StatusColor = "Orange";
                    }

                    //                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              "~/Pics/Bus.png" 
                    RoomStr = RoomStr + "<td style=\"width:19%\"> <div class=\"" + RoomStyle + "\">   <table  width=\"100%\" cellspacing=\"5\">   <tr><td align=\"center\" colspan=\"2\" style=\"color:Gray\" class=\"RoomNameStyle\">   " + tripname + "   </td></tr>  <tr>  <td colspan=\"2\" style=\"padding:1px;\">  <table width=\"100%\"  cellspacing=\"0\">   <tr>   <td align=\"center\" class=\"RoomStatusStyle\">  <a href=\"javascript:mywindow=window.open('StudentTripMap.aspx?TripId=" + TripId + "&routename=" + routename + "&tripname=" + tripname + "&RoutId=" + RoutId + "&Occupancy=" + Occupancy + "','Info','status=1, width=950, height=500,resizable = 1,scrollbars=1');mywindow.moveTo(100,100);\" title=\"View Students\" style=\"color:" + StatusColor + ";font-weight:bold;background-image:none\" >  " + RoomStatus + " </a> </td> <td  align=\"center\" class=\"RoomImageStyle\"> <a href=\"javascript:mywindow=window.open('StudentTripMap.aspx?TripId=" + TripId + "&routename=" + routename + "&tripname=" + tripname + "&RoutId=" + RoutId + "&Occupancy=" + Occupancy + "','Info','status=1, width=950, height=500,resizable = 1,scrollbars=1');mywindow.moveTo(100,100);\"  style=\"background-image:none\">  <img src=\"Pics/Bus.png\" width=\"30px\" alt=\"Select\" title=\"View Students\" />   </a>  </td>  </tr>   </table>  </td>   </tr> <tr>  <td class=\"Roomleft\">  Allotted: </td>  <td class=\"RoomRight\">  " + Occupancy + " </td> </tr>  <tr>  <td class=\"Roomleft\">  Vacancy: </td>  <td  class=\"RoomRight\">  " + (Capacity - Occupancy) + "</td></tr>  </table> &nbsp;  </div> </td>";

                    if (Count % 5 == 0)
                    {
                        RoomStr = RoomStr + " </tr> <tr> ";

                    }
                    Count++;
                }
                int width = 100;
                if (Count < 5)
                {
                    width = (Count - 1) * 20;
                }

                FloorDetails = "  <table width=\"" + width + "%\" cellspacing=\"10\">  <tr>    " + RoomStr + "</tr></table>";
            }
            else
            {
                FloorDetails = "<center> NO TRIP FOUND IN SELECTED ROUTE </center>";
            }


            return FloorDetails;
        }


    }
}
