<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="UltimateTableObjectTypes.aspx.cs" Inherits="manage_UltimateTableObjectType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <br />
    <h1>UltimateTableObjectType pregled</h1>
    <br />
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
        DataSourceID="ldsObjectTypes" EnableModelValidation="True" 
        InsertItemPosition="LastItem">
        <AlternatingItemTemplate>
            <tr style="background-color: #FFFFFF;color: #284775;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' />
                </td>
                <td>
                    <asp:Label ID="DescriptionLabel" runat="server" 
                        Text='<%# Eval("Description") %>' />
                </td>
                <td>
                    <asp:Label ID="CodeLabel" runat="server" Text='<%# Eval("Code") %>' />
                </td>
                <td>
                    <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("Priority") %>' />
                </td>
                <td>
                    <asp:Label ID="GroupCodeLabel" runat="server" Text='<%# Eval("GroupCode") %>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <tr style="background-color: #999999;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Cancel" />
                </td>
                <td>
                    <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:TextBox ID="TitleTextBox" runat="server" Text='<%# Bind("Title") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DescriptionTextBox" runat="server" 
                        Text='<%# Bind("Description") %>' />
                </td>
                <td>
                    <asp:TextBox ID="CodeTextBox" runat="server" Text='<%# Bind("Code") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PriorityTextBox" runat="server" 
                        Text='<%# Bind("Priority") %>' />
                </td>
                <td>
                    <asp:TextBox ID="GroupCodeTextBox" runat="server" 
                        Text='<%# Bind("GroupCode") %>' />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" 
                style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;">
                <tr>
                    <td>
                        No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style="">
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                        Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Clear" />
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="TitleTextBox" runat="server" Text='<%# Bind("Title") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DescriptionTextBox" runat="server" 
                        Text='<%# Bind("Description") %>' />
                </td>
                <td>
                    <asp:TextBox ID="CodeTextBox" runat="server" Text='<%# Bind("Code") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PriorityTextBox" runat="server" 
                        Text='<%# Bind("Priority") %>' />
                </td>
                <td>
                    <asp:TextBox ID="GroupCodeTextBox" runat="server" 
                        Text='<%# Bind("GroupCode") %>' />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style="background-color: #E0FFFF;color: #333333;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' />
                </td>
                <td>
                    <asp:Label ID="DescriptionLabel" runat="server" 
                        Text='<%# Eval("Description") %>' />
                </td>
                <td>
                    <asp:Label ID="CodeLabel" runat="server" Text='<%# Eval("Code") %>' />
                </td>
                <td>
                    <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("Priority") %>' />
                </td>
                <td>
                    <asp:Label ID="GroupCodeLabel" runat="server" Text='<%# Eval("GroupCode") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="1" 
                            style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                            <tr runat="server" style="background-color: #E0FFFF;color: #333333;">
                                <th runat="server">
                                </th>
                                <th runat="server">
                                    Id</th>
                                <th runat="server">
                                    Title</th>
                                <th runat="server">
                                    Description</th>
                                <th runat="server">
                                    Code</th>
                                <th runat="server">
                                    Priority</th>
                                <th runat="server">
                                    GroupCode</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" 
                        style="text-align: center;background-color: #5D7B9D;font-family: Verdana, Arial, Helvetica, sans-serif;color: #FFFFFF">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <tr style="background-color: #E2DED6;font-weight: bold;color: #333333;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' />
                </td>
                <td>
                    <asp:Label ID="DescriptionLabel" runat="server" 
                        Text='<%# Eval("Description") %>' />
                </td>
                <td>
                    <asp:Label ID="CodeLabel" runat="server" Text='<%# Eval("Code") %>' />
                </td>
                <td>
                    <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("Priority") %>' />
                </td>
                <td>
                    <asp:Label ID="GroupCodeLabel" runat="server" Text='<%# Eval("GroupCode") %>' />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:LinqDataSource ID="ldsObjectTypes" runat="server" 
        ContextTypeName="UltimateDC.UltimateDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" OrderBy="Id" 
        TableName="UltimateTableObjectTypes">
    </asp:LinqDataSource>
</asp:Content>

