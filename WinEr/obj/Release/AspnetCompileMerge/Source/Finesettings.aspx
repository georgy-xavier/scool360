<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="Finesettings.aspx.cs" Inherits="WinEr.Finesettings" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
<table width="100%">
            <tr>
            <td ></td>
            <td>
                <div id="tbldiv" runat="server" style="width:100%;">
 <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> <img alt="" src="Pics/configure1.png" width="30" height="30" /></td>
                <td class="n">Fine Configuration</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o" style="height: 149px"> </td>
                <td class="c" style="height: 149px" >   
                <table width="100%">
                <tr>
                <td>
               
              
              <asp:Panel ID="Pnl_FineSettings" runat="server">
              <table width="100%" class="tablelist">
              <tr>
              <td class="leftside">Fine Date:</td>
              <td class="rightside">
              <asp:RadioButtonList ID="Rdb_Finedate" runat="server" RepeatDirection="Horizontal"  CellSpacing="10">
              <asp:ListItem Text="Last Date" Value="1"></asp:ListItem>
              <asp:ListItem Text="Due Date" Value="2"></asp:ListItem>
              </asp:RadioButtonList>           
              </td>
              </tr>
                <tr>
              <td class="leftside">Fine Amount:</td>
              <td class="rightside">
              <asp:RadioButtonList ID="Rdb_FineAmount" runat="server" RepeatDirection="Horizontal" CellSpacing="10">
              <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
              <asp:ListItem Text="Pecentage(%)" Value="2"></asp:ListItem>
              </asp:RadioButtonList>           
              </td>
              </tr>
               <tr>
               <td class="leftside"></td>
              <td class="rightside">
             <asp:Button ID="Btn_save" runat="server"  Text="Save" Width="90px"  Class="btn btn-primary"
                      onclick="Btn_save_Click" />
              </td>
              </tr>
              <tr>
              <td colspan="2">
              <asp:Label ID="Lbl_Msg" runat="server" class="control-label" ForeColor="Red"></asp:Label>
              </td>
              </tr>
              </table>
              </asp:Panel>
            
              
                  </td>
                    </tr>
                  
                  <tr>                 
                   <td align="center">  
                   <asp:Label ID="Lbl_MsgErr"  runat="server" class="control-label" ForeColor="Red" Text=""></asp:Label> 
                   <div style="width:100%" align="right">
                   <asp:Button ID="Btn_FineMAntSave" 
                           runat="server" Text="Save" Width="90px" Class="btn btn-primary" 
                           onclick="Btn_FineMAntSave_Click" />
                           </div>         
                   <asp:GridView ID="GridFineAmount" runat="server" AutoGenerateColumns="False" 
                    CellPadding="3" CellSpacing="2" ForeColor="Black" GridLines="Vertical" Width="100%"
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                     OnSelectedIndexChanged="GridFineAmount_SelectedIndexChanged">
                   
                    <Columns>
                      
                        <asp:BoundField DataField="Id" HeaderText="Fee ID" />
                        <asp:BoundField DataField="FrequencyId" HeaderText="Frequency Id" />   
                            <asp:BoundField DataField="Type" HeaderText="Type ID" />  
                            <asp:BoundField DataField="AccountName" HeaderText="Fee Name" />                     
                        <asp:TemplateField HeaderText="Fine Amount" >
                            <ItemTemplate>
                                <asp:TextBox ID="TxtFine" runat="server"  MaxLength="8" class="form-control"
                                    Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="TxtFine_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="TxtFine" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Fine Date">
                            <ItemTemplate>
                     <asp:DropDownList ID="Drp_FineDate" Width="150px" class="form-control" runat="server">
                     <asp:ListItem Value="1" Text="Last Date"></asp:ListItem>
                     <asp:ListItem Value="2" Text="Due Date"></asp:ListItem>
                     </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Fine Type" >
                            <ItemTemplate>
                       <asp:DropDownList ID="Drp_FineType" Width="150px" runat="server" class="form-control" AutoPostBack="true"
                        OnSelectedIndexChanged="Drp_FineType_SelectedIndexChanged">
                     <asp:ListItem Value="1" Text="Fixed"></asp:ListItem>
                     <asp:ListItem Value="2" Text="Percentage"></asp:ListItem>
                     <asp:ListItem Value="3" Text="Fixed Increment"></asp:ListItem>
                     <asp:ListItem Value="4" Text="Percentage Increment"></asp:ListItem>
                     </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>    
                              <asp:TemplateField HeaderText="Fine Duration" >
                            <ItemTemplate>
                     <asp:TextBox ID="TxtDuration" runat="server"  MaxLength="8" class="form-control" Enabled="false"
                                    Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="TxtDuration_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="TxtDuration" >
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                    
                    </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                            HorizontalAlign="Left" />
                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                </asp:GridView>
                
               
              
           
                </td>
                  
                </tr>
                </table>
                </td>
                <td class="e" style="height: 149px"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
</div>   

            
            </td>
            </tr>
            </table>
            
             <WC:MSGBOX id="WC_MessageBox" runat="server" />     


<div class="clear"></div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
