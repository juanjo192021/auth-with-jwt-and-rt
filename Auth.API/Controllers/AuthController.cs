using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.DTOs.Auth;
using Auth.Application.UseCases.Auth;

namespace Auth.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly SignupUseCase _signupUseCase;

        public AuthController(LoginUseCase loginUseCase, SignupUseCase signupUseCase)
        {
            _loginUseCase = loginUseCase;
            _signupUseCase = signupUseCase;
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        //{
        //    var token = "";

        //    if (token == null)
        //    {
        //        return Unauthorized();
        //    }
        //    return Ok(null);
        //}

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto signupRequest)
        {

            try
            {
                // 2️⃣ Ejecutar caso de uso
                var authResponse = await _signupUseCase.Signup(signupRequest);

                // 3️⃣ Verificar resultado
                if (authResponse == null)
                {
                    return BadRequest(new { Message = "No se pudo registrar el usuario." });
                }

                // 4️⃣ Éxito → 200 OK
                return Ok(new
                {
                    Message = "Usuario registrado correctamente.",
                    Data = authResponse
                });
            }
            catch (InvalidOperationException ex)
            {
                // Error controlado de lógica (por ejemplo, usuario ya existente)
                return Conflict(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Si por algún motivo el registro implica validación de permisos
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                // Error inesperado del servidor
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ocurrió un error interno en el servidor.",
                    Error = ex.Message
                });
            }
        }

        //// GET: AuthController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: AuthController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AuthController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AuthController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AuthController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AuthController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AuthController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AuthController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
