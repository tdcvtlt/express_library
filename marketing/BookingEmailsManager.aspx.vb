Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports System.Web.Services
Imports System.Web.Script.Serialization

Partial Class Maintenance_BookingEmailsManager
    Inherits System.Web.UI.Page




    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using ad = New SqlDataAdapter("select comboitemid, ComboItem from t_ComboItems where comboid in (select comboid from t_Combos " & _
                                              "where ComboName = 'resortcompany') and Active = 1 order by ComboItem", cn)
                    Dim dt = New DataTable()
                    ad.Fill(dt)

                    ddlResorts.AppendDataBoundItems = True
                    ddlResorts.Items.Add(New ListItem("resort companies", "-2"))
                    ddlResorts.DataSource = dt
                    ddlResorts.DataTextField = "ComboItem"
                    ddlResorts.DataValueField = "comboitemid"
                    ddlResorts.DataBind()
                    dt.Clear()
                End Using

                Using ad = New SqlDataAdapter("select comboitemid, ComboItem from t_ComboItems where comboid in (select comboid from t_Combos " & _
                                                    "where ComboName = 'reservationstatus') and Active = 1 order by ComboItem", cn)
                    Dim dt = New DataTable()
                    ad.Fill(dt)
                    ddlReservationStatuses.DataSource = dt
                    ddlReservationStatuses.DataTextField = "ComboItem"
                    ddlReservationStatuses.DataValueField = "comboitemid"
                    ddlReservationStatuses.DataBind()
                End Using


                Using ad = New SqlDataAdapter("select * from t_letters", cn)
                    Dim dt = New DataTable()
                    ad.Fill(dt)
                    ddlLetters.AppendDataBoundItems = True
                    ddlLetters.Items.Add(New ListItem("", "-2"))
                    ddlLetters.DataSource = dt
                    ddlLetters.DataTextField = "Name"
                    ddlLetters.DataValueField = "LetterID"
                    ddlLetters.DataBind()
                End Using
            End Using
        End If

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        Dim choice = (From c As ListItem In ddlResorts.Items.OfType(Of ListItem)() Where c.Selected).Single()
        If choice.Text.Trim() = "resort companies" Then Return
        lblResort.Text = choice.Text.ToUpper()

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(String.Format("select ReservationStatusID, EmailAddress, Active, LetterID, subject, " & _
                                        "EmailID, ResortCompanyID  from t_resortEmailManager where ResortCompanyID = {0} order by ReservationStatusID", choice.Value), cn)
                Dim dt = New DataTable()
                ad.Fill(dt)

                Try
                    With gv1

                        .DataSource = dt
                        .DataBind()

                        .UseAccessibleHeader = True
                        .HeaderRow.TableSection = TableRowSection.TableHeader

                    End With
                Catch ex As Exception
                    ex = Nothing
                End Try
            End Using

            Dim DIC_COLUMNS As New Dictionary(Of String, String)

            Using ada = New SqlDataAdapter(String.Format("select * from t_reservations r inner join t_prospect p on r.prospectid = p.prospectid where r.reservationid ={0}", 63759), cn)
                Dim dt1 = New DataTable()
                ada.Fill(dt1)

                For i = 0 To dt1.Columns.Count - 1                   

                    If dt1.Columns(i).DataType.ToString() = "System.DateTime" Then

                        If dt1.Rows(0)(i).Equals(DBNull.Value) = False Then
                            DIC_COLUMNS.Add(dt1.Columns(i).ColumnName.ToLower(), DateTime.Parse(dt1.Rows(0)(i).ToString()).ToShortDateString())
                        Else
                            DIC_COLUMNS.Add(dt1.Columns(i).ColumnName.ToLower(), dt1.Rows(0)(i).ToString())
                        End If

                    Else
                        DIC_COLUMNS.Add(dt1.Columns(i).ColumnName.ToLower(), dt1.Rows(0)(i).ToString())
                    End If                   
                Next

                DIC_COLUMNS.Add("currentdate", DateTime.Now.ToShortDateString())
            End Using
        End Using

    End Sub

    <WebMethod()> _
    Public Shared Function EmailTest(email_id As String, reservation_id As String, email_address As String) As String

        Dim recordAffected = "100"
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(String.Format("select l.letterid, name, typeid, l.LetterContent, rem.subject, rem.reservationstatusid from t_letters l " & _
                            "inner join t_comboitems ci on l.typeid = ci.comboitemid  " & _
                            "inner join t_combos c on ci.comboid = c.comboid " & _
                            "inner join t_resortEmailManager rem on rem.letterid = l.LetterID " & _
                            "where REM.emailid = {0} ", email_id), cn)

                Dim DIC_COLUMNS As New Dictionary(Of String, String)

                Try
                    Dim dt = New DataTable()
                    ad.Fill(dt)

                    Using ada = New SqlDataAdapter(String.Format("select * from t_reservations r inner join t_prospect p on r.prospectid = p.prospectid where r.reservationid ={0}", reservation_id), cn)
                        Dim dt1 = New DataTable()
                        ada.Fill(dt1)

                        For i = 0 To dt1.Columns.Count - 1

                            If dt1.Columns(i).DataType.ToString() = "System.DateTime" Then

                                If dt1.Rows(0)(i).Equals(DBNull.Value) = False Then
                                    DIC_COLUMNS.Add(dt1.Columns(i).ColumnName.ToLower(), String.Format("{0}", DateTime.Parse(dt1.Rows(0)(i).ToString()).ToShortDateString()))
                                Else
                                    DIC_COLUMNS.Add(dt1.Columns(i).ColumnName.ToLower(), String.Format("{0}", dt1.Rows(0)(i).ToString()))
                                End If

                            Else
                                DIC_COLUMNS.Add(dt1.Columns(i).ColumnName.ToLower(), String.Format("{0}", dt1.Rows(0)(i).ToString()))
                            End If
                        Next

                        DIC_COLUMNS.Add("currentdate", DateTime.Now.ToShortDateString())
                    End Using

                    For Each dr In dt.Rows
                        Dim o_letter As New clsLetters

                        Dim subject = dr("subject").ToString() '.ToLower()
                        Dim arr = subject.Split(" ")

                        For i = 0 To arr.Length - 1
                            If arr(i).Trim().IndexOf("<") >= 0 And arr(i).Trim().IndexOf(">") > 1 Then
                                Dim substr = arr(i).Trim().Substring(arr(i).Trim().IndexOf("<") + 1, arr(i).Trim().IndexOf(">") - 1)

                                For Each kvp As KeyValuePair(Of String, String) In DIC_COLUMNS
                                    If substr = kvp.Key Then
                                        If kvp.Key = "reslocationid" Then

                                            Using cm = New SqlCommand("select comboitem from t_comboitems where comboitemid = " & kvp.Value, cn)
                                                Try
                                                    cn.Open()

                                                    arr(i) = arr(i).Replace("<reslocationid>", cm.ExecuteScalar())
                                                Catch ex As Exception
                                                Finally
                                                    cn.Close()
                                                End Try
                                            End Using
                                        Else
                                            arr(i) = arr(i).Replace(String.Format("<{0}>", kvp.Key), kvp.Value)
                                        End If

                                    End If
                                Next

                            End If
                        Next

                        Dim content = String.Format("{0}", o_letter.Get_Letter_Text(dr("letterid").ToString(), reservation_id))
                        Send_Mail(email_address, "confirmation@vrcvacations.com", String.Join(" ", arr), content, True)

                        Dim ud = New clsUploadedDocs()
                        ud.DateUploaded = DateTime.Now
                        ud.KeyField = "reservationid"
                        ud.KeyValue = reservation_id
                        ud.Name = "confirmation email"
                        ud.ContentText = content

                        ud.Path = ""
                        'ud.Save()

                        ud = Nothing

                    Next
                Catch ex As Exception

                    recordAffected = ex.Message
                    ex = Nothing
                End Try
            End Using
        End Using
        Return recordAffected
    End Function

   
    <WebMethod()> _    
    Public Shared Function UpdateActive(emailID As String, active As Boolean) As String

        Dim recordAffected = -1
        Dim flag As Int16 = IIf(active = True, 1, 0)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(String.Format("update t_resortEmailManager set active = {0} where emailid = {1}", flag, emailID), cnn)

                Try
                    cnn.Open()
                    recordAffected = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Return ex.Message
                Finally
                    cnn.Close()
                End Try
                Return recordAffected
            End Using
        End Using
    End Function

    <WebMethod()> _
    Public Shared Function UpdateLetter(emailID As String, letterID As String) As String

        Dim recordAffected = -1
        
        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(String.Format("update t_resortEmailManager set letterid = {0} where emailid = {1}", letterID, emailID), cnn)

                Try
                    cnn.Open()
                    recordAffected = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Return ex.Message
                Finally
                    cnn.Close()
                End Try
                Return recordAffected
            End Using
        End Using
    End Function


    <WebMethod()> _
    Public Shared Function UpdateSubject(resortid As String, reservation_status_id As String, letter_id As String, subject As String) As String
        Dim recordAffected = -1

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(String.Format("update t_resortEmailManager set subject = '{0}' where ResortCompanyID = {1} " & _
                                                     "and ReservationStatusID = {2} and LetterID = {3}", subject, resortid, reservation_status_id, letter_id), cnn)

                Try
                    cnn.Open()
                    recordAffected = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Return ex.Message
                Finally
                    cnn.Close()
                End Try
                Return recordAffected
            End Using
        End Using
    End Function


    <WebMethod()> _
    Public Shared Function AddAccount(ResortCompanyID As String, EmailAddress As String, ReservationStatusID As String, letterID As String) As String

        Dim recordAffected = -1

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(String.Format("insert into t_resortEmailManager(ResortCompanyID, EmailAddress, ReservationStatusID, LetterID) Output Inserted.emailid " & _
                                                     "select {0}, '{1}', {2}, {3}", ResortCompanyID, EmailAddress, ReservationStatusID, letterID), cnn)

                Try
                    cnn.Open()
                    recordAffected = cmd.ExecuteScalar()
                Catch ex As Exception
                    Return ex.Message
                Finally
                    cnn.Close()
                End Try
                Return recordAffected
            End Using
        End Using
    End Function

    <WebMethod()> _
    Public Shared Function DeleteAccount(emailID As String) As String

        Dim recordAffected = -1

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(String.Format("delete from t_resortEmailManager where emailid = {0}", emailID), cnn)

                Try
                    cnn.Open()
                    recordAffected = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Return ex.Message
                Finally
                    cnn.Close()
                End Try
                Return recordAffected
            End Using
        End Using
    End Function


    <WebMethod()> _
    Public Shared Function PeekReservationByLastname(beginsWith As String) As String

        Dim js As New JavaScriptSerializer()
        Dim l As New List(Of Reservation)

        Dim sqlText As String = String.Empty
        Using cnn = New SqlConnection(Resources.Resource.cns)

            Dim ar = beginsWith.Split(","c)

            If ar.Length = 1 Then
                sqlText = String.Format("select top 10 r.ReservationID, p.LastName, p.FirstName from t_Reservations r " & _
                        "inner join t_Prospect p on r.ProspectID = p.ProspectID   " & _
                        "where p.LastName like '{0}%' order by p.FirstName", beginsWith.Trim())
            ElseIf ar.Length = 2 Then
                sqlText = String.Format("select top 10 r.ReservationID, p.LastName, p.FirstName from t_Reservations r " & _
                      "inner join t_Prospect p on r.ProspectID = p.ProspectID   " & _
                      "where p.LastName like '{0}%' and p.FirstName like '{1}%' order by p.FirstName", ar(0).Trim(), ar(1).Trim())
            End If

            If ar.Length = 1 Or ar.Length = 2 Then
                Using ada = New SqlDataAdapter(sqlText, cnn)

                    Try
                        Dim dt = New DataTable()
                        ada.Fill(dt)
                        For Each r As DataRow In dt.Rows
                            l.Add(New Reservation() With {.ReservationID = r("ReservationID"), .FirstName = r("FirstName").ToString(), .LastName = r("LastName").ToString()})
                        Next

                    Catch ex As Exception
                        Return ex.Message
                    Finally
                        cnn.Close()
                    End Try

                End Using

            End If

            Return js.Serialize(l)

        End Using
    End Function


    Private Class Reservation
        Private _reservationId As String
        Private _lastName As String
        Private _firstName As String

        Public Sub New()
        End Sub

        Public Property ReservationID As String
            Get
                Return _reservationId
            End Get
            Set(value As String)
                _reservationId = value
            End Set
        End Property

        Public Property LastName As String
            Get
                Return _lastName
            End Get
            Set(value As String)
                _lastName = value
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return _firstName
            End Get
            Set(value As String)
                _firstName = value
            End Set
        End Property
    End Class




End Class
