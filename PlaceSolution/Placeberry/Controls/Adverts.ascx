<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Adverts.ascx.cs" Inherits="Controls_Adverts" EnableViewState="false" %>
<%@ Register src="/Controls/ShareButtons.ascx" tagname="ShareButtons" tagprefix="share" %>

<asp:Repeater ID="repAdverts" runat="server">
    <HeaderTemplate>
        <ul class="listings">
    </HeaderTemplate>
    <ItemTemplate>
            <li class="clearfix">
                <h1>
                    <a href="<%# Common.FormatAdvertUrl(Eval("Id"), Eval("PlaceberryAdvert"), Eval("AgencyUrlTag"), Eval("AccommodationUrlTag")) %>" target="_blank"><%#Eval("Title")%></a> <a id="saveAd-<%# Eval("Id") %>" class="save_this" href="javascript:SaveRemoveClick('<%# Eval("Id") %>')" title="<%#Eval("Title")%>" ></a>
                    <br />
                    <%# Common.FormatLocationDescription(Eval("Country"), Eval("Region"), Eval("Island"), Eval("City"))%>
                </h1>
                <a href="<%# Common.FormatAdvertUrl(Eval("Id"), Eval("PlaceberryAdvert"), Eval("AgencyUrlTag"), Eval("AccommodationUrlTag")) %>" target="_blank"><img src="/thumb.aspx?src=<%# Eval("PictureUrl") %>&mw=133&mh=100&crop=1" alt="<%# Eval("Title") %>" class="image" /></a>
                    <p>
                        <%# Eval("Description") %>
                        <%--<%# (bool)Eval("NeedsTranslation") ? Common.GetGenericDescription(Container.DataItem, Common.GetLanguageId()) : Eval("Description") %>--%>
                    </p>
                <div class="left_ad">
                <share:ShareButtons ID="ShareButtons1" runat="server" Custom="True" Url='<%# Request.Url.GetLeftPart(UriPartial.Authority) + "/Advert.aspx?id=" + Eval("Id") %>' Title='<%# Eval("Title") %>' Description='<%# Eval("Description") %>' Image='<%# Eval("PictureUrl") %>'  />
                <a href="http://www.allianz.hr/shop" target="_blank"><img src="~/resources/images/img_allianz.png" runat="server" id="img1" alt="Allianz - putno osiguranje" width="53" height="15" />&nbsp;
                <asp:Literal runat="server" ID="Literal1" Text="<%$ Resources:placeberry, Vacation_HelperLink %>"></asp:Literal></a>
                <div class="agency">
                    <a href="<%# Common.FormatSourceUrl(Eval("PlaceberryAdvert"), Eval("AgencyUrlTag"), Eval("SourceUrl")) %>"><%# (bool)Eval("PlaceberryAdvert") ? Eval("AgencyName") : Eval("SourceTitle")%></a>
                </div>
                </div>
                <div class="listing_price">
                    <div class='<%# Eval("PriceOld").ToString() != "" && Eval("Price").ToString() != "" ? (Convert.ToDecimal(Eval("PriceOld")) > Convert.ToDecimal(Eval("Price")) ? "result_item_price_lower" : "result_item_price_upper") : "" %>'
                            style="<%# Eval("PriceOld").ToString() == Eval("Price").ToString() || Eval("PriceOld").ToString().Replace(",00", "").Replace(".00", "") == "0" ? "display:none": "" %>">

                        <%# (Eval("PriceOld").ToString() != "" && Eval("Price").ToString() != "" ? (Convert.ToDecimal(Eval("PriceOld")) > Convert.ToDecimal(Eval("Price")) ? "-" : "+") : "") + String.Format("{0:0.}", Math.Abs(Convert.ToDecimal(Eval("PriceOld")) - Convert.ToDecimal(Eval("Price")))) + "" + Eval("CurrencySymbol").ToString()%>
                        <div class="<%# Eval("PriceOld").ToString() != "" && Eval("Price").ToString() != "" ? (Convert.ToDecimal(Eval("PriceOld")) > Convert.ToDecimal(Eval("Price")) ? "result_item_price_lower_tip" : "result_item_price_upper_tip") : "" %>"></div>  
                    </div>
                    <%# Eval("Price").ToString().Replace(",00", "").Replace(".00", "") == "0" ? Resources.placeberry.Vacation_UponRequest : Eval("Price").ToString().Replace(",00", "").Replace(".00", "") + " " + Eval("CurrencySymbol").ToString()%>
                    <span><%# Eval("Price").ToString().Replace(",00", "").Replace(".00", "") == "0" ? string.Empty : Resources.placeberry.Vacation_PerDay %></span>
                </div>
                <div class="info_zone">
                    <span id="info-<%# Eval("Id") %>" class="info_btn"><!-- --></span>
                    <span class="persons"><%# Common.FormatCapacity(Eval("CapacityMin"), Eval("CapacityMax")) %></span>
                    <span class="type"><%# Eval("AccommType")%></span>
                </div>     
            </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
