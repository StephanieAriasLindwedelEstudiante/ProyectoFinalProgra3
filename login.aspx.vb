Imports System.Data.SqlClient

Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.Cookies("UsuarioRecordado") IsNot Nothing Then
                txtNombreUsuario.Text = Request.Cookies("UsuarioRecordado").Value
                chkRecordar.Checked = True
            End If
        End If
        If Not Session("UsuarioID") Is Nothing Then
            Dim rol As String = Session("UsuarioRol")?.ToString()
            If rol = "1" Then
                Response.Redirect("Doctores.aspx")
            ElseIf rol = "2" Then
                Response.Redirect("Pacientes.aspx")
            End If
        End If
    End Sub
    Protected Function verificarUsuario(usuario As Usuarios) As Usuarios
        Try
            Dim helper As New DatabaseHelper()
            Dim wrapper As New Simple3Des("Encriptacion123")
            Dim passEncriptada As String = wrapper.EncryptData(usuario.PasswordHash)
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@NombreUsuario", usuario.NombreUsuario),
                New SqlParameter("@PasswordHash", passEncriptada)
            }
            Dim query As String = "SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND PasswordHash = @PasswordHash"
            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)
            If dataTable.Rows.Count > 0 Then
                Dim usuarioCompleto As Usuarios = usuario.dtToUsuarios(dataTable)
                Session("UsuarioID") = usuarioCompleto.UsuarioID.ToString()
                Session("NombreUsuario") = usuarioCompleto.NombreUsuario.ToString()
                Session("UsuarioRol") = usuarioCompleto.RolID.ToString()
                Return usuarioCompleto
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim usuario As String = txtNombreUsuario.Text.Trim()
        If chkRecordar.Checked Then
            Dim cookie As New HttpCookie("UsuarioRecordado")
            cookie.Value = usuario
            cookie.Expires = DateTime.Now.AddDays(7)
            Response.Cookies.Add(cookie)
        Else
            If Request.Cookies("UsuarioRecordado") IsNot Nothing Then
                Dim cookie As New HttpCookie("UsuarioRecordado")
                cookie.Expires = DateTime.Now.AddDays(-1)
                Response.Cookies.Add(cookie)
            End If
        End If
        lblError.Visible = False
        Dim usuarioIn As New Usuarios() With {
            .NombreUsuario = txtNombreUsuario.Text.Trim(),
            .PasswordHash = txtPass.Text.Trim()
        }
        If String.IsNullOrEmpty(usuarioIn.NombreUsuario) OrElse String.IsNullOrEmpty(usuarioIn.PasswordHash) Then
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "CamposVacios",
                "Swal.fire('Por favor, ingrese email y contraseña.');",
                True)
            Exit Sub
        End If
        Dim usuarioCompleto As Usuarios = verificarUsuario(usuarioIn)
        If usuarioCompleto IsNot Nothing Then
            Select Case usuarioCompleto.RolID
                Case 1
                    ScriptManager.RegisterStartupScript(
                        Me, Me.GetType(),
                        "AccesoExitoso",
                        "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'Doctores.aspx'; });",
                        True)
                Case 2
                    ScriptManager.RegisterStartupScript(
                        Me, Me.GetType(),
                        "AccesoExitoso",
                        "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'Pacientes.aspx'; });",
                        True)
                Case Else
                    lblError.Text = "Rol de usuario no reconocido."
                    lblError.Visible = True
            End Select
        Else
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "AccesoFallido",
                "Swal.fire('Usuario o contraseña incorrectos.');",
                True)
            txtNombreUsuario.Text = String.Empty
            txtPass.Text = String.Empty
            lblError.Visible = True
        End If
    End Sub
End Class