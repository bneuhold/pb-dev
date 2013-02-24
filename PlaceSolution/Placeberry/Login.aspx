<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link type="text/css" media="all" rel="Stylesheet" href="/resources/css/login.css" />
    
    <script language="javascript" type="text/javascript">
        $(function () {
            $(".button").button();

        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:MultiView ID="mvwLogin" ActiveViewIndex="0" runat="server">
        <asp:View ID="vwLogin" runat="server">

            <div class="login-wrapper">
                <fieldset class="editor ui-corner-all">
                <legend>Login</legend>

                <div class="login-container">
                    <div class="form-editor">
                        <div class="form-row">
                            <div class="form-cell label">Korisničko ime</div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxUserName" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="tbxUserName" ValidationGroup="login" runat="server" />
                            </div>
                            <div class="close-row"></div>
                        </div>

                        <div class="form-row">
                            <div class="form-cell label">Šifra</div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxPassword" TextMode="Password" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="tbxPassword" ValidationGroup="login" runat="server" />
                            </div>
                            <div class="close-row"></div>
                        </div>

                        <div class="form-row">
                            <div class="form-cell label"></div>
                            <div class="form-cell value">
                                <asp:CheckBox ID="chbxRememberMe" Text="Zapamti me" runat="server" Checked="false" />
                            </div>
                            <div class="close-row"></div>
                        </div>

                        <div class="form-row">
                            <div class="form-cell label"></div>
                            <div class="form-cell value"><asp:Button ID="btnLogin" Text="Prijavi me" runat="server" onclick="btnLogin_Click" ValidationGroup="login" CssClass="button login" /></div>
                            <div class="close-row"></div>
                        </div>
                
                    </div>
                </div>

               
            </fieldset>
            <div class="register-wrapper">
                Niste registrirani? Registrirajte se <a href="/Registration.aspx">ovdje</a>
            </div>
            <div class="message">
                <asp:Literal ID="ltlLoginMessage" Text="" runat="server" Visible="false" />
            </div>
        </div>
        </asp:View>
        <asp:View ID="vwActivateAccount" runat="server">

            <asp:Literal ID="ltlSucess" runat="server" Visible="false">
                <div class="message ok">
                    <p>
                        Uspješna aktivacija.
                    </p>
                    <p>
                        Bit ćete automatski preusmjereni na korisničku stranicu za 3 sekunde.
                    </p>
                </div>
            </asp:Literal>
            <asp:Literal ID="ltlError" runat="server" Visible="false">
                <div class="message error">
                    <p>
                        Aktivacijski link ne valja
                    </p>
                </div>
            </asp:Literal>

        </asp:View>
        <asp:View ID="vwResetPassword" runat="server">
            
            Ovdje će ići formular za oporavak lozinke

            korisničo ime, captcha i submit gumb... pa se na email pošalje link za password reset

            link vodi opet ovjde gdje korisnik upiše novu šifru.

        </asp:View>
    </asp:MultiView>




</asp:Content>

