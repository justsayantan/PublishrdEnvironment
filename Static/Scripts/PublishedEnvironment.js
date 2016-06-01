Type.registerNamespace("Alchemy.Plugins.PublishrdEnvironment.Views");

Alchemy.Plugins.PublishrdEnvironment.Views.PublishrdEnvironment = function PublishrdEnvironment() {
    Tridion.OO.enableInterface(this, "Alchemy.Plugins.PublishrdEnvironment.Views.PublishrdEnvironment");
    this.addInterface("Tridion.Cme.View");
};

Alchemy.Plugins.PublishrdEnvironment.Views.PublishrdEnvironment.prototype.initialize = function PublishrdEnvironment$initialize() {

    var p = this.properties;
    var c = p.controls;
    var self = this;
    p.selectedItem = $url.getHashParam("selectedItem");
    
    Alchemy.Plugins["${PluginName}"].Api.getSettings()
    .success(function (settings) {
        if (settings) {
            
            var param = { OrganizationalItemId: p.selectedItem, stagingTcmId: settings.StagingTcmID, liveTcmId: settings.LiveTcmID };
            
            Alchemy.Plugins["${PluginName}"].Api.Service.publishedEnvironment(param)
        .success(function (result) {

            if (result.length > 0) {
                var data = new Array();
                data.push(["TcmId", "Title", "Published To Preview/Staging", "Published To Live"])

                for (var i = 0; i < result.length; i++) {
                    data.push([result[i].itemTcmId, result[i].itemTitle, result[i].isPublishedToStaging, result[i].isPublishedToLive]);
                }
                var table = document.createElement("TABLE");
                table.border = "1";

                var item = data[0].length;

                var row = table.insertRow(-1);
                for (var i = 0; i < item; i++) {
                    var headerCell = document.createElement("TH");
                    headerCell.innerHTML = data[0][i];
                    row.appendChild(headerCell);
                }


                for (var i = 1; i < data.length; i++) {
                    row = table.insertRow(-1);
                    for (var j = 0; j < item; j++) {
                        var cell = row.insertCell(-1);
                        if (j == 2 || j == 3) {
                            if (data[i][j] == true) {
                                cell.innerHTML = "<img src ='/WebUI/Editors/Alchemy/Plugins/Published_Environment/assets/img/correct.png' alt= 'Yes' align='middle'/>";
                            }
                            else { cell.innerHTML = "<img src = '/WebUI/Editors/Alchemy/Plugins/Published_Environment/assets/img/incorrect1.png' alt= 'No' align='middle' />"; }
                        }
                        else { cell.innerHTML = data[i][j]; }

                    }
                }

                var dvTable = document.getElementById("dvTable");
                dvTable.innerHTML = "";
                dvTable.appendChild(table);
            }
            else
            {
                var dvTable = document.getElementById("dvTable");
                dvTable.innerHTML = "<p><font color='red'>No Components/Pages are found in this Ripository.</font></p>";
                dvTable.innerHTML.fontcolor = red;
            }

        })
        .error(function (type, error) {
            $messages.registerError("No Components/Pages are found in this Ripository.");
        })
        .complete(function () {
        });
        }
    })
    .complete(function () {

    });

    

    //Alchemy.Plugins["${PluginName}"].Api.Service.publishedEnvironment(params)
    //    .success(function (result) {

    //        var data = new Array();
    //        data.push(["TcmId", "Title", "Published To Staging", "Published To Live"])            

    //        for (var i = 0; i < result.length; i++) {
    //            data.push([result[i].itemTcmId, result[i].itemTitle, result[i].isPublishedToStaging, result[i].isPublishedToLive]);
    //        }
    //        var table = document.createElement("TABLE");
    //        table.border = "1";

    //        var item = data[0].length;

    //        var row = table.insertRow(-1);
    //        for (var i = 0; i < item; i++) {
    //            var headerCell = document.createElement("TH");
    //            headerCell.innerHTML = data[0][i];
    //            row.appendChild(headerCell);
    //        }

            
    //        for (var i = 1; i < data.length; i++) {
    //            row = table.insertRow(-1);
    //            for (var j = 0; j < item; j++) {
    //                var cell = row.insertCell(-1);
    //                cell.innerHTML = data[i][j];
    //            }
    //        }

    //        var dvTable = document.getElementById("dvTable");
    //        dvTable.innerHTML = "";
    //        dvTable.appendChild(table);

    //    })
    //    .error(function (type, error) {
    //        $messages.registerError("There was an error", error.message);
    //    })
    //    .complete(function () {
    //    });    
    
};



$display.registerView(Alchemy.Plugins.PublishrdEnvironment.Views.PublishrdEnvironment);