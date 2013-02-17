<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponView.aspx.cs" Inherits="CouponView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>PUTOVALICA</h1><br />
        Naziv ponude: <%= _offer.Translation.Title %><br />
        Šifra kupona: <%= _coup.CodeNumber %><br />
        <br />
        <h2>Opis</h2>
        <%= _offer.Translation.ContentShort %>
        <h2>Napomena</h2>
        <%= _offer.Translation.ReservationText %>
        <br />
        <asp:PlaceHolder runat="server" ID="phAgency">
        <h2>Podaci o poduzeću</h2>
        Naziv: <%= _agency.Name %><br />
        Adresa: <%= _agency.Address %><br />
        Telefon: <%= _agency.ContactPhone %><br />
        Email: <%= _agency.ContactEmail %><br />
        </asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
