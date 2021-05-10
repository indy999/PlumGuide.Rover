namespace PlumGuide.Rover.Tests
{
    using Moq;
    using PlumGuide.Rover.Service;
    using System.Drawing;
    using PlumGuide.Rover.Service.Interfaces;
    using Xunit;

    public class PlutoRoverTests
    {
        private readonly PlutoRover _rover;
        private readonly Mock<IGridService> _gridServiceMock;

        public PlutoRoverTests()
        {
            _gridServiceMock = new Mock<IGridService>();
            _rover = new(_gridServiceMock.Object);
        }

        [Theory]
        [InlineData("F",1)]
        [InlineData("FB", 2)]
        [InlineData("FRB", 2)]
        [InlineData("FLB", 2)]
        [InlineData("FRBR", 2)]
        [InlineData("FRBRF", 3)]
        [InlineData("FFRFF", 4)]
        [InlineData("RFFLF", 3)]
        public void Move_Always_PerformsExpectedWork(string command,  int expectedInvocationCount)
        {
            _gridServiceMock.Setup(x => x.CanMove(It.IsAny<Point>(), It.IsAny<string>(), It.IsAny<char>()))
                .Returns(true);

            var newPoint = new Point(1, 1);
            _gridServiceMock.Setup(x => x.CalculatePosition(It.IsAny<Point>(), It.IsAny<string>(), It.IsAny<char>()))
                .Returns(newPoint);
            
            _rover.Move(command);

            _gridServiceMock.Verify(x => x.CalculatePosition(It.IsAny<Point>(), It.IsAny<string>(), It.IsAny<char>()),Times.Exactly(expectedInvocationCount));
            Assert.Equal(newPoint, _rover.Position);
        }

        [Fact]
        public void Direction_DefaultValue_IsNorth()
        {
            Assert.Equal("N",_rover.Direction);
        }

        [Theory]
        [InlineData("R","E")]
        [InlineData("L", "W")]
        [InlineData("RR", "S")]
        [InlineData("LL", "S")]
        [InlineData("RRR", "W")]
        [InlineData("RRRR", "N")]
        [InlineData("LLLL", "N")]
        public void Move_WhenRotating_UpdatesDirection(string command, string expectedDirection)
        {
            _rover.Move(command);

            Assert.Equal(expectedDirection, _rover.Direction);
        }

        [Fact]
        public void Move_WhenObstacleExists_DoesNotMove()
        {
            var position = new Point(0, 0);
            var direction = "N";
            var command = 'F';

            _gridServiceMock.Setup(x => x.CanMove(position, direction, command))
                .Returns(false);

            _gridServiceMock.Setup(x => x.CalculatePosition(position, direction, command))
                .Returns(new Point(0,1));

            _rover.Move(command.ToString());

            Assert.Equal(position, _rover.Position);
        }

        [Fact]
        public void Move_WhenObstacleExists_StopsProcessing()
        {
            var position = new Point(0, 0);
            var direction = "N";
            var command = 'F';

            _gridServiceMock.Setup(x => x.CanMove(position, direction, command))
                .Returns(false);

            _gridServiceMock.Setup(x => x.CalculatePosition(position, direction, command))
                .Returns(new Point(0, 1));

            _rover.Move("FR");

            Assert.Equal(direction,_rover.Direction);
        }
    }
}
