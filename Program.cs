using System.Runtime.InteropServices;
using SDL2;

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
        internal static SDL.SDL_Rect clipRect;
        private static IntPtr texture;
        private static SDL.SDL_Surface screen;
        private static IntPtr font;
        private static IntPtr image_ptr;

        private static byte cnt;
        public static void Main()
        {
            CheckSDLErr(SDL.SDL_Init(SDL.SDL_INIT_VIDEO));
            CheckTTFErr(SDL_ttf.TTF_Init());

            font = SDL_ttf.TTF_OpenFont("res/Capriola-Regular.ttf", 48);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load Capriola-Regular.ttf");
            }

            _Window = SDL.SDL_CreateWindow("main", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            Renderer = SDL.SDL_CreateRenderer(_Window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            screen_ptr = SDL.SDL_CreateRGBSurface(0, SCREEN_WIDTH, SCREEN_HEIGHT, BPP, 0, 0, 0, 0);
            screen = Marshal.PtrToStructure<SDL.SDL_Surface>(screen_ptr);
            texture = SDL.SDL_CreateTexture(Renderer, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, SCREEN_WIDTH, SCREEN_HEIGHT);
            SDL.SDL_GetClipRect(screen_ptr, out clipRect);           

            cnt = 0x00;

            image_ptr = SDL_image.IMG_Load("res/image.png");
            if (image_ptr == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load image.png");
            }

            bool quit = false;
            while (quit != true) 
            {
                RenderFrame();
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;

                        case SDL.SDL_EventType.SDL_KEYUP:
                            switch (e.key.keysym.sym)
                            {
                                case SDL.SDL_Keycode.SDLK_ESCAPE:
                                    quit = true;
                                    break;
                            }
                            break;
                    }
                }
            }

            SDL.SDL_FreeSurface(screen_ptr);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_DestroyRenderer(Renderer);
            SDL.SDL_DestroyWindow(_Window);
            SDL_ttf.TTF_Quit();
            SDL.SDL_Quit();
        }

        private static void RenderFrame()
        {
            if (cnt < 0xFF)
            {
                cnt += 1;
                CheckSDLErr(SDL.SDL_FillRect(screen_ptr, ref clipRect, SDL.SDL_MapRGB(screen.format, (byte)(0xFF - cnt / 2), cnt, (byte)(0xFF - cnt))));

                IntPtr textSurface_ptr = SDL_ttf.TTF_RenderText_Blended(font, "Example", new SDL.SDL_Color() { r = (byte)( -1 * (0xFF - cnt / 2)), g = (byte)(-1 * cnt), b = (byte)(-1 * (0xFF + cnt / 2)) });
                SDL.SDL_Surface textSurface = Marshal.PtrToStructure<SDL.SDL_Surface>(textSurface_ptr);
                SDL.SDL_Rect textSurfaceRect = new() { x = (SCREEN_WIDTH - textSurface.w) / 2, y = SCREEN_HEIGHT / 2 - 120 };
                CheckSDLErr(SDL.SDL_BlitSurface(textSurface_ptr, IntPtr.Zero, screen_ptr, ref textSurfaceRect));
                SDL.SDL_FreeSurface(textSurface_ptr);

                SDL.SDL_Surface image = Marshal.PtrToStructure<SDL.SDL_Surface>(image_ptr);
                SDL.SDL_Rect imageRect = new() { x = (SCREEN_WIDTH - image.w) / 2, y = SCREEN_HEIGHT - (image.h + 20) };
                CheckSDLErr(SDL.SDL_BlitSurface(image_ptr, IntPtr.Zero, screen_ptr, ref imageRect));

                CheckSDLErr(SDL.SDL_UpdateTexture(texture, IntPtr.Zero, screen.pixels, screen.pitch));
                CheckSDLErr(SDL.SDL_RenderCopy(Renderer, texture, ref clipRect, IntPtr.Zero)); //Draw texture
                SDL.SDL_RenderPresent(Renderer); //Render
                CheckSDLErr(SDL.SDL_RenderClear(Renderer)); //Clear 
            }
        }

        public static void CheckSDLErr(int err)
        {
            if (err < 0)
            {
                Console.WriteLine("SDL Error Occured: " + SDL.SDL_GetError());
            }
        }

        private static void CheckTTFErr(int err)
        {
            if (err < 0)
            {
                Console.WriteLine("SDL_ttf Error Occured: " + SDL_ttf.TTF_GetError());
            }
        }
    }
}
