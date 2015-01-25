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
    Listener ppk;
    Listener arka;
    Listener lopputuli;
    bool pelikaynnissa;
    List<PhysicsObject> lälälä;
    Image lentokuva = LoadImage("lentokone1");
    Image lentokuva2 = LoadImage("lentokone2");
    public override void Begin()
    {
#if DEBUG
        SetWindowSize(800, 600);
#endif
        valikko();

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

        Label kohta2 = new Label("Hight scores");
        kohta2.Position = new Vector(0, 80);
        valikonKohdat.Add(kohta2);
        ppk = Mouse.ListenOn(kohta2, MouseButton.Left, ButtonState.Pressed, pprnpp, "");

        Label kohta3 = new Label("options");
        kohta3.Position=new Vector(0,120);
        valikonKohdat.Add(kohta3);
        arka=Mouse.ListenOn(kohta3,MouseButton.Left,ButtonState.Pressed,juujuujuu,"");

        Label kohta4 = new Label("Quit");
        kohta4.Position= new Vector(0,160);
        valikonKohdat.Add(kohta4);
        lopputuli=Mouse.ListenOn(kohta4,MouseButton.Left,ButtonState.Pressed,Exit,"");

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

        generoi1();

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
    int generoi1()
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
        return 50;
    }
    int generoi2()
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
        return 50;
    }
    int generoi3()
    {
        lälälä = new List<PhysicsObject>();
        Vector alkupiste = pallo1.Position;
        Angle suunta1 = RandomGen.NextAngle();
        Vector tp = alkupiste;

        for (int i = 0; i < 100; i++)
        {
            if (i % KAANTOVALI < 1)
            {
                Angle känsä = RandomGen.NextAngle(
                    Angle.FromDegrees(-KAANTOKULMA - 20),
                    Angle.FromDegrees(KAANTOKULMA + 20));
                suunta1 = känsä;
            }
            Vector vp = Vector.FromLengthAndAngle(94, suunta1);
            tp = tp + vp;
            Vector newtp = new Vector(tp.X, tp.Y);
            Timer.SingleShot(1.7 + PALLONTEKONOPEUS * i, () => luotahti(newtp, RandomGen.NextColor()));

        }
        return 100;
    }
    int generoi4()
    {
        lälälä = new List<PhysicsObject>();
        Vector alkupiste = pallo1.Position;
        Angle suunta1 = RandomGen.NextAngle();
        Vector tp = alkupiste;

        for (int i = 0; i < 60; i++)
        {
            if (i % KAANTOVALI < 6)
            {
                Angle känsä = RandomGen.NextAngle(
                    Angle.FromDegrees(-KAANTOKULMA - 20),
                    Angle.FromDegrees(KAANTOKULMA + 20));
                suunta1 = känsä;
            }
            Vector vp = Vector.FromLengthAndAngle(94, suunta1);
            tp = tp + vp;
            Vector newtp = new Vector(tp.X, tp.Y);
            Timer.SingleShot(1.7 + PALLONTEKONOPEUS * i * 2, () => luotahti(newtp, Color.Aqua));
        }
        return 60;
    }
    int generoi5()
    {
        lälälä = new List<PhysicsObject>();
        Vector alkupiste = pallo1.Position;
        Angle suunta1 = RandomGen.NextAngle();
        Vector tp = alkupiste;

        for (int i = 0; i < 80; i++)
        {
            if (i % KAANTOVALI < 6)
            {
                Angle känsä = RandomGen.NextAngle(
                    Angle.FromDegrees(-KAANTOKULMA - 45),
                    Angle.FromDegrees(KAANTOKULMA + 45));
                suunta1 = känsä;
            }
            Vector vp = Vector.FromLengthAndAngle(94, suunta1);
            tp = tp + vp;
            Vector newtp = new Vector(tp.X, tp.Y);
            Timer.SingleShot(1.7 + PALLONTEKONOPEUS * i * 4, () => luotahti(newtp, Color.Red));
        }
        return 80;
    }
    void lv()
    {
        Surfaces aweryq = Level.CreateBorders();
        for (int i = 0; i < 30; i++)
        {
            luolentsikka(new Vector(RandomGen.NextInt(-300, 300), Screen.Bottom));

        }
        Timer.SingleShot(32, delegate { puhdista(aweryq); });
    }

    void luolentsikka(Vector paikka)
    {
        PhysicsObject lentsikka = new PhysicsObject(50, 50);
        lentsikka.Position = paikka;
        Add(lentsikka);

        lentsikka.Image = lentokuva;
        Vector ljk = new Vector(Screen.Center.X, Screen.Top);


        RandomMoverBrain satunnaisAivot = new RandomMoverBrain(200);
        satunnaisAivot.ChangeMovementSeconds = 3;
        lentsikka.Brain = satunnaisAivot;

        lentsikka.LifetimeLeft = TimeSpan.FromSeconds(30);
    }
    void puhdista(Surfaces areusure)
    {
        areusure.Left.Destroy();
        areusure.Right.Destroy();
        areusure.Top.Destroy();
        areusure.Bottom.Destroy();
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
        int pallojatulossa = 0;
        if (luku == 1)
        {
            pallojatulossa=generoi1();
        }

        if (luku == 2)
        {
            pallojatulossa = generoi2();
        }

        if (luku == 3)
        {
            pallojatulossa = generoi3();
        }
        if (luku == 4)
        {
            pallojatulossa = generoi4();
        }
        if (luku == 5)
        {
            pallojatulossa = generoi5();
        }
        if (luku == 6)
        {
            lv();
        }
        voimaMittari.Value = 0.0;
        voimaMittari.MaxValue = pallojatulossa;
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















    void pprnpp()
    {
        
    }
    void juujuujuu()
    {
    }
}