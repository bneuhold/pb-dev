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

            <fieldset class="editor basic-info ui-corner-all centred">
                <legend>Rezervacija</legend>

                <div class="form-editor">

		            <div class="form-row">
			            <div class="form-cell label">Početak rezervacije</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookDateFrom"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Kraj rezervacije</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookDateTo"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Status</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookStatus"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Osnovna cijena</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookPriceBasic"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Konačna cijena</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookPricaSum"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Broj osoba</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookNumOfPersons"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Broj djece do 3 godine</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookNumOfBabies"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Datum kreiranja rezervacije</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookCreateDate"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Kreirano od administratora</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblSelBookAdminCreateName"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Datum zadnje promjene</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblLastUpdateDate"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row separator">
			            <div class="form-cell label">
                            <div class="line-holder">
                                <div class="line">
                                    Podaci o gostu
                                </div>
                            </div>
                        </div>
			            <div class="form-cell value">
                            
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Ime</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserFirstName"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Prezime</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserLastName"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Email</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserEmail"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Phone</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserPhone"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Country</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserCountry"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">City</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserCity"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
                    <div class="form-row">
			            <div class="form-cell label">Street</div>
			            <div class="form-cell value">
                            <asp:Label runat="server" ID="lblUserStreet"></asp:Label>
			            </div>
			            <div class="close-row"></div>
		            </div>
	            </div>
                <div class="action-bar no-left-padding">
                    <asp:LinkButton runat="server" ID="lbDeleteBook" CssClass="delete-button button-like ui-corner-all" OnClientClick="return confirm('Dali ste sigurni da želite obrisati rezervaciju?');">Obriši rezervaciju</asp:LinkButton>
                    <asp:HyperLink runat="server" CssClass="edit-button button-like ui-corner-all" ID="hlEditBook">Uredi rezervaciju</asp:HyperLink>
                </div>
            </fieldset>
        </asp:PlaceHolder>

        
        <a href="/manage/Customer.aspx" class="agency-link button-header-back ui-corner-all">korisničke stranice</a>
        </ContentTemplate>

    </asp:UpdatePanel>

    </div>

</asp:Content>
