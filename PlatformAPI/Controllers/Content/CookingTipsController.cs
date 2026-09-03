using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using System.Linq;

namespace PlatformAPI.Controllers.CookingTips
{
    #region DTOs

    public class CookingTipRequestDto
    {
        public List<int> ExcludeIds { get; set; } = new();
    }

    public class CookingTipResponseDto
    {
        public int TipId { get; set; }
        public string TipText { get; set; }
    }

    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class CookingTipsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CookingTipsController(AppDbContext context)
        {
            _context = context;
        }

        #region Shared Endpoint Functions

        private async Task<IActionResult> GetCookingTipInternal(int userId, CookingTipRequestDto dto)
        {
            if (userId <= 0)
                return Unauthorized("UserId claim missing or invalid.");

            // Get all tips
            var allTips = await _context.CookingTips
                .Where(t => t.IsActive)
                .ToListAsync();

            if (!allTips.Any())
                return NotFound("No cooking tips found.");

            // Filter out excluded IDs
            var filteredTips = allTips
                .Where(t => !dto.ExcludeIds.Contains(t.Id))
                .ToList();

            // If everything is excluded, fallback to full list
            if (!filteredTips.Any())
                filteredTips = allTips;

            // Random selection
            var random = new Random();
            var selectedTip = filteredTips[random.Next(filteredTips.Count)];

            var response = new CookingTipResponseDto
            {
                TipId = selectedTip.Id,
                TipText = selectedTip.Description
            };

            return Ok(response);
        }

        #endregion

        #region Real Endpoints

        [Authorize]
        [HttpPost("getTip")]
        public async Task<IActionResult> GetTip([FromBody] CookingTipRequestDto dto)
        {
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            return await GetCookingTipInternal(userId, dto);
        }

        #endregion

        #region Mock Endpoints

        [HttpPost("getTipMock")]
        public async Task<IActionResult> GetTipMock([FromBody] CookingTipRequestDto dto)
        {
            const int mockUserId = 10;
            return await GetCookingTipInternal(mockUserId, dto);
        }

        #endregion
    }
}
