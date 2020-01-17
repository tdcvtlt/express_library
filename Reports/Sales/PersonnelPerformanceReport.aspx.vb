Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization


Partial Class Reports_Sales_PersonnelPerformanceReport
    Inherits System.Web.UI.Page    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            ddlLocations.DataSource = (New clsComboItems).Load_ComboItems("tourlocation")
            ddlLocations.DataTextField = "ComboItem"
            ddlLocations.DataValueField = "ComboItemID"
            ddlLocations.DataBind()

        End If
    End Sub

End Class
