<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgencyListing.aspx.cs" Inherits="AgencyListing" %>

<%@ Register src="Controls/ContactAgency.ascx" tagname="ContactAgency" tagprefix="uc1" %>
<%@ Register src="Controls/GoogleMaps.ascx" tagname="GoogleMaps" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/resources/scripts/pikachoose_4.4.4/lib/jquery.pikachoose.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.easing-1.3.pack.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="/resources/scripts/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= ConfigurationManager.AppSettings["GoogleMapsApiKey"]%>&sensor=false" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready( function () {
			$("#pikame").PikaChoose({
				carousel: true,
				showCaption: true,
				transition: [0],
				autoPlay: false,
                thumbOpacity: 0.5,
                animationSpeed: 200
            });

            $("#contact_button").fancybox({
                'opacity': true,
                'overlayShow': true,
                'overlayOpacity': 0.7,
                //'overlayColor' : '#666'
                'transitionIn': 'elastic',
                'transitionOut': 'fade',
                'href': '#contact_form'
            });

            $("#btn_show_map").fancybox({
                'href': '#map_canvas',
                'autoDimensions': false,
                'width': 800, 
                'height': 600,
                'hideOnContentClick': false,
                'onStart' : function () {
                    startGMap({
                        longitude: '<%= Longitude %>',
                        latitude: '<%= Latitude %>',
                        address: '<%= Address %>',
                        city: '<%= City %>',
                        country: '<%= Country %>',
                        infowindowContent: '<%= AccommodationName %>',
                        overviewShow: true,
                        gmap: null,
                        map_canvas_id: 'map_canvas'
                    });
                }
            });
        }); 


    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrap">
        <div class="left_bar">
            <div class="logo_image">
                <a id="aAgencyLogo" runat="server" href="#"><img id="imgAgencyLogo" runat="server" src="resources/images/tmp_agency_logo.jpg" alt="" /></a>
            </div>
            <div class="contact_box">
                <h2 class="contact_label"><a id="aAgencyName" runat="server" href="#">Agency Name</a></h2>
                <span class="contact_data"></span>

                <span class="contact_label">Adress</span> 
                <span class="contact_data">
                    <asp:Literal ID="ltlAgencyAdress" Text="Superturska 58, Zadar, Croatia" runat="server" />
                </span>

                <span class="contact_label">Contacts</span>
                <span class="contact_data">
                    <asp:Literal ID="ltlAgencyPhones" runat="server">
                        M: +385 91 555 6666
                        <br />
                        P: +385 91 555 6666                        
                    </asp:Literal>
                    <br />
                    <asp:Literal ID="ltlAgencyEmail" runat="server">
                        E: ture@super.com            
                    </asp:Literal>
                </span>

                <span class="contact_label">Website</span> 
                <span class="contact_data">
                    <a id="aAgencyWebsite" runat="server" href="#">www.musingbuddha.com</a>
                    <br />
                    <a href="#" title="">Follow us on Facebook</a>
                </span>

                <input id="contact_button" type="button" class="contact_button" value="Contact us" />
                <div class="contact_box_top">
                    <!-- top -->
                </div>
                <div class="contact_box_bottom">
                    <!-- bottom -->
                </div>
            </div>
            <div style="display:none;">
                <div id="contact_form">
                    <uc1:ContactAgency ID="ContactAgency1" runat="server" />
                </div>
            </div>
        </div>
        <div class="content content_listing">
            <h1>
                <asp:Literal ID="ltlListingTitle" runat="server">The Centre of Zadar - sunny apt</asp:Literal>
            </h1>
            <h4>
                <asp:Literal ID="ltlListingLocation" runat="server">Bol, Brač, Srednja Dalmacija, Dalmacija, Hrvatska</asp:Literal>
            </h4>
            <div class="listing_price">
            <!--<div class="result_item_price_lower">-135kn<div class="result_item_price_lower_tip">--><!-- ARROW TIP --><!--</div></div>-->             
                <asp:Literal ID="ltlPrice" runat="server"></asp:Literal>
                <span><asp:Literal ID="ltlPriceDescription" runat="server"></asp:Literal></span>
            </div>
            <input id="btn_show_map" type="button" class="btn_map" />
            <div style="display:none;">
                <div id="map_canvas" style="width:100%; height:100%;">
                </div>
            </div>
            <div class="listing_gallery">

                <asp:Repeater ID="repListingGallery" runat="server">
                    <HeaderTemplate>
                        <ul id="pikame" class="jcarousel-skin-pika">
                    </HeaderTemplate>
                    <ItemTemplate>
                            <li>
                                <img src="/thumb.aspx?src=<%# Eval("Src") %>&mw=100&mh=100&crop=0" ref="/thumb.aspx?src=<%# Eval("Src") %>&mw=640&mh=400&crop=1" alt="<%# Eval("Alt") %>" />
                                <span>
                                    <%# !string.IsNullOrEmpty(Eval("Title").ToString()) ? String.Format("<p>{0}<p>", Eval("Title")) : string.Empty %>
                                    <%# !string.IsNullOrEmpty(Eval("Description").ToString()) ? Eval("Description").ToString().ToLower() != (Eval("Title").ToString() ?? string.Empty).ToLower() ? String.Format("<p>{0}<p>", Eval("Description")) : string.Empty : string.Empty%>
                                    <%-- Ovaj description sam namjerno zakompliciro da se ne ispisuje ako je prazan ili isti kao title (da nebude ista stvar dva puta) --%>
                                </span>
                            </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <ul class="properties">
                <li class="even">
                    <span class="label">Type:</span>
                    <span class="data"><asp:Literal ID="ltlListingType" runat="server">Apartment</asp:Literal></span>
                </li>
                <li>
                    <span class="label">Capacity:</span>
                    <span class="data"><asp:Literal ID="ltlListingCapacity" runat="server">5-8</asp:Literal></span>
                </li>
                <li class="even">
                    <span class="label">Price:</span>
                    <span class="data">
                        <asp:Repeater ID="repListingPrices" runat="server">
                            <HeaderTemplate>
                                <ul class="prices">
                            </HeaderTemplate>
                            <ItemTemplate>
                                    <li>
                                        <span class="label"><%# FormatPriceDate(Eval("DateStart"), Eval("DateEnd")) %></span>
                                        <span class="data"><%# Eval("Value") %>&nbsp;<%# Eval("Currency.Symbol") %></span></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </span>
                </li>
            </ul>
            <p class="listing_description">
                <asp:Literal ID="ltlListingDescription" runat="server">
                    Post-Graduate Student at Cambridge University with a passion for Arabian horses,
                    Persian cats and arts. I love love Istanbul and that is why I am centering my research
                    around activities in the city, will have an installation about breaking silences
                    in September in one of the art galleries in town...
                </asp:Literal> 
            </p>            
        </div>
    </div>
      
   
    <uc2:GoogleMaps ID="GoogleMaps1" runat="server" />

</asp:Content>
