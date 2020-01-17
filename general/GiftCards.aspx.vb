
Partial Class general_GiftCards
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            siLocations.Label_Caption = ""
            siLocations.Connection_String = Resources.Resource.cns
            siLocations.ComboItem = "GiftCardLocations"
            siLocations.Load_Items()
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        Label1.Text = ""
        Dim sErr As String = ""
        If txtStart.Text = "" Or txtEnd.Text = "" Or txtBalance.Text = "" Then
            sErr = "Please Fill In All Fields"
        Else
            If Not (IsNumeric(txtBalance.Text)) Or Not (IsNumeric(txtStart.Text)) Or Not (IsNumeric(txtEnd.Text)) Then
                sErr = "Please Enter a Valid Number"
            Else
                If CInt(txtStart.Text) > CInt(txtEnd.Text) Then
                    sErr = "Please Enter a Valid Range of Card Numbers."
                Else
                    If CDbl(txtBalance.Text) < 0.01 Then
                        sErr = "Please Enter a Valid Balance."
                    Else
                        If ddLocations.Items.Count = 0 Then
                            sErr = "Please Select a Location."
                        End If
                    End If
                End If
            End If
        End If

        If sErr = "" Then
            Dim i As Integer = 0
            Dim ID As Integer = 0
            Dim oGC As New clsGiftCard
            Dim oGC2Loc As New clsGiftCard2Location
            Dim startnum As Integer = CInt(txtStart.Text)
            Dim endnum As Integer = CInt(txtEnd.Text)
            For i = startnum To endnum
                ID = 0
                oGC.GiftCardID = 0
                oGC.Number = i
                oGC.Load()
                oGC.Balance = txtBalance.Text
                oGC.CreatedByID = Session("UserDBID")
                oGC.UserID = Session("UserDBID")
                oGC.Number = i
                oGC.DateCreated = System.DateTime.Now
                oGC.Active = ckActive.Checked
                oGC.Save()
                ID = oGC.GiftCardID
                oGC2Loc.Clear_Locations(ID)
                For j = 0 To ddLocations.Items.Count - 1
                    oGC2Loc.GiftCard2LocID = 0
                    oGC2Loc.Load()
                    oGC2Loc.GiftCardID = ID
                    oGC2Loc.LocationID = ddLocations.Items(j).Value
                    oGC2Loc.Save()
                Next j
            Next i
            oGC = Nothing
            oGC2Loc = Nothing
            Label1.Text = "Gift Cards Allocated"
        Else
            Label1.Text = sErr
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
        If siLocations.Selected_ID = 0 Then

        Else
            ddLocations.Items.Add(New ListItem(siLocations.SelectedName, siLocations.Selected_ID))
        End If
    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As System.EventArgs) Handles btnRemove.Click
        If ddLocations.Items.Count = 0 Then

        Else
            ddLocations.Items.Remove(ddLocations.SelectedItem)
        End If
    End Sub
End Class
