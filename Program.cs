using static SDL2.SDL;
using System.Runtime.InteropServices;


namespace MiyooExample
{
    public class Program
    {
        public static readonly int SCREEN_WIDTH = 640;
        public static readonly int SCREEN_HEIGHT = 480;
        public static readonly int BPP = 32;
        private static IntPtr _Window = IntPtr.Zero;
        internal static IntPtr Renderer;
        internal static IntPtr screen_ptr;
        private static IntPtr texture;
        internal static SDL_Rect clipRect;
        private static SDL_Surface screen;
        private static byte cnt;
        public static void Main()
        {
            CheckSDLErr(SDL_Init(SDL_INIT_VIDEO));
            _Window = SDL_CreateWindow("main", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH, SCREEN_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN);
            Renderer = SDL_CreateRenderer(_Window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            screen_ptr = SDL_CreateRGBSurface(0, SCREEN_WIDTH, SCREEN_HEIGHT, BPP, 0, 0, 0, 0);
            screen = Marshal.PtrToStructure<SDL_Surface>(screen_ptr);
            texture = SDL_CreateTexture(Renderer, SDL_PIXELFORMAT_ARGB8888, (int)SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, SCREEN_WIDTH, SCREEN_HEIGHT);
            SDL_GetClipRect(screen_ptr, out clipRect);


            cnt = 0x00;
            bool quit = false;
            while (quit != true) 
            {
                RenderFrame();
                while (SDL_PollEvent(out SDL_Event e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;

                        case SDL_EventType.SDL_KEYUP:
                            switch (e.key.keysym.sym)
                            {
                                case SDL_Keycode.SDLK_ESCAPE:
                                    quit = true;
                                    break;
                            }
                            break;
                    }
                }
            }

            SDL_FreeSurface(screen_ptr);
            SDL_DestroyTexture(texture);
            SDL_DestroyRenderer(Renderer);
            SDL_DestroyWindow(_Window);
            SDL_Quit();
        }

        private static void RenderFrame()
        {
            if (cnt < 0xFE)
            {
                cnt += 1;
                CheckSDLErr(SDL_FillRect(screen_ptr, ref clipRect, SDL_MapRGB(screen.format, (byte)(0xFF - cnt / 2), cnt, (byte)(0xFF - cnt))));
                CheckSDLErr(SDL_UpdateTexture(texture, IntPtr.Zero, screen.pixels, screen.pitch));
                CheckSDLErr(SDL_RenderCopy(Renderer, texture, ref clipRect, IntPtr.Zero)); //Draw texture
                SDL_RenderPresent(Renderer); //Render
                CheckSDLErr(SDL_RenderClear(Renderer)); //Clear 
            }
        }

        public static void CheckSDLErr(int err)
        {
            if (err != 0)
            {
                Console.WriteLine("SDL Error Occured: "+SDL_GetError());
            }
        }
    }
}
