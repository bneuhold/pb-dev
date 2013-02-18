<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectivePlacesList.aspx.cs" Inherits="manage_CollectivePlacesList" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<div class="main_content">

<div class="grid">

    <div class="back_link">
        <asp:HyperLink runat="server" ID="hlBack">Povratak</asp:HyperLink>
    </div>

    <div class="operation_title">
        Pregled mjesta
    </div>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block">
    <ContentTemplate>

        <asp:GridView ID="grdPlaces" runat="server" AutoGenerateColumns="False" ShowFooter="true" EnableViewState="true" DataKeyNames="Id" BorderStyle="None" GridLines="None">                    

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True"
                    HeaderStyle-Width="60" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" FooterStyle-CssClass="grid_footer" />

                <asp:TemplateField HeaderText="Title" HeaderStyle-Width="300">
                    <HeaderStyle CssClass="grid_header" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />

                    <ItemTemplate> 
                        <asp:Label ID="lblTitle" runat="server" Text='<%# ((Collective.Place)Container.DataItem).Title %>'></asp:Label> 
                    </ItemTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="tbTitle" runat="server" Text='<%# ((Collective.Place)Container.DataItem).Title %>' Width="300"></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="tbNewTitle" runat="server" Width="300"></asp:TextBox> 
                    </FooterTemplate> 
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="500"> 
                    <HeaderStyle CssClass="grid_header" />
                    <ItemStyle CssClass="grid_item grid_left" />
                    <FooterStyle CssClass="grid_footer" />

                    <ItemTemplate> 
                        <asp:Label ID="lblDescription" runat="server" Text='<%# ((Collective.Place)Container.DataItem).Description %>'></asp:Label> 
                    </ItemTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Text='<%# ((Collective.Place)Container.DataItem).Description %>' Width="500"></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="tbNewDescription" runat="server" TextMode="MultiLine" Width="500"></asp:TextBox> 
                    </FooterTemplate> 
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Active"> 
                    <HeaderStyle CssClass="grid_header" Width="80" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />
                    <ItemTemplate> 
                        <asp:CheckBox ID="cbViewActive" runat="server" Enabled="false" Checked='<%# ((Collective.Place)Container.DataItem).Active %>'></asp:CheckBox> 
                    </ItemTemplate>
                    <EditItemTemplate> 
                        <asp:CheckBox ID="cbActive" runat="server" Enabled="true" Checked="<%# ((Collective.Place)Container.DataItem).Active %>"></asp:CheckBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:CheckBox ID="cbNewActive" runat="server" Enabled="true" Checked="true"></asp:CheckBox> 
                    </FooterTemplate> 
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="grid_header" Width="100" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />

                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbEdit" Text="Edit" CommandName="Edit" />
                        <asp:LinkButton runat="server" ID="lbDelete" Text="Delete" CommandName="Delete" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" ID="lbUpdate" Text="Update" CommandName="Update" />
                        <asp:LinkButton runat="server" ID="lbCancel" Text="Cancel" CommandName="Cancel" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton runat="server" ID="lbInsert" Text="Insert" CommandName="InsertNew" />
                    </FooterTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>


        <asp:HiddenField runat="server" ID="hfSavedNewTitle" />
        <asp:HiddenField runat="server" ID="hfSavedNewDesc" />
        <asp:HiddenField runat="server" ID="hfSavedNewActive" />

    <div class="err_msg">
        <asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label>
    </div>

    </ContentTemplate>
    </asp:UpdatePanel>

</div>

</div>

</asp:Content>