<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="userAdmin_Default" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="/Controls/UserAdminSidebar.ascx" TagName="UserAdminSidebar" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

    $.fn.showPanel = function () { return this.css({ display: "block" }) };
    $.fn.hidePanel = function () { return this.css({ display: "none" }) };

    function showChangePass() {

        var chPassCont = $("#<%= divChangePass.ClientID %>");
        if (chPassCont.css("display") == "block") {
            chPassCont.css({ display: "none" });
        }
        else {
            chPassCont.css({ display: "block" });
        }
    }

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <uc1:UserAdminSidebar runat="server" ID="ctrlUserAdminSidebar" />

    <div class="user-profile">    

    <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>

            <div class="inner">

            <asp:HiddenField runat="server" ID="hfIsUpdate" Value="false" />
            
                <table>
                    <tr>
                        <td>Ime:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblFirstName" /><asp:TextBox runat="server" ID="tbFirstName" /></td>
                    </tr>
                    <tr>
                        <td>Prezime:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblLastName" /><asp:TextBox runat="server" ID="tbLastName" /></td>
                    </tr>
                    <tr>
                        <td>Telefon:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblPhone" /><asp:TextBox runat="server" ID="tbPhone" /></td>
                    </tr>
                    <tr>
                        <td>Zemlja:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblCountry" /><asp:TextBox runat="server" ID="tbCountry" /></td>
                    </tr>
                    <tr>
                        <td>Grad:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblCity" /><asp:TextBox runat="server" ID="tbCity" /></td>
                    </tr>
                    <tr>
                        <td>Poštanski broj:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblZipCode" /><asp:TextBox runat="server" ID="tbZipCode" /></td>
                    </tr>
                    <tr>
                        <td>Adresa:</td><td style="min-width:230px;"><asp:Label runat="server" ID="lblStreet" /><asp:TextBox runat="server" ID="tbStreet" /></td>
                    </tr>
                    <tr>
                        <td style="text-align:left; padding-top:30px;">
                            <asp:LinkButton runat="server" ID="lbCancel" Text="Odustani" />
                        </td>
                        <td style="text-align:right; padding-top:30px;">
                            <asp:LinkButton runat="server" ID="lbSave" Text="Spremi" /><asp:LinkButton runat="server" ID="lbChange" Text="Izmjeni" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; padding-top:30px;"><a href="javascript:void(0);" onclick="showChangePass()">Izmjeni Lozinku</a></td><td></td>
                    </tr>
                </table>

            </div>

        </ContentTemplate>

    </asp:UpdatePanel>


    <asp:UpdatePanel id="UpdatePanel2" runat="server" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">

        <ContentTemplate>

        <asp:Label runat="server" ID="lblMsgChangePass"></asp:Label>

        <div style="display:block; width:100%; height:15px;"></div>

    <div runat="server" id="divChangePass" style="display:none; width:100%;">
        <div class="inner" style="margin-top:0px;">

                <table>                    
                    <tr>
                        <td>Stara lozinka:</td><td style="min-width:230px;"><asp:TextBox runat="server" ID="tbOldPass" TextMode="Password" />&nbsp;
                            <asp:RequiredFieldValidator ID="reqOldPass" ErrorMessage="*" ControlToValidate="tbOldPass" runat="server" ValidationGroup="changePass" /></td>
                    </tr>
                    <tr>
                        <td>Nova lozinka:</td><td style="min-width:230px;"><asp:TextBox runat="server" ID="tbNewPass" TextMode="Password" />&nbsp;
                            <asp:RequiredFieldValidator ID="reqNewPass" ErrorMessage="*" ControlToValidate="tbNewPass" runat="server" ValidationGroup="changePass" /></td>
                    </tr>
                    <tr>
                        <td>Ponovi lozinku:</td><td style="min-width:230px;"><asp:TextBox runat="server" ID="tbNewPassRep" TextMode="Password" />&nbsp;
                            <asp:RequiredFieldValidator ID="reqRepNewPass" ErrorMessage="*" ControlToValidate="tbNewPassRep" runat="server" ValidationGroup="changePass" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                        <asp:CompareValidator ID="compPass" ForeColor="Red" ErrorMessage="Lozinke se ne podudaraju" ControlToValidate="tbNewPassRep" ControlToCompare="tbNewPass" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="text-align:right; padding-top:10px;">
                            <asp:LinkButton runat="server" ID="lbSaveNewPass" Text="Spremi" />
                        </td>
                    </tr>
                </table>
                
        </div>
    </div>

    <div style="display:block; width:100%; height:20px;"></div>

        </ContentTemplate>

    </asp:UpdatePanel>


    </div>

</asp:Content>