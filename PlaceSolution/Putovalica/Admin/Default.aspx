<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" MasterPageFile="~/MasterAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/styles/admin.css"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="main_content">

    <div class="admin_links">
        <p><a href="<%= Page.ResolveUrl("~/admin/OfferList.aspx") %>">Pregled ponuda</a></p>
        <p><a href="<%= Page.ResolveUrl("~/admin/CategoryList.aspx") %>">Pregled kategorija</a></p>
        <p><a href="<%= Page.ResolveUrl("~/admin/PlacesList.aspx") %>">Pregled mjesta</a></p>
        <p><a href="<%= Page.ResolveUrl("~/admin/CouponsList.aspx") %>">Pregled kupona</a></p>
    </div>

</div>


</asp:Content>
