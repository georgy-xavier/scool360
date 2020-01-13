<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeeCollectionReport.aspx.cs" Inherits="Winer.Portal.FeeCollectionReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/main.css" rel="stylesheet" />
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     
 <div class="table-responsive">          
  <table class="ux-table">
    <thead>
		<tr>
			<th>SL No</th>
			<th>School Name</th>
			<th>Fee Collected</th>
			<th>Unpaid Amount</th>
		</tr>
	</thead>
	<tbody id="tbody-grid">
	</tbody>
  </table>
  </div>
        <input id="load-more-school" type="button" value="Load more"/>
    </div>
    </form>
    <%--<script src="https://code.jquery.com/jquery-3.2.1.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="js/config.js" async="async"></script>
    <script src="js/api-module.js"></script>
    <script src="js/feecollection-report.js"></script>
</body>
</html>
