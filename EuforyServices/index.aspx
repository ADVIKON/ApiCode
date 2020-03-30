<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="EuforyServices.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     
</head>
<body>
    <div style="position:fixed; top:0; left:0; right:0; bottom:0; width:100%; height:100%">
        <div style="position:absolute; top:0; bottom:0; left:0; right:0; margin:auto; width:218px; height:218px;">
            <img src="img/logo.png" style="width:100%" />
            <form runat="server">
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </form>
        </div>
    </div>    
</body>
</html>
