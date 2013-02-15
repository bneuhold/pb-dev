<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="/Controls/Sidebar.ascx" TagName="Sidebar" TagPrefix="uc1" %>
<%@ Register Src="/Controls/FirstOffer.ascx" TagName="FirstOffer" TagPrefix="uc1" %>
<%@ Register Src="/Controls/OfferList.ascx" TagName="OfferList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <uc1:Sidebar runat="server" ID="ctrlSideBar" />

    <!-- START .content -->

    <div class="content">

    <uc1:FirstOffer runat="server" ID="ctrlFirstOffer" />

    <uc1:OfferList runat="server" ID="ctrlOfferList" />

    </div>
    
    <!-- END .content -->

</asp:Content>