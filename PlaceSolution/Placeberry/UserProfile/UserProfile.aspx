<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile_UserProfile" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="width:100%; text-align:center; margin:150px 0 150px 0;">
        <a href="<%= Page.ResolveUrl("~/UserProfile/UserCoupons.aspx") %>">Moji kuponi</a>
    </div>

</asp:Content>