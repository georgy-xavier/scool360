<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeTableView.aspx.cs"  Inherits="WinEr.TimeTableView" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="WC" TagName="TimeTableControl" Src="~/WebControls/TimetableControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

       <title>School</title>
 
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
     <style type="text/css">
             body{
         width:1000px;
         margin:0 auto;
         position:relative;
         background :#FFF;
         margin-top:5px;
         border-style:outset;
         border-width:thin;
         border-color:Gray;
        }
     </style>
</head>
<body>
    <form id="form1" runat="server" >
    <div>
        <ajaxToolkit:ToolkitScriptManager Id="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  

                   <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate>

          <asp:panel ID="Panel2"  runat="server"> 
    
            <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Time Table View of 
				
                    <asp:Label ID="Lbl_Class" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                    
                    </td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<br />
                    
                    
          
                   <WC:TimeTableControl id="WC_TimeTableControl" runat="server" />  
                  
							                 
                  <br />
                   
                  
   
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
                                         <span style="color:Black">Message</span>
                                        </td>
                                        <td align="right">
                                          
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/cross.png" Width="20px" OnClientClick="window.close()" />
                                        
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
                                               
                                                 <asp:Label ID="Label3" runat="server" Text="Error in retriving class details. Pelase try later" ></asp:Label>
                                               
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




       </ContentTemplate>
       </asp:UpdatePanel>          
                            
</div>
    </form>
</body>
</html>
