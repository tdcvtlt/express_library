
Partial Class general_EditPersonnelTrans
    Inherits System.Web.UI.Page
    Dim oPT As New clsPersonnelTrans

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select_Item1.Connection_String = Resources.Resource.cns
            Select_Item1.Load_Items()
            Dim oPers As New clsPersonnel
            ddlPersonnel.DataSource = oPers.List
            ddlPersonnel.DataTextField = "Name"
            ddlPersonnel.DataValueField = "ID"
            ddlPersonnel.DataBind()
            oPT.Personnel_Trans_ID = Request("ID")
            oPT.Load()
            ddlPersonnel.SelectedValue = oPT.PersonnelID
            txtPersonnelTransID.Text = Request("ID")
            txtCP.Text = oPT.Percentage
            txtFA.Text = oPT.Fixed_Amount
            Select_Item1.Selected_ID = oPT.TitleID
            dfDateCreated.Selected_Date = oPT.Date_Created
            dfDatePosted.Selected_Date = oPT.Date_Posted
            oPers = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If IsNumeric(Request("ID")) And Request("ID") <> "0" Then
            If Request("KeyField") = "ContractID" Then
                If Not (CheckSecurity("Contracts", "EditPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Edit Contract Personnel Records."
                End If
            ElseIf Request("KeyField") = "TourID" Then
                If Not (CheckSecurity("Tours", "EditPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Edit Tour Personnel Records."
                End If
            ElseIf Request("KeyField") = "PackageIssuedID" Then
                If Not (CheckSecurity("TourPackages", "EditPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Edit Package Personnel Records."
                End If
            ElseIf Request("KeyField") = "ReservationID" Then
                If Not (CheckSecurity("Reservations", "EditPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Edit Reservation Personnel Records."
                End If
            End If
        ElseIf IsNumeric(Request("KeyValue")) And Request("KeyField") <> "" Then
            If Request("KeyField") = "ContractID" Then
                If Not (CheckSecurity("Contracts", "AddPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Add Contract Personnel Records."
                End If
            ElseIf Request("KeyField") = "TourID" Then
                If Not (CheckSecurity("Tours", "AddPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Add Tour Personnel Records."
                End If
            ElseIf Request("KeyField") = "PackageIssuedID" Then
                If Not (CheckSecurity("TourPackages", "AddPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Add Package Personnel Records."
                End If
            ElseIf Request("KeyField") = "ReservationID" Then
                If Not (CheckSecurity("Reservations", "AddPersonnel", , , Session("UserDBID"))) Then
                    bProceed = False
                    sErr = "You Do Not Have Persmission to Add Reservation Personnel Records."
                End If
            End If
        Else
            Exit Sub
        End If

        If bProceed Then
            oPT.Personnel_Trans_ID = Request("ID")
            oPT.Load()
            If Request("ID") = 0 Then
                oPT.KeyField = Request("KeyField")
                oPT.KeyValue = Request("KeyValue")
                oPT.Created_By_ID = Session("UserDBID")
            End If
            oPT.UserID = Session("UserDBID")
            oPT.PersonnelID = ddlPersonnel.SelectedValue
            oPT.TitleID = Select_Item1.Selected_ID
            oPT.Fixed_Amount = txtFA.Text
            oPT.Percentage = txtCP.Text
            oPT.Date_Created = dfDateCreated.Selected_Date
            oPT.Date_Posted = dfDatePosted.Selected_Date
            oPT.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
