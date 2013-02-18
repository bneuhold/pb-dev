<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Controls_Header" %>

<%@ Register Src="/Controls/PlaceList.ascx" TagName="PlaceList" TagPrefix="uc1" %>

<!--[if lt IE 7]>
    <p class="chromeframe">You are using an outdated browser. <a href="http://browsehappy.com/">Upgrade your browser today</a> or <a href="http://www.google.com/chromeframe/?redirect=true">install Google Chrome Frame</a> to better experience this site.</p>
<![endif]-->

<header class="header">
    <div class="container">

        <div class="header-top">
            <a class="logo" id="logo" href="/">
                <img src="/resources/img/logo.png" alt="Putovalica" />
            </a>

        <uc1:PlaceList runat="server" ID="ctrlPlaceList" />

        <asp:PlaceHolder runat="server" ID="phLogout">
            <ul class="user-menu">
                <%--<li><a href="#"><img src="/resources/img/fb-icon.png" alt="Facebook connect" />Connect</a></li>--%>
                <li><asp:Label runat="server" ID="lblUserName"></asp:Label></li>
                <li><a href="<%= Page.ResolveUrl("~/useradmin") %>"><%= Resources.putovalica.LoginCtrl_Profile %></a></li>
                <li><asp:LinkButton runat="server" ID="lbLogout" Text="<%$ Resources:putovalica, LoginCtrl_Logout %>" /></li>
            </ul>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="phLogin">
            <ul class="user-menu">
                <%--<li><a href="#"><img src="/resources/img/fb-icon.png" alt="Facebook connect" />Connect</a></li>--%>
                <li><asp:LinkButton runat="server" ID="lbLogin" Text="<%$ Resources:putovalica, LoginCtrl_Login %>" /></li>
                <li><a href="<%= Page.ResolveUrl("~/Registration.aspx") %>">REGISTRIRAJ SE</a></li>
                <li><a class="head-fb" href="javascript:login()" id="facebook-connect">FEJS LOGIN</a></li>
            </ul>
        </asp:PlaceHolder>

        </div>

        <div class="header-nav">
            <ul class="main-nav">
                <li><a class="active" href="#">Dnevna ponuda</a></li>
                <li><a href="#">Aktivne ponude</a></li>
                <li><a href="#">Prošle ponude</a></li>
                <li><a href="#">Kako radi putovalica.hr</a></li>
            </ul>
        </div>

    </div>
</header>
