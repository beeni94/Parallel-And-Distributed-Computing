using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq; // Added for clarity, though not strictly needed for the simple dictionary operations
class AuthServer
{
    static readonly Dictionary<string, string> users = new Dictionary<string, string>
    {
        {"Zara", "1212"},
        {"Zeenat", "8013"},
        {"Zimal", "0011"}
    };
    static void Main(string[] args)
    {for(int i=0; i<10;i++)
        {
            var s = new TcpListener(IPAddress.Any, 5000);
            s.Start();
            Console.WriteLine(" Auth Server Running...");
            while (true)
            {
                try
                {
                    using var c = s.AcceptTcpClient();
                    using var ns = c.GetStream();

                    byte[] buf = new byte[1024];
                    int n = ns.Read(buf, 0, buf.Length);
                    string request = Encoding.UTF8.GetString(buf, 0, n).Trim();
                    string[] parts = request.Split(',');
                    string user, pass, operation;
                    string msg;
                    if (parts.Length != 3)
                    {
                        msg = "Error: Invalid request format. Expected: OPERATION,username,password";
                        Console.WriteLine($"Received malformed request: {request}");
                    }
                    else
                    {
                        operation = parts[0].Trim().ToUpperInvariant(); // Expected: LOGIN or SIGNUP
                        user = parts[1].Trim();
                        pass = parts[2].Trim();
                        switch (operation)
                        {
                            case "LOGIN":
                                // Check for existing user and correct password
                                if (users.ContainsKey(user) && users[user] == pass)
                                {
                                    msg = "Login Successful";
                                }
                                else
                                {
                                    msg = "Invalid Credentials";
                                }
                                break;

                            case "SIGNUP":
                                // Check if user already exists
                                if (users.ContainsKey(user))
                                {
                                    msg = "Registration Failed: User already exists";
                                }
                                else
                                {
                                    // Register the new user
                                    users.Add(user, pass);
                                    msg = "Registration Successful! Please log in.";
                                }
                                break;

                            default:
                                msg = "Error: Invalid Operation. Use LOGIN or SIGNUP.";
                                break;
                        }
                        Console.WriteLine($"{user} ({operation}) -> {msg}");
                    }

                    // Send the response back to the client
                    ns.Write(Encoding.UTF8.GetBytes(msg));
                }
                catch (Exception ex)
                {
                    // General error handling (e.g., client disconnection)
                    Console.WriteLine($"Error handling client connection: {ex.Message}");
                }
            }
        }
    }
}
