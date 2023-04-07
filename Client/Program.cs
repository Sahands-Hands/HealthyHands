using HealthyHands.Client;
using HealthyHands.Client.HttpRepository.AdminHttpRepository;
using HealthyHands.Client.HttpRepository.UserRepository;
using HealthyHands.Client.HttpRepository.WorkoutsHttpRepository;
using HealthyHands.Client.HttpRepository.WorkoutsRepository;
using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HealthyHands.Client.HttpRepository.MealHttpRepository;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("HealthyHands.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HealthyHands.ServerAPI"));
builder.Services.AddScoped<IAdminHttpRepository, AdminHttpRepository>();
builder.Services.AddScoped<IMealsHttpRepository, MealsHttpRepository>();
builder.Services.AddScoped<IWorkoutsHttpRepository, WorkoutsHttpRepository>();
builder.Services.AddScoped<IUserHttpRepository, UserHttpRepository>();
builder.Services.AddScoped<IWeightHttpRepository, WeightHttpRepository>();

builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
