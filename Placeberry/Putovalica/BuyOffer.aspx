<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuyOffer.aspx.cs" Inherits="BuyOffer" MasterPageFile="~/MasterPage.master" %>

<%@ Import Namespace="System.Globalization"%>
<%@ Register Src="/Controls/OfferSidebar.ascx" TagName="OfferSidebar" TagPrefix="uc1" %>
<%@ Register Src="/Controls/FormRegistrationCtrl.ascx" TagName="Registration" TagPrefix="uc1" %>
<%@ Register Src="/Controls/FormLoginCtrl.ascx" TagName="Login" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

    function changeCount(ddl) {

        var ddlCtrl = $(ddl);
        var price = parseFloat(ddlCtrl.val()) * parseFloat(ddlCtrl.attr("basicPrice"));
        $("#lblSum").text(price);

        var aBuy = $("#aBuy");

        if (_userBoughtCount + parseInt(ddlCtrl.val()) > _maxBoughtCount) {

            $("#lblErrorMsg").css({ display: "block" });

            aBuy.attr("onclick", "");
            aBuy.css({ opacity: "0.4" });
        }
        else {
            $("#lblErrorMsg").css({ display: "none" });

            aBuy.attr("onclick", "submitForm(this);");
            aBuy.css({ opacity: "1" });
        }
    }

    function selPayMethod(radio) {

        $("#div_card").css("display", "none");
        $("#div_eBank").css("display", "none");

        $("#div_" + $(radio).attr("id").split("_")[1]).css("display", "block");
    }

    function submitForm(aSub) {

        var url = "/WSpay/WSpayPreForm.aspx?OfferId=<%= _currOffer.OfferId.ToString() %>&count=" + $("#<%= ddlSelCount.ClientID %>").val();
        var form = $(aSub).closest('form');
        form.attr("target", "_blank");
        form.attr("action", url);
        form.submit();
    }

    $(document).ready(function () {

        if (_userBoughtCount >= _maxBoughtCount) {

            $("#lblErrorMsg").css({ display: "block" });

            var aBuy = $("#aBuy");
            aBuy.attr("onclick", "");
            aBuy.css({ opacity: "0.4" });
        }
    });


</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <uc1:OfferSidebar runat="server" ID="ctrlOfferSidebar" />


    <!-- START .content -->

    <div class="content">
        <article class="main-offer" id="offer-main">

            <table class="buy-offer-table">
                <thead>
                    <tr>
                        <th>Ponuda</th>
                        <th></th>
                        <th>Cijena</th>
                        <th>Kolicina</th>
                        <th>Ukupan iznos</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><img width="154" height="86" src="<%= String.IsNullOrEmpty(_currOffer.FirstImgSrc) ? "/uploads/offerimages/default.jpg" :_currOffer.FirstImgSrc %>" alt="<%=_currOffer.Translation.Title %>"></td>
                        <td><%= _currOffer.Translation.Title %></td>
                        <td><%= _currOffer.Price.ToString() + " " + _currOffer.CurrencySymbol %></td>
                        <td><asp:DropDownList AutoPostBack="false" runat="server" ID="ddlSelCount"></asp:DropDownList></td>
                        <td><label id="lblSum"><%= (_currOffer.Price * Double.Parse(this.ddlSelCount.SelectedValue)).ToString() %></label>&nbsp;<%= _currOffer.CurrencySymbol%></td>
                    </tr>
                </tbody>
            </table>

            <div class="description">

                <asp:PlaceHolder runat="server" ID="phLoginRegister">

                    <div class="login-register-inner" style="width:380px; height:740px; vertical-align:top;">
                        <uc1:Registration runat="server" ID="ctrlRegistration" />
                    </div>
                    <div class="login-register-inner" style="width:380px; height:680px; vertical-align:top;">
                        <uc1:Login runat="server" ID="ctrlLogin" />
                    </div>

                </asp:PlaceHolder>

                <div class="details">

                    <label id="lblErrorMsg" style="font-size:14px; color:Red; display:none;">Kupnju za odabrani broj bonova nije moguće ostvariti. <br /> Imate kupljeno već <%= _userBoughtCount.ToString() %> kupona.</label>

                <asp:PlaceHolder runat="server" ID="phBuyForm">
                <div style="width:100%;">

                    <p>Načini plačanja</p>
                    <p><input type="radio" id="radio_card" name="payMethodSel" checked="checked" onclick="javascript:selPayMethod(this);" /><label for="radio_card">Kreditne kartice</label></p>
                    
                    <div id="div_card" style="width:100%; min-height:150px; display:block; padding:10px;">
                        <p>Tu dode odabir kartice...</p>


                        <a id="aBuy" href="javascript:void(0);" class="buy-next-button" onclick="submitForm(this);">Kupi</a>

                    </div>

                    <p><input type="radio" id="radio_eBank" name="payMethodSel" onclick="javascript:selPayMethod(this);" /><label for="radio_eBank">E-bankarstvo / Uplatnica</label></p>
                    <div id="div_eBank" style="width:100%; display:none; padding:10px;">
                        <p>Tu je e-bankarstvo i ovak link vodi na formu za ispis...</p>
                        <a class="buy-next-button" href="<%= Page.ResolveUrl("~/OfferBuyEbankForm.aspx?offerid=" + _currOffer.OfferId.ToString()) %>" onclick="javascript:addNumOfCoups(this)">Kupi</a>
                    </div>                    

                </div>
                </asp:PlaceHolder>

                </div>
            </div>


        </article>

    </div> <!-- END .content -->

</asp:Content>