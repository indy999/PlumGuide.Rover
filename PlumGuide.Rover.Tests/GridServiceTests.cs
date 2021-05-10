namespace PlumGuide.Rover.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using PlumGuide.Rover.Service;
    using Xunit;

    public class GridServiceTests
    {
        private readonly GridService _gridService;
        private readonly List<Point> _obstacles;

        public GridServiceTests()
        {
            _obstacles = new List<Point>();
            _gridService = new GridService(100,100, _obstacles);
        }

        [Theory]
        [InlineData(0, 0, "N",'F', 0,1)]
        [InlineData(0, 0, "E",'F', 1, 0)]
        [InlineData(0, 1, "N",'B', 0, 0)]
        [InlineData(0, 0, "N", 'B', 0, 100)]
        [InlineData(0, 0, "E", 'B', 100, 0)]
        [InlineData(0, 100, "N", 'F', 0, 0)]
        [InlineData(100, 0, "E", 'F', 0, 0)]
        public void CalculatePosition_Always_ReturnsExpectedValue(int x, int y,string direction,char command,
                                                                int expectedX, int expectedY)
        {
            var position = new Point(x, y);
            var expectedPosition = new Point(expectedX, expectedY);

            var result = _gridService.CalculatePosition(position, direction, command);

            Assert.Equal(expectedPosition, result);
        }

        [Theory]
        [InlineData(0,0,"N",'F',false)]
        [InlineData(0,2, "N", 'F', true)]
        public void CanMove_Always_ReturnsExpectedResult(int x, int y, string direction, char command,
                                                bool expectedResult)
        {
            var currentPosition = new Point(x, y);
            _obstacles.Add(new Point(0,1));

            var result = _gridService.CanMove(currentPosition, direction,  command);

            Assert.Equal(expectedResult,result);
        }

    }
}
