using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCWebUIComponent.Models
{
    public class AppModel
    {
        public int Id { get; set; }

        public string AppName { get; set; }

        public string AppFileNameWithExtension { get; set; }

        [Required(ErrorMessage = "User Name is required!")]
        public string User { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Psd { get; set; }

        public List<IdeaModel> IdeaList { get; set; } 

        public List<AppUserModel> UserList { get; set; }

        public int UserId { get; set; }
    }

    public class AppUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}