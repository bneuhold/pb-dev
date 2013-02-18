<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveTest.aspx.cs" Inherits="CollectiveTest" MasterPageFile="~/MasterHome.master" %>

<%@ Register src="Controls/CollectiveOfferList.ascx" tagname="OfferList" tagprefix="ctrl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <asp:Button runat="server" ID="btnTest" Text="Test" />
        <asp:Label runat="server" ID="lblResult"></asp:Label>
    <br />

    <ctrl:OfferList runat="server" ID="ctrlOfferList" />

        <br />

</asp:Content>