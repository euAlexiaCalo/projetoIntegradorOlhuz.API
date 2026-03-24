using Microsoft.EntityFrameworkCore;
using projetoIntegradorOlhuz.API.Data;
using Scalar.AspNetCore;
using projetoIntegradorOlhuz.API.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace projetoIntegradorOlhuz.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CONFIGURACAO DO BANCO DE DADOS
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // REGISTRO DO SEU SERVICO DE TOKEN
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<CriarContaService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<UsuarioService>();

            // CONFIGURACAO DE AUTENTICACAO JW
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<CriarContaService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<TokenService>();

            var app = builder.Build();

            // PIPELINE DE EXECUCAO (A ORDEM IMPORTA!)
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            // AUTENTICACAO DE AUTORIZACAO
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}