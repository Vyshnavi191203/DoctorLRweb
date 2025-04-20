using System.Security.Claims;

using System.Text;

using DoctorLRweb;

using DoctorLRweb.Data;

using DoctorLRweb.Repositories;

using DoctorLRweb.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<Context>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddSwaggerGen(c =>

{

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DoctorLRweb", Version = "v1" });

    // Define the security scheme

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme

    {

        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",

        Name = "Authorization",

        In = ParameterLocation.Header,

        Type = SecuritySchemeType.ApiKey,

        Scheme = "Bearer"

    });

    // Define the security requirement

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

var key = "This is my first secret Test Key for authentication, test it and use it when needed";


builder.Services.AddAuthentication(x =>

{

    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>

{

    x.RequireHttpsMetadata = false;

    x.SaveToken = true;

    x.TokenValidationParameters = new TokenValidationParameters

    {

        ValidateIssuerSigningKey = true,

        ValidateIssuer = false,

        ValidateAudience = false,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))

    };

});

builder.Services.AddScoped<IAuth>(provider =>

{

    var context = provider.GetRequiredService<Context>();

    return new Auth(key, context);

});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();

builder.Services.AddScoped<IDoctorScheduleService, DoctorScheduleServices>();

builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();

builder.Services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();

//life time registration of object

builder.Services.AddCors(options =>

{

    options.AddPolicy("AllowAll",

        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

});


builder.Services.AddControllers();

// Add Swagger services

builder.Services.AddEndpointsApiExplorer(); // all the methods are added

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())

{

    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI();

}

//classes are defined for the specific operations

app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication(); // Enable Authentication Middleware

app.UseAuthorization(); // Enable Authorization Middleware

app.MapControllers();

app.Run();
