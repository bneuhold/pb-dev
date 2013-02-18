<%@ Page Title="" Language="C#" MasterPageFile="~/MasterManage.master" AutoEventWireup="true" CodeFile="Agency.aspx.cs" Inherits="Agency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

            var autocomplmaxresults = 20;

            $("#<%= tbxCountry.ClientID %>").autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/services/PlaceInteractionService.asmx/SuggestCountry",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'maxresults': '" + autocomplmaxresults + "' }",
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
                    //nista
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

            $("#<%= tbxCity.ClientID %>").autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/services/PlaceInteractionService.asmx/SuggestCity",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        data: "{ 'term': '" + request.term + "', 'maxresults': '" + autocomplmaxresults + "' }",
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
                    //nista
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });

        });
    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <asp:MultiView ID="mvwContainer" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwRead" runat="server" EnableViewState="false">
            <div class="header-bar"><asp:Literal ID="ltlAgency" runat="server">Agencija</asp:Literal></div>
            
            <div class="in-content top-m">         
            <a href="" id="aEdit" class="button edit" runat="server">Uređivanje</a>
            <br />
            <img id="imgLogoR" runat="server" src="" alt="" />
            <br />

            <fieldset class="editor basic-info ui-corner-all">
                <legend>Podaci o korisniku</legend>
                <div class="form-editor">
                    <div class="form-row">
                        <div class="form-cell label">Tip</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlPrivate" runat="server">Agencija/Privatna osoba</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Država</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlCountry" runat="server">Drzava</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Grad</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlCity" runat="server">Grad</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Ulica</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlStreet" runat="server">Ulica</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Telefon</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlContactPhone" runat="server">099 555 6666</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">E-mail</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlEmail" runat="server">email@email.com</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Web stranica</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlUrlWebsite" runat="server">www.website.com</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Jezik</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlLanguage" runat="server">Language</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                </div>
            </fieldset>
            <h2>Smještaji</h2>
            <asp:Repeater ID="repAccommodation" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                        <li>
                            <span><a href="<%# "/manage/Accommodation.aspx?accommid=" + Eval("Id") %>"><%# Eval("Name") %></a></span> 
                        </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>

            <br />
            <a href="" id="aNewAccommodation" runat="server">Dodaj smještaj</a>
            <br />
            <br />
            <a href="" id="aBoking" runat="server">Pregled rezervacija</a>
            <br />

            </div>

        </asp:View>

        <asp:View ID="vwNewEdit" runat="server">

            <h1 id="hAgency" runat="server">Uređivanje agencije|Nova agencija</h1>

            <p>Naziv agencije ili ime privatne osobe</p>
        
            <asp:TextBox ID="tbxName" runat="server" CssClass="unos" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxName" ID="rfvName" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>
            <asp:CustomValidator ControlToValidate="tbxName" ID="cstvName" runat="server" 
                CssClass="validator" ErrorMessage="Već postoji agencija s tim imenom" 
                ValidationGroup="sve" onservervalidate="cstvName_ServerValidate" ></asp:CustomValidator>

            <asp:RadioButtonList ID="rbtnList" runat="server">
                <asp:ListItem Value="a" Selected="True">Agencija</asp:ListItem>
                <asp:ListItem Value="p" Selected="False">Privatna osoba</asp:ListItem>
            </asp:RadioButtonList>

            <p>Država</p>    
    
            <asp:TextBox ID="tbxCountry" runat="server" CssClass="unos" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxCountry" ID="rfvCountry" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>
            
            <p>Grad</p>    
    
            <asp:TextBox ID="tbxCity" runat="server" CssClass="unos" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxCity" ID="rfvCity" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>
            
            <p>Ulica i broj</p>    
    
            <asp:TextBox ID="tbxStreet" runat="server" CssClass="unos" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxStreet" ID="rfvStreet" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>
    
            <p>Telefonski broj</p>
    
            <asp:TextBox ID="tbxContactPhone" runat="server" CssClass="unos" ToolTip="unos više brojeva(odvaja se zarezom): +385 91 777 8888, 091 777 8888" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxContactPhone" ID="rfvContactPhone" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ControlToValidate="tbxContactPhone" ID="revContactPhone" runat="server" CssClass="validator" ErrorMessage="Neispravan broj telefona" ValidationExpression="(\+?0*[1-9]{0,10} ?[0-9]{2,10}( [0-9]{1,10})*(, )?)+" ValidationGroup="sve" />

            <p>Email</p>
    
            <asp:TextBox ID="tbxContactEmail" runat="server" CssClass="unos" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxContactEmail" ID="rfvContactEmail" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator  ControlToValidate="tbxContactEmail" ID="revContactEmail" runat="server" CssClass="validator" ErrorMessage="Neispravna e-mail adresa" ValidationExpression="[\w_.\-+]{4,}@[\w_\-+]{2,}([\w_\-+]+.?)*\.[\w]{2,3}" ValidationGroup="sve" />


            <p>Web stranica (ako postoji)</p>
    
            <asp:TextBox ID="tbxUrlWebsite" runat="server" CssClass="unos" ></asp:TextBox>
            <asp:RegularExpressionValidator ControlToValidate="tbxUrlWebsite" ID="revUrlWebsite" runat="server" CssClass="validator" ErrorMessage="Neispravni URL oblik" ValidationExpression="(http://)?\w+?([\w_\-+]+\.?)*\.\w{2,3}(/[\w_\-+]+?.*)?" ValidationGroup="sve" />

            <p>Jezična postavka</p>

            <asp:DropDownList ID="ddlLanguage" runat="server" DataSourceID="ldsLanguage" 
                DataTextField="Title" DataValueField="Id">
            </asp:DropDownList>
            <asp:LinqDataSource ID="ldsLanguage" runat="server" 
                ContextTypeName="UltimateDC.UltimateDataContext" OrderBy="Title" 
                Select="new (Id, Title)" TableName="Languages">
            </asp:LinqDataSource>


            <p>Opis</p>
    
            <asp:TextBox ID="tbxDescription" runat="server" CssClass="unos" Rows="8" TextMode="MultiLine" ></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="tbxDescription" ID="revDescription" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>

            <p>Logo</p>
            <img id="imgLogoE" runat="server" alt="" src="" visible="false" />
            <asp:FileUpload ID="fuLogo" runat="server" />
            <asp:RequiredFieldValidator  ControlToValidate="fuLogo" ID="rfvUrlLogo" runat="server" CssClass="validator" ErrorMessage="*" ValidationGroup="sve" ></asp:RequiredFieldValidator>

            <br />
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Spremi promjene" oncommand="btnSave_Command" CommandName="" />
            <br />

        </asp:View>
    </asp:MultiView>
            
    <br />
    <asp:Literal ID="ltlStatus" runat="server" EnableViewState="false"></asp:Literal>
    <br />
    <br />

    <a id="aAgencyPage" href="" runat="server" target="_blank">Pregled stranice agencije</a>
    <br />
    <br />


    <a href="/manage/Customer.aspx" class="customer-link button-header-back ui-corner-all">korisničke stranice</a>

</asp:Content>

