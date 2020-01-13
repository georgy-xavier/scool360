<%@ Page Language="C#" MasterPageFile="~/WinerPortalMaster.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Winer.Portal._default" %>




    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="css/main.css" rel="stylesheet" />
 <script language="javascript" type="text/javascript">
     function Datevalue(organizationid) {
         getTotalFee(organizationid);
     }


</script>

</asp:Content>



    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="col-lg-4 col-xs-6">
          <div class="small-box ">
              <a class="small-box-footer bg-orange-dark" href="StudentReport.aspx">
                  <div class="icon bg-orange-dark" style="padding: 9.5px 18px 8px 18px;">
                      <i class="fa fa-graduation-cap fa"></i>
                  </div>
                  <div class="inner ">
                      <h3 class="text-white" id="totalStudents">0</h3>
                      <img src="img/Loading_icondefault.gif" alt="" id="loader" style="visibility: visible;"/>
                      <p class="text-white">
                          Student                  </p>
                  </div> 
              </a>
          </div>
        </div>
         <div class="col-lg-4 col-xs-6">
              <div class="small-box ">
                  <a class="small-box-footer bg-pink-light" href="StaffReport.aspx">
                      <div class="icon bg-pink-light" style="padding: 9.5px 18px 8px 18px;">
                          <i class="fa fa-users fa"></i>
                      </div>
                      <div class="inner ">
                          <h3 class="text-white" id="totalStaffs">0</h3>
                          <img src="img/Loading_icondefault.gif" alt="" id="loader1" style="visibility: visible;"/>
                          <p class="text-white">
                              Staffs                  </p>
              
                                  </div>
                  </a>
              </div>
        </div>
        <div class="col-lg-4 col-xs-12">
          <div class="small-box ">
              <a class="small-box-footer bg-purple-light" href="FeeReport.aspx">
                  <div class="icon bg-purple-light" style="padding: 9.5px 18px 8px 18px;">
                      <i class="fa fa-money fa"></i>
                  </div>
                  <div class="inner ">
                       <h3 class="text-white" id="totalFees">0</h3>
                      <img src="img/Loading_icondefault.gif" alt="" id="loader2" style="visibility: visible;"/>
                      <p class="text-white">
                          Fees Collected                  </p>
                  </div>
              </a>
          </div>
        </div>
       



        <%--<div class="box">
           <div class="main-text">
                 <span class="label">Total Students: </span>
                 <span class="value" id="totalStudents">0</span>
           </div>
           <div class="sub-text">
                 <span class="label">Male Students: </span>
                 <span class="value" id="maleStudents">0</span>
            </div>
            <div class="sub-text">
                 <span class="label">Female Students: </span>
                 <span class="value" id="femaleStudents">0</span>
            </div>
        </div>--%>

         <%--<div class="box">
           <div class="main-text">
                 <span class="label">Total Fees: </span>
                 <span class="value" id="totalFees">0</span>
           </div>
           <div class="sub-text">
                <span class="label">Due Fees: </span>
                <span class="value" id="dueFees">0</span>
           </div>
        </div>--%>
       <%-- <div class="box">
           <div class="main-text">
                 <span class="label">Total Staffs: </span>
                 <span class="value" id="totalStaffs">0</span>
           </div>
           <div class="sub-text">
                 <span class="label">Male Students: </span>
                 <span class="value" id="maleStaffs">0</span>
            </div>
            <div class="sub-text">
                 <span class="label">Female Students: </span>
                 <span class="value" id="femaleStaffs">0</span>
            </div>
        </div>--%>
        
   <%-- </div>--%>
  <asp:HiddenField ID="HiddenField1" runat="server" />
   
    <script type="text/javascript" src="js/api-module.js"></script>
    <script type="text/javascript" src="js/dashboard.js"></script>
       <%-- <script type="text/javascript" src="js/config.js"></script>--%>
        </asp:Content>

