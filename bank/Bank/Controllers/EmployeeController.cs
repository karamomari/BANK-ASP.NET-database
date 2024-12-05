using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Controllers;
using Microsoft.OpenApi.Any;
using Bank.DTO;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Bank.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestSharp;
using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        static int emp_id, adm_id;
        private readonly BankContext context;

        public EmployeeController(BankContext context)
        { this.context = context;
        }

        [HttpGet]
        [Route("employee")]
        public IActionResult Employee(int id)
        {
            emp_id = id;
            var t = context.Employees.Where(op => op.Id == id).ToList();
            return Ok(t);
        }






        [HttpPost]
        [Route("regcust1")]
        public IActionResult registercus([FromBody] modelcustmer cust)
        {



            if (cust != null)
            {
                var cusss=context.Custmers.Where(op=>op.Email == cust.Email).FirstOrDefault();
                var cusss2= context.Custmers.Where(op => op.Ssn == cust.Ssn).FirstOrDefault();
                if (cusss == null && cusss2==null)
                {
                    Account account = new Account();
                    context.Add(account);
                    context.SaveChanges();
                    int id = account.AccountId;
                    Atm atm = new Atm();
                    atm.AccountId = id;
                    context.Add(atm);
                    context.SaveChanges();

                    //  string muupload = Path.Combine(hosting.WebRootPath, "Image/Custmer image");
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
                        Enable = "E",
                        CreditPoint = 0.0,
                        AccountId = id

                    };
              
                //if (cust.image != null) {
                //    var fileName = Guid.NewGuid().ToString() + "-" + cust.image.FileName;
                //    var filePath = Path.Combine("Image/Custmer_image", fileName);
                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        cust.image.CopyTo(stream);
                //    }
                //    cus.Photo = fileName;
                //}
              
                
                
                context.Custmers.Add(cus);
                context.SaveChanges();
                //لازمة يرجع ع صفحه الاكاونت
                return Ok(id);
                }
                else
                {
                    return Ok(new { message = "this Email aredey exites" });

                }
            }
            return Ok(new { message = "you dont have information" });


        }



        [HttpGet]
        [Route("viewCus")]
        public IActionResult viewEmp(string ssn)
        {
            var cus = context.Custmers.Where(op => op.Ssn == ssn).FirstOrDefault();
            var emo = context.Employees.Where(op => op.Id == emp_id).FirstOrDefault();
            if (cus.Email == emo.Email){ return Ok(new { message = "this your email" }); }
            if (cus != null)
            {
                return Ok(cus);
            }
            else
                return Ok(new { message = "dont faund Custmer that has this ssn" });

        }



     



        [HttpPatch]
        [Route("updateCus/id")]
        public IActionResult updateCus(int id, [FromBody] SubCus cus)
        {
            var cuss = context.Custmers.Where(op => op.CustmerId == id).FirstOrDefault();
            if (cuss != null)
            {
                // Assuming SubCus has properties that match those in Customer
                if (!string.IsNullOrEmpty(cus.FirstName))
                {
                    cuss.FirstName = cus.FirstName;
                }
                if (!string.IsNullOrEmpty(cus.LastName))
                {
                    cuss.LastName = cus.LastName;
                }
                if (!string.IsNullOrEmpty(cus.City))
                {
                    cuss.City = cus.City;
                }
                if (!string.IsNullOrEmpty(cus.Street))
                {
                    cuss.Street = cus.Street;
                }
                if (!string.IsNullOrEmpty(cus.Ssn))
                {
                    cuss.Ssn = cus.Ssn;
                }
                if (!string.IsNullOrEmpty((cus.CreditPoint).ToString()))
                {
                    cuss.CreditPoint = cus.CreditPoint;
                }
                if (!string.IsNullOrEmpty(cus.Email))
                {
                    cuss.Email = cus.Email;
                }
                if (!string.IsNullOrEmpty(cus.Pass))
                {
                    cuss.Pass = cus.Pass;
                }
                context.SaveChanges();
                return Ok(cuss);
            }
            return Ok(new { message = "errore please agin" });

        }

        //تعطيل عميل
        [HttpPut]
        [Route("suspendingCus")]
        public IActionResult suspendingCus(int id)
        {
            var cus = context.Custmers.Where(op => op.CustmerId == id).FirstOrDefault();
            if (cus.Enable == "D") { return Ok(new { message = "this Employee alredy suspending" }); }
            cus.Enable = "D";
            context.Custmers.Update(cus);
            context.SaveChanges();
            return Ok();
        }

        // عميل تفعيل
        [HttpPut]
        [Route("activationCus")]
        public IActionResult activationCus(int id)
        {
            var cus = context.Custmers.Where(op => op.CustmerId == id).FirstOrDefault();
            if (cus.Enable == "E") { return Ok(new { message = "this Employee alredy activation" }); }
            cus.Enable = "E";
            context.Custmers.Update(cus);
            context.SaveChanges();
            return Ok();
        }



        [HttpGet]
        [Route("viewEmail")]
        public IActionResult viewEmail()
        {
            var help = context.Helps.ToList();
            if (help != null)
            {
                return Ok(help);
            }
            return Ok(new { message = "There are no inquiries" });

        }


        [HttpGet]
        [Route("viewoneEmail")]
        public IActionResult viewoneEmail(int id)
        {
            var em = context.Helps.Where(op => op.HlepNum == id).FirstOrDefault();
            return Ok(em);
        }


        //الرد على مسج
        [HttpDelete]
        [Route("Reply/id")]
        public IActionResult ReplyEmail(int id ,string msg)
        {
            var help = context.Helps.Where(op => op.HlepNum == id).FirstOrDefault();
            CustomerMessage customerMessage = new CustomerMessage();
            customerMessage.CustomerId = help.CustmerId;
            customerMessage.Message = msg;
            context.CustomerMessages.Add(customerMessage);
            context.Helps.Remove(help);
            context.SaveChanges();
            return Ok();
        }


         

        [HttpPost]
        [Route("UplaodImage")]
        public Custmer Uplaodimage()
        {
            var file = Request.Form.Files[0];
            var fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
            var filePath = Path.Combine("C:\\Users\\karam\\Desktop\\bank\\Bank\\Image\\Custmer_image", fileName);
        
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Custmer custmer = new Custmer();
            custmer.Photo = fileName;
            return custmer;
        }




        [HttpGet]
        [Route("viewprocess")]
        public IActionResult viewprocess(int id)
        {
            var cus = context.Custmers.Where(op => op.CustmerId == id).FirstOrDefault();
            var pro = context.Processes.Where(op => op.AccountId == cus.AccountId).ToList();
            if (pro.Count>0 && pro != null) { return Ok(pro); }
            
            else
            {  return Ok(new { message = "No transactions have been made on this account" }); }


         }



        [HttpPut]
        [Route("stopAtm")]
        public IActionResult stopAtm(int id)
        {
            var cus = context.Custmers.Where(op => op.CustmerId == id).FirstOrDefault();
            var acc = context.Accounts.Where(op => op.AccountId == cus.AccountId).FirstOrDefault();
            if (acc != null)
            {
                var atm = context.Atms.Where(op => op.AccountId == acc.AccountId).FirstOrDefault();

                if (atm != null)
                {
                    atm.Enable = "D";
                    context.Atms.Update(atm);
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    { return Ok(new { message = "This Custmer dont have ATM" }); }

                }
            }
            else
            {
                { return Ok(new { message = "The account is still inactive" }); }


            }



        }








        //[HttpPost]
        //[Route("hh")]
        //public IActionResult hh([FromBody] string em)
        //{

        //    var client = new RestClient("https://sandbox.api.mailtrap.io/api/send/3102225");
        //    var request = new RestRequest();
        //    request.AddHeader("Authorization", "Bearer 57e98674a03783685727765cd2f5da9f");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("application/json", "{\"from\":{\"email\":\"mailtrap@example.com\",\"name\":\"Mailtrap Test\"},\"to\":[{\"email\":\"karamomari20112001@gmail.com\"}],\"subject\":\"You are awesome!\",\"text\":\"Congrats for sending test email with Mailtrap!\",\"category\":\"Integration Test\"}", ParameterType.RequestBody);
        //    var response = client.Post(request);
        //    return Ok();
        //}









        //[HttpPost]
        //[Route("hh")]
        //public IActionResult hh([FromBody] string em)
        //{
        //    var client = new RestClient("https://sandbox.api.mailtrap.io/api/send/3102225");
        //    var request = new RestRequest();
        //    request.AddHeader("Authorization", "Bearer 57e98674a03783685727765cd2f5da9f");
        //    request.AddHeader("Content-Type", "application/json");

        //    var body = new
        //    {
        //        from = new { email = "karamomari20010@gmail.com", name = "Mailtrap Test" },
        //        to = new[] { new { email = em } },
        //        subject = "You are awesome!",
        //        text = "Congrats for sending test email with Mailtrap!",
        //        category = "Integration Test"
        //    };

        //    request.AddJsonBody(body);
        //    var response = client.Post(request);
        //    return Ok(response);
        //}

      







            //بدي ال الاي دي تبع الاكاونت من فوق مرجعه
            [HttpPut]
        [Route("complereg/id")]
        public IActionResult complereg(int id,[FromBody]Acctoadd account)
        {
            var emppp = context.Employees.Where(op => op.Id == emp_id).FirstOrDefault();
            var acc = context.Accounts.Where(op => op.AccountId == id).FirstOrDefault();
            acc.Balance = account.Balance;
            acc.UserName=account.UserName;
            acc.Password=account.Password;
            acc.TypeAccount = account.TypeAccount;
            acc.BranchId = emppp.BranchId;
            //acc.BranchId = 1;
            acc.CreditPoint = 1;
            context.Accounts.Update(acc);
            context.SaveChanges();
            return Ok();
        }






        [HttpGet]
        [Route("viewNewCus")]
        public IActionResult stopAtm()
        {
            var newCus = context.Custmers.Where(op => op.Enable == "D").ToList();
            if (newCus != null) { return Ok(newCus); }
            else return Ok(new { message = "There are no new accounts" });

        }



        //رساله للعميل تم تفعيل الحساب
        [HttpPut]
        [Route("consentCus")]
        public IActionResult consentCus(int id)
        {

            Account account = new Account();
            context.Add(account);
            context.SaveChanges();
            int idAcc = account.AccountId;
            Atm atm = new Atm();
            context.Add(atm);
            context.SaveChanges();
            atm.AccountId = idAcc;
            var a = context.Custmers.Where(op => op.CustmerId == id).FirstOrDefault();
            a.Enable = "E";
            a.AccountId = idAcc;
            context.Custmers.Update(a);
            context.SaveChanges();
            return Ok();
        }






        //[HttpPut]
        //[Route("consentCus")]
        //public async Task<IActionResult> consentCus(int id)
        //{
        //    Account account = new Account();
        //    await context.AddAsync(account);
        //    await context.SaveChangesAsync();
        //    int idAcc = account.AccountId;

        //    Atm atm = new Atm();
        //    atm.AccountId = idAcc;
        //    await context.AddAsync(atm);
        //    await context.SaveChangesAsync();

        //    var a = await context.Custmers.Where(op => op.CustmerId == id).FirstOrDefaultAsync();
        //    if (a != null)
        //    {
        //        a.Enable = "E";
        //        a.AccountId = idAcc;
        //        context.Custmers.Update(a);
        //        await context.SaveChangesAsync();
        //    }

        //    return Ok();
        //}


        //to admin just


        [HttpGet]
        [Route("Admin")]
        public IActionResult Admin(int id)
        {
            adm_id = id;
            var admin = context.Employees.Where(op => op.Type == "Admin" && op.Id == id).ToList();
            return Ok(admin);
        }



        //القروض الجديده
        [HttpGet]
        [Route("Cheackloan")]
        public IActionResult cheakloan()
        {
              var a=  context.Loans.Where(op => op.Enable == "D").ToList();
            return Ok(a);

        }



        //عرض معومات القرض
        [HttpGet]
        [Route("viewdetalis")]
        public IActionResult viewdetailsloan(int id)
        {
            var a = context.Loans.Where(op => op.LoanId == id).FirstOrDefault();
            var b = context.Accounts.Where(op => op.AccountId == a.AccountId).FirstOrDefault();
            var c = context.Custmers.Where(op => op.AccountId == b.AccountId).FirstOrDefault();
            infoloan infoloan = new infoloan();
            infoloan.fname = c.FirstName;
            infoloan.lname = c.LastName;
            infoloan.ssn = c.Ssn;
            infoloan.age = (int)c.Age;
            infoloan.typeaccount = b.TypeAccount;
            infoloan.balance =(double)b.Balance;
            infoloan.point =(int)c.CreditPoint;
            infoloan.ammount = (int)a.Amount;
            infoloan.Email = c.Email;
            List<infoloan> listt = new List<infoloan>();
            listt.Add(infoloan);
            return Ok(listt);
            
        }



        //موافقه علر القرض
        [HttpPut]
        [Route("consentloan")]
        public IActionResult consent([FromBody]int id)
        {
            var a = context.Loans.Where(op => op.LoanId == id).FirstOrDefault();
            var b = context.Accounts.Where(op => op.AccountId == a.AccountId).FirstOrDefault();
            a.Enable = "E";
            b.Balance += a.Amount;
            context.Loans.Update(a);
            context.Accounts.Update(b);
            context.SaveChanges();
            return Ok();
        }



        //رفض قرض
        [HttpDelete]
        [Route("rejectloan")]
        public IActionResult reject([FromBody] int id)
        {
            var a = context.Loans.Where(op => op.LoanId == id).FirstOrDefault();
            context.Loans.Remove(a);
            context.SaveChanges();
            return Ok();
            //نبعث رساله انه نرفض
        }



        //عرض
        [HttpPost]
        [Route("viewEmp")]
        public IActionResult viewEmp([FromBody]subEmp empp)
        {
            var emp = context.Employees.Where(op => op.FirstName == empp.FirstName &&op.LastName==empp.LastName).FirstOrDefault();
            if (emp != null)
            {
                return Ok(emp);
            }
            else
             
         return Ok(new { message = "I'm sorry, I couldn't find any employee by that name" }); 

        }



        //تعديل
        [HttpPut]
        [Route("updateEmp")]
        public IActionResult updateEmp(int id,[FromBody]Employee employee)
        {
            var a = context.Employees.Where(op => op.Id == id).FirstOrDefault();
           
            a.BranchId = employee.BranchId;
            a.Salary = employee.Salary;
            a.Type = employee.Type;
            
            context.Employees.Update(a);
            context.SaveChanges();
            return Ok(a);
        }


        //تعطيل موضف
        [HttpPut]
        [Route("suspending")]
        public IActionResult suspendingEmp(int id)
        {
            var emp = context.Employees.Where(op => op.Id == id).FirstOrDefault();
            if (emp.Enable == "D") { return Ok(new { message = "this Employee alredy suspendingEmp" }); }
            emp.Enable = "D";
            context.Employees.Update(emp);
            context.SaveChanges();
            return Ok();
        }

        // موضف تفعيل
        [HttpPut]
        [Route("activation")]
        public IActionResult activation(int id)
        {
            var emp = context.Employees.Where(op => op.Id == id).FirstOrDefault();
            if (emp.Enable == "E") { return Ok(new { message = "this Employee alredy activation" }); }
            emp.Enable = "E";
            context.Employees.Update(emp);
            context.SaveChanges();
            return Ok();
        }



        //اضافه موضف
        [HttpPost]
        [Route("addEmp")]
        public IActionResult AddEmp([FromBody]subEmp subEmp)

        {
            var emp = context.Employees.Where(op => op.Email == subEmp.Email).FirstOrDefault();


            if (emp != null) {  return Ok(new { message = "this Email alredy exist" }); } 

            if (subEmp != null) {
                var ad=context.Employees.Where(op=>op.Id== subEmp.idadmin).FirstOrDefault();
            Employee employee = new Employee();
            employee.Enable = "E";
            employee.FirstName = subEmp.FirstName;
            employee.LastName = subEmp.LastName;
            employee.Salary = subEmp.Salary;
            employee.StartDate =DateTime.Now;
            employee.Type =subEmp.Type;
            employee.Email = subEmp.Email;
            employee.BranchId = ad.BranchId;
            Account account = new Account();
            context.Accounts.Add(account);

            context.SaveChanges();

            employee.AccountId = account.AccountId;
            context.Employees.Add(employee);
            context.SaveChanges();
             return Ok(new { message = "sucess" });
            }

            else  { return Ok(new { message = "please input all info" }); }

        }




        //ريبورت
        [HttpGet]
        [Route("reportemp/id")]
        public IActionResult reporetEmp(int id)
        {
            var admin = context.Employees.Where(op => op.BranchId == id).ToList();
          

            return Ok(admin);
        }

        //معلوات كل القروض
        [HttpGet]
        [Route("reportLoan")]
        public IActionResult reportLoan()
        {
            int onee = 0;
            int tow = 0;
            int three = 0;

            var one = context.Loans.ToList();
            foreach (var x in one)
            {
                if(x.Amount < 3000)
                {
                    onee += 1;
                }
                else if (x.Amount > 3000 && x.Amount < 10000)
                {
                    tow += 1;
                }
                else
                {
                    three++;
                }
            }

            List<int> loan = new List<int>();
            loan.Add(onee);
            loan.Add(tow);
            loan.Add(three);
            return Ok(loan);
        }





        //1
        //[HttpPatch]
        //[Route("jf/{id}")]
        //public IActionResult t([FromBody] JsonPatchDocument<Employee> employee, [FromRoute] int id)
        //{
            
        //    var c = context.Employees.Where(op=>op.Id==id).SingleOrDefault();
        //    employee.ApplyTo(c);
        //    context.SaveChanges();
        //    return Ok(c);
        //}

        ////2
        //[HttpPost]
        //[Route("ll")]
        //public IActionResult tt([FromForm] int id)
        //{

        //    var c = context.Custmers.Where(op => op.AccountId == id).FirstOrDefault();
        //    var m = context.Custmers.Where(op => op.CustmerId == id).Select(o=>o.Account.UserName).FirstOrDefault();
        //    if (m != null)
        //    {
          
        //        return Ok(m);
        //    }
        //    return Ok();
        //}


    }
}
