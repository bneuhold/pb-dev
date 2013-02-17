<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectiveCategoryTranslations.aspx.cs" Inherits="manage_CollectiveCategoryTranslations" MasterPageFile="~/MasterHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" type="text/css" href="../resources/css/collective_admin.css"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<div class="main_content">

<div class="grid">

<asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>

    <asp:HiddenField ID="hfCreatedCategoryId" runat="server" />

    <div class="operation_title">
        <div style="display:block; text-align:left; font-size:13px;"><asp:HyperLink runat="server" ID="hlBackToCatLst">Povratak</asp:HyperLink></div>
        <p>Uređivanje prijevoda kategorije</p>
    </div>


    <div class="cat_data">
        <p>Naziv kategorije:&nbsp;<b><asp:Label ID="lblCatName" runat="server"></asp:Label></b></p>
    </div>
                

    <div class="edit_trans_title">Uređivanje prijevoda</div>
        

    <asp:GridView ID="grdTrans" runat="server" AutoGenerateColumns="False" ShowFooter="true" EnableViewState="true" DataKeyNames="CategoryId,LanguageId" BorderStyle="None" GridLines="None">                    

        <Columns>
            <asp:TemplateField HeaderText="Jezik"> 
                <HeaderStyle CssClass="grid_header" Width="100" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />
                <ItemTemplate> 
                    <asp:Label ID="lblLang" runat="server"></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:Label ID="lblLangEdit" runat="server"></asp:Label> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:DropDownList runat="server" ID="ddlNewLang"></asp:DropDownList>
                </FooterTemplate> 
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Title" HeaderStyle-Width="300">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate> 
                    <asp:Label ID="lblTitle" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Title %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbTitle" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Title %>' Width="300"></asp:TextBox> 
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
                    <asp:Label ID="lblDescription" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Description %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Description %>' Width="500"></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="tbNewDescription" runat="server" TextMode="MultiLine" Width="500"></asp:TextBox> 
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
    <asp:HiddenField runat="server" ID="hfSavedNewLangId" />

<div class="err_msg">
    <asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label>
</div>

</ContentTemplate>
</asp:UpdatePanel>

</div>

</div>

</asp:Content>