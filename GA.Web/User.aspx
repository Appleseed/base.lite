<%@ Page Language="C#" Inherits="GA.Web.User" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>User</title>
</head>
<body>
	<form id="form1" runat="server">

		<asp:Label id="lblCount" runat="server"></asp:Label>

		<asp:ListView id="lvCollectionItems" runat="server" ></asp:ListView>


		<asp:Panel id="pnCollectionItemViewer" runat="server" Visible="false">
			TODO: add title
			TODO: add html contents from DB
			...
		</asp:Panel>

	</form>
</body>
</html>

