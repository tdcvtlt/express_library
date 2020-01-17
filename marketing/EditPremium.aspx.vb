
Imports System.Data.SqlClient

Partial Class marketing_EditPremium
    Inherits System.Web.UI.Page
    Dim oPremium As New clsPremium

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CInt(txtPremiumID.Text) > 0 Then
            If CheckSecurity("Premiums", "Edit", , , Session("UserDBID")) Then
                Save()
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "AD", "alert('Access to Modify a premium is denied');", True)
            End If
        Else
            If CheckSecurity("Premiums", "Add", , , Session("UserDBID")) Then
                Save()
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "AD", "alert('Access to Add a premium is denied');", True)
            End If
        End If

    End Sub

    Protected Sub Save()
        oPremium.PremiumID = txtPremiumID.Text
        oPremium.Load()
        oPremium.UserID = Session("UserDBID")
        oPremium.PremiumID = txtPremiumID.Text
        oPremium.PremiumName = txtPremiumName.Text
        oPremium.Description = txtDescription.Text
        oPremium.Cost = txtCost.Text
        oPremium.CBCost = txtCBCost.Text
        If CheckSecurity("Premiums", "EditQtyOnHand", , , Session("UserDBID")) Then oPremium.QtyOnHand = txtQtyOnHand.Text
        oPremium.TypeID = siType.Selected_ID
        oPremium.LocationID = 1
        oPremium.Active = ckActive.Checked
        oPremium.Save()

        '05/20/2019
        'updates the CostEA for each row in the t_PackageTourPremium table for this premium
        Using cn As New SqlConnection(Resources.Resource.cns)
            Using cm As New SqlCommand(String.Format("update t_PackageTourPremium set CostEA = {0} where PremiumID = {1}", oPremium.Cost, oPremium.PremiumID), cn)
                Try
                    cn.Open()
                    cm.ExecuteNonQuery()
                Catch ex As Exception
                    cn.Close()
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        If oPremium.Error_Message <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Error", "alert('" & Replace(oPremium.Error_Message, "'", "\'") & "');", True)
        Else
            Response.Redirect("Premiums.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Premiums", "View", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 0
                '*** Create view events *** '
                If IsNumeric(Request("PremiumID")) Then
                    If CInt(Request("PremiumID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("PremiumID", Request("PremiumID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("PremiumID", Request("PremiumID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '
                Load_Lookups()
                oPremium.PremiumID = IIf(IsNumeric(Request("PremiumID")), Request("PremiumID"), 0)
                oPremium.Load()
                Set_Fields()
                'Label6.Text = oPros.Error_Message
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "AD", "alert('Access to View a premium is denied');", True)
            End If
        End If
    End Sub

    Protected Sub Set_Fields()
        txtPremiumID.Text = oPremium.PremiumID
        txtPremiumName.Text = oPremium.PremiumName
        txtDescription.Text = oPremium.Description
        txtCost.Text = FormatCurrency(oPremium.Cost).ToString.Replace("$", "")
        txtCBCost.Text = FormatCurrency(oPremium.CBCost).ToString.Replace("$", "")
        txtQtyOnHand.Text = oPremium.QtyOnHand
        siType.Selected_ID = oPremium.TypeID
        ckActive.Checked = oPremium.Active
    End Sub

    Protected Sub Load_Lookups()
        siType.Connection_String = Resources.Resource.cns
        siType.Label_Caption = ""
        siType.ComboItem = "PremiumType"
        siType.Load_Items()
    End Sub

    Protected Sub Premium_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Premium.Click
        If CheckSecurity("Premiums", "View", , , Session("UserDBID")) Then MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Events_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events.Click
        If txtPremiumID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Events1.KeyField = "PremiumID"
            Events1.KeyValue = txtPremiumID.Text
            Events1.List()
        End If
    End Sub
End Class
