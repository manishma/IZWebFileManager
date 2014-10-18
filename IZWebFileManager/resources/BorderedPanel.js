function BorderedPanel_SetStyle(ClientID, ImagesArray, CssClass) {
    var panel = document.getElementById(ClientID);
    if (panel == null)
        return;
    if (panel.normalClass == null)
        panel.normalClass = panel.className;
    panel.className = CssClass;

    BorderedPanel_SetBackImage(ClientID + '_BorderT', ImagesArray[0]);
    BorderedPanel_SetBackImage(ClientID + '_BorderL', ImagesArray[1]);
    BorderedPanel_SetBackImage(ClientID + '_BorderR', ImagesArray[2]);
    BorderedPanel_SetBackImage(ClientID + '_BorderB', ImagesArray[3]);

    BorderedPanel_SetBackImage(ClientID + '_BorderLT', ImagesArray[4]);
    BorderedPanel_SetBackImage(ClientID + '_BorderLB', ImagesArray[5]);
    BorderedPanel_SetBackImage(ClientID + '_BorderRT', ImagesArray[6]);
    BorderedPanel_SetBackImage(ClientID + '_BorderRB', ImagesArray[7]);
}

function BorderedPanel_SetBackImage(ClientID, Image) {
    var el = document.getElementById(ClientID);
    if (el) {
        if (el.normalBackgroundImage == null)
            el.normalBackgroundImage = el.style.backgroundImage;
        el.style.backgroundImage = Image ? 'url(' + Image + ')' : 'none';
    }
}

function BorderedPanel_RestoreStyle(ClientID) {
    var panel = document.getElementById(ClientID);
    if (panel != null && panel.normalClass != null)
        panel.className = panel.normalClass;

    BorderedPanel_RestoreBackImage(ClientID + '_BorderT');
    BorderedPanel_RestoreBackImage(ClientID + '_BorderL');
    BorderedPanel_RestoreBackImage(ClientID + '_BorderR');
    BorderedPanel_RestoreBackImage(ClientID + '_BorderB');

    BorderedPanel_RestoreBackImage(ClientID + '_BorderLT');
    BorderedPanel_RestoreBackImage(ClientID + '_BorderLB');
    BorderedPanel_RestoreBackImage(ClientID + '_BorderRT');
    BorderedPanel_RestoreBackImage(ClientID + '_BorderRB');
}

function BorderedPanel_RestoreBackImage(ClientID) {
    var el = document.getElementById(ClientID);
    if (el != null && el.normalBackgroundImage != null)
        el.style.backgroundImage = el.normalBackgroundImage;
}
//# sourceMappingURL=BorderedPanel.js.map
