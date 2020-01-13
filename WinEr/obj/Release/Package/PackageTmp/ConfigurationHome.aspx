<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" Inherits="ConfigurationHome"  Codebehind="ConfigurationHome.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
       
        .style2
        {
            
        }
        .style3
        {
            
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">About Scool360</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  <asp:Panel ID="Panel1" runat="server" 
        Height="302px">
        
                      <table class="tablelist">
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Version:</td>
                              <td class="rightside">
                                  <b>3.0 Beta</b></td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Platform:</td>
                              <td class="rightside">
                                 <b> Windows</b></td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Year:</td>
                              <td class="rightside">
                                 <b> 2009</b></td>
                          </tr>
                          <tr>
                              <td class="leftside">
                                  Company:</td>
                              <td class="rightside">
                                  <b>Narayan Solutions</b></td>
                          </tr>
                      </table>
        
    
    </asp:Panel>
                  
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


<div class="clear"></div>
</div>
</asp:Content>

