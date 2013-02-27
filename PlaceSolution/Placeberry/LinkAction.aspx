<%@ Page language="c#" %>
<script runat="server">

    private void Page_Load(object sender, System.EventArgs e)
    {
        string action = Request.QueryString["action"];
        if (!string.IsNullOrEmpty(action))
        {
            action = action.ToLower();
        }

        switch (action)
        {
            case "login":
                Response.Redirect("~/manage");
                break;
            case "logout":
                FormsAuthentication.SignOut();
                Roles.DeleteCookie();
                Session.Clear();
                Response.Redirect(FormsAuthentication.DefaultUrl);
                break;
            default:
                Response.Redirect("~");
                break;
        }
    }

</script>
