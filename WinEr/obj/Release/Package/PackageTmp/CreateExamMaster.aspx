<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateExamMaster.aspx.cs" Inherits="WinEr.CreateExamMaster" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager2" runat="server" />
<div id="contents">    

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
                <div class="container skin1" >
		            <table cellpadding="0" cellspacing="0" class="containerTable">
			            <tr >
				            <td class="no"> </td>
				            <td class="n">Create Exam</td>
				            <td class="ne"> </td>
			            </tr>
			            <tr >
				            <td class="o"> </td>
				            <td class="c" >
				           
				            <table class="tablelist" >
				                <tr>
				                    
				                    <td class="leftside">
                                        &nbsp;</td>
				                    <td class="rightside">
                                        &nbsp;</td>
				                   </tr>
				                    <tr>
                                        <td class="leftside">
                                            <asp:Label ID="Label1" runat="server" Text="Exam Name " class="control-label"></asp:Label>
                                        </td>
                                        <td class="rightside">
                                            <asp:TextBox ID="Txt_ExamName" runat="server"  MaxLength="25" ValidationGroup = "Exam" class="form-control"
                                                Width="180px"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtenderWFName" 
                                                runat="server" Enabled="True" FilterMode="InvalidChars" 
                                                InvalidChars="'/\~`!@#$%^&amp;*()-=+{}[]|;:&gt;&lt;,.?" 
                                                TargetControlID="Txt_ExamName" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Exam" ControlToValidate="Txt_ExamName" ErrorMessage="Enter Exam Name"></asp:RequiredFieldValidator>

                                        </td>
                                </tr>
				                    <tr>
				                    <td class="leftside">
                                        <asp:Label ID="Label2" runat="server" Text="Exam Type " class="control-label"></asp:Label>
				                    </td>
				                    <td class="rightside">
                                        <asp:DropDownList ID="Drp_ExamType" runat="server" Width="180px" class="form-control">
                                        </asp:DropDownList>
				                    </td>
				                    </tr>
				                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				               
				                <tr>
				                   
				                    <td class="leftside">
				                        <asp:Label ID="Label3" runat="server" Text="Frequency Type " class="control-label"></asp:Label>
				                    </td>
				                    <td class="rightside">
                                        <asp:DropDownList ID="Drp_PeriodType" runat="server" Width="180px" class="form-control">
                                        </asp:DropDownList>
				                    </td>
				                    	                    
				                </tr>
				                
				                <tr>
                                    <td class="leftside">
                                        &nbsp;</td>
                                    <td class="rightside">
                                        &nbsp;</td>
                                </tr>
				                
				                <tr>
				                    <td class="leftside"></td>
				                    <td class="rightside">
                                        <asp:Button ID="Btn_Create" runat="server" Text="Create" Class="btn btn-success"  OnClick="Btn_Create_Click" ValidationGroup = "Exam" />&nbsp;&nbsp;
                                        <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" OnClick="Btn_Cancel_Click" />
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
            <WC:MSGBOX id="WC_MessageBox" runat="server" />  
            
                <%--<asp:Button runat="server" ID="Button1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Panel1" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:200px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png" 
                        Height="28px" Width="29px" />
             </td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button2" runat="server" Text="OK" Width="50px"/>
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
             </asp:Panel> --%>
         
            </ContentTemplate>
        </asp:UpdatePanel>


    <div class="clear"></div>
</div>

</asp:Content>
