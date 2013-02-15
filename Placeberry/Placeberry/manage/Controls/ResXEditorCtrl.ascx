<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResXEditorCtrl.ascx.cs" Inherits="manage_Controls_ResXEditorCtrl" %>
<%@ Register TagPrefix="resx" Namespace="ResxWebEditor" Assembly="LibUtility" %>

<style type="text/css">
    .UpdateProgressFooter 
    {
	    color:White;
	    background-color: Black;
	    border:outset 2px;
        text-align:center;
	    position:absolute;
	    top: expression(this.offsetParent.scrollTop + this.offsetParent.clientHeight-this.offsetHeight -10);
	    left: expression(this.offsetParent.clientWidth/2-this.offsetWidth/2);
	    width: 90%;
    }

    h2
    {
	    margin-bottom: 10px;
    }

    select
    {
	    background-color: whitesmoke;
    }

    .GridViewTranslate tr, .GridViewTranslate td
    {
	    padding: 2px;
    }

    .GridViewTranslate input[type=text]
    {
	    background-color:White;
    }
</style>


<%-- Helvetica Neue,Lucida Grande,Segoe UI,Arial,Helvetica,Verdana,sans-serif--%>
<div style="width:100%; text-align:center; font-family:Segoe UI;">
<div style="display:inline-block; margin-top:20px;">
<span style="font-size:25px;">Resource Translation</span>
    <asp:PlaceHolder runat="server" ID="phDisplay">
	    <div style="clear: both; width: auto;">
		    <asp:Panel ID="pnlAddLang" runat="server" style="float:left; margin-bottom:30px; margin-top:30px; text-align:left;" meta:resourcekey="pnlAddLangResource1">
			    <asp:Literal ID="Literal1" runat="server" Text="Add new language:" /><br />
			    <asp:DropDownList ID="ddLanguage" runat="server" />
			    <asp:Button ID="btAddLang" runat="server" onclick="btAddLang_Click" OnClientClick="this.disable=true;" Enabled="true" Text="Add" />
                <span style="margin-left:50px;"><asp:CheckBox TextAlign="Left" ID="chShowEmpty" runat="server" Text="Show empty strings"
                Checked="false" AutoPostBack="true" OnCheckedChanged="OnShowEmpty" /></span>
            </asp:Panel>
		    <br style="clear:both" />
		    <table style="width: 100%;">
			    <tr>
				    <td>
					    <h2><asp:Label ID="lblFileName" runat="server" /></h2>
					    <resx:BulkEditGridViewEx id="gridView" runat="server" GridLines="Both" Width="880px" CssClass="GridViewTranslate"
                        AutoGenerateColumns="False" EnableInsert="False" onrowdatabound="GridView_RowDataBound" SaveButtonID="btSave"
                        DataKeyNames="Key" onrowupdating="gridView_RowUpdating" OnSaved="gridView_Saved" InsertRowCount="1" />
                        <div style="display:inline-block; margin-top:20px;margin-bottom:20px; ">
                            <asp:Button ID="btSave" runat="server" Text="Save" OnClientClick="this.disabled = true; this.value = 'Submitting...';" UseSubmitBehavior="false"/>
                        </div>
                        <div style="display:inline; margin-top:20px; margin-bottom:20px; float:right;">
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="this.disabled = true; this.value = 'Uploading...';"
                            UseSubmitBehavior="false" OnClick="btnUpload_Click" />
                        </div>
					    <asp:Panel ID="pnlMsg" runat="server" EnableViewState="false" Visible="false">
						    <asp:MultiView ID="MultiViewMsg" runat="server">
							    <asp:View ID="View1" runat="server">
								    <asp:Image ID="imgResult" runat="server" ImageUrl="~/resources/images/tick.png" />
							    </asp:View>
							    <asp:View ID="View2" runat="server">
								    <asp:Image ID="Image1" runat="server" ImageUrl="~/resources/images/exclamation.png" />
							    </asp:View>
						    </asp:MultiView>
						    <asp:Label ID="lblMsg" runat="server" style="padding-left: 10px; position:relative; top: -5px;" />
					    </asp:Panel>
				    </td>
			    </tr>
		    </table>
	    </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phEmptyFolderMsg">
        <div style="font-size:20px; margin-top:30px"><asp:Literal runat="server" ID="ltEmptyFolderMsg"></asp:Literal></div>
    </asp:PlaceHolder>
</div>  
</div>
