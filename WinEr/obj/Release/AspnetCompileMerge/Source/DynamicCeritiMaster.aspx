<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="DynamicCeritiMaster.aspx.cs" Inherits="WinEr.DynamicCeritiMaster" %>
 <%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
 <%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .tableareaClass
  {
  	border:solid 1px gray;
  }
  .seperatorBordercss
  {
  	border:solid 1px gray;
  	}
  .new
   {
            font-weight:bold;
   }
       
     .style1
     {
         width: 100%;
     }
     .style2
     {
         height: 18px;
     }
       
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
   </ajaxToolkit:ToolkitScriptManager>
   
   
   
           <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div id="progressBackgroundFilter">
                </div>
                <div id="processMessage">
                    <table style="height: 100%; width: 100%">
                        <tr>
                            <td align="center">
                                <b>Please Wait...</b><br />
                                <br />
                                <img src="images/indicator-big.gif" alt="" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
      <div id="contents">  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
            
      <div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable" >
			<tr >
				<td class="no"> </td>
				<td class="n">Certificate Maters</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
            
         <div style="min-height:300px;">
          <br />
          <table width="100%">
           <tr>
            <td style="width:30%" align="right">
               Select Certificate Type : 
            </td>
            <td>
             <div class="form-inline">
                <asp:DropDownList ID="Drp_CertificateType" runat="server" Width="180px" class="form-control">
                </asp:DropDownList>
             
                <asp:Button ID="Btn_Load" runat="server" Text="Load" 
                    class="btn btn-info" onclick="Btn_Load_Click" />
                 
                 <asp:Button ID="Btn_Clear" runat="server" Text="Clear" class="btn btn-danger" 
                    onclick="Btn_Clear_Click" />
                  
                  <asp:Button ID="Btn_AddNew" runat="server" Text="Add New" class="btn btn-success" 
                    onclick="Btn_AddNew_Click" />
                    </div>
             
            </td>
            
           </tr>
          </table>
          
          <br />
          
       
        <asp:Panel ID="Panel_CertificateBody" runat="server"  >
          
          <table width="100%" cellspacing="5" class="tableareaClass">
           <tr>
              <td colspan="2">
              
              </td>
          </tr>
           <tr>
            <td style="width:10%;" align="right">
               Certificate Name : 
            </td>
            <td style="width:90%;" align="left">
                <asp:TextBox ID="Txt_CertificateName" runat="server" Text="" Width="180px" class="form-control" MaxLength="90"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                   runat="server" Enabled="True" TargetControlID="Txt_CertificateName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                  </ajaxToolkit:FilteredTextBoxExtender>
            </td>
           </tr>
           <tr>
            <td colspan="2" align="center">
             <br />
             <table width="100%">
              <tr>
               <td style="width:70%;" valign="top">
                  <HTMLEditor:Editor ID="Ceretificate_Body" runat="server" Height="300px" Width="100%" />
               </td>
               <td  valign="top" class="seperatorBordercss">
               <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" runat="server" class="control-label" Text="Representations of keywords"></asp:Label>
               <div style="height:350px;width:300px; overflow:auto">
   	                       <center>
                                  
   	                        <div id="Seperators" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td align="right" style="width:50%;">
   	                             Student : 
   	                           </td>
   	                           <td align="left">
   	                              ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
               
               
               </td>
              </tr>

              
             </table>
            
             
            </td>
           </tr>
             <tr>
              <td colspan="2" align="center">
                  <asp:Label ID="lbl_msg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
              </td>
          </tr>
           <tr>
            <td align="right">
            <asp:Button ID="Btn_Save" runat="server" Text="Save" class="btn btn-info" 
                    onclick="Btn_Save_Click" />
            </td>
            <td align="left">
            
                &nbsp;&nbsp;
            
              <asp:Button ID="Btn_Delete" runat="server" Text="Delete" class="btn btn-info" Visible="false"
                    onclick="Btn_Delete_Click" />
             
             
                &nbsp;&nbsp;
             
             
             <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" 
                    class="btn btn-info" onclick="Btn_Cancel_Click" />
            
            </td>
           </tr>
           <tr>
              <td colspan="2">
              
                  <table class="style1">
                      <tr>
                          <td>
                              &nbsp;</td>
                          <td rowspan="3">
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td class="style2">
                          </td>
                          <td class="style2">
                          </td>
                      </tr>
                  </table>
              
              </td>
          </tr>
          </table>
         
         
         </asp:Panel>
         
         
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
	   <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
            <asp:HiddenField ID="Hd_New" runat="server" />
         </ContentTemplate>
         </asp:UpdatePanel>   
   </div>
</asp:Content>
