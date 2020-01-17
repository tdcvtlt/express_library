
Partial Class Accounting_DoNotSellListOverRide
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckSecurity("Prospects", "DoNotSellOverride", , , Session("UserDBID")) Then
            Dim oDNSCode As New clsDoNotSellListCodes
            Dim rndNum As Integer = 0
            rndNum = CInt(Math.Floor((9999 - 1000 + 1) * Rnd())) + 1000
            Do While Not (oDNSCode.Dupe_Check(rndNum))
                rndNum = CInt(Math.Floor((9999 - 1000 + 1) * Rnd())) + 1000
            Loop
            oDNSCode.ID = 0
            oDNSCode.Load()
            oDNSCode.CreatedByID = Session("UserDBID")
            oDNSCode.Code = rndNum
            oDNSCode.DateCreated = System.DateTime.Now
            oDNSCode.ExpirationDate = System.DateTime.Now.AddHours(1)
            oDNSCode.Save()
            txtCode.Text = rndNum
            oDNSCode = Nothing
        Else
            lbErr.Text = "You Do Not Have Permission To Generate OverRide Codes."
        End If
    End Sub
End Class
