using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Qrame.CoreFX.Helper
{
  public static class NetworkHelper
  {
    // 내부망으로 PrivacyAPIServer 서버에 접근할 수 있는지 확인
    public static bool IsIntranet(string IPAddress, int Port)
    {
      bool Result = false;

      try
      {
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
          socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);

          IAsyncResult asyncResult = socket.BeginConnect(IPAddress, Port, null, null);

          Result = asyncResult.AsyncWaitHandle.WaitOne(1000, true);
        }
      }
      catch
      {
        Result = false;
      }

      return Result;
    }

    public enum ServerType
    {
      PrivacyAPI,
      Monitoring,
      Document,
      WebSocket
    }

    // PrivacyAPIServer 서버에 Ping 확인
    public static bool IsPing(ServerType serveType)
    {
      string domainName = "";

      switch (serveType)
      {
        case ServerType.PrivacyAPI:
          domainName = "localhost";
          break;
        case ServerType.Monitoring:
          domainName = "localhost";
          break;
        case ServerType.Document:
          domainName = "localhost";
          break;
        case ServerType.WebSocket:
          //domainName = "1.1.2.61";
          domainName = "1.1.1.208";
          break;
      }

      bool Result = false;

      try
      {
        using (TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork))
        {
          Ping pingSender = new Ping();
          PingOptions options = new PingOptions();

          options.DontFragment = true;

          string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
          byte[] buffer = Encoding.ASCII.GetBytes(data);
          int timeout = 120;
          PingReply reply = pingSender.Send(domainName, timeout, buffer, options);

          Result = (reply.Status == IPStatus.Success);
        }
      }
      catch
      {
        Result = false;
      }

      return Result;
    }
  }
}
