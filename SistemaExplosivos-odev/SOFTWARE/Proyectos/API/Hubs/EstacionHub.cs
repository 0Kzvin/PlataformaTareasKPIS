using API.Hubs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EstacionHub : Hub
{
    private readonly IConfiguration _configuration;

    private static readonly Dictionary<string, string> _userConnections = new();
    
    public EstacionHub(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (!httpContext.User.Identity.IsAuthenticated)
        {
            bool existeApiKey = httpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey);

            if (!existeApiKey)
            {
                Context.Abort();
                return;
            }

            if (apiKey != _configuration["ApiKey"])
            {
                Context.Abort();
                return;
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = (string)Context.Items["UserId"];

        if (String.IsNullOrWhiteSpace(userId))
        {
            await base.OnDisconnectedAsync(exception);
            return;
        }

        if (userId.Contains("Optix"))
        {
            await EnviarDatosAlmacenamiento(new object());
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task RegistrarUsuario(string userId)
    {
        _userConnections[Context.ConnectionId] = userId;
        Context.Items["UserId"] = userId;
        await Clients.All.SendAsync("UserConnected", userId);
    }

    public async Task EnviarDatosAlmacenamiento(object datos)
    {
        var userId = (string)Context.Items["UserId"];
        var datosSignal = new EnvioDatosSignalRDTO
        {
            UserId = userId,
            Datos = datos
        };
        await Clients.All.SendAsync("RecibirDatosAlmacenamiento", datosSignal);
    }
}