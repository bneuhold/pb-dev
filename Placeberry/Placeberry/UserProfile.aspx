<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="width:100%; text-align:center;">
        <a href="<%= Page.ResolveUrl("~/UserProfile/UserCoupons.aspx") %>">Moji kuponi</a>
    </div>

</asp:Content>