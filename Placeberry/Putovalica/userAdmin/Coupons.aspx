<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Coupons.aspx.cs" Inherits="userAdmin_Coupons" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="/Controls/UserAdminSidebar.ascx" TagName="UserAdminSidebar" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

    function toggleActiveTab(link) {

        $("#div_avail").css("display", "none");
        $("#div_used").css("display", "none");
        $("#div_timeout").css("display", "none");

        $("#a_avail").css("font-weight", "normal");
        $("#a_used").css("font-weight", "normal");
        $("#a_timeout").css("font-weight", "normal");

        $("#div_" + $(link).attr("id").split("_")[1]).css("display", "block");
        $(link).css("font-weight", "bold");
    }

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <uc1:UserAdminSidebar runat="server" ID="ctrlUserAdminSidebar" />

    <asp:UpdatePanel id="upUserProfile" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>

            <div class="user-coupons">

                <p class="menu"><a href="javascript:void(0);" id="a_avail" onclick="javascript:toggleActiveTab(this);" style="font-weight:bold;">Raspoloživo</a> | 
                <a href="javascript:void(0);" id="a_used" onclick="javascript:toggleActiveTab(this);">Iskorišteno</a> | 
                <a href="javascript:void(0);" id="a_timeout" onclick="javascript:toggleActiveTab(this);">Isteklo</a>
                </p>

                <div id="div_avail" style="display:block; text-align:center;">

                    <asp:Label runat="server" ID="lblAvilEmptyMsg" Visible="false" ForeColor="#666666" style="font-size:14px;">Nema raspolozivih kupona</asp:Label>

                    <asp:Repeater runat="server" ID="rptAvail">
                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr><th></th><th></th><th>Vrijedi</th><th>Cijena</th><th>Kupljeno</th><th>Ističe</th></tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td><a target="_blank" href="<%# Page.ResolveUrl("~/CouponView.aspx?coupid=" + Eval("Id").ToString()) %>"><img style="height:86px; width:154px; display:block;" src="<%# Page.ResolveUrl(Eval("FirstImgSrc") != null ? Eval("FirstImgSrc").ToString() : "~/uploads/offerimages/default.jpg")%>" alt="<%# Eval("OfferTitle") %>" /></a></td>
                                    <td>
                                        <a target="_blank" href="<%# Page.ResolveUrl("~/CouponView.aspx?coupid=" + Eval("Id").ToString()) %>"><%# Eval("OfferTitle") %></a>
                                        <br />
                                        <br />
                                        <a style="font-size:12px;" href="<%# Page.ResolveUrl("~/offer.aspx?offerid=" + Eval("CollectiveOfferId").ToString()) %>">pogledaj ponudu</a>
                                    </td>
                                    <td><%# Eval("OfferPriceReal") %></td>
                                    <td><%# Eval("OfferPrice") %></td>
                                    <td><%# ((DateTime)Eval("DateBought")).ToString("dd.MM.yyyy") %></td>
                                    <td><%# Eval("DateEnd") != null ? ((DateTime)Eval("DateEnd")).ToString("dd.MM.yyyy") : "-" %></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <div id="div_used" style="display:none; text-align:center;">

                    <asp:Label runat="server" ID="lblUsedEmptyMsg" Visible="false" ForeColor="#666666" style="font-size:14px;">Nema iskorištenih kupona</asp:Label>

                    <asp:Repeater runat="server" ID="rptUsed">
                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr><th></th><th></th><th>Vrijedi</th><th>Cijena</th><th>Kupljeno</th><th>Ističe</th></tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td><img style="height:86px; width:154px; display:block;" src="<%# Page.ResolveUrl(Eval("FirstImgSrc") != null ? Eval("FirstImgSrc").ToString() : "~/uploads/offerimages/default.jpg")%>" alt="<%# Eval("OfferTitle") %>" /></td>
                                    <td>
                                        <%# Eval("OfferTitle") %>
                                        <br />
                                        <br />
                                        <a style="font-size:12px;" href="<%# Page.ResolveUrl("~/offer.aspx?offerid=" + Eval("CollectiveOfferId").ToString()) %>">pogledaj ponudu</a>
                                    </td>
                                    <td><%# Eval("OfferPriceReal") %></td>
                                    <td><%# Eval("OfferPrice") %></td>
                                    <td><%# ((DateTime)Eval("DateBought")).ToString("dd.MM.yyyy") %></td>
                                    <td><%# Eval("DateEnd") != null ? ((DateTime)Eval("DateEnd")).ToString("dd.MM.yyyy") : "-" %></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <div id="div_timeout" style="display:none; text-align:center;">

                    <asp:Label runat="server" ID="lblTimeoutEmptyMsg" Visible="false" ForeColor="#666666" style="font-size:14px;">Nema isteklih kupona</asp:Label>

                    <asp:Repeater runat="server" ID="rptTimeout">
                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr><th></th><th></th><th>Vrijedi</th><th>Cijena</th><th>Kupljeno</th><th>Ističe</th></tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td><img style="height:86px; width:154px; display:block;" src="<%# Page.ResolveUrl(Eval("FirstImgSrc") != null ? Eval("FirstImgSrc").ToString() : "~/uploads/offerimages/default.jpg")%>" alt="<%# Eval("OfferTitle") %>" /></td>
                                    <td><%# Eval("OfferTitle") %>                                        
                                        <br />
                                        <br />
                                        <a style="font-size:12px;" href="<%# Page.ResolveUrl("~/offer.aspx?offerid=" + Eval("CollectiveOfferId").ToString()) %>">pogledaj ponudu</a>
                                    </td>
                                    <td><%# Eval("OfferPriceReal") %></td>
                                    <td><%# Eval("OfferPrice") %></td>
                                    <td><%# ((DateTime)Eval("DateBought")).ToString("dd.MM.yyyy") %></td>
                                    <td><%# Eval("DateEnd") != null ? ((DateTime)Eval("DateEnd")).ToString("dd.MM.yyyy") : "-" %></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>