using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FPXApplicationInterface.Interface;

namespace FPXAppDesign.DesignerClass
{
    public class Header : IHeader
    {
        #region attribute
        [XmlAttribute]
        public int Height { get; set; }

        #endregion

        #region element
       
        #endregion

        #region constructor
        public Header()
        {
        }
        #endregion
    }
}
