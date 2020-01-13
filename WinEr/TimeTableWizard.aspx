<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="TimeTableWizard.aspx.cs" Inherits="WinEr.TimeTableWizard" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="TimeTableControl" Src="~/WebControls/TimetableControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script type="text/javascript">
    function CancelEdit() {
    }


    function PeriodCount() {
        var TotalCount = 0;
        var gridViewCtl = document.getElementById('<%=Grd_SelectedSubjects.ClientID%>');
        var Txt_AllotedPerods = document.getElementById('<%=Txt_AllotedPerods.ClientID%>');
        var Txt_MaxPeriods = document.getElementById('<%=Txt_MaxPeriods.ClientID%>');
        var Txt_FreePeriods = document.getElementById('<%=Txt_FreePeriods.ClientID%>');
        if (gridViewCtl != null && gridViewCtl.rows.length>1) {
            for (var i = 1; i < gridViewCtl.rows.length; i++) {
                var Txt_Period = gridViewCtl.rows[i].cells[2].children[0];
                if (parseFloat(Txt_Period.value) > 0) {
                    TotalCount = TotalCount + parseFloat(Txt_Period.value);
                    Txt_Period.value = parseFloat(Txt_Period.value);
                }
                else {
                    Txt_Period.value = 0;
                }
            }
        }
        var MaxCount = parseFloat(Txt_MaxPeriods.value);
        var Remaining = MaxCount - TotalCount;
        Txt_AllotedPerods.value = TotalCount;
        Txt_FreePeriods.style.backgroundColor = '#CCCCCC';
        if (Remaining < 0) {
            Remaining = Remaining * -1;
            alert("Number of allotted periods should not be greater than maximum possible");
            Txt_FreePeriods.style.backgroundColor = 'Red';
        }
        Txt_FreePeriods.value = Remaining;
    }
 



    function SelectAll() {
        var Monday_chk = false;
        var Tuesday_chk = false;
        var Wednesday_chk = false;
        var Thursday_chk = false;
        var Friday_chk = false;
        var Saturday_chk = false;
        var Sunday_chk = false;
        var gridViewCtl = document.getElementById('<%=Grd_SchoolConfig.ClientID%>');
        for (var i = 0; i < 1; i++) {

            var cb = gridViewCtl.rows[i].cells[1].children[0];
            Monday_chk = cb.checked;
            cb = gridViewCtl.rows[i].cells[2].children[0];
            Tuesday_chk = cb.checked;
            cb = gridViewCtl.rows[i].cells[3].children[0];
            Wednesday_chk = cb.checked;
            cb = gridViewCtl.rows[i].cells[4].children[0];
            Thursday_chk = cb.checked;
            cb = gridViewCtl.rows[i].cells[5].children[0];
            Friday_chk = cb.checked;
            cb = gridViewCtl.rows[i].cells[6].children[0];
            Saturday_chk = cb.checked;
            cb = gridViewCtl.rows[i].cells[7].children[0];
            Sunday_chk = cb.checked;
        }
        for (var i = 1; i < gridViewCtl.rows.length; i++) {
            if (Monday_chk) {
                var cb = gridViewCtl.rows[i].cells[1].children[0];
                cb.checked = Monday_chk;
            }
            if (Tuesday_chk) {
                var cb = gridViewCtl.rows[i].cells[2].children[0];
                cb.checked = Tuesday_chk;
            }
            if (Wednesday_chk) {
                var cb = gridViewCtl.rows[i].cells[3].children[0];
                cb.checked = Wednesday_chk;
            }
            if (Thursday_chk) {
                var cb = gridViewCtl.rows[i].cells[4].children[0];
                cb.checked = Thursday_chk;
            }
            if (Friday_chk) {
                var cb = gridViewCtl.rows[i].cells[5].children[0];
                cb.checked = Friday_chk;
            }
            if (Saturday_chk) {
                var cb = gridViewCtl.rows[i].cells[6].children[0];
                cb.checked = Saturday_chk;
            }
            if (Sunday_chk) {
                var cb = gridViewCtl.rows[i].cells[7].children[0];
                cb.checked = Sunday_chk;
            }

        }
    }

    function IndividualClick() {
        var Monday_chk = true;
        var Tuesday_chk = true;
        var Wednesday_chk = true;
        var Thursday_chk = true;
        var Friday_chk = true;
        var Saturday_chk = true;
        var Sunday_chk = true;
        var gridViewCtl = document.getElementById('<%=Grd_SchoolConfig.ClientID%>');
        for (var i = 1; i < gridViewCtl.rows.length; i++) {
            var cb = gridViewCtl.rows[i].cells[1].children[0];
            if (cb.checked==false) {
                Monday_chk = false;
            }
            var cb = gridViewCtl.rows[i].cells[2].children[0];
            if (cb.checked == false) {
                Tuesday_chk = false;
            }
            var cb = gridViewCtl.rows[i].cells[3].children[0];
            if (cb.checked == false) {
                Wednesday_chk = false;
            }
            var cb = gridViewCtl.rows[i].cells[4].children[0];
            if (cb.checked == false) {
                Thursday_chk = false;
            }
            var cb = gridViewCtl.rows[i].cells[5].children[0];
            if (cb.checked == false) {
                Friday_chk = false;
            }
            var cb = gridViewCtl.rows[i].cells[6].children[0];
            if (cb.checked == false) {
                Saturday_chk = false;
            }
            var cb = gridViewCtl.rows[i].cells[7].children[0];
            if (cb.checked == false) {
                Sunday_chk = false;
            }

        }
        for (var i = 0; i < 1; i++) {

            var cb = gridViewCtl.rows[i].cells[1].children[0];
            cb.checked = Monday_chk
            cb = gridViewCtl.rows[i].cells[2].children[0];
            cb.checked = Tuesday_chk
            cb = gridViewCtl.rows[i].cells[3].children[0];
            cb.checked = Wednesday_chk
            cb = gridViewCtl.rows[i].cells[4].children[0];
            cb.checked = Thursday_chk
            cb = gridViewCtl.rows[i].cells[5].children[0];
            cb.checked = Friday_chk
            cb = gridViewCtl.rows[i].cells[6].children[0];
            cb.checked = Saturday_chk
            cb = gridViewCtl.rows[i].cells[7].children[0];
            cb.checked = Sunday_chk
        }
    }
    
</script>

<style type="text/css">
        .TdLeft
        {
            width:50%;
            text-align:right;
            color:Black;
        }
        .TdRight
        {
            width:50%;
            text-align:left;
            color:Black;
        }
        .Invisible
        {
            display:none;
        }
        .Position
        {
            padding-left:5px;
        }
        .FiledCss
        {
            padding-left:10px;
            font-weight:bold;
            border:solid 1px gray;
            border-left:none;
        }
        
        .GreenCell
        {

             background-image:url(images/grn.jpg);
      background-repeat:repeat;
            color:White;
            width:150px;
            height:40px;
            overflow:hidden;
            text-align:center;
            font-weight:bold;
            clear:both;
            padding:10px 10px 10px 10px;
           vertical-align:top;
             font-size:13px;
        }
        
         .RedCell
        {

            background-image:url(images/Red.jpg);
      background-repeat:repeat;
            color:White;
            width:130px;
             height:40px;
            overflow:hidden;
            text-align:center;
            font-weight:bold;
            clear:both;
            vertical-align:top;
            padding:10px 10px 10px 10px;
            font-size:13px;
        }


.YellowCell
        {

            background-image:url(images/ylo.jpg);
	background-repeat:repeat;
            color:Black;
            width:130px;
             height:40px;
            overflow:hidden;
            text-align:center;
            font-weight:bold;
            clear:both;
            vertical-align:top;
            padding:10px 10px 10px 10px;
            font-size:13px;
        }
.ClassTable
{
    width:100%;
    background-color:White;
}

.ClassTR
{
    height:60px;
    width:130px;
}

.AnchorStyle
{
    color:White;
    font-weight:bolder;
    font-size:11px;
}

.HeaderPadding
{
    padding:10px 10px 10px 10px;
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
         
         

         

    <asp:panel ID="Panel2"  runat="server"> 
    
      <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no" style="height: 36px"><asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/3D_cube.gif"  Width="35px" Height="35px"/> </td>
				<td class="n" style="height: 36px">Time Table Wizard</td>
				<td class="ne" style="height: 36px"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			     <br />
			       
			       
			       <asp:Panel ID="PanelWizard"  runat="server" >
			     
                  
                     <asp:Wizard ID="Wizard1" runat="server" DisplayCancelButton="true" Width="100%" 
                           DisplaySideBar="false"  BackColor="#F7F6F3" BorderColor="#CCCCCC" BorderWidth="1px"
                      onfinishbuttonclick="TimeTable_FinishButtonClick"   CancelDestinationPageUrl="~/TimeTableWizard.aspx"  
                     onpreviousbuttonclick="Wizard1_PreviousButtonClick"  CancelButtonText="Exit"
                           OnNextButtonClick="TimeTable_OnNextButtonClick" CellPadding="10" 
                           CellSpacing="10">
                         <StartNextButtonStyle Height="30px" Width="80px" Font-Bold="true" />
                         <FinishCompleteButtonStyle Height="30px" Width="80px" Font-Bold="true" />
                         <StepNextButtonStyle Height="30px" Width="80px"  Font-Bold="true"/>
                         <FinishPreviousButtonStyle Height="30px" Width="80px"/>
                     <WizardSteps>
                            <asp:WizardStep ID="WizardStep1" runat="server" Title="GenRules" >

                              <asp:Panel ID="PanelStep1" runat="server">
                             
                                    <table width="100%" cellspacing="10">
                                        <tr>
                                          <td colspan="2" align="center">
                                           
                                             <h2><u> General Setting </u> </h2>
                                             <br />
                                            
                                          </td>
                                        </tr>
                                        <tr>
                                            <td class="TdLeft">
                                                Periods/day :
                                            </td>
                                            <td class="TdRight">
                                                <asp:TextBox ID="Txt_PeridDay" runat="server" Width="110px" class="form-control" MaxLength="2"></asp:TextBox>
                                                <asp:TextBox ID="PeridDayImage" runat="server" Text='<%#Txt_PeridDay.Text%>' CssClass="Invisible"></asp:TextBox>
                                            
                                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                             runat="server" Enabled="True" FilterType="Numbers"
                                             TargetControlID="Txt_PeridDay">
                                             </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="Txt_PeridDay_ReqVal" runat="server" ControlToValidate="Txt_PeridDay" ErrorMessage="Enter periods/Day" ValidationGroup="ImgRuleSave"></asp:RequiredFieldValidator>
                                            </td>
                                          
                                        </tr>
                                        <tr>
                                        
                                            <td class="TdLeft">
                                                 First period should be handled by class teacher :
                                            </td>
                                            <td class="TdRight">
                                                <asp:CheckBox ID="Chk_FirsrClassT" runat="server" />
                                                <asp:CheckBox ID="FirsrClassTImage" runat="server"  CssClass="Invisible" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TdLeft">
                                                Max number of consective periods for a staff :
                                            </td>
                                            <td class="TdRight">
                                                 <asp:TextBox ID="Txt_MaxConsecutive" runat="server" class="form-control" Width="110px" MaxLength="1"></asp:TextBox>
                                                 <asp:TextBox ID="MaxConsecutiveImage" runat="server" Text='<%#Txt_MaxConsecutive.Text%>' CssClass="Invisible"></asp:TextBox>
                                            
                                              <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                             runat="server" Enabled="True" FilterType="Numbers"
                                             TargetControlID="Txt_MaxConsecutive">
                                             </ajaxToolkit:FilteredTextBoxExtender>
                                             <asp:RequiredFieldValidator ID="Txt_MaxConsecutive_ReqVal" runat="server" ControlToValidate="Txt_MaxConsecutive" ErrorMessage="Enter Consecutive periods/Day" ValidationGroup="ImgRuleSave"></asp:RequiredFieldValidator>
                                             </td>
                                        </tr>
                                        <tr>
                                            <td class="TdLeft">
                                                Max number of teachers/subject for a class :
                                            </td>
                                            <td class="TdRight">
                                                  <asp:TextBox ID="Txt_MaxteacherSub" runat="server" class="form-control" Width="110px" MaxLength="1"></asp:TextBox>
                                                  <asp:TextBox ID="MaxteacherSubImage" Text='<%#Txt_MaxteacherSub.Text%>' runat="server"  CssClass="Invisible"></asp:TextBox>
                                            
                                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                             runat="server" Enabled="True" FilterType="Numbers"
                                             TargetControlID="Txt_MaxteacherSub">
                                             </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_MaxConsecutive" ErrorMessage="Enter Consecutive periods/Day" ValidationGroup="ImgRuleSave"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TdLeft">
                                                Max number of periods for staff/week :
                                            </td>
                                            <td class="TdRight">
                                                  <asp:TextBox ID="Txt_MaxStaffWeekperiod" runat="server" class="form-control" Width="110px" MaxLength="2"></asp:TextBox>
                                                  <asp:TextBox ID="MaxStaffWeekperiodImage" Text='<%#Txt_MaxStaffWeekperiod.Text%>' runat="server"  CssClass="Invisible"></asp:TextBox>
                                            
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                             runat="server" Enabled="True" FilterType="Numbers"
                                             TargetControlID="Txt_MaxStaffWeekperiod">
                                             </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_MaxStaffWeekperiod" ErrorMessage="Enter staff maximum periods/week" ValidationGroup="ImgRuleSave"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TdLeft">
                                                Max number of periods for staff/day :
                                            </td>
                                            <td class="TdRight">
                                                  <asp:TextBox ID="Txt_MaxStaffDayperiod" runat="server" class="form-control" Width="110px" MaxLength="2"></asp:TextBox>
                                                   <asp:TextBox ID="MaxStaffDayperiodImage" Text='<%#Txt_MaxStaffDayperiod.Text%>' runat="server"  CssClass="Invisible"></asp:TextBox>
                                            
                                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_MaxStaffDayperiodFilteredTextBoxExtender" 
                                             runat="server" Enabled="True" FilterType="Numbers"
                                             TargetControlID="Txt_MaxStaffDayperiod">
                                             </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Txt_MaxStaffDayperiod" ErrorMessage="Enter staff maximum periods/day" ValidationGroup="ImgRuleSave"></asp:RequiredFieldValidator>
                                            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TdLeft">Delete generated timetable before starting :</td>
                                            <td class="TdRight">
                                                     <asp:CheckBox ID="Chk_DeleteAll" runat="server" />
                                            </td>
                                        </tr>  
                                    </table>
                             </asp:Panel>
                                
                            </asp:WizardStep>                    
                            
                            <asp:WizardStep ID="WizardStep2" runat="server" Title="SchoolConfig"  >
                            <br /> 
                              <asp:Panel ID="Wzd2_ScholConfig" runat="server" CssClass="Position">
                              
                              <table width="100%">
                               <tr>
                                    <td colspan="2" align="center">
                                           
                                          <h2><u> Weekly Period Configuration </u> </h2>
                                          <br />
                                            
                                      </td>
                                </tr>
                                <tr>
                                    <td>Class
                                        <asp:DropDownList ID="Drp_Wzd2Class" runat="server" Width="160px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_Wzd2Class_OnselectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right"> 
                                        <asp:LinkButton ID="Lnk_ClearAll" runat="server" OnClick="Lnk_ClearAll_Click">Clear All</asp:LinkButton> 
                                        
                                     </td>
                                </tr>
                                <tr>
                                 <td colspan="2">
                                  <br />
                                   <asp:GridView ID="Grd_SchoolConfig" runat="server"   Visible="true" 
                                         CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                                         Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                         BorderWidth="1px">
                                            <RowStyle BackColor="#F7F7DE" />
                                            <Columns>   
                                            <asp:BoundField DataField="PeriodId" HeaderText="Period"   />                 
                                            <asp:BoundField DataField="FrequencyName" HeaderText="Period" 
                                                    ItemStyle-CssClass="FiledCss"   >
                                                <ItemStyle CssClass="FiledCss" />
                                                </asp:BoundField>
                                            <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"  ItemStyle-BorderWidth="1px" HeaderText="Monday"  >
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_MOn" runat="server"  onclick="IndividualClick()" />
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll1" runat="server" Text="Monday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Tuesday">
                                                <ItemTemplate>
                                                     <asp:CheckBox ID="Chk_Tues" runat="server"  onclick="IndividualClick()"/>
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll2" runat="server" Text="Tuesday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Wednesday">
                                                <ItemTemplate>
                                                       <asp:CheckBox ID="Chk_Wed" runat="server"  onclick="IndividualClick()"/>
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll3" runat="server" Text="Wednesday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Thursday">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_Thur" runat="server"  onclick="IndividualClick()"/>
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                 <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll4" runat="server" Text="Thursday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Friday">
                                                <ItemTemplate>
                                                     <asp:CheckBox ID="Chk_Fri" runat="server"  onclick="IndividualClick()"/>
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                 <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll5" runat="server" Text="Friday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Saturday">
                                                <ItemTemplate>
                                                      <asp:CheckBox ID="Chk_Sat" runat="server"  onclick="IndividualClick()"/>
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll6" runat="server" Text="Saturday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Sunday">
                                                <ItemTemplate>
                                                      <asp:CheckBox ID="Chk_Sun" runat="server"  onclick="IndividualClick()"/>
                                                </ItemTemplate>
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                                <HeaderTemplate > 
                                                 <asp:CheckBox ID="cbSelectAll7" runat="server" Text="Sunday" Checked="false" onclick="SelectAll()"/>
                                               </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px" HeaderText="Next Period break" ControlStyle-Height="30px" >
                                                <ItemTemplate>
                                                     <asp:CheckBox ID="Chk_Nxtbrk" runat="server" />
                                                </ItemTemplate>
                                                <ControlStyle Height="30px" />
                                                <ItemStyle BorderWidth="1px" Width="90px" />
                                            </asp:TemplateField>
                                    </Columns>
                                   <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                           <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                           <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                           HorizontalAlign="Left" />
                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                           HorizontalAlign="Left" />
                                                                                                                        
                                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                      <EditRowStyle Font-Size="Medium" />     
                                   </asp:GridView>
                                 </td>
                                </tr>
                              </table>

                          
                            
                    <br />
                    <div style="text-align:right" >
                              <asp:Button runat="server" ID="Img_SaveSchoolConfig" Text="Save Changes"  Width="140px"  class="btn btn-primary" OnClick="Img_SaveSchoolConfig_Click" ToolTip="Save"/>
                            
                             <%-- <asp:ImageButton ID="Img_SaveSchoolConfig" runat="server"   ImageUrl="~/Pics/save.png" Height="45px" Width="45px" OnClick="Img_SaveSchoolConfig_Click" ToolTip="Save"/>--%>&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="Img_ApplyAll" runat="server" Text="Apply Changes to All Class"  class="btn btn-primary" Width="200px"   OnClick="Img_ApplyAll_Click" ToolTip="Apply to all class"/>
                             <%-- <asp:ImageButton ID="Img_ApplyAll" runat="server"  ImageUrl="~/images/accept.png" Height="45px" Width="45px" OnClick="Img_ApplyAll_Click" ToolTip="Apply to all class"/>--%>
                                    <%--<asp:ImageButton ID="Img_EditClass" runat="server"  ImageUrl="~/Pics/tablet.png" Height="45px" Width="45px" OnClick="Img_EditClass_Click" ToolTip="Edit Class settings" Visible="false"/>--%>
                                </div>
                            </asp:Panel>
                            
                               <br /> 
                            </asp:WizardStep>
                            
                            <asp:WizardStep ID="WizardStep3" runat="server" Title="Subject Configuration"  >
                  
                                <asp:Panel ID="Wzd3_SubGroup" runat="server">
                                   
                                   <table width="100%" cellspacing="10">
                                    <tr>
                                      <td align="center">
                                           
                                             <h2><u> Subject Configuration </u> </h2>
                                             <br />
                                            
                                            
                                      </td>
                                    </tr>
                                    <tr>
                                     <td align="right">
                                         
                                         <asp:LinkButton ID="Lnk_SubjectGroups" runat="server" OnClick="Lnk_SubjectGroups_Click">Manage Subject Groups</asp:LinkButton>
                                         
                                     </td>
                                    </tr>
                                    <tr>
                                     <td>
                                      <br />
                                     
                                          <asp:Panel ID="Panel_SubjectGrouping" runat="server" Visible="false">
                                          
                                          
                                          <div style="border:solid 2px silver;padding:10px 10px 10px 10px;background-color:#dadada;margin:10px 10px 10px 10px ">  
                                               
                                              <table width="100%">
                                            <tr>
                                             <td align="left">
                                             
                                              <asp:Button runat="server" ID="Img_EditsubGrp" Class="btn btn-primary" Text="New Group" OnClick="Img_EditsubGrp_Click" ToolTip="New Subject Group"/>
                                             
                                             </td>
                                             <td align="right">
                                              <asp:ImageButton ID="Image_Pnael_SubjectGrouping" runat="server" 
                                                     ImageUrl="~/images/cross.png" Width="20px" 
                                                     OnClick="Image_Pnael_SubjectGrouping_Click" />
                                             </td>
                                            </tr>
                                            <tr>
                                             <td colspan="2">
                                                 
                                                   
                                                  <br />
                                                    <asp:GridView ID="Grd_SubGrp" runat="server" DataKeyNames="Id"
                                                     CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                                                     Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"
                                                     BorderWidth="1px" OnRowDeleting="GrdSubGrp_Deleting" onrowediting="Grd_studentlist_RowEditing">
                                                        <RowStyle BackColor="#F7F7DE" />
                                                        <Columns>   
                                                        <asp:BoundField DataField="Id" HeaderText="Id"   HeaderStyle-Height="30px" >                 
                                                            <HeaderStyle Height="30px" />
                                                            </asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Group Name"  
                                                                 ItemStyle-VerticalAlign="Middle" 
                                                                HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                        <asp:BoundField DataField="AdjPeriods" HeaderText="No.adj prd"  
                                                                 ItemStyle-VerticalAlign="Middle" 
                                                                HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                        <asp:BoundField DataField="MaxPeriodWeek" HeaderText="MaxPrd/Week"  
                                                               ItemStyle-VerticalAlign="Middle" 
                                                                HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle  VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                        <asp:BoundField DataField="MinPeriodWeek" HeaderText="MinPrd/Week"  
                                                                 ItemStyle-VerticalAlign="Middle" 
                                                                HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle  VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                        <asp:CommandField ShowEditButton="true" 
                                                                EditText="&lt;img src='pics/configure1.png' width='30px' border=0 title='Select group to edit'&gt;" 
                                                                HeaderText="Manage Subjects"  ItemStyle-Width="120px" 
                                                                ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                            </asp:CommandField>
                                                        <asp:CommandField DeleteText="&lt;img src='pics/DeleteRed.png' width='30px' border=0 title='Delete Group'&gt;" 
                                                                ShowDeleteButton="True" HeaderText="Delete Group" ItemStyle-Width="100px"  
                                                                ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            </asp:CommandField>
                                                </Columns>
                                                
                                                 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                       <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                       <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                       HorizontalAlign="Left" />
                                                       <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                       HorizontalAlign="Left" />
                                                                                                                                    
                                                 <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                  <EditRowStyle Font-Size="Medium" />     
                                                </asp:GridView>
                                                 
                                                 
                                             </td>
                                            </tr>
                                           </table> 
                                               
                                               
                                              
                                                
                                                </div>
                                          
                                          
                                           
                                          
                                             
                                            
                                          </asp:Panel>
                                     </td>
                                    </tr>
                                    <tr>
                                     <td>
                                     
                                      <br />
                                     
                                       <div style="height:300px;overflow:auto">
                                        <asp:GridView ID="Grid_SubjectConfig" runat="server"   Visible="true" DataKeyNames="SubjectId,GroupId"
                                             CellPadding="4" ForeColor="Black" GridLines="Both"  AutoGenerateColumns="False" 
                                             Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                             BorderWidth="1px" 
                                             onrowdeleting="Grid_SubjectConfig_RowDeleting" 
                                               OnRowEditing="Grid_SubjectConfig_RowEditing">
                                             <RowStyle BackColor="#F7F7DE" />
                                             <Columns>    
                                                <asp:BoundField DataField="SubjectId" HeaderText="SubjectId" />
                                                <asp:BoundField DataField="GroupId" HeaderText="GroupId" />
                                                <asp:BoundField DataField="Subject" HeaderText="Subject"   
                                                     HeaderStyle-HorizontalAlign="Left" >   
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                 </asp:BoundField>
                                                <asp:BoundField DataField="Group" HeaderText="Groups" />  
                                                <asp:BoundField DataField="NoOfClass" HeaderText="No Of Classes"  />  
                                                <asp:BoundField DataField="EstimatedPeriods" HeaderText="Periods / Week"  />  
                                                <asp:BoundField DataField="EstimatedStaffs" HeaderText="Estimated Staffs" /> 
                                                <asp:BoundField DataField="AssignedStaffs" HeaderText="Assigned Staffs"  
                                                     ItemStyle-Font-Bold="true" >      
                                                             
                                                    <ItemStyle Font-Bold="True" />
                                                 </asp:BoundField>
                                                 <asp:CommandField EditText="&lt;img src='pics/users-grey.png' width='30px' border=0 title='Change Group'&gt;" 
                                                         ShowEditButton="True" HeaderText="Change Group" 
                                                     ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" >
                                                     <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                 </asp:CommandField>
                                                 <asp:CommandField DeleteText="&lt;img src='pics/configure1.png' width='30px' border=0 title='Configure Staffs'&gt;" 
                                                         ShowDeleteButton="True" HeaderText="Manage Staff" 
                                                     ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                 </asp:CommandField>
                                                            </Columns>
                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" CssClass="HeaderPadding" Font-Size="12px" ForeColor="Black" Height="30px" VerticalAlign="Middle"
                                                       HorizontalAlign="Left" />
                                                <RowStyle BackColor="White" BorderColor="Olive" Font-Size="12px" ForeColor="Black"
                                                           HorizontalAlign="Left" />
                                                <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                <EditRowStyle Font-Size="Medium" />     
                                              </asp:GridView>
                                       
                                       </div>
                                       
                                     </td>
                                    </tr>
                                    
                                   </table>
                                 
                                 
                                          	


                                </asp:Panel>
                                
                              <br /> 
                            </asp:WizardStep>
                            
                            <asp:WizardStep ID="WizardStep4" runat="server" Title="Class-Subject map" >
                              <br /> 
                                <asp:Panel ID="Wzd4_ClasSubMap" runat="server">
                                   
                                   
                                   <table width="100%">
                                   <tr>
                                     <td align="right">
                                           
                                           
                                             <h2><u> Class Configuration </u> </h2>
                                             <br />
                                            
                                     </td>
                                      <td align="right" style="padding-right:30px;">
                                     
                                       <table width="50%" cellspacing="0">
                                        <tr>
                                         <td align="center" valign="middle" style="width:20px;height:20px;border:solid 1px gray;">
                                           
                                             <img src="images/GreenButton.png" width="15px" height="15px"  alt="" />
                                             
                                         </td>
                                         <td align="left"  style="border:solid 1px gray;">
                                            Fully Configured
                                         </td>
                                        </tr>
                                        <tr>
                                         <td  align="center" valign="middle" style="width:20px;height:20px;border:solid 1px gray;">
                                           
                                           <img src="images/RedButton.png" width="15px" height="15px"  alt=""/>
                                         </td>
                                         <td align="left" style="border:solid 1px gray;">
                                          Partially Configured
                                         </td>
                                        </tr>
                                         <tr>
                                         <td  align="center" valign="middle" style="width:20px;height:20px;border:solid 1px gray;">
                                           
                                           <img src="images/YloButton.png" width="15px" height="15px"  alt=""/>
                                         </td>
                                         <td align="left" style="border:solid 1px gray;">
                                          Period Not Configured
                                         </td>
                                        </tr>
                                       </table>
                                       
                                     <br />
                                     
                                     </td>
                                   </tr>
                                    <tr>
                                     <td colspan="2" style="padding-left:10px;">
                                     
                                     
                                                         <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                                         
                                                         </asp:PlaceHolder>
                                     
                                       
                                    
                                     </td>
                                    </tr>
                                  </table>
                                       
                                   
                                </asp:Panel>
                              <br /> 
                             
                            </asp:WizardStep>
                                                                                  
                            <asp:WizardStep ID="WizardStep5" runat="server" Title="Manual Fixing"  >
                             <br />

                             <table width="100%" cellspacing="10">
                                <tr>
                                  <td align="center" colspan="2">
                                           
                                        <h2><u>Modify Timetable</u> </h2>
                                         <br />                                           
                                  </td>
                                </tr>
                                <tr>
                                   <td valign="top">Class
                                        <asp:DropDownList ID="Drp_WzdStep7Class" runat="server" Width="160px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_WzdStep7Class_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                   </td>
                                   <td align="right">
                                   
                                       <asp:LinkButton ID="Lnk_StaffWorkLoad" runat="server" 
                                           OnClick="Lnk_StaffWorkLoad_Click">View Staff WorkLoad</asp:LinkButton>
                                       
                                   
                                   </td>                             
                                </tr>
                                
                             </table>
                              <br />   
                             <WC:TimeTableControl id="WC_TimeTableControl" runat="server" />  
                             
                             </asp:WizardStep>
                            
                       
                      </WizardSteps>
                       
                               
                        
                        
                            <SideBarTemplate>
                   <asp:DataList ID="SideBarList" runat="server" >
                     <ItemTemplate>
            <!-- Return false when linkbutton is clicked -->
                      <asp:LinkButton  ID="SideBarButton" OnClientClick="return false" ForeColor="White" runat="server"  ></asp:LinkButton>
                       </ItemTemplate>
                      <SelectedItemStyle Font-Bold="false" ForeColor="White"  />
                      </asp:DataList> 
                          </SideBarTemplate>
                          
                            <SideBarButtonStyle Font-Names="Verdana" 
                                ForeColor="White" BorderWidth="0px" />
                            <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                                ForeColor="#284775" />
                            <SideBarStyle BackColor="#7C6F57" Font-Size="0.9em" HorizontalAlign="Center" 
                                VerticalAlign="Top" Width="20%" BorderStyle="Inset" 
                                Font-Names="Times New Roman" BorderWidth="0px" />
                            <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" 
                            Font-Size="0.9em" ForeColor="White" 
                                HorizontalAlign="Left" />
                            <StepPreviousButtonStyle Height="30px" Width="80px" />
                            <CancelButtonStyle Height="30px" Width="80px" Font-Bold="True" />
                            
                    </asp:Wizard>
                 
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
          
    </asp:panel> 
     

 <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
            
 
            
            
                 
                 <asp:Panel ID="Pnl_EdtSubGrp" runat="server">
                         <asp:Button runat="server" ID="Btn_EdtSubGrp" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_EdtSubGrp"  runat="server" CancelControlID="Btn_EdtSubGrpCancel" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_EdtSubjectGrp" TargetControlID="Btn_EdtSubGrp"  />
                          <asp:Panel ID="Pnl_EdtSubjectGrp" runat="server" style="display:none">
                         <div class="container skin5" style="width:600px; top:600px;left:600px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"> <span style="color:White;">Subject Group</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                        <div style="text-align:center">
                                 
                                              <asp:Label ID="Lbl_EdtSubGrpMessage" runat="server" Text=""></asp:Label>
                                        </div>
                                       
                                       
                                                    <table width="100%">
                                                <tr>
                                                    <td class="TdLeft">Group Name <span style="color:Red">*</span></td>
                                                    <td class="TdRight"> 
                                                         <asp:TextBox ID="Txt_SubGrpName" runat="server" class="form-control" Width="100px" MaxLength="50"></asp:TextBox>
                                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_SubGrpName_FilteredTextBoxExtender" 
                                                         runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'\`~!@#$%^&*()_+={}|[]<>,.?;:" 
                                                         TargetControlID="Txt_SubGrpName" />
                                                        <asp:HiddenField ID="Hdn_SubGrp" runat="server" />
                                                        <asp:RequiredFieldValidator ID="Txt_SubGrpName_Reqval" runat="server" ValidationGroup="Btn_EditSubjectGrp" ControlToValidate="Txt_SubGrpName" ErrorMessage="Enter group name"></asp:RequiredFieldValidator>  
                                                        <asp:HiddenField ID="Hdn_SubGrpName" runat="server" />
                                                        <asp:HiddenField ID="Hdn_Grpid" runat="server" />
                                                   </td>
                                                </tr>
                                                <tr>
                                                    <td class="TdLeft"> No. adjecent periods <span style="color:Red">*</span></td>
                                                    <td class="TdRight">
                                                         <asp:TextBox ID="Txt_SubGrpAdjPeriods" runat="server" class="form-control" Width="100px" MaxLength="1"></asp:TextBox>
                                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_SubGrpAdjPeriods_FilteredTextBoxExtender" 
                                                         runat="server"  FilterType="Numbers"    TargetControlID="Txt_SubGrpAdjPeriods" />
                                                          <asp:RequiredFieldValidator ID="Txt_SubGrpAdjPeriods_Reqval" runat="server" ValidationGroup="Btn_EditSubjectGrp" ControlToValidate="Txt_SubGrpAdjPeriods" ErrorMessage="Enter adjacent periods"></asp:RequiredFieldValidator>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TdLeft">Max periods/week <span style="color:Red">*</span></td>
                                                    <td class="TdRight">
                                                         <asp:TextBox ID="Txt_SubGrpMaxPrd" runat="server" Width="100px" class="form-control" MaxLength="2"></asp:TextBox>
                                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_SubGrpMaxPrd_FilteredTextBoxExtender" 
                                                         runat="server"  FilterType="Numbers"    TargetControlID="Txt_SubGrpMaxPrd" />
                                                          <asp:RequiredFieldValidator ID="Txt_SubGrpMaxPrd_ReqVal" runat="server" ValidationGroup="Btn_EditSubjectGrp" ControlToValidate="Txt_SubGrpMaxPrd" ErrorMessage="Enter maximum periods"></asp:RequiredFieldValidator>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TdLeft">Min periods/week <span style="color:Red">*</span></td>
                                                    <td class="TdRight">
                                                         <asp:TextBox ID="Txt_SubGrpMinPrd" runat="server" Width="100px" class="form-control" MaxLength="2"></asp:TextBox>
                                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_SubGrpMinPrd_FilteredTextBoxExtender" 
                                                         runat="server"  FilterType="Numbers"    TargetControlID="Txt_SubGrpMinPrd" />
                                                         <asp:RequiredFieldValidator ID="Txt_SubGrpMinPrd_ReqVal" runat="server" ValidationGroup="Btn_EditSubjectGrp" ControlToValidate="Txt_SubGrpMinPrd" ErrorMessage="Enter minimum periods"></asp:RequiredFieldValidator>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                    ---------------------------------------------------
                                                    ---------------------------------------------------
                                                    --------------------------------
                                                   
                                                    </td>
                                                </tr>
                                                 <tr>
                                                       <td class="TdLeft">SubName</td>
                                                       <td class="TdRight">
                                                         <asp:TextBox ID="Txt_SubName" runat="server" MaxLength="40" class="form-control" Width="100px"></asp:TextBox>
                                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_SubName_FilteredTextBoxExtender" 
                                                         runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'\`~!@#$%^&*()_+={}|[]<>,.?;:"   TargetControlID="Txt_SubName" />
                                                          <asp:RequiredFieldValidator ID="ReqVal_Txt_SubName" runat="server" ValidationGroup="Btn_SubGrp_AddSub" ControlToValidate="Txt_SubName" ErrorMessage="Enter subject name"></asp:RequiredFieldValidator>  
                                                       </td>
                                                  </tr>
                                                  <tr>
                                                        <td class="TdLeft">Subject code</td>
                                                        <td class="TdRight">
                                                          <asp:TextBox ID="Txt_SubCode" runat="server" MaxLength="10" class="form-control" Width="100px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="ReqVal_Txt_SubCode" runat="server" ValidationGroup="Btn_SubGrp_AddSub" ControlToValidate="Txt_SubCode" ErrorMessage="Enter subject code"></asp:RequiredFieldValidator>  
                                                          <ajaxToolkit:FilteredTextBoxExtender ID="Txt_SubCode_FilteredTextBoxExtender" 
                                                         runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'\`~!@#$%^&*()_+={}|[]<>,.?;:"   TargetControlID="Txt_SubCode" />
                                                          <asp:Button ID="Btn_SubGrp_AddSub" runat="server" Text="Add" Width="110px" Class="btn btn-info"  OnClick="Btn_SubGrp_AddSub_Click" ValidationGroup="Btn_SubGrp_AddSub"/>
                                                        </td>
                                                                                                                
                                                  </tr>         
                                                <tr>
                                                  
                                                    <td colspan="2">
                                                         <div style="max-height:250px; overflow:auto">                 
                                                                 <asp:GridView ID="Grd_Subject" runat="server"   Visible="true"
                                                                 CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                                                                 Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                                                 BorderWidth="1px" onrowdeleting="DeleteSubjects">
                                                                    <RowStyle BackColor="#F7F7DE" />
                                                                    <Columns>   
                                                             
                                                                     <asp:BoundField DataField="SubName" HeaderText="Subject name" />       
                                                                     <asp:BoundField DataField="SubCode" HeaderText="Subject code" ControlStyle-Width="100px" />
                                                                  
                                                                    <asp:TemplateField HeaderText="Group" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="Grd_Subject_DrpSubject" Width="100px" class="form-control" runat="server">
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                      <asp:CommandField DeleteText="&lt;img src='pics/block.png' width='30px' border=0 title='Select subject to delete'&gt;" 
                                                                       ShowDeleteButton="True" HeaderText="Delete" />
                                                                    </Columns>
                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                                   <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                                   <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                   HorizontalAlign="Left" />
                                                                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                   HorizontalAlign="Left" />
                                                                                                                                                
                                                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                              <EditRowStyle Font-Size="Medium" />     
                                                        </asp:GridView>
                                                      </div>                
                                                    </td>
                                                </tr>
                                                
                                            </table>

                                        <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_EditSubjectGrp"  OnClick="Btn_EditSubjectGrpSave_Click" runat="server" Text="Save" Class="btn btn-success" Width="100px" ValidationGroup="Btn_EditSubjectGrp"/>     
                                            <asp:Button ID="Btn_EdtSubGrpCancel" runat="server" Text="Close" Class="btn btn-danger" Width="100px" />
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
                 
                  
                 
                 
                 <asp:Panel ID="Pnl_Messsage" runat="server">
                 
                     <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                         <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                              <tr >
                                 <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png"  Height="28px" Width="29px" /> </td>
                                 <td class="n"><span style="color:White">Message!</span></td>
                                 <td class="ne">&nbsp;</td>
                              </tr>
                              <tr >
                                  <td class="o"> </td>
                                  <td class="c" >
                                     <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                                     <br /><br />
                                    <div style="text-align:center;">
                                         <asp:Button ID="Btn_magok" runat="server" class="btn btn-primary" Text="OK" Width="50px"/>
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
                 
                 <asp:Panel ID="Panel4" runat="server">
                         <asp:Button runat="server" ID="Button4" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteGroup"  runat="server" CancelControlID="ButtonClose" BackgroundCssClass="modalBackground"
                                  PopupControlID="Panel5" TargetControlID="Button4"  />
                          <asp:Panel ID="Panel5" runat="server" style="display:none;"> <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:Black">Delete Group</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                       <br />
                                       <asp:HiddenField ID="Hidden_DeleteGroupId" runat="server" />
                                        <asp:Label ID="Label1" runat="server" Text="Before deleting a subject group, please select another group under which subject of deleting group should be placed"></asp:Label>
                                       <br />
                                       <table width="100%" cellspacing="10">
                                            <tr>
                                                <td class="TdLeft" valign="top">Replacing Group</td>
                                                <td class="TdRight">
                                                    <asp:DropDownList ID="Drp_ReplaceGroup" runat="server" class="form-control" Width="140">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td align="center" colspan="2">
                                               
                                                   <asp:Label ID="Lbl_GroupDeleteMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                <br />
                                               </td>
                                            </tr>
                                            <tr>
                                               <td align="center" colspan="2">
                                                 <asp:Button ID="Button_DeleteGroup"   runat="server" Text="Delete"  class="btn btn-primary"
                                                Width="100px" onclick="Button_DeleteGroup_Click"/>     
                                            &nbsp;<asp:Button ID="ButtonClose" runat="server" Text="Close" class="btn btn-danger" Width="100px" />
                                               </td>
                                            </tr>
                                       </table>
                                       
                   
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
                 
                 
                 <asp:Panel ID="Panel6" runat="server">
                         <asp:Button runat="server" ID="Button5" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_SelectedSubjects"  runat="server" BackgroundCssClass="modalBackground"
                                  PopupControlID="Panel7" TargetControlID="Button5"  />
                          <asp:Panel ID="Panel7" runat="server" style="display:none;" > <%--style="display:none;"--%>
                         <div class="container skin1" style="width:700px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:Black">Associated Subjects</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                      <br />
                                       <asp:HiddenField ID="HdnSelectedClassId" runat="server" />
                                      
                                       <table width="100%" cellspacing="0">
                                            <tr>
                                             <td style="width:7%">
                                               <asp:Label ID="Label2" runat="server" Text="Class : "></asp:Label>
                                             </td>
                                             <td align="left"  style="width:40%">
                                               <asp:Label ID="Lbl_SlectedClassName" runat="server" Text="" Font-Bold="true" ForeColor="Black" Font-Size="13px"></asp:Label>
                                             </td>
                                             <td align="right">
                                                 <asp:ImageButton ID="Img_AddSubject" runat="server" ImageUrl="~/Pics/add.png" 
                                                     Width="20px" onclick="Img_AddSubject_Click1" />
                                                 <asp:LinkButton ID="Lnk_AddSubject" runat="server" 
                                                     onclick="Lnk_AddSubject_Click">Add Subject</asp:LinkButton>
                                             </td>
                                            </tr>
                                            
                                            <tr>
                                             <td align="center" colspan="3" style="border-top:solid 1px gray;">
                                               <asp:Panel ID="Panel_SelectedSubjects" runat="server" Visible="true">
                                               <div style="height:150px;overflow:auto;border-bottom:solid 1px gray;"> 
                                                 <asp:GridView ID="Grd_SelectedSubjects" runat="server"   Visible="true" DataKeyNames="ClassSubId,SubjectId,StaffId"
                                                         CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                                                         Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                                         BorderWidth="1px" onrowdeleting="Grd_SelectedSubjects_RowDeleting">
                                                            <RowStyle BackColor="#F7F7DE" />
                                                            <Columns>   
                                                             <asp:BoundField DataField="ClassSubId" HeaderText="ClassSubId" />  
                                                             <asp:BoundField DataField="SubjectId" HeaderText="SubjectId" />  
                                                             <asp:BoundField DataField="StaffId" HeaderText="StaffId" />  
                                                             <asp:BoundField DataField="PerodCount" HeaderText="PerodCount" />  
                                                     
                                                             <asp:BoundField DataField="SubName" HeaderText="Subject name" />       
                                                             
                                                          
                                                            <asp:TemplateField HeaderText="Staff" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="170px" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="Drp_GridStaff" Width="140px" class="form-control" runat="server">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="Periods / Week" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_GridPeriodCount" runat="server" class="form-control" Width="60" onkeyup="PeriodCount()"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                              <asp:CommandField DeleteText="&lt;img src='pics/DeleteRed.png' width='30px' border=0 title='Select subject to delete'&gt;" 
                                                               ShowDeleteButton="True" HeaderText="Delete" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                                            </Columns>
                                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                           <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                           <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                           HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                           HorizontalAlign="Left" />
                                                                                                            
                                                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                      <EditRowStyle Font-Size="Medium" />     
                                                </asp:GridView>
                                                 
                                               </div>
                                               </asp:Panel>
                                             </td>
                                            </tr>
                                            <tr>
                                             <td colspan="3" align="center">
                                              <asp:Label ID="Lbl_GridSubjectsError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                             </td>
                                            </tr>
                                            <tr>
                                              <td align="right" colspan="3" style="padding-right:20px;">
                                                
                                                <table width="60%">
                                                 <tr>
                                                  <td>
                                                     Maximum Periods For A Week : 
                                                  </td>
                                                  <td align="left" style="font-weight:bold">
                                                      <input ID="Txt_MaxPeriods" runat="server" onkeydown="return false" 
                                                        style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                                                        type="text" />
                                                  </td>
                                                 </tr>
                                                 <tr>
                                                  <td>
                                                     Allotted Periods : 
                                                  </td>
                                                  <td align="left">
                                                       <input ID="Txt_AllotedPerods" runat="server" onkeydown="return false" 
                                                        style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                                                        type="text" />
                                                  </td>
                                                 </tr>
                                                 <tr>
                                                  <td>
                                                    Remaining Period : 
                                                  </td>
                                                  <td align="left">
                                                     <input ID="Txt_FreePeriods" runat="server" onkeydown="return false" 
                                                        style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                                                        type="text" />
                                                  </td>
                                                 </tr>
                                                </table>
                                                
                                              </td>
                                             
                                            </tr>
                                            <tr>
                                               <td align="center" colspan="3">
                                                 <asp:Button ID="Btn_SelectedSubjects_Save"   runat="server" Text="Save"  Class="btn btn-success"
                                                Width="100px" onclick="Btn_SelectedSubjects_Save_Click"/>     
                                                   &nbsp; &nbsp;<asp:Button ID="Btn_SelectedSubjects_Close" runat="server" Text="Close"  Class="btn btn-danger"
                                                       Width="100px" 
                                                       OnClientClick="return confirm('Unsaved details will be discarded. Do you want to continue closing?');" 
                                                       onclick="Btn_SelectedSubjects_Close_Click" />
                                               </td>
                                            </tr>
                                       </table>
                                       
                   
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
                 
                 
                 <asp:Panel ID="Panel8" runat="server">
                         <asp:Button runat="server" ID="Button6" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_AddSubject"  runat="server" BackgroundCssClass="modalBackground"
                                  PopupControlID="Panel9" TargetControlID="Button6"  />
                          <asp:Panel ID="Panel9" runat="server" style="display:none;"> <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:Black">Add Subject</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                       <br />                                    
                                       <table width="100%" cellspacing="10">
                                            <tr>
                                             <td align="right"  style="width:50%">
                                               <asp:Label ID="Label4" runat="server" Text="Subject : "></asp:Label>
                                             </td>
                                             <td align="left"  style="width:50%">
                                                 <asp:DropDownList ID="Drp_AddSubjects" runat="server" class="form-control" Width="140px">
                                                 </asp:DropDownList>
                                             </td>
                                            </tr>
                                            
                                            
                                            
                                            <tr>
                                               <td align="center" colspan="2">
                                               
                                                   <asp:Label ID="Lbl_AddSubjectError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                <br />
                                               </td>
                                            </tr>
                                            <tr>
                                               <td align="center" colspan="2">
                                                 <asp:Button ID="Btn_AddSubject"   runat="server" Text="Add"  Class="btn btn-success"
                                                Width="100px" onclick="Btn_AddSubject_Click"/>     
                                                   &nbsp; &nbsp;<asp:Button ID="Btn_AddSubject_Close" runat="server" Text="Close" Class="btn btn-danger" Width="100px" onclick="Btn_AddSubject_Close_Click" />
                                               </td>
                                            </tr>
                                       </table>
                                       
                   
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
                
                
                
                 <asp:Panel ID="Panel10" runat="server">
                         <asp:Button runat="server" ID="Button7" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_AssignedStaff"  runat="server" BackgroundCssClass="modalBackground"
                             CancelControlID="Img_Close"      PopupControlID="Panel11" TargetControlID="Button7"  />
                          <asp:Panel ID="Panel11" runat="server"  style="display:none;"> <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n">
                                      
                                      <table width="100%">
                                       <tr>
                                        <td align="left">
                                         <span style="color:Black">Staff Assigned</span>
                                        </td>
                                        <td align="right">
                                          
                                            <asp:ImageButton ID="Img_Close" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                                        
                                        </td>
                                       </tr>
                                      </table>
                                    
                                    </td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                      <br />
                                       <asp:HiddenField ID="Hidden_SelectedSubject" runat="server" />
                                      
                                       <table width="100%" cellspacing="0">
                                            
                                            <tr>
                                               <td align="center">
                                                 
                                                   <asp:DropDownList ID="Drp_AddStaff" runat="server" class="form-control" Width="140px">
                                                   </asp:DropDownList>
                                                   
                                                   &nbsp;<asp:Button ID="Btn_AddStaff" runat="server" Text="Add Staff" 
                                                       Class="btn btn-primary" onclick="Btn_AddStaff_Click" />
                                               </td>
                                            </tr>
                                            <tr>
                                             <td align="center">
                                               <div style="height:300px;overflow:auto"> 
                                              
                                                <br />
                                                 <asp:Label ID="Lbl_StaffErrorMsg" runat="server" Text=" " ForeColor="Red"></asp:Label>
                                                 <asp:GridView ID="Grd_AssignedStaffs" runat="server"   Visible="true" DataKeyNames="StaffId"
                                                         CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                                                         Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                                         BorderWidth="1px" onrowdeleting="Grd_AssignedStaffs_RowDeleting">
                                                            <RowStyle BackColor="#F7F7DE" />
                                                            <Columns>   
 
                                                             <asp:BoundField DataField="StaffId" HeaderText="StaffId" /> 
                                                     
                                                             <asp:BoundField DataField="SurName" HeaderText="Staff" />       
                                                             
                                                          
                                                            
                                                              <asp:CommandField DeleteText="&lt;img src='pics/DeleteRed.png' width='30px' border=0 title='Select subject to delete'&gt;" 
                                                               ShowDeleteButton="True" HeaderText="Delete" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                                            </Columns>
                                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                           <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                           <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                           HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                           HorizontalAlign="Left" />
                                                                                                            
                                                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                      <EditRowStyle Font-Size="Medium" />     
                                                </asp:GridView>
                                                 
                                              </div>
                                         
                                             </td>
                                            </tr>
                                          
                                       </table>
                                       
                   
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
                
                 
                 
                 <asp:Panel ID="Panel12" runat="server">
                         <asp:Button runat="server" ID="Button8" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_ChangeGroup"  runat="server" BackgroundCssClass="modalBackground"
                             CancelControlID="Img_Close"      PopupControlID="Panel13" TargetControlID="Button8"  />
                          <asp:Panel ID="Panel13" runat="server" style="display:none;" > <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n">
                                      
                                      <asp:HiddenField ID="Hidden_SelectedGroupId" runat="server" />
                                      
                                      <table width="100%">
                                       <tr>
                                        <td align="left">
                                         <span style="color:Black">Change Group</span>
                                        </td>
                                        <td align="right">
                                          
                                            <asp:ImageButton ID="ImageButton_Close" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                                        
                                        </td>
                                      
                                      </table>
                                    
                                    </td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                      <br />
                                      
                                       <table width="100%" cellspacing="0">
                                            
                                            <tr>
                                               <td align="center">
                                                 
                                                   <asp:DropDownList ID="Drp_ChangeGroup" runat="server" class="form-control" Width="140px">
                                                   </asp:DropDownList>
                                                   
                                                   &nbsp;<asp:Button ID="Btn_ChangeGroup" runat="server" Text="Update" 
                                                       Class="btn btn-primary" onclick="Btn_ChangeGroup_Click"  />
                                               </td>
                                            </tr>
                                            <tr>
                                             <td align="center">
                                               
                                                 <asp:Label ID="Lbl_ChangeGroupError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                               
                                             </td>
                                            </tr>

                                           
                                       </table>
                                       
                   
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
                         <ajaxToolkit:ModalPopupExtender ID="Mpe_Finishpopup"  runat="server" BackgroundCssClass="modalBackground"
                             CancelControlID="ImageButton1"      PopupControlID="Panel3" TargetControlID="Button1"  />
                          <asp:Panel ID="Panel3" runat="server" style="display:none;"> <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n">
                                      
                      
                                      
                                      <table width="100%">
                                       <tr>
                                        <td align="left">
                                         <span style="color:Black">TimeTable Finish</span>
                                        </td>
                                        <td align="right">
                                          
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                                        
                                        </td>
                                      
                                      </table>
                                    
                                    </td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                      <br />
                                      
                                       <table width="100%" cellspacing="0">
                                            
                                            <tr>
                                             <td align="center">
                                               
                                                 <asp:Label ID="Label3" runat="server" Text="You are about to finish timetable wizard. Select 'Auto Fix' button to complete automatic fixing of remaining unfixed periods. Otherwise select 'Finish' button." ></asp:Label>
                                               
                                             </td>
                                            </tr>
                                            <tr>
                                             <td>
                                             
                                                  <br />
                                                  <br />
                                             
                                             </td>
                                            </tr>
                                           <tr>
                                            <td align="center">
                                              
                                                <asp:Button ID="Btn_AutoFix" runat="server" Text="Auto Fix" 
                                                    Class="btn btn-primary" onclick="Btn_AutoFix_Click" />
                                                &nbsp;
                                                <asp:Button ID="Btn_FinishWizard" runat="server" Text="Finish" 
                                                    Class="btn btn-success" onclick="Btn_FinishWizard_Click" />
                                              
                                            </td>
                                           </tr>
                                       </table>
                                       
                   
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
                 
                 
                   <WC:MSGBOX id="WC_MessageBox" runat="server" />
             </ContentTemplate>
            </asp:UpdatePanel>    
<div class="clear"></div>
  </div>
  
  
</asp:Content>