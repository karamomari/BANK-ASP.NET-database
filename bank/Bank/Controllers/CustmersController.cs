using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Microsoft.EntityFrameworkCore;
using Bank.DTO;
using RestSharp;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustmersController : ControllerBase
    {
        static int idd;
        private readonly BankContext context;
        public CustmersController(BankContext context)
        {
            this.context = context;
        }




        [HttpGet]
        [Route("page")]
        public IActionResult page(int id)
        {
            idd = id;
            var cus = context.Custmers.Where(op => op.CustmerId == id).ToList();
            return Ok(cus);
        }









        [HttpGet]
        [Route("Cridt")]
        public IActionResult cridt()
        {
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            cus.CreditPoint += 0.5;
            context.SaveChanges();
            return Ok();
        }






        [HttpGet]
        [Route("stopAtm")]
        public IActionResult stopAtm()
        {
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            var acc = context.Accounts.Where(op => op.AccountId == cus.AccountId).FirstOrDefault();
            var atm = context.Atms.Where(op => op.AccountId == acc.AccountId).FirstOrDefault();
            if (atm!=null)
            {
                if (atm.Enable == "D") { return Ok(new { message = "Your credit card is already suspended" }); }
                else
                {
                    atm.Enable = "D";
                    context.Atms.Update(atm);
                    context.SaveChanges();
                     return Ok(new { message = "Your card has been suspended" }); 

                }
            }
            else
            {
                return Ok(new { message = "You do not have a credit card" });
            }
        }



        [HttpGet]
        [Route("activeAtm")]
        public IActionResult activeAtm()
        {
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            var acc = context.Accounts.Where(op => op.AccountId == cus.AccountId).FirstOrDefault();
            var atm = context.Atms.Where(op => op.AccountId == acc.AccountId).FirstOrDefault();
            if (atm != null)
            {
                if (atm.Enable == "E") { return Ok(new { message = "Your credit card is already active" }); }
                else
                {
                    atm.Enable = "E";
                    context.Atms.Update(atm);
                    context.SaveChanges();
                     return Ok(new { message = "Your card has been activated" });

                }
            }
            else
            {
                return Ok(new { message = "You do not have a credit card" });

            }
        }


        [HttpGet]
        [Route("inquiry")]
        public IActionResult inquiry(string mas)
        {
            Help help = new Help();
            help.CustmerId = idd;
            help.Message = mas;
            context.Helps.Add(help);
            context.SaveChanges();
            return Ok();
        }





        [HttpGet]
        [Route("allinfo")]
        public IActionResult allinfo(){
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            var acc = context.Accounts.Where(op => op.AccountId == cus.AccountId).FirstOrDefault();
            var loan = context.Loans.Where(op => op.AccountId == acc.AccountId).FirstOrDefault();
            allinfoTOcus allinfoT = new allinfoTOcus();
            if (loan!= null)
            {
                allinfoT.Amountloan = loan.Amount;
            }
            if (acc !=null)
            {
                allinfoT.Balance = acc.Balance;
                allinfoT.UserName = acc.UserName;
                allinfoT.CreditPoint = acc.CreditPoint;
                allinfoT.TypeAccount = acc.TypeAccount;
            }

            allinfoT.FirstName = cus.FirstName;
            allinfoT.LastName = cus.LastName;
            allinfoT.City = cus.City;
            allinfoT.Street = cus.Street;
            allinfoT.Ssn = cus.Ssn;
            return Ok(allinfoT);
        }


        [HttpPost]
        [Route("tran")]
        public IActionResult transaction(transinfo transaction)
        
        {
            //DepositeAccount الي بدي احول عليه
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            int a =(int)cus.AccountId;
            var acc = context.Accounts.Where(op => op.AccountId == a).FirstOrDefault();
            var toacc = context.Accounts.Where(op => op.AccountId == transaction.DepositeAccount).FirstOrDefault();
            var tocust = context.Custmers.Where(op => op.AccountId == transaction.DepositeAccount).FirstOrDefault();
            if (toacc == null)
            {
                return Ok(new { message = "id account dont valid" });

            }

            else if (cus.AccountId == tocust.AccountId) { return Ok(new { message = "Sorry, you cannot transfer funds to the same account" }); }
            else
            {
                if (tocust.Enable != "E")
                {
                  
                    return Ok(new { message = "You are trying to deposit to an unapproved account" });

                }

                else
                {
                    if (acc.Balance < transaction.ammount)
                    {
                        return Ok(new { message = "you dont have balance" });


                    }

                    else
                    {
                        acc.Balance = acc.Balance - transaction.ammount;
                        toacc.Balance = toacc.Balance + transaction.ammount;
                        cus.CreditPoint += 1;
                        Process process = new Process();
                        process.Tyoe = transaction.Type;
                        process.Date = DateTime.Now;
                        process.AccountId = acc.AccountId;
                        context.Processes.Add(process);
                        tocust.CreditPoint += 0.5;
                        Process process1 = new Process();
                        process1.Tyoe = "Deposit";
                        process1.Date = DateTime.Now;
                        process1.AccountId = toacc.AccountId;
                        context.Processes.Add(process1);
                        
                        context.SaveChanges();
                        return Ok(new { message = "The transaction was successful" });


                    }

                }
            }
          
               
            
        }




        //بده تست

        [HttpPost]
        [Route("Getloan")]
        public IActionResult Getloan([FromBody]int ammount)
        {

           // var cust = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            var cust = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();


            switch (ammount)
            {

                 //Case one
                case int q when q > 0 && q < 3000:
                    {
                        if (cust.CreditPoint >= 10)
                        {
                            Loan loan = new Loan();
                            loan.Enable = "D";
                            loan.Amount = ammount;
                            loan.CustmerId = cust.CustmerId;
                            loan.AccountId = cust.AccountId;
                            //.loan.LoanId = 1;
                            context.Loans.Add(loan);
                            context.SaveChanges();
                        return Ok(new { message = "success please Wait for approval" });

                        }

                        else
                        {
                        return Ok(new { message = "your point lest" });


                        }


                    }



                //Case tow
                case int q when q > 3000 && q < 10000:
                    if (cust.CreditPoint >= 50)
                    {
                        Loan loan = new Loan();
                        loan.Enable = "D";
                        loan.Amount = ammount;
                        loan.CustmerId = cust.CustmerId;
                        loan.AccountId = cust.AccountId;
                        context.Loans.Add(loan);
                        context.SaveChanges();
                        return Ok(new { message = "success please Wait for approval" });

                    }

                    else
                    {
                        return Ok(new { message = "your point lest" });
                    
                    }




                //Antouer case 
                default:

                    if (cust.CreditPoint >= 100)
                    {
                        Loan loan = new Loan();
                        loan.Enable = "D";
                        loan.Amount = ammount;
                        loan.CustmerId = cust.CustmerId;
                        loan.AccountId = cust.AccountId;
                        context.Loans.Add(loan);
                        context.SaveChanges();
                        return Ok(new { message = "success please Wait for approval" });

                    }

                    else
                    {
                        return Ok(new { message = "your point lest" });

                    }
            }

          
        }





        [HttpPut]
        [Route("updateinfo")]
        public IActionResult updateinfo([FromBody]SubCus subCus)
        {
            if (subCus == null)
            {
                return Ok(new { message = "you dont send any info to update" });

            }
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            var cusphone = context.CustmerPhones.Where(op => op.CustmerId == idd).FirstOrDefault();
            if (subCus.Pass != null) { cus.Pass = subCus.Pass; }     
            if (subCus.City != null) { cus.City = subCus.City; }
            if (subCus.Street != null) { cus.Street = subCus.Street; }
            context.SaveChanges();
            return Ok(new { message = "The uodate successful" });
        }











        [HttpDelete]
        [Route("deleteacount")]
        public IActionResult deleteaccount()
        {
            var loan = context.Loans.Where(op => op.CustmerId == idd).FirstOrDefault();
            if (loan != null && loan.Enable == "E")
            {
                return Ok(new { message = "Sorry, we cannot delete your account due to an outstanding loan. Please contact the bank for further details." });
            }
            var cus = context.Custmers.Where(op => op.CustmerId == idd).FirstOrDefault();
            var acc = context.Accounts.Where(op => op.AccountId == cus.AccountId).FirstOrDefault();
            var atm = context.Atms.Where(op => op.AccountId == acc.AccountId).FirstOrDefault();
            var inq = context.Helps.Where(op => op.CustmerId == cus.CustmerId).FirstOrDefault();
            var pho = context.CustmerPhones.Where(op => op.CustmerId == idd).FirstOrDefault();
            var proo = context.Processes.Where(op => op.AccountId == acc.AccountId).ToList();
            var mas = context.CustomerMessages.Where(op => op.CustomerId == idd).FirstOrDefault();
            if (inq != null) { context.Helps.Remove(inq); }
            if (atm != null) { context.Atms.Remove(atm); }
            if (pho != null) { context.CustmerPhones.Remove(pho); }
            if (loan != null) { context.Loans.Remove(loan); }
            if (proo != null)
            {
                foreach (var p in proo)
                {
                    context.Processes.Remove(p);
                }
            }
            if (mas != null) { context.CustomerMessages.Remove(mas); }
            context.Accounts.Remove(acc);
            context.Custmers.Remove(cus);
            context.SaveChanges();
            return Ok(new { message = "The account has been successfully deleted" });
        }
























    }



}
