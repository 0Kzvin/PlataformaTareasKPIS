using API;
using API.Modelos.Configuraciones;
using API.Servicios;
using API.Utilidades.Constantes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ServiciosAPIG3;
using System;


var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddOpenApi();
builder.Services.InstalarServiciosEnEnsamblados(builder.Configuration);

builder.WebHost.UseUrls($"http://0.0.0.0:8082");

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/{ConstantesModulos.ADMINISTRACION}/swagger.json", $"{ConstantesModulos.ADMINISTRACION}");
    c.SwaggerEndpoint($"/swagger/{ConstantesModulos.ALMACENAMIENTO}/swagger.json", $"{ConstantesModulos.ALMACENAMIENTO}");
    c.SwaggerEndpoint($"/swagger/{ConstantesModulos.ACCESORIOS}/swagger.json", $"{ConstantesModulos.ACCESORIOS}");
    c.SwaggerEndpoint($"/swagger/{ConstantesModulos.RECEPCION}/swagger.json", $"{ConstantesModulos.RECEPCION}");
    c.SwaggerEndpoint($"/swagger/{ConstantesModulos.GERENCIA}/swagger.json", $"{ConstantesModulos.GERENCIA}");
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

var opcionesDeLocalizacion = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(opcionesDeLocalizacion.Value);

app.UseCors("ConSignalR");

app.UseAuthentication();

app.MapStaticAssets();

var opcionesAlmacenador = app.Services.GetRequiredService<OpcionesAlmacenadorDTO>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(opcionesAlmacenador.RutaBase),
});

app.UseAuthorization();

app.UseExceptionHandler();

// Mapear los hubs de SignalR
app.MapHub<EstacionHub>("/EstacionHub");

app.MapControllers();

app.UseServiciosAPIG3();

// Configuración del servidor
await ServerConfig.ConfigurarServer(app);

Console.WriteLine("Aplicación iniciada completamente");

await app.RunAsync();