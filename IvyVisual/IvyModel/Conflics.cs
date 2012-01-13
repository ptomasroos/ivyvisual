using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("conflicts")]
    public class Conflics
    {
        [XmlElement("manager")]
        public List<Manager> ManagerList { get; set; }
    }
}
