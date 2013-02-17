<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingNovakComplete.aspx.cs" Inherits="BookingIframes_BookingNovakComplete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="resources/css/novak.css"/>
</head>
<body>
    <!-- Google Code for Booking Conversion Page -->
    <script type="text/javascript">
    /* <![CDATA[ */
    var google_conversion_id = 973082455;
    var google_conversion_language = "en";
    var google_conversion_format = "3";
    var google_conversion_color = "ffffff";
    var google_conversion_label = "nSFVCMn5ogQQ156A0AM";
    var google_conversion_value = 0;
    /* ]]> */
    </script>
    <script type="text/javascript" src="http://www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
    <div style="display:inline;">
    <img height="1" width="1" style="border-style:none;" alt="" src="http://www.googleadservices.com/pagead/conversion/973082455/?value=0&amp;label=nSFVCMn5ogQQ156A0AM&amp;guid=ON&amp;script=0"/>
    </div>
    </noscript>

    <form id="form1" runat="server">

    <div class="main_content" style="text-align:center;">
        <div class="booking_success">
            <%= Resources.booking.Booking_success %>
        </div>
        <div class="booking_success_msg">
            <%= Resources.booking.Booking_success_msg %>
        </div>    
        <div class="err_msg">
            <asp:Label runat="server" ID="lblErrorMsg"></asp:Label>
        </div>

    </div>

    </form>
</body>
</html>
