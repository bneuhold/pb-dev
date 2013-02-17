<%@ Page Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" Culture="auto" UICulture="auto" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Import Namespace="System.Globalization" %>

<%@ Register src="Controls/SeoDirectory.ascx" tagname="SeoDirectory" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="description" content="<%$ Resources:placeberry, Home_MetaDescription %>"
        runat="server" id="metaDescription" />
    <meta name="keywords" content="<%$ Resources:placeberry, Home_MetaKeywords %>" runat="server" id="metaKeywords" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content">
        <a href="Default.aspx">
            <img class="logo_index" runat="server" id="imgLogo" src="~/resources/images/logo_placeberry.png" alt="<%$ Resources:placeberry, General_Slogan %>" /></a>
        <asp:Panel runat="server" ID="pnlSearch" CssClass="search_index clearfix" DefaultButton="btnSearch">
            <label for="input">
                <asp:Literal runat="server" ID="litDesc" Text="<%$ Resources:placeberry, Home_QueryQuestion %>"></asp:Literal></label>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="<%$ Resources:placeberry, Home_SearchText %>" />
            <div class="inlangs">
                <asp:Literal runat="server" ID="litLangs" Text="<%$ Resources:placeberry, Home_OtherLanguages %>"></asp:Literal>
                <a href="?lang=hr">
                    <img src="resources/images/hr.png" width="18" height="12" alt="Hrvatski" />Hrvatski</a>
                | <a href="?lang=en">
                    <img src="resources/images/gb.png" width="18" height="12" alt="English" />English</a>
                | <a href="?lang=it">
                    <img src="resources/images/it.png" width="18" height="12" alt="Italiano" />Italiano</a>
                | <a href="?lang=de">
                    <img src="resources/images/de.png" width="18" height="12" alt="Deutsch" />Deutsch</a>
                | <a href="?lang=cz">
                    <img src="resources/images/cz.png" width="18" height="12" alt="Český" />Český</a></div>
        </asp:Panel>
        <div>
           <div class="home_seo_directory">
            <uc1:SeoDirectory ID="SeoDirectory1" runat="server" LandingPage="<%$ Resources:placeberry, General_VacationUrl %>" ParentTerm="" />
           </div>
        </div>
        <div class="popular_searches">
            <a class="ps" runat="server" href="<%$ Resources:placeberry, URL_PopularQueries %>">
                <asp:Literal runat="server" ID="litPopular" Text="<%$ Resources:placeberry, Home_PopularQueries %>"></asp:Literal>
                »</a>

            <asp:Repeater ID="repPopularQueries" runat="server">
                <ItemTemplate>
                    <a href='<%# Eval("Link") %>'><%# Eval("Title") %></a>
                </ItemTemplate>
                <SeparatorTemplate>
                    &nbsp;|&nbsp;
                </SeparatorTemplate>
            </asp:Repeater>

        </div>
        <a href="http://www.youtube.com/watch?v=wK-3RZI8i24" target="_blank" class="watch_video">
            <asp:Literal runat="server" ID="litVideo" Text="<%$ Resources:placeberry, Home_WatchVideo %>"></asp:Literal>
            »</a>
    </div>
</asp:Content>
