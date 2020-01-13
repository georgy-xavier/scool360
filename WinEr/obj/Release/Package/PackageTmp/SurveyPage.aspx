<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SurveyPage.aspx.cs" Inherits="WinEr.SurveyPage" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" >
         function SelectAllEdit(cbSelectAll) {
             var gridViewCtl = document.getElementById('<%=Grid_edit.ClientID%>');
             var Status = cbSelectAll.checked;
             for (var i = 1; i < gridViewCtl.rows.length; i++) {

                 var cb = gridViewCtl.rows[i].cells[0].children[0];
                 cb.checked = Status;
             }
         }
        function DeleteConfirmation()
        {
            if (confirm("Are you sure,you want to delete selected record ?")==true)
            return true;
            else
            return false;
        }
         </script>
    <style type="text/css">

         /*.overflowTest 
         {
            padding: 15px;
            overflow-x: scroll;
        overflow-y: hidden;
}*/

        
      
    </style>
 
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
				            <td class="n">
                                <asp:img src=<img src="images/SurveyLogo.png" width="30" height="30"/>Survey</td>
				            <td class="ne"> </td>
			            </tr>
			            <tr >
				            <td class="o"> </td>
				            <td class="c" >
				           
				            <table class="tablelist" >
                                 <tr>     
                                       <td  colspan="2" align="right"> 
                                        <asp:Image ID="Img_AddUser" ImageUrl="Pics/add.png" Width="25px" Height="20px" runat="server" style="vertical-align:top" />
                                        <asp:LinkButton ID="Lnk_AddSurvey" runat="server" CssClass="grayadd" 
                                        Height="22px" OnClick="Lnk_AddSurvey_Click">Add Survey</asp:LinkButton>
                                        <asp:Image ID="Img_EditUser"  ImageUrl="~/Pics/edit.png" Width="25px" Height="20px" runat="server" style="vertical-align:top" />
                                        <asp:LinkButton ID="Lnk_EditSurvey" runat="server" CssClass="grayadd" 
                                        Height="22px" OnClick="Lnk_EditSurvey_Click" >Edit Survey</asp:LinkButton>    
                                        </td>
                                </tr>
				                <tr>
				                    
				                    <td class="leftside">
                                        &nbsp;</td>
				                    <td class="rightside">
                                        &nbsp;</td>
				                   </tr>
                                <tr>
                                    <td class="leftside">
                                    <asp:Label ID="lbl_Surveyname" runat="server" Text="Survey Name" Visible="false" class ="control-label" ></asp:Label>
                                        </td>
                                    <td class="rightside">
                                        <asp:DropDownList ID="Drp_Surveyname" runat="server" Visible="false" Width="180px" AutoPostBack="true" class="form-control"></asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
				                    
				                    <td class="leftside">
                                        &nbsp;</td>
				                    <td class="rightside">
                                        &nbsp;</td>
				                   </tr>
				                   
				                    <tr>
				                    <td class="leftside">
                                         <asp:Label ID="group_all" runat="server" Text="Group All : " class="control-label"></asp:Label>
                                        <asp:Label ID="group_select" runat="server" Text="Group" Visible ="false" class="control-label"></asp:Label>
				                    </td>
				                    <td class="rightside">
                                        <asp:DropDownList ID="Drp_Groupall" runat="server" Width="180px" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="Drp_Groupall_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="Drp_Group" Visible="false" runat="server" AutoPostBack="true" Width="180px" class="form-control" OnSelectedIndexChanged="Drp_Group_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Drp_Group" ErrorMessage="Please select the Group" ForeColor="Red" Display="Dynamic" />
               
				                    </td>
				                    </tr>
                                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                <tr>
				                   <td class="leftside">
				                        <asp:Label ID="lbl_ques" runat="server" Text="Question" Visible="false" class="control-label"></asp:Label>
				                    </td>
				                    <td class="rightside">
                                        <asp:TextBox ID="Ques" runat="server" Visible="false"  TextMode="MultiLine" Rows="3" Width="250px" class="form-control" MaxLength="150"></asp:TextBox>
				                    </td>
				                    	                    
				                </tr>
                               
                                <tr>
                                 <td class="leftside"><br /></td>
                                 <td class="rightside"><br /></td>
                               </tr>
				                
                                 <tr>
				                    <td class="leftside">
                                        <asp:Label ID="lbl_Qtype" runat="server" Visible="false" Text="Question Type" class="control-label"></asp:Label>
				                    </td>
				                    <td class="rightside">
                                        <asp:DropDownList ID="Drp_Qtype" Visible="false" runat="server" Width="180px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_Qtype_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="TextBox"></asp:ListItem>
                                             <asp:ListItem Value="2" Text="CheckBox"></asp:ListItem>
                                             <asp:ListItem Value="3" Text="RadioButton"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="ValidateDrp_Qtype" runat="server" ControlToValidate="Drp_Qtype" ErrorMessage="Please select an item" ForeColor="Red" Display="Dynamic" />
               
				                    </td>
				                    </tr>
                                 <tr>
                                 <td class="leftside"><br />
                                      <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
                                        <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

                                 </td>
                                 <td class="rightside"><br /></td>
                               </tr>
                                
                                <tr>
                                    <td class="leftside"><asp:Label ID="Lbl_option" runat="server" Text="Options" Visible="false" class="control-label"></asp:Label></td>
				                    <td class="rightside">                                        
                                        <asp:TextBox ID="Txt_answer" runat="server" Visible="false" AutoCompleteType="Disabled" Width="180px" class="form-control" autocomplete="off" MaxLength="150"></asp:TextBox>
                                        <asp:Button ID="Btn_answer" runat="server" Text="Add Answer" Visible="false" Class="btn btn-success" OnClick="Btn_answer_Click"/>
                                        <%--<asp:Button ID="Btn_View" runat="server" Text="View" OnClick="Btn_View_Click" Visible="false" Class="btn btn-success"/>--%>
                                   </td>
                                </tr>
                               
				               
				                
				                <tr>
                                    <td class="leftside">
                                        &nbsp;</td>
                                    <td class="rightside">
                                        &nbsp;</td>
                                </tr>
                                 <tr>
				                    <td class="leftside">&nbsp;</td>
				                    <td class="rightside">
                                        <asp:Button ID="Btn_Add" runat="server" Text="Add" Visible="false" Class="btn btn-success" OnClick="Btn_Add_Click"/>&nbsp;&nbsp;
                                        <asp:Button ID="Btn_Update" runat="server" Text="Update" Visible="false" Class="btn btn-primary" OnClick="Btn_Update_Click"  />
                                         <asp:HiddenField ID="GroupMapId" runat="server" />&nbsp;&nbsp;
				                    </td>
				                </tr>
                                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        
                         <%-- <div style="text-align:center">
                        <asp:Label ID="Lbl_ErrStaffMap" runat="server" ForeColor="Red"></asp:Label>
                              </div>--%>
                        
                     

                     <tr>
                         
                                 
                  <asp:GridView ID="Grd_Survey" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" AllowPaging="true" PageSize="10" OnPageIndexChanging="Grd_Survey_PageIndexChanging" CellPadding="3" CellSpacing="2" Font-Size="15px"  
                   Width="100%">
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                             
                             <%-- <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>--%>
                              <asp:BoundField DataField="Id" HeaderText="ID" />
                              <asp:BoundField DataField="Group_id" HeaderText="Group ID"  />
                               <asp:BoundField DataField="Survey_Name" HeaderText="Survey Name"  />
                               <asp:BoundField DataField="Group_name" HeaderText="Group Name"  />
                                <asp:BoundField DataField="Question" HeaderText="Question" />
                                <asp:BoundField DataField="Ques_type" HeaderText="Question Type" />
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                      
              </asp:GridView>
                                      
                                </div>    
                                </tr>
                   <%--             <tr>
                                 
                  <asp:GridView ID="Grid_edit" runat="server" Visible="false" AutoGenerateColumns="false" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" AllowPaging="true" PageSize="10" CellPadding="3" CellSpacing="2" Font-Size="15px"  
                   Width="100%" OnPageIndexChanging="Grid_edit_PageIndexChanging" >
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                             
                             <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>                             
                             <asp:CheckBox runat="server" ID="chk_select"/>
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAllEdit(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
                              <asp:BoundField DataField="Id" HeaderText="ID" />
                              <asp:BoundField DataField="Group_id" HeaderText="Group ID"  />
                               <asp:BoundField DataField="Group_name" HeaderText="Group Name"  />
                                <asp:BoundField DataField="Question" HeaderText="Question"  />
                                <asp:BoundField DataField="Ques_type" HeaderText="Question Type" />
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                      
              </asp:GridView>
                                      
                                    
                                </tr>--%>
                                 
                                <tr>
                                    

                                                    <asp:GridView ID="Grid_edit" runat="server"
                                                        AutoGenerateColumns="false" BackColor="#EBEBEB"
                                                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"
                                                        CellPadding="3" CellSpacing="2" Font-Size="15px" AllowPaging="True"
                                                        PageSize="10" Width="100%" Visible="false" OnSelectedIndexChanged="Grid_edit_SelectedIndexChanged" OnPageIndexChanging="Grid_edit_PageIndexChanging" OnRowDeleting="Grid_edit_RowDeleting">

                                                        <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                        <EditRowStyle Font-Size="Medium" />
                                                        <Columns>
                                                               <asp:BoundField DataField="Id" HeaderText="ID" />
                                                               <asp:BoundField DataField="Group_id" HeaderText="Group ID"  />
                                                               <asp:BoundField DataField="Survey_Name" HeaderText="Survey Name"  />
                                                               <asp:BoundField DataField="Group_name" HeaderText="Group Name"  />
                                                               <asp:BoundField DataField="Question" HeaderText="Question"/>
                                                               <asp:BoundField DataField="Ques_type" HeaderText="Question Type" />
                                                            <asp:CommandField ItemStyle-Width="35" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="100px"
                                                                ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center"
                                                                SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
                                                                ShowSelectButton="True">
                                                                <ControlStyle />
                                                                <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton1" OnClientClick="return DeleteConfirmation()" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ControlStyle ForeColor="#FF3300" />
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                            HorizontalAlign="Left" />
                                                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                            HorizontalAlign="Left" />
                                                    </asp:GridView>
                                        
                                </tr>
				            </table>
                                <div style="text-align:center">
                        <asp:Label ID="Lbl_ErrStaffMap" runat="server" ForeColor="Red"></asp:Label>
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
                
<%--<div id="divPopUp" class="containerTable" style="border:dotted;border-color:aquamarine;background-color:antiquewhite;width:20%;height:20%" >
     <div id="Header" class="header" >MyHeader</div>
     <div id="main" class="main">Main PopUp </div>
     <div id="buttons">
          <div id="DivbtnOK" class="buttonOK"><asp:Button id="btnOk" runat="server" text="Ok"/></div>
          <div id="Divbtncancel" class="buttonOK"><asp:Button id="btnCancel" runat="server" text="Cancel"/></div>
     </div>
</div>--%>
               <%-- <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp" BackgroundCssClass="Background"></ajaxToolkit:ModalPopupExtender>
                <div id="divPopUp" class="Popup">
                <div>
                <span class="close"> <asp:ImageButton ID="ImageButton1" src="images/img_116511.ico"  height="20" width="20"  runat="server" OnClick="ImageButton1_Click" /></span>
                </div>
                <h4 align="center"> Hello World </h4>
                <div class="Design">                
                <table align="center">
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </table>              
                </div>
                </div>--%>

            <WC:MSGBOX id="WC_MessageBox" runat="server" />  

                 </ContentTemplate>
        </asp:UpdatePanel>


    <div class="clear"></div>
</div>

</asp:Content>
