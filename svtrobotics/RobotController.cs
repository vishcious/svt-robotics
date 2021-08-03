using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace svtrobotics {
    public class RobotController {
        public RobotStatusService RobotStatusService {get;set;}
        public RobotController(RobotStatusService robotStatusService) {
            this.RobotStatusService = robotStatusService;
        }

        [HttpPost("search")]
        public async Task<ActionResult<SearchResponseModel>> SearchBestRobot(SearchRequestModel input) {
            var robotStatuses = await RobotStatusService.GetRobotStatuses();
            var robotDistances = from item in robotStatuses select new SearchResponseModel{RobotId = item.RobotId, BatteryLevel = item.BatteryLevel, DistanceToGoal = CalculateDistance(input.X, input.Y, item.X, item.Y)};
            var closeRobots = from robot in robotDistances where robot.DistanceToGoal <= 10 select robot;
            if(closeRobots.Any()) {
                return closeRobots.Aggregate((currentMax, x) => currentMax == null ? x : (x.BatteryLevel > currentMax.BatteryLevel ? x : currentMax));
            }
            else { // The requirements for how to handle cases where there is no robot within 10 distance units are not well specified and will need to be clarified
                // Currently going with the following logic
                // return the closest robot no matter the battery level
                // if multiple robots are exactly the same units of distance away, use the one with the best battery
                // the following information would be useful for a more complete logic
                // 1) will the remaining battery suffice for the distance needed to travel i.e. some sort of battery / distance relationship
                // 2) some sort of weight basis to use for battery and distance values to pick the best robot amongst multiple capable robots that have enough battery to travel the distance
                return robotDistances.Aggregate((currentMin, x) => currentMin == null ? x : (x.DistanceToGoal < currentMin.DistanceToGoal ? x : (x.DistanceToGoal == currentMin.DistanceToGoal ? (x.BatteryLevel > currentMin.BatteryLevel ? x : currentMin) : currentMin)));
            }
        }

        public static decimal CalculateDistance(decimal x1, decimal y1, decimal x2, decimal y2) {
            return Convert.ToDecimal(Math.Sqrt(Math.Pow(Decimal.ToDouble(x2 - x1), 2) + Math.Pow(Decimal.ToDouble(y2 - y1), 2)));
        }
    }
}