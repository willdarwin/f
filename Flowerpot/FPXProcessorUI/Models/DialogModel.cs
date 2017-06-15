using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPXProcessorUI.Models
{
    public class DialogModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsModal { get; set; }
        public DialogButtonType DialogType { get; set; }
    }

    public enum DialogButtonType
    { 
        OKCancel = 0,
        YesNo = 1
    }
}