<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentTripMap.aspx.cs" Inherits="WinEr.StudentTripMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <script type="text/javascript">
        function Reload(page) {

            if (testParent()) {
                window.opener.location = page;
               
            }
            else {
                window.location = page;
            }
        }

      function testParent() {
          if (window.opener != null && !window.opener.closed)
              return true;
          else {
              return false;
          }
      }

      function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('Grd_StudentTripMap');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {
            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }

    }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 

  <div id="HeaderDiv" runat="server">
   <%--<center>        
        <table width="100%" border="1">
        <tr>
        <td style="font-size:larger; font-weight:bold;">Route: 500D</td>
        <td style="font-size:larger; font-weight:bold;">Trip: Mng8.30-9.30</td>
        </tr>
        </table>
        </center>--%>
        </div>  
    <center>
    <br />
        <asp:Panel Width="450px" BorderStyle="Groove" ID="Pnl_Data" runat="server">
        
        <table width="400px" cellspacing="1">
        <tr>
        <td align="right">Class</td>
        <td align="left"><asp:DropDownList ID="Drp_Class" runat="server" Width="153px" 
                AutoPostBack="True" onselectedindexchanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList></td>
      
        <td align="right">Destination</td>
        <td align="left"><asp:DropDownList ID="Drp_Destination" runat="server" 
                Width="153px" AutoPostBack="True" 
                onselectedindexchanged="Drp_Destination_SelectedIndexChanged"></asp:DropDownList></td>
       </tr>
       <tr>
        <td align="right">Sex</td>
        <td align="left"><asp:DropDownList ID="Drp_Sex" runat="server" Width="153px" 
                AutoPostBack="True" onselectedindexchanged="Drp_Sex_SelectedIndexChanged">
           <asp:ListItem Text="All" Value="0"></asp:ListItem>
        <asp:ListItem Text="Male" Value="1"></asp:ListItem>
        <asp:ListItem Text="Female" Value="2"></asp:ListItem>
        </asp:DropDownList></td>

        <td  align="right">Status</td>
        <td  align="left">
        <asp:DropDownList ID="Drp_Status" runat="server" Width="153px" AutoPostBack="True" 
                onselectedindexchanged="Drp_Status_SelectedIndexChanged">
        <asp:ListItem Text="Assigned" Value="0"></asp:ListItem>
        <asp:ListItem Text="Unassigned" Value="1"></asp:ListItem>
        </asp:DropDownList>
        </td>
       </tr>      
<tr>
    <td align="center" colspan="4">
        <asp:Button ID="Btn_ShowAll" runat="server" Text="Show" 
            onclick="Btn_ShowAll_Click" />
        <asp:Button ID="Btn_Calcel" runat="server" Text="Cancel"  
            OnClientClick="javascript:window.close();" onclick="Btn_Calcel_Click"  />
    </td>
   
              
        </tr>
       
        </table>
       </asp:Panel>
        <table width="100%"> <tr><td align="center">
            <asp:HiddenField ID="Hdn_alloted" runat="server" />
            <asp:HiddenField ID="Hdn_Routename" runat="server" />
            <asp:HiddenField ID="Hdn_tripname" runat="server" />
            <asp:HiddenField ID="Hdn_routId" runat="server" />
            <asp:HiddenField ID="Hdn_TripId" runat="server" />
            
            
            <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label></td></tr></table>
       
        <asp:Panel ID="Pnl_Display" runat="server">
        <table width="100%">
        <tr><td align="right"><asp:Button ID="Btn_Assign" runat="server" Text="Assign" 
                onclick="Btn_Assign_Click" /><asp:Button ID="Btn_Remove" runat="server" 
                Text="Remove" onclick="Btn_Remove_Click" /></td></tr>
        <tr>
        <td>
                         <asp:GridView ID="Grd_StudentTripMap" runat="server" AutoGenerateColumns="false"   SkinID="GrayNoRowstyle"
                       BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                       CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="true" 
                             PageSize="10" Width="100%" 
                             onpageindexchanging="Grd_StudentTripMap_PageIndexChanging">
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" 
                        />
                       <EditRowStyle Font-Size="Medium" />
                       <Columns>
                         <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                         <ItemTemplate>
                        <asp:CheckBox ID="ChkFee" runat="server" />
                         </ItemTemplate>
                       <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField>                    
                             <asp:BoundField DataField="Id" HeaderText="Id" />
                           <asp:BoundField DataField="StudentName" HeaderText="Name" 
                               SortExpression="StudentName" HeaderStyle-HorizontalAlign="Left" />
                           <asp:BoundField DataField="ClassName" HeaderText="Class" 
                               SortExpression="ClassName" HeaderStyle-HorizontalAlign="Left"  />                          
                           <asp:BoundField DataField="Sex" HeaderText="Sex" SortExpression="Sex" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="Address" HeaderText="Address" 
                               ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="Destination" HeaderText="Destination" 
                               SortExpression="Destination" HeaderStyle-HorizontalAlign="Left"  />    
                                <asp:BoundField DataField="DestinationId" HeaderText="DestinationId" />                     
                          
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
     </center>
     <WC:MSGBOX id="WC_MessageBox" runat="server" /> 


    </div>
    </form>
</body>
</html>
