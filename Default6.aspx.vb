Imports System.Data.SqlClient
Imports System.Data
Partial Class Default6
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim cn As SqlConnection
        Dim cm As SqlCommand
        Dim dread As SqlDataReader
        cn = New SqlConnection(Resources.Resource.cns)
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm = New SqlCommand("Select * from t_Note where keyfield = 'LeadEntryID' and noteid <> 2549725 and noteid <> 2549435", cn)
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Do While dread.Read()
                Import_Leads(dread("Note"))
            Loop
        Else
            Response.Write("NO ROWS")
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()

    End Sub

    Public Function Import_Leads(ByVal jString As String) As Boolean
        Dim oLead As New leadwrapper
        oLead = (New System.Web.Script.Serialization.JavaScriptSerializer).Deserialize(Of leadwrapper)(jString)
        Dim oleads() As lead = oLead.leads
        Dim oCombo As New clsComboItems
        Dim oLP As New clsLeadProgram
        Dim oDraw As New clsLeadDrawing
        For i = 0 To UBound(oleads)
            Dim oLD As New clsLeads
            If oLD.Check_Duplicate(oleads(i).phone, oleads(i).email, CDate(oleads(i).signdate)) = False Then
                oLD.LeadID = 0
                oLD.Load()
                oLD.FirstName = oleads(i).first
                oLD.LastName = oleads(i).last
                oLD.SpouseName = oleads(i).spouse
                oLD.EmailAddress = oleads(i).email
                oLD.PhoneNumber = oleads(i).phone
                oLD.PostalCode = oleads(i).zip
                oLD.Age = oleads(i).age
                oLD.MaritalStatus = oleads(i).marital
                oLD.IncomeRange = oleads(i).income
                oLD.MaleFemale = oleads(i).gender
                oLD.EntryForm = 1 'oleads(i).entryForm
                oLD.OwnRent = oleads(i).own
                oLD.Signed = True
                oLD.SignDate = CDate(oleads(i).signdate).ToShortDateString
                oLD.DateEntered = System.DateTime.Now
                oLD.DateCollected = System.DateTime.Now.ToShortDateString
                oLD.DrawingID = oDraw.Get_Active_Drawing(oleads(i).signdate)
                oLD.VendorID = oLP.Get_Vendor(oLead.oem)
                oLD.Source = oCombo.Lookup_ComboItem(oLP.Get_Active_Location(oLead.oem, oleads(i).signdate))
                If oLD.Source <> "" Then
                    If oCombo.Lookup_ID("ProspectSource", oLD.Source) = 0 Then
                        Dim oCom As New clsCombos
                        oCombo.Comboitem = oLD.Source
                        oCombo.Active = True
                        oCombo.ComboID = oCom.LookUp_Combo("ProspectSource")
                        oCombo.Save()
                        oCom = Nothing
                    End If
                End If
                oLD.Save()
            Else
                Response.Write("Duplicate " & oleads(i).phone)
            End If
            Response.Write(oLD.Err)
            oLD = Nothing
        Next
        oDraw = Nothing
        oCombo = Nothing
        oLead = Nothing
        oleads = Nothing
        Return True
    End Function

    Private Class Config
        Public location As String
        Public pictureinterval As String
        Public usevideo As String
        Public usepictures As String
        Public url As String
        Public device As device
        Public files As files
        Public exe As String
        Public exetimer As String
        Public validscript As String
        Public screensaver As String
        Public screentimer As String
        Public termstimer As String
        Public registration As String
        Public version As String
        Public html As html
        Public entryformid As String
    End Class

    Private Class device
        Public model As String
        Public platform As String
        Public uuid As String
    End Class

    Private Class files
        Public videos As videos
        Public images As images
    End Class

    Private Class videos
        Public count As String
        Public videourls() As videourl
    End Class

    Private Class videourl
        Public videourl As String
    End Class

    Public Class images
        Public count As String
        Public images() As clsimages
    End Class

    Public Class clsimages
        Public image As String
    End Class

    Public Class html
        Public html As String
        Public sidebar As String
        Public terms As String
    End Class

    Public Class leadwrapper
        Public oem As String
        Public leads() As lead
    End Class

    Public Class lead
        Public first As String
        Public last As String
        Public spouse As String
        Public email As String
        Public phone As String
        Public zip As String
        Public age As String
        Public marital As String
        Public income As String
        Public gender As String
        Public entryForm As String
        Public own As String
        Public signdate As String
    End Class
End Class
