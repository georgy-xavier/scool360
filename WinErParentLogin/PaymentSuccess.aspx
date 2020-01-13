<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentSuccess.aspx.cs" Inherits="WinErParentLogin.PaymentSuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
        <style type="text/css">
        .style2
        {
            width: 42px;
        }
        .style3
        {
            height: 22px;
        }
                fieldset { border:4px solid #808080 ;
                   	


	
	}
                legend {
  padding: 0.2em 0.5em;
  border:2px solid #808080;
  color:#008B8B;
  font-size:90%;
  text-align:right;
  
  }

.container{
    position: relative;
}
#destination{
    position: absolute;
    top: 0;
    left: 0;
}

.myButton {
background-color:#ed8223;
	color:#fff;
	font-family:'Helvetica Neue',sans-serif;
	font-size:18px;
	line-height:30px;
	border-radius:20px;
	-webkit-border-radius:20px;
	-moz-border-radius:20px;
	border:0;
	text-shadow:#C17C3A 0 -1px 0;
	width:120px;
	height:32px

}

    </style>
</head>
<body>
    <form id="form1" runat="server">
       <fieldset>
  <legend><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/currency_dollargreen.png"
                        Height="20px" Width="20px" />&nbsp<asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Transaction Success"></asp:Label></legend>
                   <table width="100%">
                   <tbody>
                   <tr>
                   <td class="style2" colspan="2" align="right">
                       <asp:Button ID="Btn_Back" runat="server" CssClass="myButton" Text="Back" OnClick="Btn_Back_Click" />&nbsp;&nbsp
                   </td>
                   </tr>
                   <tr>
                   <td class="style2" rowspan="2">
                   <asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/accept.png"
                        Height="39px" Width="39px" />
                   </td>
                        <td style="color:Green;font-style:oblique;font-weight:bold;font-size:larger" 
                           class="style3">
                        YOUR TRANSACTION HAS BEEN SUCCESS!
                        </td>
                   </tr>
                   <tr>
                   <td></td>
                   </tr>
                   <tr>
                   <td></td>
                   <td style="font-weight:bold">
                  <asp:Label ID="Lbl_TransName" runat="server" Text="Transaction ID:"></asp:Label>  <asp:Label ID="lblTransactionId" runat="server" Text=""></asp:Label>
                   </td>
                   </tr>
                   <tr>
                   <td></td>
                   <td><asp:Label ID="LblTransref" runat="server" Text="Save transaction id for future reference:"></asp:Label></td>
                   </tr>
                   <tr>
                   <td>
                   </td>
                   <td>
                       <asp:Label ID="Lbl_FeeBillMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                   </td>
                   </tr>
                   </tbody>
                   </table>
                   <br />
                   <center>
                   <asp:Panel ID="Pnl_FeeDetails" runat="server">
                

                  
                   
                    <asp:GridView  ID="Grd_Feetopay" runat="server" BackColor="White" 
                        AutoGenerateColumns="False" 
                           BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="4" 
                           ForeColor="Black" GridLines="Vertical" Width="100% "
                          >
                           
                           <Columns> 
                        <asp:BoundField DataField="Fee_Name" HeaderText="Fee Name" />
                        <asp:BoundField DataField="BatchName" HeaderText="Batch" />
                        <asp:BoundField DataField="Period" HeaderText="For Period" />
                       
                        <asp:BoundField DataField="Amount" HeaderText="Amount" /> 
                        <asp:BoundField DataField="Fine" HeaderText="Fine" />  
                         <asp:BoundField DataField="Status" HeaderText="Status" />                                                      
                            </Columns>

                      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Height="25px" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                       </asp:GridView>
                      
               
               </asp:Panel>
                   </center>
       </fieldset>      

    </form>
</body>
</html>
