Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Partial Class Reports_Packages_VendorReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oVendor As New clsVendor
            ddVendors.DataSource = oVendor.List_Vendors
            ddVendors.DataTextField = "Vendor"
            ddVendors.DataValueField = "VendorID"
            ddVendors.DataBind()
            oVendor = Nothing
        End If
    End Sub

  


    Private Sub Button1_ClickExtracted()
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select x.PackageIssuedID, x.Firstname, x.LastName, pa.Address1, pa.City, s.ComboItem as State, pa.PostalCode, x.Phone, x.Email, x.Package, x.Cost, vs.VSNumber from (SELECT pi.PackageIssuedID, p.FirstName, p.LastName, pkg.Package, pi.Cost, vp.VendorID, (Select Top 1 AddressID from t_ProspectAddress where prospectID = pi.ProspectID and activeFlag = 1) as AddID, (Select Top 1 Email from t_ProspectEmail where prospectID = pi.ProspectID and isActive = 1 and isPrimary = 1) as Email, (Select Top 1 number from t_PRospectPhone where prospectid = pi.ProspectID and active = 1) as Phone, (SELECT TOP 1 DateCreated FROM t_Event WHERE keyfield = 'Packageissuedid' AND keyvalue = pi.Packageissuedid AND type = 'CREATE') AS DateCreated FROM t_PackageIssued pi inner join t_Prospect p on pi.ProspectID = p.ProspectID Inner join t_Package pkg on pi.PackageID = pkg.PackageID Left outer join t_Vendor2Package vp on pi.PackageID = vp.PackageID) x left outer join t_ProspectAddress pa on x.AddID = pa.AddressID left outer join t_ComboItems s on pa.StateID = s.ComboItemID left outer join t_VoiceStamps vs on x.PackageIssuedID = vs.KeyValue where x.DateCreated between '" & dteSDate.Selected_Date & "' and '" & dteEDate.Selected_Date & "' and x.vendorid = '" & ddVendors.SelectedValue & "' and (vs.KeyField = 'PackageIssuedID' or vs.KeyField Is Null) ORDER BY PackageIssuedID DESC"
        'gvReport.DataSource = ds
        'gvReport.DataBind()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click


        Dim Sdate As String = dteSDate.Selected_Date
        Dim Edate As String = dteEDate.Selected_Date
        Dim Vendor As String = ddVendors.SelectedValue


        Dim sql As String = String.Format( _
            "Select x.PackageIssuedID, x.Firstname, x.LastName, pa.Address1, pa.City, s.ComboItem as State, pa.PostalCode, " & _
            "x.Phone, x.Email, x.Package, x.Cost, vs.VSNumber from (SELECT pi.PackageIssuedID, p.FirstName, p.LastName, pkg.Package, " & _
            "pi.Cost, vp.VendorID, " & _
            "(Select Top 1 AddressID from t_ProspectAddress where prospectID = pi.ProspectID and activeFlag = 1) as AddID, " & _
            "(Select Top 1 Email from t_ProspectEmail where prospectID = pi.ProspectID and isActive = 1 and isPrimary = 1) as Email, " & _
            "(Select Top 1 number from t_PRospectPhone where prospectid = pi.ProspectID and active = 1) as Phone, " & _
            "(SELECT TOP 1 DateCreated FROM t_Event WHERE keyfield = 'Packageissuedid' AND keyvalue = pi.Packageissuedid AND type = 'CREATE') AS DateCreated " & _
            "FROM t_PackageIssued pi inner join t_Prospect p on pi.ProspectID = p.ProspectID Inner join " & _
            "t_Package pkg on pi.PackageID = pkg.PackageID Left outer join t_Vendor2Package vp on pi.PackageID = vp.PackageID) x " & _
            "left outer join t_ProspectAddress pa on x.AddID = pa.AddressID left outer join " & _
            "t_ComboItems s on pa.StateID = s.ComboItemID left outer join t_VoiceStamps vs on x.PackageIssuedID = vs.KeyValue " & _
            "where x.DateCreated between '{0}' and '{1}' and " & _
            "x.vendorid = {2} and (vs.KeyField = 'PackageIssuedID' or vs.KeyField Is Null) ORDER BY PackageIssuedID DESC", _
            Sdate, Edate, Vendor)

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(sql, cnn)

                cmd.CommandTimeout = 0
                Dim dt As New DataTable(), html As New StringBuilder()

                Try
                    cnn.Open()

                    Dim rdr As SqlDataReader = cmd.ExecuteReader()

                    dt.Load(rdr)

                    If dt.Rows.Count > 0 Then

                        html.Append("<table style='border-collapse:collapse;' border='1px'>")
                        Dim rowh() As String = {"Package #ID", "First Name", "Last Name", "Address", "City", "State", "Postal Code", "Phone", "Email", "Package", "Cost", "VS #"}

                        For Each s As String In rowh
                            html.AppendFormat("<td>{0}</td>", s)
                        Next

                        For Each row As DataRow In dt.Rows

                            html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td></tr>", _
                                              row("PackageIssuedID"), _
                                            IIf(row("FirstName").Equals(DBNull.Value), String.Empty, row("FirstName")), _
                                            IIf(row("LastName").Equals(DBNull.Value), String.Empty, row("LastName")), _
                                            IIf(row("Address1").Equals(DBNull.Value), String.Empty, row("Address1")), _
                                            IIf(row("City").Equals(DBNull.Value), String.Empty, row("City")), _
                                            IIf(row("State").Equals(DBNull.Value), String.Empty, row("State")), _
                                            IIf(row("PostalCode").Equals(DBNull.Value), String.Empty, row("PostalCode")), _
                                            IIf(row("Phone").Equals(DBNull.Value), String.Empty, row("Phone")), _
                                            IIf(row("Email").Equals(DBNull.Value), String.Empty, row("Email")), _
                                            IIf(row("Package").Equals(DBNull.Value), String.Empty, row("Package")), _
                                            IIf(row("Cost").Equals(DBNull.Value), String.Empty, String.Format("{0:C}", row("Cost"))), _
                                            IIf(row("VSNumber").Equals(DBNull.Value), String.Empty, row("VSNumber")))
                        Next

                        html.AppendFormat("<tr>{0}</tr>", html.ToString())

                    Else
                        html.AppendFormat("<h1>No records</h1>")
                    End If

                Catch ex As Exception
                    Response.Write(String.Format("<strong>{0}</strong>", ex.Message))
                Finally
                    cnn.Close()
                End Try

                lit.Text = html.ToString()
            End Using
        End Using
    End Sub
End Class
