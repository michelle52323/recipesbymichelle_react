using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Models.Users;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Data;
using Microsoft.AspNetCore.Authorization;
using PlatformAPI.Helpers;
using PlatformAPI.Enums;

namespace PlatformAPI.Controllers.Recipes
{

    #region DTOs
    public class FractionDecimalDto
    {
        public int Id { get; set; }
        public string? Fraction { get; set; }
        public float? Decimal { get; set; }
        public bool Primary { get; set; }
    }

    public class UnitDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Abbreviation { get; set; }
        public int? System { get; set; }
        public string? Plural { get; set; }
    }


    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeasurementController(AppDbContext context)
        {
            _context = context;
        }

        #region Get Functions

        // ------------------------------------------------------------
        // GET: /api/measurement/fractions
        // Returns ALL FractionDecimal rows as DTOs
        // ------------------------------------------------------------
        [HttpGet("fractions")]
        public async Task<ActionResult<List<FractionDecimalDto>>> GetFractionTable()
        {
            var fractions = await _context
                .Set<FractionDecimal>()
                .ToListAsync();

            var dto = fractions.Select(f => new FractionDecimalDto
            {
                Id = f.Id,
                Fraction = f.Fraction,
                Decimal = f.Decimal,
                Primary = f.Primary
            }).ToList();

            return Ok(dto);
        }

        // ------------------------------------------------------------
        // GET: /api/measurement/units
        // Returns units filtered by user's MeasurementSystem claim
        // ------------------------------------------------------------
        [HttpGet("units")]
        public async Task<ActionResult<List<UnitDto>>> GetUnits()
        {
            // Extract measurement system from auth claims
            var claimValue = User?
                .Claims
                .FirstOrDefault(c => c.Type == "MeasurementSystem")
                ?.Value;

            MeasurementSystem measurementSystem =
                Enum.TryParse<MeasurementSystem>(claimValue, ignoreCase: true, out var parsedEnum)
                    ? parsedEnum
                    : MeasurementSystem.Imperial; // default

            // Load all units
            var allUnits = await _context
                .Set<Unit>()
                .ToListAsync();

            // Filter:
            // 1. Units matching the user's measurement system
            // 2. Units where System is null (universal)
            var filtered = allUnits
                .Where(u =>
                    u.System == (int)measurementSystem ||
                    u.System == null
                )
                .ToList();

            // Convert to DTOs
            var dto = filtered.Select(u => new UnitDto
            {
                Id = u.Id,
                Description = u.Description,
                Abbreviation = u.Abbreviation,
                System = u.System,
                Plural = u.Plural
            }).ToList();

            return Ok(dto);
        }


        #endregion
    }
}
