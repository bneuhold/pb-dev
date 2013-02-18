<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserAdminSidebar.ascx.cs" Inherits="Controls_UserAdminSidebar" %>


<aside class="sidebar" style="float:left;">

    <ul class="user-admin-menu">
        <li runat="server" id="liProfile"><a href="<%= Page.ResolveUrl("~/userAdmin/Default.aspx") %>">Moj profil</a></li>
        <li runat="server" id="liCoupons"><a href="<%= Page.ResolveUrl("~/userAdmin/Coupons.aspx") %>">Moji kuponi</a></li>
    </ul>

</aside>


