<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterManage.master" CodeFile="BookingAdmin.aspx.cs" Inherits="manage_BookingAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/css/booking_admin.css"/>        
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="header-bar">
        <asp:Label runat="server" ID="lblAgencyName"></asp:Label> - pregled rezervacija
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <div class="main_content" style="width:800px; min-height:400px;">

    <div class="accomm_list">
        <asp:Repeater ID="repAccommodation" runat="server">
            <HeaderTemplate>
                <div class="list-wrapper in-content">
                <h2 class="title"><%= Resources.booking.AdminAccommList %>:</h2>
                <ul class="link-list">
            </HeaderTemplate>
            <ItemTemplate>
                    <li class="sub-item">
                        <span><a href="<%# "/manage/bookingadmin.aspx?agencyId=" + GetAgency().Id + "&accommid=" + Eval("Id") %>"><%# Eval("Name") %></a></span> 
                    </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <asp:PlaceHolder runat="server" ID="phSelectedAccomm">
        <div style="width:100%; display:inline-block; margin:10px 0; text-align:center; font-size:15px;">
        Pregled rezervacija za smještaj <asp:Label runat="server" ID="lblSelAccommName"></asp:Label>
        </div>
    </asp:PlaceHolder>

    <asp:UpdatePanel runat="server" id="UpdatePanel" RenderMode="Block">
    <ContentTemplate>

        <asp:HiddenField runat="server" ID="hfSelectedBookingId" EnableViewState="true" />

        <asp:PlaceHolder runat="server" ID="phCreateBook">
            <div style="clear:both; float:left; display:inline; margin:20px 0; width:100%;">
                <asp:HyperLink runat="server" ID="hlCreateBook">Kreiraj rezervaciju</asp:HyperLink>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="phNoBookingsMsg">
            <div style="display:inline-block; width:100%; text-align:center;">
            Nema rezervacija za odabrani smještaj.
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="phBookingsCalendars">

        <div class="calendar_container">

            <div class="calendar_admin" style="float:left;">
                <asp:Calendar ID="cal1" runat="server"
                BorderStyle="Solid" BorderColor="#999999" BorderWidth="1px" DayStyle-ForeColor="#404040" SelectedDayStyle-BackColor="#0099E6" SelectedDayStyle-ForeColor="White" DayStyle-BorderStyle="None" DayStyle-BorderWidth="0">
                    <TitleStyle ForeColor="#1E81A9" />
                    <NextPrevStyle ForeColor="#1E81A9" />
                    <TitleStyle BackColor="#EDEDED" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                </asp:Calendar>
            </div>

            <div class="calendar_admin" style="display:inline-block; margin:0 auto;">
                <asp:Calendar ID="cal2" runat="server"
                BorderStyle="Solid" BorderColor="#999999" BorderWidth="1px" DayStyle-ForeColor="#404040" SelectedDayStyle-BackColor="#0099E6" SelectedDayStyle-ForeColor="#F7F7F7" DayStyle-BorderStyle="None" DayStyle-BorderWidth="0">
                    <TitleStyle ForeColor="#1E81A9" />
                    <NextPrevStyle ForeColor="#1E81A9" />
                    <TitleStyle BackColor="#EDEDED" />
                    <OtherMonthDayStyle ForeColor="#999999" />     
                </asp:Calendar>
            </div>

            <div class="calendar_admin" style="float:right;">
                <asp:Calendar ID="cal3" runat="server"
                BorderStyle="Solid" BorderColor="#999999" BorderWidth="1px" DayStyle-ForeColor="#404040" SelectedDayStyle-BackColor="#0099E6" SelectedDayStyle-ForeColor="#F7F7F7" DayStyle-BorderStyle="None" DayStyle-BorderWidth="0">
                    <TitleStyle ForeColor="#1E81A9" />
                    <NextPrevStyle ForeColor="#1E81A9" />
                    <TitleStyle BackColor="#EDEDED" />
                    <OtherMonthDayStyle ForeColor="#999999" />     
                </asp:Calendar>
            </div>

        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="phSelectedBooking">
            <div style="width:100%; text-align:center;">
                <table style="margin:0 auto;">
                <tr>
                    <td style="text-align:right;">Početak rezervacije:</td><td><asp:Label runat="server" ID="lblSelBookDateFrom"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Kraj rezervacije:</td><td><asp:Label runat="server" ID="lblSelBookDateTo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Status:</td><td><asp:Label runat="server" ID="lblSelBookStatus"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Osnovna cijena:</td><td><asp:Label runat="server" ID="lblSelBookPriceBasic"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Konačna cijena:</td><td><asp:Label runat="server" ID="lblSelBookPricaSum"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Broj osoba:</td><td><asp:Label runat="server" ID="lblSelBookNumOfPersons"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Broj djece do 3 godine:</td><td><asp:Label runat="server" ID="lblSelBookNumOfBabies"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Datum kreiranja rezervacije:</td><td><asp:Label runat="server" ID="lblSelBookCreateDate"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Kreirano od administratora:</td><td><asp:Label runat="server" ID="lblSelBookAdminCreateName"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Datum zandnje promjene:</td><td><asp:Label runat="server" ID="lblLastUpdateDate"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:center; height:40px; vertical-align:middle; padding-top:10px; font-weight:bold;" colspan="2">Podaci o gostu:</td>
                </tr>
                <tr>
                    <td style="text-align:right;">Ime:</td><td><asp:Label runat="server" ID="lblUserFirstName"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Prezime:</td><td><asp:Label runat="server" ID="lblUserLastName"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Email:</td><td><asp:Label runat="server" ID="lblUserEmail"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Phone:</td><td><asp:Label runat="server" ID="lblUserPhone"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Country:</td><td><asp:Label runat="server" ID="lblUserCountry"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">City:</td><td><asp:Label runat="server" ID="lblUserCity"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:right;">Street:</td><td><asp:Label runat="server" ID="lblUserStreet"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align:center; vertical-align:bottom; height:50px;">
                        <asp:LinkButton runat="server" ID="lbDeleteBook" OnClientClick="return confirm('Dali ste sigurni da želite obrisati rezervaciju?');">Obriši rezervaciju</asp:LinkButton>
                    </td>
                    <td style="text-align:center; vertical-align:bottom; height:50px;">
                        <asp:HyperLink runat="server" ID="hlEditBook">Uredi rezervaciju</asp:HyperLink>
                    </td>
                </tr>
                </table>
            </div>
        </asp:PlaceHolder>

        
        <a href="/manage/Customer.aspx" class="agency-link button-header-back ui-corner-all">korisničke stranice</a>
        </ContentTemplate>

    </asp:UpdatePanel>

    </div>

</asp:Content>
