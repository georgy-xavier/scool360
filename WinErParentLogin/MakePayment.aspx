<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MakePayment.aspx.cs" Inherits="WinErParentLogin.MakePayment" %>
<%@ PreviousPageType VirtualPath="~/FeeDetails.aspx" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
html { width: 100%; height:100%; overflow:hidden; }


body { 



     background-color:White;
     
     -webkit-background-size: cover;
      -moz-background-size: cover;
      -o-background-size: cover;
  
    
}
.login { 
	position: absolute;
	top: 40%;
	left: 44%;
	margin: -150px 0 0 -150px;
	width:500px;
	height:500px;
	BorderColor:# 808080;
	BorderWidth :2px;
}
.login h1 { color:#008B8B; text-align:center; }
    </style>
        <style type="text/css">

        .button-success,
        .button-error,
        .button-warning,
        .button-secondary {
            color: white;
            border-radius: 4px;
            text-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
        }

        .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }

        .button-error {
            background: rgb(202, 60, 60); /* this is a maroon */
        }

        .button-warning {
            background: rgb(223, 117, 20); /* this is an orange */
        }

        .button-secondary {
            background: rgb(66, 184, 221); /* this is a light blue */
        }
        
        fieldset { border:4px solid #808080 ;
                   	position: absolute;
	top: 40%;
	left: 40%;
	margin: -150px 0 0 -150px;
	
	}

legend {
  padding: 0.2em 0.5em;
  border:2px solid #808080;
  color:#008B8B;
  font-size:90%;
  text-align:right;
  
  }
  TextBox {
	border:2px solid #456879;
	border-radius:10px;
	height: 22px;
	width: 200px;
}

.container{
    position: relative;
}
#destination{
    position: absolute;
    top: 0;
    left: 0;
}

    </style>
    
</head>
<body>
<form id="form1" runat="server" method="post">
    
    <div id ="frmError" runat="server">
      
      <br/>
      </div>
      
    
   <input type="hidden" runat="server" id="key" name="key" />
   <input type="hidden" runat="server" id="hash" name="hash"  />
   <input type="hidden" runat="server" id="txnid" name="txnid" />
   
  <fieldset>
  <legend>Make A Payment</legend>
 <table style="margin-left:5%;">

         <tr>
         
         <td align="right"> </td>
          <td align="left">
           </td>
        </tr>
         
           <tr>
          <td align="right">
          <asp:Label ID="lbl1" runat="server" Text="Fees Header Group Name :"></asp:Label>
           </td>
          <td align="left">
          <asp:Label ID="lbl_Group_Name" runat="server" Text="" ForeColor="#1E90FF"></asp:Label></td>
         
        </tr>
     <%--     <tr>
          <td align="right">
          <asp:Label ID="Label1" runat="server" Text=" Total Amount :"></asp:Label> </td>
          <td align="left">
          <asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/currency_dollargreen.png"
                        Height="20px" Width="20px" /><asp:Label ID="lbl_Amount" runat="server" Text="" ForeColor="#1E90FF"></asp:Label>
          </td>
         
        </tr>--%>
         <tr>
          <td align="right"><asp:Label ID="Label2" runat="server" Text=" Total Amount "></asp:Label>
          <asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/currency_dollargreen.png"
                        Height="20px" Width="20px" /> :<asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
          </td>
          <td align="left">
          <div class="container">
              <asp:TextBox ID="amount" runat="server"/>
            <div id="destination">
         <asp:TextBox ID="Hide_Amount" runat="server" Enabled="false"/>
            <%--<asp:Label ID="Hide_Amount" runat="server" Text="" ForeColor="Green"></asp:Label>--%>
         </div>
          </div>
          </td>
          </tr>
             <tr>
          <td align="right"> <asp:Label ID="Label3" runat="server" Text="Payment For :"></asp:Label><asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label></td>
          <td align="left">
           <div class="container">
          <asp:TextBox ID="productinfo" runat="server"  />
              <div id="destination">
         <asp:TextBox ID="Hide_ProductInfo" runat="server" Enabled="false"/>
            <%--<asp:Label ID="Hide_ProductInfo" runat="server" Text="" ForeColor="Green"></asp:Label>--%>
         </div>
          </div>
          </td>
        </tr>
          <tr>
          <td align="right">First Name: <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label></td>
          <td align="left">
           <asp:TextBox ID="firstname" runat="server" /></td>
            <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBox" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom"
                    ValidChars=" " TargetControlID="firstname"></ajaxToolkit:FilteredTextBoxExtender>--%>
        </tr>
        <tr>
          <td align="right">Email: <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label></td>
          <td align="left">
           <asp:TextBox ID="email" runat="server" />
           <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="email"  EnableClientScript="true"  
                                   SetFocusOnError="true" 
                                ValidationGroup="MandtryValdn" ForeColor="Red"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="Please enter valid email" />
           </td>
           </tr>
           <tr>
          <td align="right">Phone No: <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label></td>
          <td align="left">
          <asp:TextBox ID="phone" runat="server" />
          <%-- <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PhNo_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="phone">
                    </ajaxToolkit:FilteredTextBoxExtender>--%>
          </td>
        </tr>
      

 <tr>
        <td colspan="2">
        <asp:Panel ID="pnl_optional" runat="server" Visible="false">
        <table>
                <tr>
          <td>Success URI: </td>
          <td colspan="3">
          <asp:TextBox ID="surl" runat="server" /></td>
        </tr>
        <tr>
          <td>Failure URI: </td>
          <td colspan="3">
          <asp:TextBox ID="furl" runat="server" /></td>
        </tr>
        <tr>
          <td>Last Name: </td>
          <td>
          <asp:TextBox ID="lastname" runat="server" /></td>
          <td>Cancel URI: </td>
          <td>
           <asp:TextBox ID="curl" runat="server" /></td>
         
        </tr>
        <tr>
          <td>Address1: </td>
          <td>
            <asp:TextBox ID="address1" runat="server" /></td>
          <td>Address2: </td>
          <td>
          <asp:TextBox ID="address2" runat="server" /></td>
        </tr>
        <tr>
          <td>City: </td>
          <td>
          <asp:TextBox ID="city" runat="server" /></td>
          <td>State: </td>
          <td>
          <asp:TextBox ID="state" runat="server" /></td>
        </tr>
        <tr>
          <td>Country: </td>
          <td>
          <asp:TextBox ID="country" runat="server" /></td>
          <td>Zipcode: </td>
          <td>
            <asp:TextBox ID="zipcode" runat="server" /></td>
        </tr>
        <tr>
          <td>UDF1: </td>
          <td>
           <asp:TextBox ID="udf1" runat="server" /></td>
          <td>UDF2: </td>
          <td>
           <asp:TextBox ID="udf2" runat="server" /></td>
        </tr>
        <tr>
          <td>UDF3: </td>
          <td>
           <asp:TextBox ID="udf3" runat="server" /></td>
          <td>UDF4: </td>
          <td>
           <asp:TextBox ID="udf4" runat="server" /></td>
        </tr>
        <tr>
          <td>UDF5: </td>
          <td>
           <asp:TextBox ID="udf5" runat="server" /></td>
          <td>PG: </td>
          <td>
           <asp:TextBox ID="pg" runat="server" /></td>
        </tr>
		<tr>
		<td>Service Provider: </td>
          <td>
           <asp:TextBox ID="service_provider" runat="server" Text="payu_paisa"/></td>
		</tr>
		</table>
		</asp:Panel>
		</td>
		</tr>
        <tr>
        
            <td colspan="2"></td>
            
        </tr>
        <tr>
        <td align="right">
        
        </td>
        <td align="left">
          <asp:Button ID="submit" Text="Make Payment" class="button-secondary pure-button"  runat="server" OnClick="Button1_Click"  />&nbsp;&nbsp;<asp:Button ID="Btn_Cancel" Text="Cancel" class="button-error pure-button"  runat="server" OnClick="Cancel_Click"  />
        </td>
        </tr>
        <tr>
        <td colspan="2" align="center">
          <br />
                       <asp:Label ID="lbl_msg" runat="server" Text="" ForeColor="Red"></asp:Label>
                   <br />
                   
        </td>
        </tr>
      </table>
        </fieldset>
    </form>
    
    
<%--    <script >

        document.getElementById("amount").disabled = true;
        document.getElementById("productinfo").disabled = true;

</script>--%>
</body>
</html>
