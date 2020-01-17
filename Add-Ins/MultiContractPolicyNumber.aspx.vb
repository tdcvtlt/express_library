Imports System.Data
Partial Class Add_Ins_MultiContractPolicyNumber
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim oCon As New clsContract
        If txtContract.Text = "" Or Not (oCon.Verify_Contract(txtContract.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Contract');", True)
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
                dr("PN") = txtPolicy.Text
                dt.Rows.Add(dr)
                Session("fundingTable") = dt
                gvFunding.DataSource = dt
                gvFunding.DataBind()
                txtContract.Text = ""
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Contract Has Already Been Added.');", True)
            End If
        End If
        oCon = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim dt As New DataTable
            dt.Columns.Add("ContractNumber")
            dt.Columns.Add("PN")
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

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtPolicy.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Policy Number.');", True)
        Else
            Dim dt As New DataTable
            Dim drow As DataRow
            dt = Session("fundingTable")
            Dim i As Integer
            If dt.Rows.Count = 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Policy Item.');", True)
            Else
                Dim oCon As New clsContract
                Dim oUserField As New clsUserFields
                For i = 0 To dt.Rows.Count - 1
                    drow = dt.Rows(i)
                    'Policy
                    oUserField.ID = 0
                    oUserField.Load()
                    oUserField.KeyValue = oCon.Get_Contract_ID(drow("ContractNumber"))
                    oUserField.UFValue = drow("PN") 'txtPolicy.Text
                    oUserField.UFID = oUserField.Get_UserFieldID(oUserField.Get_GroupID("Contract"), "Policy Number")
                    oUserField.Save()
                Next
                oCon = Nothing
                oUserField = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Policy Numbers Added.');", True)
                Response.Redirect("MultiContractPolicyNumber.aspx")
            End If
        End If
    End Sub
End Class
