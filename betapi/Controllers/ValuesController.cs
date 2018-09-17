using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace betapi.Controllers
{
    [RoutePrefix("Bet")]
    public class ValuesController : ApiController
    {

        [HttpGet]
        [Route("GetUid")]
        public string GetUid()
        {
            clsBet betDAC = new clsBet();
            return betDAC.getUid();
        }

        // GET api/values
        [HttpGet]
        [Route("GetBetList")]
        public List<betObject> GetBetList(string type, int id)
        {
            clsBet betDAC = new clsBet();
            return betDAC.getBetList(type,id);
        }

        [HttpGet]
        [Route("GetBetListByBatch")]
        public List<betObject> GetBetListByBatch(string batchId)
        {
            clsBet betDAC = new clsBet();
            return betDAC.GetBetListByBatch(batchId);
        }

        [HttpGet]
        [Route("GetBetListById")]
        public betObject GetBetListById([FromUri] int id)
        {
            clsBet betDAC = new clsBet();
            return betDAC.getBetListById(id);
        }

        [HttpPost]
        [Route("LoginUser")]
        public int LoginUser([FromUri]  string userName, string pass)
        {
            int userId = 0;
            Login ac = new Login();
            userId = ac.loginUser(userName, pass);
            return userId;
        }

        [HttpPost]
        [Route("UpdateBet")]
        public string UpdateBet([FromUri] int id,int num, int up,int down)
        {
            clsBet betDAC = new clsBet();
             betDAC.update(id,num,up,down);        
            return "test post post";
        }

        [HttpPost]
        [Route("InsertBet")]
        public string InsertBet([FromBody] List<betObject> betitem)
        {
            clsBet betDAC = new clsBet();
            foreach (var itm in betitem)
            {
                betDAC.insert(itm);
            }

            return "test post post";
        }

        [HttpDelete]
        [Route("Delete")]
        public string Delete([FromUri] int id)
        {
            clsBet betDAC = new clsBet();
                betDAC.delete(id);


            return "test post post";
        }



    }
}
