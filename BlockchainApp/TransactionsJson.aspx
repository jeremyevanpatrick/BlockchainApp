<%@ Page Language="C#" %>

<%@ Import Namespace="BlockchainApp" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Configuration" %>

<script runat="server">
    
    public static string transactionsPath => ConfigurationManager.AppSettings["TransactionsPath"];

    protected void Page_Load(object sender, EventArgs e)
    {
        string fullPath =
            Path.IsPathRooted(transactionsPath)
                ? transactionsPath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools", transactionsPath);

        Response.Clear();
        Response.ContentType = "application/json";
        Response.Write(File.ReadAllText(fullPath));
        Response.End();

    }

</script>