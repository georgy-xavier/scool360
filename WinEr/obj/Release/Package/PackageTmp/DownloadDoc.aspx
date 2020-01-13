<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="DownloadDoc.aspx.cs" Inherits="WinEr.DownloadDoc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet"><ProgressTemplate>                               
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /> <br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>          
    </ProgressTemplate></asp:UpdateProgress>
     <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="container skin1">
                <table cellpadding="0" cellspacing="0" class="containerTable">
                    <tr><td class="no"></td>
                        <td class="n"></td>
                        <td class="ne"></td>
                    </tr>
                    <tr>
                        <td class="o"></td>
                        <td class="c">
                            <div align="center">
                                <table width="100%">
                                    <tr>
                                        <td  align="right">Select Class</td>
                                        <td  align="left">
                                            
                                          <asp:DropDownList ID="Drp_class" runat="server" AutoPostBack="true" class="form-control" Width="200px" OnSelectedIndexChanged ="Drp_SelectClass_SelectedIndexChanged">
                                              </asp:DropDownList>

                                           
                                        </td>
                                    </tr>
                                       
                                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                    
                                    <tr>
                                        <td  align="right">Select Student</td>
                                        <td  align="left">
                                           <asp:DropDownList ID="Drp_Student" runat="server" AutoPostBack="true" class="form-control" Width="200px" OnSelectedIndexChanged ="Drp_Student_SelectedIndexChanged">
                                              </asp:DropDownList>

                                        </td>
                                    </tr>
                                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                     <tr>
                                        <td  align="right">Select Document</td>
                                        <td  align="left">
                                           <asp:DropDownList ID="Drp_Doc" runat="server" class="form-control" Width="200px">
                                              </asp:DropDownList>

                                        </td>
                                    </tr>
                                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                  
                                     <tr>
                                        <td></td>
                                        <td  align="left"><asp:Button ID="Btn_ShowDoc" runat="server" Text="Download Document" class="btn btn-primary" OnClick="Btn_ShowDoc_Click"/></td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" align="center"><asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>                                                
                        </td>
                        <td class="e"></td>
                    </tr>
                    <tr>
                        <td class="so"></td>
                        <td class="s"></td>
                        <td class="se"></td>
                    </tr>
                </table>
            </div>		                     
        </ContentTemplate> 
         <Triggers><asp:PostBackTrigger ControlID="Btn_ShowDoc" />
   <%--  <asp:PostBackTrigger ControlID="Drp_SelectList" />
      <asp:PostBackTrigger ControlID="Drp_Class" />--%>
    </Triggers>  
    </asp:UpdatePanel>
</asp:Content>
