using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sellopedia.Models
{
    public class EnumerationsClass
    {
        public enum AccountType
        {
            Particular,
            Organisation
        }

        public enum Gender
        {
            Male,
            Female
        }

        public enum MessageState
        {
            Pending,
            Seen
        }
    }
}