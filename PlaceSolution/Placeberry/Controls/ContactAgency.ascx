<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactAgency.ascx.cs" Inherits="Controls_ContactAgency" %>

<div class="contact_form">
    <asp:UpdatePanel ID="upnlSubmitContactForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="mvwSendState" ActiveViewIndex="0" runat="server">
                <asp:View ID="View1" runat="server">
                    <br />
                    <label>Ime i prezime
                     <asp:RequiredFieldValidator ID="rfvName" runat="server"
                        CssClass="validator" 
                        Display="Dynamic"
                        ErrorMessage="*" 
                        ValidationGroup="contact"
                        ControlToValidate="tbxName" />
                    </label>
                    <asp:TextBox ID="tbxName" runat="server" />
    

                    <br />
                    <br />
                    <label>E-mail<asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        CssClass="validator"
                        Display="Dynamic"
                        ErrorMessage="*" 
                        ValidationGroup="contact"
                        ControlToValidate="tbxEmail" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        CssClass="validator"
                        Display="Dynamic"
                        ErrorrMessage="Neispravna e-mail adresa"
                        ValidationGroup="contact"
                        ControlToValidate="tbxEmail" 
                        ValidationExpression="[\w_.\-+]{4,}@[\w_\-+]{2,}([\w_\-+]+.?)*\.[\w]{2,3}" />
                        </label>
                    <asp:TextBox ID="tbxEmail" runat="server" />
        
        

                    <br />
                    <br />
                    <label>Mob</label>
                    <asp:TextBox ID="tbxTelephone" runat="server" />

                    <br />
                    <br />
                    <div class="dates clearfix">
                        <div  class="pair">
                            <label>Broj osoba</label>
                            <asp:TextBox ID="tbxCapacity" runat="server" />
                        </div>
                        <div class="pair">
                            <label>Datum dolaska</label>
                            <asp:TextBox ID="tbxDateStart" runat="server" />
                        </div>
                        <div class="pair">   
                            <label>Datum odlaska</label>
                            <asp:TextBox ID="tbxDateEnd" runat="server" />
                        </div>
                    </div> 
                    <br />
                    <label>Poruka
                    <asp:RequiredFieldValidator ID="rfvMessage" runat="server"
                        CssClass="validator"
                        Display="Dynamic"
                        ErrorMessage="*" 
                        ValidationGroup="contact"
                        ControlToValidate="tbxMessage" />
                    </label>
                    <asp:TextBox ID="tbxMessage" runat="server" Rows="8" TextMode="MultiLine" Width="460px" />
        

                    <br />
                    <br />
                    <asp:Button ID="btnContactSubmit" runat="server" 
                        Text="Submit"
                        CausesValidation="true" 
                        ValidationGroup="contact" 
                        CssClass="btnSend" 
                        onclick="btnContactSubmit_Click"  />

                </asp:View>

                <asp:View runat="server">
                    <b>Poslano!</b>
                </asp:View>
                <asp:View runat="server">
                    <b>Greška pri slanju, pokušajte kasnije</b>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnContactSubmit" />
        </Triggers>
    </asp:UpdatePanel>
        <asp:UpdateProgress ID="upSubmitProgress" runat="server" AssociatedUpdatePanelID="upnlSubmitContactForm">
        <ProgressTemplate>
            <br />
            <img src="/resources/images/loading.gif" alt="" />
        </ProgressTemplate>
    </asp:UpdateProgress>
<%-- OVO TREBA INCLUDEATI U HEAD DA RADI KALENDAR
    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
--%>
    <script type="text/javascript">
        $(function () {
            var dates = $("#<%= tbxDateStart.ClientID %>, #<%= tbxDateEnd.ClientID %>").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    dateFormat: "dd.mm.yy.",
                    onSelect: function (selectedDate) {
                        var option = this.id == "<%= tbxDateStart.ClientID %>" ? "minDate" : "maxDate",
				        instance = $(this).data("datepicker"),
				        date = $.datepicker.parseDate(
					        instance.settings.dateFormat ||
					        $.datepicker._defaults.dateFormat,
					        selectedDate, instance.settings);
                        dates.not(this).datepicker("option", option, date);
                    }
                });

            });
    </script>
</div>