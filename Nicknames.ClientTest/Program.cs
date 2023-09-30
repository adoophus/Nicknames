using Nicknames.ClientTest;
using Steamworks;
using Nicknames.Shared.Entities;

// Connect to the Steam API. Client needs to be logged into steam.
// Requires owning Bannerlord.
SteamAPI.Init();

// Steam API will send the client callbacks for any methods
// we have invoked and we need to tick their method in order
// to receive them.
new Thread(() =>
{
    while (true)
    {
        SteamAPI.RunCallbacks();
        Thread.Sleep(50);
    }
}).Start();

string nickname = String.Empty;

APIManager.OnLoginEvent += async () => {
    nickname = await APIManager.GetNickname();
    await RunCommandLoop();
};

// Testing for the API.
if (APIManager.HasStoredToken())
{
    await APIManager.Login(token: APIManager.GetToken());
}
else
{
    APIManager.GenerateSteamTicket();
}


async Task RunCommandLoop()
{
    while (true)
    {
        Console.WriteLine(">");
        string command = Console.ReadLine();


        if (command == "set")
        {
            Console.WriteLine("Enter the new nickname:");

            string nicknameToUpdate = Console.ReadLine();
            bool updated = await APIManager.SetNickname(nicknameToUpdate);
   
        }
        else if (command == "get")
        {
            nickname = await APIManager.GetNickname();

            Console.WriteLine("Nickname: " + nickname);
        }
        else if (command == "exit")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid command. Please enter 'get' or 'set'");
        }
    }
}
