<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="CreateClassIncident.aspx.cs" Inherits="WinEr.WebForm25" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
<div id="right">



<div class="label">Class Manager</div>
<div id="SubClassMenu" runat="server">
		
 </div>

</div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<div id="left">
 
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no"> </td>
                <td class="n"> Report Class Incident</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                    <asp:Panel ID="Pnl_mainarea" runat="server">
                        <table width="100%" class="tablelist">
                            <%--<tr>
                                <td>
                                    &nbsp;</td>
                                <td> &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                    
                            </tr>--%>
                            <tr>
                                <td class="leftside">
                                    Incident Type<span class="style1">*</span></td>
                                <td class="rightside">
                                    <asp:DropDownList ID="Drp_InceType" runat="server" Width="160px" AutoPostBack="true" class="form-control"
                                        onselectedindexchanged="Drp_InceType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    
                                     &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                    <asp:Image ID="Img_Up" runat="server" ImageUrl="images/pt1_up.png" Width="30px" Height="30px" />
                                    <asp:Image ID="Img_Down" runat="server" ImageUrl="images/pt1_dwn.png" Width="30px" Height="30px" />
                                    <asp:Label ID="lbl_PointText" runat="server" Font-Bold="false" Text="Points :"></asp:Label>
                                    <asp:Label ID="lbl_Points" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    
                                </td>
                                </tr>
                              <tr>
                                  <td>
                                      &nbsp;</td>
                                  <td>
                                      &nbsp;</td>
                            </tr>
                              <tr>
                                <td class="leftside">Incident Title<span class="style1">*</span>
                                </td>                   
                                <td class="rightside">
                                    <asp:DropDownList ID="Drp_Title" runat="server" Width="304px" AutoPostBack="true" class="form-control"
                                        onselectedindexchanged="Drp_Title_SelectedIndexChanged">
                                    </asp:DropDownList>
                                
                                </td>      
                              </tr>
                              <tr>
                                  <td>
                                      &nbsp;</td>
                                  <td >
                                      &nbsp;</td>
                            </tr>
                              <tr>
                                <td class="leftside"> Incident Description<span class="style1">*</span></td>
                                <td class="rightside">
                                    <asp:TextBox ID="Txt_Dese" runat="server" Width="300px"  class="form-control"
                                        MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                             
                                  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Dese_FilteredTextBoxExtender1" 
                                      runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                      InvalidChars="'\/" TargetControlID="Txt_Dese">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_Dese" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </td>
                              </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Incident Date<span class="style1">*</span>
                                </td>
                                <td class="rightside">
                                    <asp:TextBox ID="Txt_Date" runat="server" Width="160px" class="form-control"></asp:TextBox>
                                
                                <ajaxToolkit:CalendarExtender ID="Txt_Date_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_Date" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                               
                                                           <asp:RegularExpressionValidator ID="Txt_Date_RegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_Date" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                    runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                    TargetControlID="Txt_Date_RegularExpressionValidator3" />
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_Date" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td class="rightside">
                                    <asp:Button ID="Btn_Create" runat="server" Text="Create" Class="btn btn-primary"
                                        onclick="Btn_Create_Click"/>
                                        
                                    <asp:Button ID="Btn_CrAndApr" runat="server" Text="Create &amp; Approve" 
                                        onclick="Btn_CrAndApr_Click" Class="btn btn-success"  Width="145px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                        </table>
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

</div>
<WC:MSGBOX id="WC_MessageBox" runat="server" />  
<div class="clear"></div>
     
</div>
</asp:Content>
