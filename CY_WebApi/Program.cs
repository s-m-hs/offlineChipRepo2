using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using CY_WebApi.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Session;
var builder = WebApplication.CreateBuilder(args);

// Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDistributedMemoryCache(); // For storing session data in memory
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddScoped<ExcelReader>();
builder.Services.AddScoped<RecaptchaService>();
builder.Services.AddScoped<SmsService>();
builder.Services.AddScoped<OrderCodeService>();

builder.Services.AddDbContext<CyContext>(opt =>
{
    //opt.UseSqlServer(builder.Configuration.GetConnectionString("NajmWebsiteDb"));
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CyConStr"));

});




//builder.Services.AddDbContext<CyContext>(opt =>
// opt.UseInMemoryDatabase("CyInMemDb"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CY Api",
        Description = "Secures API using JWT",
    });
    // To Enable authorization using Swagger (JWT)  
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()

    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});


builder.Services.AddCors(p => p.AddPolicy("corsApp", builder =>
{
    builder.SetIsOriginAllowed(_ => true).WithOrigins("http://localhost:3000").WithMethods("GET", "POST", "PUT", "DELETE").AllowAnyHeader().AllowCredentials()
       .WithExposedHeaders("Content-Disposition");
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),

        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidAudience = builder.Configuration["Jwt:Audience"],
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var tokenAdmin = context.Request.Cookies["AdminAccessTokenOffLine"];
            var token = tokenAdmin; // ?Ç åÑ äÇã ˜æ˜? ˜å Êæ ÐÎ?Ñå ˜ÑÏ?
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };


    //options.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        string subdomain = context.Request.Headers.Origin.ToString(); // Get the current subdomain

    //        string tokenName = "AdminAccessTokenOffLine";// Set the token name based on subdomain

    //        context.Request.Cookies.TryGetValue(tokenName, out var accessToken); // Get the token by name

    //        if (!string.IsNullOrEmpty(accessToken))
    //            context.Token = accessToken;

    //        return Task.CompletedTask;
    //    }
    //};
});

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

//app.UseMiddleware<SwaggerBasicAuthMiddleware>();

{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.UseDeveloperExceptionPage();


app.UseSession();

// Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("corsApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var filedownloadPath = Path.Combine(Directory.GetCurrentDirectory(), @"FileContents", "public");
if (!Directory.Exists(filedownloadPath))
{
    Directory.CreateDirectory(filedownloadPath);
}
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(filedownloadPath),
    RequestPath = new PathString("/GFiles")
}
);
//if (app.Environment.IsDevelopment())
//{
//    app.Urls.Add("http://0.0.0.0:8181");
//}
app.Run();
