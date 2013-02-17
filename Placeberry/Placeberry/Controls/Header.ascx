<%@ Control Language="C#" ClassName="Header" %>

<%@ Register src="LoginCtrl.ascx" tagname="LoginCtrl" tagprefix="ctrl" %>



<script type="text/javascript">
    $(document).ready(function () {
        $('#<%= langsSelected.ClientID %>').hover(
            function () { $("#langsMenu").show(); },
            function () { $("#langsMenu").hide(); }
        );
    });
</script>
<script runat="server">
    private const string ImageBasePath = "~/resources/images/";

    protected void Page_Load(object sender, EventArgs e)
    {
        SelectLanguage();
    }

    private void SelectLanguage()
    {
        string lang = Common.GetLanguage();

        switch (lang)
        {
            case "hr":
                SetLanguage("hr", "Hrvatski", ImageBasePath + "hr.png");
                break;
            case "en":
                SetLanguage("en", "English", ImageBasePath + "gb.png");
                break;
            case "it":
                SetLanguage("it", "Italiano", ImageBasePath + "it.png");
                break;
            case "de":
                SetLanguage("de", "Deutsch", ImageBasePath + "de.png");
                break;
            case "cz":
                SetLanguage("cz", "Český", ImageBasePath + "cz.png");
                break;
            default:
                SetLanguage("hr", "Hrvatski", ImageBasePath + "hr.png");
                break;
        }
    }

    private void SetLanguage(string abbrLang, string fullName, string imagePath)
    {
        this.lnkLang.HRef = "/Default.aspx?lang=" + abbrLang;
        this.litLang.Text = fullName;
        this.imgLang.Alt = fullName;
        this.imgLang.Src = imagePath;

        Common.SetLanguage(abbrLang);
    }
</script>

<div id="header">
    <a href="/">
        <img class="logo" id="imgLogo" runat="server" src="~/resources/images/logo_placeberry_white.png" alt="<%$ Resources:placeberry, General_Slogan %>" />
        <span class="slogan"><asp:Literal runat="server" ID="litSlogan" 
        Text="<%$ Resources:placeberry, General_Slogan %>"></asp:Literal></span></a>

    <div style="display:block; position:absolute; width:100%; text-align:right;">
        <div style="display:inline-block; margin:3px 12px 0 0; font-size:12px; color:">
           <ctrl:LoginCtrl runat="server" ID="ctrlLogin" />
        </div>
    </div>

    <ul class="nav">
        <li><a href="<%$ Resources:placeberry, URL_Home %>" runat="server"><asp:Literal runat="server" ID="litHome" 
                Text="<%$ Resources:placeberry, Menu_Home %>"></asp:Literal></a></li>
        <li><a href="<%$ Resources:placeberry, URL_PopularQueries %>" runat="server" id="lnkPopular"><asp:Literal runat="server" ID="litPopular" 
                Text="<%$ Resources:placeberry, Menu_PopularQueries %>"></asp:Literal></a></li>
        <li><a href="<%$ Resources:placeberry, URL_Contact %>" runat="server" id="lnkConact"><asp:Literal runat="server" ID="litContact" 
                Text="<%$ Resources:placeberry, Menu_Contact %>"></asp:Literal></a></li>
        <li><a href="<%$ Resources:placeberry, URL_About %>" runat="server" id="lnkAbout"><asp:Literal runat="server" ID="litAbout" 
                Text="<%$ Resources:placeberry, Menu_About %>"></asp:Literal></a></li>
    </ul>
    <ul class="langs">
        <li id="langsSelected" runat="server"><a href="/Default.aspx?lang=hr" runat="server" id="lnkLang">
            <span>
                <asp:Literal runat="server" ID="litLang" Text="<%$ Resources:placeberry, Menu_Language %>"></asp:Literal></span><img src="~/resources/images/hr.png"
                    runat="server" id="imgLang" alt="<%$ Resources:placeberry, Menu_Language %>" /></a>
            <ul id="langsMenu" style="display: none">
                <li><a href="/Default.aspx?lang=hr"><span>Hrvatski</span><img src="~/resources/images/hr.png" alt="Hrvatski" runat="server" /></a></li>
                <li><a href="/Default.aspx?lang=en"><span>English</span><img src="~/resources/images/gb.png" alt="English" runat="server" /></a></li>
                <li><a href="/Default.aspx?lang=it"><span>Italiano</span><img src="~/resources/images/it.png" alt="Italiano" runat="server" /></a></li>
                <li><a href="/Default.aspx?lang=de"><span>Deutsch</span><img src="~/resources/images/de.png" alt="Deutsch" runat="server" /></a></li> 
                <li><a href="/Default.aspx?lang=cz"><span>Český</span><img src="~/resources/images/cz.png" alt="Český" runat="server" /></a></li>
            </ul>
        </li>
    </ul>
</div>
