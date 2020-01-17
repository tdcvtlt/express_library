
Partial Class setup_EditResortAccom
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siUnitType.Label_Caption = ""
            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.ComboItem = "UnitType"
            siUnitType.Load_Items()
            Dim oRateTable As New clsRateTable
            ddRateTable.Items.Add(New ListItem("", 0))
            ddRateTable.DataSource = oRateTable.Get_Rate_Tables
            ddRateTable.DataTextField = "Name"
            ddRateTable.DataValueField = "RateTableID"
            ddRateTable.AppendDataBoundItems = True
            ddRateTable.DataBind()
            oRateTable = Nothing
            If Request("ID") > 0 Then
                Load_Items(Request("ID"))
            End If
        End If
    End Sub

    Private Sub Load_Items(ByVal ID As Integer)
        Dim oAccom2Res As New clsAccom2Resort
        oAccom2Res.Accom2ResortID = ID
        oAccom2Res.Load()
        siUnitType.Selected_ID = oAccom2Res.UnitTypeID
        ddBedroom.SelectedValue = oAccom2Res.BD
        txtMaxOcc.Text = oAccom2Res.MaxOccupancy
        ddRateTable.SelectedValue = oAccom2Res.RateTableID
        oAccom2Res = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If ddBedroom.SelectedValue = "" Or siUnitType.Selected_ID < 1 Or txtMaxOcc.Text = "" Or Not (IsNumeric(txtMaxOcc.Text)) Then
            bProceed = False
            sErr = "Please Fill in All Fields"
        ElseIf siUnitType.SelectedName = "Cottage" And (ddBedroom.SelectedValue = "1BD-DWN" Or ddBedroom.SelectedValue = "1BD-UP" Or ddBedroom.SelectedValue = "4") Then
            bProceed = False
            sErr = "Invalid UnitType/BD matchup"
        ElseIf siUnitType.SelectedName = "Townes" And (Left(ddBedroom.SelectedValue, 1) = "1" Or ddBedroom.SelectedValue = "3") Then
            bProceed = False
            sErr = "Invalid UnitType/BD matchup"
        ElseIf siUnitType.SelectedName = "Estates" And (ddBedroom.SelectedValue = "1") Then
            bProceed = False
            sErr = "Invalid UnitType/BD matchup"
        End If
        If bProceed Then
            Dim oAccom2Res As New clsAccom2Resort
            oAccom2Res.Accom2ResortID = Request("ID")
            oAccom2Res.Load()
            If Request("ID") = 0 Then
                oAccom2Res.AccomID = Request("AccomID")
            End If
            oAccom2Res.UserID = Session("UserDBID")
            oAccom2Res.UnitTypeID = siUnitType.Selected_ID
            oAccom2Res.BD = ddBedroom.SelectedValue
            oAccom2Res.MaxOccupancy = txtMaxOcc.Text
            oAccom2Res.RateTableID = ddRateTable.SelectedValue
            oAccom2Res.Save()
            oAccom2Res = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Resort();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
