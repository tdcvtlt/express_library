
Partial Class setup_Printers_EditPrinter
    Inherits System.Web.UI.Page

    Protected Sub Printer_Click(sender As Object, e As EventArgs) Handles Printer.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub OIDs_Click(sender As Object, e As EventArgs) Handles OIDs.Click
        If PrinterID.Text <> 0 Then
            MultiView1.ActiveViewIndex = 1
            gvOIDs.DataSource = (New clsPrinterOID).List_By_Printer(PrinterID.Text)
            gvOIDs.DataBind()
        End If
    End Sub

    Protected Sub Reads_Click(sender As Object, e As EventArgs) Handles Reads.Click
        If PrinterID.Text <> 0 Then
            MultiView1.ActiveViewIndex = 2
            gvReads.DataSource = (New clsPrinterOIDValues).List_By_Printer(PrinterID.Text)
            gvReads.DataBind()
        End If
    End Sub

    Protected Sub Events_Click(sender As Object, e As EventArgs) Handles Events.Click
        If PrinterID.Text <> 0 Then
            MultiView1.ActiveViewIndex = 3
            Events1.KeyField = "PrinterID"
            Events1.KeyValue = PrinterID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub Save_Click(sender As Object, e As EventArgs) Handles Save.Click
        Dim oPrinter As New clsPrinters
        oPrinter.PrinterID = PrinterID.Text
        oPrinter.Load()
        oPrinter.Name = Name.Text
        oPrinter.HostName = HostName.Text
        oPrinter.UserID = Session("UserDBID")
        oPrinter.Save()
        PrinterID.Text = oPrinter.PrinterID
        Load_Printer()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("PrinterID") <> "" And Request("PrinterID") <> "0" And IsNumeric(Request("PrinterID")) Then
                PrinterID.Text = Request("PrinterID")
                Load_Printer()
            End If
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Private Sub Load_Printer()
        Dim oPrinter As New clsPrinters
        oPrinter.PrinterID = PrinterID.Text
        oPrinter.Load()
        PrinterID.Text = oPrinter.PrinterID
        Name.Text = oPrinter.Name
        HostName.Text = oPrinter.HostName
        oPrinter = Nothing
    End Sub

    Protected Sub lbRefresh_Click(sender As Object, e As EventArgs) Handles lbRefresh.Click
        gvOIDs.DataSource = (New clsPrinterOID).List_By_Printer(PrinterID.Text)
        gvOIDs.DataBind()
    End Sub
End Class
