



using JWTPractice.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/// Configuration Of Data Bases
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No Connection String Was found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(ConnectionString));

//setting up the configuration to bind the values from  appsettings.json into JWT Class
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


// Configuration Of Autho Service
builder.Services.AddScoped<IAuthoServices, AuthoServices>();


// Set The Authentication of JwtBearer
builder.Services.AddAuthentication(options =>
{
    /// Set The Defult Secheme to JwtBearerDefaults
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    /*
     The authentication challenge is the process
     where the server responds to an unauthenticated request,
     instructing the client on how to authenticate 
     itself (usually by sending a 401 Unauthorized status code)
     */
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}
).AddJwtBearer(options=>
{ 
    // binding For Quiqe Access to Jwt Values 
    var Config = builder.Configuration.GetSection("JWT").Get<JWT>();
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Config.Issure,
        ValidAudience = Config.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Key))

    };

});

///---- How to add Authentication in SwaggerGen--- 
 
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' followed by a space and your JWT token"
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

///---------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
///authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
