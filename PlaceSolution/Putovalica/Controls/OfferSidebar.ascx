<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OfferSidebar.ascx.cs" Inherits="Controls_OfferSidebar" %>

<%@ Import Namespace="System.Globalization"%>

    <!-- START .sidebar -->


    <asp:Repeater runat="server" ID="rptOtherOffers">
        <HeaderTemplate>
        <aside class="sidebar">

            <h3 class="sidebar-title">Vezane Ponude</h3>
        </HeaderTemplate>

        <ItemTemplate>
        <article id="offer1">

            <h2><a href="<%# Page.ResolveUrl("~/Offer.aspx") + "?offerid=" + Eval("OfferId") %>"><%# DataBinder.Eval(Container.DataItem, "Translation.Title") %></a></h2>

            <div class="offer-pic">
                <a href="<%# Page.ResolveUrl("~/Offer.aspx") + "?offerid=" + Eval("OfferId") %>"><asp:Image runat="server" ID="imgOffer" width="154" height="86" /></a>
                <a href="<%# Page.ResolveUrl("~/Offer.aspx") + "?offerid=" + Eval("OfferId") %>" class="discount">-50%</a>
                <a href="<%# Page.ResolveUrl("~/Offer.aspx") + "?offerid=" + Eval("OfferId") %>" class="action more-info">VIŠE</a>
            </div>

            <h3 class="subtitle">Za <strong><%# Eval("Price")%></strong> <%# DataBinder.Eval(Container.DataItem, "Translation.ContentShort") %></h3>

            <ul class="details">
                <li class="vrijednost">
                    VRIJEDNOST
                    <span><%# Eval("PriceReal")%> <%# Eval("CurrencySymbol")%></span>
                </li>
                <li class="usteda">
                    UŠTEDA
                    <span><%# Eval("Discount")%>%</span>
                </li>
                <li class="cijena">
                    CIJENA
                    <span><%# Eval("Price")%> <%# Eval("CurrencySymbol")%></span>
                </li>
            </ul>

            <div class="time-left">
                Do isteka ponude ostalo je:
                <div class="timer" data-time="<%# ((DateTime)Eval("DateEnd")).ToString("mm:ss MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) %>">
                    <span class="days">4</span>
                    dana
                    <span class="hours">12</span><span>:</span><span class="minutes">34</span><span>:</span><span class="seconds">56</span>
                </div>
            </div>

        </article>

        </ItemTemplate>

        <FooterTemplate>
            </aside> <!-- END .sidebar -->
        </FooterTemplate>
    </asp:Repeater>

