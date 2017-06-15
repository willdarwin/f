using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCWebUIComponent.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessageResourceName = "UsernameValidate", ErrorMessageResourceType = typeof(Resources.Resources))]
        [LocalizedDisplayName("", ResourceName = "Username", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordValidate", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "PasswordToShort", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("", ResourceName = "Password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [LocalizedDisplayName("", ResourceName = "RememberMe", ResourceType = typeof(Resources.Resources))]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "UsernameValidate", ErrorMessageResourceType = typeof(Resources.Resources))]
        [LocalizedDisplayName("", ResourceName = "Username", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailValidate", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.EmailAddress)]
        [LocalizedDisplayName("", ResourceName = "EmailAddress", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordValidate", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(100,MinimumLength = 6, ErrorMessageResourceName = "PasswordToShort", ErrorMessageResourceType = typeof(Resources.Resources) )]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("", ResourceName = "Password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("", ResourceName = "ConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password",ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "InvitationCodeError", ErrorMessageResourceType = typeof(Resources.Resources))]
        [LocalizedDisplayName("", ResourceName = "InvitationCode", ResourceType = typeof(Resources.Resources))]
        public string InvitationCode { get; set; }
    }
    
    public class GenerateInvitationCodeModel
    {
        [Required(ErrorMessage = "Number can not be null")]
        public int Number { get; set; }
    }

    public class InvitationCodeModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool Obsolete { get; set; }

        public int UserId { get; set; }
    }

    public class CodesJqGridModel
    {
        public JQGrid CodesGrid { get; set; }

        public CodesJqGridModel()
        {
            CodesGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                                 {
                                     new JQGridColumn { DataField = "Id", 
                                                        // always set PrimaryKey for Add,Edit,Delete operations
                                                        // if not set, the first column will be assumed as primary key
                                                        PrimaryKey = true,
                                                        Editable = false,
                                                        Width = 50 },
                                     new JQGridColumn { DataField = "Value", 
                                                        Editable = false,
                                                        Width = 100 },
                                     new JQGridColumn { DataField = "Obsolete", 
                                                        Editable = false,
                                                        Width = 100 },
                                     new JQGridColumn { DataField = "UserId", 
                                                        Editable = false,
                                                        Width = 75 }
                                 },
                Width = Unit.Pixel(800)
            };

            CodesGrid.ToolBarSettings.ShowRefreshButton = true;
        }
    }

    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsDeleted { get; set; }
    }
}
