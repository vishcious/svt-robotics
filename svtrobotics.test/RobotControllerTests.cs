using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace svtrobotics.test
{
    public class RobotControllerTests
    {
        [Fact]
        public async Task SearchBestRobotTest_Find_Only_Closest()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new []{
                        new { RobotId = "1", BatteryLevel = 99, Y = 92, X = 48},
                        new { RobotId = "2", BatteryLevel = 29, Y = 8, X = 26},
                        new { RobotId = "3", BatteryLevel = 99, Y = 84, X = 91},
                        new { RobotId = "4", BatteryLevel = 37, Y = 7, X = 2},
                    })
                });
            
            RobotStatusService service = new RobotStatusService(new HttpClient(mockHttpMessageHandler.Object));

            var target = new RobotController(service);
            var result = await target.SearchBestRobot(new SearchRequestModel{
                LoadId = "123", X = 1, Y = 1
            });

            Assert.Equal("4", result.Value.RobotId);
        }

        [Fact]
        public async Task SearchBestRobotTest_Find_Equal_Distance_With_Best_Battery()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new []{
                        new { RobotId = "1", BatteryLevel = 99, Y = 92, X = 48},
                        new { RobotId = "2", BatteryLevel = 49, Y = 0, X = 0},
                        new { RobotId = "3", BatteryLevel = 99, Y = 84, X = 91},
                        new { RobotId = "4", BatteryLevel = 37, Y = 2, X = 2},
                    })
                });
            
            RobotStatusService service = new RobotStatusService(new HttpClient(mockHttpMessageHandler.Object));

            var target = new RobotController(service);
            var result = await target.SearchBestRobot(new SearchRequestModel{
                LoadId = "123", X = 1, Y = 1
            });

            Assert.Equal("2", result.Value.RobotId);
        }

        [Fact]
        public async Task SearchBestRobotTest_Find_Closest_With_Best_Battery()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new []{
                        new { RobotId = "1", BatteryLevel = 99, Y = 92, X = 48},
                        new { RobotId = "2", BatteryLevel = 49, Y = 3, X = 3},
                        new { RobotId = "3", BatteryLevel = 99, Y = 84, X = 91},
                        new { RobotId = "4", BatteryLevel = 37, Y = 0, X = 0},
                    })
                });
            
            RobotStatusService service = new RobotStatusService(new HttpClient(mockHttpMessageHandler.Object));

            var target = new RobotController(service);
            var result = await target.SearchBestRobot(new SearchRequestModel{
                LoadId = "123", X = 1, Y = 1
            });

            Assert.Equal("2", result.Value.RobotId);
        }

        [Fact]
        public async Task SearchBestRobotTest_Find_Closest_Of_Far_Robots()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new []{
                        new { RobotId = "1", BatteryLevel = 99, Y = 25, X = 26},
                        new { RobotId = "2", BatteryLevel = 49, Y = 33, X = 38},
                        new { RobotId = "3", BatteryLevel = 99, Y = 84, X = 91},
                        new { RobotId = "4", BatteryLevel = 37, Y = 70, X = 70},
                    })
                });
            
            RobotStatusService service = new RobotStatusService(new HttpClient(mockHttpMessageHandler.Object));

            var target = new RobotController(service);
            var result = await target.SearchBestRobot(new SearchRequestModel{
                LoadId = "123", X = 1, Y = 1
            });

            Assert.Equal("1", result.Value.RobotId);
        }

        [Fact]
        public async Task SearchBestRobotTest_Find_Best_Battery_Of_Equal_Distance_Far_Robots()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new []{
                        new { RobotId = "1", BatteryLevel = 99, Y = 20, X = 20},
                        new { RobotId = "2", BatteryLevel = 49, Y = 60, X = 60},
                        new { RobotId = "3", BatteryLevel = 99, Y = 84, X = 91},
                        new { RobotId = "4", BatteryLevel = 37, Y = 70, X = 70},
                    })
                });
            
            RobotStatusService service = new RobotStatusService(new HttpClient(mockHttpMessageHandler.Object));

            var target = new RobotController(service);
            var result = await target.SearchBestRobot(new SearchRequestModel{
                LoadId = "123", X = 40, Y = 40
            });

            Assert.Equal("1", result.Value.RobotId);
        }
        [Fact]
        public void CalculatedDistanceTest1() {
            var result = RobotController.CalculateDistance(2, 2, 1, 1);
            Assert.True(Math.Abs(Convert.ToDouble(result) - 1.414) < 0.001);
        }

        [Fact]
        public void CalculatedDistanceTest2() {
            var result = RobotController.CalculateDistance(1, 1, 2, 2);
            Assert.True(Math.Abs(Convert.ToDouble(result) - 1.414) < 0.001);
        }

        [Fact]
        public void CalculatedDistanceTest3() {
            var result = RobotController.CalculateDistance(1, 1, 0, 0);
            Assert.True(Math.Abs(Convert.ToDouble(result) - 1.414) < 0.001);
        }

        [Fact]
        public void CalculatedDistanceTest4() {
            var result = RobotController.CalculateDistance(1, 1, -1, -1);
            Assert.True(Math.Abs(Convert.ToDouble(result) - 2.828) < 0.001);
        }
    }
}
