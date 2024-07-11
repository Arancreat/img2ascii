using img2ascii.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace img2ascii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController(IMainService mainService) : ControllerBase
    {
        private readonly IMainService _mainService = mainService;

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertImageToASCII(IFormFile file)
        {
            try
            {
                var data = await _mainService.ConvertToAscii(file);
                return File(data, "image/jpeg", "test.jpg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex });
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
