using DataLayer.Data;
using LoggerFactory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using NgCore.Common;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using NgCore.Common.PdfHelpers;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NgCoreJwtProj.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public IUserObj _userobj;
        public UserController(IUserObj userobj ,IConfiguration configuration)
        {
            _userobj = userobj;
           // string path = configuration["PdfPath"];
            //PdfFile obj = new PdfFile(path);
            //obj.CreatePdf(path, "test2.pdf");
            //obj.ReadPdfFile(path + "IP-2958_V1(1)2.pdf");
            Configuration = configuration;

            //SmtpClient client = new SmtpClient("mysmtpserver");
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("mailid", "password");

            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("srikanthreddy.puli@gmail.com");
            //mailMessage.To.Add("srikanth.p@mwebware.com");
            //mailMessage.Body = "hi this is from code";
            //mailMessage.Subject = "test mail";
            //client.Send(mailMessage);
        }
        public IConfiguration Configuration { get; }

        [Route("GetUserList")]
        public List<User> GetListUser()
        {
           // return new List<User> { new User { id = 1, name = "srikanth", mailId = "sri@gmail.com",role="admin" }, new User { id = 2, name = "anvesh", mailId = "anv@gmail.com",role="user" } };
            return _userobj.GetUsers();
        }

        // GET: api/<controller> 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetUsers")]
        public ReturnData GetUsers()
        {
            try
            {
                List<User> users = _userobj.GetUsers();
                using (ReturnData result = new ReturnData())
                {
                    result.data = users;
                    result.isError = false;
                    result.isList = true;
                    result.message = "RecordFetch Successfull.";

                    return result;
                }

            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);

                using (ReturnData result = new ReturnData())
                {
                    result.data = null;
                    result.isError = true;
                    result.isList = false;
                    result.message = "RecordFetch Failed.";

                    return result;
                }
            }
        }

        [HttpGet("GetUsersbyId/{id}")]
        public IActionResult GetUsersbyId(int id)
        {
            try
            {
                return Ok(_userobj.GetUser(id));
            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);
                return BadRequest("Exception occurred");
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userobj.DeleteUser(id);
                return Ok(new { isError = false, user = "", Message = "Delete Successfull!" });
            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);
                return BadRequest("Exception occurred");
                return Ok(new { isError = true, user = "", Message = "delete failed!" });
            }
        }

        [HttpPost("InsertUser")]
        public IActionResult InsertUser([FromBody] User user)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                var result = _userobj.SaveUser(user);
                if (result != null)
                {
                    user.password = null;
                    return Ok(new { isError = false, user = user, Message = "Insert Successfull!" });
                }
                else
                {
                    return BadRequest(new { isError = true, User = user, Message = "Insert Failed!" });
                }
                //}
                //else
                //{
                //    user = null;
                //    return BadRequest(new { isError = true, user = user, Message = "Null Object Returned." });
                //}
            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);
                return BadRequest("Exception occurred");
            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(User user)
        {
            try
            {
                _userobj.UpdateUser(user);
                return Ok(new { isError = false, user = user, Message = "Update Successfull!" });
            }
            catch (Exception ex)
            {

                throw;
            }
        }



















        //get users with dapper
        #region Dapper
        [HttpGet("GetUserswithDapper")]
        public ReturnData GetUserswithDapper()
        {
            try
            {
              List<User> users = _userobj.GetUserswithDapper();
                using (ReturnData result=new ReturnData())
                {
                    result.data = users;
                    result.isError = false;
                    result.isList = true;
                    result.message = "RecordFetch Successfull.";

                return result;
                }

            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);

                using (ReturnData result = new ReturnData())
                {
                    result.data = null;
                    result.isError = true;
                    result.isList = false;
                    result.message = "RecordFetch Failed.";

                    return result;
                }
            }
        }

        [HttpGet("GetUsersbyIdDapper/{id}")]
        public IActionResult GetUsersbyIdDapper(int id)
        {
            try
            {
                return Ok(_userobj.GetUserByIdDapper(id));
            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);
                return BadRequest("Exception occurred"); 
            }
        }

        [HttpDelete("DeleteUserDapper/{id}")]
        public IActionResult DeleteUserDapper(int id)
        {
            try
            {
                return Ok(_userobj.DeleteUserDapper(id));
            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);
                return BadRequest("Exception occurred");

            }
        }

        [HttpPost("InsertUserDapper")]
        public IActionResult InsertUserDapper([FromBody]User user)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    var result = _userobj.InsertUserDapper(user);
                    if (result !=null)
                    {
                        user.password = null;
                        return Ok(new { isError = false, user = user, Message = "Insert Successfull!" });
                    }
                    else
                    {
                        return BadRequest(new { isError = true, User = user, Message = "Insert Failed!" });
                    }
                //}
                //else
                //{
                //    user = null;
                //    return BadRequest(new { isError = true, user = user, Message = "Null Object Returned." });
                //}
            }
            catch (Exception ex)
            {
                Log.LogException(ex.StackTrace);
                return   BadRequest("Exception occurred");
            }
        }

        [HttpPut("UpdateUserDapper")]
        public IActionResult UpdateUserDapper(User user)
        {
            try
            {
                int result = _userobj.UpdateUserDapper(user);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        #endregion 
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
