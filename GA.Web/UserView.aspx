<%@ Page Language="C#" Inherits="GA.Web.UserView" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>UserView</title>
</head>
<body>
	<form id="form1" runat="server">


		<asp:Label id="lblCount" runat="server"></asp:Label>

		<asp:DataList id="lvCollectionItems" runat="server" >
			<ItemTemplate>
				<%# Eval("ItemTitle") %> : 
				<%# Eval("ItemDescription") %> <br>
				<asp:LinkButton id="lbViewItem" runat="server" CommandName="View" CommandArgument='<%# Eval("ItemID") %>' OnCommand="lbViewItem_OnCommand" >View </asp:LinkButton>
			</ItemTemplate>
		</asp:DataList>

		<asp:FormView id="fvCollectionItem"
			DataKeyNames="ItemID"
			runat="server"
			DefaultMode="ReadOnly"
			visible="false"
			>
			<ItemTemplate>
				<asp:Label id="lblItemTitle" runat="server" Text='<%# Eval("ItemTitle") %>' /><br>
				<asp:Label id="lblItemUrl" runat="server" Text='<%# Eval("ItemUrl") %>' /><br>
				<asp:Label id="lblItemDescription" runat="server" Text='<%# Eval("ItemDescription") %>' /><br>
				<hr>
				<pre><%# Eval("ItemContentCache") %></pre><br>

			</ItemTemplate>
		</asp:FormView>

	</form>
</body>
</html>

