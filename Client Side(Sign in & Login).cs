using System;
using System.Net.Sockets;
using System.Text;
using System.IO;

class AuthClient
{
    static void Main()
    {
        // Define server address and port
        // IMPORTANT: If running on a different PC, change "127.0.0.1" to the actual
        // IP address of the machine running the AuthServer.cs application.
        const string ServerIp = "127.0.0.1";
        const int Port = 5000;
        for (int i = 0; i < 10; i++)
        {
            try
            {
                string operation = "";

                // 1. Get user choice (Sign Up or Login)
                Console.WriteLine("--- Authentication Client ---");
                Console.WriteLine("Please choose an action:");
                Console.WriteLine("1. Sign Up (Register a new user)");
                Console.WriteLine("2. Login (Authenticate an existing user)");
                Console.Write("Enter choice (1 or 2): ");
                string choice = Console.ReadLine()?.Trim();

                if (choice == "1")
                {
                    operation = "SIGNUP";
                }
                else if (choice == "2")
                {
                    operation = "LOGIN";
                }
                else
                {
                    Console.WriteLine("Invalid choice. Exiting client.");
                    return;
                }

                // 2. Get credentials
                Console.Write("Enter Username: ");
                string user = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Enter Password: ");
                string pass = Console.ReadLine()?.Trim() ?? "";

                // Basic validation
                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    Console.WriteLine("Username and password are required. Exiting client.");
                    return;
                }

                // 3. Connect to the server
                // Note: Updated IP to 127.0.0.1 for typical local development
                using var client = new TcpClient(ServerIp, Port);
                using var networkStream = client.GetStream();

                // 4. Construct and send the message in the format: OPERATION,username,password
                string messageToSend = $"{operation},{user},{pass}";
                byte[] buffer = Encoding.UTF8.GetBytes(messageToSend);

                Console.WriteLine($"\nSending request: {messageToSend}");
                networkStream.Write(buffer, 0, buffer.Length);

                // 5. Read the server's response
                byte[] responseBuffer = new byte[1024];
                int bytesRead = networkStream.Read(responseBuffer, 0, responseBuffer.Length);

                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

                // 6. Display the response
                Console.WriteLine("\n--- Server Response ---");
                Console.WriteLine(response);
                Console.WriteLine("-----------------------\n");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"\nConnection Error: Could not connect to the server at {ServerIp}:{Port}. Ensure the server is running and the IP address is correct.");
                Console.WriteLine($"Details: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nAn unexpected error occurred: {e.Message}");
            }
            Console.ReadLine();
        }
    }
}
