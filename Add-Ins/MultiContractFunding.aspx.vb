Imports System.Data
Partial Class Add_Ins_MultiContractFunding
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim oCon As New clsContract
        If txtContract.Text = "" Or Not (oCon.Verify_Contract(txtContract.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
        ElseIf txtDeed.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Deed');", True)
        ElseIf txtDOT.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a DOT');", True)
        Else
            For Each row As GridViewRow In gvFunding.Rows
                If row.Cells(1).Text = txtContract.Text Then
                    bProceed = False
                    Exit For
                End If
            Next
            If bProceed Then
                Dim dt As New DataTable
                Dim dr As DataRow
                dt = Session("fundingTable")
                dr = dt.NewRow
                dr("ContractNumber") = txtContract.Text
                dr("Deed") = txtDeed.Text
                dr("DOT") = txtDOT.Text
                dt.Rows.Add(dr)
                Session("fundingTable") = dt
                gvFunding.DataSource = dt
                gvFunding.DataBind()
                txtContract.text = ""
                txtDeed.Text = ""
                txtDOT.Text = ""
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Contract Has Already Been Added.');", True)
            End If
        End If
        oCon = Nothing
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim dt As New DataTable
            dt.Columns.Add("ContractNumber")
            dt.Columns.Add("Deed")
            dt.Columns.Add("DOT")
            Session("fundingTable") = dt
        Else
            gvFunding.DataSource = Session("fundingTable")
            gvFunding.DataBind()
        End If
    End Sub

    Protected Sub gvFunding_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvFunding.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("fundingTable")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("fundingTable") = dt
        gvFunding.DataSource = Session("fundingTable")
        gvFunding.DataBind()
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If txtFunding.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Funding Number.');", True)
        ElseIf dteRecordingDate.Selected_Date.toString = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Recording Date.');", True)
        Else
            Dim dt As New DataTable
            Dim drow As DataRow
            dt = Session("fundingTable")
            Dim i As Integer
            If dt.Rows.Count = 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Funding Item.');", True)
            Else
                Dim oCon As New clsContract
                Dim oUserField As New clsUserFields
                For i = 0 To dt.Rows.Count - 1
                    drow = dt.Rows(i)
                    'Funding
                    oUserField.ID = 0
                    oUserField.Load()
                    oUserField.KeyValue = oCon.Get_Contract_ID(drow("ContractNumber"))
                    oUserField.UFValue = txtFunding.Text
                    oUserField.UFID = oUserField.Get_UserFieldID(oUserField.Get_GroupID("Contract"), "Funding Number")
                    oUserField.Save()
                    'Rec Date
                    oUserField.ID = 0
                    oUserField.Load()
                    oUserField.KeyValue = oCon.Get_Contract_ID(drow("ContractNumber"))
                    oUserField.UFValue = dteRecordingDate.Selected_Date
                    oUserField.UFID = oUserField.Get_UserFieldID(oUserField.Get_GroupID("Contract"), "Deed Rec Date")
                    oUserField.Save()
                    'Deed
                    oUserField.ID = 0
                    oUserField.Load()
                    oUserField.KeyValue = oCon.Get_Contract_ID(drow("ContractNumber"))
                    oUserField.UFValue = drow("Deed")
                    oUserField.UFID = oUserField.Get_UserFieldID(oUserField.Get_GroupID("Contract"), "Deed Instrument #")
                    oUserField.Save()
                    'DOT
                    oUserField.ID = 0
                    oUserField.Load()
                    oUserField.KeyValue = oCon.Get_Contract_ID(drow("ContractNumber"))
                    oUserField.UFValue = drow("DOT")
                    oUserField.UFID = oUserField.Get_UserFieldID(oUserField.Get_GroupID("Contract"), "Deed-of-Trust Instr #")
                    oUserField.Save()
                Next
                oCon = Nothing
                oUserField = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Funding Items Added.');", True)
                Response.Redirect("MultiContractFunding.aspx")
            End If
        End If
    End Sub
End Class
