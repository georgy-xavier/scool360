<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SurveyAnswer.aspx.cs" Inherits="Scool360student.SurveyAnswer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
    .clickme {
    background-color: #EEEEEE;
    padding: 8px 40px;
    text-decoration:none;
    font-weight:bold;
    border-radius:10px;
    cursor:pointer;
}

</style>    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="contents">
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

  <%-- <div align="right">
  
  
  <p style="position:fixed;"
                         
                         
  <asp:LinkButton ID="Btn_lnk" 
            runat="server" 
                
            OnClick="Btn_link_Click">
    <span aria-hidden="true" class="glyphicon glyphicon-question-sign" style="font-size:20px;padding-right:30px;"></span>
</asp:LinkButton>
                         
      
          
          </p></div>--%>
     
  <asp:Panel ID="pnl" runat="server" DefaultButton="Btn_CreateClass"> 
   
   <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Survey</td>
				<td class="ne"> </td>
			</tr>
           <tr>
               <td></td>
<td></td>
               <td></td>
           </tr>
            

			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				 <table style="width:100%; text-align:center;" class="tablelist">
        <tr>
            <td colspan="2">         &nbsp;</td>
          
        </tr>

        <tr>
            
            <td   class="leftside">
               <asp:Label ID="Label2" runat="server" style="font-weight:bold;font-size:medium" Text="Label">Select Survey</asp:Label> &nbsp;&nbsp;&nbsp; :</td>
            <td class="rightside">
                <asp:DropDownList ID="Drp_Stand" runat="server" class="form-control" Height="35px" Width="250px" OnSelectedIndexChanged="Drp_Stand_SelectedIndexChanged" AutoPostBack="True" >

                </asp:DropDownList>
            </td>
      </tr>
                      <tr>
            
            <td   class="leftside">
               <asp:Label ID="Label5" runat="server" style="font-weight:bold;font-size:medium" Text="Label">Select Question</asp:Label> &nbsp;&nbsp;&nbsp; :</td>
            <td class="rightside">
                <asp:DropDownList ID="DropDownList1" runat="server" class="form-control" Height="35px" Width="250px"   OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True" >

                </asp:DropDownList>
            </td>
      </tr>
                      <tr>
            
            <td   class="leftside">
                 <asp:Label ID="Label3" runat="server" style="font-weight:bold;font-size:medium" Text="Label">Survey Question </asp:Label> &nbsp;&nbsp;&nbsp; :</td>
             
            <td class="rightside">
                <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
            </td>
      </tr>
                           
   
      
      
        
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
         <tr>
            <td colspan="2">         &nbsp;</td>
          
        </tr>
      </table>
      <asp:Panel ID="Pnl_subject" runat="server">
     
      <table width="100%">
        <tr>
            <td class="style14">
                &nbsp;</td>
            <td class="style15">
                &nbsp;</td>
            <td class="style24" align="left" style="padding-left:350px" >
                 <asp:Label ID="Label4" runat="server" style="font-weight:bold;font-size:medium" Text="Label">Answer  </asp:Label>:</td>
            <td class="style24">
                &nbsp;</td>
            
        </tr>
          <tr>
            <td class="style14">
                &nbsp;</td>
            <td class="style15">
                &nbsp;</td>
            <td class="style24" align="left" style="padding-left:350px" >
                 <asp:Label ID="Label6" runat="server" style="font-weight:bold;font-size:medium" Text="Label" Visible="False"></asp:Label>
                 <asp:Label ID="Label7" runat="server" style="font-weight:bold;font-size:medium" Text="Label" Visible="False"></asp:Label></td>
            </td>
            <td class="style24">
                &nbsp;</td>
            
        </tr>
        <tr>
            <td class="style25">
                &nbsp;</td>
            <td class="style35">
                &nbsp;</td>
            <td class="style10" align="center">
                <div  style="OVERFLOW: auto; WIDTH: 350px; HEIGHT: 100px;  id="checkboxdiv" >
                    <asp:CheckBoxList ID="ChkBox_AllsSub" runat="server" Font-Bold="False" 
                        Font-Size="Small" ForeColor="Black" Width="188px">
                    </asp:CheckBoxList>
                   
                  <asp:TextBox ID="TextBox2" runat="server"  WIDTH="250px"  HEIGHT="80px"></asp:TextBox>
                   
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" Font-Bold="False" 
                        Font-Size="Small" ForeColor="Black" Width="188px"></asp:RadioButtonList>
                </div>
                

            </td>
            
            
        </tr>
        

        <tr>
            <td class="style14">
                &nbsp;</td>
            <td class="style15">
                &nbsp;</td>
            <td class="style3" colspan="3">
                &nbsp;</td>
        </tr>
        
        </table>
       
        </asp:Panel>
        <table width="100%">
        <tr>
        
            <td >
                &nbsp;</td>
            <td  align="center">
                <asp:Button ID="Btn_CreateClass" runat="server" ValidationGroup="Save"
                      Text="Save" 
                  Class="btn btn-success" OnClick="Btn_CreateClass_Click"  />
                &nbsp;&nbsp;   
                <asp:Button ID="But_Reset" runat="server"  
                    Text="Reset" Class="btn btn-danger" OnClick="But_Reset_Click" />
                &nbsp;&nbsp;&nbsp;
           
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
   
 </asp:Panel>    

<asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-info" Text="OK" Width="50px"/>
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


</ContentTemplate>
                    </asp:UpdatePanel>
                    
<div class="clear">
    
    </div>
    
</div>
</asp:Content>


