Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class prospects
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Prospects", "List", , , Session("UserDBID")) Then
            lblErr.Text = ""
            Select Case LCase(ddFilter.Text)
                Case "phone"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%' order by ph.number")
                        Else
                            Run_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where ph.number like '" & filter.Text & "%' and vp.VendorID in (" & Session("Vendors") & ") order by ph.number")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where ph.number like '" & filter.Text & "%' and vp.VendorID in (" & Session("Vendors") & ")")
                        End If
                    End If

                Case "address1"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by Address1")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by Address1")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.address1 like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where a.address1 like '" & filter.Text & "%' and vp.VendorID in (" & Session("Vendors") & ")")
                        End If
                    End If

                Case "city"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.City")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by a.City")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.City like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and a.City like '" & filter.Text & "%'")
                        End If
                    End If
                Case "state"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid order by s.Comboitem")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by s.Comboitem")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid where s.comboitem like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and s.comboitem like '" & filter.Text & "%'")
                        End If
                    End If
                Case "email"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid order by e.Email")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by e.Email")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid where e.Email like '" & filter.Text & "%'  order by e.Email")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and e.Email like '" & filter.Text & "%'  order by e.Email")
                        End If
                    End If

                Case "name"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by Lastname, Firstname")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by Lastname, Firstname")
                        End If
                    Else
                        'If InStr(filter.Text, ",") > 0 And InStr(filter.Text, ", ") < 1 Then filter.Text = Replace(filter.Text, ",", ", ")
                        If InStr(filter.Text, ",") > 0 Then
                            Dim sName(2) As String
                            sName = filter.Text.Split(",")
                            If Session("Vendors") = "0" Then
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                            Else
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                            End If
                        Else
                            If Session("Vendors") = "0" Then
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname  like '" & filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                            Else
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.lastname  like '" & filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                            End If
                        End If

                    End If

                Case "id"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.Prospectid")
                        Else
                            Run_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by p.Prospectid")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.Prospectid like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.Prospectid like '" & filter.Text & "%'")
                        End If
                    End If

                Case "postalcode"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.PostalCode")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by a.PostalCode")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.PostalCode like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and a.PostalCode like '" & filter.Text & "%'")
                        End If
                    End If

                Case "ssn"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SSN")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by p.SSN")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SSN like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.SSN like '" & filter.Text & "%'")
                        End If
                    End If

                Case "spousessn"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SpouseSSN")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by p.SpouseSSN")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SpouseSSN like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.SpouseSSN like '" & filter.Text & "%'")
                        End If
                    End If
                Case "club explore membership"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN, ufv.UFValue as [CE Membership Number] from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_UF_Value ufv on p.ProspectID = ufv.KeyValue left outer join t_UFields uf on ufv.UFID = uf.UFID left outer join t_UF_Tables uft on uf.UFTableID = uft.UFTableID where uft.UFTable = 'Prospect' and uf.UFName = 'Club Explore Membership' order by ufv.UFValue")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN, ufv.UFValue as [CE Membership Number] from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID left outer join t_UF_Value ufv on p.ProspectID = ufv.KeyValue left outer join t_UFields uf on ufv.UFID = uf.UFID left outer join t_UF_Tables uft on uf.UFTableID = uft.UFTableID where uft.UFTable = 'Prospect' and uf.UFName = 'Club Explore Membership' and vp.VendorID in (" & Session("Vendors") & ") order by ufv.UFValue")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN, ufv.UFValue as [CE Membership Number] from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_UF_Value ufv on p.ProspectID = ufv.KeyValue left outer join t_UFields uf on ufv.UFID = uf.UFID left outer join t_UF_Tables uft on uf.UFTableID = uft.UFTableID where uft.UFTable = 'Prospect' and uf.UFName = 'Club Explore Membership' and ufv.UFValue like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN, ufv.UFValue as [CE Membership Number] from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID left outer join t_UF_Value ufv on p.ProspectID = ufv.KeyValue left outer join t_UFields uf on ufv.UFID = uf.UFID left outer join t_UF_Tables uft on uf.UFTableID = uft.UFTableID where uft.UFTable = 'Prospect' and uf.UFName = 'Club Explore Membership' and vp.VendorID in (" & Session("Vendors") & ") and ufv.UFValue like '" & filter.Text & "%'")
                        End If
                    End If
                Case "spousename"
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by Lastname, Firstname")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by Lastname, Firstname")
                        End If
                    Else
                        'If InStr(filter.Text, ",") > 0 And InStr(filter.Text, ", ") < 1 Then filter.Text = Replace(filter.Text, ",", ", ")
                        If InStr(filter.Text, ",") > 0 Then
                            Dim sName(2) As String
                            sName = filter.Text.Split(",")
                            If Session("Vendors") = "0" Then
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.spouselastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.spousefirstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                            Else
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.spouselastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.spousefirstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                            End If
                        Else
                            If Session("Vendors") = "0" Then
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.spouselastname  like '" & filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                            Else
                                Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and p.spouselastname  like '" & filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                            End If
                        End If

                    End If
                Case Else
                    If filter.Text = "" Then
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by ph.number")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") order by ph.number")
                        End If
                    Else
                        If Session("Vendors") = "0" Then
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.Number like '" & filter.Text & "%'")
                        Else
                            Run_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & Session("Vendors") & ") and ph.Number like '" & filter.Text & "%'")
                        End If
                    End If

            End Select
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim p As Page = DirectCast(HttpContext.Current.Handler, Page)
        'Dim url As String = p.ResolveClientUrl("editprospect.aspx?prospectid=" & GridView1.SelectedValue)
        'Dim script As String = "window.open(""{0}"", ""{1}"");"
        'script = String.Format(script, url, "_blank")
        'ScriptManager.RegisterStartupScript(p, GetType(Page), "Redirect", script, True)
        'Response.Redirect("editprospect.aspx?prospectid=" & GridView1.SelectedValue)
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
            Label2.Text = ex.ToString
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If CheckSecurity("Prospects", "Add", , , Session("UserDBID")) Then
            Response.Redirect("editprospect.aspx?prospectid=0")
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Button1_Click(sender, e)
        Else
            If Request("Phone") & "" <> "" Then
                ddFilter.Text = "Phone"
                Dim tArr() As String = Trim(Request("Phone")).Split(" ")
                filter.Text = Trim(Replace(Replace(tArr(0), "-", ""), "%20", ""))
                If tArr.Length > 1 Then
                    If filter.Text.Length < 6 And tArr(1) <> "" Then filter.Text = Right(Trim(Replace(Replace(tArr(1), "-", ""), "%20", "")), Trim(Replace(Replace(tArr(1), "-", ""), "%20", "")).Length - 1)
                End If
                filter.Text = IIf(Left(filter.Text, 1) = "1", Right(filter.Text, filter.Text.Length - 1), filter.Text)
                filter.Text = IIf(filter.Text.Length < 10, "757", "") & filter.Text.Replace("+", "")

                Lookup_Number(sender, e)
            End If
            End If
    End Sub

    Sub Lookup_Number(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Replace This", cn)
        Dim dr As SqlDataReader
        Dim bList As Boolean = False
        Dim lProspectID As Long = 0
        Try
            cn.Open()
            cm.CommandText = "Select top 100 (select count(distinct p.prospectid) as Count from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%') as Count,p.prospectid from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & filter.Text & "%'"
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                If dr.Item("Count") > 1 Then
                    bList = True
                Else
                    lProspectID = dr.Item("Prospectid")
                End If
            End If
            dr.Close()
            cn.Close()
        Catch ex As Exception
            Label2.Text = ex.ToString
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
        If Not (bList) And lProspectID > 0 Then
            Response.Redirect("editprospect.aspx?prospectid=" & lProspectID)
        Else
            Button1_Click(sender, e)
        End If
    End Sub
   
    Protected Sub lnkRedirect_Click(ByVal sender As Object, ByVal e As EventArgs)


    End Sub
End Class
