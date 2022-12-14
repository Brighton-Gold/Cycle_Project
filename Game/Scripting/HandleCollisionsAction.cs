using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the snake 
    /// collides with the food, or the snake collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool isGameOver = false;
        private bool player1wins = false;
        private bool player2wins = false;



        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)
        {
            if (isGameOver == false)
            {
                HandleFoodCollisions(cast);
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
            }
        }

        /// <summary>
        /// Updates the score nd moves the food if the snake collides with it.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleFoodCollisions(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            Snake player2 = (Snake)cast.GetFirstActor("player2");

            Score score = (Score)cast.GetFirstActor("score");
            Food food = (Food)cast.GetFirstActor("food");
            Food food2 = (Food)cast.GetFirstActor("food2");
            Powerup powerup = (Powerup)cast.GetFirstActor("powerup");


            if (snake.GetHead().GetPosition().Equals(food.GetPosition()))
            {
                int points = food.GetPoints();
                snake.GrowTail(points);
                score.AddPoints(points);
                food.Reset();
            }

            if (snake.GetHead().GetPosition().Equals(food2.GetPosition()))
            {
                int points = food.GetPoints();
                snake.GrowTail(points);
                score.AddPoints(points);
                food2.Reset();
            }

            if (player2.GetHead().GetPosition().Equals(food.GetPosition()))
            {
                int points = food.GetPoints();
                player2.GrowTail(points);
                score.AddPointsPlayer2(points);
                food.Reset();
            }

            if (player2.GetHead().GetPosition().Equals(food2.GetPosition()))
            {
                int points = food.GetPoints();
                player2.GrowTail(points);
                score.AddPointsPlayer2(points);
                food2.Reset();
            }

            if (player2.GetHead().GetPosition().Equals(powerup.GetPosition()))
            {
                powerup.Reset();

            }
            if (snake.GetHead().GetPosition().Equals(powerup.GetPosition()))
            {
                powerup.Reset();
            }
        }

        /// <summary>
        /// Sets the game over flag if the snake collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            Actor head = snake.GetHead();
            List<Actor> body = snake.GetBody();

            Snake player2 = (Snake)cast.GetFirstActor("player2");
            Actor head2 = player2.GetHead();
            List<Actor> body2 = player2.GetBody();

            foreach (Actor segment in body)
            {
                foreach (Actor segment2 in body2)
                {

                    if (segment.GetPosition().Equals(head.GetPosition())
                    || head.GetPosition().Equals(segment2.GetPosition()))
                    {
                        player2wins = true;

                        isGameOver = true;
                    }

                    if (segment2.GetPosition().Equals(head2.GetPosition())
                    || head2.GetPosition().Equals(segment.GetPosition()))
                    {
                        player1wins = true;

                        isGameOver = true;
                    }
                }
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (isGameOver == true)
            {
                Snake snake = (Snake)cast.GetFirstActor("snake");
                List<Actor> segments = snake.GetSegments();

                Snake player2 = (Snake)cast.GetFirstActor("player2");
                List<Actor> segments2 = player2.GetSegments();

                Food food = (Food)cast.GetFirstActor("food");
                Food food2 = (Food)cast.GetFirstActor("food2");


                // create a "game over" message
                int x = Constants.MAX_X / 2;
                int y = Constants.MAX_Y / 2;
                Point position = new Point(x, y);

                Actor message = new Actor();
                message.SetText("Game Over!");
                message.SetPosition(position);
                cast.AddActor("messages", message);

                // make everything white

                if (player1wins == true)
                {
                    foreach (Actor segment in segments2)
                    {
                        segment.SetColor(Constants.WHITE);
                    }
                }
                if (player2wins == true)
                {
                    foreach (Actor segment in segments)
                    {
                        segment.SetColor(Constants.WHITE);
                    }
                }
                food.SetColor(Constants.WHITE);
                food2.SetColor(Constants.WHITE);

            }
        }

    }
}