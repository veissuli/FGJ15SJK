using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class crashfeator1 : PhysicsGame
{
    Image taustaKuva = LoadImage("taustakuva1");
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
        // PELAAJA HIIRI
        // SATUNNAINEN PALLOPOLKU
        // KERÄTTÄVÄT PALLOT = FYSIIKKAA = EI
        // RÄJÄHDYS 
        //TAUSTA 
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
        Mouse.ListenOn(kohta1, MouseButton.Left, ButtonState.Pressed, Startpainettu, "Start the game");
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
        
    }
    protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
        base.Update(gameTime);
        if (!pelikaynnissa)
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
        pelikaynnissa = true;
        ClearGameObjects();
        pelaa();
        Level.Background.CreateStars();
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
        Camera.Follow(matta);
    }
}