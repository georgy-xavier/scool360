<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StaffSubjectReport.aspx.cs" Inherits="WinEr.StaffSubjectReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" type="text/css" media="screen" href="css files/DockMenuStyle.css" />
	<script type="text/javascript" src="js files/DockMenu.js"></script>
 <style type="text/css">
         .TableHeaderStyle
        {
           border-color:#eeeeee;
           border-style:solid;
           border-width:1px;
           background-color:#666666;
           font-weight:bold;
           color:White;
           text-align:center;
           padding-left:10px;
           padding-right:10px;
          
        }
        .SubHeaderStyle
        {
           background-color:Gray;
           color:White;
           font-weight:bold;
           text-align:center;
           border-color:#eeeeee;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           padding-right:10px;
        }
        .CellStyle
        {
           border-color:#eeeeee;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           color:#333333;
        }
         * {
	    margin: 0;
	    padding: 0;
        }

        body {

	        font-family: Helvetica, sans-serif;
        }

        .clear {
	        clear: both;
        }

        #page-wrap 
        {
            text-align:center;
	        width:100%;
	        background : white;
	        margin: 20px auto;

         

        }

        .button 
        {
        width: 205px;
        float: left;
        margin: 10px;
        cursor:pointer;	
        font-family:Garamond;
        font-size:15px;
        font-weight:lighter;
        opacity: 1.0;
	    border-bottom: 1px solid gray;
	    vertical-align:middle;
	    padding-top:0px;
	    padding-bottom:0px;
	    background-color:White;
        font-weight:bold;
        }

        .DockMenuContainer
        {
           padding-top:60px;
           
        }
        
        #ClassListContainer
        {
            padding-top:0px;
        }
        #content
        {
            border:solid 1px gray;
            width:250px;
            position:relative;
        }
        .StaffDetails
        {
            padding-left:20px;
            text-align:left;
            font-weight:bold;
        }
     .buttongroup
     {
         height:300px;
         overflow:auto;
     }
 </style>
 
<script type="text/javascript">

   function Onload()
   {
       $(".button").css({
           opacity: 0.5
       });
       $("#class1-button").css("font-weight", "bold");
       $("#class1-button").css("background-color", "Black");
       $("#class1-button").css("color", "White");
       $("#class1-button").css("cursor", "text");
       $("#class1-button").css({
           opacity: 1,
           borderWidth: 1
       });
       $("#class1-button").css("font-weight", "bold");
       $("#content").find("div:visible").fadeOut("fast", function() {
           //once the fade out is completed, we start to fade in the right div
           $("#class1").fadeIn();
       })

       $("#page-wrap td.button").click(function() {

           $clicked = $(this);



           // if the button is not already "transformed" AND is not animated
           if ($clicked.css("opacity") != "1" && $clicked.is(":not(animated)")) {
               $(".button").animate({
                   opacity: 0.5,
                   borderWidth: 1
               }, 600);
               $(".button").css("font-weight", "bold");
               $(".button").css("background-color", "White");
               $(".button").css("color", "Black");
               $(".button").css("cursor", "pointer");
               $clicked.animate({
                   opacity: 1,
                   borderWidth: 1
               }, 600);
               $clicked.css("font-weight", "bold");
               $clicked.css("background-color", "Black");
               $clicked.css("color", "White");
               $clicked.css("cursor", "text");
               // each button div MUST have a "xx-button" and the target div must have an id "xx" 
               var idToLoad = $clicked.attr("id").split('-');

               //we search trough the content for the visible div and we fade it out

               $("#content").find("div:visible").fadeOut("fast", function() {
                   //once the fade out is completed, we start to fade in the right div
                   $(this).parent().find("#" + idToLoad[0]).fadeIn();
               })


               SelectValue = $("#" + idToLoad[0]).find("span:first").text();
           }

           //we reset the other buttons to default style


       });



   }


    var SelectValue;
    var GlobalName;

    $(function() {
        $(".button").css({
            opacity: 0.5
        });
        $("#class1-button").css("font-weight", "bold");
        $("#class1-button").css("background-color", "Black");
        $("#class1-button").css("color", "White");
        $("#class1-button").css("cursor", "text");
        $("#class1-button").css({
            opacity: 1,
            borderWidth: 1
        });
        $("#class1-button").css("font-weight", "bold");
        $("#content").find("div:visible").fadeOut("fast", function() {
            //once the fade out is completed, we start to fade in the right div
            $("#class1").fadeIn();
        })

        $("#page-wrap td.button").click(function() {

            $clicked = $(this);



            // if the button is not already "transformed" AND is not animated
            if ($clicked.css("opacity") != "1" && $clicked.is(":not(animated)")) {
                $(".button").animate({
                    opacity: 0.5,
                    borderWidth: 1
                }, 600);
                $(".button").css("font-weight", "bold");
                $(".button").css("background-color", "White");
                $(".button").css("color", "Black");
                $(".button").css("cursor", "pointer");
                $clicked.animate({
                    opacity: 1,
                    borderWidth: 1
                }, 600);
                $clicked.css("font-weight", "bold");
                $clicked.css("background-color", "Black");
                $clicked.css("color", "White");
                $clicked.css("cursor", "text");
                // each button div MUST have a "xx-button" and the target div must have an id "xx" 
                var idToLoad = $clicked.attr("id").split('-');

                //we search trough the content for the visible div and we fade it out

                $("#content").find("div:visible").fadeOut("fast", function() {
                    //once the fade out is completed, we start to fade in the right div
                    $(this).parent().find("#" + idToLoad[0]).fadeIn();
                })


                SelectValue = $("#" + idToLoad[0]).find("span:first").text();
            }

            //we reset the other buttons to default style


        });



    });
		
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
</ajaxToolkit:ToolkitScriptManager>  
<asp:UpdatePanel ID="updatepanel" runat="server">
<ContentTemplate>
<div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Subject Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >

                   <table width="100%">
                    <tr>
                     <td style="width:33%" align="right">
                       Select Subject
                     </td>
                     <td  align="left">
                         <asp:DropDownList ID="Drp_Subjects" runat="server" Width="180" class="form-control">
                         </asp:DropDownList>
</tr>
<tr>
 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"> &nbsp;<asp:Button ID="Btn_Load" runat="server" Text="Load" 
                             Class="btn btn-primary" onclick="Btn_Load_Click" />
                             
                          &nbsp;
                         <asp:Button ID="Img_Export" runat="server" Text="Export" Class="btn btn-primary" Visible="false"
                             onclick="Img_Export_Click" />
</td>
                     </tr>
                        
                   
                     
                    <tr>
                     <td colspan="2"  style="height:200px;" valign="top">
                       <br />
                       <asp:Label ID="lbl_gridmsg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                       <div style="width:900px;overflow:auto;">
                       <div id="DivGrid" runat="server">
                        
                       
                       </div>
                       
                       </div>
                     </td>
                    </tr>
                   </table>
                    </div>
                    
                          <%--<div id="page-wrap"> 
                           <table width="100%">  
                             <tr> 
                               <td align="center" style="padding-left:40px;width:20%" valign="top"> 
                                <table cellspacing="20px">
                                  <tr> 
                                   <td id="class1-button" class="button" align="center" > <p>Pre-KG</p> </td>  

                                  </tr>
                                  <tr>
                                   <td id="class2-button" class="button" align="center" > <p>LKG A</p> </td> 
                                 
                                   </tr>
                                                                     <tr>
                                   <td id="class3-button" class="button" align="center" > <p>LKG A</p> </td> 
                                 
                                   </tr>
                                   </table> 
                                 </td> 
                                 <td align="left"  style="padding-left:10px;padding-top:20px" valign="top">
                                   <div id="content" style="padding-left:0px"> 
                                     <div id="class1" >
                                     
                                       <table width="100%" cellspacing="10">
                                          <tr>
                                           <td class="StaffDetails">
                                            Staff1
                                           </td>
                                          </tr>
                                          <tr>
                                           <td class="StaffDetails">
                                            Staff2(UKG A)
                                           </td>
                                          </tr>
                                         </table>
                                     
                                     </div>
                                     <div id="class2" >
                                        
                                         <table width="100%" cellspacing="10">
                                          <tr>
                                           <td class="StaffDetails">
                                            class2
                                           </td>
                                          </tr>
                                          <tr>
                                           <td class="StaffDetails">
                                            class21
                                           </td>
                                          </tr>
                                         </table>
                                           
                                     
                                     </div>
                                     <div id="class3" >
                                        class3 
                                     
                                     </div>
                                   </div>
                                 </td>
                               </tr> 
                            </table> 
                         </div>--%>

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
    <asp:Label ID="Label1" runat="server" Text="" Visible="false" class="control-label"></asp:Label>
 </ContentTemplate>
 <Triggers >
  <asp:PostBackTrigger ControlID="Img_Export" />
 </Triggers>
 </asp:UpdatePanel>   


</div>
</asp:Content>
