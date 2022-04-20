using System.Reflection;
using backend.Recycle.Abstracts.Repositories;
using backend.Recycle.Cores.Repositories;
using backend.Recycle.Data;
using backend.Recycle.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using backend.Recycle.Extensions.Services;
using backend.Recycle.Services;
using ServiceCollection = backend.Recycle.Extensions.Services.ServiceCollection;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<REGISTERDbContext>(o=>
{
    o.EnableSensitiveDataLogging();
    o.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddIdentity<Users, IdentityRole>(o =>
    {
        
        o.User.RequireUniqueEmail = true;
        o.SignIn.RequireConfirmedEmail = true;
    }).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<REGISTERDbContext>()
    .AddDefaultTokenProviders();
if (args.Length != 0 && args[0].Equals("addRole"))
{

    await ServiceCollection.AddRoleAsync(builder.Services.BuildServiceProvider().GetService<IServiceProvider>());
    return;
}
builder.Services.AddControllers();
builder.Services.AddTransient<REGISTERDbContext>();
builder.Services.AddTransient<IEmailProvider,GmailProvider>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRequestRepository, RequestRepository>();

builder.Services.AddSwagger().
    AddAuthentication(builder.Configuration).AddAuthorization();
///////






var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSwaggerConfiguration();


app.UseRouting();

app.UseCors(options => options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
