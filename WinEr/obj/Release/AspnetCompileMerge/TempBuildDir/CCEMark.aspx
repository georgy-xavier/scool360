<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEMark.aspx.cs" Inherits="WinEr.CCEMark" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX"  Src="~/WebControls/MsgBoxControl.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function Calculate() {

        
        var Txt_Maxmark = document.getElementById('<%=Txt_Maxmark.ClientID%>');
        var _MaxMark = parseFloat(Txt_Maxmark.value);
       
        var _GrandTotal = 0;
        var gridViewCtl = document.getElementById('<%=Grd_CCEMark.ClientID%>');
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var Tx_Mark = gridViewCtl.rows[i].cells[2].children[0];
            Tx_Mark.style.backgroundColor = 'White';
            Tx_Mark.title = "Enter Mark";
            var _parsed_value = 0;
            if ((Tx_Mark.value != "") && (Tx_Mark.value != "a") && (Tx_Mark.value != "A")) {
                _parsed_value = parseFloat(Tx_Mark.value);
            }
            if (_MaxMark < _parsed_value) {
                Tx_Mark.style.backgroundColor = 'Red';
                Tx_Mark.title = "Wrong Mark";
                alert("Please enter mark less than maximum mark");
            }
            _GrandTotal = _GrandTotal + _parsed_value;
        }
       
    }
    function setTextBoxvalue(obj) {

        var txtbox_Name = obj.id;
        var Txt_value = document.getElementById(txtbox_Name);
        if (Txt_value.value == "" || Txt_value.value == null) {
            Txt_value.value = 0;
        }
    }
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
       <div class="container skin1" >
             <table cellpadding="0" cellspacing="0" class="containerTable">
              <tr>
                              <td class="no">
                                  <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
                              </td>
                              <td class="n">
                                  CCE Mark Entry</td>
                              <td class="ne">
                              </td>
                          </tr>
              
              <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                
                
                 <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                 
                     <table class="tablelist">
                     <tr>
                     <td class="leftside">&nbsp;Class Name</td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_Class" class="form-control" runat="server" AutoPostBack="true" 
                             Width="250px" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                     </tr>
                         <caption>
                            
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                             <tr>
                                 <td class="leftside">
                                     &nbsp;Exam Name</td>
                                 <td class="rightside">
                                     <asp:DropDownList ID="Drp_Exam" runat="server" AutoPostBack="true" 
                                         class="form-control" OnSelectedIndexChanged="Drp_Exam_SelectedIndexChanged" 
                                         Width="250px">
                                     </asp:DropDownList>
                                 </td>
                             </tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                             <tr>
                                 <td class="leftside">
                                     &nbsp;Subject Name</td>
                                 <td class="rightside">
                                     <asp:DropDownList ID="Drp_subject" runat="server" AutoPostBack="true" 
                                         class="form-control" OnSelectedIndexChanged="Drp_subject_SelectedIndexChanged" 
                                         Width="250px">
                                     </asp:DropDownList>
                                 </td>
                             </tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                             <tr>
                                 <td class="leftside">
                                     Maximum Mark</td>
                                 <td class="rightside">
                                     <asp:TextBox ID="Txt_Maxmark" runat="server" class="form-control" 
                                         ReadOnly="True" Width="250px"></asp:TextBox>
                                     &nbsp;
                                     <asp:LinkButton ID="Lnk_importmark" runat="server" 
                                         onclick="Lnk_importmark_Click">Import Mark</asp:LinkButton>
                                 </td>
                             </tr>
                             <tr>
                                 <td align="left" colspan="2">
                                     &nbsp;&nbsp;&nbsp;
                                     <asp:Label ID="Label1" runat="server" class="control-label" Font-Bold="true" 
                                         Font-Size="Medium" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                 </td>
                             </tr>
                         </caption>

                     </table>
                     
                     <table class="tablelist" id="GridViewtable" runat="server">
                    
                     <tr>
                     <td class="leftside">
                     </td>
                     <td class="rightside">
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:ImageButton ID="img_export_Excel"  ToolTip="Export to Excel" 
                             ImageUrl="~/Pics/Excel.png" runat="server" 
                             Height="47px" Width="42px" OnClick="img_export_Excel_Click"/>
                     </td>
                     </tr>
                     
                     <tr>
                     <td colspan="2"><hr /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>
                     
                     <tr>
                     <td colspan="2">
                
                     <div style="width:auto;height:250px;overflow:auto;" >
                      <asp:GridView ID="Grd_CCEMark" runat="server" AutoGenerateColumns="false"
                            Width="97%" 
                            BackColor="#EBEBEB" OnRowDataBound="Grd_CCEMark_RowDataBound"
                            BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                            CellPadding="3" CellSpacing="2" Font-Size="12px">
                            <Columns>
                                <asp:BoundField DataField="StudentId" HeaderText="Student Id" />
                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                <asp:BoundField DataField="StudentRollNo" HeaderText="Student RollNo" />
                          
                                
                                <asp:TemplateField HeaderText="Mark" ItemStyle-Width="200">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Mark" runat="server" Height="20" MaxLength="6" Text="0" class="form-control" onkeyup="Calculate();setTextBoxvalue(this)"
                                            Width="100" ></asp:TextBox>
                                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Mark_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="Txt_Mark" ValidChars=".a">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                         <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                     <br />
                     </div>
                     
                    
                     
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>
                     
                     <tr>
                     <td colspan="2"><hr /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"></td>
                     <td class="rightside">
                     </td>
                     </tr>
                     
                     <tr>
                     <td colspan="2" align="center">
                      <asp:Button ID="Btn_Save" runat="server" Text="Update" Class="btn btn-success"
                                     ToolTip="save cce mark" onclick="Btn_Save_Click" />
                          &nbsp;<asp:Button ID="Btn_Clear" runat="server" Text="Clear" Class="btn btn-danger"
                                   ToolTip="clear all cce mark" onclick="Btn_Clear_Click"/>         
                     </td>
                     </tr>
                     
                     
                     </table>
                    
                    <asp:Panel ID="YesOrNoMessageBox" runat="server">
                                                          <asp:Button ID="Btn_header" runat="server" class="btn btn-info" style="display:none" />
                                                          <ajaxToolkit:ModalPopupExtender ID="MPE_yesornoMessageBox" runat="server" 
                                                              PopupControlID="Pnl_yesornomsg"  CancelControlID="Btn_no"
                                                              TargetControlID="Btn_header" />
                                                          <asp:Panel ID="Pnl_yesornomsg" runat="server" style="display:none;">
                                                              <div class="container skin5" style="width:400px; top:600px;left:500px;">
                                                                  <table cellpadding="0" cellspacing="0" class="containerTable">
                                                                      <tr>
                                                                          <td class="no">
                                                                          </td>
                                                                          <td class="n">
                                                                              <span style="color:White">Alert Message</span></td>
                                                                          <td class="ne">
                                                                              &nbsp;</td>
                                                                      </tr>
                                                                      <tr>
                                                                          <td class="o">
                                                                          </td>
                                                                          <td class="c">
                                                                              <div style="text-align:left">
                                                                                  <br />
                                                                                  <table>
                                                                                      <tr>
                                                                                          <td>
                                                                                              <asp:Image ID="Image1" runat="server" Height="28px" 
                                                                                                  ImageUrl="~/elements/alert.png" Width="29px" />
                                                                                          </td>
                                                                                          <td>
                                                                                              <asp:Label ID="Label2" runat="server" Text="0" class="control-label" Visible="false"></asp:Label>
                                                                                          
                                                                                              <asp:Label ID="Lbl_popupmsg" runat="server" class="control-label" Text="YOU WANT TO REGENERATE THE REPORT?"></asp:Label>
                                                                                          </td>
                                                                                      </tr>
                                                                                  </table>
                                                                                  <br />
                                                                              </div>
                                                                              <div style="text-align:center;">
                                                                                  <asp:Button ID="Btn_yes" runat="server" Class="btn btn-info" OnClick="Btn_yes_Click" Text="YES" ToolTip="Remove selected group" />
                                                                                  <asp:Button ID="Btn_no" runat="server" Class="btn btn-info" Text="NO THANKS" 
                                                                                      />
                                                                                  <br />
                                                                                  <br />
                                                                              </div>
                                                                          </td>
                                                                          <td class="e">
                                                                          </td>
                                                                      </tr>
                                                                      <tr>
                                                                          <td class="so">
                                                                          </td>
                                                                          <td class="s">
                                                                          </td>
                                                                          <td class="se">
                                                                          </td>
                                                                      </tr>
                                                                  </table>
                                                              </div>
                                                          </asp:Panel>
                                                      </asp:Panel>
                     
                 </asp:Panel>
                     
            
                <br />
               
                </td>
                <td class="e"> </td>
              </tr>
                              
              <tr>
                              <td class="so">
                              </td>
                              <td class="s">
                              </td>
                              <td class="se">
                              </td>
                          </tr>
             </table>
       </div> 
       <WC:MSGBOX  ID="WC_MessageBox" runat="server" />
       </ContentTemplate>
       <Triggers>
       <asp:PostBackTrigger ControlID="img_export_Excel" />
       </Triggers>
    </asp:UpdatePanel>  
    
</div>
</asp:Content>
