<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WSpayPreForm.aspx.cs" Inherits="WSpay_WSpayPreForm" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">        window.jQuery || document.write('<script src="scripts/vendor/jquery.min.js"><\/script>')</script>

    <script type="text/javascript">

        $(document).ready(function () {

            var form = $("#formPay");
            form.submit();
        });
        
    </script>
</head>
<body>

    <form name="pay" id="formPay" action="https://form.wspay.biz/test/Authorization.aspx" method="post">

    <input type="hidden" name="ShopID" value="<%= ipgShopID %>" />
    <input type="hidden" name="ShoppingCartID" value="<%= shopingCartID %>" />
    <input type="hidden" name="TotalAmount" value="<%= ipgTotalAmount %>" />
    <input type="hidden" name="Signature" value="<%= signature %>" />
    <input type="hidden" name="ReturnURL" value="<%= "http://" + Request.Url.Host + (Request.Url.Port != 80 ? ":" + Request.Url.Port : "") + "/wspay/success.aspx"%>" />
    <input type="hidden" name="CancelURL" value="<%= "http://" + Request.Url.Host + (Request.Url.Port != 80 ? ":" + Request.Url.Port : "") + "/wspay/cancel.aspx"%>" />
    <input type="hidden" name="ReturnErrorURL" value="<%= "http://" + Request.Url.Host + (Request.Url.Port != 80 ? ":" + Request.Url.Port : "") + "/wspay/error.aspx"%>" />
    <input type="hidden" name="Lang" id="Lang" value="hr" />
    <input type="hidden" name="PaymentPlan" value="" />
    <input type="hidden" name="CustomerFirstName" value="<%= firstName %>" />
    <input type="hidden" name="CustomerLastName" value="<%= lastName %>" />
    <input type="hidden" name="CustomerAddress" value="<%= address %>" />
    <input type="hidden" name="CustomerCity" value="<%= city %>" />
    <input type="hidden" name="CustomerZIP" value="<%= zip %>" />
    <input type="hidden" name="CustomerCountry" value="<%= country %>" />
    <input type="hidden" name="CustomerPhone" value="<%= phone %>" />
    <input type="hidden" name="CustomerEmail" value="<%= email %>" />
    
    </form>
</body>
</html>
