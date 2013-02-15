<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:MultiView ID="mvwLogin" ActiveViewIndex="0" runat="server">
        <asp:View ID="vwLogin" runat="server">

            <h1>Login</h1>

            <h2>Korisničko ime</h2>
            <asp:TextBox ID="tbxUserName" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="tbxUserName" ValidationGroup="login" runat="server" />
    
            <h2>Šifra</h2>
            <asp:TextBox ID="tbxPassword" TextMode="Password" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="tbxPassword" ValidationGroup="login" runat="server" />
    
            <br />
            <asp:CheckBox ID="chbxRememberMe" Text="Zapamti me" runat="server" Checked="false" />

            <br /><br />
            <asp:Button ID="btnLogin" Text="Prijavi me" runat="server" onclick="btnLogin_Click" ValidationGroup="login" />
    
            <br />


            <p>
                Niste registrirani? Registrirajte se <a href="/Registration.aspx">ovdje</a>
            </p>

            <br />
            <asp:Literal ID="ltlLoginMessage" Text="" runat="server" Visible="false" />

        </asp:View>
        <asp:View ID="vwActivateAccount" runat="server">

            <asp:Literal ID="ltlSucess" runat="server" Visible="false">
                <p>
                    Uspješna aktivacija.
                </p>
                <p>
                    Bit ćete automatski preusmjereni na korisničku stranicu za 3 sekunde.
                </p>
            </asp:Literal>
            <asp:Literal ID="ltlError" runat="server" Visible="false">
                <p>
                    Aktivacijski link ne valja
                </p>
            </asp:Literal>

        </asp:View>
        <asp:View ID="vwResetPassword" runat="server">
            
            Ovdje će ići formular za oporavak lozinke

            korisničo ime, captcha i submit gumb... pa se na email pošalje link za password reset

            link vodi opet ovjde gdje korisnik upiše novu šifru.

        </asp:View>
    </asp:MultiView>




</asp:Content>

