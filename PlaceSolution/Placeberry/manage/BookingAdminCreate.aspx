<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterManage.master" CodeFile="BookingAdminCreate.aspx.cs" Inherits="manage_BookingAdminCreate" %>


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
    
    <div class="header-bar">
        <asp:Label runat="server" ID="lblAgencyName"></asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblAccommName"></asp:Label>
    </div>

    <asp:HyperLink runat="server" class="booking-link button-header-back ui-corner-all" ID="hlReturn"><%= Resources.booking.RetToApartmentsList %></asp:HyperLink>


    <asp:UpdatePanel runat="server" id="UpdatePanel1" updatemode="Conditional">

        <Triggers>
            <asp:AsyncPostBackTrigger controlid="lbNext" eventname="Click" />
        </Triggers>

        <ContentTemplate>

        <!-- DATE SELECT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepDateSelect">
        
            <fieldset class="editor basic-info ui-corner-all centred">
                <legend>Booking</legend>

                <div class="form-editor">
                    <div class="form-row">
                        <div class="form-cell label">><%= Resources.booking.DateOfArrival %></div>
                        <div class="form-cell value">
                            <asp:Calendar ID="calFrom" runat="server"
                            BorderStyle="Solid" BorderColor="#999999" BorderWidth="1px" DayStyle-ForeColor="#404040" SelectedDayStyle-BackColor="#0099E6" SelectedDayStyle-ForeColor="#F7F7F7" DayStyle-BorderStyle="None" DayStyle-BorderWidth="0">
                                <TitleStyle ForeColor="#1E81A9" />
                                <NextPrevStyle ForeColor="#1E81A9" />
                                <TitleStyle BackColor="#EDEDED" />
                                <OtherMonthDayStyle ForeColor="#999999" />     
                            </asp:Calendar>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label"><%= Resources.booking.NumOfNights %>:</div>
                        <div class="form-cell value">
                            <div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfNights" AutoPostBack="true"></asp:DropDownList></div>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label"><%= Resources.booking.NumOfPersons %>:</div>
                        <div class="form-cell value">
                            <div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfPersons" AutoPostBack="true"></asp:DropDownList></div>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label"><%= Resources.booking.NumOfBabies %>:</div>
                        <div class="form-cell value">
                            <div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfBabies" AutoPostBack="true"></asp:DropDownList></div>
                        </div>
                        <div class="close-row"></div>
                    </div>
                </div>
            </fieldset>

        <asp:PlaceHolder runat="server" ID="phPriceByDay">
            <div class="list-wrapper booking-days top-m bottom-m centred">
                <h2 class="title"><%= Resources.booking.PriceByDay %>:</h2>
                <ul class="link-list">
                    <asp:Repeater runat="server" ID="rptDaysWithPrices">
                        <HeaderTemplate>
                        <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li class="sub-item"><span class="day"><%= Resources.booking.Day %>: <%# ((DateTime)Eval("Key")).ToShortDateString() %></span>, <span class="price"><%= Resources.booking.Price %>: <%# Eval("Value") %> €</span></li>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </asp:PlaceHolder>

        </asp:PlaceHolder>

        <!-- INFO INPUT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepInfoInput" Visible="false">

            <fieldset class="editor basic-info ui-corner-all centred">
                <legend>Personal info</legend>
                <div class="form-editor">
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.FirstName %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ForeColor="#DB7070" id="reqFirstName" controltovalidate="tbFirstName" errormessage=" *" />
		                </div>
		                <div class="close-row"></div>
	                </div>
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.LastName %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbLastName"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ForeColor="#DB7070" id="reqLastName" controltovalidate="tbLastName" errormessage=" *" />
		                </div>
		                <div class="close-row"></div>
	                </div>
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.Email %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbEmail"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ForeColor="#DB7070" id="reqEmail" controltovalidate="tbEmail" errormessage=" *" />
		                </div>
		                <div class="close-row"></div>
	                </div>
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.Telephone %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbPhone"></asp:TextBox>
		                </div>
		                <div class="close-row"></div>
	                </div>
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.Country %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbCountry"></asp:TextBox>
		                </div>
		                <div class="close-row"></div>
	                </div>
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.City %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbCity"></asp:TextBox>
		                </div>
		                <div class="close-row"></div>
	                </div>
                    <div class="form-row">
		                <div class="form-cell label"><%= Resources.booking.Address %>:</div>
		                <div class="form-cell value">
                            <asp:TextBox runat="server" ID="tbStreet"></asp:TextBox>
		                </div>
		                <div class="close-row"></div>
	                </div>
                </div>
                <div class="email_err warning-message">
                    <asp:RegularExpressionValidator ID="regexEmail" runat="server"    
                                            ErrorMessage="Neispravan format Email adrese!" 
                                            ControlToValidate="tbEmail"     
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                </div>
            </fieldset>
        </div>
        </asp:PlaceHolder>

        <!-- PAYMENT STEP -->

        <asp:PlaceHolder runat="server" ID="phStepPayment" Visible="false">
            <h1 class="step-title"><%= Resources.booking.Booking_confirm %></h1>
            <fieldset class="editor promocode ui-corner-all centred">
                <legend><%= Resources.booking.PromoCode %></legend>
                
                <asp:PlaceHolder runat="server" ID="phPromoCode">
                    <div class="promo-editor">
                        <asp:TextBox runat="server" ID="tbPromoCode"></asp:TextBox>
                        <asp:LinkButton runat="server" CssClass="button" ID="lbAddPromoCode"><%= Resources.booking.Add %></asp:LinkButton>
                        <div class="warning-message">
                            <asp:Label runat="server" ID="lblPromoCodeErrorMsg"></asp:Label>
                        </div>
                    </div>
                </asp:PlaceHolder>
                
            </fieldset>

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
                <div class="centred">
                    <%= Resources.booking.NO_PRICES %>
                </div>
            </asp:PlaceHolder>
            
            <div class="sum-footer ui-corner-all centred">

		        <div class="form-row">
			        <div class="form-cell label"><%= Resources.booking.PriceSum %>:</div>
			        <div class="form-cell value">
                            <asp:Label runat="server" ID="lblPriceSum"></asp:Label> €
			        </div>
			        <div class="close-row"></div>
		        </div>
                <div class="form-row">
			        <div class="form-cell label"></div>
			        <div class="form-cell value">
                        <asp:Label runat="server" ID="lblDateMsg"></asp:Label>        
			        </div>
			        <div class="close-row"></div>
		        </div>
                <div class="form-row">
			        <div class="form-cell label"><%= Resources.booking.NumOfPersons %>:</div>
			        <div class="form-cell value">
                            <asp:Label runat="server" ID="lblNumOfPersons"></asp:Label>
			        </div>
			        <div class="close-row"></div>
		        </div>
                <div class="form-row">
			        <div class="form-cell label"><%= Resources.booking.NumOfBabies %>:</div>
			        <div class="form-cell value">
                         <asp:Label runat="server" ID="lblNumOfBabies"></asp:Label>   
			        </div>
			        <div class="close-row"></div>
		        </div>
	        </div>

            <div class="action-bar step-buttons centred">
                <asp:LinkButton runat="server" id="lbBack" CssClass="back ui-corner-all" CausesValidation="false" Visible="false" OnClientClick="return lbBackClick();"><%= Resources.booking.Back %></asp:LinkButton>
                <asp:LinkButton runat="server" id="lbNext" CssClass="next ui-corner-all" OnClientClick="return lbNextClick();"><%= Resources.booking.Next %></asp:LinkButton>
                <div class="clearfix"></div>
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