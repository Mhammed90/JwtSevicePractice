
namespace JWTPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthController : ControllerBase
    {

        private readonly IAuthoServices _authoServices;


        public AuthController(IAuthoServices authoServices)
        {
            _authoServices = authoServices;
        }
        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _authoServices.RegisterModle(model);

            if (!res.IsAuthenticated)
                return BadRequest(res.Message);

            return Ok(res);

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _authoServices.Login(model);

            if (!res.IsAuthenticated)
                return BadRequest(res.Message);

            return Ok(res);

        }



        [HttpPost("AddRole")]

        public async Task<IActionResult> AddtorRole(RoleModelDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _authoServices.AddRole(modelDto);
            if (!string.IsNullOrEmpty(res))

                return BadRequest(res);

            return Ok(modelDto);
        }
    }

}