<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponsList.aspx.cs" Inherits="admin_CouponsList" MasterPageFile="~/MasterAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/styles/admin.css"/>

    <script type="text/javascript">

        function searchUrl() {

            var offerName = $.trim($('#<%= tbOfferName.ClientID %>').val());
            var email = $.trim($('#<%= tbEmail.ClientID %>').val());
            var firstName = $.trim($('#<%= tbFirstName.ClientID %>').val());
            var lastName = $.trim($('#<%= tbLastName.ClientID %>').val());
            window.location.href = searchHref + '?page=1' + (offerName != '' ? '&offername=' + offerName : '') + (email != '' ? '&email=' + email : '') + (firstName != '' ? '&firstname=' + firstName : '') + (lastName != '' ? '&lastname=' + lastName : '');
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<div class="main_content">

<div class="grid">

    <div class="back_link">
        <a href="<%= Page.ResolveUrl("~/admin/Default.aspx") %>">Povratak</a>
    </div>

    <div class="operation_title">
        Pregled kupona
    </div>

    <div class="back_link" style="text-align:center; margin-bottom:25px;">
        <p>Naziv Ponude: <asp:TextBox runat="server" ID="tbOfferName" />&nbsp; Email: <asp:TextBox runat="server" ID="tbEmail" />&nbsp; Ime: <asp:TextBox runat="server" ID="tbFirstName" />&nbsp; Prezime: <asp:TextBox runat="server" ID="tbLastName" /> <a id="aSearch" href="javascript:void(0);" onclick="searchUrl();">Traži</a></p>
    </div>


        <asp:GridView ID="grdCoupons" runat="server" AutoGenerateColumns="False" ShowFooter="true" EnableViewState="true" DataKeyNames="Id" BorderStyle="None" GridLines="None">                    

        <AlternatingRowStyle backcolor="#F2F2F2" />
        <RowStyle BackColor="white" />

            <Columns>
                <asp:BoundField DataField="OfferName" HeaderText="OfferName" ReadOnly="True"
                    HeaderStyle-Width="250" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="UserEmail" HeaderText="Email" ReadOnly="True"
                    HeaderStyle-Width="250" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="UserFirstName" HeaderText="First Name" ReadOnly="True"
                    HeaderStyle-Width="200" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="UserLastName" HeaderText="Last Name" ReadOnly="True"
                    HeaderStyle-Width="200" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="DateStart" DataFormatString="{0:dd.MM.yyyy}" HtmlEncode="false" HeaderText="Date Start" ReadOnly="True"
                    HeaderStyle-Width="100" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="DateEnd" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Date End" ReadOnly="True"
                    HeaderStyle-Width="100" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="DateBought" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Date Bought" ReadOnly="True"
                    HeaderStyle-Width="100" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="DateUsed" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Date Used" ReadOnly="True"
                    HeaderStyle-Width="100" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="ShopingCartID" HeaderText="ShopingCartID" ReadOnly="True"
                    HeaderStyle-Width="250" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:BoundField DataField="CodeNumber" HeaderText="CodeNumber" ReadOnly="True"
                    HeaderStyle-Width="200" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" />

                <asp:TemplateField HeaderText="Active"> 
                    <HeaderStyle CssClass="grid_header" Width="80" />
                    <ItemStyle CssClass="grid_item" />
                    <ItemTemplate>

                <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block" ChildrenAsTriggers="true">
                    <ContentTemplate>

                        <asp:CheckBox runat="server" ID="cbActive" OnCheckedChanged="cbActive_CheckedChanged" AutoPostBack="true" />

                    </ContentTemplate>
                </asp:UpdatePanel>

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="grid_header" Width="80" />
                    <ItemStyle CssClass="grid_item" />
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbDelete" Text="Delete" CommandName="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>


    <div class="back_link" style="text-align:center; margin-top:20px;">        
        <asp:HyperLink runat="server" ID="hlFirst"><<</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink runat="server" ID="hlPrev"><</asp:HyperLink>&nbsp;&nbsp;
        <asp:PlaceHolder runat="server" ID="phPages" />&nbsp;
        <asp:HyperLink runat="server" ID="hlNext">></asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink runat="server" ID="hlLast">>></asp:HyperLink>
    </div>


</div>

</div>

</asp:Content>