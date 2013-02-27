<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginCtrl.ascx.cs" Inherits="Controls_LoginCtrl" %>

<asp:PlaceHolder runat="server" ID="phLogout">
    <p style="color:#FFFFFF;"><asp:Literal runat="server" Text="<%$ Resources:placeberry, LoginCtrl_User %>"></asp:Literal>: <asp:Label runat="server" ID="lblUserName"></asp:Label>
    &nbsp;|&nbsp;<a style="color:#FFFFFF;" href="<%= Page.ResolveUrl("~/UserProfile/UserProfile.aspx") %>"><asp:Literal runat="server" Text="<%$ Resources:placeberry, LoginCtrl_Profile %>"></asp:Literal></a>
    &nbsp;|&nbsp;<asp:HyperLink ID="lbLogout" runat="server" NavigateUrl="/LinkAction.aspx?action=logout" CssClass="top-user-link"  Text="<%$ Resources:placeberry, LoginCtrl_Logout %>"></asp:HyperLink>
    </p>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="phLogin">
    <asp:HyperLink runat="server" ID="lbLogin" NavigateUrl="/LinkAction.aspx?action=login" CssClass="top-user-link"  Text="<%$ Resources:placeberry, LoginCtrl_Login %>"></asp:HyperLink>
</asp:PlaceHolder>