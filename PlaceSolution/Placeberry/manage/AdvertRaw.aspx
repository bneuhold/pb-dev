<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="AdvertRaw.aspx.cs" Inherits="AdvertRaw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>AdvertRaw</h1>
    <br />
    <asp:Repeater ID="repAdverts" runat="server">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <%--<td><%#Eval("Source") %></td>
                <td><%#Eval("Language") %></td>
                <td><%#Eval("GroupType") %></td>
                <td><%#Eval("GroupSubType") %></td>
                <td><%#Eval("SourceCategory") %></td>--%>
                <td>
                    <a href='/Manage/AdvertRawItem.aspx?id=<%#Eval("Id") %>'><%#Eval("Title") %></a>
                </td>
                <%--<td><%#Eval("AccommType") %></td>
                <td><%#Eval("AccommSubType") %></td>
                <td><%#Eval("VacationType") %></td>
                <td><%#Eval("AdvertCode") %></td>
                <td><%#Eval("UrlLink") %></td>
                <td><%#Eval("PictureUrl") %></td>
                <td><%#Eval("Stars") %></td>
                <td><%#Eval("LocationDesc") %></td>
                <td><%#Eval("Country") %></td>
                <td><%#Eval("Region") %></td>
                <td><%#Eval("Subregion") %></td>
                <td><%#Eval("Island") %></td>
                <td><%#Eval("City") %></td>
                <td><%#Eval("PriceOld") %></td>
                <td><%#Eval("PriceFrom") %></td>
                <td><%#Eval("PriceDesc") %></td>
                <td><%#Eval("Date1") %></td>
                <td><%#Eval("Date2") %></td>
                <td><%#Eval("DateDesc") %></td>
                <td><%#Eval("DaysNum") %></td>
                <td><%#Eval("Description") %></td>
                <td><%#Eval("Activities") %></td>
                <td><%#Eval("Facilities") %></td>
                <td><%#Eval("Beach") %></td>
                <td><%#Eval("BeachDistanceM") %></td>
                <td><%#Eval("DistanceFromCentreM") %></td>
                <td><%#Eval("PetsDesc") %></td>
                <td><%#Eval("InfoDesc") %></td>--%>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <a href="AdvertRawItem.aspx">Novi zapis</a>
</asp:Content>

