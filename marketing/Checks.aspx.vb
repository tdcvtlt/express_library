Imports Microsoft.VisualBasic
Partial Class marketing_Checks
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddPagePos.Items.Add(1)
            ddPagePos.Items.Add(2)
            ddPagePos.Items.Add(3)

            siLocation.Connection_String = Resources.Resource.cns
            siLocation.ComboItem = "TourLocation"
            siLocation.Label_Caption = ""
            siLocation.Load_Items()
        End If
    End Sub
    Protected Sub Add_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Add_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Print_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Print_Link.Click
        MultiView1.ActiveViewIndex = 1
        Dim oChecks As New clsChecks
        gvChecksPrint.DataSource = oChecks.Get_Printable_Checks(siLocation.Selected_ID)
        Dim sKeys(0) As String
        sKeys(0) = "PremiumIssuedID"
        gvChecksPrint.DataKeyNames = sKeys
        gvChecksPrint.DataBind()
    End Sub

    Protected Sub Void_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Void_Link.Click
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub Lookup_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lookup_Link.Click
        MultiView1.ActiveViewIndex = 3
    End Sub

    Protected Sub Report_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Report_Link.Click
        MultiView1.ActiveViewIndex = 4
    End Sub


    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMsg.Text = ""
        If siLocation.Selected_ID < 1 Then
            lblMsg.Text = "Please Select a Location."
        ElseIf txtStartNum.Text = "" Or txtEndNum.Text = "" Then
            lblMsg.Text = "Please Enter a Starting and Ending Number."
        Else
            Dim oChecks As New clsChecks
            If oChecks.Validate_Check_Range(CInt(txtStartNum.Text), CInt(txtEndNum.Text)) Then
                Dim pos As Integer = ddPagePos.SelectedValue
                Dim i As Integer
                For i = CInt(txtStartNum.Text) To CInt(txtEndNum.text)
                    If oChecks.Add_Check(i, pos, siLocation.Selected_ID) Then
                        If pos = 3 Then
                            pos = 1
                        Else
                            pos = pos + 1
                        End If
                    Else
                        lblMsg.Text = oChecks.Err
                        Exit For
                    End If
                Next
                If lblMsg.Text = "" Then
                    lblMsg.Text = "Checks Added."
                End If
            Else
                lblMsg.Text = "Checks already exist within the requested range."
            End If
            oChecks = Nothing
        End If
    End Sub

    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oChecks As New clsChecks
        gvChkLookup.DataSource = oChecks.Check_Lookup(txtLookup.Text)
        gvChkLookup.DataBind()
        oChecks = Nothing
    End Sub

    Protected Sub Unnamed4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oChecks As New clsChecks
        gvCheckRpt.DataSource = oChecks.Check_Report(dteStartDate.Selected_Date, dteEndDate.Selected_Date, siLocation.Selected_ID)
        gvCheckRpt.DataBind()
        oChecks = Nothing
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oChecks As New clsChecks
        If oChecks.Validate_Check_Void(txtVoidNum.Text) Then
            oChecks.UserID = Session("UserDBID")
            If oChecks.Void_Check(txtVoidNum.Text) Then
                lblVoid.Text = "Check Voided."
            Else
                lblVoid.Text = oChecks.Err
            End If
        Else
            lblVoid.Text = "Check Does Not Exist."
        End If
        oChecks = Nothing
    End Sub

    Protected Sub chkPrint_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oChecks As New clsChecks
        Dim sChecks() As String = Nothing
        Dim sErr As String = ""
        Dim bProceed As Boolean = True
        Dim i As Integer = 0
        For Each row As GridViewRow In gvChecksPrint.Rows
            row.Cells(5).Text = ""
        Next
        If txtChksStart.Text <> "" Then
            If IsNumeric(txtChksStart.Text) Then
                sChecks = oChecks.Get_Available_Checks(CInt(txtChksStart.Text))
            Else
                sErr = "Please Enter a Valid Starting Number"
                bProceed = False
                Dim cb As CheckBox = sender
                cb.Checked = False
            End If
        Else
            sChecks = oChecks.Get_Available_Checks(0)
        End If
        If bProceed Then
            If sChecks(0) = "Err" Then
                sErr = "No Checks Available to Print. Please Add More to Inventory."
                Dim cb As CheckBox = sender
                cb.Checked = False
            Else
                For Each row As Gridviewrow In gvChecksPrint.Rows
                    Dim cb As CheckBox = row.FindControl("chkPrint")
                    If cb.Checked Then
                        If bProceed Then
                            If i > UBound(sChecks) Then
                                sErr = "Please Add More Checks To Inventory"
                                bProceed = False
                                cb.Checked = False
                            Else
                                row.Cells(5).text = sChecks(i)
                            End If
                            i = i + 1
                        Else
                            cb.Checked = False
                        End If
                    End If
                Next
            End If
        End If
        If sErr <> "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub gvChecksPrint_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub Unnamed2_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkNos As String = ""
        Dim IDs As String = ""
        For Each Row As GridViewRow In gvChecksPrint.Rows
            Dim cb As CheckBox = Row.FindControl("chkPrint")
            If cb.Checked Then
                If IDs = "" Then
                    IDs = Row.Cells(1).Text
                Else
                    IDs = IDs & "," & Row.Cells(1).Text
                End If
                If checkNos = "" Then
                    checkNos = Row.Cells(5).Text
                Else
                    checkNos = checkNos & "," & Row.Cells(5).Text
                End If
            End If
        Next
        If IDs = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Err');", True)
        Else
            If CheckSecurity("Checks", "Print", , , Session("UserDBID")) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('printChecks.asp?IDs=" & IDs & "&checks=" & checkNos & "&CheckDate=" & dtechkDate.Selected_Date & "&loc=" & siLocation.Selected_ID & "&UserID=" & Session("UserDBID") & "');", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
            End If
        End If
    End Sub
End Class
