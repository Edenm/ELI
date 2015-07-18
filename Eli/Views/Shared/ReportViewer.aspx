<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>ReportViewer</title>
    <script runat="server">
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportPath = Server.MapPath("~/Reports/FinanceFactorDebators.rdlc");

            //ReportParameter[] param = new ReportParameter[1]; 
            //param[0] = new ReportParameter("CustomerID", txtparam.Text);
            //ReportViewer1.ServerReport.SetParameters(param);
            System.Web.UI.ScriptManager scriptManager = new ScriptManager();
            Page page = new Page();
            System.Web.UI.HtmlControls.HtmlForm form = new HtmlForm();
            form.Controls.Add(scriptManager);
            form.Controls.Add(ReportViewer1);
            page.Controls.Add(form);
            //page.DataBind();  //exception here 
            ReportViewer1.ServerReport.Refresh();
            
           
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="745px">
            </rsweb:ReportViewer>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true">
            </rsweb:ReportViewer>        
        </div>
    </form>
</body>
</html>
