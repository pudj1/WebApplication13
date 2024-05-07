using Quartz.Impl;
using Quartz;
using System.Collections.Specialized;
using WebApplication13.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHostedService<EmailNotificationService>();
builder.Services.AddHostedService<WebsiteChecker>();
builder.Services.AddHostedService<WeatherService>();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ISchedulerFactory>(provider =>
{
    var properties = new NameValueCollection
    {
        ["quartz.serializer.type"] = "binary"
    };
    var schedulerFactory = new StdSchedulerFactory(properties);
    return schedulerFactory;
});
builder.Services.AddSingleton(new JobSchedulerOptions
{
    CronExpression = "0 0 0 * * ?"
});
builder.Services.AddHostedService<QuartzHostedService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
