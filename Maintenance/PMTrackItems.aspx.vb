Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading

Partial Class Maintenance_PMTrackItems
    Inherits System.Web.UI.Page

    Private MyAccess As New Access()

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init


        ddlCategory.DataSource = (New Access()).GetTrackCategories
        ddlCategory.DataTextField = "value"
        ddlCategory.DataValueField = "key"
        ddlCategory.DataBind()

    End Sub

    Private Class Access

        Private _category As Int32

        Public Sub New()
        End Sub

        Public ReadOnly Property GetTrackCategories As List(Of KeyValuePair(Of Int32, String))
            Get

                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using ad = New SqlDataAdapter("select comboItemID, comboItem from t_ComboItems where ComboID in " & _
                                                  "(select ComboID from t_Combos where ComboName = 'PMCategoryTrackItem')  " & _
                                                  "and active = 1 order by comboitem", cn)
                        Dim dt = New DataTable()
                        ad.Fill(dt)
                        Dim l As New List(Of KeyValuePair(Of Int32, String))
                        For Each dr As DataRow In dt.Rows
                            l.Add(New KeyValuePair(Of Int32, String)(dr("comboItemID"), dr("ComboItem")))
                        Next
                        Return l
                    End Using
                End Using
            End Get
        End Property

        Public ReadOnly Property GetCycle As List(Of KeyValuePair(Of Int32, String))
            Get
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using ad = New SqlDataAdapter("select comboItemID, comboItem from t_ComboItems where ComboID in " & _
                                                  "(select ComboID from t_Combos where ComboName = 'PMCycle')  " & _
                                                  "and active = 1 order by comboitem", cn)
                        Dim dt = New DataTable()
                        ad.Fill(dt)
                        Dim l As New List(Of KeyValuePair(Of Int32, String))
                        For Each dr As DataRow In dt.Rows
                            l.Add(New KeyValuePair(Of Int32, String)(dr("comboItemID"), dr("ComboItem")))
                        Next
                        Return l
                    End Using
                End Using                
            End Get
        End Property

        Public WriteOnly Property SetCategory As Int32
            Set(value As Int32)
                _category = value
            End Set
        End Property

        Public ReadOnly Property GetData As List(Of TrackItem)
            Get
                Dim l As New List(Of TrackItem)               
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using ad = New SqlDataAdapter(String.Format("select * from t_PMCategoryTrackItem where CategoryID = {0}", _category), cn)
                        Dim dt = New DataTable()
                        ad.Fill(dt)                        
                        For Each dr As DataRow In dt.Rows
                            l.Add( _
                                New TrackItem With { _
                                    .ID = dr("CategoryTrackItemID"), _
                                    .CategoryId = dr("CategoryID"), _
                                    .Name = dr("Name"), _
                                    .CycleId = dr("CycleID")})
                        Next
                        Return l
                    End Using
                End Using
            End Get
        End Property
    End Class

    Private Class TrackItem

        Private _id As Int32
        Private _name As String
        Private _cycleId As Int32
        Private _categoryId As Int32

        Public Sub New()
        End Sub
        Public Property ID As Int32
            Get
                Return _id
            End Get
            Set(value As Int32)
                _id = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Public Property CycleId As Int32
            Get
                Return _cycleId
            End Get
            Set(value As Int32)
                _cycleId = value
            End Set
        End Property

        Public Property CategoryId As Int32
            Get
                Return _categoryId
            End Get
            Set(value As Int32)
                _categoryId = value
            End Set
        End Property
    End Class



    Protected Sub gvGridView_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGridView.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddl As DropDownList = CType(gvr.FindControl("ddl1"), DropDownList)
            ddl.DataSource = (New Access()).GetCycle
            ddl.DataTextField = "value"
            ddl.DataValueField = "key"
            ddl.DataBind()

            Dim key = gv.DataKeys(e.Row.RowIndex).Value
            Dim re = (From trk As TrackItem In MyAccess.GetData _
                      Where trk.ID = key _
                      Select trk).Single()

            ddl.SelectedValue = re.CycleId

            Dim tb As TextBox = CType(gvr.FindControl("tbx1"), TextBox)
            tb.Text = re.Name

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            Dim ddl As DropDownList = CType(gvr.FindControl("ddl2"), DropDownList)
            ddl.DataSource = (New Access()).GetCycle
            ddl.DataTextField = "value"
            ddl.DataValueField = "key"
            ddl.DataBind()
        End If
    End Sub

    Protected Sub submit_Click(sender As Object, e As System.EventArgs) Handles submit.Click

        MyAccess.SetCategory = ddlCategory.SelectedValue
        gvGridView.DataSource = MyAccess.GetData
        gvGridView.DataBind()

    End Sub

    
    Protected Sub gvGridView_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGridView.RowCommand

        If e.CommandName = "List" Then

        ElseIf e.CommandName = "SaveChanges" Then
            Response.Write(e.CommandName)
        ElseIf e.CommandName = "SaveNew" Then
            Response.Write(e.CommandName)
        End If

        If e.CommandName = "SaveChanges" Or e.CommandName = "SaveNew" Then
            submit_Click(Nothing, EventArgs.Empty)
        End If

    End Sub

    Protected Sub gvGridView_PreRender(sender As Object, e As System.EventArgs) Handles gvGridView.PreRender
        With CType(sender, GridView)
            .UseAccessibleHeader = True                       
        End With
    End Sub
End Class


