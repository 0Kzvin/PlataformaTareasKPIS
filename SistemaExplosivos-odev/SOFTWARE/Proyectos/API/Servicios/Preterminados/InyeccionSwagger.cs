using API.Utilidades.Constantes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace API.Servicios.Preterminados
{
    public class InyeccionSwagger : IInyeccionServicios
    {
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            //Swagger Configuraciones
            servicios.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    ConstantesModulos.ADMINISTRACION,
                    new OpenApiInfo
                    {
                        Title = "Administración",
                        Version = "V2.0.0",
                        Description = ""
                    });

                config.SwaggerDoc(
                    ConstantesModulos.ACCESORIOS,
                    new OpenApiInfo
                    {
                        Title = "Accesorios",
                        Version = "V2.0.0",
                        Description = ""
                    });

                config.SwaggerDoc(
                    ConstantesModulos.ALMACENAMIENTO,
                    new OpenApiInfo
                    {
                        Title = "Almacenamiento",
                        Version = "V2.0.0",
                        Description = ""
                    });

                config.SwaggerDoc(
                    ConstantesModulos.RECEPCION,
                    new OpenApiInfo
                    {
                        Title = "Recepción",
                        Version = "V2.0.0",
                        Description = ""
                    });

                config.SwaggerDoc(
                    ConstantesModulos.GERENCIA,
                    new OpenApiInfo
                    {
                        Title = "Gerencia",
                        Version = "V2.0.0",
                        Description = ""
                    });

                config.CustomSchemaIds(type => type.FullName);

                // DEFINICIÓN DE JWT
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Introduce el token JWT"
                });

                // NUEVO FORMATO EN .NET 10: Requiere un delegado
                config.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>()
                    }
                });

                // DEFINICIÓN DE API KEY
                config.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "ApiKeyAceites",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "API Key para acceso"
                });

                // NUEVO FORMATO EN .NET 10: Requiere un delegado
                config.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("ApiKey", document),
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }
    }
}
