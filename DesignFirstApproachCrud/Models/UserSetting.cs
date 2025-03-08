using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignFirstApproachCrud.Models
{
    public class UserSetting
    {
        public string FirstName {  get; set; }  
        public string LastName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserId { get; internal set; }
        public string MobileNo { get; internal set; }
        public string ConfirmPassword { get; internal set; }
    }
}