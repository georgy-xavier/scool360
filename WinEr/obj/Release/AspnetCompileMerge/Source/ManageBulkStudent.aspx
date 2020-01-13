<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" Inherits="ManageBulkStudent"  Codebehind="ManageBulkStudent.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .Tdleft
        {
            text-align:right;
            color:Black;
        }
        .TdRight
        {
            text-align:left;
            color:Black;
            font-weight:bold;
        }
    </style>
    
<style type="text/css">
        .IncBlock
        {
            height: 220px;
            
        }
        .HightLightedArea
        {
            font-weight: bold;
            
            
        }
         .IncBlock a 
       {
   
     color: #546078; text-decoration: none;  
         } 
    </style>
 <script language="javascript" type="text/javascript">
       function openIncpopup(strOpen) {
           open(strOpen, "Info", "status=1, width=700, scrollbars = 1,  height=550,resizable = 1");
       }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<div id="contents">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


    
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><img alt="" src="Pics/Class.png" width="30" height="30" /> </td>
				<td class="n">
				<div id="topdiv" runat="server" style="width:100%;">
<table width="100%" >
<tr>
<td align="left" >
<div class="form-inline">
&nbsp;  
&nbsp;Class: &nbsp;<asp:DropDownList ID="drp_selectClass" runat="server" AutoPostBack="true" Width="150px" class="form-control"
        onselectedindexchanged="drp_selectClass_SelectedIndexChanged">
    </asp:DropDownList>
    </div>
</td>
<td align="right" >
&nbsp;  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/Student Total.png"   
        Height="25px" Width="25px" /> 
&nbsp; Students: &nbsp;<asp:Label ID="lbl_students" runat="server" Text="20"></asp:Label>
</td>
<td align="right" >
&nbsp;  <asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/Student Male.png"   
        Height="25px" Width="25px" />
&nbsp;Boys: &nbsp;<asp:Label ID="lbl_boys" runat="server" Text="10"></asp:Label>
</td>
<td align="left" >
&nbsp;  <asp:Image ID="Image3" runat="server" 
        ImageUrl="~/Pics/Student Female.png"   Height="25px" Width="25px" />
&nbsp;Girls: &nbsp;<asp:Label ID="lbl_girls" runat="server" Text="10"></asp:Label>
</td>
</tr>
</table>
</div>
				
				</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
	<asp:Panel ID="Pnl_studlist" runat="server" >
    <div>
        <asp:GridView ID="Grd_Students" runat="server" CellPadding="4" ForeColor="Black" 
            GridLines="Vertical" Width="100%" AutoGenerateColumns="False"  OnSelectedIndexChanged="Grd_Students_SelectedIndexChanged"
            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Student Id" />
                
                <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="Img_studImage" runat="server" Width="45px" Height="50px" />  
                            </ItemTemplate>
                </asp:TemplateField>
              <%--  <asp:TemplateField HeaderText="Rl No." ItemStyle-Width = "50px">
                   <ItemTemplate >
                      
                       
                   </ItemTemplate>  
                </asp:TemplateField>--%>
              <%--  <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                <asp:BoundField DataField="AdmitionNo" HeaderText="Admission No" />      
                <asp:BoundField DataField="Sex" HeaderText="Sex" />   
                <asp:BoundField DataField="DOB" HeaderText="DOB" />       
                <asp:BoundField DataField="Religion" HeaderText="Religion" />       
                <asp:BoundField DataField="CastName" HeaderText="Cast" />    
                <asp:BoundField DataField="GardianName" HeaderText="Gaurdian Name" />   --%>   
               <%-- <asp:BoundField DataField="ResidencePhNo" HeaderText="Phone (Res)" />  
                <asp:BoundField DataField="OfficePhNo" HeaderText="Phone (Off)" />      
                <asp:BoundField DataField="GroupName" HeaderText="Bld Grp" />   --%>
                <asp:TemplateField>
        <ItemTemplate>
    <div id = "DivDetailsInGrid" runat="server"  style="width:100%;">
    <table width="100%">
    <tr>
    <td style="text-align:left;"><h4><%# Eval("StudentName")%></h4></td>
    <td style="text-align:left;width:30%;">Father/Guardian:<span class="HightLightedArea"><%# Eval("GardianName")%></span></td>
    <td style="text-align:left;width:30%;">Blood Group:<span class="HightLightedArea"><%# Eval("GroupName")%></span></td>
    </tr>
    <tr >
    <td style="text-align:left;"><span class="HightLightedArea"><%# Eval("Sex")%></span>, DOB:<span class="HightLightedArea"><%# Eval("DOB")%></span></td>
    <td style="text-align:left;">Religion:<span class="HightLightedArea"><%# Eval("Religion")%></span></td>
    <td style="text-align:left;">Caste:<span class="HightLightedArea"><%# Eval("CastName")%></span></td>
    </tr>
    <tr >
    <td style="text-align:left;">RollNo:<span class="HightLightedArea"><asp:Label  ID="Lbl_RollNumber" runat="server" Text="0"  ></asp:Label></span>, Admission No:<span  class="HightLightedArea"><%# Eval("AdmitionNo")%></span></td>
    <td style="text-align:left;">School Bus:<span class="HightLightedArea"><%# Eval("SchoolBus")%></span></td>
    <td style="text-align:left;">Hostel:<span class="HightLightedArea"><%# Eval("Hostel")%></span></td>
    </tr>
    
    </table>
    <%--<table width="100%" ><tr><td align="center">
    <table width="100%" >
    <tr>
    
    <td  align="left" valign= "middle"  >
    <table>
    <tr>
    <td valign="top" >
    <h5> <%# Eval("StudentName") %> </h5> </a></td>
    
    <td valign="bottom" align="right" style="font-size:smaller;" >Admition NO: <%# Eval("AdmitionNo")%> </td>
    </tr>
    <tr>
    <td  >
    <h5>Class Rank</h5>
    <div id="Div2" class="linestyle" runat="server"></div>
    </td>
    </tr>
    <tr>
    <td >
     Secured 3 marks out of 100 for the WS Exam.The result is Failed and the grade is F .Rank obtained is 0.
    </td>
    </tr>
    </table>
    </td>
    </tr>
    </table>
    </td></tr></table>--%>
    </div>
        
        </ItemTemplate>
        </asp:TemplateField>
                <asp:CommandField ShowSelectButton="True" HeaderText="Edit" SelectText="&lt;img src='Pics/edit student.png' width='30px' border=0 title='Edit Student' &gt;" />
               
             </Columns>  
            <RowStyle BackColor="#F7F7DE" />
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                HorizontalAlign="Left" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
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

</ContentTemplate>
</asp:UpdatePanel>

	                                             
</div>

</asp:Content>

