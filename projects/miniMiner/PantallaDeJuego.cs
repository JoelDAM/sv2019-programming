﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MiniMiner
{
    public class PantallaDeJuego
    {
        Personaje personaje;
        Enemigo enemigo;
        Nivel01 nivel01;
        Marcador marcador;

        public bool Terminado { get; set; }

        public PantallaDeJuego(int maxX, int maxY)
        {
            // ...
        }

        public void CargarContenidos(ContentManager Content)
        {
            personaje = new Personaje(Content);
            enemigo = new Enemigo(Content);
            nivel01 = new Nivel01(Content);
            marcador = new Marcador(Content);

            Reiniciar();
        }

        public void Reiniciar()
        {
            Terminado = false;
            nivel01.Reiniciar();
            personaje.Vidas = 3;
            marcador.SetVidas(personaje.Vidas);
            marcador.SetNombreNivel(nivel01.GetNombre());
            marcador.ReiniciarPuntos();
        }

        public void Actualizar(GameTime gameTime)
        {
            MoverElementos(gameTime);
            ComprobarColisiones();
            ComprobarEntrada(gameTime);
        }

        protected void MoverElementos(GameTime gameTime)
        {
            enemigo.Mover(gameTime);
        }

        protected void ComprobarEntrada(GameTime gameTime)
        {
            var estadoTeclado = Keyboard.GetState();
            var estadoGamePad = GamePad.GetState(PlayerIndex.One);

            if (estadoGamePad.Buttons.Back == ButtonState.Pressed
                    || estadoTeclado.IsKeyDown(Keys.Escape))
                Terminado = true;

            // ...
            if (estadoTeclado.IsKeyDown(Keys.Right)
                || estadoGamePad.DPad.Right > 0
                || estadoGamePad.ThumbSticks.Left.X > 0)
            {
                personaje.MoverDerecha(gameTime, nivel01);
            }

            if (estadoTeclado.IsKeyDown(Keys.Left)
                || estadoGamePad.DPad.Left > 0
                || estadoGamePad.ThumbSticks.Left.X < 0)
            {
                personaje.MoverIzquierda(gameTime, nivel01);
            }

            if (estadoTeclado.IsKeyDown(Keys.Up)
                || estadoGamePad.DPad.Up > 0
                || estadoGamePad.ThumbSticks.Left.Y > 0)
            {
                personaje.MoverArriba(gameTime, nivel01);
            }

            if (estadoTeclado.IsKeyDown(Keys.Down)
                || estadoGamePad.DPad.Down > 0
                || estadoGamePad.ThumbSticks.Left.Y < 0)
            {
                personaje.MoverAbajo(gameTime, nivel01);
            }
        }

        protected void ComprobarColisiones()
        {
            int puntosEnEsteFotograma = nivel01.ComprobarPuntosPorItems(personaje);
            if (puntosEnEsteFotograma > 0)
            {
                marcador.IncrementarPuntos(puntosEnEsteFotograma);
            }

            if ((personaje.ColisionaCon(enemigo)) ||
                (nivel01.HayColisionesMortales(personaje)))
            {
                personaje.Vidas--;
                personaje.MoverAPosicionInicial();
                marcador.SetVidas(personaje.Vidas);

                if (personaje.Vidas <= 0)
                {
                    Terminado = true;
                }
            }
        }

        public void Dibujar(GameTime gameTime, SpriteBatch spriteBatch)
        {
            nivel01.Dibujar(spriteBatch);
            personaje.Dibujar(spriteBatch);
            enemigo.Dibujar(spriteBatch);
            marcador.Dibujar(spriteBatch);
        }
    }
}
