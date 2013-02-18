<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true"
    CodeFile="SubmitRequest.aspx.cs" Inherits="SubmitRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" class="clearfix">
        <div id="results_wrap" class="clearfix">
            <div class="page">
                <div class="page_container">
                    <h1>
                        <asp:Literal runat="server" ID="litMainHeader" Text="Prijavite smještaj/ponudu"></asp:Literal></h1>
                    <h2>
                        <asp:Literal runat="server" ID="litFormHeader" meta:resourcekey="litFormHeaderResource1"
                            Text="Kontakt forma"></asp:Literal></h2>
                    <asp:ScriptManager runat="server" ID="ScriptManager1">
                    </asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="mainPnl">
                        <ContentTemplate>
                            <fieldset runat="server" id="contactForm">
                                <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="Form" HeaderText="&lt;b&gt;Nisu ispunjeni svi potrebni podaci:&lt;/b&gt;&lt;br/&gt;&lt;br/&gt;"
                                    Style="padding-bottom: 10px" />
                                <label>
                                    <asp:Literal runat="server" ID="litURL" Text="URL"></asp:Literal>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtURL"
                                        ValidationGroup="Form" CssClass="validators" Display="Dynamic" SetFocusOnError="True"
                                        Text="*" ErrorMessage="Molimo unesite URL."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator2" ControlToValidate="txtURL" runat="server"
                                            ValidationGroup="Form" ErrorMessage="URL nije u dobrom formatu." Display="Dynamic"
                                            SetFocusOnError="True" Text="*" ValidationExpression="(http(s)?://)*([\w-]+\.)+[\w-]+([\w\-\.,@?^=%&amp;:/~\+#\(\)]*[\w\-\@?^=%&amp;/~\+#\(\)])"
                                            ></asp:RegularExpressionValidator>
                                </label>
                                <asp:TextBox ID="txtURL" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litName" Text="Kontakt osoba"></asp:Literal>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                        ValidationGroup="Form" CssClass="validators" Display="Dynamic" SetFocusOnError="True"
                                        Text="*" ErrorMessage="Molimo unesite ime i prezime osobe za kontakt."></asp:RequiredFieldValidator></label>

                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litPhone" Text="Kontakt telefon"></asp:Literal></label>
                                <asp:TextBox ID="txtPhone" runat="server" ></asp:TextBox>
                                <label>
                                    <asp:Literal runat="server" ID="litEmail" Text="Kontakt e-mail"></asp:Literal>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="Form" CssClass="validators" Display="Dynamic" SetFocusOnError="True"
                                        Text="*" ErrorMessage="Molimo unesite svoju e-mail adresu." ></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" ControlToValidate="txtEmail" runat="server"
                                            ValidationGroup="Form" ErrorMessage="E-mail adresa nije u dobrom formatu." Display="Dynamic"
                                            SetFocusOnError="True" Text="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ></asp:RegularExpressionValidator></label>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                <asp:Button runat="server" ID="btnSendMessage" CssClass="wpcf7-submit" Text="Pošalji"
                                    OnClientClick="Page_ClientValidate(); if (Page_IsValid) { $('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_btnSendMessage').hide(); }"
                                    ValidationGroup="Form" OnClick="btnSendMessage_Click"  />
                            </fieldset>
                            <fieldset runat="server" id="successForm" visible="False">
                                <label style="width: 550px">
                                    <asp:Literal runat="server" ID="litMsg" Text="Zahvaljujemo na prijavi ponude/smještaja. Upit ćemo obraditi u najkraćem roku."></asp:Literal></label>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
