using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class crashfeator1 : PhysicsGame
{

    
    DoubleMeter voimaMittari;
    static int TUHOUTUMINEN = 5;
    static double KAMERANOPEUS = 500.0;
    static double PALLONTEKONOPEUS = 0.1;
    static int KAANTOVALI = 5;
    static int KAANTOKULMA = 50;
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
       pallo1.IgnoresExplosions = true;

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
            GameObject currentBackground = GetObjectAt(Camera.Position, "background");
            if (currentBackground != null)
            {
                foreach (GameObject background in Backgrounds)
                {
                    if (background == currentBackground)
                    {
                        continue;
                    }

                    if (Camera.Y > currentBackground.Y &&
                        background.Y < currentBackground.Y)
                    {
                        background.Y = currentBackground.Y + Screen.Height;
                    }

                    else if (Camera.Y < currentBackground.Y &&
                        background.Y > currentBackground.Y)
                    {
                        background.Y = currentBackground.Y - Screen.Height;
                    }



                    if (Camera.X > currentBackground.X &&
                       background.X < currentBackground.X)
                    {
                        background.X = currentBackground.X + Screen.Width;
                    }

                    else if (Camera.X < currentBackground.X &&
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
            pallo1.Position = Mouse.PositionOnWorld;
        }


    }
    void generoi()
    {
        lälälä = new List<PhysicsObject>();
        Vector alkupiste = pallo1.Position;
        Angle suunta1 = RandomGen.NextAngle();
        Vector tp = alkupiste;

        for (int i = 0; i < 50; i++)
        {
            if (i % KAANTOVALI == 0)
            {
                Angle känsä = RandomGen.NextAngle(
                    Angle.FromDegrees(-KAANTOKULMA),
                    Angle.FromDegrees(KAANTOKULMA));
                suunta1 = känsä;
            }
            Vector vp = Vector.FromLengthAndAngle(70, suunta1);
            tp = tp + vp;
            Vector newtp = new Vector(tp.X, tp.Y);
            Timer.SingleShot(2.0 + PALLONTEKONOPEUS * i, () => luotahti(newtp, RandomGen.NextColor()));
        }
    }
    void Startpainettu()
    {
        alotuspainikekuuntelija.Destroy();
        pelikaynnissa = true;
        ClearGameObjects();
        pelaa();
        voimamittari();
        teksti();
        Image seuraavaks = LoadImage("nimetön");
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
    void luotahti(Vector paikka, Color vari)
    {
        PhysicsObject matta = new PhysicsObject(35, 35);
        matta.Shape = Shape.Circle;
        matta.Position = paikka;
        matta.Color = vari;
        matta.IgnoresExplosions = true;

        Add(matta);
        matta.LifetimeLeft = TimeSpan.FromSeconds(TUHOUTUMINEN);
        matta.Destroying += () => voimaMittari.Value += 1;
        lälälä.Add(matta);
        AddCollisionHandler(matta, pallo1, mittaritayttyy);
        
       
        piilari.MoveTo(matta.Position, KAMERANOPEUS);
    }
    void generoi2()
    {
        lälälä = new List<PhysicsObject>();
        Vector alkupiste = pallo1.Position;
        Angle suunta1 = RandomGen.NextAngle();
        Vector tp = alkupiste;

        for (int i = 0; i < 50; i++)
        {
            if (i % KAANTOVALI == 0)
            {
                Angle känsä = RandomGen.NextAngle(
                    Angle.FromDegrees(-KAANTOKULMA),
                    Angle.FromDegrees(KAANTOKULMA));
                suunta1 = känsä;
            }
            Vector vp = Vector.FromLengthAndAngle(43, suunta1);
            tp = tp + vp;
            Vector newtp = new Vector(tp.X, tp.Y);
            Timer.SingleShot(1.6 + PALLONTEKONOPEUS * i, () => luotahti(newtp, RandomGen.NextColor()));
        }
    }
    void generoi3()
    {
    }
    void generoi4()
    {
    }
    void generoi5()
    {
    }
    void lv()
    {
    }





   void voimamittari()
    {
     voimaMittari = new DoubleMeter(0);
     voimaMittari.MaxValue = 50;
     ProgressBar voimaPalkki = new ProgressBar(150, 10);
     voimaPalkki.X = Screen.Left + 150;
     voimaPalkki.Y = Screen.Top - 70;
     voimaMittari.Value = 0;
     voimaPalkki.BorderColor = Color.Aqua;
     voimaPalkki.BindTo(voimaMittari);
     voimaPalkki.Image = LoadImage("palkki tyhj");
     voimaPalkki.BarImage = LoadImage("palkki sini");
       
       Add(voimaPalkki);
     
       
     voimaMittari.UpperLimit += VoimaMittariTaynna;
    }



    void VoimaMittariTaynna()
   {
     
       int luku = RandomGen.NextInt(1, 6);
       if (luku == 1)
       {
           generoi();
       }
      
       if (luku == 2)
       {
           generoi2();
       }
      
       if (luku == 3)
       {
           generoi3();
       }
       if (luku == 4)
       {
           generoi4();
       }
       if (luku == 5)
       {
           generoi5();
       }
       if (luku == 6)
       {
           lv();
       }
    }


    void mittaritayttyy(PhysicsObject matta, PhysicsObject pallo1)
    {
       Explosion rajahdys = new Explosion(100);
       rajahdys.Position = matta.Position;
       matta.Destroy();
       Add(rajahdys);

    }
    void teksti()
    {
        Label teksti = new Label("What do we do next?");
        teksti.X = Screen.Right - 560;
        teksti.Y = Screen.Top - 50;
        teksti.TextColor = Color.Green;
        teksti.Font = Font.DefaultLarge;

        Add(teksti);

    }
}