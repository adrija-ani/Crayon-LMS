using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}
