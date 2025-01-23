using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using APIBanXeDap.Repository.Token;
using System.Security.Principal;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        public readonly ITokenServices TokenServices;

        private readonly Csharp5Context db;

        public AccountsController(Csharp5Context db, ITokenServices TokenServices)
        {
            this.TokenServices = TokenServices;
            this.db = db;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                var AddCustomerDb = new Khachhang
                {
                    HoTen = model.HoTen,
                    TenTaiKhoan = model.TenTaiKhoan,
                    Email = model.Email,
                    MatKhau = model.MatKhau,
                    IsDelete = false,
                    TinhTrang = "Đang hoạt động",
                };
                db.Khachhangs.Add(AddCustomerDb);
                await db.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Register successfully",
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                });
            }
           
        }
        [HttpPost("LoginCustomer")]
        public async Task<IActionResult> LoginCustomer(Login model)
        {
            var findUser = db.Khachhangs.AsNoTracking().FirstOrDefault(p => (p.TenTaiKhoan == model.Email_TenTaiKhoan && 
            p.MatKhau == model.MatKhau) || (p.Email == model.Email_TenTaiKhoan && p.MatKhau == model.MatKhau) );

            if (findUser == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Login failed"
                });
            }
            var AccessToken = TokenServices.GenerateAccessToken(findUser.MaKh.ToString());
            var RefreshToken = TokenServices.GenerateRefreshToken();
            var AddRefreshTokenDb = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = findUser.MaKh.ToString(),
                Token = RefreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(1),
            };
            db.RefreshTokens.Add(AddRefreshTokenDb);
            await db.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Login successfully",
                IDCustomer = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub),
                Data = new TokenResponse
                {
                    AccessToken = AccessToken,
                    RefreshToken = RefreshToken,
                },
            });
        }
        [HttpPost("LoginStaff")]
        public async Task<IActionResult> LoginStaff(Login model)
        {
            var findUser = db.Nhanviens.AsNoTracking().FirstOrDefault(p => (p.TenTaiKhoan == model.Email_TenTaiKhoan &&
            p.MatKhau == model.MatKhau) || (p.Email == model.Email_TenTaiKhoan && p.MatKhau == model.MatKhau));

            if (findUser == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Login failed"
                });
            }
            var AccessToken = TokenServices.GenerateAccessToken(findUser.MaNv.ToString());
            var RefreshToken = TokenServices.GenerateRefreshToken();
            var AddRefreshTokenDb = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = findUser.MaNv.ToString(),
                Token = RefreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(1),
            };
            db.RefreshTokens.Add(AddRefreshTokenDb);
            await db.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Login successfully",
                Data = new TokenResponse
                {
                    AccessToken = AccessToken,
                    RefreshToken = RefreshToken,
                },
            });
        }
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout(string RefreshToken)
        {
            var checkRefreshToken = await db.RefreshTokens.FirstOrDefaultAsync(p => p.Token == RefreshToken);
            if(checkRefreshToken == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Logout failed"
                });
            }
            try
            {
                db.RefreshTokens.Remove(checkRefreshToken);
                await db.SaveChangesAsync();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }           
            return Ok(new
            {
                Success = true,
                Message = "Logout successfully"
            });
        }
        [HttpGet("ForgotPasswordCustomer")]
        public async Task<IActionResult> ForgotPasswordCustomer(string email)
        {
            var checkEmail = await db.Khachhangs.FirstOrDefaultAsync(p => p.Email == email);
            if( checkEmail == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Email chưa được đăng ký với tài khoản nào",
                    Data = "",
                });
            }

            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 10);
            checkEmail.MatKhau = newPassword;
            db.Update(checkEmail);
            await db.SaveChangesAsync();
            return Ok(new
            {
                Success = true,
                Message = "Yêu cầu đổi mật khẩu mới được chấp nhận. Vui lòng kiểm tra Email của bạn",
                Data = newPassword,
            });
        }
        [HttpGet("ForgotPasswordStaff")]
        public async Task<IActionResult> ForgotPasswordStaff(string email)
        {
            var checkEmail = await db.Nhanviens.FirstOrDefaultAsync(p => p.Email == email);
            if (checkEmail == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Email chưa được đăng ký với tài khoản nào",
                    Data = "",
                });
            }

            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 10);
            checkEmail.MatKhau = newPassword;
            db.Update(checkEmail);
            await db.SaveChangesAsync();
            return Ok(new
            {
                Success = true,
                Message = "Yêu cầu đổi mật khẩu mới được chấp nhận. Vui lòng kiểm tra Email của bạn",
                Data = newPassword,
            });
        }
        [HttpPost("RenewAccessToken")]
        public async Task<IActionResult> RenewToken([FromBody] string RefreshToken)
        {
            var checkRefreshToken = await db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(p => p.Token == RefreshToken);
            var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            if ((checkRefreshToken == null) || (checkRefreshToken != null && checkRefreshToken.ExpiredAt < DateTime.UtcNow))
            {
                return Ok(new
                {
                    Success = false,
                    Message = "RefreshToken has expired. Login again",
                });
            }
            var GenerateAccessToken = TokenServices.GenerateAccessToken(checkRefreshToken.UserId);
            return Ok(new
            {
                Success = true,
                Message = "Renew AccessToken successfully",
                Data = new TokenResponse
                {
                    AccessToken = GenerateAccessToken,
                    RefreshToken = RefreshToken,
                }
            });
        }
        [HttpGet("checkCCCD")]
        public async Task<IActionResult> checkCCCD(string cccd)
        {
            var findCCCD = await db.Khachhangs.AsNoTracking().FirstOrDefaultAsync(p => p.Cccd == cccd);
            if(findCCCD != null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "CCCD này đã tồn tại"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "CCCD này hợp lệ"
            });
        }
        [HttpGet("checkUsername")]
        public async Task<IActionResult> checkUsername(string username)
        {
            var findusername = await db.Khachhangs.AsNoTracking().FirstOrDefaultAsync(p => p.TenTaiKhoan == username);
            if (findusername != null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Tên tài khoản này đã tồn tại"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Tên tài khoản này hợp lệ"
            });
        }
        [HttpGet("checkEmail")]
        public async Task<IActionResult> checkEmail(string email)
        {
            var findemail = await db.Khachhangs.AsNoTracking().FirstOrDefaultAsync(p => p.Email == email);
            if (findemail != null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Email này đã tồn tại"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Email này hợp lệ"
            });
        }



        [HttpGet("LoginGoogle")]
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }
        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);
            var existingUser = db.Khachhangs.FirstOrDefault(u => u.Email == email);
            if (existingUser == null)
            {
                existingUser = new Khachhang
                {
                    HoTen = name,
                    Email = email,
                    TinhTrang = "Đang hoạt động",
                    IsDelete = false,
                };
                db.Khachhangs.Add(existingUser);
                db.SaveChanges();
            }

            var AccessToken = TokenServices.GenerateAccessToken(existingUser.MaKh.ToString());
            var RefreshToken = TokenServices.GenerateRefreshToken();
            var AddRefreshTokenDb = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = existingUser.MaKh.ToString(),
                Token = RefreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(1),
            };
            db.RefreshTokens.Add(AddRefreshTokenDb);
            await db.SaveChangesAsync();
            return Redirect($"https://localhost:7029/Accounts/GoogleLoginSuccess?accessToken={AccessToken}&refreshToken={RefreshToken}");
            //return Ok(new
            //{
            //    Success = true,
            //    Message = "Login successfully",
            //    Data = new TokenResponse
            //    {
            //        AccessToken = AccessToken,
            //        RefreshToken = RefreshToken,
            //    },
            //});
        }

    }
}
