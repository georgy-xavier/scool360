<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="BookSearchDetails.aspx.cs" Inherits="WinEr.BookSearchDetails"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
         .leftside
        {
            font-size:12px;
            width: 250px;
            text-align:right;
            font-weight:lighter;
        }
        .rightside
        {
             font-size:12px;
             text-align:left;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="contents">

<div id="right">

<div class="label">Library Manager</div>
<div id="SubLibMenu" runat="server">
		
 </div>
</div>



<div id="left">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:Panel ID="Pnl_mainarea" runat="server">
    
        
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/book_accept.png" 
                        Height="28px" Width="29px" />  </td>
				<td class="n">Book Details</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<br />
				
				
				        <table class="tablelist" cellspacing="5">
                          <tr>
                              <td >
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Book Name : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_BkName" runat="server" Font-Bold="True" ForeColor="Black" ></asp:Label>
                            
                              </td>
                           </tr>
                           <tr>
                              <td class="leftside">
                                  Book No : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_BNo" runat="server" Font-Bold="True" ForeColor="Black" ></asp:Label>
                            
                              </td>
                           </tr>
                          
                           <tr>    
                              <td class="leftside">
                                  Author Name : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_AuthorName" runat="server" Font-Bold="True" 
                                      ForeColor="Black" ></asp:Label>
               
                              </td>
                                
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Publisher : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_Publisher" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                           </tr>
                           <tr>
                              <td class="leftside">
                                  Year : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_Year" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                 </td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Edition : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_Edition" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                              
                              </td>
                           </tr>
                           <tr>
                              <td class="leftside">
                                  Type : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_Type" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                               
                              </td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Category : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_Category" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                               
                              </td>
                          </tr><tr>
                              <td class="leftside">
                                  Count : </td>
                              <td class="rightside">
                                  <asp:Label ID="Lbl_Count" runat="server" Font-Bold="True" ForeColor="Black" ></asp:Label>
                               
                              </td>
                               
                          </tr>
                          
                            <tr>
                                <td class="leftside">
                                    Rack No : </td>
                                <td class="rightside">
                                    <asp:Label ID="Lbl_RackNo" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                                 
                                </td>
                                </tr>
                            <tr>
                                <td class="leftside">
                                    Price : </td>
                                <td class="rightside">
                                    <asp:Label ID="Lbl_Price" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                                  
                                </td>
                            </tr>
                           <tr>
                              <td class="leftside">
                                  BarCode : </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_Bbarcode" runat="server" Font-Bold="True" ForeColor="Black" ></asp:Label>
                            
                              </td>
                           </tr>
                            <tr>
                              <td class="leftside">
                                  Book Sl No : </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_bookslno" runat="server" Font-Bold="True" ForeColor="Black" ></asp:Label>
                            
                              </td>
                           </tr>
                          <tr>
                              <td >
                                  &nbsp;</td>
                              <td >
                                  &nbsp;</td>
                            </tr>
                            <tr>
                              <td >
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
                      </table>
				        
				<br />
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
    </asp:Panel>
</div>

<div class="clear">
</div></div>
</asp:Content>
