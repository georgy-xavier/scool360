<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ImportCCEDescriptive.aspx.cs" Inherits="WinEr.ImportCCEDescriptive" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var validFilesTypes = ["xls"];

    function CheckExtension(file) {
        /*global document: false */
        var isValidFile = false;
        var filePath = file.value;
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
        }

        if (!isValidFile) {
            file.value = null;
            alert("Invalid File. Valid extensions are:\n\n" + validFilesTypes.join(", "));
        }
        return isValidFile;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
       <ContentTemplate>
       <div class="container skin1" >
       <table cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
				<td class="no"> </td>
				<td class="n">Import CCE Descriptive</td>
				<td class="ne"> </td>
			</tr>
			
				<tr >
				<td class="o"></td>
				<td class="c">
				 <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                     <table class="tablelist">
                     
                     <tr>
                     <td class="leftside"><br /><br /></td>
                     <td class="rightside"><br /><br /></td>
                     </tr>
                    
                     <tr>
                     <td class="leftside">Select Class Name</td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" AutoPostBack="true" 
                             Width="250px" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Select Part</td>
                     <td class="rightside">
                     <asp:DropDownList ID="Drp_part" runat="server" AutoPostBack="true" class="form-control"
                          Width="250px" OnSelectedIndexChanged="Drp_part_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Select Subject Name</td>
                     <td class="rightside">
                      <asp:DropDownList ID="Drp_Subject" runat="server" AutoPostBack="true" class="form-control"
                               Width="250px" OnSelectedIndexChanged="Drp_Subject_SelectedIndexChanged">
                          </asp:DropDownList>
                     </td>
                     </tr>
                     
                    <tr>
                    <td class="leftside"></td>
                    <td class="rightside"></td>
                    </tr>
                     
                     <tr id="validationrow" runat="server" visible="false">
                     <td class="leftside"><br />Select an Excel File</td>
                     <td class="rightside">
                     <asp:FileUpload ID="FileUpload_Excel" runat="server" Height="30px" Width="100px" onchange="return CheckExtension(this);"/>
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_upload" runat="server" Text="Upload" Class="btn btn-primary" ToolTip="Upload student grade" onclick="Btn_upload_Click" />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:ImageButton ID="Btn_Template"  ToolTip="Download Template" 
                             ImageUrl="~/Pics/Excel.png" runat="server" 
                             Height="47px" Width="42px" OnClick="Btn_Template_Click"/>
                     
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     <tr><td colspan="2" align="center">
                         <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" class="control-label" Visible="false"></asp:Label>
                     </td></tr>
                     <tr>
                     <td class="leftside"><br /><br /></td>
                     <td class="rightside"><br /><br /></td>
                     </tr>
                     
                     </table>
                 </asp:Panel>
				</td>
				<td class="e"></td>
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
        <Triggers>

             <asp:PostBackTrigger ControlID="Btn_upload" />
             <asp:PostBackTrigger ControlID="Btn_Template" />
   </Triggers>
     </asp:UpdatePanel>

</asp:Content>
