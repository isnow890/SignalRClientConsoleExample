using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClientConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // SignalR 서버 URL 설정
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5080/gameHub") // 서버 URL 설정
                .Build();

            // 이벤트 수신 설정
            hubConnection.On<GameUpdateModel>("PlayerLeft", update =>
            {
                Console.WriteLine($"PlayerLeft event received: Game No {update.GameNo}, UserSeq {update.Data["userSeq"]}");
            });

            hubConnection.On<GameUpdateModel>("GameCreated", update =>
            {
                Console.WriteLine($"GameCreated event received: Game No {update.GameNo}, update : gameNo : {update.Data["gameNo"] } creatorId : {update.Data["creatorId"] }");
            });



            hubConnection.On<GameUpdateModel>("GameStarted", update =>
            {
                Console.WriteLine($"GameStarted event received: Game No {update.GameNo}, StartTime {update.Data["startTime"]}");
            });

            hubConnection.On<GameUpdateModel>("GameEnded", update =>
            {
                Console.WriteLine($"GameEnded event received: Game No {update.GameNo}, EndTime {update.Data["endTime"]}");
            });


            hubConnection.On<GameUpdateModel>("PlayerJoined", update =>
            {
                Console.WriteLine($"PlayerJoined event received: Game No {update.GameNo}, update : userSeq : {update.Data["userSeq"] } nickName : {update.Data["nickName"] } ");
            });



            try
            {
                // 서버 연결 시작
                await hubConnection.StartAsync();
                Console.WriteLine("Connection started.");

                // 서버로부터 이벤트 수신 대기
                Console.WriteLine("Listening for events. Press any key to exit...");
                // 무기한 대기 상태 유지
                await Task.Delay(-1);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
            finally
            {
                // 연결 종료^
                await hubConnection.DisposeAsync();
                Console.WriteLine("Connection closed.");
            }
        }
    }
}

public record GameUpdateModel(
    int GameNo,
    string UpdateType,
    string AccountId,
    Dictionary<string, object> Data
);
