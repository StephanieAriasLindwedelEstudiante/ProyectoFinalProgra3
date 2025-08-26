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
    Private Sub LimpiarFormulario()
        CitaID.Value = String.Empty
        PacienteID.Value = String.Empty
        ddlPacientes.SelectedIndex = 0
        ddlEstado.SelectedIndex = 0
        txtFecha.Text = String.Empty
        txtHora.Text = String.Empty
        ddlPacientes.Enabled = True
        txtFecha.Enabled = True
        txtHora.Enabled = True
    End Sub
    Protected Sub btnActualizarEstado_Click(sender As Object, e As EventArgs)
        Try
            If String.IsNullOrEmpty(CitaID.Value) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorSeleccion", "Swal.fire('Seleccione una cita para actualizar.');", True)
                Return
            End If
            Dim cita As Integer = Convert.ToInt32(CitaID.Value)
            Dim nuevoEstado As String = ddlEstado.SelectedValue
            Dim query As String = "UPDATE Citas SET Estado = @Estado WHERE CitaID = @CitaID AND DoctorID = @DoctorID"
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Estado", nuevoEstado),
                New SqlParameter("@CitaID", cita),
                New SqlParameter("@DoctorID", Convert.ToInt32(Session("DoctorID")))
            }
            If helper.ExecuteNonQuery(query, parametros) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Exito", "Swal.fire('Estado actualizado con éxito.');", True)
                LimpiarFormulario()
                CargarCitas()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorEstado", "Swal.fire('Error al actualizar el estado.');", True)
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al actualizar el estado. " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Protected Sub btnReprogramar_Click(sender As Object, e As EventArgs)
        Try
            Dim pacienteID As Integer = Convert.ToInt32(ddlPacientes.SelectedValue)
            Dim fecha As Date
            Dim hora As TimeSpan
            Dim estado As String = ddlEstado.SelectedValue
            If pacienteID = 0 OrElse Not Date.TryParse(txtFecha.Text.Trim(), fecha) OrElse Not TimeSpan.TryParse(txtHora.Text.Trim(), hora) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorDatos", "Swal.fire('Por favor, ingrese datos válidos.');", True)
                Return
            End If
            Dim query As String = "INSERT INTO Citas (PacienteID, DoctorID, Fecha, Hora, Estado) " &
                                  "VALUES (@PacienteID, @DoctorID, @Fecha, @Hora, @Estado)"
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@PacienteID", pacienteID),
                New SqlParameter("@DoctorID", Convert.ToInt32(Session("DoctorID"))),
                New SqlParameter("@Fecha", fecha),
                New SqlParameter("@Hora", hora),
                New SqlParameter("@Estado", estado)
            }
            If helper.ExecuteNonQuery(query, parametros) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Exito", "Swal.fire('Cita reprogramada con éxito.');", True)
                LimpiarFormulario()
                CargarCitas()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorReprogramar", "Swal.fire('Error al reprogramar la cita.');", True)
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al reprogramar la cita. " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Protected Sub gvCitas_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim row As GridViewRow = gvCitas.SelectedRow
            Dim cita As Integer = Convert.ToInt32(gvCitas.DataKeys(row.RowIndex).Value)
            Dim estado As String = gvCitas.SelectedDataKey("Estado").ToString().Trim()
            CitaID.Value = cita.ToString()
            ddlEstado.SelectedValue = estado
            Dim query As String = "SELECT c.PacienteID, p.Nombre + ' ' + p.Apellidos AS NombreCompleto, " &
                                  "c.Fecha, c.Hora FROM Citas c " &
                                  "INNER JOIN Pacientes p ON c.PacienteID = p.PacienteID " &
                                  "WHERE c.CitaID = @CitaID"
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@CitaID", cita)
            }
            Dim dt As DataTable = helper.ExecuteQuery(query, parametros)
            If dt.Rows.Count > 0 Then
                ddlPacientes.SelectedValue = dt.Rows(0)("PacienteID").ToString()
                txtFecha.Text = Convert.ToDateTime(dt.Rows(0)("Fecha")).ToString("yyyy-MM-dd")
                txtHora.Text = TimeSpan.Parse(dt.Rows(0)("Hora").ToString()).ToString("hh\:mm")
                ddlPacientes.Enabled = False
                txtFecha.Enabled = False
                txtHora.Enabled = False
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al seleccionar la cita. " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub
End Class