using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignFirstApproachCrud.Models
{
	public class Game
	{
		public int GID {  get; set; }	
		public string GameName { get; set; }
		public string GameTypes {  get; set; }
		public string Video {  get; set; }	
	}
}