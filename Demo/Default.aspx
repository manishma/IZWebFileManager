<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Title="Untitled Page" %>

<%@ Import Namespace="System.Reflection" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<script runat="server">
    class TagedCheckBox : CheckBox
    {
        object _tag;
        public object Tag {
            get { return _tag; }
        }

        public TagedCheckBox (object tag) {
            _tag = tag;
            AutoPostBack = true;
            Checked = true;
        }
    }

    protected override void OnInit (EventArgs e) {
        base.OnInit (e);

        foreach (PropertyInfo prop in typeof (ToolbarOptions).GetProperties ()) {
            if (prop.PropertyType == typeof (bool)) {
                TagedCheckBox check = new TagedCheckBox (prop);
                check.Text = prop.Name.Substring (4, prop.Name.Length - 10);
                check.CheckedChanged += new EventHandler (TagedCheckBox_CheckedChanged);
                PlaceHolder1.Controls.Add (check);
            }
        }

        string[] langs = new string[] { "en-US", "de-DE", "es-ES", "fr-FR", "he-IL", "it-IT", "nb-NO", "nl-NL", "pt-BR", "ru-RU", "cs-CZ", "sk-SK", "sr-Cyrl-CS", "sr-Latn-CS", "sv-se", "tr-TR", "zh-CN" };
        
        if(!IsPostBack)
            foreach (string lang in langs)
            {
                string langName = new CultureInfo(lang).NativeName;
                DropDownList1.Items.Add(new ListItem(langName, lang));
            }
    }

    protected void DropDownList1_SelectedIndexChanged (object sender, EventArgs e) {
        System.Globalization.CultureInfo culture = null;
        if (DropDownList1.SelectedValue.Length > 0)
            culture = new System.Globalization.CultureInfo (DropDownList1.SelectedValue);
        FileManager1.Culture = culture;

    }

    protected void TagedCheckBox_CheckedChanged (object sender, EventArgs e) {
        TagedCheckBox check = (TagedCheckBox) sender;
        PropertyInfo prop = (PropertyInfo) check.Tag;
        prop.SetValue (FileManager1.ToolbarOptions, check.Checked, null);
    }

    protected void CheckBox1_CheckedChanged (object sender, EventArgs e) {
        FileManager1.ShowToolBar = ((CheckBox) sender).Checked;
    }

    protected void CheckBox2_CheckedChanged (object sender, EventArgs e) {
        FileManager1.ShowAddressBar = ((CheckBox) sender).Checked;
    }

    protected void CheckBox3_CheckedChanged (object sender, EventArgs e) {
        FileManager1.ShowUploadBar = ((CheckBox) sender).Checked;
    }

    protected void CheckBox4_CheckedChanged (object sender, EventArgs e) {
        FileManager1.RootDirectories[0].ShowRootIndex = ((CheckBox) sender).Checked;
    }

    protected void CheckBox5_CheckedChanged(object sender, EventArgs e)
    {
        FileManager1.ShowSearchBox = ((CheckBox)sender).Checked;
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top: 8px; margin-bottom: 8px;">
        <div style="margin-top: 2px;  margin-bottom: 2px;">
            Chose Language: 
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div style="margin-top: 2px;  margin-bottom: 2px;">
            <asp:CheckBox runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"  Checked="True" Text="Show ToolBar" />
            <asp:CheckBox runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged" Checked="True" Text="Show AddressBar" />
            <asp:CheckBox runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox5_CheckedChanged" Checked="True" Text="Show SearchBox" />
            <asp:CheckBox runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox3_CheckedChanged" Checked="True" Text="Show UploadBar" />
            <asp:CheckBox runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox4_CheckedChanged" Checked="True" Text="Show RootIndex" />
        </div>
        <div style="margin-top: 2px;  margin-bottom: 2px;">
            Customize Toolbar:
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </div>
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400" Width="600">
            <RootDirectories>
                <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
            </RootDirectories>
        </iz:FileManager>
    </div>
</asp:Content>
