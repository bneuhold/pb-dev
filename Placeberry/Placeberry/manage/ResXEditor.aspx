<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="ResXEditor.aspx.cs" Inherits="manage_ResXEditor" ValidateRequest="false" %>
<%@ Register TagName="ResXEditor" TagPrefix="resxEditor" Src="~/manage/Controls/ResXEditorCtrl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ResX Editor</title>
</head>
<body>
    <form id="form1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server" CombineScripts="True" />
    <div>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <resxEditor:ResXEditor ID="editor" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:UpdateProgress ID="UpdateProgress" runat="server" DisplayAfter="300" AssociatedUpdatePanelID="UpdatePanel">
            <ProgressTemplate>
                <asp:Panel ID="progressPanel" runat="server" CssClass="UpdateProgressFooter">
                    <asp:Image ID="imgLoading" runat="server" SkinID="LoadingImage" />
					<span class="LoadingMessage" style="padding-left: 8px;">
						<asp:Literal ID="Literal1" runat="server">Please wait...</asp:Literal>
					</span>
                </asp:Panel>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>