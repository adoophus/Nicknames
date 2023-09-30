using Nicknames.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.Server
{
    public class ModuleState : IModuleState
    {
        Server.HarmonyPatcher _patcher;

        public async Task Load()
        {

        }

        IHarmonyPatcher IModuleState.GetPatcher() => _patcher;
    }
}
