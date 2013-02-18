<%@ Control Language="C#" ClassName="FacebookJsSDK" %>

<div id="fb-root"></div>
<script>

    // OVA KONTROLA MORA BITI PRVA U BODY TAGU!!!

  // Additional JS functions here
  window.fbAsyncInit = function() {
    FB.init({
      appId:    '382211285191328', // App ID
      channelUrl: '//www.putovalica.net/fcb/channel.html', // Channel File
      status     : true, // check login status
      cookie     : true, // enable cookies to allow the server to access the session
      xfbml      : true  // parse XFBML
    });

    // Additional init code here

  };

  // Load the SDK Asynchronously
  (function(d){
     var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
     if (d.getElementById(id)) {return;}
     js = d.createElement('script'); js.id = id; js.async = true;
     js.src = "//connect.facebook.net/en_US/all.js";
     ref.parentNode.insertBefore(js, ref);
   }(document));
   

    function login() {

        FB.login(function (response) {

            if (response.authResponse) {

                if (window.location.href.indexOf("?") !== -1) {
                    window.location = window.location.href + '&fbtoken=' + response.authResponse.accessToken;
                }
                else {
                    window.location = window.location.href + '?fbtoken=' + response.authResponse.accessToken;
                }
                //window.location = '/Login.aspx?fbtoken='+ response.authResponse.accessToken;
            } else {
                // cancelled
            }
        }, { scope: 'email,user_about_me' });
    }
 
</script>