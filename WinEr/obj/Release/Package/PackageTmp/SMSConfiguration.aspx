<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSConfiguration.aspx.cs" Inherits="WinEr.SMSConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .new
        {
            font-weight:bold;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
           
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
         <asp:Panel ID="Panel1" runat="server" >

    
            <div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable" >
			<tr >
				<td class="no"> </td>
				<td class="n">SMS Configuration</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
			    <div style="min-height:250px;padding-top:10px;padding-left:10px">
			      <table cellspacing="15">
			       <tr>
			         <td>
			             Select Configuration
			         </td>
			         <td>
                         <asp:DropDownList ID="Drp_SMS_Options" runat="server" class="form-control" AutoPostBack="true" 
                             onselectedindexchanged="Drp_SMS_Options_SelectedIndexChanged" Width="300px">
                         
                         </asp:DropDownList>
			         </td>
			       </tr>
			      
			      </table>
					 
   	              <asp:Panel ID="Panel_SMS" runat="server" Visible="false">
   	              <table cellspacing="15">
   	                 <tr>
               	        <td valign="top">
                               <asp:CheckBox ID="Chk_EnableSMS" runat="server" Text="SMS Enable" />
               	        </td>
               	        <td valign="top" colspan="2">
               	                  <asp:TextBox ID="Txt_Message" runat="server" TextMode="MultiLine" class="form-control" Width="400px" Height="60px"></asp:TextBox>
					        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                   runat="server" Enabled="True" TargetControlID="Txt_Message" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Txt_Message"
                                    Display="Dynamic" ErrorMessage="<br>Number of Characters has gone over the limit"
                                   ValidationExpression="[\s\S]{1,160}"></asp:RegularExpressionValidator>
                                   
					        
               	        </td>
               	        <td valign="top">
                               <asp:ImageButton ID="Img_EditMessage" runat="server" width="30px" Height="30px" Visible="false"
                                   ImageUrl="~/Pics/lock.png" onclick="Img_EditMessage_Click" ToolTip="Click to Unlock"/>
               	        </td>
   	                     <td rowspan="2" valign="top">
   	                       <div style="height:150px;width:250px; overflow:auto">
   	                       <center>
                                  <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" class="control-label" runat="server" Text="Representations of keywords"></asp:Label>
   	                        <div id="Seperators" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td align="left">
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
   	                    
                        </td>
   	                 </tr>
   	                 <tr>
               	        <td >
                               <asp:CheckBox ID="Chk_ScheduleTime" runat="server" Text="Schedule Enable" AutoPostBack="true"
                                   oncheckedchanged="Chk_ScheduleTime_CheckedChanged" />
               	        </td>
               	        <td>
               	        <div class="form-inline">
                               <asp:DropDownList ID="Drp_Schedulehour" class="form-control" runat="server">
                                <asp:ListItem Text="HH" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                               </asp:DropDownList>   
                               <asp:DropDownList ID="Drp_ScheduleMinute" class="form-control" runat="server">
                                <asp:ListItem Text="MM" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                <asp:ListItem Text="60" Value="60"></asp:ListItem>
                               </asp:DropDownList>  
                               </div>
               	        </td>
               	         <td align="right">
                             <asp:Button ID="Btn_Update" runat="server" onclick="Btn_Update_Click" class="btn btn-success"
                                 Text="Update" />
                             <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" class="btn btn-danger"
                                 Text="Cancel" />
                         </td>
               	        <td >
                              
               	        </td>
   	                 </tr>
   	                 <tr>
   	                   <td>
   	                   </td>
   	                   <td  colspan="2">
                              <asp:Label ID="lbl_msg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
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
          
        </asp:Panel>



          </ContentTemplate>
            </asp:UpdatePanel>
<div class="clear"></div>
</div>
</asp:Content>
