using Microsoft.AspNetCore.Mvc;

namespace AcumaticaUniversity.ValidationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnapshotController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(SnapshotDto snapshotDto)
        {
            var path = $"C:\\db\\{snapshotDto.LessonId}.bak";

            var database = new Database();
            database.Restore(path);

            return Ok();
        }
    }
}
