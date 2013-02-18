<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InfoBox.aspx.cs" Inherits="InfoBox" EnableViewState="false" %>
<%@ Register src="Controls/GoogleForecast.ascx" tagname="GoogleForecast" tagprefix="uc1" %>

<%--<script type="text/javascript">
//    //primjer kak zgledaju objekti u parameters arrayu
////            var ControlParameters = {            
////                longitude : '<%= Latitude %>',
////                latitude : '<%= Longitude %>',
////                address : '<%= Address %>',
////                city : '<%= City %>',
////                country : '<%= Country %>',
////                gmap : null,
////                map_canvas_id: 'map_canvas_<%= Request.QueryString["infoid"] %>'
////            };
</script>--%>

    <script type="text/javascript">
        $(document).ready(function () {
             $('#infocontainer<%= Request.QueryString["infoid"] %>').minitabs();
        });

        function ShowMap<%= Request.QueryString["infoid"] %>() {

        //ova linija je fix da se mapa ne razmrda zbog display: none zbog minitabs plugina
        $("#map<%= Request.QueryString["infoid"] %>").css('display','block');        
            startGMap({
                longitude: '<%= Latitude %>',
                latitude: '<%= Longitude %>',
                address: '<%= Address %>',
                city: '<%= City %>',
                country: '<%= Country %>',
                infowindowContent : '<%= AdvertName %>',
                overviewShow : false,
                gmap: null,
                map_canvas_id: 'map_canvas_<%= Request.QueryString["infoid"] %>'
            });
        }
    </script>

    <div id="infocontainer<%= Request.QueryString["infoid"] %>">
        <ul class="mini_menu">
            <li id="liWiki" runat="server"><a href="#wiki<%= Request.QueryString["infoid"] %>">wiki</a></li>
            <li id="liInfo" runat="server"><a href="#info<%= Request.QueryString["infoid"] %>">info</a></li>
            <li id="liWeather" runat="server"><a href="#weather<%= Request.QueryString["infoid"] %>">weather</a></li>
            <li id="liMap" runat="server">
            <a href="#map<%= Request.QueryString["infoid"] %>" 
                onclick="ShowMap<%= Request.QueryString["infoid"] %>()">view map</a></li>
        </ul>

        <div id="wiki<%= Request.QueryString["infoid"] %>" class="infotab">
            <br />
            <p>
                <asp:Literal ID="ltlWikiDesc" Text="No wiki" runat="server" />
            </p>
            <br />
            <a id="aWikiLink" runat="server" href="" target="_blank">Link</a>
        </div>

        <div id="info<%= Request.QueryString["infoid"] %>" class="infotab">
            <p>
                <asp:Literal ID="ltlInfo" Text="No info" runat="server" />
            </p>
            <p>
                <asp:Literal ID="ltlCarriers" Text="" runat="server" />
            </p>
        </div>

        <div id="weather<%= Request.QueryString["infoid"] %>" class="infotab">
            <uc1:GoogleForecast ID="GoogleForecast1" runat="server" />
        </div>

        <div id="map<%= Request.QueryString["infoid"] %>" class="infotab" style="width:253px; height:220px;" >        
            <div id="map_canvas_<%= Request.QueryString["infoid"] %>" style="width:100%; height:100%;"></div>    
            
        </div>
    </div>
