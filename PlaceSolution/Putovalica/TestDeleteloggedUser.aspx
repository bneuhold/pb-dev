<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestDeleteloggedUser.aspx.cs" Inherits="TestDeleteloggedUser" MasterPageFile="~/MasterSimple.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="login-register" style="background-color:transparent;">

        <asp:Button runat="server" ID="btnDeleteUser" Text="Delete Logged User!" />

        <br />
        <br />
        <br />
        <br />
        <asp:TextBox runat="server" ID="tbUserName"></asp:TextBox>
        <br />
        <asp:Button runat="server" ID="btnChangePass" Text="Change Password" />
        <br />
        <asp:Label runat="server" ID="lblNewPassword"></asp:Label>

    </div>


</asp:Content>