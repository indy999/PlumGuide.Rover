namespace PlumGuide.Rover.Service
{
    using System.Drawing;
    using System.Linq;
    using Interfaces;

    public class PlutoRover
    {
        private readonly IGridService _gridService;
        private readonly char[] _rotationCommands = { 'R', 'L' };
        private readonly char[] _moveCommands = { 'F', 'B' };

        public PlutoRover(IGridService gridService)
        {
            _gridService = gridService;
        }

        public void Move(string command)
        {
            var commands = command.ToCharArray();

            foreach (var inlineCommand in commands)
            {
                if(_rotationCommands.Contains(inlineCommand))
                    Rotate(inlineCommand);

                if (!_moveCommands.Contains(inlineCommand)) continue;

                if (_gridService.CanMove(Position, Direction, inlineCommand))
                    Move(inlineCommand);
                else
                    break;
            }
        }

        private void Move(char inlineCommand)
        {
            Position =  _gridService.CalculatePosition(Position, Direction, inlineCommand);
        }

        private void Rotate(char inlineCommand)
        {
            switch (Direction)
            {
                case "N":
                    Direction = inlineCommand == 'L' ? "W" : "E";
                    break;
                case "E":
                    Direction = inlineCommand == 'L' ? "N" : "S";
                    break;
                case "W":
                    Direction = inlineCommand == 'L' ? "S" : "N";
                    break;
                case "S":
                    Direction = inlineCommand == 'L' ? "E" : "W";
                    break;
            }
        }

        public Point Position { get; private set; } = new(0, 0);
        public string Direction { get; internal set; } = "N";
    }
}