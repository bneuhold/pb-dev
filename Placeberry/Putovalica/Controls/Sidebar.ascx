<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sidebar.ascx.cs" Inherits="Controls_Sidebar" %>


</form> <!-- Close the ASP.NET close form tag that's embedded in the master page. -->

            <!-- START .sidebar -->

            <aside class="sidebar">

                <h3 class="sidebar-title">PRETRAŽI Putovalicu</h3>

                <article>                
                    <form action="#">
                        <input id="search" name="search" class="text" type="text" placeholder="upiši pojam" />
                        <input type="submit" class="btn" value="" />
                    </form>
                </article>

                <h3 class="sidebar-title">IZABERI KATEGORIJU</h3>

                <asp:Repeater runat="server" ID="rptCategories">
                <HeaderTemplate>
                    <ul class="category-menu">
                </HeaderTemplate>
                <ItemTemplate>
                    <li runat="server" id="liCat"><asp:HyperLink runat="server" ID="hlCat"><%# DataBinder.Eval(Container.DataItem, "Translation.Title")%></asp:HyperLink>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
                </asp:Repeater>

                <h3 class="sidebar-title">ŽELIŠ PONUDE NA MAIL?</h3>

                <article>
                    <form action="#">
                        <input id="mail" name="mail" class="text" type="text" placeholder="upiši e-mail">
                        <input type="submit" class="btn" value="">
                    </form>
                </article>

            </aside> <!-- END .sidebar -->


<form id="dummyForm"> <!-- put in a dummy form to deal with the </form> tag that's embedded at the end of the master page. -->
