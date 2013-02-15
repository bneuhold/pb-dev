<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="FavoriteCookieAds.ascx.cs"
    Inherits="Controls_FavoriteCookieAds" %>

<div id="favorite_ads" class="favorite_ads">
    <ul id="favoriteAdList">
        <asp:Repeater ID="repFavoriteAds" runat="server">
            <ItemTemplate>
                <li id="favoriteAdId-<%# Eval("Id") %>">
                    <span class="one_ad">
                        <a href="/RedirectAd.aspx?Id=<%# Eval("Id") %>"><%# Eval("Title") %></a> 
                        <a href="javascript:RemoveFromFavoritesClick('<%# Eval("Id") %>')" title="<%# Eval("Title") %>" class="action_remove"></a>
                    </span>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <script type="text/javascript">

        var CookieAds = {
            savetext: "",
            removetext: "",
            cookiename: "savedAds",

            cookieexpire: 365,
            cookiepath: "/",
            cookiesplit: ",",

            elementid: "saveAd-",
            savedAds: new Array(),

            Init: function (savetext, removetext, cookiename) {
                this.savetext = savetext;
                this.removetext = removetext;
                this.cookiename = cookiename;

                GetCookieAds();
            },

            GetCookieAds: function () {
                var cookie = $.cookie(this.cookiename);
                this.savedAds = cookie ? cookie.split(this.cookiesplit) : new Array();
            },

            SaveCookieAds: function () {
                $.cookie(this.cookiename, this.savedAds.join(this.cookiesplit), { expires: this.cookieexpire, path: this.cookiepath });
            },

            SaveRemoveClick: function (ad) {
                if (this.savedAds.indexOf(ad) < 0) {
                    this.savedAds.push(ad);
                }
                else {
                    this.savedAds.splice($.inArray(ad, this.savedAds), 1);
                }
                this.ChangeSaveRemoveText(ad);
                this.SaveCookieAds();
            },

            ChangeSaveRemoveText: function (ad) {
                var obj = $("#" + this.elementid + ad);
                if (obj) {
                    if (this.savedAds.indexOf(ad) < 0) {
                        obj.removeClass("remove_this").addClass("save_this");
                    }
                    else {
                        obj.removeClass("save_this").addClass("remove_this");
                    }
                }
            },

            IsAdInCookie: function (ad) {
                return this.savedAds.indexOf(ad) < 0;
            }
        }

        //Inicijalizacija CookieAds klase"
        CookieAds.GetCookieAds();

        function SaveRemoveClick(ad, title) {
            var obj = $("#" + CookieAds.elementid + ad);
            CookieAds.SaveRemoveClick(ad);
            if (CookieAds.IsAdInCookie(ad)) {
                FavoriteAds.RemoveFromDom(ad);
            }
            else {
                FavoriteAds.AddToDom(ad, obj.attr("title"));
            }
        }

        function RemoveFromFavoritesClick(ad) {
            FavoriteAds.RemoveFromDom(ad);
            CookieAds.SaveRemoveClick(ad);
        }
    </script>


    <script type="text/javascript">
        var FavoriteAds = {
            idul: "favoriteAdList",
            idli: "favoriteAdId-",

            removetext: "",

            RemoveFromDom: function (ad) {
                var obj = $("#" + this.idli + ad);
                obj.remove();
                
            },

            AddToDom: function (ad, title) {
                //alert("dodajem");
                var obj = $("#" + this.idul);

                obj.append('<li id="' + this.idli + ad + '"><span class="one_ad"><a href="/RedirectAd.aspx?Id=' + ad + '">' + title + '</a><a href="javascript:RemoveFromFavoritesClick(\'' + ad + '\')" title="' + title + '" class="action_remove">' + this.removetext + '</a></span></li>')
                $('#favoriteAdList span').stop().animate({ 'marginLeft': '10px' }, 1000);
                $('#' + this.idli + ad).hover(
                    function () { $('span.one_ad', $(this)).stop().animate({ 'marginLeft': '-280px' }, 200); },
                    function () { $('span.one_ad', $(this)).stop().animate({ 'marginLeft': '10px' }, 200); });
            }
        }

        $(function () {
            $('#favoriteAdList span').stop().animate({ 'marginLeft': '10px' }, 1000);
            $('#favoriteAdList > li').hover(
                    function () { $('span.one_ad', $(this)).stop().animate({ 'marginLeft': '-280px' }, 200); },
                    function () { $('span.one_ad', $(this)).stop().animate({ 'marginLeft': '10px' }, 200); });
            
        });
    </script>

    <script type="text/javascript">
        $(".save_this").each(function () {
            CookieAds.ChangeSaveRemoveText($(this).attr("id").split("-")[1]);
        });
    </script>
</div>
