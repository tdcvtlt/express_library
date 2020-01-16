Imports System.Data

Partial Class general_EditPersonnelTrans
    Inherits System.Web.UI.Page
    Dim oPT As New clsPersonnelTrans
    Dim dtPT As DataTable
    Dim dr As DataRow
    Dim bNew As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtPT = Session("Commissions")
        Select_Row()
        If Not IsPostBack Then
            Select_Item1.Connection_String = Resources.Resource.cns
            Select_Item1.Load_Items()
            Dim oPers As New clsPersonnel
            ddlPersonnel.DataSource = oPers.List
            ddlPersonnel.DataTextField = "Name"
            ddlPersonnel.DataValueField = "ID"
            ddlPersonnel.DataBind()
            ddlPersonnel.SelectedValue = dr("PersonnelID")
            txtPersonnelTransID.Text = dr("ID")
            txtCP.Text = dr("Percentage")
            txtFA.Text = dr("FixedAmount")
            Select_Item1.Selected_ID = dr("TitleID")
            dfDateCreated.Selected_Date = "" 'oPT.Date_Created
            dfDatePosted.Selected_Date = "" 'oPT.Date_Posted
        End If
    End Sub

    Private Sub Select_Row()
        Dim id As Integer = -1
        For i = 0 To dtPT.Rows.Count - 1
            If id = dtPT.Rows(i).Item("ID") Then id -= 1
            If dtPT.Rows(i).Item("ID") = Request("ID") Then
                dr = dtPT.Rows(i)
                bNew = False
                Exit For
            End If
        Next
        If dr Is Nothing Then
            dr = dtPT.NewRow
            dr("PersonnelID") = 0
            dr("ID") = id
            dr("Percentage") = 0
            dr("FixedAmount") = 0
            dr("TitleID") = 0
            bNew = True
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        
        dr("PersonnelID") = ddlPersonnel.SelectedValue
        dr("TitleID") = Select_Item1.Selected_ID
        dr("Percentage") = IIf(IsNumeric(txtCP.Text), txtCP.Text, 0)
        dr("FixedAmount") = IIf(IsNumeric(txtFA.Text), txtFA.Text, 0)
        'dr("DateCreated") = dfDateCreated.Selected_Date
        'dr("DatePosted") = dfDatePosted.Selected_Date
        Dim aName() As String = Split(ddlPersonnel.SelectedItem.Text, ",")
        dr("LastName") = Trim(aName(LBound(aName)))
        dr("FirstName") = Trim(aName(UBound(aName)))
        dr("Title") = Select_Item1.SelectedName
        dr("Dirty") = True
        If bNew Then dtPT.Rows.Add(dr)
        Session("Commissions") = dtPT
        ClientScript.RegisterClientScriptBlock(Me.GetType, "SetValue", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');", True)
        Close()
    End Sub
    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

End Class
