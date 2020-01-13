<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="ScheduleBookToClass.aspx.cs" Inherits="WinEr.ScheduleBookToClass" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
     function SelectAll(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=Grd_SelectItem.ClientID%>');
         var Status = cbSelectAll.checked;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             cb.checked = Status;
         }
     }
     function SelectStudent(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=Grd_SelectStudent.ClientID%>');
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
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Schedule Item</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <asp:Panel ID="Pnl_ScheduleBook" runat="server">
                <table width="100%" class="tablelist">
                <tr>
                <td class="leftside">Select Class</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server" Width="153px" class="form-control"
                        AutoPostBack="True" onselectedindexchanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">Select Category</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Category" runat="server" class="form-control" Width="153px"></asp:DropDownList></td>
                </tr>
                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                 <tr>
                <td class="leftside"></td>
                <td class="rightside">
                <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label>
                </td>
                </tr>
                <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Show" class="btn btn-primary" 
                        onclick="Btn_Show_Click" />
                </td>
                </tr>               
                </table>
                </asp:Panel>
                <table width="100%">
                <tr>
                <td align="right">
                <asp:ImageButton ID="Img_Search" runat="server" ImageUrl="~/Pics/search_female_user.png" ToolTip="Select Student" onclick="Img_Search_Click" 
                 Width="30px" Height="30px"  />
                <%--<asp:Button ID="Btn_SelectStudent" runat="server" Text="Select Student" OnClick="Btn_SelectStudent_Click" />--%>
                <asp:Label ID="Lbl_StudentCount" runat="server"></asp:Label>
                </td>
                </tr>
                <tr>
                <td>
                
                <asp:Panel ID="Pnl_DisplayItem" runat="server">
                 <div class="roundbox">
		            <table width="100%">
		            <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		            <tr><td class="centerleft"></td><td class="centermiddle">
		           
                	<div style=" overflow:auto; max-height: 400px;">
                        <asp:GridView ID="Grd_SelectItem" runat="server" AutoGenerateColumns="False" 
                             Width="97%">
                            <Columns>                                
                       <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                         <ItemTemplate>
                        <asp:CheckBox ID="ChkFee" runat="server" />
                         </ItemTemplate>
                       <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                                    <ItemStyle Width="30px" />
                    </asp:TemplateField> 
                   <%-- Id,ItemName,Category,TotalStock,StudCount--%>
                   <%-- <asp:TemplateField HeaderText="StudCount" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_StudCount" runat="server" Height="20" MaxLength="10" Text="" 
                                            Width="75"></asp:TextBox>

                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_StudCount_FilteredTextBoxExtender" 
                                           FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_StudCount">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="Id" HeaderText="ItemId" />
                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                <asp:BoundField DataField="Category" HeaderText="Category" />                                
                                <asp:BoundField DataField="TotalStock" HeaderText="TotalStock" />   
                                <asp:BoundField DataField="ScheduledCount" HeaderText="Schedule Count" />             
                                <asp:TemplateField HeaderText="Schedule Count" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Count" runat="server" Height="20" MaxLength="10" Text="1"  class="form-control"
                                            Width="75"></asp:TextBox>

                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Count_FilteredTextBoxExtender" 
                                           FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_Count">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Scheduledstatus" HeaderText="Status" />
                            </Columns>                          
                        </asp:GridView>
                    </div>
                    <br />
                     <div style=" overflow:auto; max-height: 400px; text-align:center;">
		            <asp:Button ID="Btn_Schedule" runat="server" Text="Schedule" class="btn btn-success" 
                             onclick="Btn_Schedule_Click"/>
                             <asp:Button ID="Btn_CancelSchedule" runat="server" Text="Cancel Schedule" class="btn btn-danger"
                             onclick="Btn_CancelSchedule_Click"/>
		            </div>
                      </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	
                </asp:Panel>
                
                </td></tr>
             
                   </table>
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

<asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server"  DefaultButton="Btn_magok" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin5" style="width:400px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Label3" runat="server" Text="Message"></asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label><br /><br />
                        <div style="text-align:center;">
                            <asp:HiddenField ID="Hdn_ItemID" runat="server" />
                            <asp:Button ID="Btn_magok" runat="server" Text="Yes" class="btn btn-success"
                                onclick="Btn_magok_Click" />
                            <asp:Button ID="Btn_MsgCancel" runat="server" Text="No" class="btn btn-danger" 
                                onclick="Btn_MsgCancel_Click"/>
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
                        
                        
  <asp:Panel ID="Panel1" runat="server">
                         <asp:Button runat="server" ID="Button1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_SelectStudent"  runat="server"
                           BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_SelectStudent" TargetControlID="Button1" CancelControlID="Btn_Cancel" />
                          <asp:Panel ID="Pnl_SelectStudent" runat="server"  
                          style="display:none;" ><%--style="display:none;"--%>
                         <div class="container skin5" style="width:500px;top:500px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Label1" runat="server" Text="Select Student">
                </asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
            
                         <asp:Panel ID="Panel2" runat="server">
                 <div class="roundbox">
		            <table width="100%">
		            <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		            <tr><td class="centerleft"></td><td class="centermiddle">
		           
                	<div style=" overflow:auto; max-height: 300px;">
                        <asp:GridView ID="Grd_SelectStudent" runat="server" AutoGenerateColumns="False" 
                             Width="100%">
                            <Columns>                                
                       <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                         <ItemTemplate>
                        <asp:CheckBox ID="ChkFee" runat="server"  Checked="true" AutoPostBack="true" OnCheckedChanged="ChkFee_CheckedChanged"/>
                         </ItemTemplate>
                       <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" AutoPostBack="true" OnCheckedChanged="cbSelectAll_CheckedChanged"                                 
                                  Checked="true" onclick="SelectStudent(this)"/>
                            </HeaderTemplate>
                                    <ItemStyle Width="30px" />
                    </asp:TemplateField> 
                    <asp:BoundField DataField="StudentId" HeaderText="Student Id" />
                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                
                            </Columns>                          
                        </asp:GridView>
                    </div>
                    <br />                   
                      </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	
                </asp:Panel>
                        <div style="text-align:center;">
                            <asp:HiddenField ID="Hdn_Value" runat="server" />
                            <asp:Button ID="Btn_Ok" runat="server" Text="Ok" class="btn btn-success"  OnClick="Btn_Ok_Click"
                               />
                                 <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger"
                               />
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
                        
                        
  </ContentTemplate>
  </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
