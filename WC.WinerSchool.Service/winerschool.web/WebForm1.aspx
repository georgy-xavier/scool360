<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="winerschool.web.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>

      <script>
   $( document ).ready(function() {
    console.log( "ready!" );
	var data='123';
	var url = "http://localhost:26264/api/device";
	var successFn =function(data){
	console.log( "ssss!" );
	};
	  $.ajax({
	  type: "POST",
	  url: url,
	  data: JSON.stringify(data),
	  success: successFn,
	  contentType: 'application/json; charset=utf-8'
	});
	  
	  
	});
  </script>
</body>
</html>
