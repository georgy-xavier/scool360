<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="EditTransportationFee.aspx.cs" Inherits="WinEr.EditTransportationFee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
     function SelectAll(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=Grd_Amound.ClientID%>');
         var Status = cbSelectAll.checked;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             cb.checked = Status;
         }
     }
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

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
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/Bus.png" 
                        Height="30px" Width="30px" />  </td>
                <td class="n">Edit Fee Schedule</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                               <asp:Panel ID="Pnl_Details" runat="server">
                      <table class="tablelist">
                        
                          <tr>
                              <td  class="leftside">
                                  Class Name<span style="color:Red">*</span></td>
                              <td >
                                  <asp:DropDownList ID="Drp_class2" runat="server" class="form-control" Width="160px" 
                                      AutoPostBack="True" 
                                      onselectedindexchanged="Drp_class2_SelectedIndexChanged">
                                  </asp:DropDownList>
                              </td>
                              <td  class="leftside">
                                  Period<span style="color:Red">*</span></td>
                              <td>
                                  <asp:DropDownList ID="Drp_Perod2" runat="server" class="form-control" Width="160px" 
                                      AutoPostBack="True" 
                                      onselectedindexchanged="Drp_Perod2_SelectedIndexChanged">
                                  </asp:DropDownList>
                              </td>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         
                          <tr>
                              <td  class="leftside">
                                  Due Date<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_DueStud" runat="server" class="form-control" Width="160px" ></asp:TextBox>
                                   <ajaxToolkit:CalendarExtender
                                      ID="Txt_DueStud_CalendarExtender1" runat="server" TargetControlID="Txt_DueStud" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                                  </ajaxToolkit:CalendarExtender>
                              </td>
                               <asp:RegularExpressionValidator ID="Txt_DueStud_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_DueStud" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />   
                                  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                      runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                      TargetControlID="Txt_DueStud_DateRegularExpressionValidator3" />
                              <td  class="leftside">
                                  Last date<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_LastStud" runat="server" class="form-control" Width="160px"></asp:TextBox>
                                  <ajaxToolkit:CalendarExtender
                                      ID="Txt_LastStud_CalendarExtender1" runat="server" TargetControlID="Txt_LastStud" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                                  </ajaxToolkit:CalendarExtender>
                              </td>
                              
                                      <asp:RegularExpressionValidator ID="Txt_LastStud_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_LastStud" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                      runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                      TargetControlID="Txt_LastStud_DateRegularExpressionValidator3" />
                                  
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                            <td class="leftside">Destination</td> 
                             <td align="left">
                                 <asp:DropDownList ID="Drp_Location" runat="server" Width="160px" class="form-control"
                        AutoPostBack="True" onselectedindexchanged="Drp_Location_SelectedIndexChanged"></asp:DropDownList></td>     
                            <td class="leftside"><asp:Label ID="Label_NextBatch" runat="server" Text="Batch"></asp:Label></td>
                            <td>
                                  <asp:RadioButtonList ID="Rdo_Batch" runat="server" RepeatDirection="Horizontal" 
                                      AutoPostBack="true" onselectedindexchanged="Rdo_Batch_SelectedIndexChanged">
                                      <asp:ListItem Text="Current" Value="0" Selected="True"></asp:ListItem>
                                      <asp:ListItem Text="Next" Value="1" ></asp:ListItem>
                                 </asp:RadioButtonList>
                            </td>
                         </tr>
                          <tr>
                              <td >
                                  <asp:TextBox ID="Txt_schduleId" runat="server" class="form-control" Visible="False" 
                                      Width="79px"></asp:TextBox>
                              </td>
                              <td >
                                  &nbsp;</td>
                              <%--<td  colspan="2">
                                  <asp:Button ID="Btn_Update" runat="server"  
                                      Text="Update" Width="80px" onclick="Btn_Update_Click" />
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="Btn_Cancel1" runat="server"  
                                      Text="Cancel" Width="80px" onclick="Btn_Cancel1_Click" />
                              </td>--%>
                          </tr>
                      </table>

                  </asp:Panel>
                  <table width="100%"><tr><td align="center"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label></td></tr></table>
                              <asp:Panel ID="Pnl_AssStud" runat="server">
                 
                 
                 
                 
                   <div class="roundbox">
		<table width="100%">		
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">		
                 <div style=" overflow:auto; max-height: 400px;">
                        <asp:GridView ID="Grd_Amound" runat="server" AutoGenerateColumns="False" 
                            Width="97%">
                            <Columns>
                             <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                         <ItemTemplate>
                        <asp:CheckBox ID="ChkFee" runat="server" />
                         </ItemTemplate>
                       <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField> 
                                <asp:BoundField DataField="Id" HeaderText="Destination Id" />
                                <asp:BoundField DataField="classid" HeaderText="classid" />
                                <asp:BoundField DataField="StudentId" HeaderText="StudentId" />
                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                                <asp:BoundField DataField="Sex" HeaderText="Sex" />                                
                                <asp:BoundField DataField="Destination" HeaderText="Destination" />
                                
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_NewAmount" runat="server" class="form-control" MaxLength="10" Text="" 
                                            Width="75"></asp:TextBox>

                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NewAmound_FilteredTextBoxExtender" 
                                           FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_NewAmount">
                                        </ajaxToolkit:FilteredTextBoxExtender>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Status" runat="server" class="form-control"  Text="" 
                                            Width="100" ReadOnly="true"></asp:TextBox>
                                
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                              
                        </asp:GridView>
                    </div>
                    <br />
                    <div style="overflow:auto;max-height: 400px; text-align:center;">
		<asp:Button ID="Btn_Edit" runat="server" Text="Update" Width="75px" class="btn btn-primary" onclick="Btn_Edit_Click" />
		</div>
                 
           </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	           
                 

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
<WC:MSGBOX id="WC_MessageBox" runat="server" />   
  </ContentTemplate>
  </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
