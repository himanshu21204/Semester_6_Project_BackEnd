using ConsumeCoffeeShopWebAPI;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Helper;
using RealEstateWebAPI.Repositories;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(45);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Retrieve JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// Configure Authentication: JWT & Google
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ClockSkew = TimeSpan.Zero
	};

	options.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = async context =>
		{
			if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
			{
				if (!context.Response.HasStarted)
				{
					context.Response.StatusCode = 401;
					context.Response.ContentType = "application/json";
					await context.Response.WriteAsync("{\"message\": \"Token has expired. Please log in again.\"}");
				}
			}
			else
			{
				if (!context.Response.HasStarted)
				{
					context.Response.StatusCode = 401;
					context.Response.ContentType = "application/json";
					await context.Response.WriteAsync("{\"message\": \"Authentication failed. Invalid token.\"}");
				}
			}
		},
		OnChallenge = async context =>
		{
			if (!context.Response.HasStarted)
			{
				context.HandleResponse();
				context.Response.StatusCode = 401;
				context.Response.ContentType = "application/json";
				await context.Response.WriteAsync("{\"message\": \"Unauthorized access. Login required.\"}");
			}
		},
		OnForbidden = async context =>
		{
			if (!context.Response.HasStarted)
			{
				context.Response.StatusCode = 403;
				context.Response.ContentType = "application/json";
				await context.Response.WriteAsync("{\"message\": \"You do not have permission to access this resource.\"}");
			}
		}
	};
})
.AddGoogle(googleOptions =>
{
	googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
	googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
	googleOptions.CallbackPath = "/signin-google";
})
.AddCookie();

// Configure CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", policy =>
	{
		policy.WithOrigins("https://localhost:44382", "http://localhost:3000")
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials();
	});
});

// Register Services (Dependency Injection)
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LoginRegisterRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<PropertyRepository>();
builder.Services.AddScoped<PropertyImagesRepository>();
builder.Services.AddScoped<AppointmentRepository>();
builder.Services.AddScoped<FavoriteRepository>();
builder.Services.AddScoped<ContactUSRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<InstallmentRepository>();
builder.Services.AddScoped<ReviewsRepository>();

// Configure MVC with FluentValidation
builder.Services.AddControllersWithViews()
	.AddFluentValidation(c => c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Middleware Order
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
