namespace PlumGuide.Rover.Service
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Interfaces;

    public class GridService : IGridService
    {
        private readonly int _maxX;
        private readonly int _maxY;
        private readonly List<Point> _obstacles;

        public GridService(int maxX, int maxY, List<Point> obstacles)
        {
            _maxX = maxX;
            _maxY = maxY;
            _obstacles = obstacles;
        }

        public Point CalculatePosition(Point currentPosition, string direction, char command)
        {
            return Move(currentPosition, direction,  command);
        }

        private Point Move(Point position, string direction,  char inlineCommand)
        {
            switch (direction)
            {
                case "N":
                    position = inlineCommand == 'F' ? new Point(position.X, position.Y + 1) : new Point(position.X, position.Y - 1);
                    break;
                case "E":
                    position = inlineCommand == 'F' ? new Point(position.X + 1, position.Y) : new Point(position.X - 1, position.Y);
                    break;
                case "W":
                    position = inlineCommand == 'F' ? new Point(position.X - 1, position.Y) : new Point(position.X + 1, position.Y);
                    break;
                case "S":
                    position = inlineCommand == 'F' ? new Point(position.X, position.Y - 1) : new Point(position.X, position.Y + 1);
                    break;
            }

            position = Wrap(position);

            return position;
        }

        private Point Wrap(Point position)
        {
            if (position.Y < 0)
                return new Point(position.X, _maxY);
            if (position.Y > _maxY)
                return new Point(position.X, 0);
            if (position.X < 0)
                return new Point(_maxX, position.Y);
            return position.X > _maxX ? new Point(0, position.Y) : position;
        }

        public bool CanMove(Point position, string direction, char command)
        {
            var newPosition = Move(position, direction,  command);

            return _obstacles.All(x => x != newPosition);
        }
    }
}