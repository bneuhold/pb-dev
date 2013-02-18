<%@ Control Language="C#" ClassName="GoogleMaps" EnableViewState="false" %>


<script type="text/javascript">

        var parameters = new Array();
        
        //primjer kak zgledaju objekti u parameters arrayu
//        var ControlParameters = {            
//            longitude : 150.644,
//            latitude : -34.397,
//            address : 'pripizdina 1A',
//            city : null,
//            country : null,              
//            gmap : null,
//            infowindowContent : 'neki tekst ili html koji se otvara na klik markera ako je zadana longitude i latitude',
//            overviewShow : false,
//            map_canvas_id: "map_canvas"
//        }

        function startGMap(cp) {
            var map = null;
            $.each(parameters, function (index, value) {
                if (value.map_canvas_id == cp.map_canvas_id)
                    map = value.gmap;
            });

            if (map == null) {
                initializeGMap(cp);
            }
            else {
                refreshGMap(cp);
            }


        }

        //ovaj refresh niš ne pomaže
        function refreshGMap(cp) {
            
            if(cp.gmap != null)
                cp.gmap.setCenter(cp.gmap.getCenter());
        
        }

        function initializeGMap(cp) {
            var THEADRESS = '';
            
            if (cp.address != null && cp.address != '') {
                THEADRESS = cp.address;
            }
            if (cp.city != null && cp.city != '') {
                if (THEADRESS != '') THEADRESS += ', ';
                THEADRESS += cp.city;
            }
            if (cp.country != null && cp.country != '') {
                if (THEADRESS != '') THEADRESS += ', ';
                THEADRESS += cp.country;
            }            

            //Ako nema nikakvih podataka nemoj ni učitavat mapu, sve preskoči i begaj van
            if (THEADRESS == '' && (cp.latitude == null || cp.latitude == '' || cp.longitude == null || cp.longitude == '')) {
                return;
            }

            var latlng = new google.maps.LatLng(-34.397, 150.644);
            var myOptions = {
                zoom: 16,
                mapTypeId: google.maps.MapTypeId.HYBRID,
                overviewMapControl: true,
                overviewMapControlOptions: { opened: cp.overviewShow }
            };

            var map = new google.maps.Map(document.getElementById(cp.map_canvas_id), myOptions);
            cp.gmap = map;

            var geocoder = new google.maps.Geocoder();

            var marker = null;
            var newlonglat = null;
            //ako nije zadana izračunaj je iz adrese
            if(cp.latitude == null || cp.latitude == '' || cp.longitude == null || cp.longitude == ''){
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': THEADRESS }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        map.setCenter(results[0].geometry.location);
                        newlonglat = results[0].geometry.location;
                        marker = new google.maps.Marker({
                            map: map,
                            position: newlonglat,
                            clickable: true,
                            title: THEADRESS
                        });

                        var infowindow = new google.maps.InfoWindow({
                            content: THEADRESS
                        });

                        google.maps.event.addListener(marker, 'click', function () {
                            infowindow.open(map, marker);
                        });

                    } else {
                        newlonglat = null;
                        var canvasDiv = map.getDiv();
                        var parent = canvasDiv.parentNode;
                        canvasDiv.style.display = 'none';
                        parent.innerHTML = '<p>' + "Error" + '</p>';
                    }
                });
            }
            else{
                newlonglat = new google.maps.LatLng(cp.latitude, cp.longitude);
                map.setCenter = newlonglat;
                marker = new google.maps.Marker({
                    map: map,
                    position: newlonglat,
                    clickable: true,
                    title: THEADRESS
                });

                var infowindow = new google.maps.InfoWindow({
                    content: cp.infowindowContent
                });

                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.open(map, marker);
                });
            }






            parameters.push(cp);
        }
</script>

