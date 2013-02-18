<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Advert.aspx.cs" Inherits="Advert" %>

<%@ Register Src="/Controls/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Placeberry.com - pametna tražilica turističke ponude</title>
    <link type="text/css" media="all" rel="Stylesheet" href="/resources/css/style.css" />
    <link rel="shortcut icon" href="/resources/images/favicon2.ico" type="image/x-icon" />
    <script src="resources/scripts/jquery-1.6.1.min.js" type="text/javascript"></script>
</head>

<style type="text/css">
   html, body, div, iframe { margin:0; padding:0; height:100%; }
   iframe { position:fixed; display:block; width:100%; border:none; }
</style>

<body>
    <form id="form1" runat="server">
    <div id="global_wrapper">
        <uc1:Header ID="Header1" runat="server" />

        <iframe id="advertframe" runat="server" >
        </iframe>
    </div>
    </form>
    <!-- #include file="/googleAnalyticsSD.inc" -->
</body>
</html>
