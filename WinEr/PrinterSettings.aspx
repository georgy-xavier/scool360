<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" CodeBehind="PrinterSettings.aspx.cs" Inherits="WinEr.PrinterSetUp" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
            <table width="100%">
            <tr>
            <td style="width:270px" ></td>
            <td>
                <div id="tbldiv" runat="server" style="width:60%;">
 <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> <img alt="" src="Pics/configure1.png" width="30" height="30" /></td>
                <td class="n">Fee Configuration</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o" style="height: 149px"> </td>
                <td class="c" style="height: 149px" >
                <br />
                <table class="tablelist">
                    <tr>
                         <td class="leftside">Bill Type :</td>
                         <td class="rightside">
                         <asp:DropDownList ID="Drp_BillType" runat="server" class="form-control" Width="180px"></asp:DropDownList>
                          </td>
                          </tr>
                    <tr>
                         <td class="leftside">&nbsp;</td>
                         <td class="rightside">
                             &nbsp;</td>
                          </tr>
                          <tr>
                              <td class="leftside">Bill Prefix :</td>
                         <td class="rightside">
                             <asp:TextBox ID="txt_billprefix" runat="server" class="form-control" Width="180px"></asp:TextBox>
                             </td>
                             </tr>
                          <tr>
                              <td class="leftside">&nbsp;</td>
                         <td class="rightside">
                             &nbsp;</td>
                             </tr>
                          <tr>
                              <td class="leftside">Bill Clearance :</td>
                         <td class="rightside">
                             <asp:CheckBox ID="chk_Clrcheque" runat="server" Text="Cheque Clearance" />&nbsp;&nbsp;&nbsp;
                             <asp:CheckBox ID="chk_Clrdd" runat="server" Text="DD Clearance" />
                             </td>
                             </tr>
                          <tr>
                              <td class="leftside">&nbsp;</td>
                         <td class="rightside">
                             &nbsp;</td>
                             </tr>
                          <tr>
                              <td class="leftside">Fee Schedule:</td>
                         <td class="rightside">
                             <asp:CheckBox ID="Chk_SchAllowforNextBatch" runat="server" 
                                 Text=" Allow For Next Batch" />
                             </td>
                             </tr>
                          <tr>
                              <td class="leftside">&nbsp;</td>
                         <td class="rightside">
                             &nbsp;</td>
                             </tr>
                          <tr>
                              <td class="leftside">Fee Advance Settlement:</td>
                         <td class="rightside">
                             <asp:CheckBox ID="Chk_AutoSettelement" runat="server" 
                                 Text=" Auto Settle while Schedule." />
                              </td>
                             </tr>
                           <tr>
                           <td colspan="2"> &nbsp;</td>
                           </tr>
                          <tr>
                              <td class="leftside">&nbsp;</td>
                         <td class="rightside">
                         
                           <asp:Button ID="Btn_BillPrinter" runat="server" 
                                Text="Save" Class="btn btn-success" onclick="Btn_BillPrinter_Click" ValidationGroup="FeeBillValdn"/>
                             &nbsp;<asp:Button ID="BtnCncl" runat="server" 
                                Text="Cancel" Class="btn btn-danger" onclick="BtnCncl_Click" />
                             
                         </td>
                    </tr>
                  
                    
                  
                </table>
               
                  <br />
                   
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
</asp:Content>

