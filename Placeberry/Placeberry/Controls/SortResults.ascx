<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SortResults.ascx.cs" Inherits="Controls_SortResults" %>
<asp:Literal ID="litSortResults" runat="server" Text="<%$ Resources:placeberry, Vacation_SortResults %>"></asp:Literal>
<a href='<%= Resources.placeberry.General_VacationUrl + "?q="+Request["q"]+"&o=0" %>'><asp:Literal ID="litSortRelevance" runat="server" Text="<%$ Resources:placeberry, Vacation_SortByRelevance %>"></asp:Literal></a>
<a href='<%= Resources.placeberry.General_VacationUrl + "?q="+Request["q"]+"&o=1" %>'><asp:Literal ID="litSortPriceAsc" runat="server" Text="<%$ Resources:placeberry, Vacation_SortByPriceAsc %>"> </asp:Literal></a>
<a href='<%= Resources.placeberry.General_VacationUrl + "?q="+Request["q"]+"&o=2" %>'><asp:Literal ID="litSortPriceDesc" runat="server" Text="<%$ Resources:placeberry, Vacation_SortByPriceDesc %>"> </asp:Literal></a>
<a href='<%= Resources.placeberry.General_VacationUrl + "?q="+Request["q"]+"&o=3" %>'><asp:Literal ID="litSortNameAsc" runat="server" Text="<%$ Resources:placeberry, Vacation_SortByNameAsc %>"> </asp:Literal></a>
<a href='<%= Resources.placeberry.General_VacationUrl + "?q="+Request["q"]+"&o=4" %>'><asp:Literal ID="litSortNameDesc" runat="server" Text="<%$ Resources:placeberry, Vacation_SortByNameDesc %>"> </asp:Literal></a>
