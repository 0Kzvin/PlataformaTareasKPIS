using API;
using API.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstalarServiciosEnEnsamblados(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<API.Hubs.NotificacionesHub>("/hubs/notificaciones");

// Custom Server Config
await ServerConfig.ConfigurarServer(app);

app.Run();
