using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module
{
    interface IHarmonyPatcher
    {
        Task OnPatch();
    }
}
