using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class crashfeator1 : PhysicsGame
{
    PhysicsObject pallo1;
    public override void Begin()
    {
        
        // TO DO LIST!!!
        // PELAAJA HIIRI
        // SATUNNAINEN PALLOPOLKU
        // KERÄTTÄVÄT PALLOT = FYSIIKKAA = EI
        // RÄJÄHDYS 
        //TAUSTA 
        //LIIKE TUNNISTUS



        pelaa();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void valikko()
    {

    }
    void pelaa()
    {
        pallo1 = new PhysicsObject(30, 30);
        pallo1.Color = Color.Red;
        Add(pallo1);
    }
    protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
        base.Update(gameTime);
        if (pallo1 != null)
        {
            pallo1.Position = Mouse.PositionOnScreen;
        }

    }
    void generoi()
    {
    }
}
