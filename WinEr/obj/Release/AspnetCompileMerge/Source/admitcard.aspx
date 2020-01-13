<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admitcard.aspx.cs" Inherits="WinEr.admitcard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1" runat="server">
<script src="js files/jquery-1.4.4.min.js"></script>
<script src="js%20files/jquery_v3.3.1.js?v=5.0.0"></script>
		<script src="js%20files/jquery.tools.min.js?v=5.0.0"></script>
		<script src="js%20files/jQueryUI_v1.12.1.js?v=5.0.0"></script>
		<script src="js%20files/Bootstrap_v3.3.7.js?v=5.0.0"></script>
<script type='text/javascript' src='js files/knockout-3.3.0.js'></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div data-bind="template: { name: 'admitcard-template', foreach: basicinfo }"></div>

                <%--<div style="height:5.85cm">
                        <div data-bind="template: { name: 'feedetails-template', foreach: examinfo }"></div>
                        <%--<tr data-bind="template: {name: 'goalsTemplate', foreach: $parent.goals}">--%>
               <%-- </div>
                <div data-bind="template: { name: 'TotalAmount-template', data: total }">

                </div>--%>
            </div>
                <asp:HiddenField ID="Hdn_admitcardJSON" runat="server" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
               <asp:hiddenfield id="HiddenField2" runat="server" />
               <asp:hiddenfield id="Hdn_ExamJSON" runat="server" />
                
                <asp:HiddenField ID="HdnFld_ServerPath" runat="server" />

            <script type="text/javascript">
                function MyViewModel() {


                    var GettingValue = document.getElementById("Hdn_admitcardJSON").value;
                    
                    this.basicinfo = JSON.parse(document.getElementById("HiddenField1").value);
                    this.examinfo = JSON.parse(document.getElementById("HiddenField2").value);
                    var exam_schedule = document.getElementById("Hdn_ExamJSON").value;

                }



                $(function() {
                    ensureTemplates(["admitcard-template"]);

                    examschedule(["admitcard-template"]);

                });

                function ensureTemplates(list) {
                    var loadedTemplates = [];
                    ko.utils.arrayForEach(list, function(name) {
                        $.get(HdnFld_ServerPath.value + name + ".html", function(template) {
                            $("body").append("<script id=\"" + name + "\" type=\"text/html\">" + template + "<\/script>");
                            loadedTemplates.push(name);
                            if (list.length === loadedTemplates.length) {
                                ko.applyBindings(new MyViewModel());
                            }
                        });
                        var exam_schedule = document.getElementById("Hdn_ExamJSON").value;
                        $('.examschedule').html(exam_schedule);


                    });
                }
                function examschedule(list) {

                    var loadedTemplates = [];
                    ko.utils.arrayForEach(list, function(name) {
                        $.get(HdnFld_ServerPath.value + name + ".html", function(template) {
                            var exam_schedule = document.getElementById("Hdn_ExamJSON").value;
                            $('.examschedule').html(exam_schedule);
                            loadedTemplates.push(name);
                        });
                    });
                }
                
            </script>
        </div>
    </form>
</body>
</html>
