
Imports System.Data
Imports System.Data.SqlClient
Imports Resources.Resource

Partial Class setup_EditVendorSalesLocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView_Main.ActiveViewIndex = 0
            Try
                Dim ds = New clsVendorSalesLocations().List_Tradeshow_Vendors()
                Ddl_Sales_Location.DataTextField = "Vendor"
                Ddl_Sales_Location.DataValueField = "VendorID"
                Ddl_Sales_Location.DataSource = CType(ds.Select(DataSourceSelectArguments.Empty), DataView).ToTable()
                Ddl_Sales_Location.DataBind()
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub Lk_Sales_Location_Click(sender As Object, e As EventArgs) Handles Lk_Sales_Location.Click
        MultiView_Main.ActiveViewIndex = 0
    End Sub
    Protected Sub Lk_Sales_Location_Edit_Click(sender As Object, e As EventArgs) Handles Lk_Sales_Location_Edit.Click
        MultiView_Main.ActiveViewIndex = 1
    End Sub
    Protected Sub bt_List_Sales_Locations_Click(sender As Object, e As EventArgs) Handles bt_List_Sales_Locations.Click
        Dim vendor_id = Convert.ToInt32(Ddl_Sales_Location.SelectedValue)
        Dim ds = New clsVendorSalesLocations().List_Sales_Locations_By_Vendor(vendor_id)
        Gv_Sales_Location.DataSource = CType(ds.Select(DataSourceSelectArguments.Empty), DataView).ToTable()
        Gv_Sales_Location.DataBind()
    End Sub
    Protected Sub Gv_Sales_Location_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gv_Sales_Location.SelectedIndexChanged

        MultiView_Main.ActiveViewIndex = 1
        Dim SalesLocationID = Int32.Parse(Gv_Sales_Location.DataKeys(Gv_Sales_Location.SelectedRow.RowIndex).Value.ToString())
        hf_SalesLocationID.Value = SalesLocationID
        hf_VendorID.Value = Convert.ToInt32(Ddl_Sales_Location.SelectedValue)
        With New clsVendorSalesLocations
            .SalesLocationID = SalesLocationID
            .Load()
            tb_Location.Text = .Location
            tb_VRCCost.Text = .VRCCost
            cb_Active.Checked = .Active
            df_DateCreated.Selected_Date = .DateCreated
            df_DateDeActivated.Selected_Date = .DateDeActivated
        End With
    End Sub
    Protected Sub bt_New_Sales_Location_Click(sender As Object, e As EventArgs) Handles bt_New_Sales_Location.Click
        hf_VendorID.Value = Convert.ToInt32(Ddl_Sales_Location.SelectedValue)
        hf_SalesLocationID.Value = ""
        MultiView_Main.ActiveViewIndex = 1
        tb_Location.Text = ""
        tb_VRCCost.Text = ""
        cb_Active.Checked = False
        df_DateCreated.Selected_Date = ""
        df_DateDeActivated.Selected_Date = ""
    End Sub
    Protected Sub bt_Save_Sales_Location_Click(sender As Object, e As EventArgs) Handles bt_Save_Sales_Location.Click
        If Not (CheckSecurity("Vendor Sales Location", "Add", , , Session("UserDBID"))) Then

            ClientScript.RegisterClientScriptBlock(Me.GetType, Guid.NewGuid.ToString(), "alert('You do not have permissions to make changes to this record!');", True)
        Else
            With New clsVendorSalesLocations
                .SalesLocationID = IIf(hf_SalesLocationID.Value.Length > 0, hf_SalesLocationID.Value, 0)
                .Load()
                .Location = tb_Location.Text.Trim()
                .VRCCost = tb_VRCCost.Text
                .Active = cb_Active.Checked
                .DateCreated = df_DateCreated.Selected_Date
                .DateDeActivated = df_DateDeActivated.Selected_Date
                .VendorID = hf_VendorID.Value
                .Save()
            End With
            bt_List_Sales_Locations_Click(Nothing, EventArgs.Empty)
            MultiView_Main.ActiveViewIndex = 0
        End If
    End Sub
    Protected Sub bt_Cancel_Sales_Location_Click(sender As Object, e As EventArgs) Handles bt_Cancel_Sales_Location.Click
        tb_Location.Text = ""
        tb_VRCCost.Text = ""
        cb_Active.Checked = False
        df_DateCreated.Selected_Date = ""
        df_DateDeActivated.Selected_Date = ""
        MultiView_Main.ActiveViewIndex = 0
        hf_SalesLocationID.Value = ""
    End Sub
End Class
