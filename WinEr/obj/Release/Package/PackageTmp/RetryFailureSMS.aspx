<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RetryFailureSMS.aspx.cs" Inherits="WinEr.RetryFailureSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
         <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>

    <script language="JavaScript" type="text/javascript">

     var Page;

     var postBackElement;

     function pageLoad()

    {      

      Page = Sys.WebForms.PageRequestManager.getInstance();  

      Page.add_beginRequest(OnBeginRequest);

       Page.add_endRequest(endRequest);

    }

 

    function OnBeginRequest(sender, args)

    {       

      postBackElement = args.get_postBackElement();     

      postBackElement.disabled = true;

    }  

    

     function endRequest(sender, args)

    {      

      postBackElement.disabled = false;

  }

    function CloseWindow() 
    {
        window.close();
    }

    function CancelEdit() {
        window.close();
    }
  </script>
  
    <style type="text/css">
        
        
        body{
         width:570px;
         margin:0 auto;
         position:relative;
         background :#FFF;
         margin-top:20px;
         border-style:outset;
         border-width:thin;
         border-color:Gray;
        }
        
        .GVFixedHeader {  position:relative ; 
        top:auto;
         z-index: 10; 
        }


        
       .modalBackgroundtest {
	    background-color:Black;
	    filter:alpha(opacity=90);
	    opacity:0.7;
      }
      .BottomBorder
      {
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
      }
     .LeftBottomBorder
     {
         border-left-style:solid;
         border-left-width:thin;
         border-left-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
     }
        .RightBottomBorder
     {
         border-right-style:solid;
         border-right-width:thin;
         border-right-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
     }
     #InnerStructure
     {
         border-left-style:solid;
         border-left-width:thin;
         border-left-color:Gray;
         border-right-style:solid;
         border-right-width:thin;
         border-right-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
         width:100%;
         height:200px;
         padding-top:20px;
         
     }
      #heading
      {
        	background-image: url(images/TopStrip.jpg);
        	width:300px;
        	height:30px;
        	color:White;
        	margin-bottom:10px;
      }
      .style1
      {
          border-left: thin solid Gray;
          border-top: thin solid Gray;
          border-bottom: thin solid Gray;
          width: 12%;
      }
      
  </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <div>
    
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
    
     <ProgressTemplate>
                
                
                        <div id="progressBackgroundFilter">

                        </div>

                        <div id="processMessage">

                        <table style="height:100%;width:100%" >

                        <tr>

                        <td align="center">

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
    
</asp:UpdateProgress>
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
<ContentTemplate> 
            
    <asp:Panel ID="Panel1" runat="server" >
        <table width="100%">
           <tr>
             <td>
                <div id="heading">
                   <center>
                      <h3>
                      SMS Failure Phone Numbers
                     </h3>
                   </center>
                  </div>
             </td>
           </tr> 
        </table>
          
         
       
          <table style="font-size:13px; width: 100%;" cellspacing="0">
                 
            <tr>
              <td id="InnerStructure" valign="top" >
              
                 
                     
              
                 <asp:Panel ID="Panel_Retry" runat="server">
                 
                   <table  width="100%" cellspacing="10">

                    <tr>
                    <td style="width:20%;">
                    </td>
                    <td  style="width:60%;"  align="right">
                        <asp:Button ID="Btn_Retry" runat="server" Text="Retry"  Width="100" CssClass="grayempty"
                             onclick="Btn_Retry_Click"/> 
                                
                             &nbsp; 
                        </td>
                        <td style="width:20%;"  align="left">        
                             <asp:Button ID="Btn_Cancel" runat="server" CssClass="grayempty"
                                 Text="Cancel"  OnClientClick="javaScript:window.close(); return false;" />
                                
                      </td>
                    </tr>
                        <tr>
                         <td  align="center" colspan="2">
                             <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red"></asp:Label>
                                
                      </td>
                    </tr>
                    <tr>
                    
                      <td>
                      </td>
                      <td>
                       <div style="height:350px;overflow:auto">
                       
                        <asp:GridView ID="Grd_FailureSMS" runat="server" CellPadding="4" ForeColor="Black"  GridLines="Both" AutoGenerateColumns="False"
                           Width="98%"   BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                              <Columns>
        					            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
        					            <ItemTemplate>
        					                <asp:CheckBox ID="Chk_Select" runat="server" Checked="true" />
        					            </ItemTemplate>
        					            </asp:TemplateField>
                					      
                			             <asp:TemplateField HeaderText="Phone Numbers" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" >
                					     <ItemTemplate  >
                					           <asp:TextBox ID="Txt_Phone" runat="server"  MaxLength="10" Text='<%# Eval("Phone")%>' ></asp:TextBox>
                					           <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ExperienceFilteredTextBoxExtender" 
                                                runat="server" Enabled="True" FilterType="Numbers" 
                                                TargetControlID="Txt_Phone"></ajaxToolkit:FilteredTextBoxExtender>
                      
                                               <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="Txt_Phone" Display="None"  ValidationGroup="Staff"
                                                ErrorMessage="Invalid Mobile No" ValidationExpression="^0|[0-9]{10,12}" />
                                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidextndrMobile" runat="Server" 
                                                 HighlightCssClass="validatorCalloutHighlight" 
                                                 TargetControlID="RegularExpressionValidator1" />
                					     </ItemTemplate>
                					     </asp:TemplateField>
                					     <asp:BoundField DataField="NativeLanguage" HeaderText="Native Language"  />
        					            </Columns>
                            <RowStyle BackColor="White"  HorizontalAlign="Center" Font-Size="Medium"/>
                            <FooterStyle BackColor="#CCCC99" />
                           
                            <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White"  CssClass="GVFixedHeader"
                                HorizontalAlign="Left"/>
                            <AlternatingRowStyle BackColor="White" />
                            
                        </asp:GridView>
                        
                        
                       </div> 
                       </td>
                       <td>
                       
                       </td>
             
                    </tr>
                   </table>
                 
                 </asp:Panel>
                   
              </td>
             </tr>
              
             </table>
                 
                       

 </asp:Panel>
 </ContentTemplate>
 </asp:UpdatePanel>
    
    
    </div>
    
    </div>
    </form>
</body>
</html>
