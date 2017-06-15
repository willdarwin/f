using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FPXApplicationInterface.Interface;

namespace FPXAppDesign.DesignerClass
{
    public class UserInfo
    {
        #region attribute

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public long Id { get; set; }

        #endregion

        #region nodes
        #endregion

        #region constructor
        public UserInfo() 
        {
            Name = string.Empty;
            Id = 0;
        }
        #endregion
    }
}
