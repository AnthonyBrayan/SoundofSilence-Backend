//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;



//namespace Segurity.TokenHandlerService
//{
//    public class TokenHandlerService
//    {
//        private readonly IConfiguration _configuration;

//        public TokenHandlerService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        private int? ExtractUserIdFromAuthorizationHeader(HttpContext httpContext)
//        {
//            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

//            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
//            {
//                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
//                var userId = DecodeUserIdFromToken(token);
//                return userId;
//            }

//            return null;
//        }

//        private int? DecodeUserIdFromToken(string token)
//        {
//            // Configura la validación de tokens
//            var tokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = false, // Puedes ajustar esto según tu configuración
//                ValidateAudience = false, // Puedes ajustar esto según tu configuración
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"])), // Reemplaza con tu clave secreta
//            };

//            try
//            {
//                // Crea un token handler
//                var tokenHandler = new JwtSecurityTokenHandler();

//                // Valida y deserializa el token
//                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

//                // Extrae el ID de usuario del token
//                var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name); // "sub" es la claim común para el ID de usuario

//                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
//                {
//                    return userId;
//                }
//            }
//            catch (SecurityTokenExpiredException ex)
//            {
//                // El token ha expirado, puedes manejar esto de acuerdo a tus necesidades
//                Console.WriteLine("El token ha expirado: " + ex.Message);
//            }
//            catch (SecurityTokenInvalidSignatureException ex)
//            {
//                // La firma del token no es válida, puedes manejar esto de acuerdo a tus necesidades
//                Console.WriteLine("La firma del token no es válida: " + ex.Message);
//            }
//            catch (Exception ex)
//            {
//                // Manejo genérico para otras excepciones
//                Console.WriteLine("Error al descodificar o validar el token: " + ex.Message);
//            }

//            return null;
//        }


//    }
//}
