using MVCBanXeDap.Services.Email;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using VNPAY.NET;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<IjwtToken, JwtToken>();
// Đăng ký HttpClient
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7137/api");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<IVnpay, Vnpay>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
