<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EnterCity.ascx.cs" Inherits="Controls_EnterCity" %>

<script type="text/javascript">

    var tbxCountry;
    var tbxCity;
    var tbxRegion;
    var tbxSubregion;
    var tbxIsland;
    var divParents;

    var countryselected;
    var cityselected;
    var regionselected;
    var subregionselected;
    var islandselected;

    var maxresults = 20;
    var languageId = '<%= this.LanguageId %>';

    var countries;
    var cities;
    var regions;
    var subregions;
    var islands;

    var REGION = 5;
    var SUBREGION = 6;
    var ISLAND = 1;

    function InitStuff() {

        tbxCountry = $("#tbxCountry");
        tbxCity = $("#tbxCity");
        tbxRegion = $("#tbxRegion");
        tbxSubregion = $("#tbxSubregion");
        tbxIsland = $("#tbxIsland");

        divIsland = $("#parents");

        var countryselected = false;
        var cityselected = false;
        var regionselected = false;
        var subregionselected = false;
        var islandselected = false;

        tbxCountry.bind("change", function () {
            if (!countryselected) {
                var tekst = tbxCountry.val();
                var id, title;
                $.each(countries, function () {
                    if (this.Title.toLowerCase() == tekst.toLowerCase()) {
                        countryselected = true;
                        id = this.Id;
                        title = this.Title;
                    }
                });

                if (countryselected) {
                    UnSetCountry();
                    SetCountry(id, title);
                }
            }
        });
        tbxCity.bind("change", function () {
            if (!cityselected) {
                var tekst = tbxCity.val();
                var id, title;
                $.each(cities, function () {
                    if (this.Title.toLowerCase() == tekst.toLowerCase()) {
                        cityselected = true;
                        id = this.Id;
                        title = this.Title;
                    }
                });

                if (cityselected) {
                    UnSetCity();
                    SetCity(id, title);

                    divParents..each(function (index) {
                        $(this).hide();
                    });
                }
                else {
                    divParents.each(function (index) {
                        $(this).hide();
                    });
                }
            }
        });
        tbxRegion.bind("change", function () {
            if (!regionselected) {
                var tekst = tbxRegion.val();
                var id, title;
                $.each(regions, function () {
                    if (this.Title.toLowerCase() == tekst.toLowerCase()) {
                        regionselected = true;
                        id = this.Id;
                        title = this.Title;
                    }
                });

                if (regionselected) {
                    UnSetRegion();
                    SetRegion(id, title);
                }
            }
        });
        tbxSubregion.bind("change", function () {
            if (!subregionselected) {
                var tekst = tbxSubregion.val();
                var id, title;
                $.each(subregions, function () {
                    if (this.Title.toLowerCase() == tekst.toLowerCase()) {
                        subregionselected = true;
                        id = this.Id;
                        title = this.Title;
                    }
                });

                if (subregionselected) {
                    UnSetSubregion();
                    SetSubregion(id, title);
                }
            }
        });
        tbxIsland.bind("change", function () {
            if (!islandselected) {
                var tekst = tbxIsland.val();
                var id, title;
                $.each(islands, function () {
                    if (this.Title.toLowerCase() == tekst.toLowerCase()) {
                        islandselected = true;
                        id = this.Id;
                        title = this.Title;
                    }
                });

                if (islandselected) {
                    UnSetIsland();
                    SetIsland(id, title);
                }
            }
        });
    }


    function pageLoad() {
        InitStuff();
        if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
            SetAutocomplete();
        }
    }

    $(document).ready(function () {
        $(".lala").css('display', 'none');
        InitStuff();
        SetAutocomplete();
        
    });

    function SetCountry(id, title) {
        FetchCities(id);
        FetchChildren(id, REGION);
        FetchChildren(id, SUBREGION);
        FetchChildren(id, ISLAND);
       
        countryselected = true;
        tbxCountry.val(title);

        tbxCountry.attr('disabled', true);
    }
    function SetCity(id, title) {
        FetchParents(id);

        cityselected = true;
        tbxCity.val(title);

        tbxCity.attr('disabled', true);
    }
    function SetRegion(id, title) {
        regionselected = true;
        tbxRegion.val(title);

        tbxRegion.attr('disabled', true);
    }
    function SetSubregion(id, title) {
        subregionselected = true;
        tbxSubregion.val(title);

        tbxSubregion.attr('disabled', true);
    }
    function SetIsland(id, title) {
        islandselected = true;
        tbxIsland.val(title);

        tbxIsland.attr('disabled', true);
    }


    function UnSetCountry() {
        tbxCountry.val("");
        cities = null;
        countryselected = false;

        UnSetCity();
    }
    function UnSetCity() {
        tbxCity.val("");
        cityselected = false;

        UnSetRegion();
        UnSetSubregion();
        UnSetIsland();
    }
    function UnSetRegion() {
        tbxRegion.val("");
        regionselected = false;

        UnSetSubregion();
        UnSetIsland();
    }
    function UnSetSubregion() {
        tbxSubregion.val("");
        subregionselected = false;

        UnSetIsland();
    }
    function UnSetIsland() {
        tbxIsland.val("");
        islandselected = false;
    }

    function PopulateCityParents(values) {

        UnSetRegion();

        $.each(values, function () {

            if (this.Type == "REGION") {
                regions = this;
                SetRegion(this.Id, this.Title);
            }
            else if (this.Type == "SUBREGION") {
                subregions = this;
                SetSubregion(this.Id, this.Title);
            }
            else if (this.Type == "ISLAND" || this.Title == "PENNINSULA") {
                islands = this;
                SetIsland(this.Id, this.Title);
            }
        })
        
    }

    function FetchCities(countryId) {

        $.ajax({
            url: "/services/PlaceInteractionService.asmx/GetParentCities",
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: "{ 'countryId': '" + countryId + "', 'languageId': '" + languageId + "' }",
            success: function (data) {
                cities = data.d;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }

    function FetchParents(childId) {
        $.ajax({
            url: "/services/PlaceInteractionService.asmx/GetParents",
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: "{ 'childTitle': '" + null + "', 'childId': '" + childId + "', 'languageId': '" + languageId + "' }",
            success: function (data) {
                PopulateCityParents(data.d);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }

    function FetchChildren(parentId, childTypeId) {
        $.ajax({
            url: "/services/PlaceInteractionService.asmx/GetChildrenByType",
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: "{ 'parentId': '" + parentId + "', 'childTypeId': '" + childTypeId + "', 'languageId': '" + languageId + "' }",
            success: function (data) {
                if (childTypeId == REGION) {
                    regions = data.d;
                }
                else if (childTypeId == SUBREGION) {
                    subregions = data.d;
                }
                else if (childTypeId == ISLAND) {
                    islands = data.d;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }

    function SetAutocomplete() {

        tbxCountry.autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: "/services/PlaceInteractionService.asmx/SuggestCountry",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: "{ 'term': '" + request.term + "', 'maxresults': '" + maxresults + "' }",
                    success: function (data) {
                        countries = data.d;
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

            },


            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });

        tbxCity.autocomplete({
            minLength: 2,
            source: function (request, response) {
                response($.map(cities, function (item) {
                    if (item.Title.toLowerCase().match(request.term.toLowerCase())) {
                        return {
                            label: item.DisplayTitle,
                            value: item.Title,
                            id: item.Id
                        }
                    }
                }));
            },
            select: function (event, ui) {

            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });

        tbxRegion.autocomplete({
            minLength: 2,
            source: function (request, response) {
                response($.map(regions, function (item) {
                    if (item.Title.toLowerCase().match(request.term.toLowerCase())) {
                        return {
                            label: item.Title,
                            value: item.Title,
                            id: item.Id
                        }
                    }
                }));
            },
            select: function (event, ui) {

            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });

        tbxSubregion.autocomplete({
            minLength: 2,
            source: function (request, response) {
                response($.map(subregions, function (item) {
                    if (item.Title.toLowerCase().match(request.term.toLowerCase())) {
                        return {
                            label: item.Title,
                            value: item.Title,
                            id: item.Id
                        }
                    }
                }));
            },
            select: function (event, ui) {

            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });

        tbxIsland.autocomplete({
            minLength: 2,
            source: function (request, response) {
                response($.map(islands, function (item) {
                    if (item.Title.toLowerCase().match(request.term.toLowerCase())) {
                        return {
                            label: item.Title,
                            value: item.Title,
                            id: item.Id
                        }
                    }
                }));
            },
            select: function (event, ui) {
                
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });

    }


</script>


<div id="countrycity">
    
    Država:
    <input id="tbxCountry"type="text" name="country" value="" />
    
    <br />
    Grad:
    <input id="tbxCity"type="text" name="country" value="" />


</div>

<div id="parents" >
    Regija:
    <input id="tbxRegion" type="text" name="region" value="" />
            
    <br />
    Podregija:
    <input id="tbxSubregion" type="text" name="subregion" value="" />
            
    <br />
    Otok/Poluotok
    <input id="tbxIsland" type="text" name="island" value="" />
</div>

<div id="konjo" class="lala" >text
</div>
