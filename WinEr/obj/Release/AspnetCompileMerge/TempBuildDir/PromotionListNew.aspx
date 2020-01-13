<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="PromotionListNew.aspx.cs" Inherits="WinEr.PromotionListNew" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        </style>
    
    <style type="text/css">
        #Text2
        {
            width: 126px;
            height: 24px;
        }
    .style1
    {
        width: 100%;
    }
    #Div2
    {
        height: 89px;
    }
     .newtable
    {
        width: 878px;
    }
    .textarea
    {
        border-style:double double double none;
         border-color: #999966; 
         border-width:thick;
         text-align: center;
        
        
    }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
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


             
                
        <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Promotion Manager</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" ><br/>
                
                <asp:Panel ID="Pnl_All" runat="server" style="min-height:350px">
                
                <ajaxToolkit:TabContainer runat="server" ID="Tabs" Width="100%" 
                          CssClass="ajax__tab_yuitabview-theme">
				
				     <ajaxToolkit:TabPanel runat="server" ID="Tab_Details" HeaderText="Signature and Bio"><HeaderTemplate><asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/evolution-tasks.png" /><b>PROMOTION LIST</b></HeaderTemplate><ContentTemplate><asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1"><ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate></asp:UpdateProgress><asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate><table class="tablelist"><tr><td colspan="2">&nbsp;</td></tr><tr><td class="leftside">Select Batch:</td><td class="rightside" ><asp:DropDownList ID="Drp_SelectType" class="form-control" runat="server" Width="220px" 
                                AutoPostBack="True" onselectedindexchanged="Drp_SelectType_SelectedIndexChanged" ></asp:DropDownList></td></tr><tr><td class="leftside">Select Class:</td><td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server"  Width="220px" class="form-control"
                                AutoPostBack="True" 
                                onselectedindexchanged="Drp_Class_SelectedIndexChanged" ></asp:DropDownList>&nbsp;&nbsp;&nbsp; <asp:Button ID="Btn_GenerateList" runat="server" Text="Generate" 
                                onclick="Btn_GenerateList_Click" class="btn btn-info"/></td></tr><tr><td colspan="2">&#160;</td></tr><tr><td colspan="2" align="center"><asp:Label ID="Lbl_note" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></td></tr></table><div id="Div1" runat="server" class="linestyle"></div><asp:Panel ID="Pnl_StudentsList" runat="server"><%--
                      <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">--%>
		
		<table width="100%">
		<tr><td align="right"><asp:Button ID="Btn_Update" runat="server" class="btn btn-info" Text="Update" 
                    onclick="Btn_Update_Click" />
                    <asp:Button ID="Btn_ClearPromotion" runat="server" 
                class="btn btn-info" Text="Clear List" onclick="Btn_ClearPromotion_Click"  />
                    <asp:Button ID="Btn_PromotionExcel" class="btn btn-info" Width="120px" 
                    runat="server" Text="Promotion List" onclick="Btn_PromotionExcel_Click" /></td></tr>
                    <tr><td><div id="Div2" class="linestyle" runat="server"></div></td></tr>
                    <tr><td ><div>
                    <asp:GridView ID="Grd_StudentsList" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                             Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px"     onselectedindexchanged="Grd_StudentsList_SelectedIndexChanged"><Columns><asp:BoundField DataField="Id" HeaderText="Id" /><asp:BoundField DataField="StudentId" HeaderText="StudentId" /><asp:BoundField DataField="StudentName" HeaderText="StudentName" 
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"/><asp:BoundField DataField="RollNo" HeaderText="Roll No" 
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/><asp:BoundField DataField="AdmissionNo" HeaderText="Admission No" 
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"/><asp:BoundField DataField="Remarks" HeaderText="Remarks" /><asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Left" 
                                    HeaderStyle-HorizontalAlign="Center"><ItemTemplate><div style="width:200px;overflow:auto"><table><tr><td><%# Eval("Remarks")%></td></tr></table></div></ItemTemplate></asp:TemplateField><asp:BoundField DataField="Link" HeaderText="Link" /><asp:TemplateField HeaderText="To Class" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"><ItemTemplate ><asp:DropDownList ID="Drp_ToClass" runat="server" Width="140px"></asp:DropDownList></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Edit"><ItemTemplate><asp:LinkButton ID="Lnk_Edit"  CommandName="Select" CommandArgument="Select" runat="server" Text='<%# Eval("Link")%>'></asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" /></asp:TemplateField><asp:BoundField DataField="IsEligible" HeaderText="IsEligible" /><asp:BoundField DataField="ToClassId" HeaderText="ToClassId" /></Columns><PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" /><FooterStyle BackColor="#bfbfbf" ForeColor="Black" /><EditRowStyle Font-Size="Medium" /><SelectedRowStyle BackColor="White" ForeColor="Black" /><PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" /><HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" /><RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" /></asp:GridView></div></td></tr></table>
   </asp:Panel></ContentTemplate><Triggers ><asp:PostBackTrigger ControlID="Btn_PromotionExcel"/></Triggers>
   </asp:UpdatePanel></ContentTemplate></ajaxToolkit:TabPanel>
                
                 <ajaxToolkit:TabPanel runat="server" ID="Tab_Details3" HeaderText="Signature and Bio"><HeaderTemplate><asp:Image ID="Image333" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/Details.png" /><b>PROMOTION PREVIEW</b></HeaderTemplate><ContentTemplate><asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel3"><ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate></asp:UpdateProgress><asp:UpdatePanel ID="UpdatePanel3" runat="server"><ContentTemplate><%--<div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">--%><table width="100%" class="tablelist"><tr><td colspan="2">&nbsp;</td></tr><tr><td class="leftside">Select Batch:</td><td class="rightside" ><asp:DropDownList ID="Drp_SelectBatchType" class="form-control" runat="server"  Width="180px" ></asp:DropDownList></td></tr><tr><td class="leftside">Select Class:</td><td class="rightside"><asp:DropDownList ID="Drp_PreviewClass" runat="server" class="form-control" Width="180px"></asp:DropDownList>
		&nbsp;<asp:Button ID="Btn_Preview" runat="server" Text="Show" class="btn btn-info" 
                    onclick="Btn_Preview_Click" />
                    &nbsp;<asp:Button ID="Btn_PreviewExcel" runat="server" Text="Export to Excel"  
                    class="btn btn-info" Width="140px" onclick="Btn_PreviewExcel_Click"/></td></tr><tr><td colspan="2"><div id="Div4" class="linestyle" runat="server"></div></td></tr><tr><td colspan="2" align="center"><asp:Label ID="lbl_PreviewNote" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></td></tr><tr><td colspan="2"><div><asp:GridView ID="Grd_Preview" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                             Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" ><Columns><asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-Width="120px"/><asp:BoundField DataField="AdmissionNo" HeaderText="AdmissionNo"  HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"/><asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="200px"/><asp:BoundField DataField="Sex" HeaderText="Sex" ItemStyle-Width="60px" /><asp:BoundField DataField="DOB" HeaderText="DOB" ItemStyle-Width="60px"/></Columns><PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" /><FooterStyle BackColor="#bfbfbf" ForeColor="Black" /><EditRowStyle Font-Size="Medium" /><SelectedRowStyle BackColor="White" ForeColor="Black" /><PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" /><HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" /><RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" /></asp:GridView></div></td></tr></table></ContentTemplate><Triggers ><asp:PostBackTrigger ControlID="Btn_PreviewExcel"/></Triggers></asp:UpdatePanel></ContentTemplate></ajaxToolkit:TabPanel>
                    
                     <ajaxToolkit:TabPanel runat="server" ID="Tab_Details2"
                      HeaderText="Signature and Bio"><HeaderTemplate><asp:Image
                       ID="Image3333333" runat="server" Width="20px" Height="18px" 
                       ImageUrl="~/Pics/page_process.png" /><b>PROMOTION</b></HeaderTemplate>
                       <ContentTemplate><asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel2"><ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate></asp:UpdateProgress><asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate><asp:Panel ID="Pnl_PromotionStatus" runat="server"><%-- <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">--%><table width="100%" class="tablelist"><tr><td colspan="2">&nbsp;</td></tr><tr><td class="leftside">Select Batch:</td><td class="rightside" ><asp:DropDownList ID="Drp_SelectType2" runat="server" class="form-control" AutoPostBack="true" 
               Width="220px" onselectedindexchanged="Drp_SelectType2_SelectedIndexChanged" ></asp:DropDownList></td></tr><tr><%-- <td style="width:90%" align="center"> 
		    <asp:Label ID="lbl_PromotionNote" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></td>--%><td align="right" colspan="2"><asp:Button ID="Btn_Promotion" runat="server" Text="Promote" class="btn btn-info"
                    onclick="Btn_Promotion_Click" /></td></tr><tr><td colspan="2"><div id="Div3" class="linestyle" runat="server"></div></td></tr><tr><td colspan="2"><div><asp:GridView ID="Grd_PromotionStatus" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                             Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px"  ><Columns><asp:TemplateField HeaderText="Select" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"><ItemTemplate ><asp:CheckBox ID="Chk_Select" runat="server"/></ItemTemplate></asp:TemplateField><asp:BoundField DataField="ClassName" HeaderText="Class Name" ItemStyle-Width="120px"/><asp:BoundField DataField="Status" HeaderText="Status"  HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="170px"/><asp:BoundField DataField="PendingStudents" HeaderText="Pending Students" ItemStyle-Width="120px"/><asp:BoundField DataField="PromotionCount" HeaderText="Students in Promotion-List" ItemStyle-Width="120px" /><asp:BoundField DataField="Id" HeaderText="Id" /></Columns><PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" /><FooterStyle BackColor="#bfbfbf" ForeColor="Black" /><EditRowStyle Font-Size="Medium" /><SelectedRowStyle BackColor="White" ForeColor="Black" /><PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" /><HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" /><RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" /></asp:GridView></div></td></tr></table></asp:Panel></ContentTemplate></asp:UpdatePanel></ContentTemplate></ajaxToolkit:TabPanel>
                
                	</ajaxToolkit:TabContainer>              
              
              </asp:Panel>
              
              
                </td>
                <td class="e"> </td>
            </tr>
            <tr>
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
             
           
        
        <asp:Panel ID="Pnl_CancelBill" runat="server">
                         <asp:Button runat="server" ID="Btn_Cancel" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_ChangePromotion"  runat="server" CancelControlID="Btn_ChangeCancel" 
                                  PopupControlID="Pnl_Bill" TargetControlID="Btn_Cancel"  BackgroundCssClass="modalBackground"/>
                          <asp:Panel ID="Pnl_Bill" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Remarks</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                        <asp:Label ID="Lbl_ChangeMsg" runat="server" Text="" class="control-label"></asp:Label>
                                        <div style=" text-align:center">
                                        <asp:TextBox ID="Txt_CancelReason" runat="server" style="vertical-align:middle" Width="220" Height="60" class="form-control" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_CancelReason_FilteredTextBoxExtender" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="Txt_CancelReason">
                                         </ajaxToolkit:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Change" ControlToValidate="Txt_CancelReason" ErrorMessage="*"></asp:RequiredFieldValidator>
                                 </div>
                                       <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_Change"  runat="server" Text="Save"  class="btn btn-info" 
                                                ValidationGroup="Change" onclick="Btn_Change_Click"/>     
                                            <asp:Button ID="Btn_ChangeCancel" runat="server" Text="Cancel" class="btn btn-info"/>
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


        <asp:Panel ID="Pnl_ToClassSelection" runat="server">
                         <asp:Button runat="server" ID="Btn_ToClass" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_TOClassSelection"  runat="server" CancelControlID="Btn_ClassCancel" 
                                  PopupControlID="Pnl_ToClass" TargetControlID="Btn_ToClass" BackgroundCssClass="modalBackground" />
                          <asp:Panel ID="Pnl_ToClass" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Select To-Class</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    
                                        <table class="tablelist" width="100%">
                                        
                                        <tr>
                                        <td class="leftside">Select Type:</td>
                                        <td class="rightside">
                                            <asp:RadioButtonList ID="Rdb_ToClassType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Selected="True">Normal</asp:ListItem>
                                             <asp:ListItem Value="1">Random</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        </tr>
                                        
                                        <tr>
                                        <td class="leftside">Select Class:</td>
                                        <td class="rightside">
                                            <asp:DropDownList ID="Drp_ToClassSelect" runat="server" class="btn btn-info" Width="140px">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="Hdn_SelectedToClass" Value="0" runat="server" />
                                             <asp:HiddenField ID="Hdn_ToClassIdType" Value="0" runat="server" />
                                        </td>
                                        </tr>
                                        <tr><td colspan="2">&nbsp;</td></tr>
                                        
                                         <tr><td class="leftside">&nbsp;</td><td class="rightside">
                                         <asp:Button ID="Btn_ToClassSave"  runat="server" Text="Select" class="btn btn-info" onclick="Btn_ToClassSave_Click"/>     
                                            <asp:Button ID="Btn_ClassCancel" runat="server" Text="Cancel" class="btn btn-info" />
                                            </td></tr>
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
