<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Pacientes.aspx.vb" Inherits="ProyectoFinalProgra3.Pacientes1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="PacienteID" runat="server" />
    <asp:HiddenField ID="CitaID" runat="server" />
    <div class="row mb-3">

        <div class="col-md-6">

            <div class="form-group mb-2">
                <label style="font-family: 'Times New Roman'; font-size: larger; color: black; font: bolder" for="ddlDoctores">Doctor</label>
                <asp:DropDownList ID="ddlDoctores" runat="server" CssClass="form-control">
                    <asp:ListItem Text="--Seleccione un Doctor--" Value="0" Selected="True"></asp:ListItem>
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

            <div class="form-group mb-3">
                <asp:Button ID="btnSolicitarCita" CssClass="btn btn-primary" runat="server" Text="Solicitar Cita" OnClick="btnSolicitarCita_Click" />
                <asp:Button ID="btnCancelarCita" CssClass="btn btn-secondary ms-2" runat="server" Text="Cancelar Cita" OnClick="btnCancelarCita_Click" />
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="text-success fw-bold"></asp:Label>
        </div>
    </div>

<asp:GridView ID="gvCitas" runat="server" AllowPaging="True"
    AutoGenerateColumns="False" DataKeyNames="CitaID,Estado"
    CssClass="table table-bordered table-striped"
    OnSelectedIndexChanged="gvCitas_SelectedIndexChanged">

    <Columns>
        <asp:CommandField ShowSelectButton="True" />
        <asp:BoundField DataField="DoctorNombre" HeaderText="Nombre Doctor" ReadOnly="True" />
        <asp:BoundField DataField="DoctorApellido" HeaderText="Apellido Doctor" ReadOnly="True" />
        <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" ReadOnly="True" />
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
