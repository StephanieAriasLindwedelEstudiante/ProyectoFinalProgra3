Imports System.Data.SqlClient

Public Class Pacientes1
    Inherits System.Web.UI.Page
    Private helper As New DatabaseHelper()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            btnCancelarCita.Visible = False
            If Session("UsuarioID") Is Nothing OrElse Session("UsuarioRol")?.ToString <> "2" Then
                Response.Redirect("login.aspx")
                Exit Sub
            End If
            Dim usuarioId As Integer = Convert.ToInt32(Session("UsuarioID"))
            Dim query As String = "SELECT PacienteID FROM Pacientes WHERE UsuarioID = @UsuarioID"
            Dim dt As DataTable = helper.ExecuteQuery(query, New List(Of SqlParameter) From {
            New SqlParameter("@UsuarioID", usuarioId)
        })

            If dt.Rows.Count > 0 Then
                Session("PacienteID") = Convert.ToInt32(dt.Rows(0)("PacienteID"))
            Else
                lblMensaje.Text = "No se encontró un paciente asociado a este usuario."
                lblMensaje.ForeColor = Drawing.Color.Red
                Exit Sub
            End If
            CargarDoctores()
            CargarCitas()
        End If
    End Sub
    Private Sub CargarDoctores()
        Try
            Dim query As String = "SELECT DoctorID,Nombre + ' ' + Apellidos AS NombreCompleto, Especialidad, (Nombre + ' ' + Apellidos + ' - ' + Especialidad) as NombreEspecialidad " &
                                  "FROM Doctores ORDER BY Nombre,Apellidos"
            Dim dt As DataTable = helper.ExecuteQuery(query)
            ddlDoctores.DataSource = dt
            ddlDoctores.DataTextField = "NombreEspecialidad"
            ddlDoctores.DataValueField = "DoctorID"
            ddlDoctores.DataBind()
            If ddlDoctores.Items.Count = 0 Then
                ddlDoctores.Items.Add(New ListItem("--No hay doctores disponibles--", "0"))
            End If
            ddlDoctores.Items.Insert(0, New ListItem("--Seleccione un Doctor--", "0"))
        Catch ex As Exception
            lblMensaje.Text = "Error al cargar doctores:" & ex.Message
            lblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Private Sub CargarCitas()
        Try
            Dim pacienteId As Integer = Convert.ToInt32(Session("PacienteID"))
            Dim query As String = "SELECT c.CitaID, d.Nombre AS DoctorNombre, d.Apellidos AS DoctorApellido, " &
                                  "d.Especialidad, c.Fecha, c.Hora, c.Estado " &
                                  "FROM Citas c " &
                                  "INNER JOIN Doctores d ON c.DoctorID = d.DoctorID " &
                                  "WHERE c.PacienteID = @PacienteID " &
                                  "ORDER BY c.Fecha DESC, c.Hora"
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@PacienteID", pacienteId)
            }
            Dim dt As DataTable = helper.ExecuteQuery(query, parameters)
            gvCitas.DataSource = dt
            gvCitas.DataBind()
            btnCancelarCita.Visible = False
        Catch ex As Exception
            lblMensaje.Text = "Error al cargar sus citas:" & ex.Message
            lblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Private Sub LimpiarCampos()
        ddlDoctores.SelectedIndex = 0
        txtFecha.Text = String.Empty
        txtHora.Text = String.Empty
        btnCancelarCita.Visible = False
    End Sub
    Protected Sub btnSolicitarCita_Click(sender As Object, e As EventArgs)
        Try
            Dim pacienteId As Integer = Convert.ToInt32(Session("PacienteID"))
            Dim doctorId As Integer = Convert.ToInt32(ddlDoctores.SelectedValue)
            Dim fecha As Date
            Dim hora As TimeSpan
            If doctorId = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorDoctor", "Swal.fire('Por favor, seleccione un doctor.');", True)
                Exit Sub
            End If
            If String.IsNullOrEmpty(txtFecha.Text) OrElse Not Date.TryParse(txtFecha.Text, fecha) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorFecha", "Swal.fire('Por favor, ingrese una fecha válida.');", True)
                Exit Sub
            End If
            If String.IsNullOrEmpty(txtHora.Text) OrElse Not TimeSpan.TryParse(txtHora.Text, hora) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorHora", "Swal.fire('Por favor, ingrese una hora válida.');", True)
                Exit Sub
            End If
            If fecha < DateTime.Today Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorFechaPasada", "Swal.fire('La fecha no puede ser en el pasado.');", True)
                LimpiarCampos()
                Exit Sub
            End If
            Dim existeQuery As String = "SELECT COUNT(*) FROM Citas " &
                                        "WHERE DoctorID = @DoctorID " &
                                        "AND Fecha = @Fecha " &
                                        "AND Hora = @Hora AND Estado <> 'Cancelada'"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@DoctorID", doctorId),
                New SqlParameter("@Fecha", fecha),
                New SqlParameter("@Hora", hora)
            }
            Dim dt As DataTable = helper.ExecuteQuery(existeQuery, params)
            Dim duplicado As Integer = 0
            If dt.Rows.Count > 0 AndAlso Not dt.Rows(0)(0) Is DBNull.Value Then
                duplicado = Convert.ToInt32(dt.Rows(0)(0))
            End If
            If duplicado > 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CitaDuplicada", "Swal.fire('Ya existe una cita para este doctor en esa fecha y hora.');", True)
                LimpiarCampos()
                Exit Sub
            End If
            Dim insertQuery As String = "INSERT INTO Citas (PacienteID, DoctorID, Fecha, Hora) " &
                                         "VALUES (@PacienteID, @DoctorID, @Fecha, @Hora)"
            Dim insertParams As New List(Of SqlParameter) From {
                New SqlParameter("@PacienteID", pacienteId),
                New SqlParameter("@DoctorID", doctorId),
                New SqlParameter("@Fecha", fecha),
                New SqlParameter("@Hora", hora)
            }
            Dim resultado As Boolean = helper.ExecuteNonQuery(insertQuery, insertParams)
            If resultado Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CitaExitosa", "Swal.fire('Cita solicitada exitosamente.');", True)
                LimpiarCampos()
                CargarCitas()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorInsertarCita", "Swal.fire('Error al solicitar la cita.');", True)
                LimpiarCampos()
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al agendar su cita:" & ex.Message
            lblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Protected Sub btnCancelarCita_Click(sender As Object, e As EventArgs)
        Try
            If String.IsNullOrEmpty(CitaID.Value) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorSeleccionCita", "Swal.fire('Por favor, seleccione una cita para cancelar.');", True)
                Return
            End If
            Dim CitasId As Integer = Convert.ToInt32(CitaID.Value)
            Dim query As String = "SELECT Estado FROM Citas WHERE CitaID = @CitaID AND PacienteID = @PacienteID"
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@CitaID", CitasId),
                New SqlParameter("@PacienteID", Convert.ToInt32(Session("PacienteID")))
            }
            Dim dt As DataTable = helper.ExecuteQuery(query, parameters)
            If dt.Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AccesoDenegado", "Swal.fire('No posee los permisos para cancelación de citas.');", True)
            Return
            End If
            Dim estadoActual As String = dt.Rows(0)("Estado").ToString().Trim()
            If estadoActual <> "Pendiente" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorEstadoCita", "Swal.fire('Solo se pueden cancelar citas en estado Pendiente.');", True)
                Return
            End If
            Dim cancelQuery As String = "UPDATE Citas SET Estado = 'Cancelada' WHERE CitaID = @CitaID AND PacienteID = @PacienteID"
            Dim cancelParams As New List(Of SqlParameter) From {
                New SqlParameter("@CitaID", CitasId),
                New SqlParameter("@PacienteID", Convert.ToInt32(Session("PacienteID")))
            }
            Dim resultado As Boolean = helper.ExecuteNonQuery(cancelQuery, cancelParams)
            If resultado Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CitaCancelada", "Swal.fire('Cita cancelada exitosamente.');", True)
                LimpiarCampos()
                CargarCitas()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorCancelarCita", "Swal.fire('Error al cancelar la cita.');", True)
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al cancelar la cita:" & ex.Message
            lblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Protected Sub gvCitas_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim row As GridViewRow = gvCitas.SelectedRow
            Dim citasId As Integer = Convert.ToInt32(gvCitas.DataKeys(row.RowIndex).Value)
            Dim estado As String = gvCitas.SelectedDataKey("Estado").ToString().Trim()
            CitaID.Value = citasId.ToString()
            If estado = "Pendiente" Then
                btnCancelarCita.Visible = True
            Else
                btnCancelarCita.Visible = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorSeleccion", "Swal.fire('Solo se pueden cancelar citas en estado 'Pendiente'');", True)
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al seleccionar la cita:" & ex.Message
            lblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
End Class