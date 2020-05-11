using ZanP.OrderBooks.Models.Data;
using Newtonsoft.Json;

namespace ZanP.OrderBooks.Handlers
{
    public class ExchangeHandler
    {
        public static Exchange LoadFromJson(string p_json)
        {
            return JsonConvert.DeserializeObject<Exchange>(p_json);
        }
    }
}