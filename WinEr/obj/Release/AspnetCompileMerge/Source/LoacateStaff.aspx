<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="LoacateStaff.aspx.cs" Inherits="WinEr.LoacateStaff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
 .OtherClassStyle
 {
     width:25%;
     background: #e9e9e9;
     border:solid 1px gray;
   -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
 }
 
 .FreeClassStyle
 {
     width:25%;
     background: #ffc891;
      border:solid 1px gray;
   -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
 }
 
 .CurrentClassStyle
 {
     width:25%;
     background: #a6ff88;
      border:solid 1px gray;
     -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
 }
 
 .NextClassStyle
 {
     width:25%;
     background: #e9e9e9;
      border:solid 1px gray;
    -moz-border-radius: 12px;
   -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
    border-radius: 12px;
 }
 
 .RightStyle
 {
     font-weight:bold;
 }

  .ClassHeadingStyle
  {
      font-weight:bolder;
      font-size:15px;
      color:Black;
  }
  
   .MyTable
   {
            width:100%;
            
            border: thin solid #333333;
   }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


            
            


<div id="contents" style="min-width:950px;">
<div id="right">

<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>

</div>


<div id="left">

<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />

         
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
                           Sachin tendulkar</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
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
    
    <div class="container skin1"  >
	<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Locate Staff</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			     
			     
			      <br />
			     <div id="StaffDetails" runat="server">
                      <%--<table width="100%" cellspacing="10">  <tr>  <td class="OtherClassStyle">   <table width="100%"  cellspacing="5"> <tr>  <td colspan="4" align="center" class="ClassHeadingStyle">     P2 10-11AM    </td>    </tr>  <tr>    <td align="right">   Class:    </td>   <td class="RightStyle">   UKG B  </td>   <td align="right">   Subject:  </td>   <td class="RightStyle">  English </td> </tr> <tr>  <td align="right"> From:   </td>   <td class="RightStyle">  09   </td>  <td align="right">    To:    </td>   <td class="RightStyle">    10  </td>  </tr>  </table>  </td>  <td class="FreeClassStyle"> <table width="100%"  cellspacing="5"> <tr> <td  align="center" class="ClassHeadingStyle">   Free Period  </td>  </tr> </table> </td>  <td class="CurrentClassStyle">   </td>  <td class="NextClassStyle"> </td>   </tr> </table>--%>
                   </div>
			      
                    <asp:Label ID="Lbl_Error" runat="server" Text="" ForeColor="Red"></asp:Label>
			  
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
    
<div class="clear"></div>
     
</div>





</asp:Content>
