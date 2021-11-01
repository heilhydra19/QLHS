using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using QLHS.Models;

namespace QLHS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public StudentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpGet("login")]
        public async Task<ActionResult<Student>> Login(string username, string password)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }


        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(int id, string newPass)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == id);
            if(student == null)
            {
                return NotFound();
            }
            student.Password = newPass;
            _context.Update(student);
            _context.SaveChanges();
            if(student.Email != null)
            {
                try
                {
                    SendMail(student.Name, student.Email);
                }
                catch { }
            }
            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        ////
        //static string[] Scopes = { GmailService.Scope.GmailSend };
        //static string ApplicationName = "SendMail";
        //public static string Base64UrlEncode(string input)
        //{
        //    var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        //    return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        //}
        //void SendMail(string email)
        //{
        //    UserCredential credential;
        //    using (FileStream stream = new FileStream(@"./credentials.json", FileMode.Open, FileAccess.Read))
        //    {
        //        string credPath = @"./bin/Release/net5.0/token.json";
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.Load(stream).Secrets,
        //            Scopes,
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //    }

        //    string plainText = $"To: {email}\r\n" +
        //                       $"Subject: Password has changed\r\n" +
        //                       "Content-Type: text/html; charset=utf-8\r\n\r\n" +
        //                       $"<p>Password has changed at {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}</p>";

        //    var service = new GmailService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });

        //    var newMsg = new Google.Apis.Gmail.v1.Data.Message();
        //    newMsg.Raw = Base64UrlEncode(plainText.ToString());
        //    service.Users.Messages.Send(newMsg, "me").Execute();
        //}
        void SendMail(string name, string email)
        {
            MimeMessage message = new MimeMessage();
            using (var client = new SmtpClient())
            {
                message.From.Add(new MailboxAddress("Lãnh tụ", "khoaanhdang11@gmail.com"));
                message.To.Add(new MailboxAddress(name, email));
                message.Subject = "PASSWORD CHANGED";

                var bodyBuilder = new BodyBuilder(); 
                bodyBuilder.HtmlBody = "<p>Your password has been changed on " + DateTime.UtcNow.AddHours(7).ToString("dddd, MMMM d, yyyy hh:mm:ss") + "</p>";
                message.Body = bodyBuilder.ToMessageBody();

                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                client.Authenticate("khoaanhdang11@gmail.com", "gmejtlyjhrmdlbhv");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
