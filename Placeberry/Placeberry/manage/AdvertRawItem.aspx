<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="AdvertRawItem.aspx.cs" Inherits="AdvertRawItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <asp:Literal ID="litNaslov" runat="server"></asp:Literal>
    <br />
    <br />
    <asp:Literal ID="litInfo" runat="server"></asp:Literal><br />
    <table>
        <tr>
            <td>
                Title:
            </td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="revTitle" runat="server" 
                ControlToValidate="txtTitle" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Source:
            </td>
            <td>
                <asp:TextBox ID="txtSource" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                GroupType:
            </td>
            <td>
                <asp:TextBox ID="txtGroupType" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="revGroupType" runat="server" 
                ControlToValidate="txtGroupType" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                GroupSubType:
            </td>
            <td>
                <asp:TextBox ID="txtGroupSubType" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="revGroupSubType" runat="server" 
                ControlToValidate="txtGroupSubType" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                SourceCategory:
            </td>
            <td>
                <asp:TextBox ID="txtSourceCategory" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                AccommType:
            </td>
            <td>
                <asp:TextBox ID="txtAccommType" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="revAccommType" runat="server" 
                ControlToValidate="txtAccommType" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                AccommSubType:
            </td>
            <td>
                <asp:TextBox ID="txtAccommSubType" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                VacationType:
            </td>
            <td>
                <asp:TextBox ID="txtVacationType" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                AdvertCode:
            </td>
            <td>
                <asp:TextBox ID="txtAdvertCode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                UrlLink:
            </td>
            <td>
                <asp:TextBox ID="txtUrlLink" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                PictureUrl:
            </td>
            <td>
                <asp:TextBox ID="txtPictureUrl" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="revPictureUrl" runat="server" 
                ControlToValidate="txtPictureUrl" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Stars:
            </td>
            <td>
                <asp:TextBox ID="txtStars" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                LocationDesc:
            </td>
            <td>
                <asp:TextBox ID="txtLocationDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Language:
            </td>
            <td>                        
                <asp:DropDownList ID="ddlLanguage" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="revLanguage" runat="server" 
                ControlToValidate="ddlLanguage" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
     
                <tr>
                    <td>
                        Country:
                    </td>
                    <td>
                        <asp:ComboBox ID="cbxCountry" runat="server" 
                        AutoCompleteMode="SuggestAppend" 
                        AutoPostBack="true" 
                        OnTextChanged="cbxCountry_TextChanged">
                        </asp:ComboBox>
                        <asp:RequiredFieldValidator ID="revCountry" runat="server" 
                        ControlToValidate="cbxCountry" 
                        ErrorMessage="*">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Region:
                    </td>
                    <td>
                        <asp:ComboBox ID="cbxRegion" runat="server" 
                        AutoCompleteMode="SuggestAppend" 
                        AutoPostBack="true" 
                        OnTextChanged="cbxRegion_TextChanged"></asp:ComboBox>
                        <asp:RequiredFieldValidator ID="revRegion" runat="server" 
                        ControlToValidate="cbxRegion" 
                        ErrorMessage="*">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Subregion:
                    </td>
                    <td>
                        <asp:ComboBox ID="cbxSubregion" runat="server" 
                        AutoCompleteMode="SuggestAppend" 
                        AutoPostBack="true" 
                        OnTextChanged="cbxSubregion_TextChanged">
                        </asp:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Island:
                    </td>
                    <td>
                        <asp:ComboBox ID="cbxIsland" runat="server" 
                        AutoCompleteMode="SuggestAppend" 
                        AutoPostBack="true" 
                        OnTextChanged="cbxIsland_TextChanged">
                        </asp:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        City:
                    </td>
                    <td>
                        <asp:ComboBox ID="cbxCity" runat="server" 
                        AutoCompleteMode="SuggestAppend" 
                        AutoPostBack="false" 
                        OnTextChanged="cbxCity_TextChanged">
                        </asp:ComboBox>
                        <asp:RequiredFieldValidator ID="revCity" runat="server" 
                        ControlToValidate="cbxCity" 
                        ErrorMessage="*">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>

        <tr>
            <td>
                PriceOld:
            </td>
            <td>
                <asp:TextBox ID="txtPriceOld" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                PriceFrom:
            </td>
            <td>
                <asp:TextBox ID="txtPriceFrom" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="revPriceFrom" runat="server" 
                ControlToValidate="txtPriceFrom" 
                ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                PriceDesc:
            </td>
            
            <td>
                <asp:TextBox ID="txtPriceDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Date1:
            </td>
            <td>
                <asp:TextBox ID="txtDate1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Date2:
            </td>
            <td>
                <asp:TextBox ID="txtDate2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                DateDesc:
            </td>
            <td>
                <asp:TextBox ID="txtDateDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                DaysNum:
            </td>
            <td>
                <asp:TextBox ID="txtDaysNum" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Description:
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                DistanceFromCentreM:
            </td>
            <td>
                <asp:TextBox ID="txtDistanceFromCentreM" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Activities:
            </td>
            <td>
                <asp:TextBox ID="txtActivities" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Facilities:
            </td>
            <td>
                <asp:TextBox ID="txtFacilities" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Beach:
            </td>
            <td>
                <asp:TextBox ID="txtBeach" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                BeachDistanceM:
            </td>
            <td>
                <asp:TextBox ID="txtBeachDistanceM" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                PetsDesc:
            </td>
            <td>
                <asp:TextBox ID="txtPetsDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                InfoDesc:
            </td>
            <td>
                <asp:TextBox ID="txtInfoDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Pohrani" OnClick="btnSave_Click" />
    <asp:Button ID="btnPovratak" runat="server" Text="Povratak" OnClick="btnPovratak_Click" />
</asp:Content>
