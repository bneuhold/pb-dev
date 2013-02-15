<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginCtrl.ascx.cs" Inherits="Controls_LoginCtrl" %>

<asp:PlaceHolder runat="server" ID="phLogout">
    <p style="color:#FFFFFF;"><asp:Literal runat="server" Text="<%$ Resources:placeberry, LoginCtrl_User %>"></asp:Literal>: <asp:Label runat="server" ID="lblUserName"></asp:Label>
    &nbsp;|&nbsp;<a style="color:#FFFFFF;" href="<%= Page.ResolveUrl("~/UserProfile/UserProfile.aspx") %>"><asp:Literal runat="server" Text="<%$ Resources:placeberry, LoginCtrl_Profile %>"></asp:Literal></a>
    &nbsp;|&nbsp;<asp:LinkButton runat="server" ID="lbLogout" Text="<%$ Resources:placeberry, LoginCtrl_Logout %>" ForeColor="#FFFFFF"></asp:LinkButton>
    </p>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="phLogin">
    <asp:LinkButton runat="server" ID="lbLogin" Text="<%$ Resources:placeberry, LoginCtrl_Login %>" ForeColor="#FFFFFF"></asp:LinkButton>
</asp:PlaceHolder>