using cacambaonline.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Areas.Identity.Data;
using cacambaonline.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
//using cacambaonline.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MeuDbContext>(options =>
    options.UseSqlServer(connectionString)

    );




builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

//Funcionar injeção de dependência user e role da Create(Destinatário)
//builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<MeuDbContext>();


//Adicionado para o contexto criado chamado MeuDbContext
builder.Services.AddDbContext<MeuDbContext>(options =>
   options.UseSqlServer(connectionString));
//builder.Services.AddDbContext<MeuDbContext>(options =>
//{
//    options.UseSqlServer(connectionString);
//    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//});

//builder.Services.AddScoped<DbContext, MeuDbContext>();
//builder.Services.AddSingleton<DbContext, MeuDbContext>();


//Processos em segundo plano
//builder.Services.AddHostedService<AtualizarCoordenadasHostedService>();


builder.Services.AddMvc();
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//        .AddEntityFrameworkStores<MeuDbContext>();
//builder.Services.AddControllersWithViews();

//FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    // app.UseMigrationsEndPoint();
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();

//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseFastReport();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
