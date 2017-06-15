using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebUIComponent.Models
{
    public class InputModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public LabelModel  Label { get; set; }
        public TextModel Text { get; set; }
        public ButtonModel Button { get; set; }
    }
}