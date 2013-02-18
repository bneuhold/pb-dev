<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="UltimateTable.aspx.cs" Inherits="manage_UltimateTable" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        var btnSetNewParentSubmit = null;
        var tbxSetNewParentTitle = null;
        var hfSetNewParentId = null;
        
        var btnSetNewChildSubmit = null;
        var tbxSetNewChildTitle = null;
        var hfSetNewChildId = null;


        var btnSetNewNotGeoParentSubmit = null;
        var tbxSetNewNotGeoParentTitle = null;
        var hfSetNewNotGeoParentId = null;
        
        var btnSetNewNotGeoChildSubmit = null;
        var tbxSetNewNotGeoChildTitle = null;
        var hfSetNewNotGeoChildId = null;

        var hfImportRelations = null;
        var tbxImportRelations = null;
        var btnImportRelations = null;
        var selectedImportRelations = null;

        var tbxSearchUltimateTable = null;
        var ultimateTableId = null;
        var ultimateTableObjectTypeId = null;

        var autocomplmaxresults = 20;

        function pageLoad() {
         
            btnSetNewParentSubmit = $("#<%= btnSetNewParentSubmit.ClientID %>");
            tbxSetNewParentTitle = $("#<%= tbxSetNewParentTitle.ClientID %>");
            hfSetNewParentId = $("#<%= hfSetNewParentId.ClientID %>");

            btnSetNewChildSubmit = $("#<%= btnSetNewChildSubmit.ClientID %>");
            tbxSetNewChildTitle = $("#<%= tbxSetNewChildTitle.ClientID %>");
            hfSetNewChildId = $("#<%= hfSetNewChildId.ClientID %>");


            btnSetNewNotGeoParentSubmit = $("#<%= btnSetNewNotGeoParentSubmit.ClientID %>");
            tbxSetNewNotGeoParentTitle = $("#<%= tbxSetNewNotGeoParentTitle.ClientID %>");
            hfSetNewNotGeoParentId = $("#<%= hfSetNewNotGeoParentId.ClientID %>");

            btnSetNewNotGeoChildSubmit = $("#<%= btnSetNewNotGeoChildSubmit.ClientID %>");
            tbxSetNewNotGeoChildTitle = $("#<%= tbxSetNewNotGeoChildTitle.ClientID %>");
            hfSetNewNotGeoChildId = $("#<%= hfSetNewNotGeoChildId.ClientID %>");

            hfImportRelations = $("#<%= hfImportRelations.ClientID %>");
            tbxImportRelations = $("#tbxImportRelations");
            btnImportRelations = $("#<%= btnImportRelations.ClientID %>");


            ultimateTableId = <%= ultimateTable != null ? ultimateTable.Id : -1 %>;
            ultimateTableObjectTypeId = <%= ultimateTable != null ? ultimateTable.ObjectTypeId : -1 %>;

            if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                SetAutocomplete();
            }

            btnImportRelations.hide();
            tbxImportRelations.bind("change", function() {
                if ($(this).val().toLowerCase() == selectedImportRelations.toLowerCase()){                    
                    btnImportRelations.show();
                }
                else {
                    btnImportRelations.hide();
                }
            });
        }


        $(document).ready(function () {

            $(".regexbutton").live("click", function () {
                GetRegex(this);
            });

            tbxSearchUltimateTable = $("#tbxSearchUltimateTable");

            SetAutocomplete();
        });

        function GetRegex(button) {
            var obj = $(button);

            var value = obj.val().split(";");

            var source = $("#" + value[0]);
            var destin = $("#" + value[1]);
            var lang = $("#" + value[2]);

            var term = source.val();
            var languageid = lang.val();

            $.ajax({
                url: "/manage/ultimatetable.aspx/GetAutoRegex",
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: "{ 'term': '" + term + "', 'languageid': '" + languageid + "' }",
                success: function (data) {
                    destin.val(data.d);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown + " " + textStatus);
                }
            });
        }

        function SetAutocomplete() {

            btnSetNewParentSubmit.hide();
            btnSetNewChildSubmit.hide();

            btnSetNewNotGeoParentSubmit.hide();
            btnSetNewNotGeoChildSubmit.hide();

            tbxSetNewParentTitle.autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/manage/ultimatetable.aspx/GetAutoParentSuggestions",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'childtypeid': '" + ultimateTableObjectTypeId + "', 'maxresults': '" + autocomplmaxresults + "' }",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Title,
                                    value: item.Title,
                                    id: item.Id
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                },
                select: function (event, ui) {
                    hfSetNewParentId.val(ui.item.id);
                    btnSetNewParentSubmit.show();
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

            tbxSetNewChildTitle.autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/manage/ultimatetable.aspx/GetAutoChildSuggestions",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'parentid': '" + ultimateTableId + "', 'parenttypeid': '" + ultimateTableObjectTypeId + "', 'maxresults': '" + autocomplmaxresults + "' }",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Title,
                                    value: item.Title,
                                    id: item.Id
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                },
                select: function (event, ui) {
                    hfSetNewChildId.val(ui.item.id);
                    btnSetNewChildSubmit.show();
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });


            tbxSetNewNotGeoParentTitle.autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/manage/ultimatetable.aspx/GetAutoNotGeoParentSuggestions",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'childid': '" + ultimateTableId + "', 'maxresults': '" + autocomplmaxresults + "' }",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Title,
                                    value: item.Title,
                                    id: item.Id
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                },
                select: function (event, ui) {
                    hfSetNewNotGeoParentId.val(ui.item.id);
                    btnSetNewNotGeoParentSubmit.show();
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

            tbxSetNewNotGeoChildTitle.autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/manage/ultimatetable.aspx/GetAutoNotGeoChildSuggestions",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'parentid': '" + ultimateTableId + "', 'maxresults': '" + autocomplmaxresults + "' }",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Title,
                                    value: item.Title,
                                    id: item.Id
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                },
                select: function (event, ui) {
                    hfSetNewNotGeoChildId.val(ui.item.id);
                    btnSetNewNotGeoChildSubmit.show();
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

            tbxImportRelations.autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/manage/ultimatetable.aspx/GetUltimateTableSearchSuggestions",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'maxresults': '" + autocomplmaxresults + "' }",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Title,
                                    value: item.Title,
                                    url: item.Url,
                                    id: item.Id
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                },
                select: function (event, ui) {
                    hfImportRelations.val(ui.item.id);
                   
                    selectedImportRelations = ui.item.label;
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });


            tbxSearchUltimateTable.autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/manage/ultimatetable.aspx/GetUltimateTableSearchSuggestions",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'maxresults': '" + autocomplmaxresults + "' }",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Title,
                                    value: item.Title,
                                    url: item.Url
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                },
                select: function (event, ui) {
                    window.location = ui.item.url;
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

        }

        function ConfirmImportRelations() {
            return confirm('Sigurno želiš importati relationse s pojma "' + selectedImportRelations + '" ?');
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />                    

    <h1>UltimateTable pregled</h1>
    <br />
    <label>Pretraga: </label><input id="tbxSearchUltimateTable" type="text" class="autocomplete"  value="" />
    <br />
    <br />
    <a id="aNewUltimate" runat="server" href="" >Novi unos</a>
    <br />
    <br />
    <asp:Repeater ID="repUltimateTableGrid" runat="server">
        <HeaderTemplate>
            <table class="ultimatetable">
                <tr>
                    <th>&nbsp;</th>
                    <th>Id</th>
                    <th>Title</th>
                    <th>RegexExpression</th>
                    <th>RegexExpressionExtended</th>
                    <th>Description</th>
                    <th>ObjectTypeId</th>
                    <th>LanguageId</th>
                    <th>Accuweather</th>
                    <th>IsIgnored</th>
                    <th>Active</th>
<%--                    <th>IgnoreRegex</th>
                    <th>CurrencyId</th>
                    <th>CapturingValue</th>
                    <th>CapturingOperator</th>--%>
                    <th>&nbsp;</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr id="row-<%# Eval("Id") %>" class="<%# SelectRow(Eval("Id")) ? "selectedrow" : "row" %>">
                    <td><asp:LinkButton Text="Select" runat="server" OnCommand="UltimateGridCommand" CommandName="selectrow" CommandArgument='<%# Eval("Id") %>' /></td>
                    <td><%# Eval("Id") %></td>
                    <td><%# Eval("Title") %></td>
                    <td><%# Eval("RegexExpression") %></td>
                    <td><%# Eval("RegexExpressionExtended") %></td>
                    <td><%# Eval("Description") %></td>
                    <td><%# Eval("UltimateTableObjectType.Code") %></td>
                    <td><%# Eval("LanguageId") %></td>
                    <td><%# Eval("Accuweather") %></td>
                    <td><%# Eval("IsIgnored") %></td>
                    <td><%# Eval("Active") %></td>
<%--                    <td><%# Eval("IgnoreRegex") %></td>
                    <td><%# Eval("CurrencyId") %></td>
                    <td><%# Eval("CapturingValue") %></td>
                    <td><%# Eval("CapturingOperator")%></td>--%>
                    <td><asp:LinkButton Text="Delete" runat="server" OnCommand="UltimateGridCommand" CommandName="deleterow" CommandArgument='<%# Eval("Id") %>' /></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:PlaceHolder ID="plhUltimateGridPaging" runat="server"></asp:PlaceHolder> 

    <br />
    <br />
    <asp:PlaceHolder ID="plhSelectedUltimateTable" runat="server">

        <div id="editallwraper">

            <asp:MultiView ID="mvwTitle" ActiveViewIndex="0" runat="server">
                <asp:View ID="vwEdit" runat="server" >
                    <h2>Uređivanje</h2>
                    <h3>Id: <asp:Literal ID="ltlId" Text="" runat="server" /></h3>
                    <asp:LinkButton ID="lbtnDeleteUltimateTable" runat="server" Text="Delete" OnCommand="UltimateGridCommand" CommandName="deleterow" CommandArgument="" />
                    <br />
                </asp:View>
                <asp:View ID="vwNew" runat="server">
                    <h2>Novi unos</h2>
                </asp:View>
            </asp:MultiView>
        
            <div id="ultimatetable" style="float:left;">
                <h3>Title</h3>
                <asp:TextBox ID="tbxTitle" runat="server" Text="" />
                <asp:RequiredFieldValidator runat="server"
                ErrorMessage="*" 
                ControlToValidate="tbxTitle" 
                ValidationGroup="ultimatetable" />

                <br /><br />
                <h3>ObjectType</h3>
                <asp:DropDownList ID="ddlObjectTypeId" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                InitialValue ="-1"
                ErrorMessage="*" 
                ControlToValidate="ddlObjectTypeId" 
                ValidationGroup="ultimatetable" />
                <br />
                <a href="/manage/ultimatetableobjecttypes.aspx" target="_blank">tipovi</a>
                
                <br /><br />
                <h3>Language</h3>
                <asp:DropDownList ID="ddlLanguageId" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                InitialValue ="-1"
                ErrorMessage="*" 
                ControlToValidate="ddlLanguageId" 
                ValidationGroup="ultimatetable" />
                <br />
                <a href="/manage/languages.aspx" target="_blank">jezici</a>

                <br /><br />
                <h3>RegexExpression</h3>
                <asp:TextBox ID="tbxRegexExpression" runat="server" Text="" /><button class="regexbutton" value='<%= tbxTitle.ClientID + ";" + tbxRegexExpression.ClientID + ";" + ddlLanguageId.ClientID %>' type="button" onclick="javascript:Rebind();" >Auto</button>
                <asp:RequiredFieldValidator runat="server"
                ErrorMessage="*" 
                ControlToValidate="tbxRegexExpression" 
                ValidationGroup="ultimatetable" />

                <br /><br />
                <h3>RegexExpressionExtended</h3>
                <asp:TextBox ID="tbxRegexExpressionExtended" runat="server" Text="" />

                <br /><br />
                <h3>Description</h3>
                <asp:TextBox ID="tbxDescription" runat="server" Text="" Rows="3" TextMode="MultiLine" />        


                <br /><br />
                <h3>Accuweather</h3>
                <asp:TextBox ID="tbxAccuweather" runat="server" Text="" Rows="3" TextMode="SingleLine" />

                <br /><br />
                <h3>IsIgnored</h3>
                <asp:CheckBox ID="chbxIsIgnored" runat="server" Checked="false" />

                <br /><br />
                <h3>Active</h3>
                <asp:CheckBox ID="chbxActive" runat="server" Checked="true" />

    
        <%--        <h3>IgnoreRegex</h3>
                <asp:TextBox ID="tbxIgnoreRegex" runat="server" Text="" />

                <h3>Currency</h3>
                <asp:DropDownList ID="ddlCurrencyId" runat="server">
                </asp:DropDownList>

                <h3>CapturingValue</h3>
                <asp:TextBox ID="tbxCapturingValue" runat="server" Text="" />

                <h3>CapturingOperator</h3>
                <asp:TextBox ID="tbxCapturingOperator" runat="server" Text="" />
        --%>
                <br /><br />
                <asp:Button ID="btnSubmitAll" Text="Submit" runat="server" OnCommand="SubmitChangesCommand" CommandName="" ValidationGroup="ultimatetable" />
            </div>

            <asp:PlaceHolder ID="plhTranslationsChildrenParents" runat="server" Visible="true" >          

                <div id="rightwrapper" style="float:left">

                    <div id="translations">

                        <h2>Prijevodi</h2>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                            <ContentTemplate>

                                <asp:GridView ID="grdvwTranslations" runat="server"
                                AutoGenerateColumns="false"
                                OnRowDataBound="grdvwTranslations_RowDataBound"
                                OnRowEditing="grdvwTranslations_RowEditing" 
                                OnRowCancelingEdit="grdvwTranslations_RowCancelingEdit" 
                                OnRowDeleting="grdvwTranslations_RowDeleting"
                                OnRowUpdating="grdvwTranslations_RowUpdating"
                                DataKeyNames="Id" CssClass="relationstable">
                                    <Columns>
                                        <asp:CommandField ShowEditButton="True" ValidationGroup="translationedit" />
                                        <asp:TemplateField HeaderText="LanguageId" SortExpression="Language.Title" >
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlTransLang" runat="server" Enabled="false" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%# Eval("Language.Title") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Title" SortExpression="Title">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbxTransTitle" runat="server" Text='<%# Bind("Title") %>' />
                                                <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="tbxTransTitle" ValidationGroup="translationedit" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%# Eval("Title") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Regex" SortExpression="Regex">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbxTransRegex" runat="server" Text='<%# Bind("Regex") %>' /><button id="btnEdtTransAuto" runat="server" class="regexbutton" value="" type="button"  >Auto</button>
                                                <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="tbxTransRegex" ValidationGroup="translationedit" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%# Eval("Regex") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active" SortExpression="Active">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chbxTransActive" runat="server" Checked='<%# Bind("Active") %>' />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chbx1" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowDeleteButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Button ID="btnInsertTranslation" runat="server" Text="New" OnCommand="grdvwTranslations_Insert" CommandName="showinsert" />

                                <asp:PlaceHolder ID="plhInsertTranslation" runat="server" Visible="false">
                        
                                    <br /><br />
                                    <h3>Language</h3>
                                    <asp:DropDownList ID="ddlNewTransLang" runat="server" />

                                    <br /><br />
                                    <h3>Title</h3>
                                    <asp:TextBox ID="tbxNewTransTitle" runat="server" Text="" />                        
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="tbxNewTransTitle" ValidationGroup="translationnew" />
                        
                                    <br /><br />
                                    <h3>Regex</h3>
                                    <asp:TextBox ID="tbxNewTransRegex" runat="server" Text="" /><button id="btnInsTransAuto" class="regexbutton" value='<%= tbxNewTransTitle.ClientID + ";" + tbxNewTransRegex.ClientID + ";" + ddlNewTransLang.ClientID %>' type="button" >Auto</button>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="tbxNewTransRegex" ValidationGroup="translationnew" />
                        
                                    <br /><br />
                                    <h3>Active</h3>
                                    <asp:CheckBox ID="chbxNewTransActive" runat="server" Checked="true" />
                        
                                    <br /><br />
                                    <asp:Button Text="Insert" runat="server" OnCommand="grdvwTranslations_Insert" CommandName="saveinsert" ValidationGroup="translationnew" />&nbsp;&nbsp;
                                    <asp:Button Text="Cancel" runat="server" OnCommand="grdvwTranslations_Insert" CommandName="cancelinsert" />

                                </asp:PlaceHolder>

                            </ContentTemplate>
                        </asp:UpdatePanel>
        
                    </div>

                    <div id="relations">
                    <asp:Panel ID="plhGeoRelations" runat="server" Visible="false">
                    
                        <div id="rel-parent">                        
                        
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>                              
                                    <asp:MultiView ID="mvwParents" ActiveViewIndex="0" runat="server">
                                        <asp:View runat="server">                                
                                            <h3>Current GEO Parent</h3>
                                            <table class="relationstable">
                                                <tr>
                                                    <th>Id</th>
                                                    <th>Title</th>
                                                    <th>Active</th>
                                                    <th></th>
                                                </tr>
                                                <tr>
                                                    <td><asp:Literal ID="ltlParentId" runat="server" Text="" /></td>
                                                    <td><a id="aParentTitle" runat="server" target="_blank" ></a></td>
                                                    <td><asp:Literal ID="ltlParentActive" runat="server" Text="" /></td>
                                                    <td><asp:LinkButton ID="lbtnRemoveParent" runat="server" Text="Remove" OnCommand="SetRemoveParentCommand" CommandName="removeparent" CommandArgument="" /></td>
                                                </tr>
                                            </table>
                                   
                                        </asp:View>
                                        <asp:View runat="server">

                                            <h3>Set GEO Parent</h3>
                                            <asp:TextBox ID="tbxSetNewParentTitle" runat="server" Text="" />
                                            <asp:HiddenField ID="hfSetNewParentId" runat="server" Value="" />
                                            <asp:Button ID="btnSetNewParentSubmit" runat="server" Text="Submit" OnCommand="SetRemoveParentCommand" CommandName="setnewparent" />


                                        </asp:View>
                                    </asp:MultiView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    
                        </div>

                        <br />

                        <div id="rel-children">
                        
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                <ContentTemplate>

                                    <h3>Current GEO children:</h3>                        
                                    <asp:MultiView ID="mvwChildren" ActiveViewIndex="0" runat="server">
                                        <asp:View ID="View1" runat="server">
                                            <asp:Repeater ID="repChildren" runat="server" Visible="true">
                                                <HeaderTemplate>
                                                    <table class="relationstable">
                                                        <tr>
                                                            <th>Id</th>
                                                            <th>Title</th>
                                                            <th>Type</th>
                                                            <th>Active</th>
                                                            <th></th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Id") %></td>
                                                            <td><a href='<%# Eval("Link") %>' target="_blank"><%# Eval("Title") %></a></td>
                                                            <td><%# Eval("ObjectType") %></td>
                                                            <td><%# Eval("Active") %></td>
                                                            <td><asp:LinkButton Text="Remove" runat="server" OnCommand="SetRemoveChildCommand" CommandName="removeparent" CommandArgument='<%# Eval("Id") %>' /></td>
                                                        </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:View>
                                        <asp:View ID="View2" runat="server">
                                            <p>No GEO children</p>
                                        </asp:View>
                                    </asp:MultiView>

                                    <h3>Add GEO Child</h3>
                                    <asp:TextBox ID="tbxSetNewChildTitle" runat="server" Text="" />
                                    <asp:HiddenField ID="hfSetNewChildId" runat="server" Value="" />
                                    <asp:Button ID="btnSetNewChildSubmit" runat="server" Text="Submit" OnCommand="SetRemoveChildCommand" CommandName="setnewchild" />

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>

                    </asp:Panel>
                    </div>

                    <br /><br />
                    
                    <br />
                    <h3>Importanje relationsa s nekog drugog pojma</h3>
                    <asp:HiddenField ID="hfImportRelations" runat="server" Value="" />
                    <input id="tbxImportRelations" type="text" name="name" value="" />&nbsp;&nbsp;<asp:Button ID="btnImportRelations" Text="Import" runat="server" OnClientClick="return ConfirmImportRelations();" OnClick="ImportRelations" />
                    <div id="relations2">
                    
                        <div id="rel2-parents">
                        
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                <ContentTemplate>

                                    <h3>Other Parents:</h3>                        
                                    <asp:MultiView ID="mvwNotGeoParents" ActiveViewIndex="0" runat="server">
                                        <asp:View ID="View3" runat="server">
                                            <asp:Repeater ID="repNotGeoParents" runat="server" Visible="true">
                                                <HeaderTemplate>
                                                    <table class="relationstable">
                                                        <tr>
                                                            <th>Id</th>
                                                            <th>Title</th>
                                                            <th>Type</th>
                                                            <th>Active</th>
                                                            <th></th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Id") %></td>
                                                            <td><a href='<%# Eval("Link") %>' target="_blank"><%# Eval("Title") %></a></td>
                                                            <td><%# Eval("ObjectType") %></td>
                                                            <td><%# Eval("Active") %></td>
                                                            <td><asp:LinkButton Text="Remove" runat="server" OnCommand="SetRemoveNotGeoParentCommand" CommandName="removeparent" CommandArgument='<%# Eval("Id") %>' /></td>
                                                        </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:View>
                                        <asp:View ID="View4" runat="server">
                                            <p>No parents</p>
                                        </asp:View>
                                    </asp:MultiView>

                                    <h3>Add Parent</h3>
                                    <asp:TextBox ID="tbxSetNewNotGeoParentTitle" runat="server" Text="" />
                                    <asp:HiddenField ID="hfSetNewNotGeoParentId" runat="server" Value="" />
                                    <asp:Button ID="btnSetNewNotGeoParentSubmit" runat="server" Text="Submit" OnCommand="SetRemoveNotGeoParentCommand" CommandName="setnewparent" />

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>

                        <br />

                        <div id="rel2-children">
                        
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                <ContentTemplate>

                                    <h3>Other children:</h3>                        
                                    <asp:MultiView ID="mvwNotGeoChildren" ActiveViewIndex="0" runat="server">
                                        <asp:View ID="View5" runat="server">
                                            <asp:Repeater ID="repNotGeoChildren" runat="server" Visible="true">
                                                <HeaderTemplate>
                                                    <table class="relationstable">
                                                        <tr>
                                                            <th>Id</th>
                                                            <th>Title</th>
                                                            <th>Type</th>
                                                            <th>Active</th>
                                                            <th></th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Id") %></td>
                                                            <td><a href='<%# Eval("Link") %>' target="_blank"><%# Eval("Title") %></a></td>
                                                            <td><%# Eval("ObjectType") %></td>
                                                            <td><%# Eval("Active") %></td>
                                                            <td><asp:LinkButton Text="Remove" runat="server" OnCommand="SetRemoveNotGeoChildCommand" CommandName="removechild" CommandArgument='<%# Eval("Id") %>' /></td>
                                                        </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:View>
                                        <asp:View ID="View6" runat="server">
                                            <p>No children</p>
                                        </asp:View>
                                    </asp:MultiView>

                                    <h3>Add Child</h3>
                                    <asp:TextBox ID="tbxSetNewNotGeoChildTitle" runat="server" Text="" />
                                    <asp:HiddenField ID="hfSetNewNotGeoChildId" runat="server" Value="" />
                                    <asp:Button ID="btnSetNewNotGeoChildSubmit" runat="server" Text="Submit" OnCommand="SetRemoveNotGeoChildCommand" CommandName="setnewchild" />

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>

                    </div>
            
                </div>
            </asp:PlaceHolder>
            <div style="clear:both;"></div>
        
        </div>
    </asp:PlaceHolder>
    
    <br />


</asp:Content>

