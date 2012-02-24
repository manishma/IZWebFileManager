<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Button1_Click(object sender, EventArgs e)
    {
        FormsAuthentication.RedirectFromLoginPage("user", false);
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding: 50px;">
    
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Log me in" />
    
    </div>
    </form>
</body>
</html>
