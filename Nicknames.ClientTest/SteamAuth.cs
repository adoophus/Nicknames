using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.ClientTest;

public static class SteamAuth
{
    public static HAuthTicket steamTicket;

    private static Callback<GetTicketForWebApiResponse_t> m_AuthTicketForWebApiResponseCallback;

    private static HAuthTicket m_AuthTicket;
    public static string m_SessionTicket;

    public delegate void AuthCallbackHandler(string sessionTicket);
    public static event AuthCallbackHandler OnAuthCallbackEvent;

    public static void GenerateSteamTicket()
    {
        m_AuthTicketForWebApiResponseCallback = Callback<GetTicketForWebApiResponse_t>.Create(OnAuthCallback);
        m_AuthTicketForWebApiResponseCallback.Register(OnAuthCallback);

        m_AuthTicket = SteamUser.GetAuthTicketForWebApi(null);
    }

    static void OnAuthCallback(GetTicketForWebApiResponse_t callback)
    {
        m_SessionTicket = BitConverter.ToString(callback.m_rgubTicket).Replace("-", string.Empty);
        OnAuthCallbackEvent?.Invoke(m_SessionTicket);
    }

    public static string GetSteamID()
    {
        return SteamUser.GetSteamID().m_SteamID.ToString();
    }

    public static string GetSteamName()
    {
        return SteamFriends.GetPersonaName();
    }
}
