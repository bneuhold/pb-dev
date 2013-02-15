<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeoDirectory.ascx.cs" Inherits="Controls_SeoDirectory" %>

    <asp:Panel ID="pnlParent" runat="server">
            <a id="aParent" href="" runat="server"></a>
            <br />
            <br />
    </asp:Panel>
    <asp:Repeater ID="repDirectory" runat="server">
        <ItemTemplate>
            <a href="<%# Eval("Href") %>"><%# Eval("Title")%></a>
        </ItemTemplate>
        <SeparatorTemplate>
            <%# Separator %>
        </SeparatorTemplate>
    </asp:Repeater>