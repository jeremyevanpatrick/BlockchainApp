<%@ Page Language="C#" %>

<%@ Import Namespace="BlockchainApp" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Configuration" %>

<script runat="server">
    
    public static string blockchainStoragePath => ConfigurationManager.AppSettings["BlockchainStoragePath"];

    protected void Page_Load(object sender, EventArgs e)
    {
        string fullPath =
            Path.IsPathRooted(blockchainStoragePath)
                ? blockchainStoragePath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, blockchainStoragePath);

        Response.Clear();
        Response.ContentType = "application/json";
        Response.Write(File.ReadAllText(fullPath));
        Response.End();

    }

</script>