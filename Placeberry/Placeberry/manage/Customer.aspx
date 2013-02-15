<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Customer" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1> Korisničke stranice </h1>
    <br /><br />

    <asp:PlaceHolder ID="plhAdminOptions" runat="server">    
        <a href="/manage/agency.aspx?action=newagency">Stvori novu agenciju</a>
        <br />
        <a href="/manage/ultimatetable.aspx">UltimateTable editiranje</a>
        <br />
        <a href="/manage/ultimatetableobjecttypes.aspx">UltimateTableObjectTypes editiranje</a>
        <br />
        <a href="/manage/languages.aspx">Languages editiranje</a>
        <br />
        <a href="/manage/regexlanguagerules.aspx">RegexLanguageRules editiranje</a>
        <br />
        
    </asp:PlaceHolder>

    <asp:Repeater runat="server" ID="repAgencies" >
        <HeaderTemplate>
            <h2>Agencije:</h2>
        </HeaderTemplate>
        <ItemTemplate>
            <h3><a href="/manage/Agency.aspx?agencyId=<%# Eval("Id") %>"><%# Eval("Name") %></a></h3>
        </ItemTemplate>
        <SeparatorTemplate>

        </SeparatorTemplate>
    </asp:Repeater>

    <br />
    <br />
</asp:Content>

