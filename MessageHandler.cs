using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.Utils;
using System.Text;

[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
public class MessageHandler : MySessionComponentBase
{
    private const ushort MESSAGE_ID = 22345; // Unique message ID

    public override void LoadData()
    {
        MyAPIGateway.Utilities.MessageEntered += OnMessageEntered;
        MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(MESSAGE_ID, HandleMessage);
        MyAPIGateway.Utilities.ShowMessage("Nacho Bot", "Sniffing #IRC and taking commands");
    }

    protected override void UnloadData()
    {
        MyAPIGateway.Utilities.MessageEntered -= OnMessageEntered;
        MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(MESSAGE_ID, HandleMessage);
    }

    private void OnMessageEntered(string messageText, ref bool sendToOthers)
    {
        if (messageText.StartsWith("!"))
        {
            byte[] message = Encoding.UTF8.GetBytes(messageText);
            MyAPIGateway.Multiplayer.SendMessageToServer(MESSAGE_ID, message);

            sendToOthers = false; // Prevent the message from being sent to other players
        }
    }

    private void SendMessageToPlayer(ulong steamId, string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        MyAPIGateway.Multiplayer.SendMessageTo(MESSAGE_ID, messageBytes, steamId);
        MyAPIGateway.Utilities.ShowMessage("Nacho Bot", $"Message sent to {steamId}: {message}");
    }

    private void HandleMessage(ushort messageId, byte[] data, ulong senderSteamId, bool reliable)
    {
        string message = Encoding.UTF8.GetString(data);
        MyAPIGateway.Utilities.ShowMessage("Nacho Bot", $"{message}");
    }
}