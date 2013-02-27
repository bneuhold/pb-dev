<%@ Page Title="" Language="C#" MasterPageFile="~/MasterManage.master" AutoEventWireup="true" CodeFile="Accommodation.aspx.cs" Inherits="manage_Accommodation"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="/resources/scripts/jcrop/jquery.Jcrop.min.js" type="text/javascript"></script>
    <link href="/resources/scripts/jcrop/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <link href="/resources/scripts/jqueriui/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="/resources/scripts/fancybox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.easing-1.3.pack.js" type="text/javascript"></script>
    <script src="/resources/scripts/fancybox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="/resources/scripts/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />

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

            $("#priceperiod input[type=radio]").bind("change", function () {
                var value = $(this).val();
                if (value == "wholeyear") {
                    $("#enterdates").hide();
                }
                else if (value == "specificperiod") {
                    $("#enterdates").show();
                }
            });

            $("a.gallery-link").fancybox({
                'opacity': true,
                'overlayShow': true,
                'overlayOpacity': 0.7,
                //'overlayColor' : '#666'
                'transitionIn': 'elastic',
                'transitionOut': 'fade'
            });
        });

        function showCoords(c) {
            var obj = $("#<%= hfCrop.ClientID %>");
            obj.val(c.x + "," + c.y + "," + c.w + "," + c.h);
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="header-bar">Smještaj: <a id="aAccomm" runat="server" href="">Naziv smještaja</a></div>
    <div class="in-content top-m">  
    <asp:MultiView ID="mvwAccommodation" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwRead" runat="server" EnableViewState="false">
            
            <fieldset class="editor basic-info ui-corner-all to-left">
                <legend>Podaci o smještaju</legend>
                <div class="form-editor">
                    <div class="form-row">
                        <div class="form-cell label">Agencija</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlAgency" runat="server">Naziv agencije</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Tip smještaja</div>
                        <div class="form-cell value">
                            <asp:Literal ID="ltlType" runat="server">Tip smještaja</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Lokacija</div>
                        <div class="form-cell value">
                           <asp:Literal ID="ltlCityRegionCountry" runat="server">Grad, Podregija, Regija, Država</asp:Literal>
                        </div>
                        <div class="close-row"></div>
                    </div>
                </div>
                <div class="action-bar">
                    <a id="aEdit" runat="server"  class="edit-button button" href="">Uređivanje</a>

                    <a id="aDelete" runat="server" class="delete-button button" onserverclick="aDelete_Click">Brisanje</a>
                </div>
            </fieldset>
            
            <div class="action-bar on-right">
                <asp:LinkButton ID="btnPublishAccomAdvert" Text="Publish" runat="server" CssClass="save-button button"
                onclick="btnPublishAccomAdvert_Click" />
                <a id="aImages" runat="server" class="button" href="">Dodaj ili uredi slike</a>
                <a id="aPrices" runat="server" class="button" href="">Dodaj ili uredi cjenike</a>
            </div>
            
    
        </asp:View>

        <asp:View ID="vwNewEdit" runat="server">
            
            
            <fieldset class="editor basic-info ui-corner-all to-left">
                <legend runat="server" id="hNewEdit">Uređivanje smještaja|Unos smještaja</legend>
                <div class="form-editor">
                    <div class="form-row">
                        <div class="form-cell label">Naziv</div>
                        <div class="form-cell value">
                            <asp:TextBox ID="tbxName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revName" runat="server" 
                            ControlToValidate="tbxName" 
                            ErrorMessage="*" 
                            ValidationGroup="newedit" />
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Tip smještaja</div>
                        <div class="form-cell value">
                            <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="revType" runat="server" 
                            ControlToValidate="ddlType" 
                            ErrorMessage="*"
                            ValidationGroup="newedit" 
                            InitialValue="-1" />
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">
                            <asp:Literal ID="ltlCountry" Text="Država" runat="server" /> 
                        </div>
                        <div class="form-cell value">
                            <asp:DropDownList ID="ddlCountry" runat="server" 
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ErrorMessage="*" 
                                ControlToValidate="ddlCountry"
                                ValidationGroup="newedit"  
                                InitialValue="-1" />
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">
                        
                        
                        </div>
                        <div class="form-cell value">
                            
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Kapacitet</div>
                        <div class="form-cell value">
                            <asp:TextBox ID="tbxCapacity" runat="server" ToolTip="primjer unosa: 4, 4+1, 4-5" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="tbxCapacity" 
                            ErrorMessage="*" 
                            ValidationGroup="newedit" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ControlToValidate="tbxCapacity"
                            ErrorMessage="Neispravan unos! Unjeti broj osoba, npr: '5' ili '5-6' ili '5+1'"
                            ValidationExpression="^\s*([1-9][0-9]*|[1-9][0-9]*\s*[-+]\s*[1-9][0-9]*)\s*$"
                            />
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Adresa</div>
                        <div class="form-cell value">
                            <asp:TextBox ID="tbxAdress" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="tbxAdress" 
                                    ErrorMessage="*"
                                    ValidationGroup="newedit">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label"></div>
                        <div class="form-cell value">
                            
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Kućni ljubimci</div>
                        <div class="form-cell value">
                            <asp:CheckBox ID="chbxPets" Text="Dozvoljeni" runat="server" Checked="false" />
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label"></div>
                        <div class="form-cell value">
                            
                        </div>
                        <div class="close-row"></div>
                    </div>

                


                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:MultiView ID="mvwCityEntry" runat="server" ActiveViewIndex="0">

                    <asp:View ID="vwCityAuto" runat="server">
                        
                        <div class="form-row">
                            <div class="form-cell label">Grad</div>
                            <div class="form-cell value">
                                <asp:DropDownList ID="ddlCityRegion" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                ErrorMessage="*" 
                                ControlToValidate="ddlCityRegion"
                                ValidationGroup="autocity" 
                                InitialValue="-1" />

                            </div>
                            <div class="close-row"></div>
                        </div>
                        
                        <div class="form-row">
                            <div class="form-cell label">
                                <div class="line-holder">
                                    <div class="line subtext">
                                       Grada nema na popisu? <asp:LinkButton ID="lbtnManualEntry" runat="server" onclick="lbtnManualEntry_Click">Unesite ga!</asp:LinkButton> 
                                    </div>
                                </div>

                            </div>
                            <div class="form-cell value">
                            </div>
                            <div class="close-row"></div>
                        </div>
                        
                        
                    </asp:View>

                    <asp:View ID="vwCityManual" runat="server">

                        <div class="form-row">
                            <div class="form-cell label">Region</div>
                            <div class="form-cell value">
                                <asp:ComboBox ID="cbxRegion" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="true" 
                                    OnTextChanged="cbxRegion_TextChanged"></asp:ComboBox>
                                    <asp:RequiredFieldValidator ID="revRegion" runat="server" 
                                    ControlToValidate="cbxRegion" 
                                    ErrorMessage="*"
                                    ValidationGroup="manualcity"
                                    InitialValue="-1" />   
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">Subregion</div>
                            <div class="form-cell value">
                                <asp:ComboBox ID="cbxSubregion" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="true" 
                                    OnTextChanged="cbxSubregion_TextChanged">
                                    </asp:ComboBox>
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">Island</div>
                            <div class="form-cell value">
                                <asp:ComboBox ID="cbxIsland" runat="server" 
                                    AutoCompleteMode="SuggestAppend" 
                                    AutoPostBack="true" 
                                    OnTextChanged="cbxIsland_TextChanged">
                                    </asp:ComboBox>
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">City</div>
                            <div class="form-cell value">
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
                            </div>
                            <div class="close-row"></div>
                        </div>

                    </asp:View>
                </asp:MultiView>
            

                <asp:MultiView ID="mvwDescriptions" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwNewDescription" runat="server">                   
                    
                        <div class="form-row">
                            <div class="form-cell label">Opis na jeziku
                            <asp:RadioButton ID="rbtnDefaultDescLanguage" runat="server" />
                            </div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxNewDefaultLangDescription" runat="server" TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ID="revNewDescription" runat="server" 
                                            ControlToValidate="tbxNewDefaultLangDescription" 
                                            ErrorMessage="*"
                                            ValidationGroup="newedit">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label"></div>
                            <div class="form-cell value">
                            
                            </div>
                            <div class="close-row"></div>
                        </div>
    <%--                    <asp:RadioButtonList ID="rblLanguages" runat="server" 
                            DataSourceID="ldsLanguages" DataTextField="Title" DataValueField="Id">
                        </asp:RadioButtonList>

                        <asp:LinqDataSource ID="ldsLanguages" runat="server" 
                            ContextTypeName="UltimateDC.UltimateDataContext" OrderBy="Title" 
                            Select="new (Id, Title)" TableName="Languages">
                        </asp:LinqDataSource>--%>

                    </asp:View>
                    <asp:View ID="vwEditDescription" runat="server">
                    
                        <div class="form-row separator">
                            <div class="form-cell label">Opisi</div>
                            <div class="form-cell value">
                            
                            </div>
                            <div class="close-row"></div>
                        </div>
                        
                        
                        

                        <asp:Repeater ID="repDescriptions" runat="server">
                            <ItemTemplate>
                                
                                <div class="form-row">
                                    <div class="form-cell label"><%# Eval("Language.Title")%></div>
                                    <div class="form-cell value">
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("Description") %>' />
                                        <asp:LinkButton ID="LinkButton1" runat="server" 
                                        OnCommand="lbtnDeleteDescription_Command"
                                        CommandName="deletedescription" 
                                        CommandArgument='<%# Eval("AccommodationId") + "," + Eval("LanguageId")%>'
                                        Text="Briši">
                                        </asp:LinkButton>    
                                    </div>
                                    <div class="close-row"></div>
                                </div>                
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="form-row">
                            <div class="form-cell label">
                                <div class="line-holder">
                                    <div class="line">
                                        Dodaj opis na drugom jeziku
                                    </div>

                                </div>
                            </div>
                            <div class="form-cell value">
                            
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">
                                <asp:DropDownList ID= "ddlAddDescLangs" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxAddDescription" runat="server" TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ID="rfvAddDescription" runat="server" 
                                ControlToValidate="tbxAddDescription" 
                                ErrorMessage="*"
                                ValidationGroup="adddescription">
                                </asp:RequiredFieldValidator>

                                <asp:LinkButton ID="btnAddDescription" Text="Dodaj" runat="server"
                                    onclick="btnAddDescription_Click" ValidationGroup="adddescription" />
                            </div>
                            <div class="close-row"></div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            
                </div>
                <div class="action-bar">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Spremi promjene"  CommandName="" CssClass="save-button button"
                        oncommand="btnSave_Command" ValidationGroup="newedit" />
                </div>
            </fieldset>

            <div style="float:left;">
                <div style="width:500px; height:400px;">
                    <div id="map_canvas" style="width:100%; height:100%;">
                        <!--guglj mep-->
                    </div>
                </div>
            </div>
            <div style="clear:both"></div>
        </asp:View>

        <asp:View ID="vwImages" runat="server">
        
            <fieldset class="editor basic-info ui-corner-all to-left">
                <legend runat="server" id="Legend1">Dodavanje slike na smještaj</legend>
                <div class="form-editor">
                    <div class="form-row">
                        <div class="form-cell label">Naziv</div>
                        <div class="form-cell value">
                            <asp:TextBox ID="tbxAddAccomImageTitle" runat="server" Text="" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="*" ControlToValidate="tbxAddAccomImageTitle" runat="server" ValidationGroup="uploadimage" />
                        </div>
                        <div class="close-row"></div>
                    </div>  
                    <div class="form-row">
                        <div class="form-cell label">Opis</div>
                        <div class="form-cell value">
                            <asp:TextBox ID="tbxAddAccomImageDescription" runat="server" Text="" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage="*" ControlToValidate="tbxAddAccomImageDescription" runat="server" ValidationGroup="uploadimage" />
                        </div>
                        <div class="close-row"></div>
                    </div>
                    <div class="form-row">
                        <div class="form-cell label">Odaberi sliku</div>
                        <div class="form-cell value">
                            <asp:FileUpload ID="fuAddAccommImage" runat="server" />
                            <asp:Button ID="btnUploadAccommImagePreview" Text="Upload" runat="server" 
                                onclick="btnUploadAccommImagePreview_Click" />
                            <asp:RequiredFieldValidator ID="rfvAddAccommImage" runat="server" ErrorMessage="*" ControlToValidate="fuAddAccommImage" ValidationGroup="uploadimage" />
                            <div id="preview">
                                <img id="imgThumb"  src="" alt="" runat="server" visible="false" />
                                <asp:HiddenField ID="hfCrop" runat="server" Value="" />
                            </div>
                        </div>
                        <div class="close-row"></div>
                    </div>
                </div>
                <div class="action-bar">
                    <asp:LinkButton ID="btnUploadAccommImage" CssClass="save-button button" onclick="btnUploadAccommImage_Click" runat="server" Text="Spremi sliku" ValidationGroup="uploadimage" />
                </div>
            </fieldset>
            
            <asp:Panel ID="pnlImageEdit" runat="server">
                <fieldset class="editor basic-info ui-corner-all to-left on-right">
                    <legend>Izmjena slike</legend>
                    <div class="form-editor">
                        <div class="form-row">
                            <div class="form-cell label">Naziv</div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxImageEditTitle" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvImageEditTitle" runat="server" 
                                ErrorMessage="*" 
                                ControlToValidate="tbxImageEditTitle" 
                                ValidationGroup="imageeditsave" />
                            </div>
                            <div class="close-row"></div>
                        </div>  
                        <div class="form-row">
                            <div class="form-cell label">Opis</div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxImageEditDescription" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvImageEditDescription" runat="server" 
                                ErrorMessage="*" 
                                ControlToValidate="tbxImageEditDescription" 
                                ValidationGroup="imageeditsave" />
                            </div>
                            <div class="close-row"></div>
                        </div>
                    </div>
                    <div class="action-bar">
                        <asp:Button ID="btnImageEditSave" Text="Spremi izmjene" runat="server" CssClass="save-button button"
                                    oncommand="btnImageEditSave_Command" CommandName="imageeditsave" ValidationGroup="imageeditsave" />
                    </div>
                </fieldset>
            </asp:Panel>
            <div class="clearfix"></div>
            <asp:Repeater ID="repAccommodationImages" runat="server">
                <HeaderTemplate>
                    <div class="list-wrapper photo-list-wrapper">
                        <h2 class="title">Pregled slika</h2>
                        <ul class="link-list">
                    <h2></h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <li id="<%# "img_" + Eval("Id") %>" class="image-wrapper">
                        <a class="gallery-link" href="<%# Eval("Src") %>">
                            <img src="/thumb.aspx?src=<%# Eval("Src") %>&mh=160&mw=240&gth=h&crop=1" alt="<%# Eval("Alt") %>" title="<%# Eval("Title") %>" longdesc="<%# Eval("Description") %>" />
                        </a>
                        <div class="action-bar-inside">
                            <asp:LinkButton runat="server"
                                Text="Uredi"
                                CommandName="imageedit"
                                CommandArgument='<%# Eval("Id") %>'
                                OnCommand="lbtnEditDeleteImage_Command"
                                CssClass="save"
                                 >
                            </asp:LinkButton>
                            &nbsp;|&nbsp                         
                            <asp:LinkButton runat="server"
                                Text="Briši"
                                CommandName="imagedelete"
                                CommandArgument='<%# Eval("Id") %>'
                                OnCommand="lbtnEditDeleteImage_Command"
                                CssClass="delete" >
                            </asp:LinkButton>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                        <li class="clearfix"></li>
                        </ul>
                    </div>
                    <div class="clearfix"></div>    
                </FooterTemplate>
            </asp:Repeater>

            

        </asp:View>

        <asp:View ID="vwPrices" runat="server">
            <div class="list-wrapper cjenik-list-wrapper">
                <h2 class="title">Cjenici</h2>  
                <asp:Repeater ID="repPrices" runat="server">
                    <HeaderTemplate>
                        <ul class="link-list">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="sub-item">
                                <h3><asp:LinkButton runat="server"
                                            CommandArgument='<%# Eval("Id") %>'
                                            CommandName="priceedit"
                                            OnCommand="PriceEdit_Command" >
                            <%# String.Format("{0}, {1}, {2}{3}", FormatDate((DateTime?)Eval("DateStart"), (DateTime?)Eval("DateEnd")), Eval("Name"), ((decimal)Eval("Value")).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture), Eval("Currency.Symbol"))%>
                            </asp:LinkButton>
                        </h3></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            
            </div>
            <div class="action-bar">
                <asp:LinkButton runat="server" CommandName="pricenew" CommandArgument="" OnCommand="PriceEdit_Command" CssClass="button">Unos cjenika</asp:LinkButton>
            </div>
            
            <br />
            <br />
            <asp:Panel ID="pnlPriceEdit" runat="server">
                <fieldset class="editor basic-info ui-corner-all">
                    <legend>Podaci o cjeniku</legend>
                    <div class="form-editor">
                        <div class="form-row">
                            <div class="form-cell label">Naziv cjenika</div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxPriceEditName" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvPriceEditName" runat="server" 
                                ErrorMessage="*" 
                                ControlToValidate="tbxPriceEditName" 
                                ValidationGroup="priceedit" />
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">Razdoblje</div>
                            <div class="form-cell value">
                                <div id="priceperiod">
                                <input id="rbtnPriceCalendar" runat="server" type="radio" checked="true" name="priceperiod" value="specificperiod" title="Odabir na kalendaru" />Odaberi datume
                                <br />
                                <div id="enterdates">                    
                                    <label for="<%= tbxPriceEditStart.ClientID %>">Od</label>
                                    <asp:TextBox ID="tbxPriceEditStart" runat="server" CssClass="datepicker required" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ErrorMessage="*" 
                                    ControlToValidate="tbxPriceEditStart" 
                                    ValidationGroup="priceeditcal" />
                                    <label for="<%= tbxPriceEditEnd.ClientID %>">-</label>
                                    <asp:TextBox ID="tbxPriceEditEnd" runat="server"  CssClass="datepicker required"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ErrorMessage="*" 
                                    ControlToValidate="tbxPriceEditEnd" 
                                    ValidationGroup="priceeditcal" />
                                </div>
                                <input id="rbtnPriceWholeYear" runat="server" type="radio" name="priceperiod" value="wholeyear" title="Cijela godina" />Cijela godina
                            </div>
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">Cijena</div>
                            <div class="form-cell value">
                                <asp:TextBox ID="tbxPriceEditValue" runat="server" CssClass="required price" />
                                
                                <asp:RequiredFieldValidator ID="rfvPriceEditValue" runat="server" 
                                ErrorMessage="*" 
                                ControlToValidate="tbxPriceEditValue" 
                                ValidationGroup="priceedit" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ValidationExpression="[0-9]+(\.[0-9]{1,2})?"
                                ErrorMessage="npr. 199.00" 
                                ControlToValidate="tbxPriceEditValue" 
                                ValidationGroup="priceedit" />
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label">Valuta</div>
                            <div class="form-cell value">
                                <asp:DropDownList ID="ddlCurrency" runat="server" >
                                </asp:DropDownList>
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label"></div>
                            <div class="form-cell value">
                                
                            </div>
                            <div class="close-row"></div>
                        </div>
                        <div class="form-row">
                            <div class="form-cell label"></div>
                            <div class="form-cell value">
                                
                            </div>
                            <div class="close-row"></div>
                        </div>
                    </div>
                    <div class="action-bar">
                        <asp:LinkButton ID="btnPriceEditDelete" Text="Briši" runat="server" CommandName="pricedelete" OnCommand="PriceSubmit_Command" ValidationGroup="priceedit" CssClass="delete-button button" ></asp:LinkButton>
                        <asp:LinkButton ID="btnPriceEditSave" Text="Spremi" runat="server" CommandName="pricesaveedit|pricesavenew" OnCommand="PriceSubmit_Command" ValidationGroup="priceedit" CssClass="save-button button" ></asp:LinkButton>
                    </div>
                </fieldset>
                
            </asp:Panel>

        </asp:View>
    </asp:MultiView>

    </div>
    <br />
    <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
    
    <div class="clearfix"></div>
    <a id="aBackToAgency" runat="server" class="agency-link button-header-back ui-corner-all" href="">Natrag na agenciju</a>
    
</asp:Content>

