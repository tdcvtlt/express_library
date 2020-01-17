
Partial Class marketing_DoNotSellListOverRide
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim oDNSCode As New clsDoNotSellListCodes
        If oDNSCode.Validate_Code(txtCode.Text) Then
            oDNSCode.ID = oDNSCode.Get_Code_ID(txtCode.Text)
            oDNSCode.Load()
            oDNSCode.UsedByID = Session("UserDBID")
            oDNSCode.DateUsed = System.DateTime.Now
            oDNSCode.Source = Request("Source")
            oDNSCode.Save()
            If Request("KeyField") = "Prospect" Then
                If Request("Source") = "AddTour" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editTour.aspx?TourID=0&ProspectID=" & Request("KeyValue") & "';window.close();", True)
                ElseIf Request("Source") = "AddContract" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editContract.aspx?ContractID=0&ProspectID=" & Request("KeyValue") & "';window.close();", True)
                ElseIf Request("Source") = "AddPackage" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editPackage.aspx?packageissuedID=0&ProspectID=" & Request("KeyValue") & "';window.close();", True)
                End If
            ElseIf Request("KeyField") = "Reservation" Then
                If Request("Source") = "AddTour" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editTour.aspx?TourID=0&ReservationID=" & Request("KeyValue") & "';window.close();", True)
                ElseIf Request("Source") = "BookReservation" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.Save_Res();window.close();", True)
                End If
            ElseIf Request("KeyField") = "Contract" Then
                If Request("Source") = "AddConversion" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/marketing/editConversion.aspx?ConversionID=0&ProspectID=" & Request("KeyValue") & "';window.close();", True)
                ElseIf Request("Source") = "ContractWizard" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.increment_Step();window.close();", True)
                End If
            ElseIf Request("KeyField") = "Tour" Then
                If Request("Source") = "TourWizard" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/wizards/TourCheckIn.aspx?TourID=" & Request("KeyValue") & "';window.close();", True)
                ElseIf Request("Source") = "TourWizardNew" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.document.location.href = '" & Request.ApplicationPath & "/wizards/TourCheckIn.aspx?TourID=0&ProspectID=" & Request("KeyValue") & "';window.close();", True)
                ElseIf Request("Source") = "BookTour" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.Save_Tour();window.close();", True)
                End If
            End If
        Else
            Dim sErr As String = ""
            sErr = oDNSCode.Invalid_Reason(txtCode.Text)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
        End If
        oDNSCode = Nothing
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.close();", True)

    End Sub
End Class
