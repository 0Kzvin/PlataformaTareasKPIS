using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace API.Hubs
{
    [Authorize]
    public class NotificacionesHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // Map user to groups if needed (e.g. by DepartmentId)
            // var user = Context.User;
            // await Groups.AddToGroupAsync(Context.ConnectionId, "Departamento_" + deptId);
        }
        
        public async Task EnviarNotificacion(string mensaje)
        {
            await Clients.All.SendAsync("RecibirNotificacion", mensaje);
        }
    }
}
