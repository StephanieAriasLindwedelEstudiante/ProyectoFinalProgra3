Imports System.Data.SqlClient

Public Class Registrar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LimpiarCampos()
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtEspecialidad.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtEmail.Text = String.Empty
        txtNombreUsuario.Text = String.Empty
    End Sub
    Protected Function RegistrarMedico(usuario As Usuarios, medico As Medicos) As Boolean
        Try
            Dim helper As New DatabaseHelper()
            Dim queryUsuario As String = "INSERT INTO Usuarios (NombreUsuario, PasswordHash, RolID) 
                                      VALUES (@NombreUsuario, @PasswordHash, @RolID);
                                      SELECT SCOPE_IDENTITY();"
            Dim parametrosUsuario As New List(Of SqlParameter) From {
                New SqlParameter("@NombreUsuario", usuario.NombreUsuario),
                New SqlParameter("@PasswordHash", usuario.PasswordHash),
                New SqlParameter("@RolID", 1)
            }
            Dim dt As DataTable = helper.ExecuteQuery(queryUsuario, parametrosUsuario)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return False
            End If
            Dim nuevoUsuarioID As Integer = Convert.ToInt32(dt.Rows(0)(0))
            Dim queryMedico As String = "INSERT INTO Doctores (UsuarioID, Nombre, Apellidos, Especialidad, Telefono, Correo) 
                                      VALUES (@UsuarioID, @Nombre, @Apellidos, @Especialidad, @Telefono, @Correo);"
            Dim parametrosMedico As New List(Of SqlParameter) From {
                helper.CreateParameter("@UsuarioID", nuevoUsuarioID),
                helper.CreateParameter("@Nombre", medico.Nombre),
                helper.CreateParameter("@Apellidos", medico.Apellidos),
                helper.CreateParameter("@Especialidad", medico.Especialidad),
                helper.CreateParameter("@Telefono", medico.Telefono),
                helper.CreateParameter("@Correo", medico.Correo)
            }
            Dim resultado As Boolean = helper.ExecuteNonQuery(queryMedico, parametrosMedico)
            Return resultado
        Catch ex As Exception
            Return False
        End Try
    End Function
    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse
           String.IsNullOrWhiteSpace(txtApellido.Text) OrElse
           String.IsNullOrWhiteSpace(txtEspecialidad.Text) OrElse
           String.IsNullOrWhiteSpace(txtTelefono.Text) OrElse
           String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
           String.IsNullOrWhiteSpace(txtNombreUsuario.Text) OrElse
           String.IsNullOrWhiteSpace(txtPass.Text) Then
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "CamposVacios",
                "Swal.fire('Por favor, complete todos los campos.');",
                True)
            Exit Sub
        End If
        Dim usuarioNuevo As New Usuarios() With {
            .NombreUsuario = txtNombreUsuario.Text.Trim(),
            .PasswordHash = txtPass.Text.Trim(),
            .RolID = 1 ' Rol de médico
        }
        Dim medicoNuevo As New Medicos() With {
            .Nombre = txtNombre.Text.Trim(),
            .Apellidos = txtApellido.Text.Trim(),
            .Especialidad = txtEspecialidad.Text.Trim(),
            .Telefono = txtTelefono.Text.Trim(),
            .Correo = txtEmail.Text.Trim()
        }
        If RegistrarMedico(usuarioNuevo, medicoNuevo) Then
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistroExitoso",
                "Swal.fire('Registro exitoso.');",
                True)
            LimpiarCampos()
        Else
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "ErrorRegistro",
                "Swal.fire('Error al registrar.');",
                True)
            LimpiarCampos()
        End If
    End Sub
End Class