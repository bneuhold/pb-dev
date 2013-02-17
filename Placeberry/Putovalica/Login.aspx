<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" MasterPageFile="~/MasterSimple.master" %>

<%@ Register Src="/Controls/FormLoginCtrl.ascx" TagName="Login" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="login-register" style="height:680px;">
    <div class="login-register-inner">
        <uc1:Login runat="server" ID="ctrlLogin" />
        <p style="margin-top:20px;">Niste registrirani? Registrirajte se <a class="link_style" href="<%= Page.ResolveUrl("~/Registration.aspx") %>">ovdje</a>.</p>

    </div>
</div>

</asp:Content>