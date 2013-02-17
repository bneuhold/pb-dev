<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveCategoryList.aspx.cs" Inherits="manage_CollectiveCategoryList" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<div class="main_content">

<div class="grid">

    <div class="back_link">
        <a href="<%= Page.ResolveUrl("~/manage/CollectiveAdminHome.aspx") %>">Povratak</a>
    </div>

    <div class="operation_title">
        Pregled kategorija
    </div>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block">
    <ContentTemplate>

        <asp:GridView ID="grdCategories" runat="server" AutoGenerateColumns="False" ShowFooter="true" EnableViewState="true" DataKeyNames="Id" BorderStyle="None" GridLines="None">                    

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True"
                    HeaderStyle-Width="60" HeaderStyle-CssClass="grid_header" ItemStyle-CssClass="grid_item" FooterStyle-CssClass="grid_footer" />

                <asp:TemplateField HeaderText="Name" HeaderStyle-Width="300">
                    <HeaderStyle CssClass="grid_header" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />

                    <ItemTemplate> 
                        <asp:Label ID="lblName" runat="server" Text='<%# ((Collective.Category)Container.DataItem).Name %>'></asp:Label> 
                    </ItemTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="tbName" runat="server" Text='<%# ((Collective.Category)Container.DataItem).Name %>' Width="300"></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="tbNewName" runat="server" Width="300"></asp:TextBox> 
                    </FooterTemplate> 
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Priority" HeaderStyle-Width="80">
                    <HeaderStyle CssClass="grid_header" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />

                    <ItemTemplate> 
                        <asp:Label ID="lblPriority" runat="server" Text='<%# ((Collective.Category)Container.DataItem).Priority.ToString() %>'></asp:Label> 
                    </ItemTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="tbPriority" runat="server" Text='<%# ((Collective.Category)Container.DataItem).Priority.ToString() %>' Width="80"></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="tbNewPriority" runat="server" Width="80"></asp:TextBox> 
                    </FooterTemplate> 
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Active"> 
                    <HeaderStyle CssClass="grid_header" Width="80" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />
                    <ItemTemplate> 
                        <asp:CheckBox ID="cbViewActive" runat="server" Enabled="false" Checked='<%# ((Collective.Category)Container.DataItem).Active %>'></asp:CheckBox> 
                    </ItemTemplate>
                    <EditItemTemplate> 
                        <asp:CheckBox ID="cbActive" runat="server" Enabled="true" Checked="<%# ((Collective.Category)Container.DataItem).Active %>"></asp:CheckBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:CheckBox ID="cbNewActive" runat="server" Enabled="true" Checked="true"></asp:CheckBox> 
                    </FooterTemplate> 
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="grid_header" Width="200" />
                    <ItemStyle CssClass="grid_item" />
                    <FooterStyle CssClass="grid_footer" />

                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbEdit" Text="Edit" CommandName="Edit" />
                        <asp:LinkButton runat="server" ID="lbDelete" Text="Delete" CommandName="Delete" />
                        <asp:HyperLink runat="server" ID="hlEditTrans" Text="Translations"></asp:HyperLink>
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


        <asp:HiddenField runat="server" ID="hfSavedNewName" />
        <asp:HiddenField runat="server" ID="hfSavedPriority" />
        <asp:HiddenField runat="server" ID="hfSavedNewActive" />

    <div class="err_msg">
        <asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label>
    </div>

    </ContentTemplate>
    </asp:UpdatePanel>

</div>

</div>

</asp:Content>
