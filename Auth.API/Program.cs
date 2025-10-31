using Auth.API.Common.Conventions;
using Auth.API.Common.Extensions;
using Auth.API.Common.Filters;
using Auth.API.Validators;
using Auth.Infrastructure.Extensions;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Agregas los controladores normalmente
builder.Services.AddControllers();

// Agregas la convención de prefijo global de ruta

builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new GlobalRoutePrefixConvention("api/v1"));
});

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Agregas el filtro global de manejo de excepciones
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpExceptionFilter>();
});

// Activas la validación automática
builder.Services.AddFluentValidationAutoValidation();

// Validadores
builder.Services.AddValidatorsFromAssembly(typeof(SignupRequestValidator).Assembly);

// Registrar tu infraestructura (DbContext, repos, validadores, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
