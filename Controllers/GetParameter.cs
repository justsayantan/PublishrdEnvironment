using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishrdEnvironment.Controllers
{
    public class GetParameter
    {
        public string OrganizationalItemId { get; set; }

        public string stagingTcmId { get; set; }

        public string liveTcmId { get; set; }
    }
}
