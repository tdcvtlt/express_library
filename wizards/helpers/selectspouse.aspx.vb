Imports System.Data.SqlClient
Imports System.Data

Partial Class general_selectspouse
    Inherits System.Web.UI.Page

    'Public Structure CoOwner
    '    Dim StructID As Integer
    '    Dim ContractID As Integer
    '    Dim oPros As clsProspect
    '    Dim oAdd As clsAddress
    '    Dim oPhone As clsPhone
    '    Dim oEmail As clsEmail
    'End Structure

    Dim aCoOwners() As Structures.CoOwner

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuery.Click
        Select Case LCase(ddFilter.Text)
            Case "phone"
                If filter.Text = "" Then
                    Run_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%' order by ph.number")
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%'")
                End If

            Case "address"
                If filter.Text = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by Address1")
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.address1 like '" & filter.Text & "%'")
                End If

            Case "city"
                If filter.Text = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.City")
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.City like '" & filter.Text & "%'")
                End If

            Case "email"
                If filter.Text = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid order by e.Email")
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid where e.Email like '" & filter.Text & "%'  order by e.Email")
                End If

            Case "name"
                If filter.Text = "" Then
                    Run_Query("Select top 100 LastName, FirstName, ProspectID from t_Prospect order by Lastname, Firstname")
                Else
                    If InStr(filter.Text, ",") > 0 And InStr(filter.Text, ", ") < 1 Then filter.Text = Replace(filter.Text, ",", ", ")
                    Run_Query("Select top 100 LastName, FirstName, ProspectID from t_Prospect where lastname + ', ' + firstname like '" & filter.Text & "%'")
                End If

            Case "id"
                If filter.Text = "" Then
                    Run_Query("Select top 100 HomePhone, LastName, FirstName, ProspectID from t_Prospect order by Prospectid")
                Else
                    Run_Query("Select top 100 HomePhone, LastName, FirstName, ProspectID from t_Prospect where Prospectid like '" & filter.Text & "%'")
                End If

            Case "postalcode"
                If filter.Text = "" Then
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.PostalCode")
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.PostalCode like '" & filter.Text & "%'")
                End If

            Case "ssn"
                If filter.Text = "" Then
                    Run_Query("Select top 100 HomePhone, LastName, FirstName, ProspectID from t_Prospect order by SSN")
                Else
                    Run_Query("Select top 100 HomePhone, LastName, FirstName, ProspectID from t_Prospect where SSN like '" & filter.Text & "%'")
                End If

            Case "spousessn"
                If filter.Text = "" Then
                    Run_Query("Select top 100 HomePhone, LastName, FirstName, ProspectID from t_Prospect order by SpouseSSN")
                Else
                    Run_Query("Select top 100 HomePhone, LastName, FirstName, ProspectID from t_Prospect where SpouseSSN like '" & filter.Text & "%'")
                End If

            Case Else
                If filter.Text = "" Then
                    Run_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%' order by ph.number")
                Else
                    Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%'")
                End If

        End Select
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Update(GridView1.SelectedValue)
    End Sub

    Protected Sub GridView1_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles GridView1.SelectedIndexChanging

    End Sub

    Protected Sub ddFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter.SelectedIndexChanged

    End Sub

    Sub Run_Query(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Replace This", cn)
        Dim dr As SqlDataReader
        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            GridView1.DataSource = dr
            Dim ka(0) As String
            ka(0) = "prospectid"
            GridView1.DataKeyNames = ka
            GridView1.DataBind()
            cn.Close()
        Catch ex As Exception

        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack And rbExisting.Checked Then
            Button1_Click(sender, e)
        ElseIf Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 1
            siState.ComboItem = "State"
            siState.Connection_String = Resources.Resource.cns
            siState.Label_Caption = ""
            siState.Load_Items()
            If Request("isWiz") = "1" Then
                'Load the session structure
                If IsNumeric(Session("iCoOwners")) Then
                    If CInt(Session("iCoOwners")) > 0 Then
                        ReDim aCoOwners(CInt(Session("iCoOwners")))
                        'Array.Copy(Session("aCoOwners"), aCoOwners, CInt(Session("iCoOwners")))
                        aCoOwners = Session("aCoOwners")
                    End If
                Else
                    ReDim aCoOwners(0)
                End If
                'Session only information
                If Request("ID") <> "" Then
                    If CInt(Request("ID")) > UBound(aCoOwners) Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "window.close();", True)
                    Else
                        txtFirstName.Text = aCoOwners(Request("ID")).oPros.First_Name
                        txtLastName.Text = aCoOwners(Request("ID")).oPros.Last_Name
                        txtMiddleInit.Text = aCoOwners(Request("ID")).oPros.MiddleInit
                        txtSSN.Text = aCoOwners(Request("ID")).oPros.SSN
                        txtAddress.Text = aCoOwners(Request("ID")).oAdd.Address1
                        txtCity.Text = aCoOwners(Request("ID")).oAdd.City
                        siState.selected_id = aCoOwners(Request("ID")).oAdd.StateID
                        txtZip.Text = aCoOwners(Request("ID")).oAdd.PostalCode
                        txtHomePhone.Text = aCoOwners(Request("ID")).oPhone.Number
                        txtEmail.Text = aCoOwners(Request("ID")).oEmail.Email
                        MultiView1.ActiveViewIndex = 0
                        rbNew.Checked = True
                        rbExisting.Checked = False
                    End If

                End If
            ElseIf Request("isPri") = "1" Then

            Else

            End If
        End If
    End Sub

    Protected Sub rbExisting_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbExisting.CheckedChanged
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub rbNew_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbNew.CheckedChanged
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Update(0)
    End Sub

    Private Sub Update(ByVal ProsID As Integer)
        If Request("isWiz") = "1" Then
            'Load the session structure
            If IsNumeric(Session("iCoOwners")) Then
                If CInt(Session("iCoOwners")) > 0 Then
                    aCoOwners = Session("aCoOwners")
                End If
            Else
                Session("iCoOwners") = 0
            End If
            'Session only information
            Dim iIndex As Integer
            Dim bNew As Boolean = False
            If Request("ID") <> "" Then
                iIndex = Request("ID")
            Else
                bNew = True
                Session("iCoOwners") = CInt(Session("iCoOwners")) + 1
                ReDim Preserve aCoOwners(CInt(Session("iCoOwners")) - 1)
                iIndex = CInt(Session("iCoOwners")) - 1
                aCoOwners(iIndex).oPros = New clsProspect
                aCoOwners(iIndex).oAdd = New clsAddress
                aCoOwners(iIndex).oPhone = New clsPhone
                aCoOwners(iIndex).oEmail = New clsEmail
            End If

            If rbExisting.Checked Or aCoOwners(iIndex).oPros.Prospect_ID > 0 Then
                Dim oPros As New clsProspect
                oPros.Prospect_ID = IIf(ProsID > 0, ProsID, aCoOwners(iIndex).oPros.Prospect_ID)
                oPros.Load()
                aCoOwners(iIndex).oPros.Prospect_ID = ProsID
                'aCoOwners(iIndex).oPros.Load()
                aCoOwners(iIndex).oPros.First_Name = oPros.First_Name
                aCoOwners(iIndex).oPros.Last_Name = oPros.Last_Name

            Else
                aCoOwners(iIndex).oPros.Prospect_ID = 0
                aCoOwners(iIndex).oPros.First_Name = txtFirstName.Text
                aCoOwners(iIndex).oPros.Last_Name = txtLastName.Text
                aCoOwners(iIndex).oPros.MiddleInit = txtMiddleInit.Text
                aCoOwners(iIndex).oPros.SSN = txtSSN.Text
                aCoOwners(iIndex).oAdd.ProspectID = 0
                aCoOwners(iIndex).oAdd.AddressID = 0
                aCoOwners(iIndex).oAdd.Address1 = txtAddress.Text
                aCoOwners(iIndex).oAdd.City = txtCity.Text
                aCoOwners(iIndex).oAdd.StateID = sistate.selected_id
                aCoOwners(iIndex).oAdd.PostalCode = txtZip.Text
                aCoOwners(iIndex).oPhone.ProspectID = 0
                aCoOwners(iIndex).oPhone.PhoneID = 0
                aCoOwners(iIndex).oPhone.Number = txtHomePhone.Text
                aCoOwners(iIndex).oEmail.ProspectID = 0
                aCoOwners(iIndex).oEmail.EmailID = 0
                aCoOwners(iIndex).oEmail.Email = txtEmail.Text
            End If
            Session("aCoOwners") = aCoOwners
            Session("aCoOwners2") = aCoOwners
            Dim dtCoOwners As DataTable
            dtCoOwners = Session("CoOwners")
            Dim dr As DataRow
            If bNew Then
                dr = dtCoOwners.NewRow()
            Else
                dr = dtCoOwners.Rows(iIndex)
            End If

            dr("ID") = iIndex
            dr("FirstName") = aCoOwners(iIndex).oPros.First_Name
            dr("LastName") = aCoOwners(iIndex).oPros.Last_Name
            If bNew Then dtCoOwners.Rows.Add(dr)
            Session("CoOwners") = dtCoOwners

            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');window.close();", True)
        ElseIf Request("IsPri") = "1" Then
            Dim oPros As New clsProspect
            Dim oCon As New clsContract
            oCon.ContractID = Request("ContractID")
            oCon.Load()
            oCon.UserID = Session("UserDBID")
            oPros.UserID = Session("UserDBID")
            oPros.Prospect_ID = ProsID
            oPros.Load()
            If Not (rbExisting.Checked) Then
                oPros.First_Name = txtFirstName.Text
                oPros.Last_Name = txtLastName.Text
                oPros.MiddleInit = txtMiddleInit.Text
                oPros.SSN = txtSSN.Text
                oPros.Save()
                Dim oAdd As New clsAddress
                oAdd.Address1 = txtAddress.Text
                oAdd.City = txtCity.Text
                oAdd.StateID = siState.Selected_ID
                oAdd.PostalCode = txtZip.Text
                oAdd.ProspectID = oPros.Prospect_ID
                oAdd.Save()
                oAdd = Nothing
                Dim oPhone As New clsPhone
                oPhone.Number = txtHomePhone.Text
                oPhone.ProspectID = oPros.Prospect_ID
                oPhone.Save()
                oPhone = Nothing
                Dim oEmail As New clsEmail
                oEmail.Email = txtEmail.Text
                oEmail.ProspectID = oPros.Prospect_ID
                oEmail.Save()
                oEmail = Nothing
            Else
                Dim oAdd As New clsAddress
                Dim oPhone As New clsPhone
                Dim oEmail As New clsEmail
                oAdd.ProspectID = oPros.Prospect_ID
                oPhone.ProspectID = oPros.Prospect_ID
                oEmail.ProspectID = oPros.Prospect_ID
                Session("Email_Table") = oEmail.Get_Table
                Session("Phone_Table") = oPhone.Get_Table
                Session("Address_Table") = oAdd.Get_Table
                oAdd = Nothing
                oPhone = Nothing
                oEmail = Nothing
            End If
            oCon.ProspectID = oPros.Prospect_ID
            oCon.Save()
            Session("Prospect") = oPros
            oCon = Nothing
            oPros = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lbRefresh','');window.close();", True)
        Else
            Dim oCoOwner As New clsContractCoOwner
            If rbExisting.Checked Then
                oCoOwner.ProspectID = ProsID
                oCoOwner.ContractID = Request("ContractID")
            Else
                Dim oPros As New clsProspect
                oPros.First_Name = txtFirstName.Text
                oPros.Last_Name = txtLastName.Text
                oPros.MiddleInit = txtMiddleInit.Text
                oPros.SSN = txtSSN.Text
                oPros.Save()
                Dim oAdd As New clsAddress
                oAdd.Address1 = txtAddress.Text
                oAdd.City = txtCity.Text
                oAdd.StateID = siState.Selected_ID
                oAdd.PostalCode = txtZip.Text
                oAdd.ProspectID = oPros.Prospect_ID
                oAdd.Save()
                oAdd = Nothing
                Dim oPhone As New clsPhone
                oPhone.Number = txtHomePhone.Text
                oPhone.ProspectID = oPros.Prospect_ID
                oPhone.Save()
                oPhone = Nothing
                Dim oEmail As New clsEmail
                oEmail.Email = txtEmail.Text
                oEmail.ProspectID = oPros.Prospect_ID
                oEmail.Save()
                oEmail = Nothing
                oCoOwner.ProspectID = oPros.Prospect_ID
                oCoOwner.ContractID = Request("ContractID")
                oPros = Nothing
            End If
            oCoOwner.Save()
            oCoOwner = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        End If
    End Sub
End Class
