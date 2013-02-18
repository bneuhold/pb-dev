<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CategoryTranslations.aspx.cs" Inherits="admin_CategoryTranslations" MasterPageFile="~/MasterAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/styles/admin.css"/>
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

        <AlternatingRowStyle backcolor="#F2F2F2" />
        <RowStyle BackColor="white" />

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

            <asp:TemplateField HeaderText="Title" HeaderStyle-Width="150">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate> 
                    <asp:Label ID="lblTitle" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Title %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbTitle" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Title %>' Width="150"></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="tbNewTitle" runat="server" Width="150"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Description" HeaderStyle-Width="300"> 
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item grid_left" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate> 
                    <asp:Label ID="lblDescription" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Description %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).Description %>' CssClass="admin-multitextbox"></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="tbNewDescription" runat="server" TextMode="MultiLine"  CssClass="admin-multitextbox"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>



            <asp:TemplateField HeaderText="MetaDescription" HeaderStyle-Width="200">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate> 
                    <asp:Label ID="lblMetaDesc" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).MetaDesc %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbMetaDesc" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).MetaDesc %>' Width="200"></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="tbNewMetaDesc" runat="server" Width="200"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>

            <asp:TemplateField HeaderText="MetaKeywords" HeaderStyle-Width="200">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate> 
                    <asp:Label ID="lblMetaKeywords" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).MetaKeywords %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbMetaKeywords" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).MetaKeywords %>' Width="200"></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="tbNewMetaKeywords" runat="server" Width="200"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>

            <asp:TemplateField HeaderText="UrlTag" HeaderStyle-Width="150">
                <HeaderStyle CssClass="grid_header" />
                <ItemStyle CssClass="grid_item" />
                <FooterStyle CssClass="grid_footer" />

                <ItemTemplate> 
                    <asp:Label ID="lblUrlTag" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).UrlTag %>'></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate> 
                    <asp:TextBox ID="tbUrlTag" runat="server" Text='<%# ((Collective.CategoryTranslation)Container.DataItem).UrlTag %>' Width="150"></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="tbNewUrlTag" runat="server" Width="150"></asp:TextBox> 
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
    <asp:HiddenField runat="server" ID="hfSavedMetaDesc" />
    <asp:HiddenField runat="server" ID="hfSavedMetaKeywords" />
    <asp:HiddenField runat="server" ID="hfSavedUrlTag" />

<div class="err_msg">
    <asp:Label runat="server" ID="lblErrMsg" ForeColor="Red"></asp:Label>
</div>

</ContentTemplate>
</asp:UpdatePanel>

</div>

</div>


</asp:Content>
