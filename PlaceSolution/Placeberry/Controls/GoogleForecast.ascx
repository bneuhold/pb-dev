<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GoogleForecast.ascx.cs" Inherits="Controls_GoogleForecast" %>

<asp:PlaceHolder ID="plhCurrent" runat="server">
    <div class="weather_today">
        
        <h3>Weather today<span>(<asp:Literal ID="ltToday" runat="server"></asp:Literal>)</span></h3>

        <img id="imgIcon" runat="server" src="" alt="" />

        <p class="simple">
        <asp:Literal ID="ltlCondition" Text="" runat="server" />, <asp:Literal ID="ltlTempC" Text="" runat="server" />&deg;C
        </p>

        <div style="display: none;">
        <h4>Stanje:</h4>
        <p></p>
        <h4>Vlažnost:</h4>
        <p><asp:Literal ID="ltlHumidity" Text="" runat="server" /></p>
        <h4>Temperatura:</h4>
        <p></p>
        <h4>Vjetar:</h4>
        <p><asp:Literal ID="ltlWind" Text="" runat="server" /></p>
        </div>
    </div>
</asp:PlaceHolder>

<asp:Repeater ID="repForecastDays" runat="server">
    <HeaderTemplate>
        <div class="weather_week">
        <h2>Ostali dani</h2>
    </HeaderTemplate>
    <ItemTemplate>
        <h4>Dan</h4>
        <p><%# Eval("DayOfWeek")%></p>

        <h4>Stanje</h4>
        <p><%# Eval("Condition") %></p>

        <h4>Vlažnost</h4>
        <p><%# Eval("HighC") %>&deg;C</p>

        <h4>Temperatura</h4>
        <p><%# Eval("LowC") %>&deg;C</p>

        <img src="<%# Eval("Icon") %>" alt="<%# Eval("Condition") %>" />
    </ItemTemplate>
    <SeparatorTemplate>
        <br />
    </SeparatorTemplate>
    <FooterTemplate>
    </div>
    </FooterTemplate>
</asp:Repeater>

<br />