<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Codebehind="StudentPrevPerform.aspx.cs" Inherits="StudentPrevPerform"  %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #prfomancetable
        {
          
         
        }
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">
<div id="right">

<div id="sidebar2">


</div>
<div class="label">Student Info</div>
<div id="SubStudentMenu" runat="server">
		
 </div>
</div>

<div id="left" >
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      
      <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Arun Sunny</td>
                       </tr>
                      <%-- <tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Class</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           BDS</td>
                       
                       <td class="attributeValue">Admission No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td></td>
                       </tr>
                       <tr>
                       <td class="attributeValue">Class No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       </tr>
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
       
       <%-- <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>--%>
      
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no" style="height: 36px"> </td>
				<td class="n" style="height: 36px">Previous Class Performance</td>
				<td class="ne" style="height: 36px"> </td>
			</tr> 
			<tr >
				<td class="o"> </td>
				<td class="c" >
					  <br />
				<table class="tablelist">
				<tr>
				<td class="leftside" valign="bottom">Select Class</td>
				<td class="rightside" valign="bottom"> 
                    <asp:DropDownList ID="drp_prevClass" runat="server" AutoPostBack="true" Width="162px" OnSelectedIndexChanged="drp_prevClass_change">
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    
                <asp:ImageButton ID="Img_Export" runat="server" Height="30px" 
                        ImageUrl="~/Pics/Excel-icon.png"  Width="30px"  OnClick="Img_Excel_Click"
                        ToolTip="Export to Excel" />
                </td>
				</tr>
				
				<tr>
				<td class="leftside">
				<asp:Label runat="server" ID="lbl_Err" ForeColor="Red" ></asp:Label>
				</td>
				<td class="rightside">
				
                        
                </td>
				</tr>
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
	
<%--	</ContentTemplate>
	</asp:UpdatePanel>  --%>
        
        
  <br />
  <br />  
</div>

<div class="clear"></div>
</div>
</asp:Content>

