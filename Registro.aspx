<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Registro.aspx.vb" Inherits="ProyectoFinalProgra3.Registro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="d-flex justify-content-center align-items-center vh-95">
        <main class="card form-signin shadow-lg p-4 m-auto" style="max-width:300px; width:100%; border-radius: 2rem;"">
            <div class="card-body">
                <h2 class="h4 mb-3 text-center fw-bold">Crear cuenta</h2>
                <p class="text-center mb-4" style="font-family:'Times New Roman'; font-size:large; color: black">Por favor, completa el formulario para registrarte.</p>
                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre" />
                    <label style="font-family:'Times New Roman'; font-size:large; color: black"  for="MainContent_txtNombre">Nombre</label>
                </div>

                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Apellidos" />
                    <label style="font-family:'Times New Roman'; font-size:large; color: black"  for="MainContent_txtApellido">Apellido</label>
                </div>
                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtFecha_Nacimiento" runat="server" CssClass="form-control" placeholder="Fecha_Nacimiento" TextMode="Date" />
                    <label style="font-family: 'Times New Roman'; font-size: large; color: black" for="MainContent_txtFecha_Nacimiento">Fecha Nacimiento</label>
                </div>
                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Telefono" />
                    <label style="font-family: 'Times New Roman'; font-size: large; color: black" for="MainContent_txtTelefono">Telefono</label>
                </div>
                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" TextMode="Email" />
                    <label style="font-family: 'Times New Roman'; font-size: large; color: black" for="MainContent_txtEmail">Email</label>
                </div>
                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control"  placeholder="Nombre_Usuario" />
                    <label style="font-family:'Times New Roman'; font-size:large; color: black" for="MainContent_txtNombreUsuario">Nombre Usuario</label>
                </div>

                <div class="form-floating mb-3">
                    <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña" />
                    <label style="font-family:'Times New Roman'; font-size:large; color: black" for="MainContent_txtPass">Contraseña</label>
                </div>


                <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-success w-100 py-2" Text="Registrarse" OnClick="btnRegistrar_Click" />

                <div class="text-center mt-3">
                    <a href="login.aspx" style="font-family:'Times New Roman'; font-size:x-large; color: indigo"  class="text-decoration-none">¿Ya estás registrado?</a>
                </div>

                <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger mt-3 d-block text-center" Visible="false" />
                <asp:Label ID="lblDebug" runat="server" CssClass="text-info mt-2 d-block text-center" Visible="false" />
            </div>
        </main>
    </div>
</asp:Content>
