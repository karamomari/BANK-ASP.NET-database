using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Model;
using Bank.DTO;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BankContext context;
       public LoginController(BankContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] Tolog log)
        {

            //To custmer
            if (log.type == "Custmer")
            {
                var a = context.Custmers.Where(op => op.Email == log.email && op.Pass == log.pass).FirstOrDefault();
                if (a != null)
                {
                    if (a.Enable == "E")
                    {
                        int id = a.CustmerId;
                        return Ok(id);



                    }
               
                    else
                    {
                      //return disaple
                       return Ok(new { message = "your account not Enable" });
                    }



                }
                else
                {
                    return Ok(new { message = "your email or pass in correct" });

                }

            }
            //To employee
            else if(log.type == "Employee")
            {
                string joptype = log.type;
                var a1 = context.Employees.Where(op => op.Email == log.email && op.Pass == log.pass&&op.Type== "Employee").FirstOrDefault();
                if (a1 != null)
                {
                    if (a1.Enable == "E")
                    {
                     
                            int id = a1.Id;
                            return Ok(id);

                        
                       
                    }
                    else
                    {
                        //returen to page not faund
                    return Ok(new { message = "your account not Enable" });

                    }
                }

                //return disaple
                return Ok(new { message = "your email or pass in correct" });


            }


            //To admin
            else
            {
                if (log.type == "Admin")
                {
                    var a = context.Employees.Where(op => op.Email == log.email && op.Pass == log.pass&&op.Type=="Admin").FirstOrDefault();
                    
                        if (a != null)
                        {
                            int id = a.Id;
                            return Ok(id);
                        }
                        else
                        {
                        return Ok(new { message = "your email or pass in correct" });

                        }

                }

                //your hukar
                else
                {
                    //return Ok("please out the web");
                    return Ok(new { message = "please out the web" });

                }
            }


        }



        [HttpPost]
        [Route("Register")]
        public IActionResult regitser([FromBody]modelcustmer cust)
        {
            var ema = context.Custmers.Where(op => op.Email == cust.Email).FirstOrDefault();
            var ssn = context.Custmers.Where(op => op.Ssn == cust.Ssn).FirstOrDefault();
            if (ema != null)
            {
                //returen your is exit
                return Ok(new { message = "This email already exists" });

            }
            if (ssn != null)
            {
                return Ok(new { message = "This ssn already exists" });

            }

            var cus = new Custmer
            {
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Ssn = cust.Ssn,
                City = cust.City,
                Street = cust.Street,
                Age = cust.Age,
                Email = cust.Email,
                Pass = cust.Pass,
                Enable = "D",
                CreditPoint = 0.0,
              
            };
            context.Custmers.Add(cus);
            context.SaveChanges();
            return Ok();
        }





        [HttpPut]
        [Route("remberPass")]
        public IActionResult rember(string Email,string ssn,string newpass)
        {
           var t= context.Custmers.Where(op => op.Email == Email && op.Ssn == ssn).FirstOrDefault();
            if (t != null && t.NumForPas<=3)
            {
                return Ok();
                t.Pass = newpass;
                t.NumForPas += 1;
                context.Custmers.Update(t);
                context.SaveChanges();
            }
            return NotFound();
        }
    
    
    }



}
