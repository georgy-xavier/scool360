<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tc" Codebehind="Tc.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TC</title>
    <style type="text/css">
       
        .table1
        {
            
            width: 76%;
        }
        
        .style1
        {
        }
        
        
        .style2
        {
            height: 26px;
        }
        .style3
        {
            height: 25px;
        }
        
        
    </style>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
    <div>
    &nbsp;&nbsp;
    <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderWidth="2px">
       <table style="height: 140px">
        <tr>
            <td class="style6" rowspan="4">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="Img_Logo" runat="server" Height="120px" 
                    Width="125px" />
            </td>
            <td class="style11">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Lbl_TCNUmber" runat="server" Text="Tc No:"></asp:Label>
                <asp:Label ID="Lbl_TcNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style11" align="center">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Lbl_SchoolName" runat="server" Height="24px" 
                    Font-Bold="True" Font-Size="X-Large" ForeColor="#003366"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style12" align="center">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Lbl_Address" runat="server" Font-Bold="True" 
                    Font-Size="Medium" ForeColor="#003366"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style12" align="center">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Lbl_TC" runat="server" Text="TRANSFER CERTIFICATE " 
                    Width="242px" Font-Bold="True" Font-Size="Large" ForeColor="#003366" 
                    Height="24px" Font-Underline="True"></asp:Label>
            
                </td>
        </tr>
        </table>

   <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderWidth="1px">
   <table class="table1"  >
       <tr>
           <td class="style22">
               1</td>
           <td class="style3">
               Name of the School</td>
           <td class="style45">
               <asp:Label ID="Lb_SchoolName" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style22" >
               2</td>
           <td class="style3">
               Admission No</td>
           <td class="style1">
               <asp:Label ID="Lbl_AdmissionNo" runat="server" 
                   Font-Underline="True" ForeColor="#0066FF"></asp:Label>
               &nbsp;&nbsp;&nbsp;
           </td>
       </tr>
       <tr>
           <td class="style22" >
               3 </td>
           <td class="style3">
               Cumulative Record No&nbsp;</td>
           <td class="style1" >
               <asp:Label ID="Lbl_Cumulative" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style22" >
               4</td>
           <td class="style3">
               Name of the pupil</td>
           <td class="style45">
               <asp:Label ID="Lbl_NameOfPupil" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style23">
               5</td>
           <td class="style3">
               Sex</td>
           <td class="style46">
               <asp:Label ID="Lbl_Sex" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style23">
               6</td>
           <td class="style3">
               Name of Father</td>
           <td class="style46">
               <asp:Label ID="Lbl_FatherName" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style23">
               7</td>
           <td class="style3">
               Date of birth</td>
           <td class="style46">
               <asp:Label ID="Lbl_Dob" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderWidth="1px">
       <table style="width: 758px" >
       <tr>
           <td class="style22">
               8</td>
           <td class="style3">
               Nationality&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
           <td>
               <asp:Label ID="Lbl_Nationality" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           </td>
           <td>
               9&nbsp;&nbsp;&nbsp; Religion&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Label ID="Lbl_Religion" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               &nbsp;&nbsp;
           </td>
       </tr>
       <tr>
           <td class="style22">
               10</td>
           <td class="style3">
               Caste&nbsp;</td>
           <td>
               <asp:Label ID="Lbl_Cast" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           </td>
           <td>
               11&nbsp; SC or ST&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Label ID="Lbl_CastType" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       </table>
       </asp:Panel>
     <br />
        <asp:Panel ID="Panel4" runat="server" BorderColor="Black" BorderWidth="1px">
       <table  >
       <tr>
           <td valign="top" class="style2">
               12</td>
           <td colspan="2" class="style2">
               Standard at the time of leaving the school</td>
           <td class="style2">
               <asp:Label ID="Lbl_CurrStd" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               </td>
       </tr>
       <tr>
           <td class="style3" valign="top">
               13</td>
           <td class="style2" colspan="2" valign="top">
               For&nbsp; higher standard Pupil :Language studied
           </td>
           <td class="style3">
               <asp:Label ID="Lbl_LanStd" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style24">
               14</td>
           <td class="style2">
               Medium of Instruction</td>
           <td class="style1" colspan="2" >
               <asp:Label ID="Lbl_MediumOfIns" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               &nbsp;&nbsp; 15&nbsp; Syllabus&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Label ID="Lbl_Syllabus" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           </td>
       </tr>
       <tr>
           <td class="style24">
               16</td>
           <td class="style2" colspan="2">
               Date of Admission </td>
           <td class="style36" >
               <asp:Label ID="Lbl_DteOFAdmission" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style24">
               17</td>
           <td class="style2" colspan="2">
               Whether qualified for Promotion to a higher standard</td>
           <td class="style36" >
               <asp:Label ID="Lbl_QualiFoePro" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style27">
               18</td>
           <td class="style2" colspan="2">
               Whether the pupil has paid all the fees due to the School</td>
           <td class="style37">
               <asp:Label ID="Lbl_FeesDue" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td valign="top" class="style27">
               19</td>
           <td class="style2" colspan="2" valign="top">
               Fee concessions if any (Nature &amp; period to be specified)</td>
           <td class="style19">
               <asp:Label ID="Lbl_FeeConcession" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               </td>
       </tr>
       <tr>
           <td class="style27" valign="top">
               20</td>
           <td class="style2" colspan="2" valign="top">
               Scholarships, if any&nbsp; (Nature &amp; period to be specified)</td>
           <td class="style19">
               <asp:Label ID="Lbl_Scholarship" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style27">
               21</td>
           <td class="style2" colspan="2">
               Whether medically Examined or not</td>
           <td class="style37">
               <asp:Label ID="Lbl_MedicalyExmnd" runat="server" 
                   Font-Underline="True" ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style40">
               21</td>
           <td colspan="2" class="style2">
               Date of pupil&#39;s last attendance at school</td>
           <td class="style42">
               <asp:Label ID="Lbl_LastAttendance" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style40">
               22</td>
           <td class="style2" colspan="2">
               Date on which the application for the T C was received</td>
           <td class="style42">
               <asp:Label ID="Lbl_TcRecvdDate" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style27">
               23</td>
           <td class="style2" colspan="2">
               Date of issue of Transfer certificate</td>
           <td class="style37">
               <asp:Label ID="Lbl_TCIssue" runat="server" 
                   Font-Underline="True" ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style24">
               24</td>
           <td class="style2" colspan="2">
               No of School days up to the date of leaving</td>
           <td class="style36">
               <asp:Label ID="Lbl_TotalDays" runat="server" 
                   Font-Underline="True" ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
       <tr>
           <td class="style27">
               25</td>
           <td class="style2" colspan="2">
               No of school days the pupil attended</td>
           <td class="style37">
               <asp:Label ID="Lbl_AttendedDays" runat="server" Font-Underline="True" 
                   ForeColor="#0066FF"></asp:Label>
               </td>
       </tr>
       <tr>
           <td class="style27" valign="top">
               26</td>
           <td class="style2" colspan="2" valign="top">
               Character and Conduct</td>
           <td class="style37">
               <asp:Label ID="Lbl_CC" runat="server" Font-Underline="True" ForeColor="#0066FF"></asp:Label>
           </td>
       </tr>
           <tr>
               <td class="style27" valign="top">
                   &nbsp;</td>
               <td class="style14" colspan="2" valign="top">
                   &nbsp;</td>
               <td class="style37">
                   &nbsp;</td>
           </tr>
           <tr>
               <td class="style27" valign="top">
                   &nbsp;</td>
               <td class="style14" colspan="2" valign="top">
                   &nbsp;</td>
               <td class="style37">
                   &nbsp;</td>
           </tr>
           <tr>
               <td class="style27" valign="top">
                   &nbsp;</td>
               <td class="style14" colspan="2" valign="top">
                   &nbsp;</td>
               <td class="style37">
                   &nbsp;</td>
           </tr>
           <tr>
               <td class="style27" valign="top">
                   &nbsp;</td>
               <td class="style14" colspan="2" valign="top">
                   <asp:Label ID="Label1" runat="server" 
                       Text="Signature of the Head of the Institution"></asp:Label>
               </td>
               <td class="style37">
                   <asp:Label ID="Label2" runat="server" Text="SCHOOL SEAL"></asp:Label>
               </td>
           </tr>
        </table>
        </asp:Panel>
       

</asp:Panel>
    
    
    
    </div>
    </form>
</body>
</html>
