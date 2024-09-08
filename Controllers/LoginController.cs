using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EXE101.DataAccess;
using System.Numerics;
using Microsoft.AspNetCore.Identity;
using static System.Net.WebRequestMethods;

namespace EXE101.Controllers
{
    public class LoginController : Controller
    {
        private readonly EXE101DBContext _context;
        private readonly IEmailSender _emailSender;


        public LoginController(EXE101DBContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'EXE101DBContext.Users'  is null.");
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'EXE101DBContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string EmailLogin, string PasswordLogin)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == EmailLogin && u.Password == PasswordLogin);

            if (user != null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password. Please try again!.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendOTP([FromBody] string EmailRegister)
        {
          var user = _context.Users.FirstOrDefault(u => u.Email == EmailRegister);
            if (user == null)
            {   
                    Random random = new Random();
                    int randomNumber = random.Next(100000, 999999);
                    var message = "OTP: " + randomNumber;
                    HttpContext.Session.SetString("SavedOTP", randomNumber.ToString());

                    await _emailSender.SendEmailAsync(EmailRegister, "Confirm your Email", message);

                    return Json(new { success = true });
            }
            else
            {
                ModelState.AddModelError("EmailRegister", "Email already exists. Please try again!.");
                return Json(new { success = false, errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)) });
            }    
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register([FromBody] RegisterModel model) 
        {
            var nameRegister = model.NameRegister;
            var emailRegister = model.EmailRegister;
            var passwordRegister = model.PasswordRegister;
            var otp = model.OTP;
            var savedOTP = HttpContext.Session.GetString("SavedOTP");
            var user = _context.Users.FirstOrDefault(u => u.Name == nameRegister);
            if (user == null)
            {
                if (otp == savedOTP) 
                {
                    var newUser = new User
                    {
                        Name = nameRegister,
                        Email = emailRegister,
                        Password = passwordRegister
                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("EmailRegister", "OTP does not match. Please try again!.");
                    return Json(new { success = false, errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)) });
                }
            }
            else
            {
                ModelState.AddModelError("EmailRegister", "Name already exists. Please try again!.");
                return Json(new { success = false, errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)) });
            }
        }

        public class RegisterModel
        {
            public string NameRegister { get; set; }
            public string PasswordRegister { get; set; }
            public string EmailRegister { get; set; }
            public string OTP { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPass([FromBody] string emailLogin)
        {
            if (string.IsNullOrEmpty(emailLogin))
            {
                ModelState.AddModelError("EmailLogin", "Email is required!.");
                return Json(new { success = false, errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)) });
            }
            else
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == emailLogin);
                if (user == null)
                {
                    ModelState.AddModelError("EmailLogin", "Email does not exist!.");
                    return Json(new { success = false, errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)) });
                }
                else
                {
                    Random random = new Random();
                    int randomNumber = random.Next(100000, 999999);
                    var message = "OTP: " + randomNumber;
                    HttpContext.Session.SetString("SavedOTPLogin", randomNumber.ToString());
                    HttpContext.Session.SetString("EmailChangePass", emailLogin);

                    await _emailSender.SendEmailAsync(emailLogin, "Change your password", message);
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public ActionResult VerifyOTP([FromBody]string otpnumber)
        {
            var savaOTPLogin = HttpContext.Session.GetString("SavedOTPLogin");
            if (otpnumber == savaOTPLogin)
            {
                return Json(new { success = true });
            }
            else
            { 
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult ChangePass([FromBody] string newPassWord)
        {
            var emailChangePass = HttpContext.Session.GetString("EmailChangePass");
            var user = _context.Users.FirstOrDefault(u => u.Email == emailChangePass);
            user.Password = newPassWord.ToString();
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
