using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class UDPServer
{
static void Main()
{
UdpClient server = new UdpClient(9000);
Console.WriteLine("UDP Server started...");
IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
byte[] data = server.Receive(ref clientEP);
int clientValue = int.Parse(Encoding.ASCII.GetString(data));
Console.WriteLine($"Received value from client:
{clientValue}");
Console.Write("Enter your integer value (server side): ");
int serverValue = int.Parse(Console.ReadLine());
int sum = clientValue + serverValue;
Console.WriteLine($"Sum = {sum}");
byte[] sendData = Encoding.ASCII.GetBytes(sum.ToString());
server.Send(sendData, sendData.Length, clientEP);
Console.WriteLine("Sum sent back to client.");
server.Close();
} }