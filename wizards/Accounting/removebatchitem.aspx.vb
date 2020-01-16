
Imports System.Data.SqlClient

Partial Class wizards_Accounting_removebatchitem
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If siReason.Selected_ID > 0 Then
            Dim cn As New SqlConnection(Resources.Resource.cns)
            Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where batchid='" & Request("BID") & "' and contractid=(select contractid from t_Contract where contractnumber = '" & Request("ID") & "')", cn)
            Dim da As New SqlDataAdapter(cm)
            Dim ds As New DataSet
            da.Fill(ds, "KCP")
            For Each row In ds.Tables("KCP").Rows
                Dim oBI As New clsCancellationBatch2Contract
                oBI.Batch2ContractID = row("batch2contractid")
                oBI.Load()
                oBI.RemovedByID = Session("UserDBID")
                oBI.DateRemoved = Date.Now
                oBI.ReasonRemoved = siReason.Selected_ID
                oBI.Save()
                oBI = Nothing
                GC.Collect()
            Next
            ds = Nothing
            da = Nothing
            cm = Nothing
            cn = Nothing
            Dim oCont As New clsContract
            Dim oMort As New clsMortgage
            Dim oNote As New clsNotes
            Dim oBatch As New clsCancellationBatch
            oBatch.BatchID = Request("BID")
            oBatch.Load()
            oCont.ContractNumber = Request("ID")
            oCont.Load()
            oMort.ContractID = oCont.ContractID
            oMort.Load()
            oCont.StatusID = siCS.Selected_ID
            oCont.StatusDate = Date.Now
            oCont.SubStatusID = siCSS.Selected_ID
            oCont.UserID = Session("UserDBID")
            oCont.MaintenanceFeeStatusID = siMFS.Selected_ID
            oCont.Save()
            oMort.StatusID = siMS.Selected_ID
            oMort.StatusDate = Date.Now
            oMort.UserID = Session("UserDBID")
            oMort.Save()
            oNote.Note = "Removed KCP #" & oCont.ContractNumber & " From " & (New clsComboItems).Lookup_ComboItem(oBatch.TypeID) & " - " & siReason.SelectedName
            oNote.KeyField = "ContractID"
            oNote.KeyValue = oCont.ContractID
            oNote.UserID = Session("UserDBID")
            oNote.CreatedByID = Session("UserDBID")
            oNote.Save()
            oNote = Nothing
            oNote = New clsNotes
            oNote.Note = "Removed KCP #" & oCont.ContractNumber & " From " & (New clsComboItems).Lookup_ComboItem(oBatch.TypeID) & " - " & siReason.SelectedName
            oNote.KeyField = "ProspectID"
            oNote.KeyValue = oCont.ProspectID
            oNote.UserID = Session("UserDBID")
            oNote.CreatedByID = Session("UserDBID")
            oNote.Save()
            oNote = Nothing
            oMort = Nothing
            oCont = Nothing
            oBatch = Nothing
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbContracts','');window.close();", True)
        Else
            lblError.Text = "Please select a reason for removing this contract"
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Text = ""
        If Not (IsPostBack) Then
            siReason.Connection_String = Resources.Resource.cns
            siReason.ComboItem = "CancellationRemoveReason"
            siReason.Load_Items()

            siCS.Connection_String = Resources.Resource.cns
            siCS.ComboItem = "ContractStatus"
            siCS.Load_Items()

            siCSS.Connection_String = Resources.Resource.cns
            siCSS.ComboItem = "ContractSubStatus"
            siCSS.Load_Items()

            siMS.Connection_String = Resources.Resource.cns
            siMS.ComboItem = "MortgageStatus"
            siMS.Load_Items()

            siMFS.Connection_String = Resources.Resource.cns
            siMFS.ComboItem = "MaintenanceFeeStatus"
            siMFS.Load_Items()

            If Request("bid") = "" Or Request("ID") = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.close();", True)
            Else
                txtContractID.Text = Request("ID")
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where batchid='" & Request("BID") & "' and contractid = (select contractid from t_Contract where contractnumber ='" & Request("ID") & "')", cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                da.Fill(ds, "KCP")
                If ds.Tables("KCP").Rows.Count > 0 Then
                    siCS.Selected_ID = ds.Tables("KCP").Rows(0)("ContractStatus")
                    siCSS.Selected_ID = ds.Tables("KCP").Rows(0)("ContractSubStatus")
                    siMS.Selected_ID = ds.Tables("KCP").Rows(0)("MortgageStatus")
                End If
                ds = Nothing
                da = Nothing
                cm = Nothing
                cn = Nothing
            End If
        End If
    End Sub
End Class
