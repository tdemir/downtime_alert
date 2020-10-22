using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Helpers
{
    public class JsonPascalCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name;
        }
    }
}
