using Microsoft.AspNetCore.SignalR;

namespace signalr_ejercicio.Hubs
{
    public class ChatHub : Hub
    {
        private const string room = "principal";
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            UsersManager.AgregarUsuario(Context.ConnectionId);
            var usuarios = ObtenerUsuarios();
            await Clients.Group(room).SendAsync("updateListaUsuarios", usuarios);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
            UsersManager.RemoverUsuario(Context.ConnectionId);
            var usuarios = ObtenerUsuarios();
            await Clients.Group(room).SendAsync("updateListaUsuarios", usuarios);
            await base.OnDisconnectedAsync(exception);
        }

        public List<string> ObtenerUsuarios()
        {
            return UsersManager.ObtenerUsuarios();
        }
        public async Task EnviarMensaje(string mensaje)
        {
            var emisor = Context.ConnectionId;
            await Clients.Group(room).SendAsync("RecibirMensaje", emisor, mensaje);
        }

        public async Task EnviarMensajePrivado(string receptor, string mensaje)
        {
            var emisor = Context.ConnectionId;
            await Clients.Client(receptor).SendAsync("RecibirMensajePrivado", emisor, mensaje);
        }
    }
}
