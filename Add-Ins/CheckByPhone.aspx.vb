Imports System
Imports System.Text
Imports System.Data
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq


Partial Class Add_Ins_CheckByPhone
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DateCompleted.Attributes("OnClick") = "scwShow(this,this)"
        DateToRun.Attributes("OnClick") = "scwShow(this,this)"

        If String.IsNullOrEmpty(hlBack.NavigateUrl) Then
            If Not Page.IsPostBack Then
                If Not Request Is Nothing Then
                    Try
                        hlBack.NavigateUrl = Request.UrlReferrer.PathAndQuery.ToString()
                    Catch ex As NullReferenceException
                        Response.Write(ex.Message)
                    End Try
                End If
            End If
        End If


        If Not IsPostBack Then
            h1Header.InnerText = IIf(String.IsNullOrEmpty(Request.QueryString("CheckByPhoneID")), _
                "Insert Payment By Phone", "Edit Payment By Phone")
            DataBound(Request.QueryString("CheckByPhoneID"))
        End If
    End Sub


    Private Sub DataBound(ByVal key As String)

        Dim sql As String = String.Empty
        Dim rdr As SqlDataReader = Nothing

        Using cnn As New SqlConnection(Resources.Resource.cns)

            cnn.Open()
            If Not Page.IsPostBack Then

                sql = "SELECT c.ComboItemID, c.ComboItem FROM t_ComboItems c INNER JOIN T_COMBOS B ON C.COMBOID = B.COMBOID WHERE ComboName = 'CheckPayStatus' and Active = '1' ORDER BY c.ComboItem"
                Using cmd As New SqlCommand(sql, cnn)
                    rdr = cmd.ExecuteReader()
                    If rdr.HasRows Then

                        StatusID.Items.Add(New ListItem("", ""))
                        StatusID.AppendDataBoundItems = True

                        StatusID.DataSource = rdr
                        StatusID.DataTextField = "COMBOITEM"
                        StatusID.DataValueField = "COMBOITEMID"
                        StatusID.DataBind()
                    End If
                End Using

                rdr.Close()
            End If


            If String.IsNullOrEmpty(key) Then Return


            'Store CHECKBYPHONEID key in hidden field control in case needed in Submit event
            hfKey.Value = key

            sql = "SELECT * FROM t_CheckByPhone WHERE CheckByPhoneID = '" & key & "'"

            Using cmd As New SqlCommand(sql, cnn)

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If rdr.HasRows Then

                    rdr.Read()

                    Me.AccountFirstName.Text = rdr.Item("AccountFirstName")
                    Me.AccountMiddleInit.Text = rdr.Item("AccountMiddleInit")
                    Me.AccountLastName.Text = rdr.Item("AccountLastName")
                    Me.RoutingNumber.Text = rdr.Item("RoutingNumber")
                    Me.AccountNumber.Text = rdr.Item("AccountNumber")

                    Me.CheckingFlag.Checked = rdr.Item("CheckingFlag")
                    Me.SavingsFlag.Checked = IIf(IsDBNull(rdr.Item("SavingsFlag")), 0, rdr.Item("SavingsFlag"))

                    Me.ContractNumber.Text = rdr.Item("ContractNumber")
                    Me.Amount.Text = rdr.Item("Amount")
                    Me.DateToRun.Text = rdr.Item("DateToRun")

                    Me.StatusID.SelectedValue = rdr.Item("StatusID")

                    Me.DateCompleted.Text = IIf(IsDBNull(rdr.Item("DateCompleted")), String.Empty, rdr.Item("DateCompleted"))
                    Me.TransactionID.Text = IIf(IsDBNull(rdr.Item("TransactionID")), String.Empty, rdr.Item("TransactionID").ToString().Trim())
                End If

            End Using
        End Using


    End Sub










    Protected Sub cmSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmSubmit.Click

        Dim status As String = Me.StatusID.SelectedValue
        Dim transaction As String = Me.TransactionID.Text.Trim()
        Dim dateComplete As String = Me.DateCompleted.Text
        Dim key As String = hfKey.Value.Trim()

        Dim firstName As String = AccountFirstName.Text.Trim()
        Dim lastName As String = AccountLastName.Text.Trim()
        Dim middleInit As String = AccountMiddleInit.Text.Trim()
        Dim routingNo As String = RoutingNumber.Text.Trim()
        Dim accountNo As String = AccountNumber.Text.Trim()

        Dim checkFlag As Integer = Iif(CheckingFlag.Checked, 1, 0)
        Dim saveFlag As Integer = Iif(SavingsFlag.Checked, 1, 0)

        Dim contractNo As String = ContractNumber.Text.Trim()
        Dim amt As Decimal = Decimal.Parse(Amount.Text.Trim())
        Dim dateRun As String = DateToRun.Text.Trim()



        Using cnn As New SqlConnection(Resources.Resource.cns)

            If String.IsNullOrEmpty(key) = False Then
                Dim sql As String = String.Format("UPDATE T_CHECKBYPHONE SET StatusID = '{0}', DateCompleted = '{1}', " & _
                                                  "TransactionID = '{2}', AccountLastName = '{3}', AccountMiddleInit = '{4}', " & _
                                                  "AccountFirstName = '{5}', RoutingNumber = '{6}', ContractNumber = '{7}', AccountNumber = '{8}', " & _
                                                  "CheckingFlag = {9}, SavingsFlag = {10}, Amount = {11}, DateToRun = '{12}' WHERE CheckByPhoneID = {13}", _
                                                  status, dateComplete, _
                                                  transaction, lastName, middleInit, _
                                                  firstName, routingNo, contractNo, accountNo, _
                                                  checkFlag, saveFlag, amt, dateRun, key)

                Using cmd As New SqlCommand(sql, cnn)
                    Try
                        cnn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    End Try
                End Using
            Else

                Dim sql As String = String.Format("INSERT INTO T_CHECKBYPHONE(" & _
                                "DateEntered, EnteredByID, AccountLastName, AccountMiddleInit, " & _
                                "AccountFirstName, ContractNumber, RoutingNumber, AccountNumber, " & _
                                "CheckingFlag, SavingsFlag, Amount, DateToRun, StatusID, " & _
                                "DateCompleted, TransactionID) VALUES ({0}, '{1}', '{2}', '{3}', " & _
                                "'{4}', '{5}', '{6}', '{7}', {8}, {9}, {10}, '{11}', '{12}', '{13}', '{14}')", _
                                "GetDate()", Session("USERDBID"), lastName, middleInit, _
                                firstName, contractNo, routingNo, accountNo, checkFlag, saveFlag, _
                                amt, dateRun, status, dateComplete, transaction)


                Using cmd As New SqlCommand(sql, cnn)
                    cnn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End If
        End Using

    End Sub
End Class
