<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CollectiveOfferList.ascx.cs" Inherits="Controls_CollectiveOfferList" %>

<asp:Repeater ID="rptOffers" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
            <li>
            <div style="clear:both; float:left;"><img src='<%# Eval("FirstImgSrc") %>' alt='<%# Eval("OfferName") %>' /></div>
            <div style="float:left;">
                <h1><%# Eval("OfferTitle")%></h1>
                <h2><%# Eval("ContentShort")%></h2>
                <p>Vrijedi: <%# Eval("PriceReal") %>, Popust: <%# Eval("Discount") %>%</p>
            </div>
            </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
