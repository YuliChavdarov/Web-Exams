using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SUS.HTTP
{
    public class HttpServer : IHttpServer
    {
        public static IDictionary<string, Dictionary<string, string>> Sessions = new Dictionary<string, Dictionary<string, string>>();

        List<Route> routeTable;

        public HttpServer(List<Route> routeTable)
        {
            this.routeTable = routeTable;
        }

        public async Task StartAsync(int port)
        {
            TcpListener tcpListener =
                new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ProcessClientAsync(tcpClient);
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    // TODO: research if there is faster data structure for array of bytes
                    List<byte> data = new List<byte>();
                    int position = 0;
                    byte[] buffer = new byte[HttpConstants.BufferSize]; // chunk
                    while (true)
                    {
                        int count =
                            await stream.ReadAsync(buffer, position, buffer.Length);
                        position += count;

                        if (count < buffer.Length)
                        {
                            var partialBuffer = new byte[count];
                            Array.Copy(buffer, partialBuffer, count);
                            data.AddRange(partialBuffer);
                            break;
                        }
                        else
                        {
                            data.AddRange(buffer);
                        }
                    }

                    // byte[] => string (text)
                    var requestAsString = Encoding.UTF8.GetString(data.ToArray());
                    var request = new HttpRequest(requestAsString);
                    Console.WriteLine($"{request.Method} {request.Path} => {request.Headers.Count} headers");

                    var route = this.routeTable.FirstOrDefault(
                        x => string.Compare(x.Path, request.Path, true) == 0
                            && x.Method == request.Method);

                    HttpResponse response;

                    if (route != null)
                    {
                        var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionCookieName);

                        if (sessionCookie == null)
                        {
                            var sessionId = Guid.NewGuid().ToString();
                            request.Session = new Dictionary<string, string>();
                            Sessions.Add(sessionId, request.Session);
                            request.Cookies.Add(new Cookie(HttpConstants.SessionCookieName, sessionId));
                        }
                        else if (!Sessions.ContainsKey(sessionCookie.Value))
                        {
                            request.Session = new Dictionary<string, string>();
                            Sessions.Add(sessionCookie.Value, request.Session);
                        }
                        else
                        {
                            request.Session = Sessions[sessionCookie.Value];
                        }

                        response = route.Action(request);

                        sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionCookieName);

                        if (sessionCookie != null)
                        {
                            var responseSessionCookie = new ResponseCookie(sessionCookie.Name, sessionCookie.Value);
                            responseSessionCookie.Path = "/";
                            response.Cookies.Add(responseSessionCookie);
                        }

                        else
                        {
                            // It shouldn't reach, unless route.Action(request) deletes the request's session cookie
                            Console.WriteLine("It reaches???");
                        }
                    }

                    else
                    {
                        // Not Found 404
                        response = new HttpResponse("text/html", new byte[0], HttpStatusCode.NotFound);
                    }

                    response.Headers.Add(new Header("Server", "SUS Server 1.0"));

                    var responseHeaderBytes = Encoding.UTF8.GetBytes(response.ToString());
                    await stream.WriteAsync(responseHeaderBytes, 0, responseHeaderBytes.Length);

                    if (response.Body != null)
                    {
                        await stream.WriteAsync(response.Body, 0, response.Body.Length);
                    }
                }

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
