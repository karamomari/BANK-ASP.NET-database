using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Microsoft.EntityFrameworkCore;
namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VistController : ControllerBase
    {
     private readonly  BankContext context;
     public VistController(BankContext context)
        {
            this.context = context;
        }






        [HttpGet]
        [Route("GetAllBranch")]
        public async Task<ActionResult<IEnumerable<Branch>>> getAllBranch()
        {
            
            var Branch= await context.Branches.ToListAsync();
            return Ok(Branch);
        }



     
    }

}
