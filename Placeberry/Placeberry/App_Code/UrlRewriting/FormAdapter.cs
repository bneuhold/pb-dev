using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

/// <summary>
/// Summary description for FormRewriter
/// </summary>
public class FormRewriterControlAdapter : System.Web.UI.Adapters.ControlAdapter
{
	public FormRewriterControlAdapter()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	protected override void Render(System.Web.UI.HtmlTextWriter writer)
	{
		base.Render(new RewriteFormHtmlTextWriter(writer));
	}
}

public class RewriteFormHtmlTextWriter : System.Web.UI.HtmlTextWriter
{
	public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
		: base(new HtmlTextWriter(writer))
	{
		this.InnerWriter = writer.InnerWriter;
	}

	public RewriteFormHtmlTextWriter(System.IO.TextWriter writer)
		: base(new HtmlTextWriter(writer))
	{
		base.InnerWriter = writer;
	}

	public override void WriteAttribute(string name, string value, bool fEncode)
	{
		if (name == "action")
		{
			HttpContext Context;
			Context = HttpContext.Current;

			if (Context.Items["ActionAlreadyWritten"] == null)
			{
				value = Context.Request.RawUrl;
				Context.Items["ActionAlreadyWritten"] = true;
			}
		}

		base.WriteAttribute(name, value, fEncode);
	}
}

/*
public class PanelControlAdapter : System.Web.UI.Adapters.ControlAdapter
{
    public PanelControlAdapter()
    {
    }

    protected override void RenderChildren(HtmlTextWriter writer)
    {
        //base.RenderChildren(writer); 
    }

    protected override void CreateChildControls()
    {
        //base.CreateChildControls();
    }
}
*/