<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinerSchoolSelectionold.aspx.cs" Inherits="WinErParentLogin.WinerSchoolSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>WinEr</title>
     <link rel="shortcut icon" href="images/winerlogo.ico" />
        <link rel="stylesheet" type="text/css" href="css files/Loginstyle.css" media="screen"/>
 <%--      <script type="text/javascript">
           function clearcookies() {
               //dont delete code below. it is used for clearing cookie for year view
               document.cookie ="0;";
           }
        
       </script>--%>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="Updatepanel" runat="server" >
      <ContentTemplate>
      
      </ContentTemplate>
     </asp:UpdatePanel>
    <div class="content">
 
  
   
  <div id="select"   align="center" > 
  
  <div  id ="selectpg">
    <asp:Label ID="Lbl_select" runat="server" Text="Select School" Font-Bold="True" ></asp:Label>
    <br/>
    
      <div style="overflow: auto; height: 300px;width:600px">
           
      <asp:GridView ID="Grd_School" SkinID="GrayGrid" runat="server" AutoGenerateColumns="False"
     CellPadding="4" Font-Size="11px"    Width="95%" 
    DataKeyNames="Id" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
    onselectedindexchanged="Grd_School_SelectedIndexChanged" ForeColor="Black" 
              GridLines="Vertical" >
    
    <EditRowStyle Font-Size="Medium" />
     <Columns>
          <asp:BoundField DataField="Id" HeaderText="Id" />
 <%--         <asp:BoundField DataField="LogoUrl" HeaderText="LogoUrl" Visible="false"/> --%>
         <%-- <asp:TemplateField HeaderText=""  HeaderStyle-Width="100px"  >
              <ItemTemplate>
              <img  style="border:0;" alt="" src="ThumbnailImages/<%# Eval("LogoUrl")%>"  height="35px" width="40px" />
              </ItemTemplate>
           <HeaderStyle Width="100px"></HeaderStyle>
           </asp:TemplateField>--%>
           <asp:BoundField DataField="SchoolName" HeaderText="School" /> 
             <asp:CommandField  SelectText="&lt;img src='Pics/hand.png' width='30px' border=0 title='Select College'&gt;"  
                            ShowSelectButton="True" >
                            <ItemStyle Width="35px" />
                            </asp:CommandField>
    </Columns>
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" Font-Size="Medium"   HorizontalAlign="Center" />
    <FooterStyle BackColor="#CCCC99" />
    <RowStyle BackColor="#F7F7DE" BorderColor="Olive" Font-Size="Small" Height="35px"/>
          <AlternatingRowStyle BackColor="White" CssClass="" />
</asp:GridView>
</div>
      </div>
  </div>
<div id="footer">
    

Developed by : <a href="http://www.winceron.com">Winceron Software Technologies Pvt. Ltd.</a> 
        
    </div>
    </div>
    </form>
    
</body>
</html>


