<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateFeeAccount.aspx.cs" Inherits="WinEr.WebForm10"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
            
        }
        .style5
        {
        }
        .LeftTd
        {
           text-align:right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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



      <WC:MSGBOX id="WC_MessageBox" runat="server" />    


<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> 
                    <img alt="" src="Pics/add.png" height="30" width="30" /></td>
				<td class="n">Create Fee Account</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					
					  <ajaxToolkit:TabContainer runat="server" ID="Tabs"  
                          CssClass="ajax__tab_yuitabview-theme" >
                                 <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                                    <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/book.png" /><b>GENERAL</b></HeaderTemplate>
                                    <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server"  DefaultButton="Btn_Create">
        <br />
        <br />
        <table class="tablelist">
            <tr>
                <td class="leftside">
                    Fee Name<span style="color:Red">*</span></td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_FeeName" runat="server" class="form-control" Width="180px"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" 
                                runat="server" Enabled="True" TargetControlID="Txt_FeeName" 
                        FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                </td>
                </tr>
                <tr>
                    <td class="leftside">
                        Type</td>
                    <td  class="rightside">
                        <asp:RadioButtonList ID="Rdo_Feetype" runat="server"  AutoPostBack="True"
                            RepeatDirection="Horizontal" 
                            onselectedindexchanged="Rdo_Feetype_SelectedIndexChanged"> 
                        </asp:RadioButtonList>
                    </td>
            </tr>
                <tr>
                <td class="leftside">
                    <asp:Label ID="Lbl_Frequency" runat="server" class="control-label" Text="Frequency"></asp:Label>
                </td>
                <td class="rightside">
                    <asp:DropDownList ID="Drp_Frequency" runat="server" class="form-control"
                       
                         Width="180px" >
                    </asp:DropDownList>
                </td>
            </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
            
            <tr>
                <td class="leftside">
                <asp:Label ID="Lbl_Asso" runat="server" class="control-label" Text="Associated To"></asp:Label>
                </td>
                <td class="rightside">
                    <asp:DropDownList ID="Drp_Asso" runat="server" Width="180px" class="form-control"  >
                        
                    </asp:DropDownList>
                </td>
                
            </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
            
            <tr>
                <td class="leftside">
                    Description</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Desc" runat="server"  class="form-control" TextMode="MultiLine" 
                       Width="180px" MaxLength="490"></asp:TextBox>
                        
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" TargetControlID="Txt_Desc" 
                        FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        
                </td>
            </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
          
            <tr>
                <td class="leftside"></td>
                <td  class="rightside">
                    <asp:Button ID="Btn_Create" runat="server" onclick="Btn_Create_Click" 
                         Text="Create" Class="btn btn-success" />&nbsp;
                
                    <asp:Button ID="Btn_cancel" runat="server" onclick="Btn_cancel_Click" 
                        Text="Clear" Class="btn btn-danger" />
                </td>
            </tr>
        </table>
       
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="LinkDetails" runat="server" onclick="LinkDetails_Click">Show 
        All Fees</asp:LinkButton>
        <br />
        
        <br />
</asp:Panel>


		<asp:Panel ID="Panel2" runat="server" Visible="False" >
		<div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
       <img alt="" src="Pics/currencygreen.png" width="45" height="45" /></td>
	<td><h3>Fee List</h3></td>
	<td style="text-align:right;">
		
         </td>
	</tr></table>
		
<div class="linestyle"></div> 

<div style=" overflow:auto; max-height: 350px;">
        <asp:GridView ID="GridViewFeeDetails"  runat="server"  AutoGenerateColumns="False"
             GridLines="Vertical" Width="97%" 
             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                  <Columns>
                                <asp:BoundField DataField="Fee Name" HeaderText="Fee Name" />
                                <asp:BoundField DataField="Feequency Type" HeaderText="Frequency Type"  >
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Associated To" HeaderText="Associated To" >
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fee Type" HeaderText="Fee Type" >
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                            </Columns>
           <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
                      ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
        </asp:GridView>
             <br />
        </div>
  </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div> 
                                   
		       
    </asp:Panel>


                                    </ContentTemplate>
                                 </ajaxToolkit:TabPanel> 
                                 
                                  <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                                    <HeaderTemplate><asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/book.png" /><b>OTHER</b></HeaderTemplate>
                                    <ContentTemplate>
                                    <br />
                                    <asp:Panel  ID="Pnl_Otherfee" runat="server">
                                        <table class="tablelist">
                                         <tr>
                                            <td class="leftside">Name</td>
                                            <td class="rightside">
                                                <asp:TextBox ID="Txt_Name"  Width="180px" class="form-control" runat="server" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="Txt_Name_reqVal" ErrorMessage="Enter fee name" ValidationGroup="OtherFee" ControlToValidate="Txt_Name" runat="server"></asp:RequiredFieldValidator>
                                            </td>
                                          <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Name_FilteredTextBoxExtender" 
                                          runat="server" Enabled="True" TargetControlID="Txt_Name" 
                                                 FilterMode="InvalidChars" InvalidChars="'\">
                                         </ajaxToolkit:FilteredTextBoxExtender>
                                        </tr>
                                        <tr>
                                            <td class="leftside">Description</td>
                                            <td class="rightside">
                                                <asp:TextBox ID="Txt_Description" TextMode="MultiLine" Width="180px" runat="server" class="form-control"  MaxLength="200"></asp:TextBox>
                                            </td>
                                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Description_FilteredTextBoxExtender" 
                                              runat="server" Enabled="True" TargetControlID="Txt_Description" 
                                                FilterMode="InvalidChars" InvalidChars="'\">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </tr>
                                        <tr>
                                            <td class="leftside"></td>
                                            <td class="rightside">
                                                <asp:Button ID="Btn_SaveOthr" runat="server" Class="btn btn-success" Text="Save" ValidationGroup="OtherFee" OnClick="Btn_SaveOthr_Click"/>
                                            </td>
                                        </tr>
                                        </table>
                                    </asp:Panel>
                                    
                                    <asp:Panel ID="Pnl_OtherFeeList" runat="server" >
		                <div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">              
		                  <table width="100%"><tr>
		                   <td style="width:48px;">
                           <img alt="" src="Pics/currencygreen.png" width="45" height="45" /></td>
	                       <td><h3>Fee List</h3></td>
	                       <td style="text-align:right;">
		
                          </td>
	                   </tr></table>
		         <div  style="text-align:center">
                     <asp:Label ID="Lbl_OtherFeeMessage" class="control-label" runat="server" ></asp:Label>
		         </div> 
                  <div class="linestyle"></div> 

<div style=" overflow:auto; max-height: 350px;">
        <asp:GridView ID="Grd_OtherFeeList"  runat="server"  AutoGenerateColumns="False"
             GridLines="Vertical" Width="97%" 
             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                                  <Columns>
                                  <asp:BoundField DataField="Id" HeaderText="Id"  />
                                  <asp:BoundField DataField="Name" HeaderText="Fee Name" >
                                      <ItemStyle Width="200px" />
                                      </asp:BoundField>
                                  <asp:BoundField DataField="Description" HeaderText="Description"  >
                                      <ItemStyle Width="300px" />
                                      </asp:BoundField>
                                  </Columns>
                   <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                                      HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black" Height="25px"  HorizontalAlign="Left" VerticalAlign="Top" />
        </asp:GridView>
             <br />
        </div>
  </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div> 
                                   
		       
    </asp:Panel>
                                    
                                    </ContentTemplate>
                                 </ajaxToolkit:TabPanel> 
                                 
					</ajaxToolkit:TabContainer>
					
					
					
			
					
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


	<br />
              
					
       
      
           
     </ContentTemplate>
    </asp:UpdatePanel>

<div class="clear"></div>
                
                    
</div>
</asp:Content>

