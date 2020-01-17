
Partial Class Accounting_Financialtransactioncodes
    Inherits System.Web.UI.Page

    Protected Sub ddCat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCat.SelectedIndexChanged
        Select Case ddCat.SelectedValue
            Case "TransCode"
                Fill_Filter("TransCode")
            Case "TransType"
                Fill_Filter("TransCodeType")
            Case "Account"
                Fill_Accounts()
            Case Else

        End Select
    End Sub

    Private Sub Fill_Accounts()
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select 0 as ComboItemID, ' All' as ComboItem union Select Accountid as ComboItemID, Description as ComboItem from t_CCMerchantAccount ")
        Bind_Filter(ds)
        ds = Nothing
    End Sub

    Private Sub Fill_Filter(ByVal sKeyField As String)
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select 0 as Comboitemid, ' All' as ComboItem union Select i.ComboitemID, i.Comboitem from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='" & sKeyField & "' and active = 1 order by comboitem")
        Bind_Filter(ds)
        ds = Nothing
    End Sub

    Private Sub Bind_Filter(ByVal ds As SqlDataSource)
        ddFilter.DataSource = ds
        ddFilter.DataTextField = "ComboItem"
        ddFilter.DataValueField = "ComboItemID"
        ddFilter.DataBind()
        Clear_List()
        ds = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then Fill_Filter("TransCode")

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Search()
    End Sub

    Private Sub Search()
        Dim ds As New SqlDataSource(Resources.Resource.cns, Build_SQL())
        Bind_GV(ds)
        ds = Nothing
    End Sub

    Private Sub Bind_GV(ByVal ds As SqlDataSource)
        gvTransCodes.DataSource = ds
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvTransCodes.DataKeyNames = sKeys
        gvTransCodes.DataBind()
        ds = Nothing
    End Sub

    Private Sub Clear_List()
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select f.fintransid as ID, tt.comboitem as TransType, tc.comboitem as TransCode, f.Description from t_FinTransCodes f inner join t_Comboitems tt on tt.comboitemid = f.transtypeid inner join t_Comboitems tc on tc.comboitemid = f.transcodeid where 1=2")
        Bind_GV(ds)
        ds = Nothing
    End Sub

    Private Function Build_SQL() As String
        Dim sRet As String = "Select f.fintransid as ID, tt.comboitem as TransType, tc.comboitem as TransCode, f.Description, m.Description as AccountName " & _
                            "from t_FinTransCodes f " & _
                            "inner join t_Comboitems tt on tt.comboitemid = f.transtypeid " & _
                            "inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " & _
                            "left outer join t_CCMerchantAccount m on m.AccountID = f.MerchantAccountID "
        If ddFilter.SelectedValue > 0 Then
            sRet += "where " & ddCat.SelectedValue & "ID = " & ddFilter.SelectedValue
        End If
        sRet += " order by tc.comboitem "
        Return sRet
    End Function

    Protected Sub ddFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter.SelectedIndexChanged
        Clear_List()
    End Sub

    Protected Sub lbRefresh_Click(sender As Object, e As System.EventArgs) Handles lbRefresh.Click
        Search()
    End Sub
End Class
