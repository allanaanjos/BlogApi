using System.Text;
using System.Text.Json.Serialization;
using Blog.Data;
using BlogApi.JwtConfigure;
using BlogApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using static BlogApi.JwtConfigure.Configure;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
configureMvc(builder);
configureServices(builder);
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

LoadConfiguretion(app);

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();
app.Run();


void LoadConfiguretion(WebApplication app)
{
    Configure.JwtKey = app.Configuration.GetValue<string>("Jwtkey");

}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configure.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

}

void configureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
   {

       options.SuppressModelStateInvalidFilter = true;
   }).AddJsonOptions(x => {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
   });

}

void configureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpSettings"));
    builder.Services.AddSingleton<IEmailService, EmailService>();
}