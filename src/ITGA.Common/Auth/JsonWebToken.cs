using System;
using System.Collections.Generic;
using System.Text;

namespace ITGA.Common.Auth
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}
