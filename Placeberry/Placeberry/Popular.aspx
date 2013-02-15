<%@ Page Language="C#" MasterPageFile="~/MasterHome.master" Culture="auto" UICulture="auto" %>

<%@ Register Src="/Controls/Sidebar.ascx" TagName="Sidebar" TagPrefix="uc1" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Linq" %>
<script runat="server">
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = Resources.placeberry.Popular_Title;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (UltimateDC.UltimateDataContext dc = new UltimateDC.UltimateDataContext())
            {
                repPopularQueries.DataSource = (from p in dc.PopularQueries
                                                where p.Active == true && p.LanguageId == Common.GetLanguageId()
                                                orderby p.Priority descending
                                                select new
                                                {
                                                    Title = p.Query,
                                                    Link = String.Format("/{0}?q={1}", Resources.placeberry.General_VacationUrl, HttpUtility.UrlEncode(p.Query))
                                                }).Take(30);
                repPopularQueries.DataBind();
            }
        }
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="description" content="<%$ Resources:placeberry, Popular_MetaDescription %>"
        runat="server" id="metaDescription" />
    <meta name="keywords" content="<%$ Resources:placeberry, Popular_MetaKeywords %>"
        runat="server" id="metaKeywords" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" class="clearfix">
        <div id="results_wrap" class="clearfix">
            <div class="page">
                <div class="page_container">
                    <h1>
                        <asp:Literal runat="server" ID="litMainHeader" Text="<%$ Resources:placeberry, Popular_MainHeader %>"></asp:Literal>
                    </h1>
                    <asp:Repeater ID="repPopularQueries" runat="server">
                        <HeaderTemplate>
                            <ul class="list clearfix">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li><a href='<%# Eval("Link") %>'>
                                <%# Eval("Title") %></a> </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <uc1:Sidebar runat="server" ID="Sidebar"></uc1:Sidebar>
        </div>
    </div>
</asp:Content>
