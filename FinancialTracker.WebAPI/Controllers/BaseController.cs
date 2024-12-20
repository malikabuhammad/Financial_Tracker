using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialTracker.WebAPI.Controllers
{
    //custom controller for global methods , etc 

    [ApiController]
 
    public abstract class BaseController : ControllerBase
    {
        // Helper method to get the UserID from the JWT token
        protected int GetUserId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID is missing in the token.");
            }

            return int.Parse(userIdClaim);
        }
    }
}
