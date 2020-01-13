<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateidCard.aspx.cs" Inherits="WinEr.CreateidCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
      .IdCard1
      {
           
         
      } 
      .Tdstyle
      {
          color:Black;
          background-color:White;
      }
      .Tdstyle1
      {
          color:Black;
      }
      .TdInfo
      {
          border-color:Black;
          border-top-color:Black;
      }
    </style>
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents" style="min-width:1000px;">

<div id="right">

<div id="sidebar2">
<h2>Student Manager</h2>
<div id="StudentMenu" runat="server">

</div>

</div>




<div class="label">Student Info</div>
<div id="SubStudentMenu" runat="server">
		
 </div>
 <div id="ActionInfo" runat="server">
 
</div>

</div>

<div id="left">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />


 <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n"><span>Create Id card</span></td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   

           

<asp:Panel ID="Panel1" runat="server" BorderColor="Black" 
        >
<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
    <br />
    <br />
    <table class="Tdstyle1">
    <tr>
        <td>
        Student Name
        </td>
        <td>
            <asp:TextBox ID="Txt_StudentName" runat="server" Width="160px" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Select Id Type</td>
        <td>
            <asp:DropDownList ID="Drp_IdType" runat="server" Width="160px" 
                AutoPostBack="True" onselectedindexchanged="Drp_IdType_SelectedIndexChanged">
                <asp:ListItem Selected="True">Horizontal</asp:ListItem>
                <asp:ListItem>Vertical</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    </table>
    <br />
    <div id="IdCard" class="IdCard1">
    <asp:Panel ID="Panel5" runat="server">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <table>
    <tr>
    <td>
    <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderWidth="1px" 
            Height="195px" Width="336px">
    <table class="Tdstyle">
        <tr>
            <td style="background-color: #4CACE6" align="center">
            
                <asp:Image ID="Img_Logo" runat="server" Height="49px" Width="62px" />
            
            </td>
            <td style="background-color: #4CACE6" colspan="2">
                <asp:Label ID="Lbl_SchoolName" runat="server" Font-Bold="True" 
                    Font-Size="Medium" ForeColor="White"></asp:Label>
            </td>
        </tr>
        <tr>
        <td colspan="3" style="background-color: #4CACE6">
            <asp:Label ID="Lbl_Address" runat="server" Font-Bold="True" ForeColor="White" 
                Font-Size="Small"></asp:Label>
        </td>
        </tr>
        <tr>
            <td rowspan="5">
                <asp:Image ID="Img_Student" runat="server" Height="95px" Width="107px" />
            </td>
            <td>
                Name</td>
            <td>
                <asp:Label ID="Lbl_StudentName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                S/D of</td>
            <td>
                <asp:Label ID="Lbl_Parent" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                DOB</td>
            <td>
                <asp:Label ID="Lbl_DOB" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                AdmissionNo
            </td>
            <td>
                <asp:Label ID="Lbl_AdmissionNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </asp:Panel>
    </td>
    <td>
    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderWidth="1px" 
            Height="195px" Width="336px">
        <table class="Tdstyle">
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Blood Group</td>
                <td>
                    <asp:Label ID="Lbl_BloodGRp" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Address</td>              
                <td>
                    <asp:Label ID="Lbl_AddressBack" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
            <td colspan="2" class="TdInfo">This card is the property of &nbsp;<asp:Label ID="Lbl_School" 
                    runat="server"></asp:Label> &nbsp;In case the card is found, the founder may kindly return to the address mentioned.
                </td>
            </tr>
        </table>
    </asp:Panel>
    </td>
    </tr>
    </table>
    </asp:Panel>
    
    </div>
         <div id="VerticalIDCard" runat="server">
             <asp:Panel ID="Panel6" runat="server">
                 <table class="Tdstyle1">
                    <tr>
                        <td><asp:Panel ID="Panel7" runat="server" Height="335px" Width="215px" 
                                BorderColor="Black" BorderWidth="1px">
                             <table>
                                 <tr>
                                      <td align="center" style="background-color: #4CACE6">
                                          <asp:Image ID="Img_Logo1" runat="server" Height="61px" Width="58px" />
                                      </td>
                                      <td style="background-color: #4CACE6" align="center">
                                          <asp:Label ID="Lbl_Ver_SchoolName" runat="server" Font-Bold="True" 
                                              Font-Size="Medium" ForeColor="White"></asp:Label></td>
                                </tr>
                                <tr>
                                        <td colspan="2" align="center" style="background-color: #4CACE6">
                                            <asp:Label ID="Lbl_Ver_SchoolAddress" runat="server" ForeColor="White"></asp:Label>
                                        </td>
                               </tr>
                              
                                 <tr>
                                     <td align="center" colspan="2">
                                         &nbsp;</td>
                                 </tr>
                              
                                 <tr>
                                     <td align="center" colspan="2">
                                         <asp:Image ID="Img_Student1" runat="server" Height="108px" />
                                     </td>
                                 </tr>
                                 <tr>
                                    <td>Name</td>
                                    <td>
                                        <asp:Label ID="Lbl_Ver_StudName" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         S/D of</td>
                                     <td>
                                         <asp:Label ID="Lbl_Ver_Parent" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         DOB</td>
                                     <td>
                                         <asp:Label ID="Lbl_Ver_DOB" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Ad.No</td>
                                     <td>
                                         <asp:Label ID="Lbl_Ver_AdNo" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                            </table>
                        </asp:Panel></td>
                        <td><asp:Panel ID="Panel4" runat="server" Height="335px" Width="215px" Wrap="False" 
                                BorderColor="Black" BorderWidth="1px">
                                <table>
                                    <tr>
                                        <td>&nbsp;</td> 
                                        <td>
                                            &nbsp;</td>    
                                    </tr>
                                    <tr>
                                        <td>
                                            Blood Group</td>
                                        <td>
                                            <asp:Label ID="Lbl_Ver_BldGrp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>Address</td>
                                    <td>
                                        <asp:Label ID="Lbl_Ver_StdAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            This card is the property of
                                            <asp:Label ID="Lbl_Ver_SchoolName1" runat="server"></asp:Label>
                                            . In case the card is found, the finder may kindly return to the school address 
                                            mentioned</td>
                                       
                                    </tr>
                                    
                                </table>
                            </asp:Panel></td>
                    </tr>
                </table>
             </asp:Panel>
         </div>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Btn_Print" runat="server" onclick="Btn_Print_Click" 
        Text="Print" Width="111px" />
    &nbsp;
    <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Width="111px" 
        onclick="Btn_Cancel_Click" />
    <br />
    <br />
</asp:Panel>



                   
                </td>
                <td class="e"> </td>
            </tr>
            <tr class="bottom">
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>



</div>

<div class="clear"></div>
</div>

</asp:Content>
