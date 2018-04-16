using System;
using System.Collections.Generic;
using System.Text;

namespace BLink.Models.RequestModels.Members
{
    public class PlayerFilterResult
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }
    }
}
