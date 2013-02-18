<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="Accommodation.aspx.cs" Inherits="manage_Accommodation"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="/resources/scripts/jcrop/jquery.Jcrop.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jcrop/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    
    <script type="text/javascript">

        //datepicker i jCrop
        $(function () {
            var dates = $("#<%= tbxPriceEditStart.ClientID %>, #<%= tbxPriceEditEnd.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                numberOfMonths: 1,
                dateFormat: "dd.mm.yy.",
                onSelect: function (selectedDate) {
                    var option = this.id == "<%= tbxPriceEditStart.ClientID %>" ? "minDate" : "maxDate",
				    instance = $(this).data("datepicker"),
				    date = $.datepicker.parseDate(
					    instance.settings.dateFormat ||
					    $.datepicker._defaults.dateFormat,
					    selectedDate, instance.settings);
                    dates.not(this).datepicker("option", option, date);
                }
            });

            $("#<%= imgThumb.ClientID %>").Jcrop({
                onChange: showCoords,
                onSelect: showCoords,
                addClass: 'jcrop-dark',
                setSelect: [0 , 0, 2000, 2000]
            });
        });

        function showCoords(c) {
            var obj = $("#<%= hfCrop.ClientID %>");
            obj.val(c.x + "," + c.y + "," + c.w + "," + c.h);
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h1><a id="aAccomm" runat="server" href="">Naziv smještaja</a></h1>


    <asp:MultiView ID="mvwAccommodation" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwRead" runat="server" EnableViewState="false">
            
            <h2><asp:Literal ID="ltlAgency" runat="server">Naziv agencije</asp:Literal></h2>
            
            <a id="aEdit" runat="server" href="">Uređivanje</a>&nbsp;&nbsp;|&nbsp;&nbsp;
            <a id="aDelete" runat="server" onserverclick="aDelete_Click">Brisanje</a>
            <br />

            <asp:Literal ID="ltlType" runat="server">Tip smještaja</asp:Literal>
            <br />

            <asp:Literal ID="ltlCityRegionCountry" runat="server">Grad, Podregija, Regija, Država</asp:Literal>
            <br />

            <br />
            <a id="aImages" runat="server" href="">Dodaj ili uredi slike</a>
            <br />
            <a id="aPrices" runat="server" href="">Dodaj ili uredi cijenike</a>
            <br />

            <br />
            <asp:Button ID="btnPublishAccomAdvert" Text="Publish" runat="server" 
                onclick="btnPublishAccomAdvert_Click" />
    
        </asp:View>

        <asp:View ID="vwNewEdit" runat="server">
            <h1 runat="server" id="hNewEdit">Uređivanje smještaja|Unos smještaja</h1>
            <br /><br />
            <div>                
                Naziv
                <asp:TextBox ID="tbxName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="revName" runat="server" 
                    ControlToValidate="tbxName" 
                    ErrorMessage="*" 
                    ValidationGroup="newedit" />
                
                <br /><br />
                Tip smještaja
                <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="revType" runat="server" 
                ControlToValidate="ddlType" 
                ErrorMessage="*"
                ValidationGroup="newedit" 
                InitialValue="-1" />


                <br /><br />
                <asp:Literal ID="ltlCountry" Text="Država" runat="server" /> 
                <asp:DropDownList ID="ddlCountry" runat="server" 
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server"
                ErrorMessage="*" 
                ControlToValidate="ddlCountry"
                ValidationGroup="newedit"  
                InitialValue="-1" />

                <br /><br />

                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:MultiView ID="mvwCityEntry" runat="server" ActiveViewIndex="0">

                    <asp:View ID="vwCityAuto" runat="server">
                        Grad
                        <asp:DropDownList ID="ddlCityRegion" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server"
                        ErrorMessage="*" 
                        ControlToValidate="ddlCityRegion"
                        ValidationGroup="autocity" 
                        InitialValue="-1" />

                        <br />
                    
                        Grada nema na popisu?
                        <asp:LinkButton ID="lbtnManualEntry" runat="server" onclick="lbtnManualEntry_Click">Unesite ga!</asp:LinkButton>                    

                    </asp:View>

                    <asp:View ID="vwCityManual" runat="server">

                        <table>
                            <tr>
                                <td>
                                    Region:
                                </td>
                                <td>
                                    <asp:ComboBox ID="cbxRegion" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="true" 
                                    OnTextChanged="cbxRegion_TextChanged"></asp:ComboBox>
                                    <asp:RequiredFieldValidator ID="revRegion" runat="server" 
                                    ControlToValidate="cbxRegion" 
                                    ErrorMessage="*"
                                    ValidationGroup="manualcity"
                                    InitialValue="-1" />                                
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Subregion:
                                </td>
                                <td>
                                    <asp:ComboBox ID="cbxSubregion" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="true" 
                                    OnTextChanged="cbxSubregion_TextChanged">
                                    </asp:ComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Island:
                                </td>
                                <td>
                                    <asp:ComboBox ID="cbxIsland" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="true" 
                                    OnTextChanged="cbxIsland_TextChanged">
                                    </asp:ComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    City:
                                </td>
                                <td>
                                    <asp:ComboBox ID="cbxCity" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="false" 
                                    OnTextChanged="cbxCity_TextChanged">
                                    </asp:ComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="cbxCity" 
                                    ErrorMessage="*"
                                    ValidationGroup="manualcity"
                                    InitialValue="-1" />
                                </td>
                            </tr>
                        </table>

                    </asp:View>
                </asp:MultiView>
            
                <br /><br />

                <h3>Kapacitet</h3>
                <asp:TextBox ID="tbxCapacity" runat="server" ToolTip="primjer unosa: 4, 4+1, 4-5" />
                    <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="tbxCapacity" 
                    ErrorMessage="*" 
                    ValidationGroup="newedit" />
                    <asp:RegularExpressionValidator runat="server" 
                    ControlToValidate="tbxCapacity"
                    ErrorMessage="Neispravan unos! Unjeti broj osoba, npr: '5' ili '5-6' ili '5+1'"
                    ValidationExpression="^\s*([1-9][0-9]*|[1-9][0-9]*\s*[-+]\s*[1-9][0-9]*)\s*$"
                    />

                <br /><br />

                <h3>Adresa</h3>
                <asp:TextBox ID="tbxAdress" runat="server" />
                    <asp:RequiredFieldValidator runat="server" 
                            ControlToValidate="tbxAdress" 
                            ErrorMessage="*"
                            ValidationGroup="newedit">
                    </asp:RequiredFieldValidator>


                <h3>Kućni ljubimci</h3>
                <asp:CheckBox ID="chbxPets" Text="Dozvoljeni" runat="server" Checked="false" />

                <br /><br />

                <asp:MultiView ID="mvwDescriptions" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwNewDescription" runat="server">                   

                        <h3>Opis na jeziku</h3>
                        <asp:RadioButton ID="rbtnDefaultDescLanguage" runat="server" />
                        <asp:TextBox ID="tbxNewDefaultLangDescription" runat="server" TextMode="MultiLine" />
                        <asp:RequiredFieldValidator ID="revNewDescription" runat="server" 
                                    ControlToValidate="tbxNewDefaultLangDescription" 
                                    ErrorMessage="*"
                                    ValidationGroup="newedit">
                        </asp:RequiredFieldValidator>

    <%--                    <asp:RadioButtonList ID="rblLanguages" runat="server" 
                            DataSourceID="ldsLanguages" DataTextField="Title" DataValueField="Id">
                        </asp:RadioButtonList>

                        <asp:LinqDataSource ID="ldsLanguages" runat="server" 
                            ContextTypeName="UltimateDC.UltimateDataContext" OrderBy="Title" 
                            Select="new (Id, Title)" TableName="Languages">
                        </asp:LinqDataSource>--%>

                    </asp:View>
                    <asp:View ID="vwEditDescription" runat="server">
                    
                        <h2>Opisi</h2>

                        <asp:Repeater ID="repDescriptions" runat="server">
                            <ItemTemplate>
                                <h3><%# Eval("Language.Title")%></h3>
                                <asp:TextBox runat="server" Text='<%# Eval("Description") %>' />
                                <asp:LinkButton runat="server" 
                                OnCommand="lbtnDeleteDescription_Command"
                                CommandName="deletedescription" 
                                CommandArgument='<%# Eval("AccommodationId") + "," + Eval("LanguageId")%>'
                                Text="Briši">
                                </asp:LinkButton>                      
                            </ItemTemplate>
                        </asp:Repeater>

                        <p>
                            Dodaj opis na drugom jeziku 
                            <asp:DropDownList ID= "ddlAddDescLangs" runat="server">
                            </asp:DropDownList>

                            <asp:TextBox ID="tbxAddDescription" runat="server" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvAddDescription" runat="server" 
                            ControlToValidate="tbxAddDescription" 
                            ErrorMessage="*"
                            ValidationGroup="adddescription">
                            </asp:RequiredFieldValidator>

                            <asp:Button ID="btnAddDescription" Text="Dodaj" runat="server" 
                                onclick="btnAddDescription_Click" ValidationGroup="adddescription" />
                        </p>


                    </asp:View>
                </asp:MultiView>

            </div>
            <div style="float:left;">
                <div style="width:500px; height:400px;">
                    <div id="map_canvas" style="width:100%; height:100%;">
                        <!--guglj mep-->
                    </div>
                </div>
            </div>
            <div style="clear:both"></div>

            <br /><br />
            <asp:Button ID="btnSave" runat="server" Text="Spremi promjene"  CommandName="" 
                        oncommand="btnSave_Command" ValidationGroup="newedit" />

        </asp:View>

        <asp:View ID="vwImages" runat="server">
        <h1>Slike smještaja</h1>
        <h2>Dodaj sliku</h2>
            <h3>Naziv</h3>
            <asp:TextBox ID="tbxAddAccomImageTitle" runat="server" Text="" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="tbxAddAccomImageTitle" runat="server" ValidationGroup="uploadimage" />
            <h3>Opis</h3>
            <asp:TextBox ID="tbxAddAccomImageDescription" runat="server" Text="" />
            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="tbxAddAccomImageDescription" runat="server" ValidationGroup="uploadimage" />
            <h3>Odaberi sliku</h3>
            <asp:FileUpload ID="fuAddAccommImage" runat="server" />
            <asp:Button ID="btnUploadAccommImagePreview" Text="Upload" runat="server" 
                onclick="btnUploadAccommImagePreview_Click" />
            <asp:RequiredFieldValidator ID="rfvAddAccommImage" runat="server" ErrorMessage="*" ControlToValidate="fuAddAccommImage" ValidationGroup="uploadimage" />
            <div id="preview">
                <img id="imgThumb"  src="" alt="" runat="server" visible="false" />
                <asp:HiddenField ID="hfCrop" runat="server" Value="" />
            </div>
            
            <br />

            <asp:Button ID="btnUploadAccommImage" Text="Submit" runat="server" 
               onclick="btnUploadAccommImage_Click" ValidationGroup="uploadimage" />
            <br />
            <br />
            <asp:Repeater ID="repAccommodationImages" runat="server">
                <HeaderTemplate>
                    <h2>Pregled slika</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <div id="<%# "img_" + Eval("Id") %>">
                        <img src="<%# Eval("Src") %>" alt="<%# Eval("Alt") %>" title="<%# Eval("Title") %>" longdesc="<%# Eval("Description") %>" />
                        <asp:LinkButton runat="server"
                            Text="Uredi"
                            CommandName="imageedit"
                            CommandArgument='<%# Eval("Id") %>'
                            OnCommand="lbtnEditDeleteImage_Command" >
                        </asp:LinkButton>
                        &nbsp;|&nbsp                         
                        <asp:LinkButton runat="server"
                            Text="Briši"
                            CommandName="imagedelete"
                            CommandArgument='<%# Eval("Id") %>'
                            OnCommand="lbtnEditDeleteImage_Command" >
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
                <SeparatorTemplate>
                    <br />
                    <br />
                </SeparatorTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlImageEdit" runat="server">

                <h3>Naslov slike</h3>
                <asp:TextBox ID="tbxImageEditTitle" runat="server" />
                <asp:RequiredFieldValidator ID="rfvImageEditTitle" runat="server" 
                ErrorMessage="*" 
                ControlToValidate="tbxImageEditTitle" 
                ValidationGroup="imageeditsave" />

                <h3>Opis slike</h3>
                <asp:TextBox ID="tbxImageEditDescription" runat="server" />
                <asp:RequiredFieldValidator ID="rfvImageEditDescription" runat="server" 
                ErrorMessage="*" 
                ControlToValidate="tbxImageEditDescription" 
                ValidationGroup="imageeditsave" />
                <br /><br />
                <asp:Button ID="btnImageEditSave" Text="Spremi izmjene" runat="server" 
                    oncommand="btnImageEditSave_Command" CommandName="imageeditsave" ValidationGroup="imageeditsave" />
            </asp:Panel>

        </asp:View>

        <asp:View ID="vwPrices" runat="server">
            
          <h1>Cijenici</h1>

            <asp:Repeater ID="repPrices" runat="server">
                <ItemTemplate>
                    <p>
                        <asp:LinkButton runat="server"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CommandName="priceedit"
                                        OnCommand="PriceEdit_Command" >
                        <%# String.Format("{0}, {1}, {2}{3}", FormatDate((DateTime?)Eval("DateStart"), (DateTime?)Eval("DateEnd")), Eval("Name"), ((decimal)Eval("Value")).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture), Eval("Currency.Symbol"))%>
                        </asp:LinkButton>
                    </p>
                </ItemTemplate>
                <SeparatorTemplate>
                </SeparatorTemplate>
            </asp:Repeater>
            <br />
            <asp:LinkButton 
                runat="server"
                CommandName="pricenew"
                CommandArgument=""
                OnCommand="PriceEdit_Command">
                Unos cijenika
            </asp:LinkButton>
            
            
            <br />
            <br />
            <asp:Panel ID="pnlPriceEdit" runat="server">
                <h2 id="hPriceEdit" runat="server">Uredi|Unesi</h2>
                <h3>Naziv</h3>
                <asp:TextBox ID="tbxPriceEditName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvPriceEditName" runat="server" 
                ErrorMessage="*" 
                ControlToValidate="tbxPriceEditName" 
                ValidationGroup="priceedit" />
                
                <h3>Razdoblje</h3>
                <div id="priceperiod">
                    <input id="rbtnPriceCalendar" runat="server" type="radio" checked="true" name="priceperiod" value="specificperiod" title="Odabir na kalendaru" />Odaberi datume
                    <br />
                    <div id="enterdates">                    
                        <label for="<%= tbxPriceEditStart.ClientID %>">Od</label>
                        <asp:TextBox ID="tbxPriceEditStart" runat="server" />
                        <asp:RequiredFieldValidator runat="server" 
                        ErrorMessage="*" 
                        ControlToValidate="tbxPriceEditStart" 
                        ValidationGroup="priceeditcal" />
                        <label for="<%= tbxPriceEditEnd.ClientID %>">do</label>
                        <asp:TextBox ID="tbxPriceEditEnd" runat="server" />
                        <asp:RequiredFieldValidator runat="server" 
                        ErrorMessage="*" 
                        ControlToValidate="tbxPriceEditEnd" 
                        ValidationGroup="priceeditcal" />
                    </div>
                    <input id="rbtnPriceWholeYear" runat="server" type="radio" name="priceperiod" value="wholeyear" title="Cijela godina" />Cijela godina
                </div>

                <script type="text/javascript">
                    $("#priceperiod input[type=radio]").bind("change", function () {
                        var value = $(this).val();
                        if (value == "wholeyear") {
                            $("#enterdates").hide();
                        }
                        else if (value == "specificperiod") {
                            $("#enterdates").show();
                        }
                    });
                </script>

                <h3>Cijena</h3>
                <asp:TextBox ID="tbxPriceEditValue" runat="server" />
                <asp:DropDownList ID="ddlCurrency" runat="server" >
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvPriceEditValue" runat="server" 
                ErrorMessage="*" 
                ControlToValidate="tbxPriceEditValue" 
                ValidationGroup="priceedit" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                ValidationExpression="[0-9]+(\.[0-9]{1,2})?"
                ErrorMessage="npr. 199.00" 
                ControlToValidate="tbxPriceEditValue" 
                ValidationGroup="priceedit" />

                <br />
                <br />

                <asp:Button ID="btnPriceEditDelete" Text="Briši" runat="server" CommandName="pricedelete" OnCommand="PriceSubmit_Command" ValidationGroup="priceedit"  />
                <asp:Button ID="btnPriceEditSave" Text="Submit" runat="server" CommandName="pricesaveedit|pricesavenew" OnCommand="PriceSubmit_Command" ValidationGroup="priceedit" />
            </asp:Panel>

        </asp:View>
    </asp:MultiView>


    <br />
    <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
    <br />
    <br />
    Povratak na <a id="aBackToAgency" runat="server" href="">stranicu agencije</a>
    
</asp:Content>

