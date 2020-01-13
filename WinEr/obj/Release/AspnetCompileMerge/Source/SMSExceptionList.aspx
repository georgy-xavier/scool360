<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSExceptionList.aspx.cs" Inherits="WinEr.SMSExceptionList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function SelectAllsearch(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_Searchstudent.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }
    }

    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=GridStudentExceptionList.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }
    }
</Script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
</ajaxToolkit:ToolkitScriptManager>  

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
<div class="container skin1" >
    <table cellpadding="0" cellspacing="0" class="containerTable">
    <tr >
    <td class="no"> </td>
    <td class="n">SMS Exception List</td>
    <td class="ne"> </td>
    </tr>
    <tr >
    <td class="o"> </td>
    <td class="c" >

        <asp:Panel ID="Pnl_ExceptionList" runat="server">
            <table width="100%">
            <tr>
            <td>
                <center>
                    <table width="100%">
                    <tr>
                    <td align="right">Select Configuration</td>
                    <td align="left">
                    <asp:DropDownList ID="Drp_Config" runat="server" Width="300px" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="Drp_Config_SelectedIndexCahnged"></asp:DropDownList></td>
                    <td align="right">
                    <asp:Button ID="Btn_AddNew" runat="server" Text="Add"  Class="btn btn-success"  OnClick="Btn_AddNew_Click"/>
                    <asp:Button ID="Btn_Remove" runat="server" Text="Remove"  Class="btn btn-danger"  OnClick="Btn_Remove_Click"/>
                    </td>
                    </tr>
                    </table>
                </center>
                <br />
            </td>
            </tr>
            <tr>
            <td  align="center">
            <asp:Label ID="Lbl_Msg" runat="server" class="control-label" ForeColor="Red"></asp:Label>
            </td>
            </tr>
            <tr>
            <td>
                <asp:GridView  ID="GridStudentExceptionList" runat="server" CellPadding="4" AllowPaging="true" PageSize="15"
                AutoGenerateColumns="False"  onpageindexchanging="GridStudentExceptionList_PageIndexChanging" 
                ForeColor="Black" GridLines="Vertical"  
                Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"   BorderWidth="1px">
                <RowStyle BackColor="#F7F7DE" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                <HeaderTemplate > 
                <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="false"   OnClick="SelectAll(this)"/>
                </HeaderTemplate>
                <ItemTemplate  >
                <asp:CheckBox ID="CheckBoxUpdate" runat="server"/>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" Width="40px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText ="StudentId" /> 
                <asp:BoundField DataField="excId" HeaderText ="Exceptionlist Id" />                 
                <asp:BoundField DataField="StudentName" HeaderText="Name" />
                <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                <asp:BoundField DataField="ClassName" HeaderText="Class" /> 
                </Columns>
                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"   HorizontalAlign="Left" />                                                          
                <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                <EditRowStyle Font-Size="Medium" />     
                </asp:GridView>
            </td>
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
<div>
        <asp:Button runat="server" ID="Button1" class="btn btn-info" style="display:none"/>
        <ajaxToolkit:ModalPopupExtender ID="MPE_ADDNEWSTUDEENTS"   runat="server" CancelControlID="Img_Close"   PopupControlID="Pnl_AddStudent" TargetControlID="Button1"  />
            <asp:Panel ID="Pnl_AddStudent" runat="server"  style="display:none;"><%-- style="display:none;"--%>
            <div class="container skin5" style="width:800px; top:600px;left:200px" >
            <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
            <td class="no">
            </td>
            <td class="n">
             <table width="100%">
                <tr>
                  <td align="left">
                 Search
                  </td>
                  <td align="right">
                      <asp:ImageButton ID="Img_Close" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                  </td>
                </tr>
              </table></td>
            <td class="ne">&nbsp;</td>
            </tr>
            <tr >
            <td class="o"> </td>
            <td class="c" >

            <asp:Panel ID="Pnl_AddSibInitial" runat="server">
            <table width="100%">
            <tr>
            <td>
            Class Name
           <asp:DropDownList ID="Drp_SearcgClass" runat="server" class="form-control" Width="153px"></asp:DropDownList>
            </td>
            <td>
            Student Name
            <asp:TextBox ID="Txt_StudentName" runat="server" class="form-control"> </asp:TextBox>
            <cc1:AutoCompleteExtender ID="TxtSearch_AutoCompleteExtender" 
            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetStudentNameData" ServicePath="~/WinErWebService.asmx"  
            TargetControlID="Txt_StudentName" UseContextKey="true"   MinimumPrefixLength="1" >
            </cc1:AutoCompleteExtender>
            </td>
            <td>
            Parent Name
            <asp:TextBox ID="Txt_ParentName" runat="server" class="form-control"> </asp:TextBox>
            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" 
            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetGurardianNameData" ServicePath="~/WinErWebService.asmx"  
            TargetControlID="Txt_ParentName" UseContextKey="true"  MinimumPrefixLength="1">
            </cc1:AutoCompleteExtender>
            </td>
            <td>
            Phone No:
            <asp:TextBox ID="Txt_PhoneNum" runat="server" class="form-control"> </asp:TextBox>
            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" 
            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetPhoneNumberData" ServicePath="~/WinErWebService.asmx"  
            TargetControlID="Txt_PhoneNum" UseContextKey="true"  MinimumPrefixLength="1">
            </cc1:AutoCompleteExtender>
            <ajaxToolkit:FilteredTextBoxExtender id="FilteredTextBoxExtender2" runat="server" 
            Enabled="True" FilterType="Numbers" TargetControlID="Txt_PhoneNum" >
            </ajaxToolkit:FilteredTextBoxExtender>
            </td>
            </tr>
            <tr>
            <td></td>
            <td></td>
            <td></td>
            <td><asp:Button ID="Btn_Search" runat="server" Text="Search" 
            onclick="Btn_Search_Click" class="btn btn-info" Width="100px"/></td>
            </tr>
            <tr>
            <td colspan="4" align="center"><asp:Label ID="Lbl_Err" runat="server" class="control-label" ForeColor="Red"></asp:Label></td>
            </tr>

            <tr>
            <td colspan="4" align="center">   
            <br />                     
            <div style=" overflow:auto; max-height:300px;">
            <asp:Panel ID="Pnl_student" runat="server">                         
            <asp:GridView ID="Grd_Searchstudent" runat="server" AutoGenerateColumns="false" 
            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4"    
            ForeColor="Black" GridLines="Vertical" Width="100%">

            <Columns>
            <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
            <HeaderTemplate > 
            <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="false"   OnClick="SelectAllsearch(this)"/>
            </HeaderTemplate>
            <ItemTemplate  >
            <asp:CheckBox ID="CheckBoxUpdate" runat="server"  
            />
            </ItemTemplate>


            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width="40px" />

            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="Id" /> 
            <asp:BoundField DataField="StudentName" HeaderText="Name" /> 
            <asp:BoundField DataField="OfficePhNo" HeaderText="Phone Number" />
            </Columns>


            </asp:GridView>
            <br />
          
            </asp:Panel>
            
        </div>
           <div style="text-align:right;">
        <asp:Button ID="Btn_Save" runat="server" class="btn btn-info" Text="Save" Width="90px" 
        onclick="Btn_Save_Click"/>
        </div>
        </td>
        </tr>
        </table>
        </asp:Panel>
        <br /><br />
       
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
        </div>
        <WC:MSGBOX id="WC_MessageBox" runat="server" />  
</ContentTemplate>
</asp:UpdatePanel>
<div class="clear"></div>
</div>

</asp:Content>
