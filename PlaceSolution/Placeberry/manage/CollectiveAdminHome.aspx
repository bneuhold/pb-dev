<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveAdminHome.aspx.cs" Inherits="manage_CollectiveAdminHome" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<div class="main_content">

    <div class="admin_links">
        <p><a href="<%= Page.ResolveUrl("~/manage/CollectiveOfferList.aspx") %>">Pregled ponuda</a></p>
        <p><a href="<%= Page.ResolveUrl("~/manage/CollectiveCategoryList.aspx") %>">Pregled kategorija</a></p>
        <p><a href="<%= Page.ResolveUrl("~/manage/CollectivePlacesList.aspx") %>">Pregled mjesta</a></p>
        <p><a href="<%= Page.ResolveUrl("~/manage/CollectiveCouponsList.aspx") %>">Pregled bonova</a></p>
    </div>

</div>

</asp:Content>
