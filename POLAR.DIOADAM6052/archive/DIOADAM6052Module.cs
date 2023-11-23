using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.DIOADAM6052
{
    public class DIOADAM6052Module : IModule
    {
        IAdam6052Module1 _adam6052Module1;
        IAdam6052Module2 _adam6052Module2;
        public DIOADAM6052Module(IAdam6052Module1 adam6052Module1, IAdam6052Module2 adam6052Module2)
        {
            _adam6052Module1 = adam6052Module1;
            _adam6052Module2 = adam6052Module2;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
