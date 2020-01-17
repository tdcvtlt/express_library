
Partial Class setup_Printers_EditOID
    Inherits System.Web.UI.Page

    Protected Sub lbClose_Click(sender As Object, e As EventArgs) Handles lbClose.Click
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim o As New clsPrinterOID
            o.OIDID = IIf(Request("OIDID") <> "" And IsNumeric(Request("OIDID")), Request("OIDID"), 0)
            o.Load()
            OID.Text = o.OID
            Description.Text = o.Description
            o = Nothing
        End If
    End Sub

    Protected Sub lbSave_Click(sender As Object, e As EventArgs) Handles lbSave.Click
        Dim o As New clsPrinterOID
        o.OIDID = IIf(Request("OIDID") <> "" And IsNumeric(Request("OIDID")), Request("OIDID"), 0)
        o.Load()
        o.OID = OID.Text
        o.Description = Description.Text
        o.PrinterID = IIf(Request("OIDID") <> "" And IsNumeric(Request("OIDID")), o.PrinterID, Request("PrinterID"))
        o.Save()
        o = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)

    End Sub
End Class
