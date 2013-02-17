<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="Directory.aspx.cs" Inherits="Directory" EnableViewState="false" %>

<%@ Register Src="Controls/Sidebar.ascx" TagName="Sidebar" TagPrefix="uc1" %>
<%@ Register src="Controls/Adverts.ascx" tagname="Adverts" tagprefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="content" class="clearfix">
        <div id="results_wrap" class="clearfix">
            <div class="page">
                <div class="page_container">
                    <asp:Panel ID="pnlCurrentTerm" runat="server">
                        
                        <asp:Repeater ID="repBreadcrumbs" runat="server">
                            <HeaderTemplate>
                                <ul id="breadcrumbs">
                            </HeaderTemplate>
                            <ItemTemplate>
                                    <li><a href="<%# Eval("UrlLink") %>"><%# Eval("Title") %></a></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                                <div class="clear"></div>
                            </FooterTemplate>
                        </asp:Repeater>

                        <h1><asp:Literal ID="ltlCurrentTerm" runat="server" Text="<%$ Resources:placeberry, Directory_DefaultTerm %>" /></h1>
                        <p><asp:Literal runat="server" Text="<%$ Resources:placeberry, Directory_SearchCurrentTermText %>" />
                        <a id="aCurrentTerm" runat="server" href="/"><asp:Literal runat="server" Text="<%$ Resources:placeberry, Directory_DefaultTerm %>" />
                            </a></p>
                    </asp:Panel>

                
                        <div id="children" style="float:left; width:25%">
                            
                            <asp:Repeater ID="repChildren" runat="server">
                                <HeaderTemplate>
                                    <p><asp:Literal runat="server" Text="<%$ Resources:placeberry, Directory_ChildrenText %>" /></p>
                                    <ul class="child_list">
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <li>
                                            <a href="<%# Eval("UrlLink") %>"><%# Eval("Title") %></a>
                                        </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                    <br />
                                </FooterTemplate>
                            </asp:Repeater>
                            
                        </div>
                    
                        <div id="details" style="float:left; width:75%;">

                            <asp:Panel ID="pnlDescription" runat="server">
                                <p><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:placeberry, Directory_DescriptionText %>" /></p>
                                <p id="description">
                                    <asp:Literal ID="ltlDescription" runat="server" Text="<%$ Resources:placeberry, Directory_DefaultDescription %>" />
                                </p>
                                <br />
                            </asp:Panel>

                         </div>


                        <div style="clear:both;"></div>

                        <div id="topads">
                            <p><asp:Literal ID="ltlTopAdsText" runat="server" Text="<%$ Resources:placeberry, Directory_TopAdsText %>" /></p>
                            <adv:Adverts ID="TopAdverts" runat="server" />
                        </div>

                    
                </div>
            </div>
            <uc1:Sidebar runat="server" ID="Sidebar"></uc1:Sidebar>            
        </div>
    </div>

</asp:Content>

