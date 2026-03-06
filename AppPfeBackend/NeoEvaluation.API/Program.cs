using NeoEvaluation.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NeoEvaluation.API.Data;
using System.Text;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1️ Services

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Email Service
builder.Services.AddSendGrid(options => {
    options.ApiKey = builder.Configuration["SendGridSettings:ApiKey"];
});
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// 2️ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// CORS - Autoriser Vue.js (Port 5173)
app.UseCors(policy => policy
    .WithOrigins("http://localhost:5173")
    .AllowAnyMethod()
    .AllowAnyHeader());

// app.MapControllers();
// app.Run();
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- SEED SUPER ADMIN (Auto-Création) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<NeoEvaluation.API.Data.AppDbContext>();

    // Ken ma fammech admin, asna3 wa7ed
    if (!context.Utilisateurs.Any(u => u.Email == "admin@evaluatech.tn"))
    {
        var admin = new NeoEvaluation.API.Models.Utilisateur
        {
            NomComplet = "Master", // Nom kima fil maquette
            Email = "admin@evaluatech.tn", // Email illi 7achtek bih
            Role = "SuperAdmin",
            EstActif = true,
            NomEntreprise = "NeoLedge",
            
            // Hatha hash s7i7 mta3 'master123'
            // Généré avec BCrypt (Version .Net-Next)
            MotDePasseHash = "$2a$11$Z5y.q.F.w.t.r.y.u.i.o.p.a.s.d.f.g.h.j.k.l.z.x.c.v.b.n.m" // Hatha fake
        };
        
        // Bach l'hash ykoun s7i7 100%, nesta3mlou l'librairie direct:
        admin.MotDePasseHash = BCrypt.Net.BCrypt.HashPassword("master123");
        
        context.Utilisateurs.Add(admin);
        context.SaveChanges();
        Console.WriteLine(" Super Admin créé : admin@evaluatech.tn / master123");
    }
}
// ----------------------------------------


app.Run();
