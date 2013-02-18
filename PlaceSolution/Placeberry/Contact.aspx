<%@ Page Language="C#" MasterPageFile="~/MasterHome.master" Culture="auto" UICulture="auto" %>

<%@ Register Src="/Controls/Sidebar.ascx" TagName="Sidebar" TagPrefix="uc1" %>
<%@ Import Namespace="System.Globalization" %>
<script runat="server">
    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = Resources.placeberry.Contact_Title;
    }

    protected void btnSendMessage_Click(object sender, EventArgs e)
    {
        try
        {
            string body = "Datum upita: " + DateTime.Now.ToString();
            body += "<br />Ime i prezime: " + txtName.Text;
            body += "<br />Tvrtka: " + txtCompany.Text;
            body += "<br />Broj telefona: " + txtPhone.Text;
            body += "<br />E-mail: " + txtEmail.Text;
            body += "<br />Web stranica: " + txtWeb.Text;
            body += "<br />Poruka:<br/>" + txtComment.Text;

            //Emailing.SendEmailThroughGmail(ConfigurationManager.AppSettings["ContactEmail"].ToString(), ConfigurationManager.AppSettings["ContactPassword"].ToString(), ConfigurationManager.AppSettings["ContactEmail"].ToString(), "Placeberry contact form", body);
            Emailing.SendEmail(txtEmail.Text.Trim(), ConfigurationManager.AppSettings["ContactEmail"].ToString(), "Placeberry contact form", body);

            txtComment.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtWeb.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtCompany.Text = string.Empty;
            txtName.Text = string.Empty;

            contactForm.Visible = false;
            successForm.Visible = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="description" content="<%$ Resources:placeberry, Contact_MetaDescription %>"
        runat="server" id="metaDescription" />
    <meta name="keywords" content="<%$ Resources:placeberry, Contact_MetaKeywords %>"
        runat="server" id="metaKeywords" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" class="clearfix">
        <div id="results_wrap" class="clearfix">
            <div class="page">
                <div class="page_container">
                    <h1>
                        <asp:Literal runat="server" ID="litMainHeader" Text="<%$ Resources:placeberry, Contact_MainHeader %>"></asp:Literal></h1>
                    <h2>
                        <asp:Literal runat="server" ID="litInfoHeader" Text="<%$ Resources:placeberry, Contact_SubHeader1 %>"></asp:Literal></h2>
                    <p>
                        <asp:Literal runat="server" ID="litInfoText" Text="<%$ Resources:placeberry, Contact_Paragraph1 %>"></asp:Literal></p>
                    <p>
                        &nbsp;</p>
                    <h2>
                        <asp:Literal runat="server" ID="litFormHeader" Text="<%$ Resources:placeberry, Contact_FormHeader %>"></asp:Literal></h2>
                    <asp:ScriptManager runat="server" ID="ScriptManager1">
                    </asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="mainPnl">
                        <ContentTemplate>
                            <fieldset runat="server" id="contactForm">
                                <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="Form" HeaderText="<%$ Resources:placeberry, Contact_ValidationHeader %>"
                                    Style="padding-bottom: 10px" />
                                <label>
                                    <asp:Literal runat="server" ID="litName" Text="<%$ Resources:placeberry, Contact_Name %>"></asp:Literal>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                        ValidationGroup="Form" CssClass="validators" Display="Dynamic" SetFocusOnError="True"
                                        Text="*" ErrorMessage="<%$ Resources:placeberry, Contact_NameRequired %>"></asp:RequiredFieldValidator></label>
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litCompany" Text="<%$ Resources:placeberry, Contact_Company %>"></asp:Literal></label><asp:TextBox
                                        ID="txtCompany" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litPhone" Text="<%$ Resources:placeberry, Contact_PhoneNumber %>"></asp:Literal></label><asp:TextBox
                                        ID="txtPhone" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litEmail" Text="<%$ Resources:placeberry, Contact_Email %>"></asp:Literal>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="Form" CssClass="validators" Display="Dynamic" SetFocusOnError="True"
                                        Text="*" ErrorMessage="<%$ Resources:placeberry, Contact_EmailRequired %>"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" ControlToValidate="txtEmail" runat="server"
                                            ValidationGroup="Form" ErrorMessage="<%$ Resources:placeberry, Contact_EmailFormat %>"
                                            Display="Dynamic" SetFocusOnError="True" Text="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></label><asp:TextBox
                                                ID="txtEmail" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litweb" Text="<%$ Resources:placeberry, Contact_WebSite %>"></asp:Literal></label><asp:TextBox
                                        ID="txtWeb" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litComment" Text="<%$ Resources:placeberry, Contact_Comment %>"></asp:Literal></label><asp:TextBox
                                        ID="txtComment" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
                                <asp:Button runat="server" ID="btnSendMessage" CssClass="wpcf7-submit" Text="<%$ Resources:placeberry, Contact_Submit %>"
                                    OnClientClick="Page_ClientValidate(); if (Page_IsValid) { $('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_btnSendMessage').hide(); }"
                                    ValidationGroup="Form" OnClick="btnSendMessage_Click" />
                            </fieldset>
                            <fieldset runat="server" id="successForm" visible="False">
                                <label style="width: 550px">
                                    <asp:Literal runat="server" ID="litMsg" Text="<%$ Resources:placeberry, Contact_ThankYou %>"></asp:Literal></label>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <uc1:Sidebar runat="server" ID="Sidebar"></uc1:Sidebar>
        </div>
    </div>
</asp:Content>
