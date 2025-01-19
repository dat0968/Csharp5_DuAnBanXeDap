using APIBanXeDap.Models;
using APIBanXeDap.Repository.ChiTietSanPham;
using APIBanXeDap.Repository.DanhMuc;
using APIBanXeDap.Repository.HinhAnhSanPham;
using APIBanXeDap.Repository.KichThuoc;
using APIBanXeDap.Repository.MaCoupon;
using APIBanXeDap.Repository.MauSac;
using APIBanXeDap.Repository.NhaCungCap;
using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.ThuongHieu;
using APIBanXeDap.Repository.Token;
using APIBanXeDap.Repository.VanChuyen;
using APIBanXeDap.Repository.TrangChu;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Csharp5Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BanXeDapContext"));
});
var SecretKey = builder.Configuration["JWT:SecretKey"];
var SecretKeyBytes = Encoding.UTF8.GetBytes("SecretKey");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(SecretKeyBytes),
        ClockSkew = TimeSpan.Zero,
    };
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddGoogle(options =>
{
    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuth["ClientId"];
    options.ClientSecret = googleAuth["ClientSecret"];
});
builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IMauSacRepository, MauSacRepository>();
builder.Services.AddScoped<IKichThuocRepository, KichThuocRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IDanhMucRepository, DanhMucRepository>();
builder.Services.AddScoped<IProductDetailsRepository, ProductDetailsRepository>();
builder.Services.AddScoped<IProductImagesRepository, ProductImagesRepository>();
builder.Services.AddScoped<IMaCouponRepository, MaCouponRepository>();
builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<ITrangChuRepository, TrangChuRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy", options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.AllowAnyOrigin();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
