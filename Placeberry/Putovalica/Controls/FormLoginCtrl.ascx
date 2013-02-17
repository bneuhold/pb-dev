<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormLoginCtrl.ascx.cs" Inherits="Controls_FormLoginCtrl" %>

<script type="text/javascript">

    function showRetPass() {

        var div = $("#divRetPass");
        if(div.css("display") == "block")
            div.css({ display: "none" });
        else
            div.css({ display: "block" });
    }

    function showSendNewActCode() {

        var div = $("#divSendNewActCode");
        if (div.css("display") == "block")
            div.css({ display: "none" });
        else
            div.css({ display: "block" });
    }

    function sendNewPassword(ancor) {
        $(ancor).css({ display: "none" });
        var msg = $("#<%= lblRetPassMsg.ClientID %>");
        msg.css({ color: "#666666" });
        msg.text("Slanje u tijeku...");
   }

   function sendNewActCode(ancor) {
       $(ancor).css({ display: "none" });
       var msg = $("#<%= lblNewActCodeMsg.ClientID %>");
       msg.css({ color: "#666666" });
       msg.text("Slanje u tijeku...");
   }

</script>

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

<asp:MultiView ID="mvwLogin" ActiveViewIndex="0" runat="server">
    <asp:View ID="vwLogin" runat="server">

        <table style="width:100%; margin-bottom:15px;">
            <tr>
                <td colspan="3" style="text-align:center;padding-bottom:54px;"><h1>Prijava</h1></td>
            </tr>
            <tr>
                <td>Korisničko ime:</td>
                <td>
                    <asp:TextBox ID="tbxUserName" runat="server" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="tbxUserName" ValidationGroup="login" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Šifra:</td>
                <td>
                    <asp:TextBox ID="tbxPassword" TextMode="Password" runat="server" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="tbxPassword" ValidationGroup="login" runat="server" />
                </td>
            </tr>
            <tr>
                <td></td><td style="text-align:right;">Zapamti me <asp:CheckBox style="margin-right:20px;" ID="chbxRememberMe" runat="server" Checked="false" /></td>
            </tr>
            <tr>
                <td colspan="3" style="text-align:center;"><asp:Label ID="lblLoginMessage" Text="" runat="server" ForeColor="Red" Visible="false"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3" style="text-align:center; height:50px;"><asp:LinkButton class="buy-next-button-big" ID="lbLogin" Text="Prijavi se" runat="server" onclick="lbLogin_Click" ValidationGroup="login" /></td>
            </tr>
        </table>

        <p><a href="javascript:void(0);" onclick="showRetPass();" class="link_style" style="padding-top:10px;">Zaboravili ste lozinku?</a></p>

        <div id="divRetPass" style="display:none;">

            <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>

                <ul style="padding-top:15px;">
                    <li style="padding-bottom:10px;"><span style="font-size:13px;">Unesite email kako bismo vam poslali novu lozinku</span></li>
                    <li style="padding-bottom:10px;">Email: <asp:TextBox runat="server" ID="tbRetPassEmail"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="lbRetPass" class="link_style" style="margin-left:5px;" OnClientClick="sendNewPassword(this)">Pošalji</asp:LinkButton></li>
                    <li>
                        <asp:Label runat="server" ID="lblRetPassMsg"></asp:Label>
                    </li>
                </ul>                    

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <br />

        <p><a href="javascript:void(0);" onclick="showSendNewActCode();" class="link_style" style="padding-top:10px;">Izgubili ste aktivacijski kod?</a></p>

        <div id="divSendNewActCode" style="display:none;">

            <asp:UpdatePanel id="updPannelSendNewActCode" runat="server" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>

                <ul style="padding-top:15px;">
                    <li style="padding-bottom:10px;"><span style="font-size:13px;">Unesite email kako bismo vam poslali novi aktivacijski kod.</span></li>
                    <li style="padding-bottom:10px;">Email: <asp:TextBox runat="server" ID="tbNewActCodeEmail"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="lbNewActCode" class="link_style" style="margin-left:5px;" OnClientClick="sendNewActCode(this)">Pošalji</asp:LinkButton></li>
                    <li>
                        <asp:Label runat="server" ID="lblNewActCodeMsg" ForeColor="Red"></asp:Label>                    
                    </li>
                </ul>                    

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>


    </asp:View>
    <asp:View ID="vwActivateAccount" runat="server">

        <asp:Literal ID="ltlSucess" runat="server" Visible="false">
            <p style="margin:30px 0 50px 0">
                Uspješna aktivacija!
            </p>
            <p style="font-size:14px;">
                Bit ćete automatski preusmjereni na korisničku stranicu za nekoliko trenutaka...
            </p>
        </asp:Literal>
        <asp:Literal ID="ltlError" runat="server" Visible="false">
            <p style="color:Red; margin-top:40px;">
                Pogrešan aktivacijaski link.
            </p>
        </asp:Literal>

    </asp:View>
    <asp:View ID="vwResetPassword" runat="server">
            
        Ovdje će ići formular za oporavak lozinke

        korisničo ime, captcha i submit gumb... pa se na email pošalje link za password reset

        link vodi opet ovjde gdje korisnik upiše novu šifru.

    </asp:View>
</asp:MultiView>


