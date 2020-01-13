<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" Inherits="SchoolDetails"  Codebehind="SchoolDetails.aspx.cs"  ValidateRequest="false" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 683px;
            
        }
        .style2
        {
            width: 137px;
        }
        .style3
        {
        }
        .style4
        {
            width: 137px;
            height: 18px;
        }
        .style6
        {
            width: 270px;
        }
        .style7
        {
            height: 18px;
            width: 270px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  


 <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr class="top">
                <td class="no"><img alt="" src="Pics/Details.png" height="35" width="35" /> </td>
                <td class="n">School Details</td>
                <td class="ne"> </td>
            </tr>
            <tr class="middle">
                <td class="o"> </td>
                <td class="c" >
                <table class="tablelist">
                
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                            &nbsp;</td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            School Name</td>
                        <td >
                            <asp:TextBox ID="Txt_SchoolName" runat="server" Width="344px" class="form-control" MaxLength="250"></asp:TextBox>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                                runat="server" Enabled="True" TargetControlID="Txt_SchoolName" 
                                FilterMode="InvalidChars" InvalidChars="\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                    
                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                                runat="server" Enabled="True" TargetControlID="Txt_SchoolName" WatermarkText="School Name">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                            
                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_WornDays_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_SchoolName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                    
                            <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_SchoolName_TextBoxWatermarkExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_SchoolName" WatermarkText="School Name">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                            
                        </td>
                        <td  rowspan="2" style="width:140px;">
                            <asp:ImageButton ID="Img_Holder" AlternateText="School Logo" runat="server" Height="100px"/>
                            
                        </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            Address</td>
                        <td >
                            <asp:TextBox ID="Txt_address" runat="server" Height="80px" MaxLength="499" class="form-control"
                                TextMode="MultiLine" Width="344px" Wrap="False"></asp:TextBox>
                          
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" 
                                runat="server" Enabled="True" TargetControlID="Txt_address" 
                                FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                          
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" TargetControlID="Txt_address" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            
                        </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            Description</td>
                        <td  class="rightside">
                            <asp:TextBox ID="Txt_Desc" runat="server" Height="150px" MaxLength="800" class="form-control"
                                TextMode="MultiLine" Width="537px" ></asp:TextBox>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" 
                                runat="server" Enabled="True" TargetControlID="Txt_Desc" 
                                FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                           
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                runat="server" Enabled="True" TargetControlID="Txt_Desc" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                                
                           </td>
                         <td>
                             <asp:ImageButton ID="Img_schoolimgHolder" runat="server" Height="100px"/>
                         </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            Syllabus</td>
                        <td colspan="2" class="rightside">
                            <asp:TextBox ID="Txt_Syllabus" runat="server" Width="180px" class="form-control" MaxLength="49"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                runat="server" Enabled="True" TargetControlID="Txt_Syllabus" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                         
                           
                          </td>
                          </tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                          <tr>
                          <td class="leftside">
                           Medium of instruction</td>
                          <td colspan="2" class="rightside">
                            <asp:TextBox ID="Txt_MediumOfIns" runat="server" Width="180px" class="form-control" MaxLength="49"></asp:TextBox>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                runat="server" Enabled="True" TargetControlID="Txt_MediumOfIns" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                        
                           
                    </tr>
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     <tr >
                    <td class="leftside">
                     Native Language</td>
                     <td class="rightside">
                     <asp:DropDownList ID="drplanguages" Width="180px" class="form-control" runat="server"></asp:DropDownList>
                     </td>
                  
                    
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td colspan="2" class="rightside">
                        <table>
                        <tr valign="middle">
                        <td>
                         <asp:ImageButton ID="IMGBtn_UpLogo" runat="server" ImageUrl="Pics/image_icon.png" 
                                Height="50px" Width="50px" onclick="IMGBtn_UpLogo_Click" /> 
                        </td>
                        <td>
                         <asp:LinkButton ID="Lnk_changelogo" runat="server" onclick="Lnk_changelogo_Click">Change Logo</asp:LinkButton>
                        
                        </td>
                        <td></td>
                        <td>
                            <asp:ImageButton ID="IMGBtn_Uploginimg" runat="server" ImageUrl="Pics/image_icon.png"
                                        Height="50px" Width="50px" onclick="IMGBtn_Uploginimg_Click"/>
                        </td>
                        <td>
                            <asp:LinkButton ID="Lnk_changeschoolimg" runat="server" onclick="Lnk_changeschoolimg_Click">Change school image</asp:LinkButton>
                        </td>
                        </tr>
                        </table>
                           
                           
                           </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                         <asp:Label ID="Lbl_Note" runat="server" class="control-label" style="color: #FF0000"></asp:Label>
                       
                          </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                            </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                             <asp:Button ID="Btn_Update" runat="server" Text="Update" 
                                onclick="Btn_Update_Click" Class="btn btn-success" />
                        <input id="Reset1" type="reset" value="Reset"  class="btn btn-danger"/></td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                            <asp:TextBox ID="Txt_TempName" runat="server" class="form-control" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                        <asp:TextBox ID="Txt_TempAddress" runat="server" class="form-control" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td class="leftside">
                            &nbsp;</td>
                        <td class="rightside" colspan="2">
                            &nbsp;</tr>
                      
                  </table>
                
                
                
                
                
               <WC:MSGBOX id="WC_MessageBox" runat="server" />    
  
                
                
                
                    <table width="100%">
                    <tr ><td align="center">
                    <asp:Panel ID="Panel2" runat="server">
                    
                    
                    
                    
                     <asp:Button runat="server" ID="hiddenTargetControlForModalPopup2" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_UploadPhoto" BackgroundCssClass="modalBackground"
                                  runat="server" 
                                  PopupControlID="Pnl_lastInfo" TargetControlID="hiddenTargetControlForModalPopup2" CancelControlID="Btn_CancelLogo"  />
                          <asp:Panel ID="Pnl_lastInfo" runat="server" style="display:none">
                         <div class="container skin5" style="width:600px; top:400px;left:400px">
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr>
            <td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/elements/image-multi.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;font-size:larger">
                <asp:Label ID="lbl_popuptitle" runat="server" class="control-label" Text=""></asp:Label>
            </span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr>
            <td class="o"> </td>
            <td class="c" >
                
                <asp:Panel ID="Pannel3" runat="server"><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="Lbl_UpMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <asp:Label ID="Lbl_hidentxt" runat="server" Text="Label" class="control-label" Visible="false"></asp:Label>    
<br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_uploadlbl" runat="server" class="control-label" Text="Label"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:FileUpload 
        ID="FileUp_Photo1" runat="server" Height="25px" />
   <br />
   <br />
         
         <div style="text-align:center;">
         
          <asp:Button ID="Btn_UploadImage" runat="server" Text="Upload" onclick="Btn_UploadImage_Click" Class="btn btn-info"
        />
             &nbsp;&nbsp;&nbsp;
             <asp:Button ID="Btn_CancelLogo" runat="server" Text="Cancel"  Class="btn btn-info"
                 />
             &nbsp;&nbsp;
                                                
                        </div>

</asp:Panel>
               
                        <br />
                       
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
             
</div>
                       
                        </asp:Panel>               
                     
   
       
                    
         
                    
                    
              
                
                </asp:Panel>  
                    
                    
                    </td></tr>
                    </table>
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



            <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /></td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
                       
                        </asp:Panel> 

                           
                    
                    
                     <asp:Button runat="server" ID="BtnUpdateCon" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_UpdateConfirm" 
                                  runat="server" CancelControlID="Btn_upCancel" 
                                  PopupControlID="Pnl_msgCon" TargetControlID="BtnUpdateCon"  />
                          <asp:Panel ID="Pnl_msgCon" runat="server" style="display:none;">
                             <div class="container skin5" style="width:400px; top:400px;left:400px" >
                                <table   cellpadding="0" cellspacing="0" class="containerTable">
                                    <tr >
                                     <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" Height="28px" Width="29px" /></td>
                                     <td class="n"><span style="color:White">alert!</span></td>
                                     <td class="ne">&nbsp;</td>
                                    </tr>
                                    <tr >
                                     <td class="o"> </td>
                                         <td class="c" >
                                            <asp:Label ID="Lbl_UpdateCon" class="control-label" runat="server" Text=""></asp:Label>
                                            <br /><br />
                                            <div style="text-align:center;">
                                                 <asp:Button ID="Btn_UpdateOk" runat="server" Text="Yes" class="btn btn-info" OnClick="Btn_UpdateOk_Click" Width="50px"/> &nbsp;&nbsp;&nbsp;
                                                 <asp:Button ID="Btn_upCancel" runat="server" Text="No" class="btn btn-info" Width="50px"/>
                                            </div>
                                    </td>
                                    <td class="e"> </td>
                                  </tr>
                                  <tr>
                                    <td class="so"> </td>
                                    <td class="s"> </td>
                                    <td class="se"> </td>
                                 </tr>
                             </table>
                            <br /><br />                   
                        </div>
                     </asp:Panel> 
   


<div class="clear"></div>
</div>
</asp:Content>




