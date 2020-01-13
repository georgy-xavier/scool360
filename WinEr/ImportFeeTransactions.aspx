<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ImportFeeTransactions.aspx.cs" Inherits="WinEr.ImportFeeTransactions" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
            
                 <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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

 <WC:MSGBOX id="WC_MessageBox" runat="server" />    

<div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Import Fee Transactions</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
               
                   <asp:Panel ID="Pnl_InitialDetails" runat="server"  HorizontalAlign="Left">
                    <center>
                   
                     <div style="min-height:200px">
                   
                     <table width="100%" cellspacing="5">
                      <tr>
                      <td style="width:25%" align="right">
                         Class : 
                      </td>
                       <td style="width:25%" align="left">
                         <asp:DropDownList ID="Drp_ClassName" runat="server" class="form-control" Width="181px" AutoPostBack="true"
                            onselectedindexchanged="Drp_ClassName_SelectedIndexChanged" >
                         </asp:DropDownList>
                      </td>
                      <td style="width:30%" align="right">
                              Download Template :&nbsp;  
                      </td>
                      <td style="width:20%" align="left">
                           <asp:ImageButton ID="Img_ExportTemplate" runat="server" 
                                   ImageUrl="~/Pics/Excel.png" Width="40px" Height="40px" 
                                   onclick="Img_ExportTemplate_Click" />
                      </td>
                      </tr>
                      <tr>
                       <td style="width:25%" align="right">
                           Select file to import :&nbsp; 
                       </td>
                       <td style="width:25%" align="left">
                          
                            <asp:FileUpload ID="FileUpload_Excel" runat="server" Height="20px" />
                           
                       </td>
                      </tr>
                      <tr>
                       <td colspan="2" align="center">
                       
                        <br />
                       
                       </td>
                      </tr>
                      <tr>
                       <td>
                       
                       
                       </td>
                       <td  align="left">
                       
                          
                           <asp:Button ID="Btn_Upload" runat="server" Text="Upload" Class="btn btn-info" 
                               onclick="Btn_Upload_Click" OnClientClick="return confirm('Before uploading fee transactions, please take one database backup. Are you sure, you want to upload the fee transaction data?')" />
                           &nbsp; 
                           &nbsp; 
                           <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" 
                               onclick="Btn_Cancel_Click" />
                           
                       </td>
                      
                      </tr>
                     </table>
                         
                         
                     </div>               
                    </center>
                   </asp:Panel> 
              

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
      <Triggers>
             <asp:PostBackTrigger ControlID="Img_ExportTemplate" />
             <asp:PostBackTrigger ControlID="Btn_Upload" />
   </Triggers>
  </asp:UpdatePanel>  

</asp:Content>
