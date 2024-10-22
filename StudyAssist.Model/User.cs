using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Model
{
    public class User
    {
        public int? UserId { get; set; }

        public required string UserName { get; set; }
    }
}
