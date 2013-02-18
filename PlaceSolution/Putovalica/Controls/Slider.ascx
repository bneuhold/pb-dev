<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Slider.ascx.cs" Inherits="Controls_Slider" %>

    <div class="slider" id="slider">

        <!-- one tockice. moraju odgovarati broju elemenata u repeater, a maksimalno ih moze biti 4 -->
        <div class="controls" id="controls">
            <a class="active" href="#"></a>
            <a href="#"></a>
            <a href="#"></a>
            <a href="#"></a>
        </div>

        <asp:Repeater runat="server" ID="rptSidebarOffers">

        <HeaderTemplate>
            <div class="slider-inner" id="sliderInner">
        </HeaderTemplate>

        <ItemTemplate>

            <article class="slide" id="offer-1">

                <h1><a href="#">Čaroban odmor u Dubrovniku, gradu živopisnih ulica!</a></h1>

                <h3 class="subtitle">
                    Za <strong>700kn</strong> cekaju vas dva nocenja s doruckom..
                </h3>

                <div class="offer-pic">
                    <a href="#"><img src="/resources/img/sample/offer.jpg" alt="Dubrovnik!" /></a>
                    <a href="#" class="discount">-50%</a>
                    <a href="#" class="action buy-button">KUPI</a>
                </div>

                <div class="offer-info">

                    <div class="price">
                        <span>CIJENA</span>
                        <span class="red-price big">780</span>
                        <span class="red-price small">kn</span>
                    </div>

                    <ul class="price-sub">
                        <li class="vrijednost">VRIJEDNOST <strong>1.278 kn</strong></li>
                        <li class="usteda">UŠTEDA <strong>733 kn</strong></li>
                    </ul>

                    <!-- USPJESNA PONUDA -->

                    <div class="offer-status offer-failed">
                        <span class="counter">4</span>
                        <p><strong>Prodano je 6 kupona</strong></p>
                        <div class="progress" >
                            <img id="progressBar" src="/resources/img/progressbar.png" alt="50%" style="right: 45%" />
                            <span class="percent">50%</span>
                        </div>
                        <p>za prolaz je potrebno još 4</p>
                    </div>

                    <div class="time-left">
                        Do isteka ponude ostalo je:
                        <div class="timer" data-time="17:28 12/27/2012">
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

                </div> <!-- END .offer-info -->

            </article> <!-- END .slide -->

        </ItemTemplate>

        <FooterTemplate>
            </div> <!-- END .inner-slider -->
        </FooterTemplate>

        </asp:Repeater>
        
    </div> <!-- END .slider -->