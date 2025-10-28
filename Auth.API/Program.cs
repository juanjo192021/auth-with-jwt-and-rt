using Auth.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Agregas los controladores normalmente
builder.Services.AddControllers();
// ✅ 2. Activas la validación automática
builder.Services.AddFluentValidationAutoValidation();
// ✅ 3. Registrar tu infraestructura (DbContext, repos, validadores, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// ✅ Swagger
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

app.UseAuthorization();

app.MapControllers();

app.Run();
