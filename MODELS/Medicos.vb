Public Class Medicos
    Public Property DoctorID As Integer
    Public Property UsuarioID As Integer
    Public Property Nombre As String
    Public Property Apellidos As String
    Public Property Especialidad As String
    Public Property Telefono As String
    Public Property Correo As String
    Public Sub New()
    End Sub
    Public Function dtToMedicos(dataTable As DataTable) As Medicos
        If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
            Return Nothing
        End If

        Dim row As DataRow = dataTable.Rows(0)
        Dim medico As New Medicos With {
            .DoctorID = Convert.ToInt32(row("DoctorID")),
            .UsuarioID = Convert.ToInt32(row("UsuarioID")),
            .Nombre = Convert.ToString(row("Nombre")),
            .Apellidos = Convert.ToString(row("Apellidos")),
            .Especialidad = Convert.ToString(row("Especialidad")),
            .Telefono = Convert.ToString(row("Telefono")),
            .Correo = Convert.ToString(row("Correo"))
        }
        Return medico
    End Function
End Class
