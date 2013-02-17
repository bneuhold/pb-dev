<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveOfferTranslationCreateEdit.aspx.cs" Inherits="manage_CollectiveOfferTranslationCreateEdit" MasterPageFile="~/MasterHome.master" %>

<%@Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="main_content">


    <div class="operation_title">
    <div style="display:inline-block;">
        <div class="back_link">
            <a href="<%= Page.ResolveUrl("~/manage/CollectiveOfferTranslations.aspx?offerid=") + GetOffer().OfferId.ToString() %>">Povratak</a>
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
            <p class="offer_trans">Title: <asp:TextBox runat="server" ID="tbTitle"></asp:TextBox></p>            
        </div>
        <br />
        <br />
        <div style="display:inline-block; text-align:left;">
            
            <p class="offer_trans">Content short:</p>
            <asp:TextBox ID="tbContentShort" runat="server" TextMode="MultiLine" Height="150" Width="590"></asp:TextBox> 
            <p class="offer_trans">Content text:</p>
            <CKEditor:CKEditorControl ID="ckeContentText" BasePath="~/manage/ckeditor" runat="server" Toolbar="Basic" Height="200" Width="600"></CKEditor:CKEditorControl>
            <p class="offer_trans">Reservation text:</p>
            <CKEditor:CKEditorControl ID="ckeReservationText" BasePath="~/manage/ckeditor" runat="server" Toolbar="Basic" Height="200" Width="600"></CKEditor:CKEditorControl>
        </div>
    </div>
    </div>


    <div class="btn_save">
        <asp:Button runat="server" ID="btnSubmit" Text="Spremi" />
    </div>


</div>

</asp:Content>