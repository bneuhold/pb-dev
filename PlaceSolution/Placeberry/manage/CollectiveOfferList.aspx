<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveOfferList.aspx.cs" Inherits="manage_CollectiveOfferList" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<div class="main_content">

<div class="grid">

    <div class="back_link" style="margin-left:10px;">
        <asp:HyperLink runat="server" ID="hlBack">Povratak</asp:HyperLink>
    </div>

    <div class="operation_title">
        Pregled ponuda
    </div>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block">
    <ContentTemplate>

    <asp:GridView ID="grdOffers" runat="server" AutoGenerateColumns="False" ShowFooter="false" EnableViewState="true" DataKeyNames="OfferId" BorderStyle="None" GridLines="None">                    

        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="60">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <HeaderTemplate>
                    <asp:HyperLink runat="server" ID="hlOfferIdSort">ID</asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate> 
                    <asp:Label ID="lblId" runat="server" Text='<%# ((Collective.Offer)Container.DataItem).OfferId %>'></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Offer Name" HeaderStyle-Width="300">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <HeaderTemplate>
                    <asp:HyperLink runat="server" ID="hlOfferNameSort">Offer Name</asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate> 
                    <asp:Label ID="lblOfferName" runat="server" Text='<%# ((Collective.Offer)Container.DataItem).OfferName %>'></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Category Name" HeaderStyle-Width="300">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <HeaderTemplate>
                    <asp:HyperLink runat="server" ID="hlCategoryNameSort">Category Name</asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate> 
                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# ((Collective.Offer)Container.DataItem).CategoryName %>'></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Bought Count" HeaderStyle-Width="100">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <HeaderTemplate>
                    <asp:HyperLink runat="server" ID="hlBoughtCountSort">BoughtCount</asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate> 
                    <asp:Label ID="lblBoughtCount" runat="server" Text='<%# ((Collective.Offer)Container.DataItem).BoughtCount %>'></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date Start" HeaderStyle-Width="100">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <HeaderTemplate>
                    <asp:HyperLink runat="server" ID="hlDateStartSort">Date Start</asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate> 
                    <asp:Label ID="lblDateStart" runat="server"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date End" HeaderStyle-Width="100">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <ItemTemplate> 
                    <asp:Label ID="lblDateEnd" runat="server"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="OfferStatus" HeaderText="Status" ReadOnly="True"
                HeaderStyle-Width="100" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" FooterStyle-CssClass="grid_footer" />

            <asp:TemplateField HeaderText="Min/Max Bought Count" HeaderStyle-Width="120">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />

                <ItemTemplate> 
                    <asp:Label ID="lblMinMaxBoughtCount" runat="server"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Priority" HeaderText="Priority" ReadOnly="True"
                HeaderStyle-Width="100" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" FooterStyle-CssClass="grid_footer" />

            <asp:TemplateField HeaderText="Active"> 
                <HeaderStyle CssClass="grid_header" Width="80" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="cbActive" OnCheckedChanged="cbActive_CheckedChanged" AutoPostBack="true" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="">
                <HeaderStyle CssClass="grid_header" Width="200" />
                <ItemStyle CssClass="grid_item" />

                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlEdit" Text="Edit"></asp:HyperLink>
                    <asp:LinkButton runat="server" ID="lbDelete" Text="Delete" CommandName="Delete" />
                    <asp:HyperLink runat="server" ID="hlEditTrans" Text="Translations"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlImages" Text="Images"></asp:HyperLink>
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

    <div class="back_link" style="text-align:center; margin-top:20px;">
        <asp:HyperLink runat="server" ID="hlCreateNewOffer">Kreiraj novu ponudu</asp:HyperLink>
    </div>

    </ContentTemplate>
    </asp:UpdatePanel>

</div>

</div>

</asp:Content>
