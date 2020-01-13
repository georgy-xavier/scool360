<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master"  AutoEventWireup="true" CodeBehind="ExamReportPreviousBatch.aspx.cs" Inherits="WinEr.ExamReportPreviousBatch" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        
        #prfomancetable
        {
          
         
        }
        

         .TableHeaderStyle
        {
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           background-color:#666666;
           font-weight:bold;
           color:White;
           text-align:center;
           padding:10px 10px 10px 10px;

         
        }
        .SubHeaderStyle
        {
           background-color:Gray;
           color:White;
           font-weight:bolder;
           text-align:center;
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           padding-right:10px;
           text-align:left;
        }
        .CellStyle
        {
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           color:#333333;
        }
     
     
    
      
        .style1
        {
            height: 34px;
        }
     
     
    
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
        $(function () {
            $("._stdntAdmNo,._stdntId,._stdntAge,._stdntClsNo,._stdntPh,._stdntSex,._stdntDOB,._stdntDOJ,._stdntStd,._stdntCls,._stdntFrstBtch,._stdntFrstStd").html('<div class="animated_bg_short_loder rectangle_sm"></div>');
            $("#submnLdr,._clsTimeLine,._stdntSiblDt,._stdntNm,._extraField,._incdentDt").html(circleLoader);
            loadStudentTopData();
            loadStudentSubMenu();

        });
     </script>
 <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    <div class="container-fluid cudtomContFluid">   
        <div class="col-lg-10 col-md-10 col-xs-12">
            <div class="row">
                <div class="card0">
                    <div class="cardHd">Edit Student Details</div>
                    <div class="row">
                         
                        <div class="col-md-4 ">
                                <div style="padding: 20px;">Contact No : <span class="_stdntPh stdDetilsTxt"></span></div>
                                <div style="padding: 20px;">Admission No : <span class="_stdntAdmNo stdDetilsTxt"></span></div>
                                <div style="padding: 20px;">Student ID : <span class="_stdntId stdDetilsTxt"></span></div>
                        </div>
                        <div class="col-md-4">
                                <div class="_stdnt_Img"></div>
                            <div class="row"><div class="_stdntNm stdntDtsNm"><h4>Student Name</h4></div></div>
                        </div>
                        <div class="col-md-4">
                                <div style="padding: 20px;">Age : <span class="_stdntAge stdDetilsTxt"></span></div>
                                <div style="padding: 20px;">Class / Roll No : <span class="_stdntClsNo stdDetilsTxt"></span></div>
                                <div style="padding: 20px;">Date Of Admission : <span class="_stdntDOJ stdDetilsTxt"></span></div>
                        </div>
                           

                    </div>
                    <div class="row"></div>
                </div>
            </div>
            <br>
             <div class="row">
                  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
      
    <div class="card0" >
        <table class="containerTable">
            <tr >
                <td class="no"> <asp:Image ID="Img_Search" runat="server" ImageUrl="~/Pics/book_search.png" Width="30px" Height="30px"/></td>
                <td class="n"> Student performance</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                      <br />
                      
                      <table class="tablelist" width="100%">
                            <tr>
                            <td >Select Class:
                                 <asp:DropDownList ID="Drp_Class" runat="server" Width="200px" AutoPostBack="true" 
                                    onselectedindexchanged="Drp_Class_SelectedIndexChanged">
                                 </asp:DropDownList>
                               
                               <%--  <asp:Button ID="Btn_Search" runat="server" Text="Search" CssClass="graysearch" 
                                     onclick="Btn_Search_Click" />--%>
                                
                           </td>
                            </tr>
                        <tr><td colspan="2">&nbsp;</td></tr>
                        <tr><td colspan="2" align="center"><asp:Label ID="Lbl_indexammsg" runat="server" ForeColor="Red"></asp:Label></td></tr>
                        
                        <tr id="MarkListArea" runat="server">
                        <td align="left" class="style1">
                        <div class="newsubheading">Mark List</div>
                        </td>
                <td align="right" class="style1"> <asp:ImageButton ID="Img_Export" runat="server" Height="30px" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Pics/Excel.png" Width="30px" onclick="Img_Export_Click" ToolTip="Export to Excel" /></td>
               </tr>
                <tr id="MarkListArea1" runat="server">
                <td colspan="2"> <div class="linestyle">    </div><br />
                    <div  style="width:100%; overflow:auto;"  class="scrollStyle">
                        <div style="width:700px;" id="ExamReport" runat="server">
                      </div> 
                    </div>
                   </td>
                </tr>		      
                
               
                        </table>
                    
                        
                                 
                    
            
                    <asp:Panel ID="Pnl_ExamGraph" runat="server" Visible="false">
                        <br />
                        <div class="newsubheading">
                        Performance Chart.
                            </div>
                                     <div class="linestyle">    </div>
                    
                     <br />
                          Select Condition:  <asp:DropDownList ID="Drp_SelectList" runat="server" 
                                AutoPostBack="True"  Width="160px" 
                            onselectedindexchanged="Drp_SelectList_SelectedIndexChanged">
                            </asp:DropDownList> <br /><br />
                            
                        <web:chartcontrol BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart"
                       runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0" 
                            Width="700px" YCustomStart="0" YValuesInterval="0"><Background Color="LightSteelBlue" /><ChartTitle StringFormat="Center,Near,Character,LineLimit"  /><XAxisFont StringFormat="Center,Near,Character,LineLimit" /><YAxisFont StringFormat="Far,Near,Character,LineLimit" /><XTitle StringFormat="Center,Near,Character,LineLimit" /><YTitle StringFormat="Center,Near,Character,LineLimit" /></web:chartcontrol>
                           
                         </asp:Panel>   
                         <br />
                         
                         <div id="ExamNames" runat="server" style="width:500px;">                         
                        
                         </div>
                    
                     
                      
                </td>
                <td class="e"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
    
    </ContentTemplate>
    <Triggers><asp:PostBackTrigger ControlID="Img_Export" />
     <asp:PostBackTrigger ControlID="Drp_SelectList" />
      <asp:PostBackTrigger ControlID="Drp_Class" />
    </Triggers>
    
    </asp:UpdatePanel>   
             </div>
          </div>
        <div class="col-lg-2 col-md-2">
            <div id="SubStudentMenu"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
        </div>
    </div>
    <WC:MSGBOX id="WC_MessageBox" runat="server" />  
</asp:Content>

