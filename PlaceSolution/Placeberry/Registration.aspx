<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true"
    CodeFile="Registration.aspx.cs" Inherits="Registration" UICulture="auto" Culture="auto"
    meta:resourcekey="PageResource1" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <h1>Registracija</h1>

    <h2>Korisničko ime</h2>
    <asp:TextBox ID="tbxUserName" runat="server" />
    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="tbxUserName" runat="server" ValidationGroup="register" />

    <h2>Lozinka</h2>
    <asp:TextBox ID="tbxPassword" TextMode="Password" runat="server" />
    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="tbxPassword" runat="server" ValidationGroup="register" />

    <h2>Potvrdi lozinku</h2>
    <asp:TextBox ID="tbxPasswordConfirm" TextMode="Password"  runat="server" />
    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="tbxPasswordConfirm" runat="server" ValidationGroup="register" />
    <asp:CompareValidator ErrorMessage="Lozinke se ne podudaraju" ControlToValidate="tbxPasswordConfirm" ControlToCompare="tbxPassword" runat="server" />

    <h2>E-mail</h2>
    <asp:TextBox ID="tbxEmail" runat="server" />
    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="tbxEmail" runat="server" ValidationGroup="register" />
    <asp:RegularExpressionValidator  ControlToValidate="tbxEmail" runat="server" ErrorMessage="Neispravna e-mail adresa" ValidationExpression="[\w_.\-+]{4,}@[\w_\-+]{2,}([\w_\-+]+.?)*\.[\w]{2,3}" ValidationGroup="register" />

    <cc1:CaptchaControl ID="ccJoin" runat="server" CaptchaBackgroundNoise="none" CaptchaLength="5" CaptchaHeight="60" CaptchaWidth="200" CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" />
    <asp:TextBox ID="tbxCaptcha" runat="server" Text="" />
    <asp:Literal ID="ltlCaptchaError" Text="Krivi unos" runat="server" Visible="false" />

    <br />
    <br />
    <asp:Literal ID="ltlStatusErrorMessage" Text="" runat="server" />
    <asp:Button ID="btnSubmit" Text="Submit" runat="server" onclick="btnSubmit_Click"  />
</asp:Content>
