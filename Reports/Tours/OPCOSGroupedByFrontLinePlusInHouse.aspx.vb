Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_Tours_OPCOSGroupedByFrontLinePlusInHouse
    Inherits System.Web.UI.Page



    Private ReadOnly Property getStartDate As DateTime
        Get
            If String.IsNullOrEmpty(dteSDate.Selected_Date) Then
                Return DateTime.MaxValue
            Else
                Return DateTime.Parse(dteSDate.Selected_Date).ToShortDateString
            End If
        End Get
    End Property


    Private ReadOnly Property getEndDate As DateTime
        Get
            If String.IsNullOrEmpty(dteEDate.Selected_Date) Then
                Return DateTime.MaxValue
            Else
                Return DateTime.Parse(dteEDate.Selected_Date).AddDays(1).ToShortDateString()
            End If
        End Get
    End Property

    Private ReadOnly Property getSql As String
        Get
            Return String.Format( _
                "SELECT     t.TourID, t.TourDate, ts.ComboItem AS TourStatus, camp.Name as Campaign, c.ContractNumber, m.SalesVolume, cs.ComboItem AS ContractStatus, rt.ComboItem AS ResType,  " & _
                "rst.ComboItem AS ResSubType, tst.ComboItem AS TourSubType, ct.ComboItem AS ContractType, se.SalesRep, os.SalesRep2 " & _
                "FROM         t_Tour AS t LEFT OUTER JOIN " & _
                "t_Reservations AS r ON t.ReservationID = r.ReservationID LEFT OUTER JOIN " & _
                "t_Contract AS c ON c.TourID = t.TourID LEFT OUTER JOIN " & _
                "t_Campaign camp on t.CampaignID = camp.CampaignID LEFT OUTER JOIN " & _
                "t_Mortgage AS m ON c.ContractID = m.ContractID LEFT OUTER JOIN " & _
                "t_ComboItems AS ts ON t.StatusID = ts.ComboItemID LEFT OUTER JOIN " & _
                "t_ComboItems AS rt ON r.TypeID = rt.ComboItemID LEFT OUTER JOIN " & _
                "t_ComboItems AS rst ON r.SubTypeID = rst.ComboItemID LEFT OUTER JOIN " & _
                "t_ComboItems AS tst ON t.SubTypeID = tst.ComboItemID LEFT OUTER JOIN " & _
                "t_ComboItems AS ct ON c.TypeID = ct.ComboItemID LEFT OUTER JOIN " & _
                "t_ComboItems AS cs ON c.StatusID = cs.ComboItemID LEFT OUTER JOIN  " & _
                "(Select p.FirstName + ' ' + p.LastName as SalesRep, pt.keyvalue " & _
                "						from t_PersonnelTrans pt  " & _
                "inner join t_Personnel p on pt.PersonnelID = p.PersonnelID  " & _
                "where PT.KEYFIELD = 'TOURID' " & _
                "and pt.titleid = ( " & _
                "Select comboitemID  " & _
                "from t_ComboItems A  " & _
                "INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID  " & _
                "where comboname = 'PersonnelTitle' and comboitem = 'Sales Executive' " & _
                ")) se on se.keyvalue = t.tourid LEFT OUTER JOIN  " & _
                "(Select p.FirstName + ' ' + p.LastName as SalesRep2, pt.keyvalue " & _
                "from t_PersonnelTrans pt  " & _
                "inner join t_Personnel p on pt.PersonnelID = p.PersonnelID  " & _
                "where  PT.KEYFIELD = 'TOURID' " & _
                "and PT.titleid = ( " & _
                "Select comboitemID  " & _
                "from t_ComboItems A  " & _
                "INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID  " & _
                "where comboname = 'PersonnelTitle' and comboitem = 'OnSite Solicitor') " & _
                ") OS on OS.Keyvalue = t.tourid " & _
                "WHERE     (t.CampaignID in " & _
                "(SELECT     CampaignID " & _
                "FROM          t_Campaign " & _
                "WHERE      (Name LIKE 'OPC-OS%' or name = 'KSDP'))) AND (t.SubTypeID NOT IN " & _
                "(SELECT     A.ComboItemID " & _
                "FROM          t_ComboItems AS A INNER JOIN " & _
                "t_Combos AS B ON A.ComboID = B.ComboID " & _
                "WHERE      (B.ComboName = 'TourSubtype') AND (A.ComboItem LIKE '%Exit%'))) AND (t.TourDate BETWEEN '{0}' AND  " & _
                "'{1}') AND (ts.ComboItem IN ('Ontour', 'Showed', 'No Tour - Overage')) OR " & _
                "(t.CampaignID in " & _
                "(SELECT     CampaignID " & _
                "FROM          t_Campaign AS t_Campaign_1 " & _
                "WHERE      (Name LIKE 'OPC-OS%' and name = 'KSDP'))) AND (t.SubTypeID NOT IN " & _
                "(SELECT     A.ComboItemID " & _
                "FROM          t_ComboItems AS A INNER JOIN " & _
                "t_Combos AS B ON A.ComboID = B.ComboID " & _
                "WHERE      (B.ComboName = 'TourSubtype') AND (A.ComboItem LIKE '%Exit%'))) AND (t.TourDate BETWEEN '{0}' AND  " & _
                "'{1}') AND (c.ContractNumber IS NOT NULL) " & _
                "ORDER BY tst.ComboItem, t.TourDate", getStartDate, getEndDate)
        End Get
    End Property

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sb As New StringBuilder()

        If DateTime.Compare(getStartDate, DateTime.MaxValue) = 0 Or DateTime.Compare(getEndDate, DateTime.MaxValue) = 0 Then
            sb.AppendFormat("<br/>")
            sb.AppendFormat("{0}Date range is not valid.{1}", "<h2>", "</h2>")
        Else

            Using ada As New SqlDataAdapter(getSql, New SqlConnection(Resources.Resource.cns))
                Dim dt As New DataTable()

                ada.Fill(dt)

                If dt.Rows.Count = 0 Then
                    sb.AppendFormat("{0}No OPC-OS Tours found during {2} and {3} {1}", "<h2>", "</h2>", getStartDate.ToShortDateString(), getEndDate.AddDays(-1).ToShortDateString())
                Else



                End If
            End Using

        End If

        litReport.Text = getSql
    End Sub
End Class
