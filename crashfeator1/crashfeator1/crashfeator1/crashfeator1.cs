using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class crashfeator1 : PhysicsGame
{
    Listener alotuspainikekuuntelija;
    List<GameObject> Backgrounds = new List<GameObject>();
    Image taustaKuva = LoadImage("taustakuva1");
    GameObject piilari;
    PhysicsObject pallo1;
    List<Label> valikonKohdat;
    bool pelikaynnissa;
    List<PhysicsObject> lälälä;
    public override void Begin()
    {
        SetWindowSize(800, 600);


        
        valikko();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");


        // TO DO LIST!!!


        // KERÄTTÄVÄT PALLOT = FYSIIKKAA = EI
        // RÄJÄHDYS 

        //LIIKE TUNNISTUS





        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void valikko()
    {
        pelikaynnissa = false;
        ClearAll();
        IsMouseVisible = true;
        valikonKohdat = new List<Label>();

        Label kohta1 = new Label("Start the game");
        kohta1.Position = new Vector(0, 40);
        valikonKohdat.Add(kohta1);
        alotuspainikekuuntelija = Mouse.ListenOn(kohta1, MouseButton.Left, ButtonState.Pressed, Startpainettu, "Start the game");

        foreach (Label valikonKohta in valikonKohdat)
        {
            Add(valikonKohta);
        }
    }
    void pelaa()
    {
        
       pallo1 = new PhysicsObject(30, 30);
       pallo1.Color = Color.Red;
       Add(pallo1);
       pallo1.Shape = Shape.Circle;
       
        generoi();

        piilari = new GameObject(1, 1);
        piilari.IsVisible = false;
        Add(piilari);
        Camera.Follow(piilari);
    }
    protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
        base.Update(gameTime);
        if (pelikaynnissa)
        {
            GameObject currentBackground = GetObjectAt(pallo1.Position, "background");
            if (currentBackground != null)
            {
                foreach (GameObject background in Backgrounds)
                {
                    if (background == currentBackground)
                    {
                        continue;
                    }

                    if (pallo1.Y > currentBackground.Y &&
                        background.Y < currentBackground.Y)
                    {
                        background.Y = currentBackground.Y + Screen.Height;
                    }

                    else if (pallo1.Y < currentBackground.Y &&
                        background.Y > currentBackground.Y)
                    {
                        background.Y = currentBackground.Y - Screen.Height;
                    }



                    if (pallo1.X > currentBackground.X &&
                       background.X < currentBackground.X)
                    {
                        background.X = currentBackground.X + Screen.Width;
                    }

                    else if (pallo1.X < currentBackground.X &&
                         background.X > currentBackground.X)
                    {
                        background.X = currentBackground.X - Screen.Width;
                    }

                }
            }

        }
        else
        {
            foreach (Label kohta in valikonKohdat)
            {
                if (Mouse.IsCursorOn(kohta))
                {
                    kohta.TextColor = Color.Red;
                }
                else
                {
                    kohta.TextColor = Color.Black;
                }
            }
        }

        if (pallo1 != null)
        {
            pallo1.Position = Mouse.PositionOnScreen;
        }
        
        
    }
    void generoi()
    {
        lälälä = new List<PhysicsObject>();
        Vector alkupiste = pallo1.Position;
        Angle suunta1 = RandomGen.NextAngle();
        Vector tp = alkupiste;
        
        for (int i = 0; i < 100; i++)
        {
            if (i%3==0)
            {
                Angle känsä = RandomGen.NextAngle(
                    Angle.FromDegrees(-10), 
                    Angle.FromDegrees(10));
                suunta1 = känsä;
            }
            Vector vp = Vector.FromLengthAndAngle(70, suunta1);
            tp = tp + vp;
            Vector newtp = new Vector(tp.X, tp.Y);
            Timer.SingleShot(2.0 + 0.7 * i, () => luotahti(newtp, RandomGen.NextColor()));
        }
    }
    void Startpainettu()
    {
        alotuspainikekuuntelija.Destroy();
        pelikaynnissa = true;
        ClearGameObjects();
        pelaa();
        GameObject background1 = new GameObject(Screen.Width, Screen.Height);
        Backgrounds.Add(background1);
        background1.Position = new Vector(Screen.Width / 2, Screen.Height / 2);

        GameObject background2 = new GameObject(Screen.Width, Screen.Height);
        Backgrounds.Add(background2);
        background2.Position = new Vector(Screen.Width / 2, -Screen.Height / 2);


        GameObject background3 = new GameObject(Screen.Width, Screen.Height);
        Backgrounds.Add(background3);
        background3.Position = new Vector(-Screen.Width / 2, Screen.Height / 2);


        GameObject background4 = new GameObject(Screen.Width, Screen.Height);
        Backgrounds.Add(background4);
        background4.Position = new Vector(-Screen.Width / 2, -Screen.Height / 2);


        Image stars = Level.Background.CreateStars();
        foreach (var bg in Backgrounds)
        {
            bg.Tag = "background";
            Add(bg, -3);
            bg.Image = stars;
          

        }

        //Level.Background.Image = taustaKuva;
        IsMouseVisible = false;
    }
    void luotahti(Vector paikka,Color vari)
    {
        PhysicsObject matta = new PhysicsObject(35, 35);
        matta.Shape = Shape.Circle;
        matta.Position = paikka;
        matta.Color = vari;
        Add(matta);
        matta.LifetimeLeft = TimeSpan.FromSeconds(3);
        lälälä.Add(matta);

        // Pista kamera siirtymaan kohti viimeista palluraa
        piilari.MoveTo(matta.Position, 100.0);
    }
}