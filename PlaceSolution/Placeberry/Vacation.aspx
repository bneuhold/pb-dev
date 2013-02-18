<%@ Page Language="C#" MasterPageFile="~/MasterResults.master" AutoEventWireup="true"
    CodeFile="Vacation.aspx.cs" Inherits="Vacation" culture="auto" uiculture="auto" %>
<%@ Register src="Controls/SeoDirectory.ascx" tagname="SeoDirectory" tagprefix="uc1" %>
<%@ Register src="Controls/ShareButtons.ascx" tagname="ShareButtons" tagprefix="uc4" %>
<%@ Register src="Controls/GoogleForecast.ascx" tagname="GoogleForecast" tagprefix="uc2" %>
<%@ Register src="Controls/SortResults.ascx" tagname="SortResults" tagprefix="uc5" %>

<%@ Register src="Controls/GoogleMaps.ascx" tagname="GoogleMaps" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= ConfigurationManager.AppSettings["GoogleMapsApiKey"]%>&sensor=false" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" class="clearfix">
        <div class="search_block clearfix">
            <a href="Default.aspx">
                <img class="logo" runat="server" id="imgLogo" src="~/resources/images/logo_placeberry_small.png" alt="<%$ Resources:placeberry, General_Slogan %>" /></a>
            <asp:Panel runat="server" ID="pnlSearch" CssClass="search_index search"
                DefaultButton="btnSearch">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:placeberry, Vacation_Search %>" OnClick="btnSearch_Click" />
            </asp:Panel>
        </div>
        <div style="display: none;">  
            <uc2:GoogleForecast ID="GoogleForecast1" runat="server" />
        </div>
            <div class="results">
                <div class="results_container">
                    <p class="results_info">
                        <asp:Literal runat="server" ID="litYourQuery" Text="<%$ Resources:placeberry, Vacation_YourQuery %>"></asp:Literal>
                        <u><asp:Label runat="server" ID="lblQuery"></asp:Label></u>
                        <asp:Literal runat="server" ID="litReturned" Text="<%$ Resources:placeberry, Vacation_QueryReturned %>"></asp:Literal>
                        <span><asp:Label runat="server" ID="lblResultsCount"></asp:Label></span>
                        <asp:Literal runat="server" ID="litResults" Text="<%$ Resources:placeberry, Vacation_Results %>"></asp:Literal>
                        <img id="beta_att" class="beta_att" src="resources/images/beta_attention.png" title='<asp:Literal runat="server" ID="litBeta" Text="<%$ Resources:placeberry, Vacation_BetaWarning %>"></asp:Literal>' alt="" />
                        <br />

                        <asp:Literal ID="ltlQueryMessage" runat="server" />


                        <br />
                    </p>
                    <fieldset class="seo_directory clearfix">
                    <legend>Search related regions</legend>  
                    <uc1:SeoDirectory ID="SeoDirectory1" runat="server"  />
                    </fieldset>   
                    <p class="results_info">
                        <uc5:SortResults ID="SortResults1" runat="server" />
                    </p>
                        <asp:ListView ID="dlResults" runat="server" ItemContainerID="DataSection"
                            
                            OnDataBound="dlResults_DataBound" 
                            EnableModelValidation="True" DataSourceID="odsResults">
                            <LayoutTemplate>
                                <ul class="listings">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="<%# Container.DataItemIndex < 3 ? "sponsored clearfix" : "clearfix" %>">
                                        <h1>
                                            <a href="<%# Common.FormatAdvertUrl(Eval("Id"), Eval("PlaceberryAdvert"), Eval("AgencyUrlTag"), Eval("AccommodationUrlTag")) %>" target="_blank"><%#Eval("Title")%></a>
                                            <a id="saveAd-<%# Eval("Id") %>" class="save_this" href="javascript:SaveRemoveClick('<%# Eval("Id") %>')" title="<%#Eval("Title")%>" ></a>
                                            <br />
                                            <%# Common.FormatLocationDescription(Eval("Country"), Eval("Region"), Eval("Island"), Eval("City"))%>
                                        </h1>
                                        <a href="<%# Common.FormatAdvertUrl(Eval("Id"), Eval("PlaceberryAdvert"), Eval("AgencyUrlTag"), Eval("AccommodationUrlTag")) %>" target="_blank"><img src="/thumb.aspx?src=<%# Eval("PictureUrl") %>&mw=133&mh=100&crop=1" alt="<%# Eval("Title") %>" class="image" /></a>
                                            <p>
                                                <%# Eval("Description") %>
                                                <%--<%# (bool)Eval("NeedsTranslation") ? Common.GetGenericDescription(Container.DataItem, Common.GetLanguageId()) : Eval("Description") %>--%>                                            </p>
                                        <div class="left_ad">
                                        <uc4:ShareButtons ID="ShareButtons1" runat="server" Custom="True" Url='<%# Request.Url.GetLeftPart(UriPartial.Authority) + "/Advert.aspx?id=" + Eval("Id") %>' Title='<%# Eval("Title") %>' Description='<%# Eval("Description") %>' Image='<%# Eval("PictureUrl") %>'  />
                                        <a href="http://www.allianz.hr/shop" target="_blank"><img src="~/resources/images/img_allianz.png" runat="server" id="img1" alt="Allianz - putno osiguranje" width="53" height="15" />&nbsp;
                                        <asp:Literal runat="server" ID="Literal1" Text="<%$ Resources:placeberry, Vacation_HelperLink %>"></asp:Literal></a>
                                        <%--<div class="agency">
                                            <a href="<%# Common.FormatSourceUrl(Eval("PlaceberryAdvert"), Eval("AgencyUrlTag"), Eval("SourceUrl")) %>"><%# (bool)Eval("PlaceberryAdvert") ? Eval("AgencyName") : Eval("SourceTitle")%></a>
                                        </div>
                                        --%>
                                        </div>
                                        <div class="listing_price">
                                            <div class='<%# Eval("PriceOld") != null && Eval("Price") != null ? (Convert.ToDecimal(Eval("PriceOld")) > Convert.ToDecimal(Eval("Price")) ? "result_item_price_lower" : "result_item_price_upper") : "" %>'
                                                 style="<%# Eval("PriceOld") != null && Eval("Price") != null && Eval("PriceOld").ToString() == Eval("Price").ToString() || Eval("PriceOld") != null && Eval("PriceOld").ToString().Replace(",00", "").Replace(".00", "") == "0" ? "display:none": "" %>">

                                                <%# (Eval("PriceOld") != null && Eval("Price") != null ? (Convert.ToDecimal(Eval("PriceOld")) > Convert.ToDecimal(Eval("Price")) ? "-" : "+")  + String.Format("{0:0.}", Math.Abs(Convert.ToDecimal(Eval("PriceOld")) - Convert.ToDecimal(Eval("Price")))) : "") + "" + 
                                                (Eval("CurrencySymbol") != null ? Eval("CurrencySymbol").ToString() : "")%>
                                                <div class="<%# Eval("PriceOld") != null && Eval("Price") != null ? (Convert.ToDecimal(Eval("PriceOld")) > Convert.ToDecimal(Eval("Price")) ? "result_item_price_lower_tip" : "result_item_price_upper_tip") : "" %>"></div>  
                                            </div>
                                            <%# Eval("Price") != null && Eval("Price").ToString().Replace(",00", "").Replace(".00", "") == "0" ? Resources.placeberry.Vacation_UponRequest : (Eval("Price") != null ? Eval("Price").ToString().Replace(",00", "").Replace(".00", "") : "") + " " + (Eval("CurrencySymbol") != null ? Eval("CurrencySymbol").ToString() : "")%>
                                            <span><%# (Eval("Price") == null || Eval("Price").ToString().Replace(",00", "").Replace(".00", "") == "0") ? string.Empty : Resources.placeberry.Vacation_PerDay %></span>
                                        </div>
                                        <div class="info_zone">
                                            <span id="info-<%# Eval("Id") %>" class="info_btn"><!-- --></span>
                                            <span class="persons"><%# Common.FormatCapacity(Eval("CapacityMin"), Eval("CapacityMax")) %></span>
                                            <span class="type"><%# Eval("AccommType")%></span>
                                        </div>     
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    <div class="pager">
                    <asp:DataPager ID="dpResults" runat="server" PagedControlID="dlResults" 
                        QueryStringField="p">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False"
                                ShowPreviousPageButton="False" FirstPageText="<%$ Resources:placeberry, Vacation_PageFirst %>" 
                                ButtonCssClass="dataPager" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="False"
                                ShowPreviousPageButton="False" LastPageText="<%$ Resources:placeberry, Vacation_PageLast %>" 
                                ButtonCssClass="dataPager" />
                        </Fields>
                    </asp:DataPager>
                    </div>

                    <asp:ObjectDataSource ID="odsResults" TypeName="Placeberry.DAL.GetResults" SelectCountMethod="GetDataCount"
                        SelectMethod="Execute" EnablePaging="True" runat="server" 
                        onselected="odsResults_Selected">
                    </asp:ObjectDataSource>

                </div>
            </div>
            <div class="results_ads">
                <p>
                    <asp:Literal runat="server" ID="litAds" Text="<%$ Resources:placeberry, General_Ads %>"></asp:Literal></p>
                <div class="ads">
                    <a href="http://www.ultra-sailing.hr" target="_blank">
                        <img src="/uploads/banners/ultra-banner-6.jpg" alt="ultra sailing charter" width="250" height="250" /></a>
                    <a href="http://www.ncp.hr" target="_blank">
                        <img src="/uploads/banners/ncp-banner-9.jpg" alt="ncp" width="250" height="250" /></a>
                    <a href="http://www.nebo-travel.hr" target="_blank">
                        <img src="/uploads/banners/nebo-banner.jpg" alt="" width="250" height="130" /></a>
                    <%-- 
                    <a href="#">
                        <img src="/resources/images/square.gif" alt="" width="250" height="250" /></a>
                    <a href="#">
                        <img src="/resources/images/square.gif" alt="" width="250" height="250" /></a>
                    --%>
                </div>
            </div>
    </div>
    <script type="text/javascript">
        $(".info_btn").each(function () {
            var info_btn = $(this);
            var advertId = info_btn.attr("id").split("-")[1];

            info_btn.qtip({
                content: { url: '/InfoBox.aspx?infoid=' + advertId },
                position: {
                    corner: {
                        target: 'rightMiddle',
                        tooltip: 'leftMiddle'
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
                    tip: 'leftMiddle'
                },
                hide: { when: 'mouseout', fixed: true }

            });

        });

        $('#beta_att').tipsy({ gravity: 'w' });
    </script>
    <uc3:GoogleMaps ID="GoogleMaps1" runat="server" />

</asp:Content>