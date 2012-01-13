using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("description")]
    public class Description
    {
        [XmlAttribute("homepage")]
        public string Homepage { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
