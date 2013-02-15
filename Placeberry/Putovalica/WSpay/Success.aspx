<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Success.aspx.cs" Inherits="WSpay_Success" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<h1 style="font-size:30px;">Čestitamo na uspješnoj kupovini.</h1>
<p>U svojem profilu možete vidjeti kupljene bonove.</p>
<p style="text-decoration:underline; margin-bottom:8px; margin-top:20px;">Testni rezultat:</p>
<asp:Literal runat="server" ID="ltQS"></asp:Literal>
<br />
<p style="text-decoration:underline; margin-bottom:8px; margin-top:20px;">Rezultat spremanja kupona:</p>
<asp:Literal runat="server" ID="ltCouponSaveMsg"></asp:Literal>

</asp:Content>
