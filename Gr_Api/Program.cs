using Gr_Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();

builder.Services.AddDbContext<GrContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("GrConStr"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(p => p.AddPolicy("corsApp", builder =>
{
    //builder.WithOrigins("http://localhost:8080").WithMethods("GET", "POST").AllowAnyHeader().AllowCredentials();
    //builder.WithOrigins("http://192.168.1.35:8080").WithMethods("GET", "POST").AllowAnyHeader().AllowCredentials();
    //builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    builder.SetIsOriginAllowed(_ => true).WithMethods("GET", "POST", "PUT", "DELETE").AllowAnyHeader().AllowCredentials();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();

app.UseCors("corsApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
