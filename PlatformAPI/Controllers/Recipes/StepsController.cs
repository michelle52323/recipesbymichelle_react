using Microsoft.AspNetCore.Mvc;
using PlatformAPI.Data;
using PlatformAPI.Helpers;
using PlatformAPI.DTO.Recipe;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;
using Microsoft.AspNetCore.Authorization;

namespace PlatformAPI.Controllers.Recipes
{
    #region DTOs


    public class StepGridDto
    {
        public List<StepDto> Steps { get; set; } = new();
        public List<UnitDto> ValidUnits { get; set; } = new();
    }

    public class InsertStepDTO : StepDto
    {
        public int RecipeId { get; set; }
    }

    public class StepSortOrderDto
    {
        public int Id { get; set; }          // StepId
        public int SortOrder { get; set; }
    }


    #endregion


    [ApiController]
    [Route("api/[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StepsController(AppDbContext context)
        {
            _context = context;
        }
        

        [HttpGet("steps/{recipeId}")]
        public async Task<ActionResult<List<StepDto>>> GetSteps(int recipeId)
        {
            // Load the recipe to ensure it exists
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == recipeId && r.IsActive);

            if (recipe == null)
                return NotFound();

            // Load steps for this recipe
            var steps = await _context.Steps
                .Include(i => i.RecipeStep)
                .Where(i => i.RecipeStep.RecipeId == recipeId && i.IsActive)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();


            // Map to DTOs
            var stepDtos = steps
                .Select(i => new StepDto
                {
                    Id = i.Id,
                    Description = i.Description,
                    SortOrder = i.SortOrder ?? 0,
                    IsActive = i.IsActive
                })
                .ToList();

            return Ok(stepDtos);
        }

        #region Insert / Update functions

        [HttpPost]
        public async Task<IActionResult> InsertStep([FromBody] InsertStepDTO dto)
        {
            try
            {

                // 2. Validate RecipeId
                if (dto.RecipeId <= 0)
                {
                    return BadRequest(new { success = false, message = "RecipeId is required" });
                }

                int recipeId = dto.RecipeId;

                // 3. Compute next SortOrder (simplified)
                var maxSortOrder = await _context.Steps
                    .Where(i => i.RecipeStep.RecipeId == recipeId && i.IsActive)
                    .MaxAsync(i => (int?)i.SortOrder) ?? 0;


                int nextSortOrder = maxSortOrder + 1;

                // 4. Create Step entity
                var step = new Step
                {
                    Description = dto.Description,
                    SortOrder = nextSortOrder,
                    IsActive = true
                };

                _context.Steps.Add(step);
                await _context.SaveChangesAsync(); // generates Step.Id

                // 5. Create UserRecipe join record
                var recipeStep = new RecipeStep
                {
                    RecipeId = recipeId,
                    StepId = step.Id
                };

                _context.RecipeSteps.Add(recipeStep);
                await _context.SaveChangesAsync();

                // 6. Return success + updated DTO
                dto.Id = step.Id;
                dto.SortOrder = nextSortOrder;
                dto.IsActive = true;

                return Ok(new { success = true, step = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }



        [HttpPut]
        public async Task<IActionResult> UpdateStep([FromBody] StepDto dto)
        {
            var step = await _context.Steps.FindAsync(dto.Id);
            if (step == null)
                return NotFound();

            // Map DTO → Entity
            step.Description = dto.Description;
            step.SortOrder = dto.SortOrder ?? step.SortOrder;
            step.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        #endregion

        #region Sorting functions

        [HttpPost("update-sort-order")]
        public async Task<IActionResult> UpdateStepSortOrder(
        [FromBody] List<StepSortOrderDto> updates)
        {
            try
            {
                // Extract step IDs from payload
                var stepIds = updates.Select(u => u.Id).ToList();

                // Load steps that belong to this recipe through RecipeStep
                var steps = await _context.Steps
                    .Where(i => stepIds.Contains(i.Id)
                                && i.IsActive
                                && i.RecipeStep.RecipeId != null)
                    .ToListAsync();

                // Apply updates
                foreach (var step in steps)
                {
                    var update = updates.FirstOrDefault(u => u.Id == step.Id);
                    if (update != null)
                    {
                        step.SortOrder = update.SortOrder;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }



        #endregion

        #region Delete functions

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStep(int id)
        {
            try
            {
                var step = await _context.Steps.FindAsync(id);
                if (step == null)
                {
                    return BadRequest(new { success = false, message = "Step not found" });
                }

                step.IsActive = false;
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }


        #endregion


    }
}
