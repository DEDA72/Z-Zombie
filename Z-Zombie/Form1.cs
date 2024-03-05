using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Z_Zombie
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammos = 10;
        int zombieSpeed = 3;
        int score = 0;
        Random random = new Random();
        List<PictureBox> zombieList = new List<PictureBox>();
        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                progressBar1.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                pictureBox1.Image = Properties.Resources.dead;
                gameTimer.Stop();
            }
                label1.Text = "Ammo: " + ammos;
            label2.Text = "Kills:" + score;
            if(goLeft==true&&pictureBox1.Left>0)
            {
                pictureBox1.Left -= speed;
            }
            if (goRight == true && pictureBox1.Width < this.ClientSize.Width)
            {
                pictureBox1.Left += speed;
            }
            if (goUp == true && pictureBox1.Top > 45)
            {
                pictureBox1.Top -= speed;
            }
            if (goDown == true && pictureBox1.Top+pictureBox1.Height<this.ClientSize.Height)
            {
                pictureBox1.Top += speed;
            }
            foreach (Control item in this.Controls)
            {
                if(item is PictureBox&&(string)item.Tag=="ammo"&&gameOver==false)
                {
                    if(pictureBox1.Bounds.IntersectsWith(item.Bounds))
                    {
                        this.Controls.Remove(item);
                        ((PictureBox)item).Dispose();
                        ammos += 5;
                    }
                }

                if(item is PictureBox && (string)item.Tag=="zombie")
                {
                    if(pictureBox1.Bounds.IntersectsWith(item.Bounds))
                    {
                        playerHealth -= 1;
                    }
                    if(item.Left>pictureBox1.Left)
                    {
                        item.Left -= zombieSpeed;
                        ((PictureBox)item).Image = Properties.Resources.zleft;
                    }
                    if (item.Left < pictureBox1.Left)
                    {
                        item.Left += zombieSpeed;
                        ((PictureBox)item).Image = Properties.Resources.zright;
                    }
                    if (item.Top < pictureBox1.Top)
                    {
                        item.Top += zombieSpeed;
                        ((PictureBox)item).Image = Properties.Resources.zdown;
                    }
                    if (item.Top > pictureBox1.Top)
                    {
                        item.Top -= zombieSpeed;
                        ((PictureBox)item).Image = Properties.Resources.zup;
                    }
                    foreach (Control j in this.Controls)
                    {
                        if(j is PictureBox&&(string)j.Tag=="bullet"&&item is PictureBox&&(string)item.Tag=="zombie")
                        {
                            if(item.Bounds.IntersectsWith(j.Bounds))
                            {
                                score++;

                                this.Controls.Remove(item);
                                ((PictureBox)j).Dispose();
                                this.Controls.Remove(item);
                                ((PictureBox)item).Dispose();
                                zombieList.Remove(((PictureBox)item));
                                MakeZombies();
                            }
                        }
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameOver==true)
            {
                return; 
            }
            if(e.KeyCode==Keys.Left)
            {
                goLeft = true;
                facing = "left";
                pictureBox1.Image = Properties.Resources.left;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                pictureBox1.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                pictureBox1.Image = Properties.Resources.up;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                pictureBox1.Image = Properties.Resources.down;
            }

            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;

            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;

            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;

            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
;
            }
            if(e.KeyCode==Keys.Space&&ammos>0&&gameOver==false)
            {
                ammos--;
                Shoot(facing);
                if (ammos < 1)
                {
                    DropAmmo();
                }
            }
            if(e.KeyCode==Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
            
        }
        private void Shoot(string dir)
        {
            Bullet shootbullet = new Bullet();
            shootbullet.direction = dir;
            shootbullet.bulletLeft = pictureBox1.Left+(pictureBox1.Width/2);
            shootbullet.bulletTop = pictureBox1.Top + (pictureBox1.Height / 2);
            shootbullet.MakeBullet(this);
        }
        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = random.Next(0, 900);
            zombie.Top = random.Next(0, 800);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombieList.Add(zombie);
            this.Controls.Add(zombie);
            pictureBox1.BringToFront();

        }
        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = random.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = random.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            pictureBox1.BringToFront();
        }
        private void RestartGame()
        {
            pictureBox1.Image = Properties.Resources.up;

            foreach (PictureBox item in zombieList)
            {
                this.Controls.Remove(item);
            }

            zombieList.Clear();

            for(int i=0;i<3;i++)
            {
                MakeZombies();
            }
            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;
            playerHealth = 100;

            score = 0;
            ammos = 10;

            gameTimer.Start();
        }
    }
}
