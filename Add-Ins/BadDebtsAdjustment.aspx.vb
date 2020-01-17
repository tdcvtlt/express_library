Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Web.Services
Imports System.IO
Imports System.Web.Script.Serialization

Partial Class Add_Ins_BadDebtsAdjustment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            mv1.SetActiveView(view2)
            rblType.SelectedIndex = 1
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using ad = New SqlDataAdapter("Select fintransid, tc.ComboItem as TransCode from t_FinTransCodes f inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID " & _
                    "where tt.ComboItem = 'MFTrans' or tt.ComboItem = 'COntractTrans'", cn)

                    Dim dt = New DataTable()
                    ad.Fill(dt)

                    ddInvoice.DataSource = dt
                    ddInvoice.DataTextField = "TransCode"
                    ddInvoice.DataValueField = "TransCode"
                    ddInvoice.DataBind()
                End Using
            End Using
        End If
        lblStatus.Text = ""       
    End Sub

    Protected Sub btnKcpAdd_Click(sender As Object, e As System.EventArgs) Handles btnKcpAdd.Click
        If tbKCP.Text.Trim().Length = 0 Then Return

        Dim oCont = New clsContract()
        oCont.ContractID = oCont.Get_Contract_ID(tbKCP.Text.Trim())

        If oCont.ContractID > 0 Then
            lbKCPs.Items.Add(New ListItem With {.Text = String.Format("{0}", tbKCP.Text.Trim()), _
                                                                      .Value = String.Format("{0}-{1}", oCont.ContractID, ddInvoice.SelectedValue)})
            tbKCP.Text = ""
        Else
            lblStatus.Text = "KCP not found!"
        End If
    End Sub

    Protected Sub btnKcpRemove_Click(sender As Object, e As System.EventArgs) Handles btnKcpRemove.Click
        Dim c = lbKCPs.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).ToList()
        For Each li As ListItem In c
            lbKCPs.Items.Remove(li)
        Next
    End Sub

    Protected Sub lbProcess_Click(sender As Object, e As System.EventArgs) Handles lbProcess.Click
        Dim counter As Long = 0
        If rblType.SelectedValue = "kcp" Then
            For Each li As ListItem In lbKCPs.Items
                counter += Process(li.Value.Split("-")(0), li.Value.Split("-")(1))
            Next
            lblStatus.Text = "Completed (" & counter & " contracts updated)"
        Else
           
            If FileIO.FileSystem.FileExists(hfFile.Value) Then
                Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
                Dim line1() As String = ts.ReadLine.Split(",")
                While Not (ts.EndOfStream)
                    Dim line() As String = ts.ReadLine.Split(",")
                    Dim oCont = New clsContract()
                    If cbIgnoreID.Checked Then
                        oCont.ContractID = oCont.Get_Contract_ID(line(ddlKcp.SelectedValue))
                    Else
                        oCont.ContractID = line(ddlKcpID.SelectedValue)
                    End If                    
                    counter += Process(oCont.ContractID, "")
                End While
                ts.Close()
                ts = Nothing
                lblStatus.Text = "Completed (" & counter & " contracts updated)"
                hfFile.Value = ""
            End If
        End If
        btnKcpRemove_Click(Nothing, EventArgs.Empty)
    End Sub

    Private Function Process(contractID As String, invoice As String) As Int16
        Dim counter = 0
        Dim sql = String.Empty
        If invoice.Length = 0 Then
            sql = String.Format("select * from v_invoices where keyfield='contractid' and keyvalue={0} and balance > 0", contractID)
        Else
            sql = String.Format("select * from v_invoices where keyfield='contractid' and invoice='{1}' and keyvalue={0} and balance > 0", contractID, invoice)
        End If
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sql, cn)
                Dim dt = New DataTable()
                Dim badDebt = 0, adjustment = 0

                adjustment = New clsComboItems().Lookup_ID("paymentmethod", "adjustment")
                badDebt = New clsComboItems().Lookup_ID("paymentmethod", "bad debt")

                Try
                    ad.Fill(dt)
                    For Each dr As DataRow In dt.Rows

                        Dim dueDate = DateTime.Parse(dr("duedate").ToString())
                        Dim balance = Decimal.Parse(dr("balance").ToString())
                        Dim paymentID = 0
                        Dim invoiceID = Int32.Parse(dr("ID").ToString())
                        Dim prospectID = 0

                        With New clsContract
                            .ContractID = dr("keyvalue").ToString()
                            .Load()
                            prospectID = .ProspectID
                        End With

                        If dueDate.CompareTo(DateTime.Today) < 0 Then
                            With New clsPayments
                                .PaymentID = 0
                                .Load()
                                .Adjustment = 1
                                .Amount = balance
                                .PosNeg = 1
                                .ApplyToID = 0
                                .MethodID = badDebt
                                .TransDate = DateTime.Today
                                .UserID = Session("userdbid")
                                .Save()
                                paymentID = .PaymentID
                            End With
                            With New clsPayment2Invoice
                                .Inv2PayID = 0
                                .Load()
                                .PaymentID = paymentID
                                .InvoiceID = invoiceID
                                .Amount = balance
                                .PosNeg = 1
                                .Save()
                            End With
                            counter = 1
                        Else
                            With New clsInvoices
                                .InvoiceID = 0
                                .FinTransID = 0
                                .Adjustment = 1
                                .PaymentMethodID = adjustment
                                .ApplyToID = invoiceID
                                .Amount = balance
                                .KeyField = "contractid"
                                .KeyValue = dr("keyvalue").ToString()
                                .ProspectID = prospectID
                                .UserID = Session("userdbid")
                                .TransDate = DateTime.Now
                                .Reference = "Adjustment"
                                .DueDate = DateTime.Now
                                .PosNeg = 1
                                .Save()
                            End With
                            counter = 1
                        End If
                    Next
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End Using
        End Using
        Return counter
    End Function

    Protected Sub rblType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rblType.SelectedIndexChanged
        If rblType.SelectedValue = "kcp" Then
            mv1.SetActiveView(view2)
        Else
            mv1.SetActiveView(view1)
        End If
    End Sub


    Private Sub Read_First_Line()
        lblStatus.Text = ""
        If FileIO.FileSystem.FileExists(hfFile.Value) Then
            Dim ts As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(hfFile.Value)
            Dim line1() As String = ts.ReadLine.Split(",")
            Dim li As New ListItem("Clear", -1)
            ddlKcpID.Items.Clear()
            ddlKcp.Items.Clear()           
            For i = 0 To UBound(line1)
                ddlKcpID.Items.Add(New ListItem(line1(i), i))
                ddlKcp.Items.Add(New ListItem(line1(i), i))                
            Next
            ts.Close()
            ts = Nothing
        End If
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As System.EventArgs) Handles btnUpload.Click
        If file1.FileName = "" Then
            lblStatus.Text = "No File"
            Exit Sub
        End If
        Dim sParentPath As String = "\\nndc\UploadedContracts\"
        Dim sFolder As String = ""
        Dim sMid As String = ""
        Dim sFileName As String = Left(file1.FileName, InStr(file1.FileName, ".") - 1)
        Dim sExt As String = Right(file1.FileName, Len(file1.FileName) - InStr(file1.FileName, ".") + 1)

        If sExt.ToUpper <> ".CSV" Then
            'ClientScript.RegisterClientScriptBlock(Me.GetType, "Type", "alert('Please select only comma separated value (CSV) files');", True)
            lblStatus.Text = "Please select only comma separated value (CSV) files"
            Exit Sub
        End If
        'save the file
        'Response.Write(UCase(_KeyField))
        sParentPath = "\\nndc\UploadedContracts\"
        sFolder = "misprojects\"

        Dim i As Integer = 0
        Do While FileIO.FileSystem.FileExists(sParentPath & sFolder & sFileName & sMid & sExt)
            i += 1
            sMid = i.ToString
        Loop
        file1.SaveAs(sParentPath & sFolder & sFileName & sMid & sExt)
        hfFile.Value = sParentPath & sFolder & sFileName & sMid & sExt
        Read_First_Line()        
    End Sub
End Class
