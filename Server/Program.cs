using HealthyHands.Server.Data;
using HealthyHands.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>( );

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
