<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="StaffWorkLoadReport.aspx.cs" Inherits="WinEr.StaffWorkLoadReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <style type="text/css">
  .HeadStaffStyle
  {
      width:20%;
      border:solid 1px gray;
      background-image:url(images/dockbar.png);
      background-repeat:repeat;
      font-weight:bold;
      color:Black
  }
 
  .HeadProgressBar
  {
      width:25%;
      border:solid 1px gray;
      background-image:url(images/dockbar.png);
      background-repeat:repeat;
      font-weight:bold;
      color:Black
  }
  .StaffStyle
  {
      width:20%;
      border-bottom:solid 1px gray;
      border-right:none;
      font-weight:bold;
  }
 
  .ProgressBar
  {
      width:25%;
      border-bottom:solid 1px gray;
      border-left:none;
  }
  
  .BorderStyle
  {
      border:solid 2px gray;
  }
  
  .OverOccupied
{
    background-color:Red;
    border-style:ridge;
    border-color:White;
    border-width:2px;
    -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
}

.FullOccupied
{
    background-color:green;
    border-style:ridge;
    border-color:White;
    border-width:2px;
    -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
}

.HalfOccupied
{
    background-color:Orange;
    border-style:solid;
    border-color:White;
    border-width:1px;
    -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
}


.EmptyStyle
{
    background-color:White;
}

.OutStyle
{
    background-color:Orange;
}


.ProgressStyle
{
    padding:1%;
    border-style:ridge;
    border-color:Gray;
    border-width:1px;
    -moz-border-radius: 12px;
    -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
     border-radius: 12px;
}
.CountStyle
{
    padding-top:8px;
    text-align:center;
    font-weight:bold;
    width:50px;
    height:25px;
    background-image:url(images/dockbar.png);
      -moz-border-radius: 12px;
    -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
     border-radius: 12px;
}
 </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
            
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
         

   <asp:panel ID="Panel2"  runat="server"> 
    
    <div class="container skin1" >
	<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Staff Work Load Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			     
			     <div style="padding-left:200px;">
			     <table width="100%">
			      <tr>
			       <td>
			        Sort By


			        
                       <asp:DropDownList ID="Drp_SortType" runat="server" AutoPostBack="true" class="form-control"
                           onselectedindexchanged="Drp_SortType_SelectedIndexChanged">
                        <asp:ListItem Text="Select Order" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Name" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Heighest Work Load" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Lowest Work Load" Value="2"></asp:ListItem>
                       </asp:DropDownList>
			       </td> 
			       
			       <td>
			        
			        <asp:ImageButton ID="Btn_Export" runat="server"  ImageUrl="~/Pics/Excel.png"
             OnClick="Btn_Export_Click" Width="40px" ToolTip="Export to Excel" /> 
			       
			       </td>
			      </tr>
			      <tr>
			       <td colspan="2">
			       
			        <div style="height:400px;overflow:auto;">
			        <br />
                        <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red"></asp:Label>
			         <div id="DivStaff" runat="server">
			     
			         <%--<table width="100%" cellspacing="10">
			      <tr>
			      <td>
			       
			        <table width="100%" cellspacing="0">
					<tr class="BorderStyle">
					 <td class="HeadStaffStyle">
					   Staff Name
					 </td>
					 <td class="HeadProgressBar">
					    Work Load Percent
					 </td>
					 <td style="width:5%;" align="center" valign="middle" >
					 </td>
					 <td>
					 
					 </td>
					</tr>	
			      </table>
			      
			      </td>
			      			      
			      </tr>
			      <tr>

			       <td>
			         <table width="100%" cellspacing="0">
					<tr class="BorderStyle">
					 <td class="StaffStyle">
					   Kumari
					 </td>
					 <td class="ProgressBar">
					     <div class="ProgressStyle" >
					     <table width="100%"  cellspacing="0"  title="34">
                           <tr>
                             <td class="OverOccupied" style="width:80%">
                                 &nbsp;
                             </td>
                             <td  class="OutStyle" style="width:20%">
                             
                             </td>
                           </tr>
                          </table>
					     </div>
					 </td>
					 <td style="width:5%;" align="center" valign="middle" >
					  <div class="CountStyle">
					    34/28
					  </div>
					 
					 
					 </td>
					 <td>
					 
					 </td>
					</tr>	
			      </table>
			       </td>
			      </tr>
			       <tr>
			       <td>
			       
			       <table width="100%" cellspacing="0">
					<tr class="BorderStyle">
					 <td class="StaffStyle">
					   Kumari
					 </td>
					 <td class="ProgressBar">
					     <div class="ProgressStyle" >
					     <table width="100%"  cellspacing="0" title="28">
                           <tr>
                             <td class="FullOccupied" style="width:100%">
                                 &nbsp;
                             </td>

                           </tr>
                          </table>
					     </div>
					 </td>
					 <td style="width:5%;" align="center" >
					  <div class="CountStyle">
					    28/28
					  </div>
					 
					 
					 </td>
					 <td></td>
					</tr>	
			      </table>
			       
			       
			       </td>
			      </tr>
			      
				  
						
		        </table>--%>
		       
		          </div>
			        
			        </div>
			       
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
          
    </asp:panel> 
         


    </ContentTemplate>
    <Triggers >
     <asp:PostBackTrigger ControlID="Btn_Export" />
    </Triggers>
 </asp:UpdatePanel>
<div class="clear"></div>
  </div>
</asp:Content>
