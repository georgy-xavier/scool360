<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SendGroupSMS.aspx.cs" Inherits="WinEr.SendGroupSMS" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .Watermark
        {
            color:#999999;
            font-size:medium;
            vertical-align:bottom;
            text-align:center;
            font-family:Times New Roman;
        }
       
    </style>
<script type="text/javascript" >

    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_UserList.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
<div id="contents">
<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			<tr >
				<td class="no"></td>
				<td class="n">Group SMS</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
	<td class="c" >
	
	<asp:Panel ID="InitialArea" runat="server">
	<table width="100%" class="tablelist">
	<tr>
	<td class="leftside">Group</td>
	<td class="rightside">
	<asp:DropDownList ID="Drp_Group" runat="server" class="form-control" Width="173px">
	</asp:DropDownList></td>
	</tr>
	<tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
	<tr>
	<td class="leftside">Type</td>
	<td class="rightside">
	<asp:DropDownList ID="Drp_Type" runat="server" class="form-control" Width="173px">
	<asp:ListItem Value="-1" Text="All"></asp:ListItem>
	<asp:ListItem Value="0" Text="Student"></asp:ListItem>
	<asp:ListItem Value="1" Text="Staff"></asp:ListItem>
	</asp:DropDownList></td>
	</tr>
	<tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
	<tr>
	<td class="leftside"></td>
	<td class="rightside"><asp:Button  ID="Btn_Show" runat="server" Width="90px" Text="Show" class="btn btn-info" OnClick="Btn_Show_Click"/></td>
	</tr>
<%--	<tr>
	<td class="leftside"></td>
	<td class="rightside"><asp:Label ID="Lbl_Msg" runat="server" Font-Bold="false" ForeColor="Red">
	</asp:Label></td>
	</tr>--%>
	 <tr>
	 <td colspan="2" align="center">
     <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red" class="control-label" Font-Bold="true"></asp:Label>
      &nbsp;  &nbsp;
      <asp:LinkButton ID="Lnk_Retry" ToolTip="Retry SMS" ForeColor="Blue" Font-Size="14px" 
               runat="server" onclick="Lnk_Retry_Click">Retry</asp:LinkButton>
		</td>
     </tr>
	</table>
	</asp:Panel>
	<asp:Panel ID="Pnl_Display" runat="server">
	<table width="100%" class="tablelist">
	<tr>
	<td class="leftside">
	Select Template
	</td>
	<td class="rightside" valign="top">
	<asp:DropDownList ID="Drp_Template" runat="server" Width="173px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_Template_SelectedIndexChanged"></asp:DropDownList>	
	<asp:Button ID="Btn_Birthday_CheckConnection" runat="server" Text="Check Connection" Class="btn btn-info" OnClick="Btn_Birthday_CheckConnection_Click" />	 
    <asp:Button ID="Btn_SendSMS" Class="btn btn-info" runat="server" Text="Send" Width="90px" OnClick="Btn_SendSMS_Click" />
           
	</td>
	</tr>
	<tr>
	<td colspan="2">
	<table width="100%">
	<tr>	  
        <td align="center" style="width:40%;">       
            <asp:TextBox ID="Txt_Message" runat="server" Height="100px" class="form-control"
                TextMode="MultiLine" Width="300px"></asp:TextBox>
            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                InvalidChars="'\" TargetControlID="Txt_Message">
            </ajaxToolkit:FilteredTextBoxExtender>
           <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ControlToValidate="Txt_Message" Display="Dynamic" 
                ErrorMessage="&lt;br&gt;Please limit to 160 characters" 
                ValidationExpression="[\s\S]{1,159}"></asp:RegularExpressionValidator>--%>
            <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Message_TextBoxWatermarkExtender" 
                runat="server" Enabled="True" TargetControlID="Txt_Message" 
                WatermarkCssClass="Watermark" WatermarkText="Enter The Message">
            </ajaxToolkit:TextBoxWatermarkExtender>
            
           
        </td>
        <td align="center" style="width:20%;">
         <asp:Button ID="btnConvert" runat="server" Class="btn btn-info" 
                                onclick="btnConvert_Click"  Text="Native Language " 
                                   width="138px" />
        </td>
        <td align="center" style="width:40%;">
        <asp:TextBox ID="txtNativelanguage" runat="server" class="form-control" TextMode="MultiLine" Width="300px" Height="100px"></asp:TextBox>
					          <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtendertxtNativelanguage"  WatermarkCssClass="Watermark"
                                      runat="server" Enabled="True" WatermarkText="The Message in native language" TargetControlID="txtNativelanguage">
                                  </ajaxToolkit:TextBoxWatermarkExtender>
        </td>
	</tr>	
	</table>
	</td>
	</tr>
	
	    <tr>
            <td colspan="2">
            <br />
                <asp:GridView ID="Grd_UserList" runat="server" AllowPaging="true" 
                    AutoGenerateColumns="false" BackColor="#EBEBEB" BorderColor="#BFBFBF" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" 
                    OnPageIndexChanging="Grd_UserList_PageIndexChanging"
                    Font-Size="15px" PageSize="15" Width="100%">
                    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                    <EditRowStyle Font-Size="Medium" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_select" runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" 
                                    onclick="SelectAll(this)" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
                        <asp:BoundField DataField="Type" HeaderText="Type" />
                        <asp:BoundField DataField="TypeId" HeaderText="TypeId" />
                        <asp:BoundField DataField="UserId" HeaderText="UserId" />
                    </Columns>
                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                        ForeColor="Black" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                        ForeColor="Black" HorizontalAlign="Left" />
                </asp:GridView>
            </td>
        </tr>
      </table>
	</asp:Panel>
	
	 <WC:MSGBOX id="WC_MessageBox" runat="server" />              
		<asp:Panel ID="Panel3" runat="server">
                       
   <asp:Button runat="server" ID="Button_main" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_Message"  runat="server" CancelControlID="Button_MainOk" 
                                  PopupControlID="PanelMain" TargetControlID="Button_main"  BackgroundCssClass="modalBackground" />
   <asp:Panel ID="PanelMain" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
             <div style="font-weight:bold">
             
              <center>
                 <div id="DivMainMessage" runat="server">
                 
                 </div>
                </center>
             
             </div>
               
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button_MainOk" runat="server" Text="OK" Class="btn btn-info" Width="80px"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />                  
  </div>
 </asp:Panel>                 
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

<div class="clear"></div>
</div>
 </ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
