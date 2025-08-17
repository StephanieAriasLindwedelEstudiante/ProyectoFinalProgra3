<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="login.aspx.vb" Inherits="ProyectoFinalProgra3.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-content-center vh-95">
        <main class="card form-signin shadow-lg p-4 m-auto" style="max-width:300px; width:100%; border-radius: 2rem;">
            <h1 class="h3 mb-3 fw-bold" style="font-family:'Times New Roman'; font-size:x-large; color: darkblue">Iniciar sesión</h1>

            <div class="form-floating mb-3">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="SingleLine" placeholder="NombreUsuario"></asp:TextBox>
                <label for="txtNombreUsuario">Correo</label>
            </div>

            <div class="form-floating mb-3">
                <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                <label for="txtPass">Clave</label>
            </div>

            <div class="form-check text-start mb-3">
                <input class="form-check-input" type="checkbox" value="" id="chkRecordar">
                <label class="form-check-label" for="chkRecordar" style="font-family:'Times New Roman'; font-size:larger; color: black">
                    Recordarme
                </label>
            </div>

            <asp:Button CssClass="btn btn-primary w-100 py-2" ID="btnLogin" runat="server" Text="Acceder" OnClick="btnLogin_Click" />
        </main>
    </div>

    <div class="text-center mt-4">
        <a style="font-family:'Times New Roman'; font-size:x-large; color: indigo" href="Registro.aspx">¿Primera vez que ingresa? (Pacientes)</a>
        <a style="font-family:'Times New Roman'; font-size:x-large; color: indigo" href="Registrar.aspx">¿Primera vez que ingresa? (Medico)</a>
    </div>

    <asp:Label ID="lblError" runat="server" Text="" CssClass="alert alert-danger" Visible="false"></asp:Label>
</asp:Content>
