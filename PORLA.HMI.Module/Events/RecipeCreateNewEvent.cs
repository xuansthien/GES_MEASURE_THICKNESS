﻿using PORLA.HMI.Module.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORLA.HMI.Module.Events
{
    public class RecipeCreateNewEvent : PubSubEvent<RecipeParameterModel>
    {
    }
}
