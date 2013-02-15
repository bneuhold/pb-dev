<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormRegistrationCtrl.ascx.cs" Inherits="Controls_FormRegistrationCtrl" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<asp:PlaceHolder runat="server" ID="phRegisterForm">

    <table style="width:100%; margin-bottom:15px;">

    <tr>
        <td colspan="3" style="text-align:center;padding-bottom:20px;"><h1>Registracija</h1></td>
    </tr>
    <tr>
        <td colspan="3" style="text-align:center; color:Red;"><asp:Literal ID="ltlStatusErrorMessage" Text="" runat="server" /></td>
    </tr>            
    <tr>
        <td>E-mail</td>
        <td><asp:TextBox ID="tbEmail" runat="server" /></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="*" ControlToValidate="tbEmail" runat="server" ValidationGroup="register" /></td>
    </tr>
    <tr><td colspan="3" style="text-align:center;"><asp:RegularExpressionValidator ID="RegularExpressionValidator1" ForeColor="Red" ControlToValidate="tbEmail" runat="server" ErrorMessage="Neispravna e-mail adresa" ValidationExpression="[\w_.\-+]{4,}@[\w_\-+]{2,}([\w_\-+]+.?)*\.[\w]{2,3}" ValidationGroup="register" /></td></tr>
    <tr>
        <td>Lozinka</td>
        <td><asp:TextBox ID="tbPassword" TextMode="Password" runat="server" /></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="tbPassword" runat="server" ValidationGroup="register" /></td>
    </tr>
    <tr>
        <td>Potvrdi lozinku</td>
        <td>
            <asp:TextBox ID="tbPasswordConfirm" TextMode="Password"  runat="server" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="*" ControlToValidate="tbPasswordConfirm" runat="server" ValidationGroup="register" />
        </td>
    </tr>
    <tr>
        <td colspan="3" style="text-align:center;"><asp:CompareValidator ID="CompareValidator1" ForeColor="Red" ErrorMessage="Lozinke se ne podudaraju" ControlToValidate="tbPasswordConfirm" ControlToCompare="tbPassword" runat="server" /></td>
    </tr>
    <tr>
        <td>Ime:</td>
        <td><asp:TextBox ID="tbFirstName" runat="server" /></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="tbFirstName" runat="server" ValidationGroup="register" /></td>
    </tr>
    <tr>
        <td>Prezime:</td>
        <td><asp:TextBox ID="tbLastName" runat="server" /></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="*" ControlToValidate="tbLastName" runat="server" ValidationGroup="register" /></td>
    </tr>
    <tr>
        <td>Telefon:</td>
        <td><asp:TextBox ID="tbPhone" runat="server" /></td>
        <td></td>
    </tr>
    <tr>
        <td>Zemlja:</td>
        <td><asp:TextBox ID="tbCountry" runat="server" /></td>
        <td></td>
    </tr>
    <tr>
        <td>Grad:</td>
        <td><asp:TextBox ID="tbCity" runat="server" /></td>
        <td></td>
    </tr>
    <tr>
        <td>Poštanski broj:</td>
        <td><asp:TextBox ID="tbZipCode" runat="server" /></td>
        <td></td>
    </tr>
    <tr>
        <td>Adresa</td>
        <td><asp:TextBox ID="tbStreet" runat="server" /></td>
        <td></td>
    </tr>
    <tr>
        <td colspan="3" style="padding-top:20px; padding-bottom:0; text-align:center;">
        <asp:PlaceHolder runat="server" ID="phCaptcha">
            <table>
                <tr>
                    <td><cc1:CaptchaControl ID="ccJoin" runat="server" CaptchaBackgroundNoise="none" CaptchaLength="5" CaptchaHeight="60" CaptchaWidth="220" CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" /></td>
                    <td>
                        <table>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="padding-bottom:0;"><asp:TextBox Width="124" ID="tbCaptcha" runat="server" Text="" /></td>
                            </tr>
                            <tr>
                                <td style="text-align:center; color:Red;"><asp:Literal ID="ltlCaptchaError" Text="Krivi unos" runat="server" Visible="false" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="text-align:center;"><asp:LinkButton class="buy-next-button-bigest" style="color:White; " ID="lbSubmit" Text="Registriraj se" runat="server" onclick="lbSubmit_Click"  /></td>
    </tr>

    </table>

    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="phSucessMsg" Visible="false">
                
        <p style="margin-bottom:50px;">Uspješno ste se registirali, na email vam trebaju stići podaci za registraciju.</p>

        <a style="font-size:14px;" href="<%= Page.ResolveUrl("~/default.aspx") %>">Povratak na naslovnicu</a>

    </asp:PlaceHolder>



