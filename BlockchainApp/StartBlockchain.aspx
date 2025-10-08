<%@ Page Language="C#" %>

<%@ Import Namespace="BlockchainApp" %>
<%@ Import Namespace="System.Diagnostics" %>

<script runat="server">
    
    public static string blockchainConsolePath = System.Configuration.ConfigurationManager.AppSettings["BlockchainConsolePath"];
    public static string transactionsPath = System.Configuration.ConfigurationManager.AppSettings["TransactionsPath"];
    public static string blockchainStoragePath = System.Configuration.ConfigurationManager.AppSettings["BlockchainStoragePath"];
    public static string applicationName = "WebApp";

    private static Dictionary<string, ExecuteProcess> processes = new Dictionary<string, ExecuteProcess>();

    protected void Page_Load(object sender, EventArgs e)
    {
        string processID = Request.QueryString["processid"];

        if (string.IsNullOrEmpty(processID))
        {
            ExecuteProcess ep = new ExecuteProcess();

            ep.Command = blockchainConsolePath;
            ep.Arguments = string.Format(@"""{0}"" ""{1}"" ""{2}""", transactionsPath, blockchainStoragePath, applicationName);

            ep.Start();

            processes.Add(ep.ProcessID.ToString(), ep);

            Response.Write(ep.ProcessID.ToString());
        }
        else if (processes != null && processes.Count > 0)
        {
            
            ExecuteProcess ep = null;
            
            if (processes.TryGetValue(processID, out ep))
            {

                string outputData = ep.GetOutputData();

                if (ep.HasExited())
                {
                    processes.Remove(processID);
                }
                else if(string.IsNullOrEmpty(outputData))
                {
                    //if program has not exited, always return a response
                    outputData = "RUNNING";
                }

                Response.Write(outputData);

            }

        }

    }

</script>