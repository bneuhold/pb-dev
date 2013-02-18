<%@ Page Title="" Language="C#" MasterPageFile="~/MasterManage.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Customer" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="header-bar">Korisničke stranice</div>

    <asp:PlaceHolder ID="plhAdminOptions" runat="server">  
        <div class="super-admin-options in-content">  
            <ul>
                <li><a href="/manage/agency.aspx?action=newagency">Stvori novu agenciju</a></li>
                <li><a href="/manage/ultimatetable.aspx">UltimateTable editiranje</a></li>
                <li><a href="/manage/ultimatetableobjecttypes.aspx">UltimateTableObjectTypes editiranje</a></li>
                <li><a href="/manage/languages.aspx">Languages editiranje</a></li>
                <li><a href="/manage/regexlanguagerules.aspx">RegexLanguageRules editiranje</a></li>
            </ul>
        </div>
        
    </asp:PlaceHolder>

    <asp:Repeater runat="server" ID="repAgencies" >
        <HeaderTemplate>
            <div class="list-wrapper agency in-content">
                <h2 class="title">Agencije:</h2>
                <ul class="link-list">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="sub-item"><h3><a href="/manage/Agency.aspx?agencyId=<%# Eval("Id") %>"><%# Eval("Name") %></a></h3></li>
        </ItemTemplate>
        <SeparatorTemplate>

        </SeparatorTemplate>
        <FooterTemplate>
                </ul>
            </div>
            <div class="clearfix"></div>
        </FooterTemplate>
    </asp:Repeater>

    <br />
    <br />
</asp:Content>

