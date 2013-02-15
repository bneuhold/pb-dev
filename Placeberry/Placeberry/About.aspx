<%@ Page Language="C#" MasterPageFile="~/MasterHome.master" Title="" Culture="auto"
    UICulture="auto" %>

<%@ Register Src="/Controls/Sidebar.ascx" TagName="Sidebar" TagPrefix="uc1" %>
<%@ Import Namespace="System.Globalization" %>
<script runat="server">
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = Resources.placeberry.About_Title;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="description" content="<%$ Resources:placeberry, About_MetaDescription %>"
        runat="server" id="metaDescription" />
    <meta name="keywords" content="<%$ Resources:placeberry, About_MetaKeywords %>" runat="server"
        id="metaKeywords" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" class="clearfix">
        <div id="results_wrap" class="clearfix">
            <div class="page">
                <div class="page_container">
                    <h1>
                        <asp:Literal runat="server" ID="litMainHeader" Text="<%$ Resources:placeberry, About_MainHeader %>"></asp:Literal></h1>
                    <h2>
                        <asp:Literal runat="server" ID="litWhatHeader" Text="<%$ Resources:placeberry, About_SubHeader1 %>"></asp:Literal></h2>
                    <p>
                        <asp:Literal runat="server" ID="litWhatText" Text="<%$ Resources:placeberry, About_Paragraph1 %>"></asp:Literal>
                    </p>
                    <h2>
                        <asp:Literal runat="server" ID="litWhyHeader" Text="<%$ Resources:placeberry, About_SubHeader2 %>"></asp:Literal></h2>
                    <p>
                        <asp:Literal runat="server" ID="litWhyText" Text="<%$ Resources:placeberry, About_Paragraph2 %>"></asp:Literal>
                    </p>
                    <h2>
                        <asp:Literal runat="server" ID="litAdsHeader" Text="<%$ Resources:placeberry, About_SubHeader3 %>"></asp:Literal></h2>
                    <p>
                        <asp:Literal runat="server" ID="litAdsText" Text="<%$ Resources:placeberry, About_Paragraph3 %>"></asp:Literal>
                    </p>
                    <h2>
                        <asp:Literal runat="server" ID="litContactHeader" Text="<%$ Resources:placeberry, About_SubHeader4 %>"></asp:Literal></h2>
                    <p>
                        <asp:Literal runat="server" ID="litContactText" Text="<%$ Resources:placeberry, About_Paragraph4 %>"></asp:Literal>
                    </p>
                </div>
            </div>
            <uc1:Sidebar runat="server" ID="Sidebar"></uc1:Sidebar>
        </div>
    </div>
</asp:Content>
