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
    public override void Begin()
    {

        
        
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

}