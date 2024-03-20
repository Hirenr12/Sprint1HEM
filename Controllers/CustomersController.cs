using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprint1HEM.Models;
using System.Security.Cryptography;
using System.Collections.Concurrent;
namespace Sprint1HEM.Controllers
{
    public class CustomersController : Controller
    {
        private readonly RacersContext _context;

        public CustomersController(RacersContext context)
        {
            _context = context;
        }
        public sealed class LoginLogger
        {
            private static readonly LoginLogger instance = new LoginLogger();
            private static readonly object padlock = new object();
            private readonly string filePath = "login_attempts.txt";
            private ConcurrentDictionary<string, int> loginAttempts;

            private LoginLogger()
            {
                loginAttempts = new ConcurrentDictionary<string, int>();
            }

            public static LoginLogger Instance
            {
                get { return instance; }
            }

            public void LogFailedLoginAttempt(string email)
            {
                lock (padlock)
                {
                    loginAttempts.AddOrUpdate(email, 1, (key, oldValue) => oldValue + 1);
                    try
                    {
                        using (StreamWriter writer = System.IO.File.AppendText(filePath))
                        {
                            writer.WriteLine($"{DateTime.Now}: Failed login attempt for user '{email}'. Attempts: {loginAttempts[email]}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // You can handle the exception as per your application's requirements
                        // For example, log the exception and proceed gracefully
                        Console.WriteLine($"Failed to write to log file: {ex.Message}");
                    }
                }
            }
        }


        /*[HttpGet]
        public IActionResult Index() 
        { 
            return View();
        }*/

        // GET: Users
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(await _context.Customers.ToListAsync());
        }


        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Customer model)
        {
            // Retrieve the user from the database based on the provided username
            var user = _context.Customers.SingleOrDefault(u => u.Email == model.Email);

            if (user != null && !string.IsNullOrEmpty(model.Password))
            {
                // Hash the provided password for comparison with the hashed password stored in the database
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                    string hashedPassword = Convert.ToBase64String(bytes);

                    // Check if the hashed password matches the one stored in the database
                    if (user.Password == hashedPassword)
                    {
                        // Authentication successful
                        // Store user information in session
                        HttpContext.Session.SetString("UserEmail", user.Email.ToString());
                       
                        // Redirect to another action or view upon successful login
                        return RedirectToAction("Index", "Items");
                    }
                }
            }

            // Invalid username or password, add a model error
            ModelState.AddModelError(string.Empty, "Invalid username or password");

            // Pass error message to error view
            TempData["ErrorMessage"] = "Invalid username or password";

            return View();
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register([Bind("Email,Password,FullName,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Hash the password using SHA256
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(customer.Password));
                    customer.Password = Convert.ToBase64String(bytes);
                }
                customer.Manager = 0;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(customer);
        }
       
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Email,Password,FullName,Address")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
