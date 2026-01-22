using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SistemaFrontEnd.Services.Spa;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
builder.WebHost.UseUrls($"http://0.0.0.0:8081");

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(x =>
                {
                    x.AllowedCertificateTypes = CertificateTypes.All;
                });

builder.Environment.IsDevelopment();
builder.Services.AddSpaService(new SpaServiceConfiguration()
{
    DevelopmentSpaFilePath = "AplicacionWeb",
    ProductionSpaFilePath = "AplicacionWeb/dist/spa",
    ScriptName = "quasar dev"
}, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseSpaService();
app.UseAuthentication();

Console.WriteLine("Aplicación iniciada completamente");
await app.RunAsync();