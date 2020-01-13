<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateBill.aspx.cs" Inherits="WinEr.TemplateBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
<script src="js files/jquery-1.4.4.min.js"></script>
<script type='text/javascript' src='js files/knockout-3.3.0.js'></script>
    <script src="js%20files/jquery_v3.3.1.js?v=5.0.0"></script>
		<script src="js%20files/jquery.tools.min.js?v=5.0.0"></script>
		<script src="js%20files/jQueryUI_v1.12.1.js?v=5.0.0"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div data-bind="template: { name: 'studinfo-template', data: basicinfo }"></div>

                <div style="height:5.85cm">
                        <div data-bind="template: { name: 'feedetails-template', foreach: feesinfo }"></div>
                        <%--<tr data-bind="template: {name: 'goalsTemplate', foreach: $parent.goals}">--%>
                </div>
                <div data-bind="template: { name: 'TotalAmount-template', data: total }">

                </div>
            </div>
                <asp:HiddenField ID="Hdn_BillJSON" runat="server" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="HiddenField2" runat="server" />
                <asp:HiddenField ID="HiddenField3" runat="server" />
                <asp:HiddenField ID="HdnFld_ServerPath" runat="server" />

            <script type="text/javascript">
                function MyViewModel() {

                    var GettingValue = document.getElementById("Hdn_BillJSON").value;
                    this.basicinfo = JSON.parse(document.getElementById("HiddenField1").value);
                    this.feesinfo = JSON.parse(document.getElementById("HiddenField2").value);
                    this.total = JSON.parse(document.getElementById("HiddenField3").value);
//                    this.basicinfo = { StudName: 'Franklin', BillNO: 'BL:2500', BillDate: '18/15/2015', AdmtnNo: 'ADC123', Standrd: 'VI-A', Batch: '2014-15' };
//                    this.feesinfo = [{ id: 1, FeeName: 'Tution Fee', Amount: '5000'}, { id: 2, FeeName: 'Exam Fee', Amount: '2000'}];
//                    this.total = { TotalAmount: '7000' };
                }

                $(function () {
                    ensureTemplates(["studinfo-template", "feedetails-template", "TotalAmount-template"]);
                });

                function ensureTemplates(list) {
                    var loadedTemplates = [];
                    ko.utils.arrayForEach(list, function (name) {
                        $.get(HdnFld_ServerPath.value+ name + ".html", function (template) {
                            $("body").append("<script id=\"" + name + "\" type=\"text/html\">" + template + "<\/script>");
                            loadedTemplates.push(name);
                            if (list.length === loadedTemplates.length) {
                                ko.applyBindings(new MyViewModel());
                            }
                        });
                    });
                }
            </script>
        </div>
    </form>
</body>
</html>
