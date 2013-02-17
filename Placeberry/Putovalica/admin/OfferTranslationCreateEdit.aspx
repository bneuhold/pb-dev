<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferTranslationCreateEdit.aspx.cs" Inherits="admin_OfferTranslationCreateEdit" MasterPageFile="~/MasterAdmin.master" ValidateRequest="false" %>

<%@Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/styles/admin.css"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="main_content">


    <div class="operation_title">
    <div style="display:inline-block;">
        <div class="back_link">
            <a href="<%= Page.ResolveUrl("~/admin/OfferTranslations.aspx?offerid=") + GetOffer().OfferId.ToString() %>">Povratak</a>
        </div>
        <asp:Label runat="server" ID="lblOperationName"></asp:Label>
        <p style="margin-top:20px; font-size:13px;">Ponuda:&nbsp;<b><asp:Label ID="lblOfferName" runat="server"></asp:Label></b></p>

        <asp:PlaceHolder runat="server" ID="phCreate">
            <p style="margin-top:15px; font-size:13px;">Odaberite jezik: <asp:DropDownList runat="server" ID="ddlLaguages"></asp:DropDownList></p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phEdit">
            <p style="margin-top:15px; font-size:13px;">Jezik: <b><asp:Label runat="server" ID="lblLanguage"></asp:Label></b></p>
        </asp:PlaceHolder>


        <div class="err_msg">
            <asp:Label runat="server" ID="lblSuccessMsg" ForeColor="Green"></asp:Label>
            <asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label>
        </div>

        <div style="display:inline-block; text-align:center;">

            <p class="offer_trans">Title: <asp:TextBox runat="server" ID="tbTitle"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqTitle" controltovalidate="tbTitle" errormessage=" *" /></p>            
        </div>
        <br />
        <br />
        <div style="display:inline-block; text-align:left;">
            
            <p class="offer_trans">Content short:</p>
            <asp:TextBox ID="tbContentShort" runat="server" TextMode="MultiLine" Height="150" Width="590"></asp:TextBox> 
            <p class="offer_trans">Content text:</p>
            <CKEditor:CKEditorControl ID="ckeContentText" BasePath="~/admin/ckeditor" runat="server" Toolbar="Basic" Height="200" Width="600"></CKEditor:CKEditorControl>
            <p class="offer_trans">Reservation text:</p>
            <CKEditor:CKEditorControl ID="ckeReservationText" BasePath="~/admin/ckeditor" runat="server" Toolbar="Basic" Height="200" Width="600"></CKEditor:CKEditorControl>
        </div>
        <br />
        <div style="display:inline-block; text-align:left;">

            <p class="offer_trans"><span style="display:inline-block; width:130px; text-align:right; padding-right:10px;">Meta Description:</span><asp:TextBox ID="tbMetaDesc" runat="server" Width="300"></asp:TextBox></p>
            <p class="offer_trans"><span style="display:inline-block; width:130px; text-align:right; padding-right:10px;">Meta Keywords:</span><asp:TextBox ID="tbMetaKeywords" runat="server" Width="300"></asp:TextBox></p>
            <p class="offer_trans"><span style="display:inline-block; width:130px; text-align:right; padding-right:10px;">UrlTag:</span><asp:TextBox ID="tbUrlTag" runat="server" Width="300"></asp:TextBox><asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqUrlTag" controltovalidate="tbUrlTag" errormessage=" *" /></p>
        </div>
    </div>
    </div>


    <div class="btn_save">
        <asp:Button runat="server" ID="btnSubmit" Text="Spremi" />
    </div>


</div>

</asp:Content>
