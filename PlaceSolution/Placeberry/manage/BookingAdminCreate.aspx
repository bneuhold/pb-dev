<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterHome.master" CodeFile="BookingAdminCreate.aspx.cs" Inherits="manage_BookingAdminCreate" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/css/booking_admin.css"/>    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" />

<script type="text/javascript" language="javascript">

    function lbNextClick() {

        try {
            var retVal = true;
            var ele = document.getElementById('<%=lbNext.ClientID%>');

            if (typeof (Page_ClientValidate) == 'function') { 
                Page_ClientValidate();
            }

            //  prvom koraku sa kalendarom Page_IsValid nije definiran. nisam siguran zasto!?
            if (typeof (Page_IsValid) == "undefined" || Page_IsValid) {

                if (ele != null) {
                    // pri put ce vratit true pa ga disableat tako da svaki drugi put vrati false
                    if (!ele.disabled)
                        retVal = true;
                    else
                        retVal = false;

                    ele.disabled = true;
                }

            }
            else {
                return retVal;
            }
        }
        catch (err) {
            alert(err.description);
        }

        return retVal;
    }


    function lbBackClick() {

        try {
            var retVal = true;
            var ele = document.getElementById('<%=lbBack.ClientID%>');

            if (ele != null) {
                // pri put ce vratit true pa ga disableat tako da svaki drugi put vrati false
                if (!ele.disabled)
                    retVal = true;
                else
                    retVal = false;

                ele.disabled = true;
            }
        }
        catch (err) {
            alert(err.description);
        }

        return retVal;
    }

</script>


    <div class="main_content">

    <asp:PlaceHolder runat="server" ID="phMainContent">
    
    <div style="clear:both; display:inline; float:left; font-weight:bold; font-size:14px; margin-bottom:10px;">
        <asp:Label runat="server" ID="lblAgencyName"></asp:Label>
    </div>
    <div style="clear:both; display:inline; float:left; font-weight:bold; font-size:14px; margin-bottom:10px;">
        <asp:Label runat="server" ID="lblAccommName"></asp:Label>
    </div>
    <div style="clear:both; display:inline; float:left;">
        <asp:HyperLink runat="server" ID="hlReturn"><%= Resources.booking.RetToApartmentsList %></asp:HyperLink>
    </div>

    <asp:UpdatePanel runat="server" id="UpdatePanel1" updatemode="Conditional">

        <Triggers>
            <asp:AsyncPostBackTrigger controlid="lbNext" eventname="Click" />
        </Triggers>

        <ContentTemplate>

        <!-- DATE SELECT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepDateSelect">

        <div class="calendar">
            <div class="select_text" style="margin-bottom:10px;"><%= Resources.booking.DateOfArrival %></div>
            <div style="clear:both; float:left;">
                <asp:Calendar ID="calFrom" runat="server"
                BorderStyle="Solid" BorderColor="#999999" BorderWidth="1px" DayStyle-ForeColor="#404040" SelectedDayStyle-BackColor="#0099E6" SelectedDayStyle-ForeColor="#F7F7F7" DayStyle-BorderStyle="None" DayStyle-BorderWidth="0">
                    <TitleStyle ForeColor="#1E81A9" />
                    <NextPrevStyle ForeColor="#1E81A9" />
                    <TitleStyle BackColor="#EDEDED" />
                    <OtherMonthDayStyle ForeColor="#999999" />     
                </asp:Calendar>
            </div>
        </div>

        <div class="right_selection">
            <div class="select_text"><%= Resources.booking.NumOfNights %>:</div>
            <div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfNights" AutoPostBack="true"></asp:DropDownList></div>
            <div class="select_text"><%= Resources.booking.NumOfPersons %>:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfPersons" AutoPostBack="true"></asp:DropDownList></div>
            <div class="select_text"><%= Resources.booking.NumOfBabies %>:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfBabies" AutoPostBack="true"></asp:DropDownList></div>
        </div>

        <asp:PlaceHolder runat="server" ID="phPriceByDay">
            <div class="price_by_day">
                <div><%= Resources.booking.PriceByDay %>:</div>
                <div class="price_by_day_rep">
                    <asp:Repeater runat="server" ID="rptDaysWithPrices">
                        <HeaderTemplate>
                        <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="margin-left:5px;">
                            <td><%= Resources.booking.Day %>: <%# ((DateTime)Eval("Key")).ToShortDateString() %></td><td>, <%= Resources.booking.Price %>: <%# Eval("Value") %> €</td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </asp:PlaceHolder>

        </asp:PlaceHolder>

        <!-- INFO INPUT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepInfoInput" Visible="false">

        <div class="info_content">

            <div class="info_label"><%= Resources.booking.FirstName %>:</div>
            <div class="info_text_box">
                <asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ForeColor="#DB7070" id="reqFirstName" controltovalidate="tbFirstName" errormessage=" *" />
            </div>

            <div class="info_label"><%= Resources.booking.LastName %>:</div>
            <div class="info_text_box">
                <asp:TextBox runat="server" ID="tbLastName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ForeColor="#DB7070" id="reqLastName" controltovalidate="tbLastName" errormessage=" *" />
            </div>

            <div class="info_label"><%= Resources.booking.Email %>:</div>
            <div class="info_text_box">
                <asp:TextBox runat="server" ID="tbEmail"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ForeColor="#DB7070" id="reqEmail" controltovalidate="tbEmail" errormessage=" *" />
            </div>

            <div class="info_label"><%= Resources.booking.Telephone %>:</div>
            <div class="info_text_box">
                <asp:TextBox runat="server" ID="tbPhone"></asp:TextBox>
            </div>

            <div class="info_label"><%= Resources.booking.Country %>:</div>
            <div class="info_text_box">
                <asp:TextBox runat="server" ID="tbCountry"></asp:TextBox>
            </div>

            <div class="info_label"><%= Resources.booking.City %>:</div>
            <div class="info_text_box">
                <asp:TextBox runat="server" ID="tbCity"></asp:TextBox>
            </div>

            <div class="info_label"><%= Resources.booking.Address %>:</div>
            <div class="info_text_box">
                 <asp:TextBox runat="server" ID="tbStreet"></asp:TextBox>
            </div>
            
            <div class="email_err">
                <asp:RegularExpressionValidator ID="regexEmail" runat="server"    
                                        ErrorMessage="Neispravan format Email adrese!" ForeColor="#DB7070"
                                        ControlToValidate="tbEmail"     
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </div>

        </div>
        </asp:PlaceHolder>

        <!-- PAYMENT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepPayment" Visible="false">

        <div class="payment_content">
            <div class="promocode_title"><%= Resources.booking.Booking_confirm %></div>

            <asp:PlaceHolder runat="server" ID="phPromoCode">
                <div class="promocode_label"><%= Resources.booking.PromoCode %>:</div>
                <div class="promocode_textbox"><asp:TextBox runat="server" ID="tbPromoCode"></asp:TextBox></div>
                <div class="prmocode_btn"><asp:LinkButton runat="server" ID="lbAddPromoCode"><%= Resources.booking.Add %></asp:LinkButton></div>
                <div class="promocode_err">
                    <asp:Label runat="server" ID="lblPromoCodeErrorMsg"></asp:Label>
                </div>
            </asp:PlaceHolder>

        </div>

        </asp:PlaceHolder>

        <!-- BOOKING COMPLETE STEP (ovaj se vise nece pokazivati jer ce se redirektati na BookingNovakComplete.aspx koji ce sadrzavati AdWords kod -->
        <asp:PlaceHolder runat="server" ID="phStepComplete" Visible="false">
            <div class="booking_success">
                <%= Resources.booking.BookingCreatedSuccess %>
            </div>
        </asp:PlaceHolder>


        <div class="err_msg">
            <asp:Label runat="server" ID="lblErrorMsg"></asp:Label>
        </div>

        <!-- BOTTOM SUM -->
        <asp:PlaceHolder runat="server" ID="phBottom">

            <asp:PlaceHolder runat="server" ID="phNoPrices">
                <div style="clear:both; width:60%; margin: 0px auto; margin-bottom:15px; text-align:left; color:#006699;">
                    <%= Resources.booking.NO_PRICES %>
                </div>
            </asp:PlaceHolder>

            <div class="bottom_content">
                <div style="clear:both; float:left; display:inline; margin:0 0 0px 0x; font-weight:bold;">
                    <%= Resources.booking.PriceSum %>: <asp:Label runat="server" ID="lblPriceSum"></asp:Label> €
                </div>

                <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                    <asp:Label runat="server" ID="lblDateMsg"></asp:Label>
                </div>
                <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                    <%= Resources.booking.NumOfPersons %>: <asp:Label runat="server" ID="lblNumOfPersons"></asp:Label>
                </div>
                <div style="clear:both; float:left; display:inline; margin:10px 0 0 0;">
                    <%= Resources.booking.NumOfBabies %>: <asp:Label runat="server" ID="lblNumOfBabies"></asp:Label>
                </div>
            </div>

            <div class="buttons">
                <div class="back button">
                    <asp:LinkButton runat="server" id="lbBack" CausesValidation="false" Visible="false" OnClientClick="return lbBackClick();"><%= Resources.booking.Back %></asp:LinkButton>
                </div>
                <div class="next button">
                    <asp:LinkButton runat="server" id="lbNext" OnClientClick="return lbNextClick();"><%= Resources.booking.Next %></asp:LinkButton>
                </div>
            </div>

        </asp:PlaceHolder>

        </ContentTemplate>

    </asp:UpdatePanel>

    </asp:PlaceHolder>
    

    <asp:PlaceHolder runat="server" ID="phLoadError">

    <div class="load_error">
        <asp:Label runat="server" ID="lblLoadErrorMsg"></asp:Label>
    </div>

    </asp:PlaceHolder>

    </div>


</asp:Content>