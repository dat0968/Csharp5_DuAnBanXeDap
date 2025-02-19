using APIBanXeDap.Models;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVCBanXeDap.EditModels;
using MVCBanXeDap.Models;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Cms;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using X.PagedList.Extensions;
using SizeVM = MVCBanXeDap.ViewModels.SizeVM;

namespace MVCBanXeDap.Controllers
{
    public class ProductController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;
        public ProductController(IjwtToken jwtToken)
        {
            this.jwtToken = jwtToken;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        [NonAction]
        [HttpGet]
        public async void SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? keywords, int? MaDanhMuc, int? MaThuongHieu, string? sort, int page = 1)
        {
            SetAuthorizationHeader();
            var ListProducts = new List<ProductVM>();
            HttpResponseMessage responseProduct = _client.GetAsync(_client.BaseAddress + $"Products/GetAllProduct?keywords={keywords}&MaDanhMuc={MaDanhMuc}&MaThuongHieu={MaThuongHieu}&sort={sort}&page={page}").Result;
            if (responseProduct.IsSuccessStatusCode)
            {
                string data = responseProduct.Content.ReadAsStringAsync().Result;
                var ConvertResponseProduct = JsonConvert.DeserializeObject<JObject>(data);
                ListProducts = ConvertResponseProduct["data"].ToObject<List<ProductVM>>();
                ViewBag.TotalPages = ConvertResponseProduct["totalPages"].Value<int>();
                ViewBag.Page = ConvertResponseProduct["page"].Value<int>();
                ViewBag.Keywords = keywords;
                ViewBag.MaDanhMuc = MaDanhMuc;
                ViewBag.MaThuongHieu = MaThuongHieu;
                ViewBag.Sort = sort;
            }
            else
            {
                return StatusCode((int)responseProduct.StatusCode);
            };
            var ListBrand = new List<BrandVM>();
            HttpResponseMessage responseBrand = await _client.GetAsync(_client.BaseAddress + "Brands/GetAllBrand");
            if (responseBrand.IsSuccessStatusCode)
            {
                string data = await responseBrand.Content.ReadAsStringAsync();
                ListBrand = JsonConvert.DeserializeObject<List<BrandVM>>(data);
                ViewBag.Brand = ListBrand;
            }
            else
            {
                return StatusCode((int)responseProduct.StatusCode);
            };

            var ListSupplier = new List<SupplierVM>();
            HttpResponseMessage responseSupplier = _client.GetAsync(_client.BaseAddress + "Suppliers/GetAllSupplier").Result;
            if (responseSupplier.IsSuccessStatusCode)
            {
                string data = responseSupplier.Content.ReadAsStringAsync().Result;
                ListSupplier = JsonConvert.DeserializeObject<List<SupplierVM>>(data);
                ViewBag.Supplier = ListSupplier;
            }
            else
            {
                return StatusCode((int)responseProduct.StatusCode);
            };

            var ListColor = new List<ColorVM>();
            HttpResponseMessage responseColor = _client.GetAsync(_client.BaseAddress + "Colors/GetAllColor").Result;
            if (responseColor.IsSuccessStatusCode)
            {
                string data = responseColor.Content.ReadAsStringAsync().Result;
                ListColor = JsonConvert.DeserializeObject<List<ColorVM>>(data);
                ViewBag.Color = ListColor;
            }
            else
            {
                return StatusCode((int)responseProduct.StatusCode);
            };
            var ListSize = new List<SizeVM>();
            HttpResponseMessage responseSize = _client.GetAsync(_client.BaseAddress + "Sizes/GetAllSize").Result;
            if (responseSize.IsSuccessStatusCode)
            {
                string data = responseSize.Content.ReadAsStringAsync().Result;
                ListSize = JsonConvert.DeserializeObject<List<SizeVM>>(data);
                ViewBag.Size = ListSize;
            }
            else
            {
                return StatusCode((int)responseProduct.StatusCode);
            };
            var ListCategory = new List<DanhmucVM>();
            HttpResponseMessage responseCategory = _client.GetAsync(_client.BaseAddress + "Categories/GetAllCategory").Result;
            if (responseCategory.IsSuccessStatusCode)
            {
                string data = responseCategory.Content.ReadAsStringAsync().Result;
                ListCategory = JsonConvert.DeserializeObject<List<DanhmucVM>>(data);
                ViewBag.Category = ListCategory;
            }
            else
            {
                return StatusCode((int)responseProduct.StatusCode);
            };
            return View(ListProducts);
        }
        [HttpGet("id")]
        public async Task<IActionResult> Details(int id)
        {
            SetAuthorizationHeader();
            var ProductVM = new ProductVM();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Products/GetProductById/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                ProductVM = JsonConvert.DeserializeObject<ProductVM>(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            };
            return PartialView("_ProductDetails", ProductVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(string TenSp, int ThuongHieu, int DanhMuc, int NhaCC, string MoTa,
        DateOnly NgaySanXuat, IFormFile? file, List<IFormFile>? files, List<DetailsProductEM> Attributes)
        {
            SetAuthorizationHeader();
            if (!string.IsNullOrEmpty(TenSp) && NhaCC > 0 && ThuongHieu > 0 && DanhMuc > 0 &&
               (file != null && file.Length > 0) && (files != null && files.Count > 0) && Attributes.Count > 0)
            {
                //Xử lí hình ảnh chính
                var folderPath = Path.Combine("wwwroot/Hinh/SanPham");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                // Xử lí thêm sản phẩm
                var NewProduct = new ProductEM
                {
                    TenSp = TenSp,
                    MaThuongHieu = ThuongHieu,
                    MaDanhMuc = DanhMuc,
                    MoTa = MoTa,
                    NgaySanXuat = NgaySanXuat,
                    MaNhaCC = NhaCC,
                    Hinh = file.FileName,
                };
                var ConvertDataToJson = JsonConvert.SerializeObject(NewProduct);
                StringContent contentProduct = new StringContent(ConvertDataToJson, Encoding.UTF8, "application/json");
                HttpResponseMessage responseProduct = await _client.PostAsync(_client.BaseAddress + "Products/CreateProduct", contentProduct);
                if (responseProduct.IsSuccessStatusCode)
                {
                    var dataProduct = await responseProduct.Content.ReadAsStringAsync();
                    var ConvertResponseProduct = JsonConvert.DeserializeObject<JObject>(dataProduct);
                    var isSuccessProduct = ConvertResponseProduct["success"].Value<bool>();
                    if (isSuccessProduct == false)
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra";
                        return RedirectToAction("Index", "Product");
                    }
                }
                else
                {
                    return StatusCode((int)responseProduct.StatusCode);
                };

                //Xử lí thêm hình ảnh phụ
                foreach (var hinhanh in files)
                {
                    if (hinhanh != null && hinhanh.Length > 0)
                    {
                        filePath = Path.Combine(folderPath, hinhanh.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            hinhanh.CopyTo(stream);
                        }
                        var NewImg = new ProductImgEM
                        {
                            HinhAnh1 = hinhanh.FileName
                        };
                        var ConvertImageToJson = JsonConvert.SerializeObject(NewImg);
                        StringContent contentImg = new StringContent(ConvertImageToJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseImg = await _client.PostAsync(_client.BaseAddress + "ProductImages/CreateProductImage", contentImg);
                        if (responseImg.IsSuccessStatusCode)
                        {
                            var dataImg = await responseImg.Content.ReadAsStringAsync();
                            var ConvertResponseImg = JsonConvert.DeserializeObject<JObject>(dataImg);
                            var isSuccessImg = ConvertResponseImg["success"].Value<bool>();
                            if (isSuccessImg == false)
                            {
                                TempData["ErrorMessage"] = "Có lỗi xảy ra";
                                return RedirectToAction("Index", "Product");
                            }
                        }
                        else
                        {
                            return StatusCode((int)responseImg.StatusCode);
                        };
                    }
                }

                //Xử lí thêm chi tiết sản phẩm
                foreach (var attr in Attributes)
                {
                    var details = new DetailsProductEM
                    {
                        MaMau = attr.MaMau,
                        MaKichThuoc = attr.MaKichThuoc,
                        SoLuongTon = attr.SoLuongTon,
                        DonGia = attr.DonGia,
                    };
                    var ConvertdetailsToJson = JsonConvert.SerializeObject(details);
                    StringContent contentDetailsProduct = new StringContent(ConvertdetailsToJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseDetailsProduct = await _client.PostAsync(_client.BaseAddress + "ProductDetails/CreateAllProductDetails", contentDetailsProduct);
                    if (responseDetailsProduct.IsSuccessStatusCode)
                    {
                        var dataDetailsProduct = await responseDetailsProduct.Content.ReadAsStringAsync();
                        var ConvertResonseDetailsProduct = JsonConvert.DeserializeObject<JObject>(dataDetailsProduct);
                        var isSuccessDetailsProduct = ConvertResonseDetailsProduct["success"].Value<bool>();
                        if (isSuccessDetailsProduct == false)
                        {
                            TempData["ErrorMessage"] = "Có lỗi xảy ra";
                            return RedirectToAction("Index", "Product");
                        }
                    }
                    else
                    {
                        return StatusCode((int)responseDetailsProduct.StatusCode);
                    };
                }

                TempData["SuccessMessage"] = "Thêm sản phẩm mới thành công";
                return RedirectToAction("Index", "Product");
            }
            TempData["ErrorMessage"] = "Có lỗi xảy ra. Có thể bạn chưa thêm thông tin về thuộc tính sản phẩm";
            return RedirectToAction("Index", "Product");
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(int MaSp, string TenSp, int ThuongHieu, int DanhMuc, int NhaCC, IFormFile? file,
        List<IFormFile>? files, string MoTa, DateOnly NgaySanXuat, List<DetailsProductEM> Attributes, List<string> DeleteImages, 
        List<DetailsProductEM> Chitietsanphams)
        {
            SetAuthorizationHeader();
            if (!string.IsNullOrEmpty(TenSp) && NhaCC > 0 && ThuongHieu > 0 && DanhMuc > 0)
            {
                //Xử lí ảnh chính
                if (file != null)
                {
                    var folderPath = Path.Combine("wwwroot/Hinh/SanPham");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    var filePath = Path.Combine(folderPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                //Cập nhật thông tin sản phẩm
                var modelEdit = new ProductEM
                {
                    MaSP = MaSp,
                    TenSp = TenSp,
                    MaThuongHieu = ThuongHieu,
                    MoTa = MoTa,
                    NgaySanXuat = NgaySanXuat,
                    MaNhaCC = NhaCC,
                    MaDanhMuc = DanhMuc,
                    Hinh = file != null ? file.FileName : null
                };
                var ConvertEditModeltoJson = JsonConvert.SerializeObject(modelEdit);
                StringContent content = new StringContent(ConvertEditModeltoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "Products/EditProduct", content);
                if (response.IsSuccessStatusCode)
                {
                    var dataProduct = await response.Content.ReadAsStringAsync();
                    var ConvertResponseProduct = JsonConvert.DeserializeObject<JObject>(dataProduct);
                    var isSuccess = ConvertResponseProduct["success"].Value<bool>();
                    if (isSuccess == false)
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện thao tác.";
                        return RedirectToAction("Index", "Product");
                    }
                    // Xử lí ảnh phụ
                    if (files.Count > 0)
                    {
                        foreach (var fileimg in files)
                        {
                            if (fileimg.Length > 0 && fileimg != null)
                            {
                                using (var stream = new FileStream($"wwwroot/Hinh/SanPham/{fileimg.FileName}", FileMode.Create))
                                {
                                    fileimg.CopyTo(stream);
                                }
                                var newImg = new ProductImgEM
                                {
                                    HinhAnh1 = fileimg.FileName,
                                    MaSp = MaSp
                                };
                                var ConvertImageToJson = JsonConvert.SerializeObject(newImg);
                                StringContent contentImg = new StringContent(ConvertImageToJson, Encoding.UTF8, "application/json");
                                HttpResponseMessage responseImg = await _client.PostAsync(_client.BaseAddress + "ProductImages/CreateProductImage", contentImg);
                                if (responseImg.IsSuccessStatusCode)
                                {
                                    var dataImg = await responseImg.Content.ReadAsStringAsync();
                                    var ConvertResponseImg = JsonConvert.DeserializeObject<JObject>(dataImg);
                                    var isSuccessImg = ConvertResponseImg["success"].Value<bool>();
                                    if (isSuccessImg == false)
                                    {
                                        TempData["ErrorMessage"] = "Có lỗi xảy ra";
                                        return RedirectToAction("Index", "Product");
                                    }
                                }
                                else
                                {
                                    return StatusCode((int)responseImg.StatusCode);
                                };
                            }
                        }
                    }
                    //Xử lí xóa ảnh phụ
                    if (DeleteImages.Count > 0)
                    {
                        foreach (var image in DeleteImages)
                        {
                            HttpResponseMessage responseImg = await _client.DeleteAsync(_client.BaseAddress + $"ProductImages/DeleteProductImage/{image}");
                            if (responseImg.IsSuccessStatusCode)
                            {
                                var dataImg = await responseImg.Content.ReadAsStringAsync();
                                var ConvertResponseImg = JsonConvert.DeserializeObject<JObject>(dataImg);
                                var isSuccessImg = ConvertResponseImg["success"].Value<bool>();
                                if (isSuccessImg == false)
                                {
                                    TempData["ErrorMessage"] = "Có lỗi xảy ra";
                                    return RedirectToAction("Index", "Product");
                                }
                            }
                            else
                            {
                                return StatusCode((int)responseImg.StatusCode);
                            };
                        }
                    }
                    //Xử lí thêm thuộc tích sản phẩm
                    foreach (var attr in Attributes)
                    {
                        var details = new DetailsProductEM
                        {
                            MaSP = MaSp,
                            MaMau = attr.MaMau,
                            MaKichThuoc = attr.MaKichThuoc,
                            SoLuongTon = attr.SoLuongTon,
                            DonGia = attr.DonGia,
                        };
                        var ConvertdetailsToJson = JsonConvert.SerializeObject(details);
                        StringContent contentDetailsProduct = new StringContent(ConvertdetailsToJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseDetailsProduct = await _client.PostAsync(_client.BaseAddress + "ProductDetails/CreateAllProductDetails", contentDetailsProduct);
                        if (responseDetailsProduct.IsSuccessStatusCode)
                        {
                            var dataDetailsProduct = await responseDetailsProduct.Content.ReadAsStringAsync();
                            var ConvertResonseDetailsProduct = JsonConvert.DeserializeObject<JObject>(dataDetailsProduct);
                            var isSuccessDetailsProduct = ConvertResonseDetailsProduct["success"].Value<bool>();
                            if (isSuccessDetailsProduct == false)
                            {
                                TempData["ErrorMessage"] = "Có lỗi xảy ra";
                                return RedirectToAction("Index", "Product");
                            }
                        }
                        else
                        {
                            return StatusCode((int)responseDetailsProduct.StatusCode);
                        };
                    }
                    //Xử lí sửa thông tin thuộc tích sản phẩm
                    foreach(var thuoctinh in Chitietsanphams)
                    {
                        var modelDetailsProduct = new DetailsProductEM
                        {
                            MaSP = MaSp,
                            MaKichThuoc = thuoctinh.MaKichThuoc,
                            MaMau = thuoctinh.MaMau,
                            SoLuongTon = thuoctinh.SoLuongTon,
                            DonGia = thuoctinh.DonGia,
                        };
                        var ConvertDetailsProductToJson = JsonConvert.SerializeObject(modelDetailsProduct);
                        StringContent contentDetailsProduct = new StringContent(ConvertDetailsProductToJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseDetailsProduct = await _client.PutAsync(_client.BaseAddress + "ProductDetails/EditDetailsProduct", contentDetailsProduct);
                        if (responseDetailsProduct.IsSuccessStatusCode)
                        {
                            var dataDetailsProduct = await responseDetailsProduct.Content.ReadAsStringAsync();
                            var ConvertResponseDetailsProduct = JsonConvert.DeserializeObject<JObject>(dataDetailsProduct);
                            var isSuccessResponseDetailsProduct = ConvertResponseDetailsProduct["success"].Value<bool>();
                            if (isSuccessResponseDetailsProduct == false)
                            {
                                TempData["ErrorMessage"] = "Có lỗi xảy ra";
                                return RedirectToAction("Index", "Product");
                            }
                        }
                        else
                        {
                            return StatusCode((int)responseDetailsProduct.StatusCode);
                        };
                    }
                    TempData["SuccessMessage"] = "Thay đổi thông tin sản phẩm thành công";
                    return RedirectToAction("Index", "Product");

                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                };
            }
            TempData["ErrorMessage"] = "Có lỗi xảy ra";
            return RedirectToAction("Index", "Product");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id )
        {
            //Ẩn sản phẩm isDelete = true
            SetAuthorizationHeader();
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"Products/DeleteProduct/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                var dataResponse = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(dataResponse);
                var isSuccess = ConvertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = ConvertResponse["message"].Value<string>();
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện thao tác";
                }               
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            };
            return RedirectToAction("index", "Product");
        }
        [HttpPost]
        public IActionResult GetPartialViewEdit([FromBody] ProductVM model)
        {
            var ListBrand = new List<BrandVM>();
            HttpResponseMessage responseBrand = _client.GetAsync(_client.BaseAddress + "Brands/GettAllBrand").Result;
            if (responseBrand.IsSuccessStatusCode)
            {
                string data = responseBrand.Content.ReadAsStringAsync().Result;
                ListBrand = JsonConvert.DeserializeObject<List<BrandVM>>(data);
                ViewBag.Brand = ListBrand;
            }
            else
            {
                return StatusCode((int)responseBrand.StatusCode);
            };
            var ListSupplier = new List<SupplierVM>();
            HttpResponseMessage responseSupplier = _client.GetAsync(_client.BaseAddress + "Suppliers/GetAllSupplier").Result;
            if (responseSupplier.IsSuccessStatusCode)
            {
                string data = responseSupplier.Content.ReadAsStringAsync().Result;
                ListSupplier = JsonConvert.DeserializeObject<List<SupplierVM>>(data);
                ViewBag.Supplier = ListSupplier;
            }
            else
            {
                return StatusCode((int)responseSupplier.StatusCode);
            };
            var ListColor = new List<ColorVM>();
            HttpResponseMessage responseColor = _client.GetAsync(_client.BaseAddress + "Colors/GetAllColor").Result;
            if (responseColor.IsSuccessStatusCode)
            {
                string data = responseColor.Content.ReadAsStringAsync().Result;
                ListColor = JsonConvert.DeserializeObject<List<ColorVM>>(data);
                ViewBag.Color = ListColor;
            }
            else
            {
                return StatusCode((int)responseColor.StatusCode);
            };
            var ListSize = new List<SizeVM>();
            HttpResponseMessage responseSize = _client.GetAsync(_client.BaseAddress + "Sizes/GetAllSize").Result;
            if (responseSize.IsSuccessStatusCode)
            {
                string data = responseSize.Content.ReadAsStringAsync().Result;
                ListSize = JsonConvert.DeserializeObject<List<SizeVM>>(data);
                ViewBag.Size = ListSize;
            }
            else
            {
                return StatusCode((int)responseSize.StatusCode);
            };
            var ListCategory = new List<DanhmucVM>();
            HttpResponseMessage responseCategory = _client.GetAsync(_client.BaseAddress + "Categories/GetAllCategory").Result;
            if (responseCategory.IsSuccessStatusCode)
            {
                string data = responseCategory.Content.ReadAsStringAsync().Result;
                ListCategory = JsonConvert.DeserializeObject<List<DanhmucVM>>(data);
                ViewBag.Category = ListCategory;
            }
            else
            {
                return StatusCode((int)responseCategory.StatusCode);
            };
            return PartialView("~/Views/Shared/_ProductEdit.cshtml", model);
        }
        [HttpPost]
        public IActionResult GetPartialViewDetails([FromBody] ProductVM model)
        {
            
            return PartialView("~/Views/Shared/_ProductDetails.cshtml", model);
        }
    }
}
