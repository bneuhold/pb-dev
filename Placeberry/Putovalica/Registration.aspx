<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" MasterPageFile="~/MasterSimple.master" %>

<%@ Register Src="/Controls/FormRegistrationCtrl.ascx" TagName="Registration" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="login-register" style="height:890px;">
    <div class="login-register-inner">
        <uc1:Registration runat="server" ID="ctrlRegistration" />
    </div>
</div>

</asp:Content>