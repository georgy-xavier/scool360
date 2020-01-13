<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentConsolidate.aspx.cs" Inherits="WinEr.StudentConsolidate" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
 
 
<div id="right">


<div class="label">Student Info</div>
<div id="SubStudentMenu" runat="server">
		
 </div>
 
</div>

<div id="left">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
         <ProgressTemplate>
                
                
                        <div id="progressBackgroundFilter">

 
 
                        </div>

                        <div id="processMessage">

                        <table style="height:100%;width:100%" >

                        <tr>

                        <td align="center">

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
    </asp:UpdateProgress>

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
<ContentTemplate>

<div id="StudentTopStrip" runat="server">    
<div id="winschoolStudentStrip">
<table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Arun Sunny</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Class</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           BDS</td>
                       
                       <td class="attributeValue">Admission No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td></td>
                       </tr>
                       <tr>
                       <td class="attributeValue">Class No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       </tr>
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>	
</div>
</div>
  
<div class="container skin1">
  <table   cellpadding="0" cellspacing="0" class="containerTable">
  
        <tr >
                <td class="no"> </td>
                <td class="n">Student performance</td>
                <td class="ne"> </td>
        </tr>
        
        <tr>
        <td class="o"></td>
        <td class="c">
        
        <div id="HeaderstrDiv" runat="server">
    
      <table width="100%">
      
      <tr>
      <td style="width:50%"></td>
      <td style="width:50%"></td>
      </tr>
      
      <tr>
      <td align="right" style="width:50%">Select Term&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;</td>
      <td align="left" style="width:50%">
          &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="Drp_term" runat="server" Width="140px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_Term_SelectedIndexChanged">
          </asp:DropDownList>
      </td>
      </tr>
      
	  <tr>
				<td align="left" style="width:50%" >
				  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				  <asp:ImageButton ID="Img_Search" runat="server" ImageUrl="~/Pics/Class.png" ImageAlign="AbsMiddle"  Width="30px" Height="30px"/>
                  <asp:LinkButton ID="Lnk_PreviousPerformance" runat="server"
                       Text="Previous Performance" onclick="Lnk_PreviousPerformance_Click" ></asp:LinkButton>
              
				</td>
				
				<td align="right" style="width:50%">
				
				</td>
      </tr>
    
      <tr>
      <td style="width:50%"></td>
      <td style="width:50%"></td>
      </tr>
    
      </table>
      
    </div>
    
        <div id="PriviousbatchDiv" runat="server">
    <table width="100%">
    
     <tr>
     <td style="width:50%"></td>
     <td style="width:50%"></td>
     </tr>
     
     <tr>
     <td style="width:50%" align="right">Select Batch&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;</td>
     <td style="width:50%" align="left">&nbsp;&nbsp;&nbsp;
         <asp:DropDownList ID="Drp_batch" runat="server" AutoPostBack="True" class="form-control" Width="140px">
         </asp:DropDownList>
     </td>
     </tr>
     
     <tr>
     <td style="width:50%"></td>
     <td style="width:50%"></td>
     </tr>
     
     <tr>
     <td style="width:50%" align="right">Select Term&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;</td>
     <td style="width:50%" align="left">&nbsp;&nbsp;&nbsp;
          <asp:DropDownList ID="Drp_batchTerm" runat="server" AutoPostBack="true" Width="140px" class="form-control" OnSelectedIndexChanged="Drp_batch_OnSelectedIndexChanged">
         </asp:DropDownList>
     </td>
     </tr>
     
     <tr>
     <td style="width:50%" align="left">
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:LinkButton ID="Lnk_back" runat="server" onclick="Lnk_back_Click">Back</asp:LinkButton>
     </td>
     <td style="width:50%" align="right">
     </td>
     </tr>
     
      <tr>
      <td style="width:50%"></td>
      <td style="width:50%"></td>
      </tr>
     
    </table>
    </div>
    
        <div id="ResultGridDiv" runat="server">
    
    <table width="100%">
    
    <tr>
    <td align="center" style="width:100%">
    <hr style="color:inherit" />
    </td>
    </tr>
    
    <tr>
    <td align="right" style="width:100%">
       <asp:ImageButton ID="img_export_Excel" runat="server" Height="47px" Width="42px" Visible="false"
                                                                ImageUrl="~/Pics/Excel.png" OnClick="Img_Export_Click" 
                                                                ToolTip="Export to Excel" />&nbsp;
    <asp:ImageButton ID="Img_export_Pdf" ToolTip="Export to PDF" runat="server" ImageUrl="~/Pics/ViewPdf.png"
                    Height="47px"  Width="42px" onclick="Img_export_Pdf_Click"/>&nbsp;
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
        <asp:Label ID="lbl_Marksub" runat="server" Text="MARK SUBJECTS" Font-Bold="true" class="control-label" Font-Underline="true" ForeColor="OrangeRed" Font-Size="Small"></asp:Label>
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
    <asp:GridView ID="grdResult_marksubject" runat="server"  
                                                      GridLines="None" Width="100%"
                                                      BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" AutoGenerateColumns="true">
                  <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                  </asp:GridView>
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
        <asp:Label ID="Lbl_graedesubject" runat="server" Text="GRADE SUBJECTS" Font-Bold="true" class="control-label" Font-Underline="true" ForeColor="OrangeRed" Font-Size="Small" Visible="false"></asp:Label>
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
    <asp:GridView ID="grdResult_gradesubject" runat="server"  
                                                      GridLines="None" Width="100%"
                                                      BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                  <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                  </asp:GridView>
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
    </td>
    </tr>
    
    
    
    </table>
      
    </div>
    
        <div id="ResultDiv" runat="server">
        <table width="100%">
    
        <tr>
        <td align="center" style="width:100%">
        <hr style="color:inherit" />
        </td>
        </tr>
    
        <tr>
        <td align="center" style="width:100%">
        <br />
        <asp:Label ID="Lbl_resultmag" runat="server" Text="RESULT" Font-Bold="true" class="control-label" Font-Size="Medium"></asp:Label>
        <br />
        </td>
    </tr>
    
        <tr>
        <td align="center" style="width:100%"></td>
        </tr>
    
        </table>
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
 
  
 <WC:MSGBOX ID="WC_MessageBox" runat="server" />
 
 </ContentTemplate>
 <Triggers>
 <asp:PostBackTrigger ControlID="img_export_Excel"/>
 </Triggers> 
 </asp:UpdatePanel>

</div> 
</div>
</asp:Content>
