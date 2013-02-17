<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="BookingClient.aspx.cs" Inherits="BookingClient" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" />

<div style="margin-left:50px;">

    <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
        <asp:Label runat="server" ID="lblAgencyName"></asp:Label>
    </div>

    <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
        <asp:Label runat="server" ID="lblAccommName"></asp:Label>
    </div>


   <asp:UpdatePanel runat="server" id="UpdatePanel" updatemode="Conditional">

        <Triggers>
            <asp:AsyncPostBackTrigger controlid="btnNext" eventname="Click" />
        </Triggers>

        <ContentTemplate>

        <!-- DATE SELECT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepDateSelect">

            <div style="clear:both; float:left; display:inline; padding-right:30px; margin:10px 0 0 0;">
                <asp:Calendar ID="calFrom" runat="server"></asp:Calendar>
            </div>

            <div style="float:left; display:inline; border-left:20px; margin:10px 0 0 0;">
                <asp:Calendar ID="calTo" runat="server"></asp:Calendar>
            </div>

            <div style="clear:both; float:left; display:inline; margin:40px 0 0 0;">
                Broj osoba: <asp:DropDownList runat="server" ID="ddlNumOfPersons" AutoPostBack="true"></asp:DropDownList>
            </div>

            <div style="clear:both; float:left; display:inline; margin:20px 0 0 0;">
                Broj djece malđe od 3 godine: <asp:DropDownList runat="server" ID="ddlNumOfBabies"></asp:DropDownList>
            </div>

            <div style="clear:both; float:left; display:inline; margin:20px 0 50px 0;">
                <asp:Repeater runat="server" ID="rptDaysWithPrices">
                    <HeaderTemplate>
                    <table>
                    <tr>
                    <td colspan="2">
                    Cijene po danu:
                    </td>
                    </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                        <td>Dan: <%# ((DateTime)Eval("Key")).ToShortDateString() %></td><td>, Cijena: <%# Eval("Value") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

        </asp:PlaceHolder>

        <!-- INFO INPUT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepInfoInput" Visible="false">

            <div style="clear:both; float:left; display:inline; margin:30px 0 0 0;">
                Ime: <asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqFirstName" controltovalidate="tbFirstName" errormessage="*" />
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Prezime: <asp:TextBox runat="server" ID="tbLastName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqLastName" controltovalidate="tbLastName" errormessage="*" />
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Email: <asp:TextBox runat="server" ID="tbEmail"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqEmail" controltovalidate="tbEmail" errormessage="*" />
                <asp:RegularExpressionValidator ID="regexEmail" runat="server"     
                                        ErrorMessage="Neispravan format Email adrese!" 
                                        ControlToValidate="tbEmail"     
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Telefon: <asp:TextBox runat="server" ID="tbPhone"></asp:TextBox>
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Zemlja: <asp:TextBox runat="server" ID="tbCountry"></asp:TextBox>
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Grad: <asp:TextBox runat="server" ID="tbCity"></asp:TextBox>
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Adresa: <asp:TextBox runat="server" ID="tbStreet"></asp:TextBox>
            </div>

        </asp:PlaceHolder>

        <!-- PAYMENT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepPayment" Visible="false">
            <div style="clear:both; float:left; display:inline; margin:10px 0 50px 0;">
                Odabir nacina placanja:
            </div>
            <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                Promo Code:&nbsp;<asp:TextBox runat="server" ID="tbPromoCode"></asp:TextBox>&nbsp;<asp:Button runat="server" ID="btnAddPromoCode" Text="Dodaj Promo Code"></asp:Button>
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 10px 0;">
                <asp:Label runat="server" ID="lblPromoCodeErrorMsg"></asp:Label>
            </div>
        </asp:PlaceHolder>

        <!-- BOOKING COMPLETE STEP -->
        <asp:PlaceHolder runat="server" ID="phStepComplete" Visible="false">
            <div style="clear:both; float:left; display:inline; margin:10px 0 50px 0;">
                Booking uspjesno izvrsen
            </div>
        </asp:PlaceHolder>


        <asp:PlaceHolder runat="server" ID="phBottom">
            <div style="clear:both; float:left; display:inline; margin:30px 0 0 400px;">
                <asp:Button runat="server" id="btnNext" text="Next" />
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 0px 0x;">
                Ukupna cijena: <asp:Label runat="server" ID="lblPriceSum"></asp:Label>
            </div>

            <div style="clear:both; float:left; display:inline; margin:20px 0 50px 0;">
                <asp:Label runat="server" ID="lblDateMsg"></asp:Label>
            </div>

            <div style="clear:both; float:left; display:inline; margin:10px 0 10px 0;">
                <asp:Label runat="server" ID="lblErrorMsg"></asp:Label>
            </div>

        </asp:PlaceHolder>

        </ContentTemplate>

    </asp:UpdatePanel>

</div>

</asp:Content>