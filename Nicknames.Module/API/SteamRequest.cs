using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.API
{
    public class SteamRequest
    {
        // This is the the buffer for the steam ticket's bytes.
        // We don't know the actual length, so set it high.
        public byte[] buffer = new byte[1024];

        // The steam api request will tell us how many bytes
        // we have received into this.
        public uint recvLength;

        // todo: needs to be made asynchronous
        public async Task<string> GenerateSteamTicket()
        {
            SteamUser.GetAuthSessionTicket(buffer, 1024, out recvLength);

            // The default callback with the SteamWorks api is inconsistant.
            // Just run our own loop on this thread until the length of 
            // the array has been given, which symbolises that
            // the bytes array is ready to be checked.

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (recvLength == 0)
            {
                // Timeout time. Default: 10000ms
                // todo: retry on fail
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    return String.Empty;
                }
            }

            stopwatch.Stop();

            // The ticket is guaranteed to be populated if this callback
            // is being executed.
            byte[] ticketBytes = new byte[recvLength];

            // Copy from the buffer into the new byte array now that we know
            // what to exclude given the received length.
            Buffer.BlockCopy(buffer, 0, ticketBytes, 0, (int)recvLength);

            // Conversion from decimal to hexadecimal.
            return BitConverter.ToString(ticketBytes).Replace("-", "");
        }

        public string GetSteamID()
        {
            return SteamUser.GetSteamID().m_SteamID.ToString();
        }

        public string GetSteamName()
        {
            return SteamFriends.GetPersonaName();
        }
    }

}
