<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AdvFeeSettlement.aspx.cs" Inherits="WinEr.AdvFeeSettlement"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 <script type="text/javascript">
 
   function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grd_advance.ClientID%>');
            var Status=cbSelectAll.checked;
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
 
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">   <img alt="" src="Pics/process.png" width="30" height="30" /> </td>
				<td class="n">Advance Settle</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					


                 <div style="min-height:400px" >
                   
                   <table width="100%">
                    <tr>

                     <td style="width:10%">
                     
                     </td>
                     <td style="width:90%" align="right">

                         
                         &nbsp;<asp:Button ID="Btn_Settle" runat="server" Text="Settle All" Class="btn btn-primary" 
                             onclick="Btn_Settle_Click" />
                           
                           
                         &nbsp;&nbsp;
                           
                           
                         <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" 
                             onclick="Btn_Cancel_Click" />
                     
                     </td>
                    </tr>
                    <tr>
                     <td colspan="2">
                     
                     <br />
                     
                     
                      <asp:GridView ID="Grd_advance" runat="server" AutoGenerateColumns="False"   BackColor="#EBEBEB" 
                           BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"      CellPadding="3" 
                           CellSpacing="2" Font-Size="12px"  Width="100%" AllowPaging="true" PageSize="20"
                             onpageindexchanging="Grd_advance_PageIndexChanging" 
                             onselectedindexchanged="Grd_advance_SelectedIndexChanged"    >                         
                           <Columns>       
                              <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                               <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server" />
                               </ItemTemplate>
                               <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="false" onclick="SelectAll(this)"/>
                               </HeaderTemplate>
                              </asp:TemplateField>  
                                <asp:BoundField DataField="StudentId"  HeaderText="StudentId"  />
                                <asp:BoundField DataField="StudentName"  HeaderText="StudentName"/>
                                <asp:BoundField DataField="ClassName"  HeaderText="ClassName"  />
                                <asp:BoundField DataField="AvailableAdvance"  HeaderText="Available Advance"/>
                                <asp:BoundField DataField="DueAmount"  HeaderText="Due Amount"/>
                                <asp:CommandField HeaderText="Settle" ShowSelectButton="True"   ItemStyle-Width="30" SelectText="&lt;img src='pics/certificate.png' height='25px' width='25px' border=0 title='Settle'&gt;" />   
                           </Columns>
                                                                     
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                          <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />
                          <SelectedRowStyle BackColor="White" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                          <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left"    VerticalAlign="Top" /> 
                      </asp:GridView>
                     
                     
                         <asp:Label ID="lbl_msg" runat="server" Text="" class="control-label" ForeColor="Red" ></asp:Label>
                     
                     </td>
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
              
<WC:MSGBOX id="WC_MessageBox" runat="server" />  

<div class="clear"></div>
</div>

 </ContentTemplate>               
</asp:UpdatePanel>

</asp:Content>
