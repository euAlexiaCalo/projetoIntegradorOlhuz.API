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

            // --- 1. CONFIGURA��O DO BANCO DE DADOS ---
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // --- 2. REGISTRO DO SEU SERVI�O DE TOKEN ---
            builder.Services.AddScoped<TokenService>();

            // --- 3. CONFIGURA��O DE AUTENTICA��O JWT ---
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

            // --- 4. PIPELINE DE EXECU��O (A ORDEM IMPORTA!) ---
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            // ESSA ORDEM � CRUCIAL: Autentica��o antes de Autoriza��o
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}