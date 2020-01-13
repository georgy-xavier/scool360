<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentImageSlider.aspx.cs" Inherits="WinEr.StudentImageSlider" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link type="text/css" href="css files/ImageSliderstyle.css" media="screen" charset="utf-8" rel="stylesheet" />
  <%--<script type="text/javascript" src="js files/ImageSliderjquery-1.4.2.min.js" charset="utf-8"></script>--%>
  <script type="text/javascript" src="js files/ImageSliderjquery.movingboxes.min.js" charset="utf-8"></script> 
  
  <script type="text/javascript">
  $(function(){
   $('.slider').movingBoxes({ startPanel: 1 });
   
   // Example of how to move the panel from outside the plugin, only works on first on called.
   // $('.slider').data('movingBoxes').currentPanel(1); // 1 = move to first panel, blank = return current panel


})



document.onkeydown = KeyCheck;
function getCharacter(Code) {
    switch (Code) {
        case 65: 
            return 'a';
            break;
        case 66: 
            return 'b';
            break;
            case 67: 
            return 'c';
            break;
            case 68: 
            return 'd';
            break;
            case 69:
                return 'e';
                break;
            case 70: 
            return 'f';
            break;
            case 71: 
            return 'g';
            break;
            case 72: 
            return 'h';
            break;
            case 73:
                return 'i';
                break;
            case 74: 
            return 'j';
            break;
            case 75: 
            return 'k';
            break;
            case 76: 
            return 'l';
            break;
            case 77: 
            return 'm';
            break;
            case 78: 
            return 'n';
            break;
            case 79: 
            return 'o';
            break;
            case 80: 
            return 'p';
            break;
            case 81: 
            return 'q';
            break;
            case 82: 
            return 'r';
            break;
            case 83:
                return 's';
                break;
            case 84: 
            return 't';
            break;
        case 85:
            return 'u';
            break;
        case 86:
            return 'v';
            break;
        case 87:
            return 'w';
            break;
        case 88:
            return 'x';
            break;
        case 89:
            return 'y';
            break;
        case 90:
            return 'z';
            break;

    }
}
function KeyCheck(e) {
    var KeyID;
   
    if (window.event) { //IE

        KeyID = event.keyCode
    }
    else  // Netscape/Firefox/Opera
    {


        KeyID = e.keyCode;

    }

    //alert(KeyID);
   
    if (KeyID > 64 && KeyID < 90) {
        var charcter = getCharacter(KeyID);
        var x = document.getElementById('<%= HiddenValueNames.ClientID %>');
 
        var nameArray = x.value.split("-");

        for (var i = 0; i < nameArray.length; i++) {
            if (nameArray[i].toUpperCase().search(charcter.toUpperCase()) == 0) {
                $('.slider').data('movingBoxes').currentPanel(i + 1);
                break;
            }
        }
        
    }
    else {

        switch (KeyID) {
            case 13: //enter

                $(".right").click();
                break;
            case 32: //space
                $(".right").click();
                break;
            case 37: //left arrow
                $(".left").click();
                break;
            case 39: //right arrow
                $(".right").click();
                break;
        }
    }
}


function returnToNormal(element) {
    $(element)
.animate({ width: regWidth })
.find("img")
.animate({ width: regImgWidth })
.end()
.find("h2")
.animate({ fontSize: regTitleSize })
.end()
.find("p")
.animate({ fontSize: regParSize });
};

function growBigger(element) {
    $(element)
.animate({ width: curWidth })
.find("img")
.animate({ width: curImgWidth })
.end()
.find("h2")
.animate({ fontSize: curTitleSize })
.end()
.find("p")
.animate({ fontSize: curParSize });
}


  
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="height:500px">
<div id="wrapper">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
  <table width="100%" style="border-bottom:solid 2px Black;">
   
   <tr>
    <td>
    <asp:Label ID="Label1" runat="server" Text="Class name : " Font-Size="10"></asp:Label>
        <asp:Label ID="Label_ClassName" runat="server" Text="Class Not Found" Font-Size="10" Font-Bold="true"></asp:Label>
         
    </td>
    
    <td align="right">
       <asp:Button ID="Btn_Back" runat="server" Text="Back" Width="111px"  OnClientClick="javascript:history.go(-1);return false;"/>
      
    </td>
   </tr>
  
  </table>
 
 <asp:Panel ID="PanelStudents" Visible="false" runat="server"><div class="slider" id="slider-one">
    <img class="scrollButtons left" src="images/leftarrow.png" alt="" />

    <div style="overflow: hidden;" class="scroll">
      <div class="scrollContainer">
  
       <div id="ImageSliderDIV" runat="server">
       
       <%--  
         <div class="panel">
          <div class="inside">
          <center>
            <img src="images/18.jpeg" alt="picture"  />

            <h2>News Heading</h2>

            <p>A very short exerpt goes here... <a href="http://flickr.com/photos/joshuacraig/2698975899/">more link</a></p>
            </center>
         </div>
       </div>--%>
      <%-- <table width="100%"><tr align="center"><td align="left">
         </td></tr></table>--%>
<%--         <table  width="100%"><tr>
             <td align="right">
             
             </td><td align="left">
             
             </td></tr>
           <tr><td colspan="2"></td></tr>
             </table>--%>
         
       </div>

      </div>

      <div class="left-shadow"></div>

      <div class="right-shadow"></div>
    </div><img class="scrollButtons right" src="images/rightarrow.png" alt="" />
   </div>
</asp:Panel>

  

<br/><br/><br/>

<asp:HiddenField ID="HiddenValueNames" runat="server" />
    <center><asp:Label ID="Lbl_Message" runat="server" Text="" ForeColor="Red" Font-Size="12"  Font-Bold="true"></asp:Label></center>
    </div>
 </div>
    </asp:Content>