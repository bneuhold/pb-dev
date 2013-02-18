<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Agency.aspx.cs" Inherits="Agency" %>

<%@ Register src="Controls/ContactAgency.ascx" tagname="ContactAgency" tagprefix="uc1" %>
<%@ Register src="Controls/Adverts.ascx" tagname="Adverts" tagprefix="adv" %>


<%@ Register src="Controls/GoogleMaps.ascx" tagname="GoogleMaps" tagprefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/resources/scripts/fancybox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.easing-1.3.pack.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="/resources/scripts/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= ConfigurationManager.AppSettings["GoogleMapsApiKey"]%>&sensor=false" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $("#contact_button").fancybox({
                'opacity': true,
                'overlayShow': true,
                'overlayOpacity': 0.7,
                //'overlayColor' : '#666'
                'transitionIn': 'elastic',
                'transitionOut': 'fade',
                'href': '#contact_form'
            });
        }); 


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- Script manager je tu zbog UpdatePanela u ContactAgency kontroli --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="wrap clearfix">
        <div class="left_bar">
            <div class="logo_image">
                <a id="aAgencyLogo" runat="server" href="#"><img id="imgLogo" runat="server" src="resources/images/tmp_agency_logo.jpg" alt="" /></a>
            </div>
            <div class="contact_box">
                <h2 class="contact_label"><a id="aAgencyName" runat="server" href="#">Agency Name</a></h2>
                <span class="contact_data"></span>

                <span class="contact_label">Adress</span>
                <span class="contact_data">
                    <asp:Literal ID="ltlAdress" Text="Superturska 58, Zadar, Croatia" runat="server" />
                </span>

                <span class="contact_label">Contacts</span>
                <span class="contact_data">
                    <asp:Literal ID="ltlPhones" runat="server">
                        M: +385 91 555 6666
                        <br />
                        P: +385 91 555 6666                        
                    </asp:Literal>
                    <br />
                    <asp:Literal ID="ltlEmail" runat="server">
                        E: ture@super.com            
                    </asp:Literal>
                </span>

                <span class="contact_label">Website</span> 
                <span class="contact_data">
                    <a id="aWebsite" runat="server" href="#">www.musingbuddha.com</a>
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
        <div class="content">
            <h1>
                <asp:Literal ID="ltlAgencyTitle" runat="server">Mussing Budha</asp:Literal>
            </h1>
            <p class="description">
                <asp:Literal ID="ltlDescription" runat="server">
                    Post-Graduate Student at Cambridge University with a passion for Arabian horses,
                    Persian cats and arts. I love love Istanbul and that is why I am centering my research
                    around activities in the city, will have an installation about breaking silences
                    in September in one of the art galleries in town...
                </asp:Literal>
            </p>          

            <div></div>
            <adv:Adverts ID="AgencyAdverts" runat="server" />            

        </div>

    </div>
    <script type="text/javascript">
        $(".info_btn").each(function () {
            var info_btn = $(this);
            var advertId = info_btn.attr("id").split("-")[1];

            info_btn.qtip({
                content: { url: 'InfoBox.aspx?infoid=' + advertId },
                position: {
                    corner: {
                        target: 'topMiddle',
                        tooltip: 'bottomMiddle'
                    }
                },
                style: {
                    width: 280,
                    height: 250,
                    padding: 1,
                    background: '#ffffff',
                    color: 'black',
                    border: {
                        width: 0,
                        radius: 3,
                        color: '#006f9a'
                    },
                    tip: 'bottomMiddle'
                },
                hide: { when: 'mouseout', fixed: true }

            });

        });
    </script>
    <uc3:GoogleMaps ID="GoogleMaps1" runat="server" />
</asp:Content>
