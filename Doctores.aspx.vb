Imports System.Data.SqlClient

Public Class Doctores
    Inherits System.Web.UI.Page
    Private helper As New DatabaseHelper()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("UsuarioID") Is Nothing OrElse Session("UsuarioRol")?.ToString() <> "1" Then
                Response.Redirect("login.aspx")
                Exit Sub
            End If
            Dim usuarioID As Integer = Convert.ToInt32(Session("UsuarioID"))
            Dim query As String = "SELECT DoctorID FROM Doctores WHERE UsuarioID = @UsuarioID"
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@UsuarioID", usuarioID)
            }
            Dim dt As DataTable = helper.ExecuteQuery(query, parametros)
            If dt.Rows.Count > 0 Then
                Session("DoctorID") = Convert.ToInt32(dt.Rows(0)("DoctorID"))
            Else
                lblMensaje.Text = "No se encontró un doctor asociado a este usuario."
                lblMensaje.ForeColor = Drawing.Color.Red
                Exit Sub
            End If
            CargarPacientes()
            CargarCitas()
        End If
    End Sub
    Private Sub CargarPacientes()
        Try
            Dim query As String = "SELECT PacienteID, Nombre + ' '+ Apellidos AS NombreCompleto FROM Pacientes ORDER BY Nombre, Apellidos"
            Dim dt As DataTable = helper.ExecuteQuery(query)
            ddlPacientes.DataSource = dt
            ddlPacientes.DataTextField = "NombreCompleto"
            ddlPacientes.DataValueField = "PacienteID"
            ddlPacientes.DataBind()
            ddlPacientes.Items.Insert(0, New ListItem("-- Seleccione un paciente --", "0"))
        Catch ex As Exception
            lblMensaje.Text = "Error al cargar pacientes. " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Private Sub CargarCitas()
        Try
            Dim doctorID As Integer = Convert.ToInt32(Session("DoctorID"))
            Dim query As String = "SELECT c.CitaID, p.Nombre AS PacienteNombre, p.Apellidos AS PacienteApellido, " &
                                  "c.Fecha, c.Hora, c.Estado " &
                                  "FROM Citas c " &
                                  "INNER JOIN Pacientes p ON c.PacienteID = p.PacienteID " &
                                  "WHERE c.DoctorID = @DoctorID " &
                                  "ORDER BY c.Fecha DESC, c.Hora"
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@DoctorID", doctorID)
            }
            Dim dt As DataTable = helper.ExecuteQuery(query, parametros)
            gvCitas.DataSource = dt
            gvCitas.DataBind()
        Catch ex As Exception
            lblMensaje.Text = "Error al cargar citas. " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Protected Sub btnActualizarEstado_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnReprogramar_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub gvCitas_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
End Class