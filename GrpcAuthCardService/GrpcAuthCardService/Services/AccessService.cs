using Google.Protobuf;
using Grpc.Core;
using MSCardAccessRequestService.Authentication;
using MSCardAccessRequestService.ServiceExtension;
using System;
using System.Text;

namespace MSCardAccessRequestService.Services;

public class AccessService : Access.AccessBase
{
    private readonly ILogger<AccessService> _logger;
    public AccessService(ILogger<AccessService> logger)
    {
        _logger = logger;
    }

    public override async Task RequestAccessByCredentials(CredentialsRequest request, IServerStreamWriter<ByteDataReply> responseStream, ServerCallContext context)
    {
        AuthenticationStatuses status = AuthenticationStatuses.Undefined;
        try
        {
            status = AuthenticationRunner.RunAsync(request).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        string answer;

        answer = status switch
        {
            AuthenticationStatuses.AuthentificationPass => "token", //dA==bw==aw==ZQ==bg==
            AuthenticationStatuses.Error => "error", //ZQ==cg==cg==bw==cg==
            AuthenticationStatuses.Undefined => "00000", //MA==MA==MA==MA==MA==

        };

        var bytes = Encoding.ASCII.GetBytes(answer);

        var byteList = new List<ByteDataReply>();

        foreach (var b in bytes)
        {
            byteList.Add(new ByteDataReply { Data = ByteString.CopyFrom(b) });
        }

        foreach (var b in byteList)
        {
            await responseStream.WriteAsync(b);
        }

    }

    public override Task<ByteDataReplyObject> RequestAccessByCredentialsObject(CredentialsRequest request, ServerCallContext context)
    {
        AuthenticationStatuses status = AuthenticationStatuses.Undefined;
        try
        {
            status = AuthenticationRunner.RunAsync(request).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        string answer;

        answer = status switch
        {
            AuthenticationStatuses.AuthentificationPass => "token", //dA==bw==aw==ZQ==bg==
            AuthenticationStatuses.Error => "error", //ZQ==cg==cg==bw==cg==
            AuthenticationStatuses.Undefined => "00000", //MA==MA==MA==MA==MA==

        };

        var bytes = Encoding.ASCII.GetBytes(answer);

        var reply = new ByteDataReplyObject();

        foreach (var b in bytes)
        {
            reply.Items.Add(ByteString.CopyFrom(b));
        }

        return Task.FromResult(reply);
    }

    public override async Task RequestAccessByToken(TokenRequest request, IServerStreamWriter<ByteDataReply> responseStream, ServerCallContext context)
    {
        /*try
        {
            //RunAsync(request).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }*/

        var bytes = Encoding.ASCII.GetBytes("token");

        var byteList = new List<ByteDataReply>();

        foreach (var b in bytes)
        {
            byteList.Add(new ByteDataReply { Data = ByteString.CopyFrom(b) });
        }

        foreach (var b in byteList)
        {
            await responseStream.WriteAsync(b);
        }
    }


}

