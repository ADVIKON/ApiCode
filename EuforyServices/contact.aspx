<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="EuforyServices.contact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form method="POST" enctype="multipart/form-data" action = "http://185.19.219.90/ReceiveUpload.aspx" >
<input name=hid1 type=hidden value="test123">
<input name=hid2 type=hidden value="abcdef">
<input name=attach1 type=file size=20><br>
<input name=attach2 type=file size=20><br>
<input type=submit value="Upload">
</form>
</body>
 
</html>
