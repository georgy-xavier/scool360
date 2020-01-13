<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" EnableTheming="false"
    AutoEventWireup="True" Inherits="SearchStudent" CodeBehind="SearchStudent.aspx.cs" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
         .ui-autocomplete {
    max-height: 50vh;
    overflow-y: auto;
    /* prevent horizontal scrollbar */
    overflow-x: hidden;
  }
         /*.ui-autocomplete-loading {
    background: white url("images/indicator-big.gif") right center no-repeat;
    background-size: 30px;
  }*/
    </style>

    <script type="text/javascript">
        $(function () {
           // document.getElementById("stdSrchDt").focus();
            $('#dtExprt').hide();
            searchStudent_aspx.initStudentSrch();
            searchStudent_aspx.LoadDfltstdntDt();
            $('#btnAdvSrch').click(function (e) {
                e.preventDefault();
                searchStudent_aspx.advancedSrchClk();
            }); 
            $('#btnStdSrch').on("click",function (e) {
                e.preventDefault();
                $('#stdSrchDt').val(ui.item.value)
                $(this).blur();
                searchStudent_aspx.stdDtSrch();
            });
            $('#advSrch').on("click", function (e) {
                e.preventDefault();
                $("#mainModal").modal();
                searchStudent_aspx.makeFilter();
            });
        }); 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></ajaxToolkit:ToolkitScriptManager>
    
<div class="container card0" style="padding: 25px;">   
    <div class="row">
        <div class="col-md-4"></div>
        <div class="col-md-4">
        <select class="form-control" id="srchByDrp" style="border: 1px solid #d8d6d6 !important;border-radius:0;">
            <option value="0">Search by Admission No</option>
            <option value="1" selected>Search by Student Name</option>
            <option value="2">Search by Class</option>
            <option value="3">Search by Joining Batch</option>
            <option value="4">Search by Student Id</option>
        </select>
            </div>
        <div class="col-md-4">
            
        </div>

    </div>
    <br>
    <div class="row">
        <div style="padding: 10px;display:  initial;">
        <div class="checkbox checkbox-primary checkbox-inline">
            <input id="chkLive" Value ="1"  type="checkbox" checked>
            <label for="chkLive">Live</label>
        </div>
            <div class="checkbox checkbox-primary checkbox-inline">
            <input id="chkHist" Value ="2"  type="checkbox">
            <label for="chkHist">History</label>
        </div>
            <div class="checkbox checkbox-primary checkbox-inline">
            <input id="chkProm" Value ="3" type="checkbox" >
            <label for="chkProm">Promotion List</label>
        </div>
                <div class="checkbox checkbox-primary checkbox-inline">
            <input id="chkAppr" Value ="4" type="checkbox" >
            <label for="chkAppr">Approval List</label>
        </div>
                <div class="checkbox checkbox-primary checkbox-inline">
            <input id="chkReg" Value ="5" type="checkbox" >
            <label for="chkReg">Registered List</label>
        </div>

       
            </div>
    </div>
    <br>
    <div class="row">
            <div class="input-group col-md-6 col-md-offset-3 col-lg-6 col-lg-offset-3 col-sm-12 col-xs-12">
            <input type="text"  id="stdSrchDt" class="form-control srchBar" placeholder="Search students here.." />
            <span id="btnStdSrch" class="glyphicon glyphicon-search" data-placement="top" data-toggle="tooltip" title="Click to search" style="padding:10px;cursor:pointer;"></span>
            </div>
    </div>
    <div class="row" style="padding-top: 25px;">
        <div class="col-md-6 col-md-offset-3 col-lg-6 col-sm-6 col-xs-12">
            <span data-placement="top" data-toggle="tooltip" title="Click to get all student details">
                <a href="AllStudentView.aspx" style="cursor:pointer;">All students Data</a>
            </span>
                &nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;
            <span data-placement="top" data-toggle="tooltip" title="Click to get advanced and more specific student details">
                <a href="#" id="advSrch" style="cursor:pointer;">Advanced search</a>
            </span>
               
        </div>
        <div class="col-lg-3 text-right">
            <span id="dtExprt">
                <span>Export Data : </span>
               
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
           
                    <asp:ImageButton ID="img_export_Excel"  runat="server" data-placement="top" data-toggle="tooltip" title="Export serch result to excel"  ImageUrl="~/Pics/Excel.png"
                        Width="30px" OnClick="img_export_Excel_Click">
                    </asp:ImageButton>&nbsp;&nbsp;
                    <asp:ImageButton ID="Img_ExcelwithStudentImage"  runat="server" data-placement="top" data-toggle="tooltip" title="Export result with student image"  ImageUrl="~/Pics/Excelimage.png"
                        Width="30px" OnClick="Img_ExcelwithStudentImage_Click"></asp:ImageButton>
      </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="img_export_Excel" />
                <asp:PostBackTrigger ControlID="Img_ExcelwithStudentImage" />      
            </Triggers>
        </asp:UpdatePanel>
            </span>
        </div>
    </div>
</div>
     
    <hr>
        
    <div id="studentDt"></div>
                
    <%--<div id="javascriptId" runat="server" hidden></div>--%>
   
</asp:Content>
