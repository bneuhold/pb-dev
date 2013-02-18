<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= ConfigurationManager.AppSettings["GoogleMapsApiKey"]%>&sensor=false" type="text/javascript"></script>

    <script type="text/javascript">
        var map;
        var geocoder;

        function initialize1() {

            geocoder = new google.maps.Geocoder();
            var latlng = new google.maps.LatLng(-34.397, 150.644);

            var myOptions = {
                center: latlng,
                zoom: 10,
                mapTypeId: google.maps.MapTypeId.HYBRID
            };
            map = new google.maps.Map(document.getElementById("map"), myOptions);  
        }

        function codeAddress() {
            var address = document.getElementById("address").value;
            var marker;

            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    map.setCenter(results[0].geometry.location);
                    marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location,
                        draggable: true,
                        title: 'kurcic'
                    });
                    map.openInfoWindow("Možete preciznije odrediti gdje se nalazite tako da pomaknete marker!");
                    map.setZoom(16);

                    google.maps.event.addListener(marker, "dragstart", function () {
                        //map.closeInfoWindow();
                        alert("start");
                    });

                    google.maps.event.addListener(marker, "dragend", function () {
                        alert("start");
//                        var hdnLat = document.getElementById("<%=hdnLat.ClientID %>");
//                        var hdnLng = document.getElementById("<%=hdnLng.ClientID %>");

//                        hdnLat.value = this.getLatLng().lat();
//                        hdnLng.value = this.getLatLng().lng();

//                        marker.openInfoWindow("New position has been set");
                    });

                } else {
                    alert("Geocode was not successful for the following reason: " + status);
                }
            });


        }
    </script>

    <script type="text/javascript">

        var longitude = null;
        var latitude = null
        var address = 'Brela, Hrvatska';
        var city = null;
        var country = null;
        function initializeGMap() {
            alert("load");

            var latlng = new google.maps.LatLng(-34.397, 150.644);
            var myOptions = {
                center: latlng,
                zoom: 10,
                mapTypeId: google.maps.MapTypeId.HYBRID
            };
            var map = new google.maps.Map(document.getElementById("map"), myOptions);


            var geocoder = new google.maps.Geocoder();


            if (address != null && address != '') {
                address = address;
            }
            else if (city != null && city != '') {
                address = city;
            }
            else if (country != null && country != '') {
                address = country;
            }
            else {
                //NEMA MAPE
            }

            var newlonglat = null;
            //ako nije zadana izračunaj je iz adrese
            if (latitude == null || longitude == null) {
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        map.setCenter(results[0].geometry.location);
                        newlonglat = results[0].geometry.location;

                    } else {
                        newlonglat = null;
                        //alert("Geocode was not successful for the following reason: " + status);
                    }
                });
            }
            else {
                newlonglat = new google.maps.LatLng(latitude, longitude);
            }

            var marker = new google.maps.Marker({
                map: map,
                position: newlonglat,
                clickable: false,
                title: address
            });
        }
    </script>

</head>

<body onload="initializeGMap()" >
    <div id="map" style="width: 500px; height: 500px"></div>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnLat" runat="server" />
        <asp:HiddenField ID="hdnLng" runat="server" />
        
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
            onclick="btnSubmit_Click" />

        <input id="address" type="text" name="adress" value="Stjepana Gradića 1, Zagreb, Hrvatska" />
        <input type="button" value="Encode" onclick="codeAddress()" />
    </form>
</body>

</html>
