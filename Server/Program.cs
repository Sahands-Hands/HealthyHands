using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WeightsRepository;
using HealthyHands.Server.Data.Repository.WorkoutsRepository;
using HealthyHands.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddScoped<IWorkoutsRepository, WorkoutsRepository>();
builder.Services.AddScoped<IWeightsRepository, WeightsRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Healthy Hands API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("NewPolicy", builder =>
//     builder.AllowAnyOrigin()
//                  .AllowAnyMethod()
//                  .AllowAnyHeader());
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Healthy Hands API v1");
});

//public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContextSeedData seeder)
//{
//    seeder.SeedAdminUser();
//}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
// app.UseCors("NewPolicy");
app.UseAuthorization();
//app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions { })


app.MapRazorPages();
app.MapControllers();
//app.MapControllerRoute(
//    name: "users",
//    pattern: "{controller = User}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
