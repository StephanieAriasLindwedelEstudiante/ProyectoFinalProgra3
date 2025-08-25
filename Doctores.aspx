<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Doctores.aspx.vb" Inherits="ProyectoFinalProgra3.Doctores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HiddenField para Doctor -->
    <asp:HiddenField ID="DoctorID" runat="server" />
    <asp:HiddenField ID="PacienteID" runat="server" />
    <asp:HiddenField ID="CitaID" runat="server" />
    <div class="row mb-3">
        <div class="col-md-6">

            <!-- Dropdown para estado de la cita -->
            <div class="form-group mb-2">
                <label style="font-family: 'Times New Roman'; font-size: larger; color: black; font: bolder" for="ddlEstado">Estado de la cita</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Pendiente" Value="Pendiente" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Presente" Value="Presente"></asp:ListItem>
                    <asp:ListItem Text="Completada" Value="Completada"></asp:ListItem>
                    <asp:ListItem Text="Cancelada" Value="Cancelada"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Dropdown de pacientes para reprogramar citas -->
            <div class="form-group mb-2">
                <label style="font-family: 'Times New Roman'; font-size: larger; color: black; font: bolder" for="ddlPacientes">Paciente</label>
                <asp:DropDownList ID="ddlPacientes" runat="server" CssClass="form-control">
                    <asp:ListItem Text="--Seleccione un Paciente--" Value="0" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group mb-2">
                <label style="font-family: 'Times New Roman'; font-size: larger; color: black; font: bolder" for="txtFecha">Fecha</label>
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <div class="form-group mb-2">
                <label style="font-family: 'Times New Roman'; font-size: larger; color: black; font: bolder" for="txtHora">Hora</label>
                <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" />
            </div>

            <!-- Botón para actualizar estado -->
            <div class="form-group mb-3">
                <asp:Button ID="btnActualizarEstado" runat="server" Text="Actualizar Estado" CssClass="btn btn-primary" OnClick="btnActualizarEstado_Click" />
                <asp:Button ID="btnReprogramar" runat="server" Text="Reprogramar Cita" CssClass="btn btn-secondary ms-2" OnClick="btnReprogramar_Click" />
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="text-success fw-bold"></asp:Label>
        </div>
    </div>

    <!-- GridView de Citas -->
    <asp:GridView ID="gvCitas" runat="server" AllowPaging="True"
        AutoGenerateColumns="False" DataKeyNames="CitaID,Estado"
        CssClass="table table-bordered table-striped"
        OnSelectedIndexChanged="gvCitas_SelectedIndexChanged">

        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="PacienteNombre" HeaderText="Nombre Paciente" ReadOnly="True" />
            <asp:BoundField DataField="PacienteApellido" HeaderText="Apellido Paciente" ReadOnly="True" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:TemplateField HeaderText="Hora">
                <ItemTemplate>
                    <%# TimeSpan.Parse(Eval("Hora").ToString()).ToString("hh\:mm") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado">
                <ItemTemplate>
                    <%# Eval("Estado").ToString() %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
