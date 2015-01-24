using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class crashfeator1 : PhysicsGame
{
    public override void Begin()
    {
        
        // TO DO LIST!!!
        // PELAAJA HIIRI
        // SATUNNAINEN PALLOPOLKU
        // KERÄTTÄVÄT PALLOT = FYSIIKKAA = EI
        // RÄJÄHDYS 
        //TAUSTA 
        //LIIKE TUNNISTUS
        
        
        
        valikko();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void valikko()
    {
    }
    void pelaa()
    {
    }
}
