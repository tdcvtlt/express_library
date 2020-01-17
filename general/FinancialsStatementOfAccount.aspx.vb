Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System


Partial Class general_FinancialsStatementOfAccount
    Inherits System.Web.UI.Page




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim keyfield As String = Request.QueryString("KeyField")


        If keyfield.Equals("ContractID") = False Then
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "06-20-2012", "window.close();", True)
        End If

        Dim contractid As String = Request.QueryString("KeyValue")                
        Dim prospectid As String = Request.QueryString("ProspectId")

        Dim sql As String = String.Format( _
                "select Distinct(i.id),i.Invoice_Description, i.Amount, i.transdate, (Select Sum(Amount + Adjustment) * -1 from v_Payments where invoiceid = i.ID) as Credits   " & _
                "FROM v_Invoices i " & _
                "where i.keyvalue={0} and i.keyfield = 'contractid' " & _
                "and i.amount > 0 " & _
                "order by i.transdate", contractid)

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sql, cnn)

                Dim INVOICES As List(Of Invoice) = New List(Of Invoice)()

                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader()

                If (rdr.HasRows = True) Then
                    Do While (rdr.Read() = True)
                        INVOICES.Add(New Invoice With {.Id = rdr.Item("ID"), _
                                                       .Description = IIf(rdr.Item("INVOICE_DESCRIPTION").Equals(DBNull.Value) = False, rdr.Item("INVOICE_DESCRIPTION"), 0), _
                                                       .Date = IIf(rdr.Item("TRANSDATE").Equals(DBNull.Value) = False, String.Format("{0:d}", rdr.Item("TRANSDATE")), String.Empty), _
                                                       .Amount = IIf(rdr.Item("AMOUNT").Equals(DBNull.Value) = False, rdr.Item("AMOUNT"), 0)})
                    Loop

                    rdr.Close()

                    cmd.CommandText = String.Format( _
                        "select i.id,i.Invoice_Description, i.Amount, i.transdate as invtransdate,p.transdate, (p.amount + p.adjustment) *-1 as credit,p.method " & _
                        "from v_Payments p " & _
                        "inner join v_Invoices i on i.id= p.invoiceid " & _
                        "where i.keyvalue='{0}' and i.keyfield = 'contractid' " & _
                        "and p.amount + p.adjustment <> 0 " & _
                        "order by i.id, p.transdate asc", contractid)

                    rdr = cmd.ExecuteReader()
                    If (rdr.HasRows = True) Then
                        Do While (rdr.Read() = True)

                            INVOICES.Where(Function(x) x.Id.Equals(Convert.ToInt32(rdr.Item("Id")))).Single().Payments.Add( _
                                New Payments With {.Id = rdr.Item("Id"), _
                                                   .Method = IIf(rdr.Item("Method").Equals(DBNull.Value) = False, rdr.Item("Method"), String.Empty), _
                                                   .Date = IIf(rdr.Item("TRANSDATE").Equals(DBNull.Value) = False, String.Format("{0:d}", rdr.Item("TRANSDATE")), String.Empty), _
                                                   .Amount = IIf(rdr.Item("CREDIT").Equals(DBNull.Value) = False, rdr.Item("CREDIT"), 0)})
                        Loop
                    End If


                    sql = String.Format( _
                                       "select top 1 a.FirstName + ' ' + a.LastName 'Guest', coalesce(b.address1, b.address2) Street, b.City, b.PostalCode, " & _
                                       "(select comboitem from t_comboitems where comboitemid =  b.stateid) State, c.ContractNumber KCP from t_prospect a " & _
                                       "inner join t_prospectaddress b on a.prospectid = b.prospectid " & _
                                       "inner join t_Contract c on c.ProspectId = a.ProspectId " & _
                                       "where(a.prospectid = {0} and c.ContractID = {1} And b.activeflag = 1) " & _
                                       "order by address1 desc", prospectid, contractid)

                    rdr.Close()
                    cmd.CommandText = sql
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    rdr.Read()
                    If (rdr.HasRows = True) Then
                        Me.Guest.Text = String.Format("{0}", IIf(rdr.Item("GUEST").Equals(DBNull.Value) = False, rdr.Item("GUEST"), String.Empty))

                        Me.Street.Text = String.Format("{0}", IIf(rdr.Item("STREET").Equals(DBNull.Value) = False, rdr.Item("STREET"), String.Empty))
                        Me.CityStateZip.Text = _
                            String.Format("{0},  ", IIf(rdr.Item("CITY").Equals(DBNull.Value) = False, rdr.Item("CITY"), String.Empty)) & _
                            String.Format("{0}  ", IIf(rdr.Item("STATE").Equals(DBNull.Value) = False, rdr.Item("STATE"), String.Empty)) & _
                            String.Format("{0}", IIf(rdr.Item("POSTALCODE").Equals(DBNull.Value) = False, rdr.Item("POSTALCODE"), String.Empty))
                        Me.Kcp.Text = String.Format("KCP # {0}", IIf(rdr.Item("KCP").Equals(DBNull.Value) = False, rdr.Item("KCP"), String.Empty))
                    End If
                End If


                Dim htmlBODY As New StringBuilder()
                Dim htmlHEADER As New StringBuilder()
                Dim htmlFOOTER As New StringBuilder()

                Dim balance As Decimal = 0

                For Each i As Invoice In INVOICES

                    balance += i.Amount
                    htmlBODY.AppendFormat("<tr><td>{0}</td><td>{1}</td><td style='text-align:right;'>{2:c}</td><td>{3}</td><td style='text-align:right;'>{4:c}</td></tr>", _
                                          i.Date, i.Description, i.Amount, String.Empty, balance)

                    For Each p As Payments In i.Payments

                        balance -= p.Amount
                        htmlBODY.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td style='text-align:right;'>{3:c}</td><td style='text-align:right;'>{4:c}</td></tr>", _
                                          p.Date, p.Method, String.Empty, p.Amount, balance)
                    Next
                Next


                htmlFOOTER.AppendFormat("<tr><td colspan='5' style='text-align:right;'><h1>Balance Due: {0}</h1></td></tr>", String.Format("{0:c}", balance))

                Dim header() As String = {"DATE", "DESCRIPTION", "CHARGES", "CREDITS", "ACCOUNT BALANCE"}

                htmlHEADER.Append("<table style='border-collapse:collapse;' border='1px'>")
                htmlHEADER.Append("<tr>")
                For Each h In header

                    Dim style As String = "text-align:right;padding:5px;width:180px;"
                    Dim index As Integer = Array.IndexOf(header, h)

                    If (index = 1) Then
                        style = "text-align:left;padding:5px;width:200px;"
                    End If

                    htmlHEADER.AppendFormat("<td style='{0}'><strong>{1}</strong></td>", style, h)
                Next
                htmlHEADER.Append("</tr>")
                htmlHEADER.Append(htmlBODY.ToString())
                htmlHEADER.Append(htmlFOOTER.ToString())


                LIT.Text = htmlHEADER.ToString()

            End Using
        End Using



    End Sub



    Private Class Invoice
        Private _Id As Integer
        Private _Description As String
        Private _Amount As Decimal
        Private _Date As String

        Private _Payments As List(Of Payments) = New List(Of Payments)
   
        Public Property [Date]() As String
            Get
                Return _Date
            End Get
            Set(ByVal value As String)
                _Date = value
            End Set
        End Property

        Public Property Amount() As Decimal
            Get
                Return _Amount
            End Get
            Set(ByVal value As Decimal)
                _Amount = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property

        Public Property Payments() As List(Of Payments)
            Get
                Return _Payments
            End Get
            Set(ByVal value As List(Of Payments))
                _Payments = value
            End Set
        End Property




    End Class


    Private Class Payments
        Private _Id As Integer
        Private _Method As String
        Private _Amount As Decimal
        Private _Date As String

   

        Public Property [Date]() As String
            Get
                Return _Date
            End Get
            Set(ByVal value As String)
                _Date = value
            End Set
        End Property

        Public Property Amount() As Decimal
            Get
                Return _Amount
            End Get
            Set(ByVal value As Decimal)
                _Amount = value
            End Set
        End Property

     

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property

        Public Property Method() As String
            Get
                Return _Method
            End Get
            Set(ByVal value As String)
                _Method = value
            End Set
        End Property

    End Class
End Class


