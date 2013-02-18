<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveOfferImages.aspx.cs" Inherits="manage_CollectiveOfferImages" MasterPageFile="~/MasterHome.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="/resources/scripts/jcrop/jquery.Jcrop.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jcrop/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        // jCrop
        $(function () {

            $("#<%= imgThumb.ClientID %>").Jcrop({
                onChange: showCoords,
                onSelect: showCoords,
                addClass: 'jcrop-dark',
                setSelect: [0, 0, 2000, 2000]
            });
        });

        function showCoords(c) {
            var obj = $("#<%= hfCrop.ClientID %>");
            obj.val(c.x + "," + c.y + "," + c.w + "," + c.h);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<div class="main_content">

    <div class="operation_title" style="margin-bottom:0;">
        <div style="display:block; text-align:left; font-size:13px; margin-left:20%;"><a href="<%= Page.ResolveUrl("~/manage/CollectiveOfferList.aspx") %>">Povratak</a></div>
        <p>Uređivanje galerije ponude</p>
        <p style="font-size:13px; margin-top:20px;">Naziv kategorije:&nbsp;<b><%= GetOffer() != null ? GetOffer().OfferName : string.Empty %></b></p>
    </div>

    <div style="display:inline-block;">

    <table class="offer_upload_img">
        <tr>
            <td colspan="2" style="text-align:center; padding:20px 0;"><asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label><asp:Label runat="server" ID="lblMsgSuccess" ForeColor="Green"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2" style="padding-bottom:10px;">
                <b>Dodavanje nove slike:</b>
            </td>
        </tr>
        <tr>
            <td>Naslov slike:</td><td><asp:TextBox runat="server" ID="tbNewTitle"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqNewTitle" controltovalidate="tbNewTitle" errormessage=" *" ValidationGroup="uploadimage" /></td>
        </tr>
        <tr>
            <td>Opis:</td><td><asp:TextBox runat="server" ID="tbNewDesc"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqNewDesc" controltovalidate="tbNewDesc" errormessage=" *" ValidationGroup="uploadimage" /></td>
        </tr>
        <tr>
            <td>Redni broj:</td><td><asp:TextBox runat="server" ID="tbNewOrder"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Aktivno:</td><td><asp:CheckBox runat="server" ID="cbNewActive" /></td>
        </tr>
        <tr>
            <td>Odaberite sliku:</td>
            <td><asp:FileUpload ID="fuAddNewImage" runat="server" /><asp:RequiredFieldValidator ID="reqNewImage" runat="server" ErrorMessage=" *" ControlToValidate="fuAddNewImage" ValidationGroup="uploadimagepreview" /><asp:Button style="margin-left:10px;" ID="btnUploadNewImagePreview" Text="Učitaj sliku" runat="server" ValidationGroup="uploadimagepreview" /></td>
        </tr>

    </table>

    </div>
    <br />
    <div id="preview" style="display:inline-block; margin-top:20px;">
        <img id="imgThumb"  src="" alt="" runat="server" visible="false" />
        <asp:HiddenField ID="hfCrop" runat="server" Value="" />
    </div>
    <br />
    <div style="display:inline-block; margin-top:20px;">
        <asp:Button ID="btnSaveNewImage" Text="Spremi sliku" runat="server" ValidationGroup="uploadimage" />
    </div>

    <br />

    
    <div style="display:inline-block; margin-top:20px;">

        <asp:UpdatePanel id="upHidFields" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfLastEditItemIndex" />
            <asp:HiddenField runat="server" ID="hfEditItemIndex" />
        </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel id="upListImages" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>

        <asp:Repeater ID="rptOfferImages" runat="server">
            <HeaderTemplate>
                <table>
                <thead>
                    <tr>
                    <th colspan="2" style="padding-bottom:20px;"><b>Pregled postojecih slika:</b></th>
                    </tr>
                </thead>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="vertical-align:top; width:320px;">
                        <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                            <table class="offer_upload_img">
                            <tr>
                                <td style="width:80px;">Naziv slike:</td>
                                <td><asp:Label style="vertical-align:top;" runat="server" ID="lblTitle"></asp:Label><asp:TextBox runat="server" ID="tbEditTitle" Visible="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Opis slike:</td>
                                <td style="padding-bottom:7px;"><asp:Label runat="server" ID="lblDesc"></asp:Label><asp:TextBox runat="server" ID="tbEditDesc" Visible="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Redni broj:</td>
                                <td><asp:Label runat="server" ID="lblOrder"></asp:Label><asp:TextBox runat="server" ID="tbOrder" Visible="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Aktivno:</td>
                                <td><asp:CheckBox runat="server" ID="cbActive" /></td>
                            </tr>
                            <tr>
                                <td style="vertical-align:bottom;"><asp:LinkButton runat="server" ID="lbEdit" Text="Izmijeni"></asp:LinkButton><asp:LinkButton runat="server" ID="lbSave" Text="Spremi"></asp:LinkButton></td>
                                <td style="vertical-align:bottom;"><asp:LinkButton runat="server" ID="lbDelete" Text="Obriši"></asp:LinkButton><asp:LinkButton runat="server" ID="lbCancel" Text="Odustani" Visible="false"></asp:LinkButton></td>
                            </tr>
                            </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <div id="<%# "img_" + Eval("ImageId") %>">
                            <img src="<%# Eval("Src") %>" alt="<%# Eval("Alt") %>" title="<%# Eval("Title") %>" longdesc="<%# Eval("Description") %>" />
                        </div>
                    </td>
                </tr>

                </div>

            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        </ContentTemplate>
        </asp:UpdatePanel>

    </div>

</div>

</asp:Content>