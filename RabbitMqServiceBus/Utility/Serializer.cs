
using Newtonsoft.Json;

namespace RabbitMqServiceBus.Utility
{
    
    public class Serializer 
    {

        public static string SerializeObject(object obj)
        {
          string strObject =  JsonConvert.SerializeObject(obj);
           return strObject;
            
        }

        public static object DeserializeObject<T>(string objString)
        {
            return JsonConvert.DeserializeObject<T>(objString);
        }

      }
   

}
