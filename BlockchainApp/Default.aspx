<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlockchainApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Blockchain App</title>
    <script src="Scripts/jquery-3.6.0.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <style>
        #outputArea{
            max-height:400px;
            display:none;
        }
    </style>
    <script>
        function StartBlockchain() {

            $("#startBtn").addClass("disabled");
            $("#outputArea").show().html('Blockchain is running...');

            $.get("StartBlockchain.aspx")
                .done(function (data) {

                    var processId = data;

                    setTimeout(function () { GetStatus(processId); }, 500);

                });
        }

        function GetStatus(processId) {
            $.get("StartBlockchain.aspx?processid=" + processId)
                .done(function (data) {

                    if (data != '') {

                        if (data != "RUNNING") {
                            $("#outputArea").html(data);
                        }

                        setTimeout(function () { GetStatus(processId); }, 500);

                        var adm = $("#outputArea");
                        var height = adm[0].scrollHeight;
                        adm.scrollTop(height);

                    } else {
                        //complete
                        $("#startBtn").removeClass("disabled");
                    }

                    $("#blockchainLink").attr("href", "blockchain.json?t=" + Date.now());

                });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">

            <div class="row my-3">
                <div class="col-12">
                    <h3>Blockchain App</h3>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <h5>Input Data</h5>
                    <div class="card py-2 px-3">
                        <a href="transactions.json" target="_blank">transactions.json</a>
                    </div>
                </div>
            </div>
        
            <div class="row my-4">
                <div class="col-12">
                    <div id="startBtn" class="btn btn-primary" onclick="StartBlockchain()">Run blockchain</div>
                    <pre id="outputArea" class="mt-2 px-3 py-2 bg-dark text-white">

                    </pre>
                </div>
            </div>
            
            <div class="row mb-3">
                <div class="col-12">
                    <h5>Output Data</h5>
                    <div class="card py-2 px-3">
                        <a id="blockchainLink" href="blockchain.json" target="_blank">blockchain.json</a>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
