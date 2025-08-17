Public Class Pacientes
    Public Property PacienteID As Integer
    Public Property UsuarioID As Integer
    Public Property Nombre As String
    Public Property Apellidos As String
    Public Property FechaNacimiento As DateTime
    Public Property Telefono As String
    Public Property Correo As String
    Public Sub New()
    End Sub
    Public Function dtToPacientes(dataTable As DataTable) As Pacientes

        If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
            Return Nothing
        End If
        Dim row As DataRow = dataTable.Rows(0)
        Dim paciente As New Pacientes With {
            .PacienteID = Convert.ToInt32(row("PacienteID")),
            .UsuarioID = Convert.ToInt32(row("UsuarioID")),
            .Nombre = Convert.ToString(row("Nombre")),
            .Apellidos = Convert.ToString(row("Apellidos")),
            .FechaNacimiento = Convert.ToDateTime(row("FechaNacimiento")),
            .Telefono = Convert.ToString(row("Telefono")),
            .Correo = Convert.ToString(row("Correo"))
        }
        Return paciente
    End Function
End Class
