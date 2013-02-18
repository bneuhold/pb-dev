<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FirstOffer.ascx.cs" Inherits="Controls_FirstOffer" %>

<%@ Import Namespace="System.Globalization"%>

    <!-- kontrola izvucena kao prvi elment slidera. nema id-eve po kojima bi ih javascript kupio i vrtio -->

    <div class="slider">

        <div class="slider-inner">

            <article class="slide">

                <h1><a href="<%= "/" + _firstOffer.Translation.UrlTag + "-" + _firstOffer.OfferId.ToString() %>"><%= _firstOffer.Translation.Title %></a></h1>

                <h3 class="subtitle">
                    Za <strong><%= _firstOffer.Price.ToString() + _firstOffer.CurrencySymbol %></strong> <%= _firstOffer.Translation.Title %>
                </h3>

                <div class="offer-pic">
                    <a href="<%= "/" + _firstOffer.Translation.UrlTag + "-" + _firstOffer.OfferId.ToString() %>"><asp:Image runat="server" ID="imgOffer" /></a>
                    <a href="<%= "/" + _firstOffer.Translation.UrlTag + "-" + _firstOffer.OfferId.ToString() %>" class="discount">-<%= _firstOffer.Discount %>%</a>
                    <a href="<%= "/buy/" + _firstOffer.Translation.UrlTag + "-" + _firstOffer.OfferId.ToString() %>" class="action buy-button">KUPI</a>
                </div>

                <div class="offer-info">

                    <div class="price">
                        <span>CIJENA</span>
                        <span class="red-price big"><%= _firstOffer.Price %></span>
                        <span class="red-price small"><%= _firstOffer.CurrencySymbol %></span>
                    </div>

                    <ul class="price-sub">
                        <li class="vrijednost">VRIJEDNOST <strong><%= _firstOffer.PriceReal.ToString() + " " + _firstOffer.CurrencySymbol %></strong></li>
                        <li class="usteda">UŠTEDA <strong><%= _firstOffer.PriceSave.ToString() + " " + _firstOffer.CurrencySymbol %></strong></li>
                    </ul>

                    <!-- USPJESNA PONUDA -->

                    <div class="offer-status <%= isOfferSuccess ? "offer-success" : "offer-failed" %>">
                        <%= !isOfferSuccess ? "<span class='counter'>" + _firstOffer.BoughtCount + "</span>" : string.Empty %>
                        <p><strong><%= isOfferSuccess ? "PONUDA JE USPJELA" : "Prodano je " + _firstOffer.BoughtCount + " kupona"%></strong></p>
                        <div class="progress" >
                            <img id="progressBar" src="/resources/img/progressbar.png" alt="50%" style="right: 45%" />
                            <span class="percent"><%= isOfferSuccess ? "100" : Math.Round(((double)_firstOffer.BoughtCount / (double)_firstOffer.MinBoughtCount) * 100).ToString() %>%</span>
                        </div>
                        <p><%= isOfferSuccess ? "ima još " + (_firstOffer.MaxBoughtCount - _firstOffer.BoughtCount).ToString() + " komada" : "za prolaz je potrebno još " + (_firstOffer.MinBoughtCount - _firstOffer.BoughtCount).ToString()%></p>
                    </div>

                    <div class="time-left">
                        Do isteka ponude ostalo je:
                        <div class="timer" data-time="<%= _firstOffer.DateEnd.ToString("mm:ss MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) %>">
                            <span class="days">4</span>
                            dana
                            <span class="hours">12</span><span>:</span><span class="minutes">34</span><span>:</span><span class="seconds">56</span>
                        </div>
                    </div>

                    <div class="share">
                        <a href="#"><img src="/resources/img/mail.png" alt="Preporuči mailom" /></a>
                        <a href="#"><img src="/resources/img/facebook.png" alt="Preporuči na facebooku" /></a>
                        <a href="#"><img src="/resources/img/twitter.png" alt="Preporuči na twitteru" /></a>
                    </div>

                </div>

            </article>

            </div>
        
    </div>