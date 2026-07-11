using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlatformAPI.Configuration;
using PlatformAPI.Data;
using PlatformAPI.Models.Users;
using PlatformAPI.Security;
using PlatformAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace PlatformAPI.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestAccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        private readonly EmailService _emailService;
        private readonly OrganizationSettings _organizationSettings;
        private readonly ILogger<SignInController> _logger;

        public GuestAccountController(
            AppDbContext context,
            AuthService authService,
            EmailService emailService,
            IOptions<OrganizationSettings> organizationSettings,
            ILogger<SignInController> logger)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            _organizationSettings = organizationSettings.Value;
            _logger = logger;
        }

        public class GuestAccessRequest
        {
            public Guid? DeviceId { get; set; }
        }

        public class GuestAccessResponse
        {
            public bool Success { get; set; }
            public bool AccessGranted { get; set; }
            public Guid? DeviceId { get; set; }
        }

        [HttpPost("access")]
        public async Task<IActionResult> AccessGuestAccount([FromBody] GuestAccessRequest request)
        {
            try
            {
                Guid? deviceId = request.DeviceId;

                var userType = new UserType();
                userType.Id = 5;
                userType.Description = "Guest User";
                userType.Code = "G";

                //
                // CASE 1: DeviceId was supplied
                //
                if (deviceId.HasValue)
                {
                    var user = await _context.Users
                        .Include(u => u.UserType)
                        .Include(u => u.Gender)
                        .FirstOrDefaultAsync(u => u.UserTypeId == 5 && u.DeviceId == deviceId.Value);

                    if (user != null)
                    {
                        var days = (DateTime.UtcNow - user.CreatedAt).TotalDays;

                        if (days < 10)
                        {
                            // Grant access
                            await _authService.SignInUserAsync(user);

                            return Ok(new GuestAccessResponse
                            {
                                Success = true,
                                AccessGranted = true,
                                DeviceId = null // FE already has it
                            });
                        }
                        else
                        {
                            // Deny access
                            return Ok(new GuestAccessResponse
                            {
                                Success = true,
                                AccessGranted = false,
                                DeviceId = null
                            });
                        }
                    }

                    //
                    // DeviceId supplied but no user found → create new guest user
                    //
                    var newUser = new User
                    {
                        Username = $"guest-{Guid.NewGuid()}",
                        FirstName = "Guest",
                        Password = PasswordHasher.Hash(new Guid().ToString()),
                        UserTypeId = 5,
                        ThemeId = 1,
                        MeasurementSystem = Enums.MeasurementSystem.Imperial,
                        HasSelectedMeasurementSystem = true,
                        DeviceId = deviceId.Value,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    newUser.UserType = userType;
                    await _authService.SignInUserAsync(newUser);

                    return Ok(new GuestAccessResponse
                    {
                        Success = true,
                        AccessGranted = true,
                        DeviceId = null // FE already has it
                    });
                }

                //
                // CASE 2: DeviceId was NOT supplied → generate new GUID + new guest user
                //
                Guid newDeviceId = Guid.NewGuid();

                var createdUser = new User
                {
                    Username = $"guest-{Guid.NewGuid()}",
                    FirstName = "Guest",
                    Password = PasswordHasher.Hash(new Guid().ToString()),
                    UserTypeId = 5,
                    ThemeId = 1,
                    MeasurementSystem = Enums.MeasurementSystem.Imperial,
                    HasSelectedMeasurementSystem = true,
                    DeviceId = newDeviceId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(createdUser);
                await _context.SaveChangesAsync();

                createdUser.UserType = userType;

                await _authService.SignInUserAsync(createdUser);

                return Ok(new GuestAccessResponse
                {
                    Success = true,
                    AccessGranted = true,
                    DeviceId = newDeviceId // FE must store this
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = ex.Message,
                    Stack = ex.StackTrace
                });
            }

        }
    }
}
