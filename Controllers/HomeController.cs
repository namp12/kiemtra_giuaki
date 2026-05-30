using Kiemtragiuaki.Data;
using Kiemtragiuaki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kiemtragiuaki.Controllers
{
    public class HomeController : Controller
    {
        private readonly ComputerStoreContext _context;

        public HomeController(ComputerStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId, int? brandId, string? searchString)
        {
            // Get categories and brands for filtering
            var categories = await _context.Categories.Where(c => c.Status == "active").ToListAsync();
            var brands = await _context.Brands.Where(b => b.Status == "active").ToListAsync();

            // Query products
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.Status == "active" || p.Status == "published" || string.IsNullOrEmpty(p.Status));

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.BrandId == brandId.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString) || (p.ShortDescription != null && p.ShortDescription.Contains(searchString)));
            }

            var products = await productsQuery.OrderByDescending(p => p.CreatedAt).ToListAsync();

            // Pass lists to view bag
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedBrand = brandId;
            ViewBag.SearchString = searchString;

            // Get statistics for the dashboard/metrics section
            ViewBag.TotalProducts = await _context.Products.CountAsync();
            ViewBag.TotalCategories = categories.Count;
            ViewBag.TotalBrands = brands.Count;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .Include(p => p.Specifications)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Add dummy data action to help user seed database in case it's empty
        [HttpPost]
        public async Task<IActionResult> SeedDemoData()
        {
            // Clear existing if user wants to refresh with full list
            if (_context.Products.Any())
            {
                _context.Carts.RemoveRange(_context.Carts);
                _context.OrderItems.RemoveRange(_context.OrderItems);
                _context.Reviews.RemoveRange(_context.Reviews);
                _context.Specifications.RemoveRange(_context.Specifications);
                _context.ProductImages.RemoveRange(_context.ProductImages);
                _context.Products.RemoveRange(_context.Products);
                _context.Categories.RemoveRange(_context.Categories);
                _context.Brands.RemoveRange(_context.Brands);
                await _context.SaveChangesAsync();
            }

            // Create Categories
            var cat1 = new Category { Name = "Laptop", Status = "active", Description = "Laptop gaming, văn phòng và sáng tạo nội dung" };
            var cat2 = new Category { Name = "Linh Kiện PC", Status = "active", Description = "VGA, CPU, Mainboard, RAM, Nguồn máy tính" };
            var cat3 = new Category { Name = "Phụ Kiện", Status = "active", Description = "Bàn phím, chuột, tai nghe gaming và gear cao cấp" };
            _context.Categories.AddRange(cat1, cat2, cat3);
            await _context.SaveChangesAsync();

            // Create Brands
            var brandAsus = new Brand { Name = "ASUS", Status = "active", Country = "Taiwan", Logo = "https://logos-world.net/wp-content/uploads/2020/07/Asus-Logo.png" };
            var brandIntel = new Brand { Name = "Intel", Status = "active", Country = "USA", Logo = "https://upload.wikimedia.org/wikipedia/commons/c/c9/Intel-logo.svg" };
            var brandRazer = new Brand { Name = "Razer", Status = "active", Country = "USA", Logo = "https://upload.wikimedia.org/wikipedia/commons/4/40/Razer_snake_logo.svg" };
            var brandApple = new Brand { Name = "Apple", Status = "active", Country = "USA", Logo = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg" };
            var brandCorsair = new Brand { Name = "Corsair", Status = "active", Country = "USA" };
            var brandLogi = new Brand { Name = "Logitech", Status = "active", Country = "Switzerland" };
            _context.Brands.AddRange(brandAsus, brandIntel, brandRazer, brandApple, brandCorsair, brandLogi);
            await _context.SaveChangesAsync();

            // Products list
            var products = new List<Product>
            {
                // Laptops
                new Product
                {
                    Name = "ASUS ROG Strix G16 (2025) Gaming Laptop",
                    Slug = "asus-rog-strix-g16",
                    Sku = "ROG-G16-001",
                    ShortDescription = "Laptop gaming đỉnh cao với CPU Intel Core i9 và GPU RTX 4070, RAM 16GB, SSD 1TB, màn hình 240Hz Nebula.",
                    FullDescription = "ASUS ROG Strix G16 mang đến sức mạnh chiến game vô song với bộ vi xử lý Intel Core i9 thế hệ mới nhất và card đồ họa NVIDIA GeForce RTX 4070. Thiết kế tản nhiệt 3 quạt thông minh giúp máy luôn mát mẻ trong những trận chiến nảy lửa. Màn hình ROG Nebula 16 inch 240Hz siêu mượt mà.",
                    Price = 45000000m,
                    CostPrice = 40000000m,
                    Quantity = 10,
                    CategoryId = cat1.Id,
                    BrandId = brandAsus.Id,
                    Status = "active",
                    Featured = true,
                    DiscountPercent = 10,
                    Views = 150,
                    MainImage = "https://images.unsplash.com/photo-1603302576837-37561b2e2302?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "MacBook Pro 16 inch M3 Max (36GB RAM / 1TB SSD)",
                    Slug = "macbook-pro-16-m3-max",
                    Sku = "MBP16-M3MAX-01",
                    ShortDescription = "Siêu phẩm máy tính dành cho lập trình viên, nhà sản xuất âm nhạc và đồ họa chuyên nghiệp.",
                    FullDescription = "MacBook Pro 16 inch trang bị chip M3 Max cực đỉnh với CPU 16 nhân và GPU 40 nhân. Bộ nhớ RAM 36GB thống nhất cùng ổ cứng SSD 1TB tốc độ siêu cao. Màn hình Liquid Retina XDR độ sáng lên đến 1600 nits đem lại màu sắc chính xác tuyệt đối. Thời lượng pin cực khủng lên tới 22 giờ liên tục.",
                    Price = 92000000m,
                    CostPrice = 82000000m,
                    Quantity = 4,
                    CategoryId = cat1.Id,
                    BrandId = brandApple.Id,
                    Status = "active",
                    Featured = true,
                    DiscountPercent = 5,
                    Views = 210,
                    MainImage = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "Laptop Dell XPS 15 9530 Core i7 / RTX 4050",
                    Slug = "dell-xps-15-9530",
                    Sku = "DELL-XPS15-01",
                    ShortDescription = "Laptop cao cấp sang trọng mỏng nhẹ dành cho doanh nhân và nhà sáng tạo nội dung.",
                    FullDescription = "Dell XPS 15 9530 là chuẩn mực của sự sang trọng với khung nhôm CNC nguyên khối cực chắc chắn cùng mặt tỳ tay bằng sợi carbon cao cấp. Trang bị vi xử lý Intel Core i7-13700H thế hệ 13 và card đồ họa NVIDIA GeForce RTX 4050. Màn hình InfinityEdge OLED 3.5K cảm ứng tràn viền siêu sắc nét.",
                    Price = 55000000m,
                    CostPrice = 49000000m,
                    Quantity = 8,
                    CategoryId = cat1.Id,
                    BrandId = brandApple.Id, // Fallback if no Dell brand, but we can assign brandApple or brandAsus. Let's use brandAsus for simplicity.
                    Status = "active",
                    Featured = false,
                    DiscountPercent = 0,
                    Views = 95,
                    MainImage = "https://images.unsplash.com/photo-1593642632823-8f785ba67e45?auto=format&fit=crop&w=600&q=80"
                },

                // PC Components
                new Product
                {
                    Name = "VGA ASUS ROG Strix GeForce RTX 4090 OC 24GB",
                    Slug = "asus-rog-strix-rtx-4090",
                    Sku = "VGA-RTX4090-ROG",
                    ShortDescription = "Card đồ họa quái vật hiệu năng RTX 4090 24GB GDDR6X, tản nhiệt hầm hố, led RGB rực rỡ.",
                    FullDescription = "Card đồ họa mạnh mẽ nhất thế giới phục vụ game thủ và nhà sáng tạo nội dung chuyên nghiệp. Trang bị kiến trúc Ada Lovelace mới nhất từ NVIDIA, tản nhiệt 3.5 khe siêu mát, cánh quạt Axial-tech tăng luồng khí 23%, khung kim loại đúc chống vặn xoắn và led ARGB rực rỡ.",
                    Price = 65000000m,
                    CostPrice = 58000000m,
                    Quantity = 5,
                    CategoryId = cat2.Id,
                    BrandId = brandAsus.Id,
                    Status = "active",
                    Featured = true,
                    DiscountPercent = 5,
                    Views = 320,
                    MainImage = "https://images.unsplash.com/photo-1591488320449-011701bb6704?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "CPU Intel Core i9-14900K Raptor Lake Refresh",
                    Slug = "cpu-intel-core-i9-14900k",
                    Sku = "CPU-I9-14900K",
                    ShortDescription = "Bộ vi xử lý siêu phân luồng 24 nhân 32 luồng, xung nhịp tối đa lên tới 6.0GHz.",
                    FullDescription = "CPU Intel Core i9-14900K là đỉnh cao của dòng vi xử lý Raptor Lake Refresh với 24 nhân (8 P-core và 16 E-core) cùng 32 luồng. Xung nhịp tối đa đạt mức kỷ lục 6.0GHz ngay khi xuất xưởng nhờ công nghệ Intel Thermal Velocity Boost. Hỗ trợ RAM DDR5 và PCIe 5.0 cực nhanh.",
                    Price = 16500000m,
                    CostPrice = 14500000m,
                    Quantity = 15,
                    CategoryId = cat2.Id,
                    BrandId = brandIntel.Id,
                    Status = "active",
                    Featured = true,
                    DiscountPercent = 0,
                    Views = 240,
                    MainImage = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "Mainboard ASUS ROG Maximus Z790 Hero WiFi",
                    Slug = "mainboard-asus-rog-z790-hero",
                    Sku = "MB-ROG-Z790-H",
                    ShortDescription = "Bo mạch chủ LGA 1700 cao cấp hỗ trợ RAM DDR5, PCIe 5.0, hệ thống cấp nguồn 20+1 phase.",
                    FullDescription = "Bo mạch chủ ASUS ROG Maximus Z790 Hero là nền tảng hoàn hảo cho vi xử lý Intel Core thế hệ 13 và 14. Thiết kế tản nhiệt VRM hầm hố tích hợp màn hình led Polymo độc đáo. Hỗ trợ RAM DDR5 lên đến 7800MHz+(OC), 5 khe cắm M.2 SSD PCIe, kết nối WiFi 6E và Thunderbolt 4 tốc độ cao.",
                    Price = 18900000m,
                    CostPrice = 16800000m,
                    Quantity = 6,
                    CategoryId = cat2.Id,
                    BrandId = brandAsus.Id,
                    Status = "active",
                    Featured = false,
                    DiscountPercent = 8,
                    Views = 115,
                    MainImage = "https://images.unsplash.com/photo-1562976540-1502c2145186?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "RAM Corsair Vengeance RGB DDR5 32GB (2x16GB) 6000MHz",
                    Slug = "ram-corsair-vengeance-ddr5-32gb",
                    Sku = "RAM-CORSAIR-V32",
                    ShortDescription = "Bộ nhớ RAM DDR5 hiệu năng cao với hệ thống tản nhiệt nhôm và dải led RGB lung linh.",
                    FullDescription = "RAM Corsair Vengeance RGB DDR5 mang lại hiệu suất vượt trội cho hệ thống PC thế hệ mới. Xung nhịp lên đến 6000MHz giúp tối ưu hóa băng thông truyền tải dữ liệu. Tích hợp phần mềm Corsair iCUE cho phép tùy chỉnh màu sắc led RGB linh hoạt và đồng bộ dễ dàng với các thiết bị khác.",
                    Price = 3800000m,
                    CostPrice = 3200000m,
                    Quantity = 25,
                    CategoryId = cat2.Id,
                    BrandId = brandCorsair.Id,
                    Status = "active",
                    Featured = false,
                    DiscountPercent = 0,
                    Views = 145,
                    MainImage = "https://images.unsplash.com/photo-1541029071515-84cc54f84dc5?auto=format&fit=crop&w=600&q=80"
                },

                // Accessories
                new Product
                {
                    Name = "Bàn phím cơ Razer BlackWidow V4 Pro Green Switch",
                    Slug = "razer-blackwidow-v4-pro",
                    Sku = "KB-RAZER-BWV4P",
                    ShortDescription = "Bàn phím cơ cao cấp switch Razer Green phản hồi tốt, led RGB Chroma, núm xoay đa năng.",
                    FullDescription = "Trải nghiệm gõ phím đỉnh cao cùng Razer BlackWidow V4 Pro. Bàn phím trang bị Green Clicky Switch phản hồi lực tốt, đèn led gầm 3 phía rực rỡ, cụm phím macro chuyên dụng, núm xoay đa năng Razer Dial và đệm kê tay bọc da êm ái tích hợp led RGB.",
                    Price = 6200000m,
                    CostPrice = 5000000m,
                    Quantity = 20,
                    CategoryId = cat3.Id,
                    BrandId = brandRazer.Id,
                    Status = "active",
                    Featured = false,
                    DiscountPercent = 15,
                    Views = 85,
                    MainImage = "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "Chuột Gaming Logitech G502 X Plus Lightspeed Wireless",
                    Slug = "logitech-g502-x-plus",
                    Sku = "MS-LOGI-G502XP",
                    ShortDescription = "Chuột chơi game không dây huyền thoại tích hợp switch lai Quang học-Cơ học, cảm biến HERO 25K.",
                    FullDescription = "Logitech G502 X Plus là phiên bản mới nhất của dòng chuột gaming bán chạy nhất thế giới. Trang bị Switch lai LIGHTFORCE đột phá mang lại tốc độ và độ bền vượt trội. Công nghệ truyền tín hiệu không dây LIGHTSPEED siêu tốc, cảm biến HERO 25K chính xác từng micromet cùng dải led LIGHTSYNC RGB chạy dài ấn tượng.",
                    Price = 3990000m,
                    CostPrice = 3400000m,
                    Quantity = 30,
                    CategoryId = cat3.Id,
                    BrandId = brandLogi.Id,
                    Status = "active",
                    Featured = true,
                    DiscountPercent = 10,
                    Views = 180,
                    MainImage = "https://images.unsplash.com/photo-1615663245857-ac93bb7c39e7?auto=format&fit=crop&w=600&q=80"
                },
                new Product
                {
                    Name = "Tai nghe Gaming Razer BlackShark V2 Pro (2023 Edition)",
                    Slug = "razer-blackshark-v2-pro-2023",
                    Sku = "HS-RAZER-BSV2P",
                    ShortDescription = "Tai nghe gaming không dây chuyên nghiệp cho Esport, microphone siêu rộng, driver TriForce 50mm.",
                    FullDescription = "Chuẩn mực mới của âm thanh thi đấu chuyên nghiệp Esport. Razer BlackShark V2 Pro trang bị kết nối không dây Hyperspeed Wireless cực nhạy. Microphone Razer HyperClear Super Wideband ghi lại chi tiết giọng nói chân thực nhất. Màng loa TriForce Titanium 50mm chia nhỏ âm thanh thành 3 phần riêng biệt cho âm treble trong trẻo và âm bass bùng nổ.",
                    Price = 4890000m,
                    CostPrice = 4100000m,
                    Quantity = 15,
                    CategoryId = cat3.Id,
                    BrandId = brandRazer.Id,
                    Status = "active",
                    Featured = false,
                    DiscountPercent = 5,
                    Views = 110,
                    MainImage = "https://images.unsplash.com/photo-1546435770-a3e426bf472b?auto=format&fit=crop&w=600&q=80"
                }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Create some specifications for the products
            foreach (var prod in products)
            {
                if (prod.Name.Contains("Laptop") || prod.Name.Contains("MacBook"))
                {
                    _context.Specifications.AddRange(
                        new Specification { ProductId = prod.Id, SpecName = "Hệ điều hành", SpecValue = prod.Name.Contains("MacBook") ? "macOS" : "Windows 11 Home", SortOrder = 1 },
                        new Specification { ProductId = prod.Id, SpecName = "Màn hình", SpecValue = prod.Name.Contains("MacBook") ? "16.2 inch Liquid Retina XDR" : "16.0 inch QHD+ 240Hz", SortOrder = 2 },
                        new Specification { ProductId = prod.Id, SpecName = "Dung lượng RAM", SpecValue = prod.Name.Contains("MacBook") ? "36 GB Unified" : "16 GB DDR5", SortOrder = 3 },
                        new Specification { ProductId = prod.Id, SpecName = "Ổ cứng lưu trữ", SpecValue = "1 TB SSD NVMe PCIe", SortOrder = 4 }
                    );
                }
                else if (prod.Name.Contains("GeForce") || prod.Name.Contains("RTX"))
                {
                    _context.Specifications.AddRange(
                        new Specification { ProductId = prod.Id, SpecName = "Nhân CUDA", SpecValue = "16384 Cores", SortOrder = 1 },
                        new Specification { ProductId = prod.Id, SpecName = "Bộ nhớ VRAM", SpecValue = "24GB GDDR6X", SortOrder = 2 },
                        new Specification { ProductId = prod.Id, SpecName = "Băng thông bộ nhớ", SpecValue = "384-bit", SortOrder = 3 },
                        new Specification { ProductId = prod.Id, SpecName = "Cổng kết nối", SpecValue = "HDMI 2.1a x 2, DisplayPort 1.4a x 3", SortOrder = 4 }
                    );
                }
                else
                {
                    _context.Specifications.AddRange(
                        new Specification { ProductId = prod.Id, SpecName = "Tính năng nổi bật", SpecValue = "Kết nối không dây thế hệ mới", SortOrder = 1 },
                        new Specification { ProductId = prod.Id, SpecName = "Bảo hành", SpecValue = "24 tháng chính hãng", SortOrder = 2 }
                    );
                }
            }
            await _context.SaveChangesAsync();

            // Create or retrieve default customer for reviews
            var defaultCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == "customer@futuretech.com");
            if (defaultCustomer == null)
            {
                defaultCustomer = new Customer
                {
                    Email = "customer@futuretech.com",
                    Password = "hashedpassword",
                    Fullname = "Nguyễn Hoàng Nam",
                    Status = "active",
                    EmailVerified = true
                };
                _context.Customers.Add(defaultCustomer);
                await _context.SaveChangesAsync();
            }

            // Create some reviews for the products
            foreach (var prod in products)
            {
                _context.Reviews.Add(new Review
                {
                    ProductId = prod.Id,
                    CustomerId = defaultCustomer.Id,
                    Rating = 5,
                    Title = "Tuyệt vời",
                    Comment = "Sản phẩm chất lượng cao, đúng như mô tả. Đóng gói cẩn thận và giao hàng siêu nhanh. Rất đáng tiền!",
                    Status = "approved",
                    HelpfulCount = 5
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
