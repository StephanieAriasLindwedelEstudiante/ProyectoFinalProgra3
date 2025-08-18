Imports System.Data.SqlClient

Public Class Registro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LimpiarCampos()
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtFecha_Nacimiento.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtEmail.Text = String.Empty
        txtNombreUsuario.Text = String.Empty
        txtPass.Text = String.Empty
    End Sub
    Protected Function RegistrarPaciente(usuario As Usuarios, paciente As Pacientes) As Boolean
        Try
            Dim helper As New DatabaseHelper()
            Dim queryUsuario As String = "INSERT INTO Usuarios (NombreUsuario, PasswordHash, RolID) 
                                          VALUES (@NombreUsuario, @PasswordHash, @RolID);
                                          SELECT SCOPE_IDENTITY();"
            Dim parametrosUsuario As New List(Of SqlParameter) From {
                New SqlParameter("@NombreUsuario", usuario.NombreUsuario),
                New SqlParameter("@PasswordHash", usuario.PasswordHash),
                New SqlParameter("@RolID", 2)
            }
            Dim dt As DataTable = helper.ExecuteQuery(queryUsuario, parametrosUsuario)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return False
            End If
            Dim nuevoUsuarioID As Integer = Convert.ToInt32(dt.Rows(0)(0))
            Dim queryPaciente As String = "INSERT INTO Pacientes (UsuarioID, Nombre, Apellidos, FechaNacimiento, Telefono, Correo) 
                                           VALUES (@UsuarioID, @Nombre, @Apellidos, @FechaNacimiento, @Telefono, @Correo);"
            Dim parametrosPaciente As New List(Of SqlParameter) From {
                helper.CreateParameter("@UsuarioID", nuevoUsuarioID),
                helper.CreateParameter("@Nombre", paciente.Nombre),
                helper.CreateParameter("@Apellidos", paciente.Apellidos),
                helper.CreateParameter("@FechaNacimiento", paciente.FechaNacimiento),
                helper.CreateParameter("@Telefono", paciente.Telefono),
                helper.CreateParameter("@Correo", paciente.Correo)
            }
            Dim resultado As Boolean = helper.ExecuteNonQuery(queryPaciente, parametrosPaciente)
            Return resultado
        Catch ex As Exception
            Return False
        End Try
    End Function
    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse
           String.IsNullOrWhiteSpace(txtApellido.Text) OrElse
           String.IsNullOrWhiteSpace(txtFecha_Nacimiento.Text) OrElse
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
            Return
        End If
        Dim usuarioNuevo As New Usuarios() With {
                .NombreUsuario = txtNombreUsuario.Text.Trim(),
                .PasswordHash = txtPass.Text.Trim(),
                .RolID = 1 ' Rol de médico
            }
            Dim pacienteNuevo As New Pacientes() With {
                .Nombre = txtNombre.Text.Trim(),
                .Apellidos = txtApellido.Text.Trim(),
                .FechaNacimiento = DateTime.Parse(txtFecha_Nacimiento.Text.Trim()),
                .Telefono = txtTelefono.Text.Trim(),
                .Correo = txtEmail.Text.Trim()
            }

        If RegistrarPaciente(usuarioNuevo, pacienteNuevo) Then
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
        End If
    End Sub
End Class