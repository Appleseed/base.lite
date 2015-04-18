<%@ Page Language="C#" Inherits="GA.Web.Default"  %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>Default</title>
</head>
<body>
	<form id="form1" runat="server">

		<asp:DropDownList id="ddlEventNames" AutoPostBack="true" runat="server" Visible="false" OnSelectedIndexChanged="ddlEventNames_SelectedIndexChanged" >
		</asp:DropDownList>
	
		<asp:Button id="button1" runat="server" Text="Click me!" OnClick="button1Clicked" />
	</form>
</body>
</html>

