<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterHome.master" CodeFile="BookingAdminEdit.aspx.cs" Inherits="manage_BookingAdminEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../resources/css/booking_admin.css"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />


<div class="main_content" style="width:550px; min-height:400px;">

<asp:PlaceHolder runat="server" ID="phMainContent">

    <asp:UpdatePanel runat="server" id="UpdatePanel1" RenderMode="Block" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>    

    <asp:HiddenField runat="server" ID="hfAccommodationId" EnableViewState="true" />
    <asp:HiddenField runat="server" ID="hfBookingId" EnableViewState="true" />

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

        <div class="right_selection" style="margin:38px 0 0 0;">
            <div class="select_text"><%= Resources.booking.NumOfNights %>:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfNights" AutoPostBack="true"></asp:DropDownList></div>
            <div class="select_text"><%= Resources.booking.NumOfPersons %>:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfPersons" AutoPostBack="true"></asp:DropDownList></div>
            <div class="select_text"><%= Resources.booking.NumOfBabies %>:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlNumOfBabies" AutoPostBack="true"></asp:DropDownList></div>
            <div class="select_text">Status:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="true"></asp:DropDownList></div>
            <div class="select_text">Osnovna cijena:</div><div class="edit_input"><asp:TextBox runat="server" Width="70" ID="tbPriceBasic"></asp:TextBox></div>
            <div class="select_text">Konacna cijena:</div><div class="edit_input"><asp:TextBox runat="server" Width="70" ID="tbPriceSum"></asp:TextBox></div>
            <div class="select_text">Valuta:</div><div class="select_style"><asp:DropDownList runat="server" ID="ddlCurrency" AutoPostBack="true"></asp:DropDownList></div>
        </div>

        <div style="display:inline-block; width:100%; text-align:center; margin:20px 0;">
            <asp:Label runat="server" ID="lblRetMsg" ForeColor="Red">&nbsp;</asp:Label>
        </div>

    </ContentTemplate>

    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbSave" EventName="Click" />
    </Triggers>

    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" id="UpdatePanel2" RenderMode="Block" UpdateMode="Conditional">    
    <ContentTemplate>    


    </ContentTemplate>

    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbSave" EventName="Click" />
    </Triggers>

    </asp:UpdatePanel>

    <div style="width:100%; display:inline-block;">
        <div class="select_text" style="width:50%; text-align:right;">Datum kreiranja rezervacije:</div><div class="select_text" style="clear:none;"><asp:Label runat="server" ID="lblCreateDate"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Kreirano od administratora:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblAdminCreateName"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Datum zadnje izmjene:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblLastUpdateDate"></asp:Label></div>

        <div class="select_text" style="font-weight:bold; margin-top:20px; width:100%; text-align:center;">Podaci o gostu:</div>
        <div class="select_text" style=" width:50%; text-align:right;">Ime:</div><div class="select_text" style="clear:none"><asp:Label runat="server" ID="lblUserFirstName"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Prezime:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblUserLastName"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Email:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblUserEmail"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Phone:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblUserPhone"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Country:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblUserCountry"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">City:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblUserCity"></asp:Label></div>
        <div class="select_text" style="margin-top:2px; width:50%; text-align:right;">Street:</div><div class="select_text" style="clear:none; margin-top:2px;"><asp:Label runat="server" ID="lblUserStreet"></asp:Label></div>
    </div>
    <div style="width:100%; height:20px; margin:30px 0 20px 0;">
        <div style="float:left;">
            <asp:HyperLink runat="server" ID="hlBack">Povratak</asp:HyperLink>
        </div>
        <div style="float:right;">
            <asp:LinkButton runat="server" ID="lbSave">Spremi promjene</asp:LinkButton>
        </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="phError" Visible="false">
<div style="width:100%; text-align:center; font-size:14px; color:Red;">
    <asp:Literal runat="server" ID="ltErrMsg"></asp:Literal>
</div>
</asp:PlaceHolder>
</div>

</asp:Content>