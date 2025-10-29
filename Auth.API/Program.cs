using Auth.API.Common.Filters;
using Auth.Infrastructure.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Agregas los controladores normalmente
builder.Services.AddControllers();

// Agregas el filtro global de manejo de excepciones
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpExceptionFilter>();
});

// Activas la validación automática
builder.Services.AddFluentValidationAutoValidation();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
