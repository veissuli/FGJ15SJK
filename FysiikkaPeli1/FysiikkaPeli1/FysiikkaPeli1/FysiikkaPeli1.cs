using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class FysiikkaPeli1 : PhysicsGame
{
    Timer aikalaskuri;
    PhysicsObject pelaaja;
    GameObject pelaajanNahka;
    PhysicsObject sininenbase;
    PhysicsObject punainenbase;
    Image allas1 = LoadImage("kakkalaava");
    Image pelaajankuva = LoadImage("akp");
    public override void Begin()
    {
        luotaso();
        liikuta();
        /* luopelaaja();
        luoallas();
        
        luosininenbase();
        luopunainenbase();*/
        // TODO: Kirjoita ohjelmakoodisi tähän

        IsMouseVisible = true;
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

    }
    void luopelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja = new PhysicsObject(110, 110, Shape.Circle);
        pelaaja.CanRotate = false;
        pelaaja.IsVisible = false;
        //pelaaja.Image = kakka;
        //pelaaja.Color = Color.Blue;
        pelaaja.Position = paikka;
        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, liikuta, "");
            
        pelaajanNahka = new GameObject(100,200);
        pelaajanNahka.Y = 85;
        pelaajanNahka.IsVisible = true;
        pelaajanNahka.Image = pelaajankuva;
        pelaaja.Add(pelaajanNahka);
        Camera.Follow(pelaaja);
            Add (pelaaja);
    }

    void luoallas(Vector paikka, double leveys, double korkeus)
    {
       PhysicsObject allas = new PhysicsObject(250, 600); 
        allas.MakeStatic();
        allas.Position = paikka;
        allas.Color = Color.Orange;
        allas.IgnoresCollisionWith(pelaaja);
        allas.Image = allas1;
        Angle laava = Angle.FromDegrees(-45);
        allas.Angle = laava;
        Add (allas);
    }
    void liikuta()
    {
        if (pelaaja != null)
        {
            pelaaja.MoveTo(Mouse.PositionOnWorld, 500);
        }
    }
    protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
   {
        base.Update(gameTime);
     
         if(pelaajanNahka != null)
         {

             double X = pelaaja.Position.X;
             double Y = pelaaja.Position.Y;
             Vector s = new Vector(X, Y-90);
             //var = new Vector(pelaaja.Position)
            

         }
   }


    void luosininenbase(Vector paikka, double leveys, double korkeus)
    {
        sininenbase = new PhysicsObject(300, 300);
        sininenbase.Position = paikka;
        sininenbase.Color = Color.DarkBlue;
        sininenbase.MakeStatic();
        sininenbase.Shape = Shape.Circle;
        //sininenbase.Position = new Vector(10, - 10);
        Add(sininenbase);

    }


    void luopunainenbase(Vector paikka, double leveys, double korkeus)
    {
       punainenbase = new PhysicsObject(300, 300);
       punainenbase.Position = paikka;
       punainenbase.Shape = Shape.Circle;
        punainenbase.Color = Color.Red;
        punainenbase.MakeStatic();

        Add(punainenbase);

    }

    void luotaso()
    {
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("kartta");

        //2. Kerrotaan mitä aliohjelmaa kutsutaan, kun tietyn värinen pikseli tulee vastaan kuvatiedostossa.
        ruudut.SetTileMethod(Color.Black, luopelaaja);
        ruudut.SetTileMethod(Color.Red, luopunainenbase);
        ruudut.SetTileMethod(Color.Blue, luosininenbase);
        ruudut.SetTileMethod(Color.Orange, luoallas);
        
        //3. Execute luo kentän
        //   Parametreina leveys ja korkeus
        ruudut.Execute(400, 400);

    }

    void luominionit()
    {
        aikalaskuri = new Timer();
        aikalaskuri.Interval = 28.0;
        aikalaskuri.Timeout += spawnaaalto;
        aikalaskuri.Start();
        

    }

    void spawnaaalto()
    {
        for (int i = 0; i >= 7; i++)
        {
            PhysicsObject minion = new PhysicsObject(65, 65);
            minion.Position = punainenbase.Position;
            minion.IgnoresCollisionWith(punainenbase); 
            Add(minion);
            
            PhysicsObject minion2 = new PhysicsObject(65, 65);
            minion2.Position = sininenbase.Position;
            minion2.IgnoresCollisionWith(sininenbase);
            Add(minion2);
        
        }

    }
}
