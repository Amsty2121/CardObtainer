using Grpc.Core;
using Grpc.Net.Client;
using MSCardAccessRequestService;
using System.Text;

var channel = GrpcChannel.ForAddress("http://localhost:5201");
var client = new Access.AccessClient(channel);


using (var call = client.RequestAccessByToken(new TokenRequest { Token = "alala" }))
{
    while (await call.ResponseStream.MoveNext())
    {
        var currentByte = call.ResponseStream.Current;
        Console.Write(currentByte.Data.ToBase64());
    }
}
Console.WriteLine();

var bytes = "";

/*using (var call = client.RequestAccessByCredentials(
    new CredentialsRequest { Login = "gusac.dmitri@isa.utm.md", Password = "Azbuca1/" }))
{
    while (await call.ResponseStream.MoveNext())
    {
        var currentByte = call.ResponseStream.Current;

        bytes += currentByte.Data.ToStringUtf8();
        Console.Write(currentByte.Data.ToBase64());
    }
}*/

var b = await client.RequestAccessByCredentialsObjectAsync(new CredentialsRequest { Login = "gusac.dmitri@isa.utm.md", Password = "Azbuca1/" });
var bb = b.Items;
foreach (var a in bb)
{
    Console.Write(a.ToStringUtf8());
}

Console.WriteLine("\n" + bytes);

Console.ReadLine();
