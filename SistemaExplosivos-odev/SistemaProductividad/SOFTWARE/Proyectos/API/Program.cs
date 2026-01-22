using API;
using API.Servicios;
using API.Utilidades.Constantes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstalarServiciosEnEnsamblados(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.ADMINISTRACION}/swagger.json", "Administración");
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.DEPARTAMENTOS}/swagger.json", "Departamentos");
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.TAREAS}/swagger.json", "Tareas");
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.KPIS}/swagger.json", "KPIs & Analítica");
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.REPORTES}/swagger.json", "Reportes");
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.NOTIFICACIONES}/swagger.json", "Notificaciones");
        options.SwaggerEndpoint($"/swagger/{ConstantesModulos.AUDITORIA}/swagger.json", "Auditoría");
    });
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
