namespace SnakeGame
{
    public class SnakeHard : Snake
    {
        public SnakeHard(Point point, GameField gameField) : base(point, gameField)
        {
            MoveDelay = 0.2;
        }

        public override void Move(bool forceUpdate = false)
        {
            base.Move();

            //end game if snake hits edge of screen
            //if (Head.X > _gameField.Width - 1) ENDGAME;
            //if (Head.X < 0) ENDGAME;
            //if (Head.Y > _gameField.Height - 1) ENDGAME;
            //if (Head.Y < 0) ENDGAME;
        }

        public override void Eat(Fruit f)
        {
            base.Eat(f);
            if (Score % 3 == 0 && MoveDelay > 0.05)
            {
                MoveDelay -= 0.025;
            }
        }
    }
}