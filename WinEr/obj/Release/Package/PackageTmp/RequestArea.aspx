<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.Master" AutoEventWireup="True" CodeBehind="RequestArea.aspx.cs" Inherits="WinEr.RequestArea" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">
       
           function openIncpopup(strOpen) {
               open(strOpen, "Info", "status=1, width=700, scrollbars = 0,  height=600,resizable = 1");
           }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="contents">
 <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>  
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
            <ProgressTemplate>  
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">        
            <ContentTemplate>            
                <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"  CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True" >
                    <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Promotion" Visible="true" >
                    <HeaderTemplate><asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>Complaints -<asp:Label ID="lblcomplaintcount" runat="server" Text=""></asp:Label></b></HeaderTemplate>         
                    <ContentTemplate>
                     
                        <asp:GridView ID="Grdcomplaints"  runat="server" AutoGenerateColumns="False" 
                        AllowPaging="true"  Width="100%" PageSize="25"   onpageindexchanging="Grdcomplaints_PageIndexChanging"
 
                        onselectedindexchanged="Grdcomplaints_SelectedIndexChanged"  OnRowDataBound="Grdcomplaints_RowDataBound" >
                        <PagerSettings Position="TopAndBottom" />    
                        <EmptyDataTemplate  > No complaints received</EmptyDataTemplate>  
                        
                        <EmptyDataRowStyle HorizontalAlign="Center" />  
                            <Columns>                                          
                                <asp:BoundField DataField="ThreadId" />           
                                <asp:BoundField DataField="Name" HeaderText="Student" />
                                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                                <asp:BoundField DataField="Description" HeaderText="Last Message" ItemStyle-Wrap="true" />              
                                <asp:BoundField DataField="Date" HeaderText="Date" /> 
                                                                                      
                                <asp:CommandField HeaderText="View Details"  
                                    SelectText="&lt;img src='Pics/hand.png' width='25px' Hight='25px' border=0 title='View Details'&gt;"  ShowSelectButton="True" >
                                    <ItemStyle Width="30px" />
                                </asp:CommandField>                                                                      
                            </Columns>      
                        </asp:GridView>
    
    
                    </ContentTemplate>                  
                </ajaxToolkit:TabPanel>                
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Promotion"  >
                    <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>Requests -<asp:Label ID="lblreqcount" runat="server" Text=""></asp:Label></b></HeaderTemplate>                 
                    <ContentTemplate>
                        <asp:GridView ID="GrdRequests"  runat="server" AutoGenerateColumns="False" 
                            AllowPaging="true"  Width="100%" PageSize="25"   onpageindexchanging="GrdRequests_PageIndexChanging" 
                            onselectedindexchanged="GrdRequests_SelectedIndexChanged"  OnRowDataBound="GrdRequests_RowDataBound" >
                            <PagerSettings Position="TopAndBottom" />  
                                       <EmptyDataTemplate >No requests received</EmptyDataTemplate>
                                          <EmptyDataRowStyle HorizontalAlign="Center" />         
                                <Columns>                                          
                                    <asp:BoundField DataField="ThreadId" />           
                                    <asp:BoundField DataField="Name" HeaderText="Student" />
                                    <asp:BoundField DataField="Subject" HeaderText="Subject" />
                                    <asp:BoundField DataField="Description" HeaderText="Last Message" ItemStyle-Wrap="true" />              
                                    <asp:BoundField DataField="Date" HeaderText="Date" /> 
                                                                                          
                                    <asp:CommandField HeaderText="View Details"  
                                        SelectText="&lt;img src='Pics/hand.png' width='25px' Hight='25px' border=0 title='View Details'&gt;"  ShowSelectButton="True" >
                                        <ItemStyle Width="30px" />
                                    </asp:CommandField>                                                                      
                                </Columns>      
                            </asp:GridView>
                    </ContentTemplate>                  
                </ajaxToolkit:TabPanel>                
                
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Promotion"  >
                <HeaderTemplate><asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>FeedBack -<asp:Label ID="lblfeedbackcount" runat="server" Text=""></asp:Label></b></HeaderTemplate>                 
                    <ContentTemplate>
                        <asp:GridView ID="GrdFeedBack"  runat="server" AutoGenerateColumns="False" 
                            AllowPaging="true"  Width="100%" PageSize="25"   onpageindexchanging="GrdFeedBack_PageIndexChanging" 
                            onselectedindexchanged="GrdFeedBack_SelectedIndexChanged"  OnRowDataBound="GrdFeedBack_RowDataBound" >
                                       <EmptyDataTemplate >No feedbacks received</EmptyDataTemplate>   
                                          <EmptyDataRowStyle HorizontalAlign="Center" />
                            <PagerSettings Position="TopAndBottom" />        
                                <Columns>                                          
                                    <asp:BoundField DataField="ThreadId" />           
                                    <asp:BoundField DataField="Name" HeaderText="Student" />
                                    <asp:BoundField DataField="Subject" HeaderText="Subject" />
                                    <asp:BoundField DataField="Description" HeaderText="Last Message" ItemStyle-Wrap="true" />              
                                    <asp:BoundField DataField="Date" HeaderText="Date" /> 
                                                                                          
                                    <asp:CommandField HeaderText="View Details"  
                                        SelectText="&lt;img src='Pics/hand.png' width='25px' Hight='25px' border=0 title='View Details'&gt;"  ShowSelectButton="True" >
                                        <ItemStyle Width="30px" />
                                    </asp:CommandField>                                                                      
                                </Columns>      
                            </asp:GridView>
                    </ContentTemplate>                  
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:tabcontainer>                
             
            
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
