using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntichamberSaveWatcher
{
    class Secret
    {
        public string FullName { get; private set; }

        public Secret(string name)
        {
            FullName = name;
        }
    }
}
