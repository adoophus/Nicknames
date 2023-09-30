using Nicknames.Module.Client;
using Nicknames.Module.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Nicknames.Module
{
    public class SubModule : MBSubModuleBase
    {
        public SubModule() { }

        private bool _isServer = false;

        private bool _startSuccessful;

        private IModuleState _moduleState;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _ = Start();
        }

        public async Task Start()
        {
            _startSuccessful = await Initialise();
        }

        public async Task<bool> Initialise()
        {
            try
            {
                if (_isServer)
                {
                    _moduleState = new Server.ModuleState();
                } 
                else
                {
                    _moduleState = new Client.ModuleState();
                }
         
                try
                {
                    await _moduleState.GetPatcher().OnPatch();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }

                await _moduleState.Load();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
