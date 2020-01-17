<%@ WebHandler Language="VB" Class="HandlerVendorReport" %>

Imports System
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Public Class HandlerVendorReport : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
                
        Dim Sdate As String = context.Request("SDATE")
        Dim Edate As String = context.Request("EDATE")
        Dim Vendor As String = context.Request("Vendor")
                
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
                                                
        context.Response.ContentType = "text/plain"
                
        Dim html As New StringBuilder()
        
        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using ada As New SqlDataAdapter(sql, cnn)
                
                Dim dt As New DataTable()
                ada.Fill(dt)
                
                If dt.Rows.Count > 0 Then
                    html.Append("<table style='border-collapse:collapse;' border='1px'>")
                    Dim rowh() As String = {"Package #ID", "First Name", "Last Name", "Address", "City", "State", "Postal Code", "Phone", "Email", "Package", "Cost", "VS #"}
                    
                    html.Append("<tr>")
                    For Each tmp As String In rowh
                        html.AppendFormat("<td>{0}</td>", tmp)
                    Next
                    
                    html.Append("</tr>")
                End If
                
                
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
                
                
                If (html.Length > 0) Then
                    html.Append("</table>")
                    context.Response.Write(html.ToString())
                End If
            End Using
        End Using
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class