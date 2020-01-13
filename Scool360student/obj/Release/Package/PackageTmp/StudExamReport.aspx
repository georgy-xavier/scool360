<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudExamReport.aspx.cs" Inherits="Scool360student.StudExamReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Student Exam Report</title>
    <style type="text/css">
        .style1
        {
            width: 100%;
           
        }

        .style4
        {
            
        }
        .style9
        {
           border-style: solid; border-width: thin;
           padding:2px;
        }
        .style19
        {
           border-style: solid; border-width: thin;
           padding:2px;background-color: #ECFBFF;
        }
      
        .rowhead
        {
            font-size:larger;
            color:Black;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Pnl_content" runat="server">
        
    <asp:Panel ID="Pnl_head" runat="server">
        
            <table class="style1" >
                <tr >
                    <td  align="center" >
                        &nbsp;&nbsp;
                        <asp:Image ID="Img_logo" runat="server" Height="120px" Width="120px" />
                    </td>
                    <td class="style4" colspan="3" align="center">
                           
                           <asp:Label ID="Lbl_schoolname" runat="server" Font-Size="XX-Large"></asp:Label><br/>
                           
                           <asp:Label ID="Lbi_subHead" runat="server" Font-Size="Large" 
                            ForeColor="#666666"></asp:Label>
                           &nbsp;</td>
                </tr>
                <tr  >
                   
                    <td  >
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td  >
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                </tr>
                <tr>
                    
                    <td class="style9"  >
                        Student Name</td>
                    <td class="style9">
                        <asp:Label ID="Lbl_StudName" runat="server"></asp:Label>
                    </td>
                    <td class="style9">
                        Batch</td>
                    <td class="style9">
                        <asp:Label ID="Lbl_CurrBatch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style9" >
                        Standard</td>
                    <td class="style9">
                        <asp:Label ID="Lbl_Standard" runat="server"></asp:Label>
                    </td>
                    <td class="style9">
                        Class
                    </td>
                    <td class="style9">
                        <asp:Label ID="Lbl_Class" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Exam
                    </td>
                    <td class="style9">
                        <asp:Label ID="Lbl_exam" runat="server"></asp:Label>
                    </td>
                    <td class="style9">
                        Period</td>
                    <td class="style9">
                        <asp:Label ID="Lbl_Type" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:TextBox ID="Txt_examid" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
                    </td>
                    <td >
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                </tr>
            </table>
        
        <br/>
        </asp:Panel>
     <div runat="server" id="ExmReportDiv">
            <table class="style1">
                <tr>
                    <td style="width:10%">
                        &nbsp;</td>
                    <td >
                        <table class="style1" 
                            style="border: thin solid #808080;">
                            <tr class="rowhead" >
                                <td class="style9">
                                    Subject Name</td>
                                <td class="style9">
                                    Obtained Mark</td>
                                <td class="style9">
                                    Max Mark</td>
                                <td class="style9">
                                    Pass Mark</td>
                                <td class="style9">
                                    Result</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    English</td>
                                <td>
                                    55</td>
                                <td>
                                    100</td>
                                <td>
                                    35</td>
                                <td>
                                    Pass</td>
                            </tr>
                            <tr>
                                <td>
                                    Hindi</td>
                                <td>
                                    60</td>
                                <td>
                                    100</td>
                                <td>
                                    45</td>
                                <td>
                                    Pass</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                        
                        <table class="style1" style="border-style: groove; border-color: #C0C0C0">
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style19">
                                    Grand Total</td>
                                <td class="style19">
                                    155</td>
                                <td class="style19">
                                    Max Total</td>
                                <td class="style19">
                                    200</td>
                            </tr>
                            <tr>
                                <td class="style19">
                                    Avrage</td>
                                <td class="style19">
                                    57.55%</td>
                                <td class="style19">
                                    Result</td>
                                <td class="style19">
                                    Pass</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td class="style19">
                                    Rank</td>
                                <td class="style19">
                                    10</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                        </table>
                        
                        <br/>
                    </td>
                    <td style="width:10%">
                        &nbsp;</td>
                </tr>
            </table>
     </div>
     
     <br/>
     
    </asp:Panel>
    
     <asp:Panel ID="Pnl_errormessage" runat="server">
    
        Exam report cannot be displayed .....................................
    </asp:Panel>
    
    </div>
    </form>
</body>
</html>
