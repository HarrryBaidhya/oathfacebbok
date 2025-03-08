using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignFirstApproachCrud.Models
{
	public class AdminUser
	{
		public int AId { get; set; }
		public string UserName {  get; set; }	
		public string Password { get; set; }
        public string session_id { get; internal set; }
        public int Code { get; internal set; }
		public string Message { get; set; }	
    }
}