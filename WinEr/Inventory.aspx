<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="Inventory.aspx.cs" Inherits="WinEr.Inventory" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">
    
     function isIE() {
         return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);
     }
   
     function SelectAll(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=Grd_Items.ClientID%>');
         var Status = cbSelectAll.checked;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             cb.checked = Status;
         }
     }
     function openIncpopup(strOpen) {
         open(strOpen, "Info", "status=1, width=600, height=400,resizable = 1");
     }
    </script>
    
<style type="text/css">
        .style1
        {
            width: 100%;
        }
         .tdleft
        {
           text-align:right;
        }
         .tdRight
        {
           font-weight:bolder;
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
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <img alt="" src="Pics/Misc-Box.png" height="35" width="35" /> </td>
				<td class="n">
				<table width="100%">
				<tr>
				<td align="left">Inventory</td>
				<td align="right">
				<div class="form-inline">
				  Location:  <asp:DropDownList ID="Drp_Searchlocation" runat="server" AutoPostBack="true"  class="form-control"
                     Width="170px" onselectedindexchanged="Drp_Searchlocation_SelectedIndexChanged" ></asp:DropDownList>
                     </div></td>
				
				</tr>
				</table>
				
				 
				</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			
				<asp:Panel ID="Pnl_SearchArea" runat="server">
				<div align="right">
			
                  
              &nbsp;<asp:Button ID="Btn_InputFlow"  runat="server" Text="GOODS RECEIPT" 
                       onclick="Btn_InputFlow_Click" class="btn btn-primary" />
                       
              &nbsp;<asp:Button ID="Btn_OutputFlow"  runat="server" Text="MOVE ITEM" 
                       onclick="Btn_OutputFlow_Click" class="btn btn-primary"/>                       
                    &nbsp;<asp:Button ID="Btn_Issue"  runat="server" Text="STAFF ISSUE " 
                      class="btn btn-primary" onclick="Btn_Issue_Click"/> 
                      &nbsp;<asp:Button ID="Btn_Sale"  runat="server" Text="SALE" 
                      class="btn btn-primary" onclick="Btn_Sale_Click"/> 
                        &nbsp;<asp:Button ID="Btn_AddItem"   runat="server" Text="ADD ITEM" 
                  onclick="Lnk_AddNewItem_Click" class="btn btn-primary" Visible="true"  />      
              &nbsp;
				</div>
				<table width="100%">
                  <tr>
          
            <td style="width:250px;">
     
                    <div class="roundboxorange">
		                <table >
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
				        
				          <asp:Panel ID="panel1" runat="server" DefaultButton="Img_Search">
				           <h3>Search Item </h3>
				           <table>
				           <tr>
				             <td> 
                                <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
                             </td>
                             <td>
                             <div class="form-inline">
                                                             <asp:DropDownList ID="Drp_Categories" runat="server" class="form-control" Width="100px">
                                </asp:DropDownList>
                                 
                                 <asp:ImageButton ID="Img_Search" runat="server" ImageAlign="AbsMiddle" 
                                     ImageUrl="~/images/SearchIcon.jpg" Width="20px" Height="20px" 
                                     onclick="Img_Search_Click" />
                                 </div>
   
                            </td>
				           </tr>
				           <tr>
				            <td class="style2" >
				            </td>
                             <td class="style2">
				               <asp:CheckBox ID="ChkWarning" runat="server" Text=" Warning Level" Checked="false" />
				           
                               
				            </td>
				           
				           </tr>
				           </table>
				
                           </asp:Panel>
				    
				        </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
         
            </td>
                 
            <td style="width:250px;">
                <div class="roundboxorange">
		                <table >
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
				        
				          <asp:Panel ID="panel_search" runat="server" DefaultButton="Img_Go">
				           <h3>Quick Search</h3>
				           <table>
				           <tr>
				             <td><asp:Label ID="Label2" runat="server" Text="Search By Name"></asp:Label>
				             </td>
				             <td>
				                  
				             </td>
				           </tr>
				           <tr>
				           
				            <td colspan="2">
				            <div class="form-inline">
				                <asp:TextBox ID="TxtSearch" runat="server"  class="form-control" ></asp:TextBox>	
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TxtSearch_TextBoxWatermarkExtender" 
                                    runat="server" Enabled="True" TargetControlID="TxtSearch" WatermarkText="Search" >
                                </ajaxToolkit:TextBoxWatermarkExtender>
                                <cc1:AutoCompleteExtender ID="TxtSearch_AutoCompleteExtender" 
                                                  runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetItem" ServicePath="WinErWebService.asmx"  
                                                  TargetControlID="TxtSearch" UseContextKey="true"  MinimumPrefixLength="1">
                                              </cc1:AutoCompleteExtender>
                                <asp:ImageButton ID="Img_Go" runat="server" ImageUrl="~/images/SearchIcon.jpg" 
                                   ImageAlign="AbsMiddle" Width="20px" Height="20px" onclick="Img_Go_Click"/>
                                   </div>
				            </td>
				            
				           
				           </tr>
				           </table>
				
                           </asp:Panel>
				    
				        </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
                
            </td>
            
            <td>
            
              <div class="roundboxorange">
		<table >
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		   
		   
		    
		    <table class="style1">
            
                <tr>
                    <td class="tdleft" >
                     <img  alt="" src="Pics/tag_green.png" height="15px"/> 
                        No of Items:</td>
                    <td class="tdRight">
                        <asp:Label ID="Lbl_Ct" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdleft">
                        <img alt="" src="Pics/warning.png" width="14px" />No of Items in warning level:</td>
                    <td class="tdRight">
                        <asp:Label ID="Lbl_WrnCt" runat="server" Text="0"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tdleft">
                        </td>
                    <td class="tdRight">
                        &nbsp;</td>
                </tr>
            </table>
		
	
		</td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		
		
		</div>	
            
            </td>
            
            
            
          </tr>
          	
      
          
                 </table>	
					</asp:Panel>				
	
					
					<asp:Panel ID="Pnl_ItemList" runat="server">
                     
              <div class="roundbox">
		<table width="100%">
		<tr>
		<td class="topleft"></td>
		<td class="topmiddle"></td>
		<td class="topright"></td></tr>
		<tr>
		<td class="centerleft"></td>
		<td class="centermiddle">
		<table width="100%">
	
		
		<tr>
		
		<td>
       <%-- <img alt="" src="Pics/evolution-tasks.png" width="35" height="35" />--%></td>
	    <td><%--<h3>Item List</h3>--%></td>
	    
	    
	     
              <td align="left" visible="false">
              
              </td>
              
                <td style="text-align:right;">
		<asp:ImageButton ID="Btn_exporttoexel" runat="server"  Width="35" Height="35" ToolTip="Export To Excel"
             ImageUrl="Pics/Excel.png" onclick="Btn_exporttoexel_Click" />
        </td>
	  
	    </tr>
	
	    <tr>
	    <td colspan="5">
	    <div class="linestyle"></div> 
	    </td>
	    </tr>
	    <tr>
	    <td colspan="5">
	    <div >
	    
					 <div style=" overflow:auto; max-height: 400px;">
                <asp:GridView ID="Grd_Items" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="false"  AllowSorting="true"
                      PageSize="100"   Width="100%"   onpageindexchanging="Grd_Items_PageIndexChanging" 
                       OnSorting="Grd_Items_Sorting"  onselectedindexchanged="Grd_Items_SelectedIndexChanged" >
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                   <asp:TemplateField ItemStyle-Width="20">
                    <ItemTemplate>
                        <asp:CheckBox ID="Chk_Item" runat="server" Checked="false" />
                    </ItemTemplate>
                     <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                  </asp:TemplateField>
                      <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                      <asp:BoundField DataField="MaxQty" HeaderText="MaxQty" SortExpression="Id" />
                      <asp:BoundField DataField="OpnQuantity" HeaderText="OpnQuantity" SortExpression="Id" />
                      <asp:BoundField DataField="ItemName" HeaderText="Name"  SortExpression="ItemName"/>
                
                     <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" ItemStyle-Width="240"  />
                      <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" ItemStyle-Width="120"  />                                                              
                      <asp:BoundField DataField="UnitType" HeaderText="Unit" SortExpression="UnitType" ItemStyle-Width="60" />
                      <asp:BoundField DataField="Stock" HeaderText=" Total Stock " SortExpression="Stock" ItemStyle-Width="60" />                                                             
                      <asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost" ItemStyle-Width="70" />
                       <asp:CommandField ItemStyle-Width="35" ControlStyle-Width="30px" ItemStyle-Font-Bold="true"  HeaderText="Edit"
                       ItemStyle-Font-Size="Smaller" SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
                              ShowSelectButton="True">
                            <ControlStyle />
                                    <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                        </asp:CommandField>
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                </asp:GridView>
                </div>
                </div>
                </td></tr>
	<tr><td colspan="3" align="center"><asp:Label ID="lbl_ItemMessage" ForeColor="Red" runat="server" Text=""></asp:Label></td></tr>
	
	</table>
		
   

		   
		   	</td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		
		
		</div>	
                   </asp:Panel>
                  
                    <asp:Panel ID="Pnl_AddInventory" runat="server" Visible="false" >
                 <div class="container skin1" >
              
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <asp:Image ID="Image2" runat="server" ImageUrl="Pics/add.png" 
                        Height="30px" Width="30px" />
                    </td>
				<td class="n">ADD NEW ITEM</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				        <br />
				        <br />
				   <table class="tablelist">
                        <tr>
                            <td class="leftside">
                               Inventory Name<span style="color:Red;font-size:14px;" >*</span>
                               </td>
                            <td class="rightside">
                                <asp:TextBox ID="txt_Inventoryname" runat="server" Text ="" class="form-control" Width="300px" MaxLength="70"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_Inventoryname" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" TargetControlID="txt_Inventoryname" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2"  runat="server"  ControlToValidate="txt_Inventoryname"  Display="Dynamic" ValidationGroup="VgSave" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        
                       
                        <tr>
                             <td class="leftside">
                              Description<span style="color:Red;font-size:14px;" >*</span>
                            </td>
                              <td class="rightside">
                                <asp:TextBox ID="txt_description" runat="server" Text ="" TextMode="MultiLine" class="form-control" Width="300px" MaxLength="250"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_description" Display="Dynamic" ErrorMessage="<br>Maximum  250 characters"  ValidationExpression="[\s\S]{1,250}"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_description" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txt_description" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr>
                             <td class="leftside" >
                              Category
                            </td>
                              <td class="rightside">
                                <asp:DropDownList ID="Drp_Category" runat="server"   Width="205px" class="form-control"></asp:DropDownList>
                                  <asp:LinkButton ID="LnkBtn_CreateCategory" runat="server" 
                                      onclick="LnkBtn_CreateCategory_Click" >Add New</asp:LinkButton>
                            </td>                    
                        </tr> 
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                      
                    
                         <tr>
                             <td class="leftside">
                              Maximum Stock Qty.<span style="color:Red;font-size:14px;" >*</span>
                                </td>
                              <td class="rightside">
                                  <asp:TextBox ID="txt_maxqty" runat="server" Text ="0" Width="200px" MaxLength="8" class="form-control"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_maxqty"> </cc1:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_maxqty" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                                  </td></tr>
                                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                           <tr>
                             <td class="leftside">
                           Minimum Stock Qty.
                            </td>
                              <td class="rightside">
                               <asp:TextBox ID="txt_minqty" runat="server" Text ="0" Width="200px" MaxLength="8" class="form-control"></asp:TextBox> 
                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_minqty"> </cc1:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_minqty" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        
                        <tr>
                        <td class="leftside" >
                              Available Stock 
                                </td>
                              <td class="rightside">
                                  <asp:TextBox ID="txt_Stock" runat="server" Text="0" Width="200px" MaxLength="8" class="form-control"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"  runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_Stock"> </cc1:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_Stock" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                           
                               
                                 </td>
                             </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                       
                                
                        <tr>
                             <td class="leftside">
                              Measurement Unit
                                </td>
                              <td class="rightside">
                                
                                <asp:DropDownList ID="Drp_Unit" runat="server"   Width="205px" class="form-control"></asp:DropDownList>
                                 </td>
                             </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        
                        <tr>
                             <td class="leftside">
                              Basic Cost <span style="color:Red;font-size:14px;" >*</span>
                            </td>
                              <td class="rightside">
                                 <asp:TextBox ID="txt_cost" runat="server" Text ="" Width="200px" MaxLength="8" class="form-control"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                                    runat="server" Enabled="True" FilterType="Numbers"  
                                                    TargetControlID="txt_cost">
                                                </cc1:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_cost" ErrorMessage="Enter an Amount"></asp:RequiredFieldValidator>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr>
                             <td class="leftside">
                             Add More Items?
                            </td>
                            <td class="rightside">
                                <asp:CheckBox ID="Chk_AddMore" Text="Yes" Checked="false" runat="server" /></td>
                                </tr> 
                                   <tr>
                             <td class="leftside">
                             <br />
                                </td>
                              <td class="rightside">
                              <br />
                               </td>
                            </tr>
                                  
                         <tr>
                             <td class="leftside">
                                </td>
                              <td class="rightside">
                              <asp:Button ID="Btn_Save" runat="server" Text="Save" class="btn btn-success"
                                         ValidationGroup="VgSave" onclick="Btn_Save_Click"/>
                               <asp:Button ID="Btn_clear" runat="server" Text="Cancel" class="btn btn-danger"
                                      onclick="Btn_clear_Click"   />
                            </td>
                        </tr>
                    </table>
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
                  </asp:Panel>
        
   
                   <asp:Panel ID="Pnl_EditItem" runat="server" Visible="false" >
                 <div class="container skin1" >
              
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <asp:Image ID="Image1" runat="server" ImageUrl="Pics/Details.png" 
                        Height="30px" Width="30px" />
                    </td>
				<td class="n">Item Details</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				        <br />
				        <br />
				   <table class="tablelist">
                        <tr>
                            <td class="leftside" style="width:300px">
                               Inventory Name
                               </td>
                            <td class="rightside">
                                <asp:Label ID="lbl_Name" runat="server" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txt_EditItemName" runat="server" Text ="" Width="300px" class="form-control" MaxLength="70"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditItemName" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txt_EditItemName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"  runat="server"  ControlToValidate="txt_EditItemName"  Display="Dynamic" ValidationGroup="VgUpdate" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                    
                        </tr>
                        
                       
                        <tr>
                             <td class="leftside">
                              Description
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_desc" runat="server" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txt_EditDesc" runat="server" Text ="" TextMode="MultiLine" class="form-control" Width="300px" MaxLength="250"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditDesc" Display="Dynamic" ErrorMessage="<br>Maximum  250 characters"  ValidationExpression="[\s\S]{1,250}"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditDesc" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txt_EditDesc" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                
                            </td>
                    
                        </tr>
                         <tr>
                             <td class="leftside" >
                              Category
                            </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_Category" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:DropDownList ID="Drp_EditCategory" runat="server"  class="form-control" Width="205px" ></asp:DropDownList>
                                  <asp:LinkButton ID="Lnk_EditnewCategory" runat="server" 
                                      onclick="LnkBtn_CreateCategory_Click" >Add New</asp:LinkButton>
                            </td>                    
                        </tr>        
                    
                         <tr>
                             <td class="leftside">
                              Maximum Stock Qty.
                                </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_maxqty" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:TextBox ID="txt_EditMaxqty" runat="server" Width="200px" MaxLength="8" class="form-control"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_EditMaxqty"> </cc1:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditMaxqty" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                                  </td></tr>
                           <tr>
                             <td class="leftside">
                           Minimum Stock Qty.
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_minqty" runat="server" Font-Bold="true"></asp:Label>
                               <asp:TextBox ID="txt_EditMinQty" runat="server"  Width="200px" MaxLength="8" class="form-control"></asp:TextBox> 
                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_EditMinQty"> </cc1:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditMinQty" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                            </td>
                    
                        </tr>
                        
                        <tr>
                        <td class="leftside" >
                              Available Stock 
                                </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_Stock" runat="server" Font-Bold="true"></asp:Label>
                                 </td>
                             </tr>
                       
                                
                        <tr>
                             <td class="leftside">
                              Measurement Unit
                                </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_EditUnit" runat="server" Font-Bold="true"></asp:Label>
                                <asp:DropDownList ID="Drp_EditUnit" runat="server"   Width="205px" class="form-control"></asp:DropDownList>
                                 </td>
                             </tr>
                        
                        <tr>
                             <td class="leftside">
                              Basic Cost
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_cost" runat="server" Font-Bold="true"></asp:Label>
                                 <asp:TextBox ID="txt_EditCost" runat="server" Text ="" Width="200px" class="form-control" MaxLength="8"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" 
                                                    runat="server" Enabled="True" FilterType="Numbers"  
                                                    TargetControlID="txt_EditCost">
                                                </cc1:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditCost" ErrorMessage="Enter an Amount"></asp:RequiredFieldValidator>
                            </td>
                    
                        </tr>
                       
                                   <tr>
                             <td class="leftside">
                             <br />
                                 <asp:Label ID="lbl_ItemId" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                              <td class="rightside">
                              <br />
                               </td>
                            </tr>
                                  
                         <tr>
                             <td class="leftside">
                            
                                </td>
                              <td class="rightside">
                                       <asp:Button ID="Btn_EditItem" runat="server" Text="Edit" onclick="Btn_EditItem_Click" class="btn btn-success" />
                                <asp:Button ID="Btn_Update" runat="server" Text="Update"
                                         ValidationGroup="VgUpdate" onclick="Btn_Update_Click" class="btn btn-primary" />  
                                   <asp:Button ID="Btn_Delete" runat="server" Text="Delete" onclick="Btn_Delete_Click" class="btn btn-danger"
                                        />
                               <asp:Button ID="Btn_EditCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                      onclick="Btn_clear_Click"   />
                            </td>
                        </tr>
                    </table>
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
                  </asp:Panel>
                  
                  
                   <asp:Panel ID="Pnl_DeleteConfirm" runat="server">
                       
                       <%--  <asp:Button runat="server" ID="Button4" style="display:none"/>--%>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteConfirm" 
                                  runat="server" CancelControlID="Btn_DeleteNo" 
                                  PopupControlID="Pnl_ConfirmDelete" TargetControlID="Btn_Delete"  />
                          <asp:Panel ID="Pnl_ConfirmDelete" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="lbl_delmsg" runat="server" Text="Are you sure to delete the item?"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_DeleteYes" runat="server" Text="Yes" class="btn btn-success" 
                                onclick="Btn_DeleteYes_Click"/>
                             <asp:Button ID="Btn_DeleteNo" runat="server" Text="No"  class="btn btn-danger"/>
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel> 
                  
                  
                 
            
            
            
                  
                          
                    
            
             <WC:MSGBOX id="WC_MessageBox" runat="server" />   
            
            
                  
					
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
       
   	<asp:Button runat="server" ID="Btn_HidAdd_New_Category" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox_AddNewCategory"   runat="server"  PopupControlID="Pnl_AddNewCategory" TargetControlID="Btn_HidAdd_New_Category"  />
     <asp:Panel ID="Pnl_AddNewCategory" runat="server" style="display:none"><%--style="display:none"--%>
 <div id="newprocess" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Create New Category</td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
             
                 <center>
                    <table >                                
                        <tr>
                            <td>
                                Enter Category Name  
                               <br />  
                            <asp:TextBox ID="txt_new_category" runat="server" Text="" Width="200px" class="form-control"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="VgSaveCategory" ControlToValidate="txt_new_category" ErrorMessage="Enter category"></asp:RequiredFieldValidator><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txt_new_category" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator4"  runat="server"  ControlToValidate="txt_new_category"  Display="Dynamic" ValidationGroup="VgSaveCategory" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator></td></tr><tr>
                        <td>Enter Category Type
                        <asp:DropDownList ID="Drp_CategoryType" runat="server" Width="200px" class="form-control"></asp:DropDownList>
                        </td>
                        </tr>
                         <tr>
                            <td>
                                 <asp:Label ID="Lbl_MsgCreateCategory" runat="server" ForeColor="Red" Text=""></asp:Label></td></tr><tr>
                            <td>
                                 <asp:Button ID="Btn_Add_new_cat" runat="server" Text="Save" class="btn btn-success" 
                                 ValidationGroup="VgSaveCategory" onclick="Btn_Add_new_cat_Click"/>
                                  <asp:Button ID="btn_cancel" runat="server" Text="Cancel" class="btn btn-danger" />  
                                
                            </td>
                        </tr>
                        </table>
                    </center>
             </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel>  

     
<asp:Button runat="server" ID="Btn_GoddsReceipt" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_Goodsreceiptrnote"   runat="server"  PopupControlID="Pnl_GoodsReceiptNote" CancelControlID="Btn_GoodsReceiptCancel" TargetControlID="Btn_GoddsReceipt"  />
     <asp:Panel ID="Pnl_GoodsReceiptNote" runat="server" style="display:none"><%-- style="display:none"--%>
 <div id="Div1" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Goods Receipt Note</td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
            <div id="divgoodsreceipt" runat="server">
            <%--<table width="100%">
            <tr><td align="right">Date:01-07-2011</td></tr>
            <tr><td>Items aaa,jj,dfdf,dfdgdf,ghvmn,vghg,ftghf purchased from sss</td></tr>
            <tr><td align="center">Totalcost:2500</td></tr>
            
            
            </table>--%>
            </div>
            <table width="100%"><tr><td align="center"> <asp:Button ID="Btn_GoodsReceiptCancel" 
            runat="server" class="btn btn-primary" Text="Ok" /></td></tr></table>
           
            
                     </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel>  

<asp:Button runat="server" ID="Btn_Salereport" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_SALEREPORT"   runat="server"  PopupControlID="Pnl_SaleReport" CancelControlID="Btn_SalereportCancel" TargetControlID="Btn_Salereport"  />
     <asp:Panel ID="Pnl_SaleReport" runat="server" style="display:none"><%--style="display:none"--%>
 <div id="Div2" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Sales Report</td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
            <div id="Div_salereport" runat="server">
            
          <%-- <table width="100%">
            <tr><td colspan="2" align="center" >Supplier Name:fgdfg</td><td align="right">Date:01-07-2011</td></tr>
            <tr><td align="center">Items</td><td align="center">Count</td><td align="center">Cost</td></tr>
            <tr><td align="center">gdfgdg</td><td align="center">2</td><td align="center">200</td></tr>
            <tr><td><td></td></td><td align="center">Totalcost:2500</td></tr>
            
            
            </table>--%>
            
            </div>
            <table width="100%"><tr><td align="center"> 
            <asp:Button ID="Btn_SalereportCancel" runat="server" class="btn btn-primary" Text="Ok" /></td></tr></table>
           
            
                     </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel> 


 
                        
                        
                        
                        
 


                        
 

             </ContentTemplate> 
    <Triggers><asp:PostBackTrigger ControlID="Btn_exporttoexel" /></Triggers>         
             
    </asp:UpdatePanel>    

<div class="clear"></div>

</div>


</asp:Content>
