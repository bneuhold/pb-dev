<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WheaterForecast.ascx.cs" Inherits="Controls_WheaterForecast" EnableViewState="false" %>

<div class="weather_forecast">
    
    <div class="today">
        <asp:FormView ID="fvwPlace" runat="server">
            <ItemTemplate>
                <h2><%# Eval("Name") %></h2>
            </ItemTemplate>
        </asp:FormView>
        <asp:FormView ID="fvwToday" runat="server">
            <ItemTemplate>
                <table>
                    <tr>
                        <th></th>
                        <th><%# ((DateTime)Eval("Date")).ToShortDateString()%></th>
                    </tr>                    
                    <tr>
                        <th>Opis dan</th>
                        <td><%# Eval("DescriptionDay")%></td>
                    </tr>                    
                    <tr>
                        <th>Ikona dan</th>
                        <td><img src="<%# Eval("IconDay")%>" /></td>
                    </tr>                    
                    <tr>
                        <th>Temp dan</th>
                        <td><%# Eval("TemperatureDay")%></td>
                    </tr>                    
                    <tr>
                        <th>Real feel dan</th>
                        <td><%# Eval("RealFeelDay")%></td>
                    </tr>                
                    <tr>
                        <th>Opis noć</th>
                        <td><%# Eval("DescriptionNight")%></td>
                    </tr>                    
                    <tr>
                        <th>Ikona noć</th>
                        <td><img src="<%# Eval("IconNight")%>" alt="iconnight" /></td>
                    </tr>                    
                    <tr>
                        <th>Temp noć</th>
                        <td><%# Eval("TemperatureNight")%></td>
                    </tr>                    
                    <tr>
                        <th>Real feel noć</th>
                        <td><%# Eval("RealFeelNight")%></td>
                    </tr>
                </table>                
            </ItemTemplate>
        </asp:FormView>        
    </div>
    <br /><br /><br />
    <div class="hourly">        
        <asp:Repeater ID="repHourly" runat="server" >
            <HeaderTemplate>
                <h2>Po satima</h2>
            </HeaderTemplate>
            <ItemTemplate>
                <table>
                    <tr>
                        <th>Sat</th>
                        <th><%#  ((DateTime)Eval("Hour")).ToShortTimeString() %></th>
                    </tr>                    
                    <tr>
                        <th>Opis</th>
                        <td><%# Eval("Description") %></td>
                    </tr>                    
                    <tr>
                        <th>Vlaga</th>
                        <td><%# Eval("Humidity")%></td>
                    </tr>                    
                    <tr>
                        <th>Real feel</th>
                        <td><%# Eval("RealFeel")%></td>
                    </tr>                    
                    <tr>
                        <th>Temperatura</th>
                        <td><%# Eval("Temperature")%></td>
                    </tr>                    
                                 
                    <tr>
                        <th>Smjer vjetra</th>
                        <td><%# Eval("WindDirection")%></td>
                    </tr>                     
                    <tr>
                        <th>Brzina vjetra</th>
                        <td><%# Eval("WindSpeed")%></td>
                    </tr>                    
                </table>
            </ItemTemplate>
            <SeparatorTemplate>
                <br /><br />
            </SeparatorTemplate>
        </asp:Repeater>      
    </div>
    <br /><br /><br />
    <div class="po_danima">
        <asp:Repeater ID="repWeeks" runat="server">
            <ItemTemplate>
                <table>
                    <tr>
                        <th></th>
                        <th><%# ((DateTime)Eval("Date")).ToShortDateString() %></th>
                    </tr>                    
                    <tr>
                        <th>Opis dan</th>
                        <td><%# Eval("DescriptionDay") %></td>
                    </tr>                    
                    <tr>
                        <th>Ikona dan</th>
                        <td><img src="<%# Eval("IconDay")%>" /></td>
                    </tr>                    
                    <tr>
                        <th>Temp dan</th>
                        <td><%# Eval("TemperatureDay")%></td>
                    </tr>                    
                    <tr>
                        <th>Real feel dan</th>
                        <td><%# Eval("RealFeelDay")%></td>
                    </tr>                
                    <tr>
                        <th>Opis noć</th>
                        <td><%# Eval("DescriptionNight")%></td>
                    </tr>                    
                    <tr>
                        <th>Ikona noć</th>
                        <td><img src="<%# Eval("IconNight")%>" alt="iconnight" /></td>
                    </tr>                    
                    <tr>
                        <th>Temp noć</th>
                        <td><%# Eval("TemperatureNight")%></td>
                    </tr>                    
                    <tr>
                        <th>Real feel noć</th>
                        <td><%# Eval("RealFeelNight")%></td>
                    </tr>
                </table>                
            </ItemTemplate>
            <SeparatorTemplate>
                <br /><br /><br />
            </SeparatorTemplate>
        </asp:Repeater>

    </div>

</div>