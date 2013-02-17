<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OfferList.ascx.cs" Inherits="Controls_OfferList" %>

<%@ Import Namespace="System.Globalization"%>

    <asp:Repeater runat="server" ID="rptOffers">

    <ItemTemplate>

        <article class="small-offer">

            <div class="offer-pic">
                <a href="<%# "/" + DataBinder.Eval(Container.DataItem, "Translation.UrlTag") + "-" + Eval("OfferId") %>"><asp:Image runat="server" ID="imgOffer" /></a>
                <a href="<%# "/" + DataBinder.Eval(Container.DataItem, "Translation.UrlTag") + "-" + Eval("OfferId") %>" class="discount">-<%# Eval("Discount")%>%</a>
                <a href="<%# "/" + DataBinder.Eval(Container.DataItem, "Translation.UrlTag") + "-" + Eval("OfferId") %>" class="action buy-button">VIŠE</a>
            </div>

            <h1><a href="<%# "/" + DataBinder.Eval(Container.DataItem, "Translation.UrlTag") + "-" + Eval("OfferId") %>"><%# DataBinder.Eval(Container.DataItem, "Translation.Title") %></a></h1>

            <h3 class="subtitle">
                Za <strong><%# Eval("Price") %><%# Eval("CurrencySymbol")%></strong>&nbsp;<%# DataBinder.Eval(Container.DataItem, "Translation.ContentShort") %>
            </h3>

            <ul>

                <li class="info-box vrijednost"><span>VRIJEDNOST</span> <strong><%# Eval("PriceReal")%><%# Eval("CurrencySymbol")%></strong></li>

                <li class="info-box"><span>UŠTEDA</span><strong><%# Eval("Discount")%>%</strong></li>
                <li class="info-box cijena"><span>CIJENA</span><strong><%# Eval("Price") %><%# Eval("CurrencySymbol")%></strong></li>

                <li class="time-left">
                    Do isteka ponude ostalo je:
                    <div class="timer" data-time="<%# ((DateTime)Eval("DateEnd")).ToString("mm:ss MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")) %>"> <!-- 17:28 12/27/2012 -->
                        <span class="days">4</span>
                        dana
                        <span class="hours">12</span><span>:</span><span class="minutes">34</span><span>:</span><span class="seconds">56</span>
                    </div>
                </li>

            </ul>

        </article>

    </ItemTemplate>

    </asp:Repeater>

