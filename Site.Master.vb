Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub CerrarSesion_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Cierra sesión al hacer clic en "Login" o "Examen Stephanie"
        Session.Clear()
        Session.Abandon()
        Response.Redirect("login.aspx")
    End Sub
End Class