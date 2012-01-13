using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;

namespace IvyVisual
{
    public class Globals
    {
        public static DTE2 DTE { get; set; }
        public static AddIn Addin { get; set; }
    }
}
