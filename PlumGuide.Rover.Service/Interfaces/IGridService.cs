namespace PlumGuide.Rover.Service.Interfaces
{
    using System.Drawing;

    public interface IGridService
    {
        Point CalculatePosition(Point currentPosition, string direction, char command);
        bool CanMove(Point position, string direction, char command);
    }
}