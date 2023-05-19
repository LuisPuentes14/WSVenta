using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WSVenta.Models.Common;
using WSVenta.Services;

var builder = WebApplication.CreateBuilder(args);

string MiCors = "MiCors";
// Add services to the container.

builder.Services.AddControllers();


var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);


//JWT

var appSettings = appSettingsSection.Get<AppSettings>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);

builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(d => { 

    d.RequireHttpsMetadata= false;
    d.SaveToken = true;
    d.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(llave),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});

//Inyeccion de dependencias
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IVentaService,VentaService>();

builder.Services.AddCors(options =>
{

    options.AddPolicy(
        name: MiCors,
        builder =>
        {
            builder.WithHeaders("*");
            builder.WithOrigins("*");
            builder.WithMethods("*");
        } );

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MiCors);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
