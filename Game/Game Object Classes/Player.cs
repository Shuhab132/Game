﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game {

    enum ActionType {
        idle,
        walk,
        run,
        die,
        jump,
        crouch,
    }

    class Player : Image {
        public int Frame { get; set; } = 0;
        public ActionType Action { get; set; } = ActionType.idle;
        public int xPos { get; set; } = 300;
        public int yPos    {get; set;} = 300;
        
        private static Dictionary<ActionType, List<BitmapImage>> Sources = new Dictionary<ActionType, List<BitmapImage>>();

        public Rectangle HitBoxRender  = new Rectangle();
        public Rect HitBox  = new Rect();

        // Create a Dictionary to hold all frames for each Actions
        private static Dictionary<ActionType, int> ActionTypeFrames = new Dictionary<ActionType, int>() {
            {ActionType.idle,   4},
            {ActionType.die,    4},
            {ActionType.jump,   4},
            {ActionType.crouch, 4},
            {ActionType.walk,    6},
            {ActionType.run,    6},
        };

        // Define size of player 
        public Player() {
            this.Width = 80;
            this.Height = 80;
            this.Stretch = System.Windows.Media.Stretch.UniformToFill;

        }

        // Static Constructor to load all the images to a Dictonary
        static Player() {

            // Create a path to the bitmaps
            string path = Environment.CurrentDirectory;

            foreach (ActionType action in Enum.GetValues(typeof(ActionType))) {

                Player.Sources.Add(action, new List<BitmapImage>());

                for (int frame = 0; frame < ActionTypeFrames[action]; frame++) {

                    Player.Sources[action].Add(new BitmapImage(
                        new Uri(string.Format(@"{0}\Bitmaps\adventurer-{1}-{2}.png", path, action, frame.ToString("00")))
                    ));
                }
            }
        }

        public void SetSource() {
            this.Source = Player.Sources[this.Action][GetActionFrame()];
        }

        private int GetActionFrame() {
            return this.Action switch
            {
                ActionType.walk => this.Frame % 6,
                ActionType.run => this.Frame % 6,
                ActionType.die => this.Frame % 4,
                ActionType.jump => this.Frame % 4,
                ActionType.crouch => this.Frame % 4,
                _ => this.Frame % 4,
            };

        }

        // Set the time for each action in Millis
        public int ActionTime() {

            return this.Action switch
            {
                ActionType.walk   =>    80,
                ActionType.run    =>    80,
                ActionType.die    =>    70,
                ActionType.jump   =>    120,
                ActionType.crouch =>    250,
                _                 =>    200,
            };

        }

        public void Collision() {

            HitBox.Width = this.Width / 2;
            HitBox.Height = this.Height;
            HitBox.X = Canvas.GetLeft(this) + (this.Width / 2);
            HitBox.Y = Canvas.GetTop(this);

            if(Action == ActionType.crouch) {
                HitBox.Height = HitBox.Height / 2;
                HitBox.Y = HitBox.Y + HitBox.Height;
            }

        }

        public void CollisionBoxRender() {
            HitBoxRender.Width = this.HitBox.Width;
            HitBoxRender.Height = this.HitBox.Height;
            HitBoxRender.Stroke = Brushes.Black;
            Canvas.SetLeft(HitBoxRender, HitBox.X);
            Canvas.SetTop(HitBoxRender, HitBox.Y);

            
        }

       



    }
}