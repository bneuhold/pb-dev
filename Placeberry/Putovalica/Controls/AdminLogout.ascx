<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminLogout.ascx.cs" Inherits="Controls_AdminLogout" %>

<ul class="user-menu">                
    <li><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:putovalica, LoginCtrl_User %>"></asp:Literal>: <asp:Label runat="server" ID="lblUserName"></asp:Label></li>
    <li><asp:LinkButton runat="server" ID="lbLogout" Text="<%$ Resources:putovalica, LoginCtrl_Logout %>"></asp:LinkButton></li>
</ul>
