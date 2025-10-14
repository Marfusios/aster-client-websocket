namespace Aster.Client.Websocket.Signing
{
    /// <summary>
    /// Interface for signing payloads.
    /// </summary>
    public interface IAsterSignatureService
    {
        string Sign(string payload);
    }
}