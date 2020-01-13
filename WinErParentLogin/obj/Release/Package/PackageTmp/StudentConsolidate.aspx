<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="StudentConsolidate.aspx.cs" Inherits="WinErParentLogin.StudentConsolidate" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate> <div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" > <tr> <td align="center"><b>Please Wait...</b><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table> </div></ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate> 
     
    <div id="HeaderstrDiv" runat="server">
    
      <table width="100%">
      
      <tr>
      <td style="width:50%"></td>
      <td style="width:50%"></td>
      </tr>
      
      <tr>
      <td align="right" style="width:50%">Select Term&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;</td>
      <td align="left" style="width:50%">
          &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="Drp_term" runat="server" Width="140px" AutoPostBack="true" OnSelectedIndexChanged="Drp_Term_SelectedIndexChanged">
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
         <asp:DropDownList ID="Drp_batch" runat="server" AutoPostBack="True" Width="140px">
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
          <asp:DropDownList ID="Drp_batchTerm" runat="server" AutoPostBack="true" Width="140px" OnSelectedIndexChanged="Drp_batch_OnSelectedIndexChanged">
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
    <br /><hr style="color:inherit" /><br />
    </td>
    </tr>
    
    <tr>
    <td align="right" style="width:100%">
    <asp:ImageButton ID="Img_export_Pdf" ToolTip="Export to PDF" runat="server" ImageUrl="~/Pics/ViewPdf.png"
                    Height="47px"  Width="42px" onclick="Img_export_Pdf_Click"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </td>
    </tr>
    
    <tr>
    <td align="center" style="width:100%">
        <asp:Label ID="lbl_Marksub" runat="server" Text="MARK SUBJECTS" Font-Bold="true" Font-Underline="true" ForeColor="OrangeRed" Font-Size="Small"></asp:Label>
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
        <asp:Label ID="Lbl_graedesubject" runat="server" Text="GRADE SUBJECTS" Font-Bold="true" Font-Underline="true" ForeColor="OrangeRed" Font-Size="Small" Visible="false"></asp:Label>
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
    <asp:Label ID="Lbl_resultmag" runat="server" Text="RESULT" Font-Bold="true" Font-Size="Medium"></asp:Label>
    <br />
    </td>
    </tr>
    
    <tr>
     <td align="center" style="width:100%"></td>
    </tr>
    
    </table>
    </div>
    
    <WC:MSGBOX ID="WC_MessageBox" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
    
</div>
</asp:Content>
