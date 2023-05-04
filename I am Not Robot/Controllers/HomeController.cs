using I_am_Not_Robot.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace I_am_Not_Robot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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


        public ActionResult CaptchaImage()
        {
            Bitmap bitmap = new Bitmap(100, 30, PixelFormat.Format24bppRgb);
            Graphics graphic = Graphics.FromImage(bitmap);

            Random r = new Random();
            string captchaNum = GenerateCaptchaCode();
            //put code here and chech this code in Login Post request first
            TempData["CaptchaCode"] = captchaNum.ToLower();

            // 'PrivateFontCollection' is in the 'System.Drawing.Text' namespace
            PrivateFontCollection foo = new PrivateFontCollection();
            Random rnd = new Random();
            //or Use any fonts you want. 
            //Better to use fonts with not good details.
            var path = Path.Combine(AppContext.BaseDirectory, "CaptchaFonts/ExposureCMixOne.ttf");
            // Provide the path to the font on the filesystem
            foo.AddFontFile(path);

            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), LinearGradientMode.Horizontal);


            float[] positions = { 0.0f, 0.5f, 1.0f };
            Color startColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Color middleColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Color endColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = new Color[] { startColor, middleColor, endColor };
            colorBlend.Positions = positions;

            LinearGradientBrush brush2 = new LinearGradientBrush(
                new Point(0, 0),
                new Point(bitmap.Width, 0),
                startColor,
                endColor);

            brush2.InterpolationColors = colorBlend;

            graphic.FillRectangle(brush2, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            graphic.DrawString(captchaNum.ToString(), new Font((FontFamily)foo.Families[0], 16f), brush, 0, 0);

            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);

            return File(memoryStream.ToArray(), "image/png");

        }


        private string GenerateCaptchaCode()
        {
            string code = "";
            try
            {
                int seed = (int)DateTime.Now.Ticks;
                Random firtrand = new Random(Guid.NewGuid().GetHashCode());
                // you can Add All chars you want. or You can take it from admin user.
                string chars = "0123456789";
                Random random = new Random(DateTime.Now.Hour + DateTime.Now.Millisecond);
                code = new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(6).ToArray());


            }
            catch (Exception)
            {
            }
            return code;
        }
    }
}