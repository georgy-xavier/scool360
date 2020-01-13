<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ClasswiseMarkSheet.aspx.cs" Inherits="WinEr.ClasswiseMarkSheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents" >

      

            <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
    
            </ajaxToolkit:ToolkitScriptManager>  

<center>

<div class="container skin1" style="width:500px" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> 
                     <img alt="" src="Pics/evolution-tasks.png" width="35" height="35" /></td>
				<td class="n" align="left">Classwise Mark Entry Sheet</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				


<div style="min-height:150px">
 <asp:Panel ID="Pnl_ExamList" runat="server" >
 <br />
  <table width="100%" cellspacing="10">
   <tr>
    <td align="right" style="width:50%">
    
     Select Class : 
    
    </td>
    <td align="left">
     
        <asp:DropDownList ID="DropDownClass" runat="server" class="form-control" Width="180">
        </asp:DropDownList>
     
    </td>
   </tr>
   
   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
   
   
   <tr>
   <td></td>
    <td align="left">
        <asp:Button ID="Btn_Generate" runat="server" Text="Generate" 
            Class="btn btn-primary" onclick="Btn_Generate_Click" />
    </td>
   </tr>
   <tr>
    <td colspan="2" align="center">
      
        <asp:Label ID="lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
      
    </td>
   </tr>
  </table>
 
 
   <%--<table width="100%" cellspacing="0">
     <tr>
      <td align="center" valign="middle" style="border:solid 2px gray;height:40px;font-size:20px;font-weight:bolder;color:Black">
         Gurukula HSS School
      </td>
     </tr>
     <tr>
      <td style="border:solid 2px gray;"> 
       
       <table width="100%" cellspacing="0">
        <tr>
         <td  align="center" valign="middle" style="width:33%;border:solid 1px gray;height:30px">
             Class : <b>III A </b>
         </td>
         <td  align="center" valign="middle" style="width:33%;border:solid 1px gray;height:30px">
             Exam : <b>___________</b>
         </td>
         
         <td  align="center" valign="middle" style="width:33%;border:solid 1px gray;height:30px">
             Date : <b>___________</b>
         </td>
        </tr>
        <tr>
         <td  align="center" valign="middle" style="width:33%;border:solid 1px gray;height:30px">
             Subject : <b>___________</b>
         </td>
         <td  align="center" valign="middle" style="width:33%;border:solid 1px gray;height:30px">
             Teacher : <b>___________</b>
         </td>
         <td  align="center" valign="middle" style="width:33%;border:solid 1px gray;height:30px">
             Max Mark : <b>___________</b>
         </td>
        </tr>
       </table>
       
      </td>
     </tr>
     <tr>
      <td style="border:solid 2px gray;">
         
         <table width="100%" cellspacing="0">
          <tr>
           <td   align="center" valign="middle"  style="height:30px;width:10%;background-color:Gray;color:White;font-weight:bolder">
            
             ROLL NO
            
           </td>
           <td valign="middle"  style="height:30px;width:65%;background-color:Gray;color:White;font-weight:bolder">
             
               &nbsp;&nbsp;&nbsp; STUDENT NAME
             
           </td>
           <td   align="center" valign="middle"  style="height:30px;width:25%;background-color:Gray;color:White;font-weight:bolder">
           
            MARKS
           
           </td>
          </tr>
          <tr valign="middle">
           <td align="center" style="height:25px;border:solid 1px gray">
             1
           </td>
            <td style="height:25px;border:solid 1px gray">
                &nbsp;&nbsp; ABIN THOMAS
           </td>
            <td  align="center"  style="height:25px;border:solid 1px gray">
           
           </td>
          </tr>
           <tr valign="middle">
           <td align="center" style="height:25px;border:solid 1px gray">
             2
           </td>
            <td style="height:25px;border:solid 1px gray">
                &nbsp;&nbsp; AKHIL MATHEW
           </td>
            <td  align="center"  style="height:25px;border:solid 1px gray">
           
           </td>
          </tr>
           <tr valign="middle">
           <td align="center" style="height:25px;border:solid 1px gray">
             3
           </td>
            <td style="height:25px;border:solid 1px gray">
                &nbsp;&nbsp; ANISH GEORGE
           </td>
            <td  align="center"  style="height:25px;border:solid 1px gray">
           
           </td>
          </tr>
           <tr valign="middle">
           <td align="center" style="height:25px;border:solid 1px gray">
             4
           </td>
            <td style="height:25px;border:solid 1px gray">
                &nbsp;&nbsp; ARUN SUNNY
           </td>
            <td  align="center"  style="height:25px;border:solid 1px gray">
           
           </td>
          </tr>
           <tr valign="middle">
           <td align="center" style="height:25px;border:solid 1px gray">
             
           </td>
            <td align="right" style="height:25px;border:solid 1px gray;color:Black;font-weight:bolder">
                 GRAND TOTAL &nbsp;&nbsp;
           </td>
            <td  align="center"  style="height:25px;border:solid 1px gray">
           
           </td>
          </tr>
         </table>
         
      </td>
     </tr>
     <tr>
      <td align="right" valign="bottom" style="border:solid 2px gray;height:50px;">
       
          Signature&nbsp;&nbsp;&nbsp;&nbsp;
      </td>
     </tr>
   </table>--%>
 
</asp:Panel>
</div>
					
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
 
</center>  
        
        <div class="clear">
        </div>
    </div>
</asp:Content>
