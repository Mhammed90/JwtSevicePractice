

namespace JWTPractice.Services
{
    public class AuthoServices : IAuthoServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthoServices(UserManager<ApplicationUser> userManager,
            IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        #region AddRole function
        public async Task<string> AddRole(RoleModelDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || await _roleManager.FindByNameAsync(model.RoleName) == null)
            {
                return "Role or userId Is Invild";
            }
            if (await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return "User already assigned to this role";
            }
            var res = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (res.Succeeded)
            {
                return string.Empty;
            }
            return "Something Went Wrong";

        }
        #endregion


        #region Login function
        public async Task<AuthoModel> Login(LoginModelDto model)
        {
            var authmodel = new AuthoModel();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authmodel.Message = "Email or Password is incorrect";
                return authmodel;
            }

            var myToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);


            authmodel.Email = user.Email;
            authmodel.Username = user.UserName;
            authmodel.IsAuthenticated = true;
            authmodel.Roles = roles.ToList();
            authmodel.ExpiresOn = myToken.ValidTo;
            authmodel.Tokens = new JwtSecurityTokenHandler().WriteToken(myToken);

            return authmodel;

        }
        #endregion

        #region RegisterUser Function
        public async Task<AuthoModel> RegisterModle(RegisterModelDto model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new AuthoModel { Message = "UserName Is already Exist" };
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthoModel { Message = "Email Is already Exist" };
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FristName = model.FirstName,
                LastName = model.LastName,

            };

            var res = await _userManager.CreateAsync(user, password: model.Password);
            if (!res.Succeeded)
            {
                var errors = "";
                foreach (var error in res.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthoModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");

            var myToken = await CreateJwtToken(user);
            return new AuthoModel
            {
                Email = user.Email,
                Username = user.UserName,
                IsAuthenticated = true,
                Roles = new List<string> { "User", },
                ExpiresOn = myToken.ValidTo,
                Tokens = new JwtSecurityTokenHandler().WriteToken(myToken),

            };
        }
        #endregion


        #region Create TokensMethod
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issure,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.ExpiresInDayes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        #endregion
    }
}
