<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Offer.aspx.cs" Inherits="Offer" MasterPageFile="~/MasterPage.master" %>

<%@ Import Namespace="System.Globalization"%>
<%@ Register Src="/Controls/OfferSidebar.ascx" TagName="OfferSidebar" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <uc1:OfferSidebar runat="server" ID="ctrlOfferSidebar" />


    <!-- START .content -->

    <div class="content">
        <article class="main-offer" id="offer-main">

            <h1><%= currOffer.Translation.Title %></h1>

            <h3 class="subtitle">
                Za <strong><%= currOffer.Price.ToString() + currOffer.CurrencySymbol %></strong> <%= currOffer.Translation.ContentShort %>
            </h3>

            <div class="offer-pic">
                <span><img src=".<%= String.IsNullOrEmpty(currOffer.FirstImgSrc) ? "/uploads/offerimages/default.jpg" : currOffer.FirstImgSrc %>" alt="<%= currOffer.Translation.Title %>"></span>
                <span class="discount">-<%= currOffer.Discount %>%</span>
                <a href="<%= "/buy/" + currOffer.Translation.UrlTag + "-" + currOffer.OfferId.ToString() %>" class="action buy-button">KUPI</a>
            </div>

            <div class="offer-info">

                <div class="price">
                    <span>CIJENA</span>
                    <span class="red-price big"><%= currOffer.Price %></span>
                    <span class="red-price small"><%= currOffer.CurrencySymbol %></span>
                </div>

                <ul class="price-sub">
                    <li class="vrijednost">VRIJEDNOST <strong><%= currOffer.PriceReal.ToString() %> <%= currOffer.CurrencySymbol %></strong></li>
                    <li class="usteda">UŠTEDA <strong><%= currOffer.PriceSave %> <%= currOffer.CurrencySymbol %></strong></li>
                </ul>

                <!-- USPJESNA PONUDA -->

                <div class="offer-status <%= isOfferSuccess ? "offer-success" : "offer-failed" %>">
                    <%= !isOfferSuccess ? "<span class='counter'>" + currOffer.BoughtCount + "</span>" : string.Empty %>
                    <p><strong><%= isOfferSuccess ? "PONUDA JE USPJELA" : "Prodano je " + currOffer.BoughtCount + " kupona"%></strong></p>
                    <div class="progress" >
                        <img id="progressBar" src="/resources/img/progressbar.png" alt="50%" style="right: 45%" />
                        <span class="percent"><%= isOfferSuccess ? "100" : Math.Round(((double)currOffer.BoughtCount / (double)currOffer.MinBoughtCount) * 100).ToString() %>%</span>
                    </div>
                    <p><%= isOfferSuccess ? "ima još " + (currOffer.MaxBoughtCount - currOffer.BoughtCount).ToString() + " komada" : "za prolaz je potrebno još " + (currOffer.MinBoughtCount - currOffer.BoughtCount).ToString()%> </p>
                </div>

                <div class="time-left">
                    Do isteka ponude ostalo je:
                    <div class="timer" data-time="<%= currOffer.DateEnd.ToString("mm:ss MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) %>">
                        <span class="days">4</span>
                        dana
                        <span class="hours">12</span><span>:</span><span class="minutes">34</span><span>:</span><span class="seconds">56</span>
                    </div>
                </div>

                <div class="share">
                    <span>PREPORUČI PONUDU</span>
                    <a href="#"><img src="/resources/img/mail.png" alt="Preporuči mailom" /></a>
                    <a href="#"><img src="/resources/img/facebook.png" alt="Preporuči na facebooku" /></a>
                    <a href="#"><img src="/resources/img/twitter.png" alt="Preporuči na twitteru" /></a>
                </div>

            </div> <!-- END .offer-info -->

            <div class="description">

                <div class="details">
                    <%= currOffer.Translation.ContentText %>
<%--                    <h4>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.</h4>
                    <h4>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. <strong>12.15.2012.</strong></h4>

                    <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque <strong>penatibus et magnis dis parturient</strong> montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.</p>

                    <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. <strong>Aenean commodo</strong> ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.</p>

                    <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. <strong>Aenean massa</strong>. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, <strong>pellentesque</strong> eu, pretium quis, sem. Nulla consequat massa quis enim.</p>
--%>

                    <a class="buy-button" href="<%= "/buy/" + currOffer.Translation.UrlTag + "-" + currOffer.OfferId.ToString() %>">KUPI</a>

                    <asp:PlaceHolder runat="server" ID="phGMapsIframe">
<%--                        <iframe class="map" width="500" height="180" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/?ie=UTF8&amp;ll=45.810854,15.972404&amp;spn=0.148609,0.231056&amp;t=m&amp;z=12&amp;output=embed"></iframe>--%>
                            <!-- http://wiki.panotools.org/Geo-referencing_panoramas_with_Google_Maps -->
                            <iframe class="map" width="500" height="180" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/?ie=UTF8&amp;q=<%= currOffer.Longitude.Value.ToString(CultureInfo.CreateSpecificCulture("en-US")) %>,<%= currOffer.Latitude.Value.ToString(CultureInfo.CreateSpecificCulture("en-US")) %>&amp;t=m&amp;z=15&amp;output=embed"></iframe>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder runat="server" ID="phAgencyInfo">
                        <%--<div class="map-info"><p>Lokacija</p> <p>Adresa</p> <p>Kontakt</p></div>--%>
                        <div class="map-info">
                            <p>Lokacija: <%= offerAgency.City %>, <%= offerAgency.Country %></p>
                            <p>Adresa: <%= offerAgency.Address %></p>
                            <p>Kontakt: <asp:Literal runat="server" ID="ltContact"></asp:Literal> </p></div>
                    </asp:PlaceHolder>


                </div>

                <aside class="notes">
                    <h4>NAPOMENE</h4>
                        <%= currOffer.Translation.ReservationText %>
<%--                    <ul class="notes-list">
                        <li><strong>Lorem ipsum</strong> dolor sit amet, consectetuer adipiscing elit.</li>
                        <li>Aenean commodo ligula eget dolor. <strong>Aenean massa</strong>.</li>
                        <li>Donec quam felis, ultricies nec, <strong>pellentesque</strong> eu, pretium quis, sem.</li>
                        <li>Aenean commodo ligula eget dolor. <strong>Aenean massa</strong>.</li>
                        <li><strong>Lorem ipsum</strong> dolor sit amet, consectetuer adipiscing elit.</li>
                        <li><strong>Lorem ipsum</strong> dolor sit amet, consectetuer adipiscing elit.</li>
                        <li>Aenean commodo ligula eget dolor. <strong>Aenean massa</strong>.</li>
                    </ul>
--%>                </aside>

            </div>

        </article>

    </div> <!-- END .content -->

</asp:Content>