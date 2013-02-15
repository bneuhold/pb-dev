<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferCreateEdit.aspx.cs" Inherits="admin_OfferCreateEdit" MasterPageFile="~/MasterAdmin.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/styles/admin.css"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    
<div class="main_content">

<div style="display:inline-block;">

    <div style="clear:both; display:block; float:left;">
    <table class="offer_form">
        <tr>
            <td colspan="2" style="text-align:left; padding-bottom:30px;"><a href="<%= Page.ResolveUrl("~/admin/OfferList.aspx") %>">Povratak na pregled ponuda</a></td>
        </tr>
        <tr>
            <td></td><td style="text-align:center; font-size:15px; font-weight:bold;"><asp:Label runat="server" ID="lblOperationTitle"></asp:Label></td>
        </tr>
        <tr>
            <td></td><td style="text-align:center; padding:20px 0;"><asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label><asp:Label runat="server" ID="lblMsgSuccess" ForeColor="Green"></asp:Label></td>
        </tr>
        <tr runat="server" ID="trId">
            <td>ID:</td><td><asp:TextBox runat="server" ID="tbId" Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Naziv ponude:</td><td><asp:TextBox runat="server" ID="tbName"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqName" controltovalidate="tbName" errormessage=" *" /></td>
        </tr>
        <tr>
            <td>Agencija:</td><td><asp:DropDownList runat="server" ID="ddlAgencies"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Kategorija:</td><td><asp:DropDownList runat="server" ID="ddlCategories"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prava cijena:</td><td><asp:TextBox runat="server" ID="tbPriceReal"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqPriceReal" controltovalidate="tbPriceReal" errormessage=" *" /></td>
        </tr>
        <tr>
            <td>Ušteda:</td><td><asp:TextBox runat="server" ID="tbPriceSave"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqPriceSave" controltovalidate="tbPriceSave" errormessage=" *" /></td>
        </tr>
        <tr>
            <td>Cijena:</td><td><asp:TextBox runat="server" ID="tbPrice"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqPrice" controltovalidate="tbPrice" errormessage=" *" /></td>
        </tr>
        <tr>
            <td>Popust(%):</td><td><asp:TextBox runat="server" ID="tbDiscount"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Valuta:</td><td><asp:DropDownList runat="server" ID="ddlCurrency"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Status:</td><td><asp:DropDownList runat="server" ID="ddlStatus"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Datum početka ponude:</td><td><asp:TextBox runat="server" ID="tbDateStart"></asp:TextBox><ajaxtoolkit:CalendarExtender ID="calDateStart"   
                                                                                                                                        runat="server"   
                                                                                                                                        PopupPosition="Right"  
                                                                                                                                        TargetControlID="tbDateStart">  
                                                                                                                                    </ajaxtoolkit:CalendarExtender>
                            <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqDateStart" controltovalidate="tbDateStart" errormessage=" *" />
                                                                                                                                    </td>
        </tr>
        <tr>
            <td>Datum završetka ponude:</td><td><asp:TextBox runat="server" ID="tbDateEnd"></asp:TextBox><ajaxtoolkit:CalendarExtender ID="calDateEnd"   
                                                                                                                                        runat="server"   
                                                                                                                                        PopupPosition="Right"  
                                                                                                                                        TargetControlID="tbDateEnd">  
                                                                                                                                    </ajaxtoolkit:CalendarExtender>
                        <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqDateEnd" controltovalidate="tbDateEnd" errormessage=" *" />
                                                                                                                                    </td>    
        </tr>
        <tr>
            <td>Datum početka kupona:</td><td><asp:TextBox runat="server" ID="tbDateCouponStart"></asp:TextBox><ajaxtoolkit:CalendarExtender ID="calDateCouponStart"   
                                                                                                                                        runat="server"   
                                                                                                                                        PopupPosition="Right"  
                                                                                                                                        TargetControlID="tbDateCouponStart">
                                                                                                                                    </ajaxtoolkit:CalendarExtender>
                        <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqDateCouponStart" controltovalidate="tbDateCouponStart" errormessage=" *" />
                                                                                                                                    </td>
        </tr>
        <tr>
            <td>Datum završetka kupona:</td><td><asp:TextBox runat="server" ID="tbDateCouponEnd"></asp:TextBox><ajaxtoolkit:CalendarExtender ID="calDateCouponEnd"   
                                                                                                                                        runat="server"   
                                                                                                                                        PopupPosition="Right"  
                                                                                                                                        TargetControlID="tbDateCouponEnd">  
                                                                                                                                    </ajaxtoolkit:CalendarExtender>
                        <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqDateCouponEnd" controltovalidate="tbDateCouponEnd" errormessage=" *" />
                                                                                                                                    </td>
        </tr>
        <tr>
            <td>Broj osoba:</td><td><asp:TextBox runat="server" ID="tbNumberOfPersons"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Broj kupona po osobi:</td><td><asp:TextBox runat="server" ID="tbNumberOfCouponsPerUser"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Tip ponude:</td><td><asp:TextBox runat="server" ID="tbOfferType"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Longitude:</td><td><asp:TextBox runat="server" ID="tbLongitude"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Latitude:</td><td><asp:TextBox runat="server" ID="tbLatitude"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Minimalano kupona za prodaju:</td><td><asp:TextBox runat="server" ID="tbMinBoughtCount"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqMinBoughtCount" controltovalidate="tbMinBoughtCount" errormessage=" *" /></td>
        </tr>
        <tr>
            <td>Maksimalano kupona za prodaju:</td><td><asp:TextBox runat="server" ID="tbMaxBoughtCount"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqMaxBoughtCount" controltovalidate="tbMaxBoughtCount" errormessage=" *" /></td>
        </tr>
        <tr runat="server" id="trBoughtCount">
            <td>Broj prodanih kupona:</td><td><asp:TextBox runat="server" ID="tbBoughtCount" Enabled="false"></asp:TextBox></td>
        </tr>
        <tr runat="server" id="trClickCount">
            <td>Broj klikova:</td><td><asp:TextBox runat="server" ID="tbClickCount" Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Prioritet:</td><td><asp:TextBox runat="server" ID="tbPriority"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Aktivna:</td><td><asp:CheckBox runat="server" ID="cbActive" CssClass="cb_active" /></td>
        </tr>
        <tr>
            <td></td><td style="text-align:center"><asp:Button runat="server" ID="btnSaveOffer" Text="Spremi ponudu" /></td>
        </tr>
    </table>
    </div>
    <div style="display:block; float:left; margin:98px 0 0 14px;">
        <asp:Repeater id="rptPlaces" runat="server">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th colspan="2" style="padding-bottom:10px;">Odaberite mjesta:</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><asp:Label runat="server" ID="lblTitle" AssociatedControlID="cbPlace"><%# Eval("Title") %></asp:Label></td>
                    <td><asp:CheckBox runat="server" ID="cbPlace" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

</div>

</div>


</asp:Content>
