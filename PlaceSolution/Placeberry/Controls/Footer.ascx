<%@ Control Language="C#" ClassName="Footer" %>
<%@ Import Namespace="System.Linq" %>

<script runat="server">

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
                                            }).Take(12);
            repPopularQueries.DataBind();
        }
    }
}

</script>

<div id="footer">
    <div class="keywords">
            <asp:Repeater ID="repPopularQueries" runat="server">
                <ItemTemplate>
                    <a href='<%# Eval("Link") %>'><%# Eval("Title") %></a>
                </ItemTemplate>
                <SeparatorTemplate>
                    &nbsp;|&nbsp;
                </SeparatorTemplate>
            </asp:Repeater>
    </div>
    <p class="credits">
        <br />
        <asp:Literal runat="server" ID="litCopy" Text="<%$ Resources:placeberry, Info_Copyright %>"></asp:Literal>
        | <a href="http://www.mri.hr" target="_blank">
            <asp:Literal runat="server" ID="litCompany" Text="<%$ Resources:placeberry, Info_CompanyName %>"></asp:Literal></a></p>
</div>
