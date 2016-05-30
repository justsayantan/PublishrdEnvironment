
Alchemy.command("${PluginName}", "SayHello", {

    init: function () {


    },

    isEnabled: function (selection) {
        return this.isAvailable(selection);
    },

    isAvailable: function (selection) {
        var item = this._getSelectedItem(selection);
        if (item != null) {
            switch ($models.getItemType(item)) {
                case $const.ItemType.FOLDER:
                case $const.ItemType.STRUCTURE_GROUP:
                    return true;
            }
        }

        return false;
    },

    execute: function (selection) {       

        var p = this.properties;
        var selectedItem = this._getSelectedItem(selection);
        var url = "${ViewsUrl}PublishedEnvironment.aspx#selectedItem=" + selectedItem;
        var parameters = "width=450px, height=500px";
        var args = { popupType: Tridion.Controls.PopupManager.Type.EXTERNAL };

        p.popup = $popupManager.createExternalContentPopup(url, parameters, args);
        $evt.addEventHandler(p.popup, "close", this.getDelegate(this.closePopup));
        p.popup.open();
    },

    closePopup: function () {
        var p = this.properties;
        if (p.popup) {
            p.popup.close();
            p.popup = null;
        }
    },

    _getSelectedItem: function (selection) {
        $assert.isObject(selection);

        switch (selection.getCount()) {
            case 0: return (selection.getParentItemUri) ? selection.getParentItemUri() : null;
            case 1: return selection.getItem(0);
            default: return null;
        }
    },
});