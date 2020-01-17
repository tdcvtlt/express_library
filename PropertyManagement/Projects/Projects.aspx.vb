
Partial Class PropertyManagement_Projects_Projects
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If CheckSecurity("Projects", "List", , , Session("UserDBID")) Then
            Dim oProjects As New clsProjects
            gvProjects.DataSource = oProjects.Query(100, ddFilter.SelectedValue, txtFilter.Text, "")
            gvProjects.DataBind()

            lblError.Text = oProjects.Error_Message
            oProjects = Nothing
        Else
            lblError.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)

            'Response.Write((New System.Web.Script.Serialization.JavaScriptSerializer).Deserialize(Of String)("function Show_TermsNow(){$(\"#terms\").removeClass(\"hidden\");};function isValid(){var valid = true;var msg = \"\";if ($(\"#fname\").val() == \"\" || $(\"#fname\").val().length < 2){msg=\"Your First Name is required.\";valid=false;} else {if ($(\"#lname\").val() == \"\" || $(\"#lname\").val().length < 2){msg=\"Your Last Name is required.\";valid=false;}else {if ($(\"#sname\").val() == \"\" || $(\"#sname\").val().length < 2){msg=\"Spouse Name is required.\";valid=false;}else {if ($(\"#zip\").val() == \"\" || $(\"#zip\").val().length < 2){msg=\"Postal Code is required.\";valid=false;}else {if ($(\"#phone\").val() == \"\" || $(\"#phone\").val().length < 2){msg=\"Your Phone Number is required.\";valid=false;}else {if ($(\"#email\").val() == \"\" || $(\"#email\").val().length < 2){msg=\"Your Email is required.\";valid=false;}else {if ($(\"input[name=age]:checked\").val() == undefined){msg=\"Your Age Range is required.\";valid=false;}else {if ($(\"input[name=marital]:checked\").val() == undefined){msg=\"Your Marital Status is required.\";valid=false;}else {if ($(\"input[name=gender]:checked\").val() == undefined){msg=\"Your Gender is required.\";valid=false;}else {if ($(\"input[name=income]:checked\").val() == undefined){msg=\"Your Income Range is required.\";valid=false;}else {if ($(\"input[name=own]:checked\").val() == undefined){msg=\"Own/Rent is required.\";valid=false;}}}}}}}}}}};if (msg != \"\"){alert(msg);};return valid;};function Clear(){    $(\"#entryform *\").filter(\":input\").each(function(){       if (this.type == \"checkbox\" || this.type == \"radio\"){           this.checked = false; $(this).parent(\".btn\").removeClass(\"active\");      } else {           if (this.type==\"text\" || this.type==\"email\" || this.type==\"number\" || this.type==\"password\" || this.type==\"textarea\"){               this.value = \"\";           } else {               if (this.tagName.toLowerCase() == \"select\"){                   this.selectedIndex = -1;                }           }       };    });};function ShowVideoScreen(){   $(\"#videoscreen\").removeClass('hidden');};"))
        End If
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("EditProject.aspx?ID=0")
    End Sub
End Class
