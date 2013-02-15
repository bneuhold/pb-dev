<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveOfferTranslations.aspx.cs" Inherits="manage_CollectiveOfferTranslations" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="main_content">

<div class="grid">
    
    <div class="operation_title">
        <div style="display:block; text-align:left; font-size:13px;"><a href="<%= Page.ResolveUrl("~/manage/CollectiveOfferList.aspx") %>">Povratak</a></div>
        <p>Prijevodi ponuda</p>
        <p style="margin-top:15px; font-size:14px;">Ponuda:&nbsp;<b><asp:Label ID="lblOfferName" runat="server"></asp:Label></b></p>
    </div>

    
    <asp:GridView ID="grdOfferTrans" runat="server" AutoGenerateColumns="False" ShowFooter="false" EnableViewState="true" DataKeyNames="OfferId, LanguageId" BorderStyle="None" GridLines="None">                    

        <Columns>
            <asp:TemplateField HeaderText="Language"> 
                <HeaderStyle CssClass="grid_header" Width="100" />
                <ItemStyle CssClass="grid_item" />
                <ItemTemplate> 
                    <asp:Label ID="lblLang" runat="server"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Title" HeaderText="Title" ReadOnly="True"
                HeaderStyle-Width="300" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" FooterStyle-CssClass="grid_footer" />

            <asp:TemplateField HeaderText="Content Short"> 
                <HeaderStyle CssClass="grid_header" Width="300" />
                <ItemStyle CssClass="grid_item" />
                <ItemTemplate> 
                    <asp:Literal ID="lblContentShort" runat="server" Text='<%# ((Collective.OfferTranslation)Container.DataItem).ContentShort %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="">
                <HeaderStyle CssClass="grid_header" Width="100" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlEdit" Text="Edit"></asp:HyperLink>
                    <asp:LinkButton runat="server" ID="lbDelete" Text="Delete" CommandName="Delete" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>


<div class="err_msg">
    <asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label>
</div>

    <div class="back_link" style="text-align:center; margin-top:20px;">
        <a href="<%= Page.ResolveUrl("~/manage/CollectiveOfferTranslationCreateEdit.aspx?offerid=") + GetOffer().OfferId.ToString() %>">Dodaj novi prijevod</a>
    </div>

</div>

</div>

</asp:Content>