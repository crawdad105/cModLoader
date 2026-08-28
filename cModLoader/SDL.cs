using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1591 // removes missing XML message
namespace cModLoader.FNA
{
    /// <summary>
    /// This was taken from the FNA.dll. its license i think allows me to just copy there code to interact with SDL.
    /// <para>https://opensource.org/license/ms-pl-html</para>
    /// <para>https://github.com/FNA-XNA/FNA/blob/master/licenses/LICENSE</para>
    /// </summary>
    public static class FNA_SDL3
    {
        public struct SDLBool
        {
            private readonly byte value;

            internal const byte FALSE_VALUE = 0;

            internal const byte TRUE_VALUE = 1;

            internal SDLBool(byte value)
            {
                this.value = value;
            }

            public static implicit operator bool(SDLBool b)
            {
                return b.value != 0;
            }

            public static implicit operator SDLBool(bool b)
            {
                return new SDLBool(b ? ((byte)1) : ((byte)0));
            }

            public bool Equals(SDLBool other)
            {
                return other.value == value;
            }

            public override bool Equals(object rhs)
            {
                if (rhs is bool)
                {
                    return Equals((bool)rhs);
                }
                if (rhs is SDLBool)
                {
                    return Equals((SDLBool)rhs);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }

        public enum SDL_AssertState
        {
            SDL_ASSERTION_RETRY,
            SDL_ASSERTION_BREAK,
            SDL_ASSERTION_ABORT,
            SDL_ASSERTION_IGNORE,
            SDL_ASSERTION_ALWAYS_IGNORE
        }

        public struct SDL_AssertData
        {
            public SDLBool always_ignore;

            public uint trigger_count;

            public unsafe byte* condition;

            public unsafe byte* filename;

            public int linenum;

            public unsafe byte* function;

            public unsafe SDL_AssertData* next;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate SDL_AssertState SDL_AssertionHandler(SDL_AssertData* data, IntPtr userdata);

        public enum SDL_AsyncIOTaskType
        {
            SDL_ASYNCIO_TASK_READ,
            SDL_ASYNCIO_TASK_WRITE,
            SDL_ASYNCIO_TASK_CLOSE
        }

        public enum SDL_AsyncIOResult
        {
            SDL_ASYNCIO_COMPLETE,
            SDL_ASYNCIO_FAILURE,
            SDL_ASYNCIO_CANCELED
        }

        public struct SDL_AsyncIOOutcome
        {
            public IntPtr asyncio;

            public SDL_AsyncIOTaskType type;

            public SDL_AsyncIOResult result;

            public IntPtr buffer;

            public ulong offset;

            public ulong bytes_requested;

            public ulong bytes_transferred;

            public IntPtr userdata;
        }

        public struct SDL_AtomicInt
        {
            public int value;
        }

        public struct SDL_AtomicU32
        {
            public uint value;
        }

        public enum SDL_PropertyType
        {
            SDL_PROPERTY_TYPE_INVALID,
            SDL_PROPERTY_TYPE_POINTER,
            SDL_PROPERTY_TYPE_STRING,
            SDL_PROPERTY_TYPE_NUMBER,
            SDL_PROPERTY_TYPE_FLOAT,
            SDL_PROPERTY_TYPE_BOOLEAN
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_CleanupPropertyCallback(IntPtr userdata, IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void SDL_EnumeratePropertiesCallback(IntPtr userdata, uint props, byte* name);

        public enum SDL_ThreadPriority
        {
            SDL_THREAD_PRIORITY_LOW,
            SDL_THREAD_PRIORITY_NORMAL,
            SDL_THREAD_PRIORITY_HIGH,
            SDL_THREAD_PRIORITY_TIME_CRITICAL
        }

        public enum SDL_ThreadState
        {
            SDL_THREAD_UNKNOWN,
            SDL_THREAD_ALIVE,
            SDL_THREAD_DETACHED,
            SDL_THREAD_COMPLETE
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDL_ThreadFunction(IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_TLSDestructorCallback(IntPtr value);

        public enum SDL_InitStatus
        {
            SDL_INIT_STATUS_UNINITIALIZED,
            SDL_INIT_STATUS_INITIALIZING,
            SDL_INIT_STATUS_INITIALIZED,
            SDL_INIT_STATUS_UNINITIALIZING
        }

        public struct SDL_InitState
        {
            public SDL_AtomicInt status;

            public ulong thread;

            public IntPtr reserved;
        }

        public enum SDL_IOStatus
        {
            SDL_IO_STATUS_READY,
            SDL_IO_STATUS_ERROR,
            SDL_IO_STATUS_EOF,
            SDL_IO_STATUS_NOT_READY,
            SDL_IO_STATUS_READONLY,
            SDL_IO_STATUS_WRITEONLY
        }

        public enum SDL_IOWhence
        {
            SDL_IO_SEEK_SET,
            SDL_IO_SEEK_CUR,
            SDL_IO_SEEK_END
        }

        public struct SDL_IOStreamInterface
        {
            public uint version;

            public IntPtr size;

            public IntPtr seek;

            public IntPtr read;

            public IntPtr write;

            public IntPtr flush;

            public IntPtr close;
        }

        public enum SDL_AudioFormat
        {
            SDL_AUDIO_UNKNOWN = 0,
            SDL_AUDIO_U8 = 8,
            SDL_AUDIO_S8 = 32776,
            SDL_AUDIO_S16LE = 32784,
            SDL_AUDIO_S16BE = 36880,
            SDL_AUDIO_S32LE = 32800,
            SDL_AUDIO_S32BE = 36896,
            SDL_AUDIO_F32LE = 33056,
            SDL_AUDIO_F32BE = 37152,
            SDL_AUDIO_S16 = 32784,
            SDL_AUDIO_S32 = 32800,
            SDL_AUDIO_F32 = 33056
        }

        public struct SDL_AudioSpec
        {
            public SDL_AudioFormat format;

            public int channels;

            public int freq;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_AudioStreamDataCompleteCallback(IntPtr userdata, IntPtr buf, int buflen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_AudioStreamCallback(IntPtr userdata, IntPtr stream, int additional_amount, int total_amount);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void SDL_AudioPostmixCallback(IntPtr userdata, SDL_AudioSpec* spec, float* buffer, int buflen);

        public enum SDL_BlendOperation
        {
            SDL_BLENDOPERATION_ADD = 1,
            SDL_BLENDOPERATION_SUBTRACT,
            SDL_BLENDOPERATION_REV_SUBTRACT,
            SDL_BLENDOPERATION_MINIMUM,
            SDL_BLENDOPERATION_MAXIMUM
        }

        public enum SDL_BlendFactor
        {
            SDL_BLENDFACTOR_ZERO = 1,
            SDL_BLENDFACTOR_ONE,
            SDL_BLENDFACTOR_SRC_COLOR,
            SDL_BLENDFACTOR_ONE_MINUS_SRC_COLOR,
            SDL_BLENDFACTOR_SRC_ALPHA,
            SDL_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
            SDL_BLENDFACTOR_DST_COLOR,
            SDL_BLENDFACTOR_ONE_MINUS_DST_COLOR,
            SDL_BLENDFACTOR_DST_ALPHA,
            SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA
        }

        public enum SDL_PixelType
        {
            SDL_PIXELTYPE_UNKNOWN,
            SDL_PIXELTYPE_INDEX1,
            SDL_PIXELTYPE_INDEX4,
            SDL_PIXELTYPE_INDEX8,
            SDL_PIXELTYPE_PACKED8,
            SDL_PIXELTYPE_PACKED16,
            SDL_PIXELTYPE_PACKED32,
            SDL_PIXELTYPE_ARRAYU8,
            SDL_PIXELTYPE_ARRAYU16,
            SDL_PIXELTYPE_ARRAYU32,
            SDL_PIXELTYPE_ARRAYF16,
            SDL_PIXELTYPE_ARRAYF32,
            SDL_PIXELTYPE_INDEX2
        }

        public enum SDL_BitmapOrder
        {
            SDL_BITMAPORDER_NONE,
            SDL_BITMAPORDER_4321,
            SDL_BITMAPORDER_1234
        }

        public enum SDL_PackedOrder
        {
            SDL_PACKEDORDER_NONE,
            SDL_PACKEDORDER_XRGB,
            SDL_PACKEDORDER_RGBX,
            SDL_PACKEDORDER_ARGB,
            SDL_PACKEDORDER_RGBA,
            SDL_PACKEDORDER_XBGR,
            SDL_PACKEDORDER_BGRX,
            SDL_PACKEDORDER_ABGR,
            SDL_PACKEDORDER_BGRA
        }

        public enum SDL_ArrayOrder
        {
            SDL_ARRAYORDER_NONE,
            SDL_ARRAYORDER_RGB,
            SDL_ARRAYORDER_RGBA,
            SDL_ARRAYORDER_ARGB,
            SDL_ARRAYORDER_BGR,
            SDL_ARRAYORDER_BGRA,
            SDL_ARRAYORDER_ABGR
        }

        public enum SDL_PackedLayout
        {
            SDL_PACKEDLAYOUT_NONE,
            SDL_PACKEDLAYOUT_332,
            SDL_PACKEDLAYOUT_4444,
            SDL_PACKEDLAYOUT_1555,
            SDL_PACKEDLAYOUT_5551,
            SDL_PACKEDLAYOUT_565,
            SDL_PACKEDLAYOUT_8888,
            SDL_PACKEDLAYOUT_2101010,
            SDL_PACKEDLAYOUT_1010102
        }

        public enum SDL_PixelFormat
        {
            SDL_PIXELFORMAT_UNKNOWN = 0,
            SDL_PIXELFORMAT_INDEX1LSB = 286261504,
            SDL_PIXELFORMAT_INDEX1MSB = 287310080,
            SDL_PIXELFORMAT_INDEX2LSB = 470811136,
            SDL_PIXELFORMAT_INDEX2MSB = 471859712,
            SDL_PIXELFORMAT_INDEX4LSB = 303039488,
            SDL_PIXELFORMAT_INDEX4MSB = 304088064,
            SDL_PIXELFORMAT_INDEX8 = 318769153,
            SDL_PIXELFORMAT_RGB332 = 336660481,
            SDL_PIXELFORMAT_XRGB4444 = 353504258,
            SDL_PIXELFORMAT_XBGR4444 = 357698562,
            SDL_PIXELFORMAT_XRGB1555 = 353570562,
            SDL_PIXELFORMAT_XBGR1555 = 357764866,
            SDL_PIXELFORMAT_ARGB4444 = 355602434,
            SDL_PIXELFORMAT_RGBA4444 = 356651010,
            SDL_PIXELFORMAT_ABGR4444 = 359796738,
            SDL_PIXELFORMAT_BGRA4444 = 360845314,
            SDL_PIXELFORMAT_ARGB1555 = 355667970,
            SDL_PIXELFORMAT_RGBA5551 = 356782082,
            SDL_PIXELFORMAT_ABGR1555 = 359862274,
            SDL_PIXELFORMAT_BGRA5551 = 360976386,
            SDL_PIXELFORMAT_RGB565 = 353701890,
            SDL_PIXELFORMAT_BGR565 = 357896194,
            SDL_PIXELFORMAT_RGB24 = 386930691,
            SDL_PIXELFORMAT_BGR24 = 390076419,
            SDL_PIXELFORMAT_XRGB8888 = 370546692,
            SDL_PIXELFORMAT_RGBX8888 = 371595268,
            SDL_PIXELFORMAT_XBGR8888 = 374740996,
            SDL_PIXELFORMAT_BGRX8888 = 375789572,
            SDL_PIXELFORMAT_ARGB8888 = 372645892,
            SDL_PIXELFORMAT_RGBA8888 = 373694468,
            SDL_PIXELFORMAT_ABGR8888 = 376840196,
            SDL_PIXELFORMAT_BGRA8888 = 377888772,
            SDL_PIXELFORMAT_XRGB2101010 = 370614276,
            SDL_PIXELFORMAT_XBGR2101010 = 374808580,
            SDL_PIXELFORMAT_ARGB2101010 = 372711428,
            SDL_PIXELFORMAT_ABGR2101010 = 376905732,
            SDL_PIXELFORMAT_RGB48 = 403714054,
            SDL_PIXELFORMAT_BGR48 = 406859782,
            SDL_PIXELFORMAT_RGBA64 = 404766728,
            SDL_PIXELFORMAT_ARGB64 = 405815304,
            SDL_PIXELFORMAT_BGRA64 = 407912456,
            SDL_PIXELFORMAT_ABGR64 = 408961032,
            SDL_PIXELFORMAT_RGB48_FLOAT = 437268486,
            SDL_PIXELFORMAT_BGR48_FLOAT = 440414214,
            SDL_PIXELFORMAT_RGBA64_FLOAT = 438321160,
            SDL_PIXELFORMAT_ARGB64_FLOAT = 439369736,
            SDL_PIXELFORMAT_BGRA64_FLOAT = 441466888,
            SDL_PIXELFORMAT_ABGR64_FLOAT = 442515464,
            SDL_PIXELFORMAT_RGB96_FLOAT = 454057996,
            SDL_PIXELFORMAT_BGR96_FLOAT = 457203724,
            SDL_PIXELFORMAT_RGBA128_FLOAT = 455114768,
            SDL_PIXELFORMAT_ARGB128_FLOAT = 456163344,
            SDL_PIXELFORMAT_BGRA128_FLOAT = 458260496,
            SDL_PIXELFORMAT_ABGR128_FLOAT = 459309072,
            SDL_PIXELFORMAT_YV12 = 842094169,
            SDL_PIXELFORMAT_IYUV = 1448433993,
            SDL_PIXELFORMAT_YUY2 = 844715353,
            SDL_PIXELFORMAT_UYVY = 1498831189,
            SDL_PIXELFORMAT_YVYU = 1431918169,
            SDL_PIXELFORMAT_NV12 = 842094158,
            SDL_PIXELFORMAT_NV21 = 825382478,
            SDL_PIXELFORMAT_P010 = 808530000,
            SDL_PIXELFORMAT_EXTERNAL_OES = 542328143,
            SDL_PIXELFORMAT_MJPG = 1196444237,
            SDL_PIXELFORMAT_RGBA32 = 376840196,
            SDL_PIXELFORMAT_ARGB32 = 377888772,
            SDL_PIXELFORMAT_BGRA32 = 372645892,
            SDL_PIXELFORMAT_ABGR32 = 373694468,
            SDL_PIXELFORMAT_RGBX32 = 374740996,
            SDL_PIXELFORMAT_XRGB32 = 375789572,
            SDL_PIXELFORMAT_BGRX32 = 370546692,
            SDL_PIXELFORMAT_XBGR32 = 371595268
        }

        public enum SDL_ColorType
        {
            SDL_COLOR_TYPE_UNKNOWN,
            SDL_COLOR_TYPE_RGB,
            SDL_COLOR_TYPE_YCBCR
        }

        public enum SDL_ColorRange
        {
            SDL_COLOR_RANGE_UNKNOWN,
            SDL_COLOR_RANGE_LIMITED,
            SDL_COLOR_RANGE_FULL
        }

        public enum SDL_ColorPrimaries
        {
            SDL_COLOR_PRIMARIES_UNKNOWN = 0,
            SDL_COLOR_PRIMARIES_BT709 = 1,
            SDL_COLOR_PRIMARIES_UNSPECIFIED = 2,
            SDL_COLOR_PRIMARIES_BT470M = 4,
            SDL_COLOR_PRIMARIES_BT470BG = 5,
            SDL_COLOR_PRIMARIES_BT601 = 6,
            SDL_COLOR_PRIMARIES_SMPTE240 = 7,
            SDL_COLOR_PRIMARIES_GENERIC_FILM = 8,
            SDL_COLOR_PRIMARIES_BT2020 = 9,
            SDL_COLOR_PRIMARIES_XYZ = 10,
            SDL_COLOR_PRIMARIES_SMPTE431 = 11,
            SDL_COLOR_PRIMARIES_SMPTE432 = 12,
            SDL_COLOR_PRIMARIES_EBU3213 = 22,
            SDL_COLOR_PRIMARIES_CUSTOM = 31
        }

        public enum SDL_TransferCharacteristics
        {
            SDL_TRANSFER_CHARACTERISTICS_UNKNOWN = 0,
            SDL_TRANSFER_CHARACTERISTICS_BT709 = 1,
            SDL_TRANSFER_CHARACTERISTICS_UNSPECIFIED = 2,
            SDL_TRANSFER_CHARACTERISTICS_GAMMA22 = 4,
            SDL_TRANSFER_CHARACTERISTICS_GAMMA28 = 5,
            SDL_TRANSFER_CHARACTERISTICS_BT601 = 6,
            SDL_TRANSFER_CHARACTERISTICS_SMPTE240 = 7,
            SDL_TRANSFER_CHARACTERISTICS_LINEAR = 8,
            SDL_TRANSFER_CHARACTERISTICS_LOG100 = 9,
            SDL_TRANSFER_CHARACTERISTICS_LOG100_SQRT10 = 10,
            SDL_TRANSFER_CHARACTERISTICS_IEC61966 = 11,
            SDL_TRANSFER_CHARACTERISTICS_BT1361 = 12,
            SDL_TRANSFER_CHARACTERISTICS_SRGB = 13,
            SDL_TRANSFER_CHARACTERISTICS_BT2020_10BIT = 14,
            SDL_TRANSFER_CHARACTERISTICS_BT2020_12BIT = 15,
            SDL_TRANSFER_CHARACTERISTICS_PQ = 16,
            SDL_TRANSFER_CHARACTERISTICS_SMPTE428 = 17,
            SDL_TRANSFER_CHARACTERISTICS_HLG = 18,
            SDL_TRANSFER_CHARACTERISTICS_CUSTOM = 31
        }

        public enum SDL_MatrixCoefficients
        {
            SDL_MATRIX_COEFFICIENTS_IDENTITY = 0,
            SDL_MATRIX_COEFFICIENTS_BT709 = 1,
            SDL_MATRIX_COEFFICIENTS_UNSPECIFIED = 2,
            SDL_MATRIX_COEFFICIENTS_FCC = 4,
            SDL_MATRIX_COEFFICIENTS_BT470BG = 5,
            SDL_MATRIX_COEFFICIENTS_BT601 = 6,
            SDL_MATRIX_COEFFICIENTS_SMPTE240 = 7,
            SDL_MATRIX_COEFFICIENTS_YCGCO = 8,
            SDL_MATRIX_COEFFICIENTS_BT2020_NCL = 9,
            SDL_MATRIX_COEFFICIENTS_BT2020_CL = 10,
            SDL_MATRIX_COEFFICIENTS_SMPTE2085 = 11,
            SDL_MATRIX_COEFFICIENTS_CHROMA_DERIVED_NCL = 12,
            SDL_MATRIX_COEFFICIENTS_CHROMA_DERIVED_CL = 13,
            SDL_MATRIX_COEFFICIENTS_ICTCP = 14,
            SDL_MATRIX_COEFFICIENTS_CUSTOM = 31
        }

        public enum SDL_ChromaLocation
        {
            SDL_CHROMA_LOCATION_NONE,
            SDL_CHROMA_LOCATION_LEFT,
            SDL_CHROMA_LOCATION_CENTER,
            SDL_CHROMA_LOCATION_TOPLEFT
        }

        public enum SDL_Colorspace
        {
            SDL_COLORSPACE_UNKNOWN = 0,
            SDL_COLORSPACE_SRGB = 301991328,
            SDL_COLORSPACE_SRGB_LINEAR = 301991168,
            SDL_COLORSPACE_HDR10 = 301999616,
            SDL_COLORSPACE_JPEG = 570426566,
            SDL_COLORSPACE_BT601_LIMITED = 554703046,
            SDL_COLORSPACE_BT601_FULL = 571480262,
            SDL_COLORSPACE_BT709_LIMITED = 554697761,
            SDL_COLORSPACE_BT709_FULL = 571474977,
            SDL_COLORSPACE_BT2020_LIMITED = 554706441,
            SDL_COLORSPACE_BT2020_FULL = 571483657,
            SDL_COLORSPACE_RGB_DEFAULT = 301991328,
            SDL_COLORSPACE_YUV_DEFAULT = 554703046
        }

        public struct SDL_Color
        {
            public byte r;

            public byte g;

            public byte b;

            public byte a;
        }

        public struct SDL_FColor
        {
            public float r;

            public float g;

            public float b;

            public float a;
        }

        public struct SDL_Palette
        {
            public int ncolors;

            public unsafe SDL_Color* colors;

            public uint version;

            public int refcount;
        }

        public struct SDL_PixelFormatDetails
        {
            public SDL_PixelFormat format;

            public byte bits_per_pixel;

            public byte bytes_per_pixel;

            public unsafe fixed byte padding[2];

            public uint Rmask;

            public uint Gmask;

            public uint Bmask;

            public uint Amask;

            public byte Rbits;

            public byte Gbits;

            public byte Bbits;

            public byte Abits;

            public byte Rshift;

            public byte Gshift;

            public byte Bshift;

            public byte Ashift;
        }

        public struct SDL_Point
        {
            public int x;

            public int y;
        }

        public struct SDL_FPoint
        {
            public float x;

            public float y;
        }

        public struct SDL_Rect
        {
            public int x;

            public int y;

            public int w;

            public int h;
        }

        public struct SDL_FRect
        {
            public float x;

            public float y;

            public float w;

            public float h;
        }

        [Flags]
        public enum SDL_SurfaceFlags : uint
        {
            SDL_SURFACE_PREALLOCATED = 1u,
            SDL_SURFACE_LOCK_NEEDED = 2u,
            SDL_SURFACE_LOCKED = 4u,
            SDL_SURFACE_SIMD_ALIGNED = 8u
        }

        public enum SDL_ScaleMode
        {
            SDL_SCALEMODE_INVALID = -1,
            SDL_SCALEMODE_NEAREST,
            SDL_SCALEMODE_LINEAR,
            SDL_SCALEMODE_PIXELART
        }

        public enum SDL_FlipMode
        {
            SDL_FLIP_NONE,
            SDL_FLIP_HORIZONTAL,
            SDL_FLIP_VERTICAL,
            SDL_FLIP_HORIZONTAL_AND_VERTICAL
        }

        public struct SDL_Surface
        {
            public SDL_SurfaceFlags flags;

            public SDL_PixelFormat format;

            public int w;

            public int h;

            public int pitch;

            public IntPtr pixels;

            public int refcount;

            public IntPtr reserved;
        }

        public struct SDL_CameraSpec
        {
            public SDL_PixelFormat format;

            public SDL_Colorspace colorspace;

            public int width;

            public int height;

            public int framerate_numerator;

            public int framerate_denominator;
        }

        public enum SDL_CameraPosition
        {
            SDL_CAMERA_POSITION_UNKNOWN,
            SDL_CAMERA_POSITION_FRONT_FACING,
            SDL_CAMERA_POSITION_BACK_FACING
        }

        public enum SDL_CameraPermissionState
        {
            SDL_CAMERA_PERMISSION_STATE_DENIED = -1,
            SDL_CAMERA_PERMISSION_STATE_PENDING,
            SDL_CAMERA_PERMISSION_STATE_APPROVED
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate IntPtr SDL_ClipboardDataCallback(IntPtr userdata, byte* mime_type, IntPtr size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_ClipboardCleanupCallback(IntPtr userdata);

        public enum SDL_SystemTheme
        {
            SDL_SYSTEM_THEME_UNKNOWN,
            SDL_SYSTEM_THEME_LIGHT,
            SDL_SYSTEM_THEME_DARK
        }

        public struct SDL_DisplayMode
        {
            public uint displayID;

            public SDL_PixelFormat format;

            public int w;

            public int h;

            public float pixel_density;

            public float refresh_rate;

            public int refresh_rate_numerator;

            public int refresh_rate_denominator;

            public IntPtr @internal;
        }

        public enum SDL_DisplayOrientation
        {
            SDL_ORIENTATION_UNKNOWN,
            SDL_ORIENTATION_LANDSCAPE,
            SDL_ORIENTATION_LANDSCAPE_FLIPPED,
            SDL_ORIENTATION_PORTRAIT,
            SDL_ORIENTATION_PORTRAIT_FLIPPED
        }

        [Flags]
        public enum SDL_WindowFlags : ulong
        {
            SDL_WINDOW_FULLSCREEN = 1uL,
            SDL_WINDOW_OPENGL = 2uL,
            SDL_WINDOW_OCCLUDED = 4uL,
            SDL_WINDOW_HIDDEN = 8uL,
            SDL_WINDOW_BORDERLESS = 0x10uL,
            SDL_WINDOW_RESIZABLE = 0x20uL,
            SDL_WINDOW_MINIMIZED = 0x40uL,
            SDL_WINDOW_MAXIMIZED = 0x80uL,
            SDL_WINDOW_MOUSE_GRABBED = 0x100uL,
            SDL_WINDOW_INPUT_FOCUS = 0x200uL,
            SDL_WINDOW_MOUSE_FOCUS = 0x400uL,
            SDL_WINDOW_EXTERNAL = 0x800uL,
            SDL_WINDOW_MODAL = 0x1000uL,
            SDL_WINDOW_HIGH_PIXEL_DENSITY = 0x2000uL,
            SDL_WINDOW_MOUSE_CAPTURE = 0x4000uL,
            SDL_WINDOW_MOUSE_RELATIVE_MODE = 0x8000uL,
            SDL_WINDOW_ALWAYS_ON_TOP = 0x10000uL,
            SDL_WINDOW_UTILITY = 0x20000uL,
            SDL_WINDOW_TOOLTIP = 0x40000uL,
            SDL_WINDOW_POPUP_MENU = 0x80000uL,
            SDL_WINDOW_KEYBOARD_GRABBED = 0x100000uL,
            SDL_WINDOW_VULKAN = 0x10000000uL,
            SDL_WINDOW_METAL = 0x20000000uL,
            SDL_WINDOW_TRANSPARENT = 0x40000000uL,
            SDL_WINDOW_NOT_FOCUSABLE = 0x80000000uL
        }

        public enum SDL_FlashOperation
        {
            SDL_FLASH_CANCEL,
            SDL_FLASH_BRIEFLY,
            SDL_FLASH_UNTIL_FOCUSED
        }

        public enum SDL_ProgressState
        {
            SDL_PROGRESS_STATE_INVALID = -1,
            SDL_PROGRESS_STATE_NONE,
            SDL_PROGRESS_STATE_INDETERMINATE,
            SDL_PROGRESS_STATE_NORMAL,
            SDL_PROGRESS_STATE_PAUSED,
            SDL_PROGRESS_STATE_ERROR
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDL_EGLAttribArrayCallback();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDL_EGLIntArrayCallback();

        public enum SDL_GLAttr
        {
            SDL_GL_RED_SIZE,
            SDL_GL_GREEN_SIZE,
            SDL_GL_BLUE_SIZE,
            SDL_GL_ALPHA_SIZE,
            SDL_GL_BUFFER_SIZE,
            SDL_GL_DOUBLEBUFFER,
            SDL_GL_DEPTH_SIZE,
            SDL_GL_STENCIL_SIZE,
            SDL_GL_ACCUM_RED_SIZE,
            SDL_GL_ACCUM_GREEN_SIZE,
            SDL_GL_ACCUM_BLUE_SIZE,
            SDL_GL_ACCUM_ALPHA_SIZE,
            SDL_GL_STEREO,
            SDL_GL_MULTISAMPLEBUFFERS,
            SDL_GL_MULTISAMPLESAMPLES,
            SDL_GL_ACCELERATED_VISUAL,
            SDL_GL_RETAINED_BACKING,
            SDL_GL_CONTEXT_MAJOR_VERSION,
            SDL_GL_CONTEXT_MINOR_VERSION,
            SDL_GL_CONTEXT_FLAGS,
            SDL_GL_CONTEXT_PROFILE_MASK,
            SDL_GL_SHARE_WITH_CURRENT_CONTEXT,
            SDL_GL_FRAMEBUFFER_SRGB_CAPABLE,
            SDL_GL_CONTEXT_RELEASE_BEHAVIOR,
            SDL_GL_CONTEXT_RESET_NOTIFICATION,
            SDL_GL_CONTEXT_NO_ERROR,
            SDL_GL_FLOATBUFFERS,
            SDL_GL_EGL_PLATFORM
        }

        public enum SDL_HitTestResult
        {
            SDL_HITTEST_NORMAL,
            SDL_HITTEST_DRAGGABLE,
            SDL_HITTEST_RESIZE_TOPLEFT,
            SDL_HITTEST_RESIZE_TOP,
            SDL_HITTEST_RESIZE_TOPRIGHT,
            SDL_HITTEST_RESIZE_RIGHT,
            SDL_HITTEST_RESIZE_BOTTOMRIGHT,
            SDL_HITTEST_RESIZE_BOTTOM,
            SDL_HITTEST_RESIZE_BOTTOMLEFT,
            SDL_HITTEST_RESIZE_LEFT
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate SDL_HitTestResult SDL_HitTest(IntPtr win, SDL_Point* area, IntPtr data);

        public struct SDL_DialogFileFilter
        {
            public unsafe byte* name;

            public unsafe byte* pattern;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_DialogFileCallback(IntPtr userdata, IntPtr filelist, int filter);

        public enum SDL_FileDialogType
        {
            SDL_FILEDIALOG_OPENFILE,
            SDL_FILEDIALOG_SAVEFILE,
            SDL_FILEDIALOG_OPENFOLDER
        }

        public struct SDL_GUID
        {
            public unsafe fixed byte data[16];
        }

        public enum SDL_PowerState
        {
            SDL_POWERSTATE_ERROR = -1,
            SDL_POWERSTATE_UNKNOWN,
            SDL_POWERSTATE_ON_BATTERY,
            SDL_POWERSTATE_NO_BATTERY,
            SDL_POWERSTATE_CHARGING,
            SDL_POWERSTATE_CHARGED
        }

        public enum SDL_SensorType
        {
            SDL_SENSOR_INVALID = -1,
            SDL_SENSOR_UNKNOWN,
            SDL_SENSOR_ACCEL,
            SDL_SENSOR_GYRO,
            SDL_SENSOR_ACCEL_L,
            SDL_SENSOR_GYRO_L,
            SDL_SENSOR_ACCEL_R,
            SDL_SENSOR_GYRO_R,
            SDL_SENSOR_COUNT
        }

        public enum SDL_JoystickType
        {
            SDL_JOYSTICK_TYPE_UNKNOWN,
            SDL_JOYSTICK_TYPE_GAMEPAD,
            SDL_JOYSTICK_TYPE_WHEEL,
            SDL_JOYSTICK_TYPE_ARCADE_STICK,
            SDL_JOYSTICK_TYPE_FLIGHT_STICK,
            SDL_JOYSTICK_TYPE_DANCE_PAD,
            SDL_JOYSTICK_TYPE_GUITAR,
            SDL_JOYSTICK_TYPE_DRUM_KIT,
            SDL_JOYSTICK_TYPE_ARCADE_PAD,
            SDL_JOYSTICK_TYPE_THROTTLE,
            SDL_JOYSTICK_TYPE_COUNT
        }

        public enum SDL_JoystickConnectionState
        {
            SDL_JOYSTICK_CONNECTION_INVALID = -1,
            SDL_JOYSTICK_CONNECTION_UNKNOWN,
            SDL_JOYSTICK_CONNECTION_WIRED,
            SDL_JOYSTICK_CONNECTION_WIRELESS
        }

        public struct SDL_VirtualJoystickTouchpadDesc
        {
            public ushort nfingers;

            public unsafe fixed ushort padding[3];
        }

        public struct SDL_VirtualJoystickSensorDesc
        {
            public SDL_SensorType type;

            public float rate;
        }

        public struct SDL_VirtualJoystickDesc
        {
            public uint version;

            public ushort type;

            public ushort padding;

            public ushort vendor_id;

            public ushort product_id;

            public ushort naxes;

            public ushort nbuttons;

            public ushort nballs;

            public ushort nhats;

            public ushort ntouchpads;

            public ushort nsensors;

            public unsafe fixed ushort padding2[2];

            public uint button_mask;

            public uint axis_mask;

            public unsafe byte* name;

            public unsafe SDL_VirtualJoystickTouchpadDesc* touchpads;

            public unsafe SDL_VirtualJoystickSensorDesc* sensors;

            public IntPtr userdata;

            public IntPtr Update;

            public IntPtr SetPlayerIndex;

            public IntPtr Rumble;

            public IntPtr RumbleTriggers;

            public IntPtr SetLED;

            public IntPtr SendEffect;

            public IntPtr SetSensorsEnabled;

            public IntPtr Cleanup;
        }

        public enum SDL_GamepadType
        {
            SDL_GAMEPAD_TYPE_UNKNOWN,
            SDL_GAMEPAD_TYPE_STANDARD,
            SDL_GAMEPAD_TYPE_XBOX360,
            SDL_GAMEPAD_TYPE_XBOXONE,
            SDL_GAMEPAD_TYPE_PS3,
            SDL_GAMEPAD_TYPE_PS4,
            SDL_GAMEPAD_TYPE_PS5,
            SDL_GAMEPAD_TYPE_NINTENDO_SWITCH_PRO,
            SDL_GAMEPAD_TYPE_NINTENDO_SWITCH_JOYCON_LEFT,
            SDL_GAMEPAD_TYPE_NINTENDO_SWITCH_JOYCON_RIGHT,
            SDL_GAMEPAD_TYPE_NINTENDO_SWITCH_JOYCON_PAIR,
            SDL_GAMEPAD_TYPE_GAMECUBE,
            SDL_GAMEPAD_TYPE_COUNT
        }

        public enum SDL_GamepadButton
        {
            SDL_GAMEPAD_BUTTON_INVALID = -1,
            SDL_GAMEPAD_BUTTON_SOUTH,
            SDL_GAMEPAD_BUTTON_EAST,
            SDL_GAMEPAD_BUTTON_WEST,
            SDL_GAMEPAD_BUTTON_NORTH,
            SDL_GAMEPAD_BUTTON_BACK,
            SDL_GAMEPAD_BUTTON_GUIDE,
            SDL_GAMEPAD_BUTTON_START,
            SDL_GAMEPAD_BUTTON_LEFT_STICK,
            SDL_GAMEPAD_BUTTON_RIGHT_STICK,
            SDL_GAMEPAD_BUTTON_LEFT_SHOULDER,
            SDL_GAMEPAD_BUTTON_RIGHT_SHOULDER,
            SDL_GAMEPAD_BUTTON_DPAD_UP,
            SDL_GAMEPAD_BUTTON_DPAD_DOWN,
            SDL_GAMEPAD_BUTTON_DPAD_LEFT,
            SDL_GAMEPAD_BUTTON_DPAD_RIGHT,
            SDL_GAMEPAD_BUTTON_MISC1,
            SDL_GAMEPAD_BUTTON_RIGHT_PADDLE1,
            SDL_GAMEPAD_BUTTON_LEFT_PADDLE1,
            SDL_GAMEPAD_BUTTON_RIGHT_PADDLE2,
            SDL_GAMEPAD_BUTTON_LEFT_PADDLE2,
            SDL_GAMEPAD_BUTTON_TOUCHPAD,
            SDL_GAMEPAD_BUTTON_MISC2,
            SDL_GAMEPAD_BUTTON_MISC3,
            SDL_GAMEPAD_BUTTON_MISC4,
            SDL_GAMEPAD_BUTTON_MISC5,
            SDL_GAMEPAD_BUTTON_MISC6,
            SDL_GAMEPAD_BUTTON_COUNT
        }

        public enum SDL_GamepadButtonLabel
        {
            SDL_GAMEPAD_BUTTON_LABEL_UNKNOWN,
            SDL_GAMEPAD_BUTTON_LABEL_A,
            SDL_GAMEPAD_BUTTON_LABEL_B,
            SDL_GAMEPAD_BUTTON_LABEL_X,
            SDL_GAMEPAD_BUTTON_LABEL_Y,
            SDL_GAMEPAD_BUTTON_LABEL_CROSS,
            SDL_GAMEPAD_BUTTON_LABEL_CIRCLE,
            SDL_GAMEPAD_BUTTON_LABEL_SQUARE,
            SDL_GAMEPAD_BUTTON_LABEL_TRIANGLE
        }

        public enum SDL_GamepadAxis
        {
            SDL_GAMEPAD_AXIS_INVALID = -1,
            SDL_GAMEPAD_AXIS_LEFTX,
            SDL_GAMEPAD_AXIS_LEFTY,
            SDL_GAMEPAD_AXIS_RIGHTX,
            SDL_GAMEPAD_AXIS_RIGHTY,
            SDL_GAMEPAD_AXIS_LEFT_TRIGGER,
            SDL_GAMEPAD_AXIS_RIGHT_TRIGGER,
            SDL_GAMEPAD_AXIS_COUNT
        }

        public enum SDL_GamepadBindingType
        {
            SDL_GAMEPAD_BINDTYPE_NONE,
            SDL_GAMEPAD_BINDTYPE_BUTTON,
            SDL_GAMEPAD_BINDTYPE_AXIS,
            SDL_GAMEPAD_BINDTYPE_HAT
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_GamepadBinding
        {
            [FieldOffset(0)]
            public SDL_GamepadBindingType input_type;

            [FieldOffset(4)]
            public int input_button;

            [FieldOffset(4)]
            public INTERNAL_SDL_GamepadBinding_input_axis input_axis;

            [FieldOffset(4)]
            public INTERNAL_SDL_GamepadBinding_input_hat input_hat;

            [FieldOffset(16)]
            public SDL_GamepadBindingType output_type;

            [FieldOffset(20)]
            public SDL_GamepadButton output_button;

            [FieldOffset(20)]
            public INTERNAL_SDL_GamepadBinding_output_axis output_axis;
        }

        public struct INTERNAL_SDL_GamepadBinding_input_axis
        {
            public int axis;

            public int axis_min;

            public int axis_max;
        }

        public struct INTERNAL_SDL_GamepadBinding_input_hat
        {
            public int hat;

            public int hat_mask;
        }

        public struct INTERNAL_SDL_GamepadBinding_output_axis
        {
            public SDL_GamepadAxis axis;

            public int axis_min;

            public int axis_max;
        }

        public enum SDL_Scancode
        {
            SDL_SCANCODE_UNKNOWN = 0,
            SDL_SCANCODE_A = 4,
            SDL_SCANCODE_B = 5,
            SDL_SCANCODE_C = 6,
            SDL_SCANCODE_D = 7,
            SDL_SCANCODE_E = 8,
            SDL_SCANCODE_F = 9,
            SDL_SCANCODE_G = 10,
            SDL_SCANCODE_H = 11,
            SDL_SCANCODE_I = 12,
            SDL_SCANCODE_J = 13,
            SDL_SCANCODE_K = 14,
            SDL_SCANCODE_L = 15,
            SDL_SCANCODE_M = 16,
            SDL_SCANCODE_N = 17,
            SDL_SCANCODE_O = 18,
            SDL_SCANCODE_P = 19,
            SDL_SCANCODE_Q = 20,
            SDL_SCANCODE_R = 21,
            SDL_SCANCODE_S = 22,
            SDL_SCANCODE_T = 23,
            SDL_SCANCODE_U = 24,
            SDL_SCANCODE_V = 25,
            SDL_SCANCODE_W = 26,
            SDL_SCANCODE_X = 27,
            SDL_SCANCODE_Y = 28,
            SDL_SCANCODE_Z = 29,
            SDL_SCANCODE_1 = 30,
            SDL_SCANCODE_2 = 31,
            SDL_SCANCODE_3 = 32,
            SDL_SCANCODE_4 = 33,
            SDL_SCANCODE_5 = 34,
            SDL_SCANCODE_6 = 35,
            SDL_SCANCODE_7 = 36,
            SDL_SCANCODE_8 = 37,
            SDL_SCANCODE_9 = 38,
            SDL_SCANCODE_0 = 39,
            SDL_SCANCODE_RETURN = 40,
            SDL_SCANCODE_ESCAPE = 41,
            SDL_SCANCODE_BACKSPACE = 42,
            SDL_SCANCODE_TAB = 43,
            SDL_SCANCODE_SPACE = 44,
            SDL_SCANCODE_MINUS = 45,
            SDL_SCANCODE_EQUALS = 46,
            SDL_SCANCODE_LEFTBRACKET = 47,
            SDL_SCANCODE_RIGHTBRACKET = 48,
            SDL_SCANCODE_BACKSLASH = 49,
            SDL_SCANCODE_NONUSHASH = 50,
            SDL_SCANCODE_SEMICOLON = 51,
            SDL_SCANCODE_APOSTROPHE = 52,
            SDL_SCANCODE_GRAVE = 53,
            SDL_SCANCODE_COMMA = 54,
            SDL_SCANCODE_PERIOD = 55,
            SDL_SCANCODE_SLASH = 56,
            SDL_SCANCODE_CAPSLOCK = 57,
            SDL_SCANCODE_F1 = 58,
            SDL_SCANCODE_F2 = 59,
            SDL_SCANCODE_F3 = 60,
            SDL_SCANCODE_F4 = 61,
            SDL_SCANCODE_F5 = 62,
            SDL_SCANCODE_F6 = 63,
            SDL_SCANCODE_F7 = 64,
            SDL_SCANCODE_F8 = 65,
            SDL_SCANCODE_F9 = 66,
            SDL_SCANCODE_F10 = 67,
            SDL_SCANCODE_F11 = 68,
            SDL_SCANCODE_F12 = 69,
            SDL_SCANCODE_PRINTSCREEN = 70,
            SDL_SCANCODE_SCROLLLOCK = 71,
            SDL_SCANCODE_PAUSE = 72,
            SDL_SCANCODE_INSERT = 73,
            SDL_SCANCODE_HOME = 74,
            SDL_SCANCODE_PAGEUP = 75,
            SDL_SCANCODE_DELETE = 76,
            SDL_SCANCODE_END = 77,
            SDL_SCANCODE_PAGEDOWN = 78,
            SDL_SCANCODE_RIGHT = 79,
            SDL_SCANCODE_LEFT = 80,
            SDL_SCANCODE_DOWN = 81,
            SDL_SCANCODE_UP = 82,
            SDL_SCANCODE_NUMLOCKCLEAR = 83,
            SDL_SCANCODE_KP_DIVIDE = 84,
            SDL_SCANCODE_KP_MULTIPLY = 85,
            SDL_SCANCODE_KP_MINUS = 86,
            SDL_SCANCODE_KP_PLUS = 87,
            SDL_SCANCODE_KP_ENTER = 88,
            SDL_SCANCODE_KP_1 = 89,
            SDL_SCANCODE_KP_2 = 90,
            SDL_SCANCODE_KP_3 = 91,
            SDL_SCANCODE_KP_4 = 92,
            SDL_SCANCODE_KP_5 = 93,
            SDL_SCANCODE_KP_6 = 94,
            SDL_SCANCODE_KP_7 = 95,
            SDL_SCANCODE_KP_8 = 96,
            SDL_SCANCODE_KP_9 = 97,
            SDL_SCANCODE_KP_0 = 98,
            SDL_SCANCODE_KP_PERIOD = 99,
            SDL_SCANCODE_NONUSBACKSLASH = 100,
            SDL_SCANCODE_APPLICATION = 101,
            SDL_SCANCODE_POWER = 102,
            SDL_SCANCODE_KP_EQUALS = 103,
            SDL_SCANCODE_F13 = 104,
            SDL_SCANCODE_F14 = 105,
            SDL_SCANCODE_F15 = 106,
            SDL_SCANCODE_F16 = 107,
            SDL_SCANCODE_F17 = 108,
            SDL_SCANCODE_F18 = 109,
            SDL_SCANCODE_F19 = 110,
            SDL_SCANCODE_F20 = 111,
            SDL_SCANCODE_F21 = 112,
            SDL_SCANCODE_F22 = 113,
            SDL_SCANCODE_F23 = 114,
            SDL_SCANCODE_F24 = 115,
            SDL_SCANCODE_EXECUTE = 116,
            SDL_SCANCODE_HELP = 117,
            SDL_SCANCODE_MENU = 118,
            SDL_SCANCODE_SELECT = 119,
            SDL_SCANCODE_STOP = 120,
            SDL_SCANCODE_AGAIN = 121,
            SDL_SCANCODE_UNDO = 122,
            SDL_SCANCODE_CUT = 123,
            SDL_SCANCODE_COPY = 124,
            SDL_SCANCODE_PASTE = 125,
            SDL_SCANCODE_FIND = 126,
            SDL_SCANCODE_MUTE = 127,
            SDL_SCANCODE_VOLUMEUP = 128,
            SDL_SCANCODE_VOLUMEDOWN = 129,
            SDL_SCANCODE_KP_COMMA = 133,
            SDL_SCANCODE_KP_EQUALSAS400 = 134,
            SDL_SCANCODE_INTERNATIONAL1 = 135,
            SDL_SCANCODE_INTERNATIONAL2 = 136,
            SDL_SCANCODE_INTERNATIONAL3 = 137,
            SDL_SCANCODE_INTERNATIONAL4 = 138,
            SDL_SCANCODE_INTERNATIONAL5 = 139,
            SDL_SCANCODE_INTERNATIONAL6 = 140,
            SDL_SCANCODE_INTERNATIONAL7 = 141,
            SDL_SCANCODE_INTERNATIONAL8 = 142,
            SDL_SCANCODE_INTERNATIONAL9 = 143,
            SDL_SCANCODE_LANG1 = 144,
            SDL_SCANCODE_LANG2 = 145,
            SDL_SCANCODE_LANG3 = 146,
            SDL_SCANCODE_LANG4 = 147,
            SDL_SCANCODE_LANG5 = 148,
            SDL_SCANCODE_LANG6 = 149,
            SDL_SCANCODE_LANG7 = 150,
            SDL_SCANCODE_LANG8 = 151,
            SDL_SCANCODE_LANG9 = 152,
            SDL_SCANCODE_ALTERASE = 153,
            SDL_SCANCODE_SYSREQ = 154,
            SDL_SCANCODE_CANCEL = 155,
            SDL_SCANCODE_CLEAR = 156,
            SDL_SCANCODE_PRIOR = 157,
            SDL_SCANCODE_RETURN2 = 158,
            SDL_SCANCODE_SEPARATOR = 159,
            SDL_SCANCODE_OUT = 160,
            SDL_SCANCODE_OPER = 161,
            SDL_SCANCODE_CLEARAGAIN = 162,
            SDL_SCANCODE_CRSEL = 163,
            SDL_SCANCODE_EXSEL = 164,
            SDL_SCANCODE_KP_00 = 176,
            SDL_SCANCODE_KP_000 = 177,
            SDL_SCANCODE_THOUSANDSSEPARATOR = 178,
            SDL_SCANCODE_DECIMALSEPARATOR = 179,
            SDL_SCANCODE_CURRENCYUNIT = 180,
            SDL_SCANCODE_CURRENCYSUBUNIT = 181,
            SDL_SCANCODE_KP_LEFTPAREN = 182,
            SDL_SCANCODE_KP_RIGHTPAREN = 183,
            SDL_SCANCODE_KP_LEFTBRACE = 184,
            SDL_SCANCODE_KP_RIGHTBRACE = 185,
            SDL_SCANCODE_KP_TAB = 186,
            SDL_SCANCODE_KP_BACKSPACE = 187,
            SDL_SCANCODE_KP_A = 188,
            SDL_SCANCODE_KP_B = 189,
            SDL_SCANCODE_KP_C = 190,
            SDL_SCANCODE_KP_D = 191,
            SDL_SCANCODE_KP_E = 192,
            SDL_SCANCODE_KP_F = 193,
            SDL_SCANCODE_KP_XOR = 194,
            SDL_SCANCODE_KP_POWER = 195,
            SDL_SCANCODE_KP_PERCENT = 196,
            SDL_SCANCODE_KP_LESS = 197,
            SDL_SCANCODE_KP_GREATER = 198,
            SDL_SCANCODE_KP_AMPERSAND = 199,
            SDL_SCANCODE_KP_DBLAMPERSAND = 200,
            SDL_SCANCODE_KP_VERTICALBAR = 201,
            SDL_SCANCODE_KP_DBLVERTICALBAR = 202,
            SDL_SCANCODE_KP_COLON = 203,
            SDL_SCANCODE_KP_HASH = 204,
            SDL_SCANCODE_KP_SPACE = 205,
            SDL_SCANCODE_KP_AT = 206,
            SDL_SCANCODE_KP_EXCLAM = 207,
            SDL_SCANCODE_KP_MEMSTORE = 208,
            SDL_SCANCODE_KP_MEMRECALL = 209,
            SDL_SCANCODE_KP_MEMCLEAR = 210,
            SDL_SCANCODE_KP_MEMADD = 211,
            SDL_SCANCODE_KP_MEMSUBTRACT = 212,
            SDL_SCANCODE_KP_MEMMULTIPLY = 213,
            SDL_SCANCODE_KP_MEMDIVIDE = 214,
            SDL_SCANCODE_KP_PLUSMINUS = 215,
            SDL_SCANCODE_KP_CLEAR = 216,
            SDL_SCANCODE_KP_CLEARENTRY = 217,
            SDL_SCANCODE_KP_BINARY = 218,
            SDL_SCANCODE_KP_OCTAL = 219,
            SDL_SCANCODE_KP_DECIMAL = 220,
            SDL_SCANCODE_KP_HEXADECIMAL = 221,
            SDL_SCANCODE_LCTRL = 224,
            SDL_SCANCODE_LSHIFT = 225,
            SDL_SCANCODE_LALT = 226,
            SDL_SCANCODE_LGUI = 227,
            SDL_SCANCODE_RCTRL = 228,
            SDL_SCANCODE_RSHIFT = 229,
            SDL_SCANCODE_RALT = 230,
            SDL_SCANCODE_RGUI = 231,
            SDL_SCANCODE_MODE = 257,
            SDL_SCANCODE_SLEEP = 258,
            SDL_SCANCODE_WAKE = 259,
            SDL_SCANCODE_CHANNEL_INCREMENT = 260,
            SDL_SCANCODE_CHANNEL_DECREMENT = 261,
            SDL_SCANCODE_MEDIA_PLAY = 262,
            SDL_SCANCODE_MEDIA_PAUSE = 263,
            SDL_SCANCODE_MEDIA_RECORD = 264,
            SDL_SCANCODE_MEDIA_FAST_FORWARD = 265,
            SDL_SCANCODE_MEDIA_REWIND = 266,
            SDL_SCANCODE_MEDIA_NEXT_TRACK = 267,
            SDL_SCANCODE_MEDIA_PREVIOUS_TRACK = 268,
            SDL_SCANCODE_MEDIA_STOP = 269,
            SDL_SCANCODE_MEDIA_EJECT = 270,
            SDL_SCANCODE_MEDIA_PLAY_PAUSE = 271,
            SDL_SCANCODE_MEDIA_SELECT = 272,
            SDL_SCANCODE_AC_NEW = 273,
            SDL_SCANCODE_AC_OPEN = 274,
            SDL_SCANCODE_AC_CLOSE = 275,
            SDL_SCANCODE_AC_EXIT = 276,
            SDL_SCANCODE_AC_SAVE = 277,
            SDL_SCANCODE_AC_PRINT = 278,
            SDL_SCANCODE_AC_PROPERTIES = 279,
            SDL_SCANCODE_AC_SEARCH = 280,
            SDL_SCANCODE_AC_HOME = 281,
            SDL_SCANCODE_AC_BACK = 282,
            SDL_SCANCODE_AC_FORWARD = 283,
            SDL_SCANCODE_AC_STOP = 284,
            SDL_SCANCODE_AC_REFRESH = 285,
            SDL_SCANCODE_AC_BOOKMARKS = 286,
            SDL_SCANCODE_SOFTLEFT = 287,
            SDL_SCANCODE_SOFTRIGHT = 288,
            SDL_SCANCODE_CALL = 289,
            SDL_SCANCODE_ENDCALL = 290,
            SDL_SCANCODE_RESERVED = 400,
            SDL_SCANCODE_COUNT = 512
        }

        public enum SDL_Keycode : uint
        {
            SDLK_SCANCODE_MASK = 1073741824u,
            SDLK_UNKNOWN = 0u,
            SDLK_RETURN = 13u,
            SDLK_ESCAPE = 27u,
            SDLK_BACKSPACE = 8u,
            SDLK_TAB = 9u,
            SDLK_SPACE = 32u,
            SDLK_EXCLAIM = 33u,
            SDLK_DBLAPOSTROPHE = 34u,
            SDLK_HASH = 35u,
            SDLK_DOLLAR = 36u,
            SDLK_PERCENT = 37u,
            SDLK_AMPERSAND = 38u,
            SDLK_APOSTROPHE = 39u,
            SDLK_LEFTPAREN = 40u,
            SDLK_RIGHTPAREN = 41u,
            SDLK_ASTERISK = 42u,
            SDLK_PLUS = 43u,
            SDLK_COMMA = 44u,
            SDLK_MINUS = 45u,
            SDLK_PERIOD = 46u,
            SDLK_SLASH = 47u,
            SDLK_0 = 48u,
            SDLK_1 = 49u,
            SDLK_2 = 50u,
            SDLK_3 = 51u,
            SDLK_4 = 52u,
            SDLK_5 = 53u,
            SDLK_6 = 54u,
            SDLK_7 = 55u,
            SDLK_8 = 56u,
            SDLK_9 = 57u,
            SDLK_COLON = 58u,
            SDLK_SEMICOLON = 59u,
            SDLK_LESS = 60u,
            SDLK_EQUALS = 61u,
            SDLK_GREATER = 62u,
            SDLK_QUESTION = 63u,
            SDLK_AT = 64u,
            SDLK_LEFTBRACKET = 91u,
            SDLK_BACKSLASH = 92u,
            SDLK_RIGHTBRACKET = 93u,
            SDLK_CARET = 94u,
            SDLK_UNDERSCORE = 95u,
            SDLK_GRAVE = 96u,
            SDLK_A = 97u,
            SDLK_B = 98u,
            SDLK_C = 99u,
            SDLK_D = 100u,
            SDLK_E = 101u,
            SDLK_F = 102u,
            SDLK_G = 103u,
            SDLK_H = 104u,
            SDLK_I = 105u,
            SDLK_J = 106u,
            SDLK_K = 107u,
            SDLK_L = 108u,
            SDLK_M = 109u,
            SDLK_N = 110u,
            SDLK_O = 111u,
            SDLK_P = 112u,
            SDLK_Q = 113u,
            SDLK_R = 114u,
            SDLK_S = 115u,
            SDLK_T = 116u,
            SDLK_U = 117u,
            SDLK_V = 118u,
            SDLK_W = 119u,
            SDLK_X = 120u,
            SDLK_Y = 121u,
            SDLK_Z = 122u,
            SDLK_LEFTBRACE = 123u,
            SDLK_PIPE = 124u,
            SDLK_RIGHTBRACE = 125u,
            SDLK_TILDE = 126u,
            SDLK_DELETE = 127u,
            SDLK_PLUSMINUS = 177u,
            SDLK_CAPSLOCK = 1073741881u,
            SDLK_F1 = 1073741882u,
            SDLK_F2 = 1073741883u,
            SDLK_F3 = 1073741884u,
            SDLK_F4 = 1073741885u,
            SDLK_F5 = 1073741886u,
            SDLK_F6 = 1073741887u,
            SDLK_F7 = 1073741888u,
            SDLK_F8 = 1073741889u,
            SDLK_F9 = 1073741890u,
            SDLK_F10 = 1073741891u,
            SDLK_F11 = 1073741892u,
            SDLK_F12 = 1073741893u,
            SDLK_PRINTSCREEN = 1073741894u,
            SDLK_SCROLLLOCK = 1073741895u,
            SDLK_PAUSE = 1073741896u,
            SDLK_INSERT = 1073741897u,
            SDLK_HOME = 1073741898u,
            SDLK_PAGEUP = 1073741899u,
            SDLK_END = 1073741901u,
            SDLK_PAGEDOWN = 1073741902u,
            SDLK_RIGHT = 1073741903u,
            SDLK_LEFT = 1073741904u,
            SDLK_DOWN = 1073741905u,
            SDLK_UP = 1073741906u,
            SDLK_NUMLOCKCLEAR = 1073741907u,
            SDLK_KP_DIVIDE = 1073741908u,
            SDLK_KP_MULTIPLY = 1073741909u,
            SDLK_KP_MINUS = 1073741910u,
            SDLK_KP_PLUS = 1073741911u,
            SDLK_KP_ENTER = 1073741912u,
            SDLK_KP_1 = 1073741913u,
            SDLK_KP_2 = 1073741914u,
            SDLK_KP_3 = 1073741915u,
            SDLK_KP_4 = 1073741916u,
            SDLK_KP_5 = 1073741917u,
            SDLK_KP_6 = 1073741918u,
            SDLK_KP_7 = 1073741919u,
            SDLK_KP_8 = 1073741920u,
            SDLK_KP_9 = 1073741921u,
            SDLK_KP_0 = 1073741922u,
            SDLK_KP_PERIOD = 1073741923u,
            SDLK_APPLICATION = 1073741925u,
            SDLK_POWER = 1073741926u,
            SDLK_KP_EQUALS = 1073741927u,
            SDLK_F13 = 1073741928u,
            SDLK_F14 = 1073741929u,
            SDLK_F15 = 1073741930u,
            SDLK_F16 = 1073741931u,
            SDLK_F17 = 1073741932u,
            SDLK_F18 = 1073741933u,
            SDLK_F19 = 1073741934u,
            SDLK_F20 = 1073741935u,
            SDLK_F21 = 1073741936u,
            SDLK_F22 = 1073741937u,
            SDLK_F23 = 1073741938u,
            SDLK_F24 = 1073741939u,
            SDLK_EXECUTE = 1073741940u,
            SDLK_HELP = 1073741941u,
            SDLK_MENU = 1073741942u,
            SDLK_SELECT = 1073741943u,
            SDLK_STOP = 1073741944u,
            SDLK_AGAIN = 1073741945u,
            SDLK_UNDO = 1073741946u,
            SDLK_CUT = 1073741947u,
            SDLK_COPY = 1073741948u,
            SDLK_PASTE = 1073741949u,
            SDLK_FIND = 1073741950u,
            SDLK_MUTE = 1073741951u,
            SDLK_VOLUMEUP = 1073741952u,
            SDLK_VOLUMEDOWN = 1073741953u,
            SDLK_KP_COMMA = 1073741957u,
            SDLK_KP_EQUALSAS400 = 1073741958u,
            SDLK_ALTERASE = 1073741977u,
            SDLK_SYSREQ = 1073741978u,
            SDLK_CANCEL = 1073741979u,
            SDLK_CLEAR = 1073741980u,
            SDLK_PRIOR = 1073741981u,
            SDLK_RETURN2 = 1073741982u,
            SDLK_SEPARATOR = 1073741983u,
            SDLK_OUT = 1073741984u,
            SDLK_OPER = 1073741985u,
            SDLK_CLEARAGAIN = 1073741986u,
            SDLK_CRSEL = 1073741987u,
            SDLK_EXSEL = 1073741988u,
            SDLK_KP_00 = 1073742000u,
            SDLK_KP_000 = 1073742001u,
            SDLK_THOUSANDSSEPARATOR = 1073742002u,
            SDLK_DECIMALSEPARATOR = 1073742003u,
            SDLK_CURRENCYUNIT = 1073742004u,
            SDLK_CURRENCYSUBUNIT = 1073742005u,
            SDLK_KP_LEFTPAREN = 1073742006u,
            SDLK_KP_RIGHTPAREN = 1073742007u,
            SDLK_KP_LEFTBRACE = 1073742008u,
            SDLK_KP_RIGHTBRACE = 1073742009u,
            SDLK_KP_TAB = 1073742010u,
            SDLK_KP_BACKSPACE = 1073742011u,
            SDLK_KP_A = 1073742012u,
            SDLK_KP_B = 1073742013u,
            SDLK_KP_C = 1073742014u,
            SDLK_KP_D = 1073742015u,
            SDLK_KP_E = 1073742016u,
            SDLK_KP_F = 1073742017u,
            SDLK_KP_XOR = 1073742018u,
            SDLK_KP_POWER = 1073742019u,
            SDLK_KP_PERCENT = 1073742020u,
            SDLK_KP_LESS = 1073742021u,
            SDLK_KP_GREATER = 1073742022u,
            SDLK_KP_AMPERSAND = 1073742023u,
            SDLK_KP_DBLAMPERSAND = 1073742024u,
            SDLK_KP_VERTICALBAR = 1073742025u,
            SDLK_KP_DBLVERTICALBAR = 1073742026u,
            SDLK_KP_COLON = 1073742027u,
            SDLK_KP_HASH = 1073742028u,
            SDLK_KP_SPACE = 1073742029u,
            SDLK_KP_AT = 1073742030u,
            SDLK_KP_EXCLAM = 1073742031u,
            SDLK_KP_MEMSTORE = 1073742032u,
            SDLK_KP_MEMRECALL = 1073742033u,
            SDLK_KP_MEMCLEAR = 1073742034u,
            SDLK_KP_MEMADD = 1073742035u,
            SDLK_KP_MEMSUBTRACT = 1073742036u,
            SDLK_KP_MEMMULTIPLY = 1073742037u,
            SDLK_KP_MEMDIVIDE = 1073742038u,
            SDLK_KP_PLUSMINUS = 1073742039u,
            SDLK_KP_CLEAR = 1073742040u,
            SDLK_KP_CLEARENTRY = 1073742041u,
            SDLK_KP_BINARY = 1073742042u,
            SDLK_KP_OCTAL = 1073742043u,
            SDLK_KP_DECIMAL = 1073742044u,
            SDLK_KP_HEXADECIMAL = 1073742045u,
            SDLK_LCTRL = 1073742048u,
            SDLK_LSHIFT = 1073742049u,
            SDLK_LALT = 1073742050u,
            SDLK_LGUI = 1073742051u,
            SDLK_RCTRL = 1073742052u,
            SDLK_RSHIFT = 1073742053u,
            SDLK_RALT = 1073742054u,
            SDLK_RGUI = 1073742055u,
            SDLK_MODE = 1073742081u,
            SDLK_SLEEP = 1073742082u,
            SDLK_WAKE = 1073742083u,
            SDLK_CHANNEL_INCREMENT = 1073742084u,
            SDLK_CHANNEL_DECREMENT = 1073742085u,
            SDLK_MEDIA_PLAY = 1073742086u,
            SDLK_MEDIA_PAUSE = 1073742087u,
            SDLK_MEDIA_RECORD = 1073742088u,
            SDLK_MEDIA_FAST_FORWARD = 1073742089u,
            SDLK_MEDIA_REWIND = 1073742090u,
            SDLK_MEDIA_NEXT_TRACK = 1073742091u,
            SDLK_MEDIA_PREVIOUS_TRACK = 1073742092u,
            SDLK_MEDIA_STOP = 1073742093u,
            SDLK_MEDIA_EJECT = 1073742094u,
            SDLK_MEDIA_PLAY_PAUSE = 1073742095u,
            SDLK_MEDIA_SELECT = 1073742096u,
            SDLK_AC_NEW = 1073742097u,
            SDLK_AC_OPEN = 1073742098u,
            SDLK_AC_CLOSE = 1073742099u,
            SDLK_AC_EXIT = 1073742100u,
            SDLK_AC_SAVE = 1073742101u,
            SDLK_AC_PRINT = 1073742102u,
            SDLK_AC_PROPERTIES = 1073742103u,
            SDLK_AC_SEARCH = 1073742104u,
            SDLK_AC_HOME = 1073742105u,
            SDLK_AC_BACK = 1073742106u,
            SDLK_AC_FORWARD = 1073742107u,
            SDLK_AC_STOP = 1073742108u,
            SDLK_AC_REFRESH = 1073742109u,
            SDLK_AC_BOOKMARKS = 1073742110u,
            SDLK_SOFTLEFT = 1073742111u,
            SDLK_SOFTRIGHT = 1073742112u,
            SDLK_CALL = 1073742113u,
            SDLK_ENDCALL = 1073742114u,
            SDLK_LEFT_TAB = 536870913u,
            SDLK_LEVEL5_SHIFT = 536870914u,
            SDLK_MULTI_KEY_COMPOSE = 536870915u,
            SDLK_LMETA = 536870916u,
            SDLK_RMETA = 536870917u,
            SDLK_LHYPER = 536870918u,
            SDLK_RHYPER = 536870919u
        }

        [Flags]
        public enum SDL_Keymod : ushort
        {
            SDL_KMOD_NONE = 0,
            SDL_KMOD_LSHIFT = 1,
            SDL_KMOD_RSHIFT = 2,
            SDL_KMOD_LCTRL = 0x40,
            SDL_KMOD_RCTRL = 0x80,
            SDL_KMOD_LALT = 0x100,
            SDL_KMOD_RALT = 0x200,
            SDL_KMOD_LGUI = 0x400,
            SDL_KMOD_RGUI = 0x800,
            SDL_KMOD_NUM = 0x1000,
            SDL_KMOD_CAPS = 0x2000,
            SDL_KMOD_MODE = 0x4000,
            SDL_KMOD_SCROLL = 0x8000,
            SDL_KMOD_CTRL = 0xC0,
            SDL_KMOD_SHIFT = 3,
            SDL_KMOD_ALT = 0x300,
            SDL_KMOD_GUI = 0xC00
        }

        public enum SDL_TextInputType
        {
            SDL_TEXTINPUT_TYPE_TEXT,
            SDL_TEXTINPUT_TYPE_TEXT_NAME,
            SDL_TEXTINPUT_TYPE_TEXT_EMAIL,
            SDL_TEXTINPUT_TYPE_TEXT_USERNAME,
            SDL_TEXTINPUT_TYPE_TEXT_PASSWORD_HIDDEN,
            SDL_TEXTINPUT_TYPE_TEXT_PASSWORD_VISIBLE,
            SDL_TEXTINPUT_TYPE_NUMBER,
            SDL_TEXTINPUT_TYPE_NUMBER_PASSWORD_HIDDEN,
            SDL_TEXTINPUT_TYPE_NUMBER_PASSWORD_VISIBLE
        }

        public enum SDL_Capitalization
        {
            SDL_CAPITALIZE_NONE,
            SDL_CAPITALIZE_SENTENCES,
            SDL_CAPITALIZE_WORDS,
            SDL_CAPITALIZE_LETTERS
        }

        public enum SDL_SystemCursor
        {
            SDL_SYSTEM_CURSOR_DEFAULT,
            SDL_SYSTEM_CURSOR_TEXT,
            SDL_SYSTEM_CURSOR_WAIT,
            SDL_SYSTEM_CURSOR_CROSSHAIR,
            SDL_SYSTEM_CURSOR_PROGRESS,
            SDL_SYSTEM_CURSOR_NWSE_RESIZE,
            SDL_SYSTEM_CURSOR_NESW_RESIZE,
            SDL_SYSTEM_CURSOR_EW_RESIZE,
            SDL_SYSTEM_CURSOR_NS_RESIZE,
            SDL_SYSTEM_CURSOR_MOVE,
            SDL_SYSTEM_CURSOR_NOT_ALLOWED,
            SDL_SYSTEM_CURSOR_POINTER,
            SDL_SYSTEM_CURSOR_NW_RESIZE,
            SDL_SYSTEM_CURSOR_N_RESIZE,
            SDL_SYSTEM_CURSOR_NE_RESIZE,
            SDL_SYSTEM_CURSOR_E_RESIZE,
            SDL_SYSTEM_CURSOR_SE_RESIZE,
            SDL_SYSTEM_CURSOR_S_RESIZE,
            SDL_SYSTEM_CURSOR_SW_RESIZE,
            SDL_SYSTEM_CURSOR_W_RESIZE,
            SDL_SYSTEM_CURSOR_COUNT
        }

        public enum SDL_MouseWheelDirection
        {
            SDL_MOUSEWHEEL_NORMAL,
            SDL_MOUSEWHEEL_FLIPPED
        }

        public struct SDL_CursorFrameInfo
        {
            public unsafe SDL_Surface* surface;

            public uint duration;
        }

        [Flags]
        public enum SDL_MouseButtonFlags : uint
        {
            SDL_BUTTON_LMASK = 1u,
            SDL_BUTTON_MMASK = 2u,
            SDL_BUTTON_RMASK = 4u,
            SDL_BUTTON_X1MASK = 8u,
            SDL_BUTTON_X2MASK = 0x10u
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void SDL_MouseMotionTransformCallback(IntPtr userdata, ulong timestamp, IntPtr window, uint mouseID, float* x, float* y);

        public enum SDL_TouchDeviceType
        {
            SDL_TOUCH_DEVICE_INVALID = -1,
            SDL_TOUCH_DEVICE_DIRECT,
            SDL_TOUCH_DEVICE_INDIRECT_ABSOLUTE,
            SDL_TOUCH_DEVICE_INDIRECT_RELATIVE
        }

        public struct SDL_Finger
        {
            public ulong id;

            public float x;

            public float y;

            public float pressure;
        }

        [Flags]
        public enum SDL_PenInputFlags : uint
        {
            SDL_PEN_INPUT_DOWN = 1u,
            SDL_PEN_INPUT_BUTTON_1 = 2u,
            SDL_PEN_INPUT_BUTTON_2 = 4u,
            SDL_PEN_INPUT_BUTTON_3 = 8u,
            SDL_PEN_INPUT_BUTTON_4 = 0x10u,
            SDL_PEN_INPUT_BUTTON_5 = 0x20u,
            SDL_PEN_INPUT_ERASER_TIP = 0x40000000u
        }

        public enum SDL_PenAxis
        {
            SDL_PEN_AXIS_PRESSURE,
            SDL_PEN_AXIS_XTILT,
            SDL_PEN_AXIS_YTILT,
            SDL_PEN_AXIS_DISTANCE,
            SDL_PEN_AXIS_ROTATION,
            SDL_PEN_AXIS_SLIDER,
            SDL_PEN_AXIS_TANGENTIAL_PRESSURE,
            SDL_PEN_AXIS_COUNT
        }

        public enum SDL_PenDeviceType
        {
            SDL_PEN_DEVICE_TYPE_INVALID = -1,
            SDL_PEN_DEVICE_TYPE_UNKNOWN,
            SDL_PEN_DEVICE_TYPE_DIRECT,
            SDL_PEN_DEVICE_TYPE_INDIRECT
        }

        public enum SDL_EventType
        {
            SDL_EVENT_FIRST = 0,
            SDL_EVENT_QUIT = 256,
            SDL_EVENT_TERMINATING = 257,
            SDL_EVENT_LOW_MEMORY = 258,
            SDL_EVENT_WILL_ENTER_BACKGROUND = 259,
            SDL_EVENT_DID_ENTER_BACKGROUND = 260,
            SDL_EVENT_WILL_ENTER_FOREGROUND = 261,
            SDL_EVENT_DID_ENTER_FOREGROUND = 262,
            SDL_EVENT_LOCALE_CHANGED = 263,
            SDL_EVENT_SYSTEM_THEME_CHANGED = 264,
            SDL_EVENT_DISPLAY_ORIENTATION = 337,
            SDL_EVENT_DISPLAY_ADDED = 338,
            SDL_EVENT_DISPLAY_REMOVED = 339,
            SDL_EVENT_DISPLAY_MOVED = 340,
            SDL_EVENT_DISPLAY_DESKTOP_MODE_CHANGED = 341,
            SDL_EVENT_DISPLAY_CURRENT_MODE_CHANGED = 342,
            SDL_EVENT_DISPLAY_CONTENT_SCALE_CHANGED = 343,
            SDL_EVENT_DISPLAY_USABLE_BOUNDS_CHANGED = 344,
            SDL_EVENT_DISPLAY_FIRST = 337,
            SDL_EVENT_DISPLAY_LAST = 344,
            SDL_EVENT_WINDOW_SHOWN = 514,
            SDL_EVENT_WINDOW_HIDDEN = 515,
            SDL_EVENT_WINDOW_EXPOSED = 516,
            SDL_EVENT_WINDOW_MOVED = 517,
            SDL_EVENT_WINDOW_RESIZED = 518,
            SDL_EVENT_WINDOW_PIXEL_SIZE_CHANGED = 519,
            SDL_EVENT_WINDOW_METAL_VIEW_RESIZED = 520,
            SDL_EVENT_WINDOW_MINIMIZED = 521,
            SDL_EVENT_WINDOW_MAXIMIZED = 522,
            SDL_EVENT_WINDOW_RESTORED = 523,
            SDL_EVENT_WINDOW_MOUSE_ENTER = 524,
            SDL_EVENT_WINDOW_MOUSE_LEAVE = 525,
            SDL_EVENT_WINDOW_FOCUS_GAINED = 526,
            SDL_EVENT_WINDOW_FOCUS_LOST = 527,
            SDL_EVENT_WINDOW_CLOSE_REQUESTED = 528,
            SDL_EVENT_WINDOW_HIT_TEST = 529,
            SDL_EVENT_WINDOW_ICCPROF_CHANGED = 530,
            SDL_EVENT_WINDOW_DISPLAY_CHANGED = 531,
            SDL_EVENT_WINDOW_DISPLAY_SCALE_CHANGED = 532,
            SDL_EVENT_WINDOW_SAFE_AREA_CHANGED = 533,
            SDL_EVENT_WINDOW_OCCLUDED = 534,
            SDL_EVENT_WINDOW_ENTER_FULLSCREEN = 535,
            SDL_EVENT_WINDOW_LEAVE_FULLSCREEN = 536,
            SDL_EVENT_WINDOW_DESTROYED = 537,
            SDL_EVENT_WINDOW_HDR_STATE_CHANGED = 538,
            SDL_EVENT_WINDOW_FIRST = 514,
            SDL_EVENT_WINDOW_LAST = 538,
            SDL_EVENT_KEY_DOWN = 768,
            SDL_EVENT_KEY_UP = 769,
            SDL_EVENT_TEXT_EDITING = 770,
            SDL_EVENT_TEXT_INPUT = 771,
            SDL_EVENT_KEYMAP_CHANGED = 772,
            SDL_EVENT_KEYBOARD_ADDED = 773,
            SDL_EVENT_KEYBOARD_REMOVED = 774,
            SDL_EVENT_TEXT_EDITING_CANDIDATES = 775,
            SDL_EVENT_SCREEN_KEYBOARD_SHOWN = 776,
            SDL_EVENT_SCREEN_KEYBOARD_HIDDEN = 777,
            SDL_EVENT_MOUSE_MOTION = 1024,
            SDL_EVENT_MOUSE_BUTTON_DOWN = 1025,
            SDL_EVENT_MOUSE_BUTTON_UP = 1026,
            SDL_EVENT_MOUSE_WHEEL = 1027,
            SDL_EVENT_MOUSE_ADDED = 1028,
            SDL_EVENT_MOUSE_REMOVED = 1029,
            SDL_EVENT_JOYSTICK_AXIS_MOTION = 1536,
            SDL_EVENT_JOYSTICK_BALL_MOTION = 1537,
            SDL_EVENT_JOYSTICK_HAT_MOTION = 1538,
            SDL_EVENT_JOYSTICK_BUTTON_DOWN = 1539,
            SDL_EVENT_JOYSTICK_BUTTON_UP = 1540,
            SDL_EVENT_JOYSTICK_ADDED = 1541,
            SDL_EVENT_JOYSTICK_REMOVED = 1542,
            SDL_EVENT_JOYSTICK_BATTERY_UPDATED = 1543,
            SDL_EVENT_JOYSTICK_UPDATE_COMPLETE = 1544,
            SDL_EVENT_GAMEPAD_AXIS_MOTION = 1616,
            SDL_EVENT_GAMEPAD_BUTTON_DOWN = 1617,
            SDL_EVENT_GAMEPAD_BUTTON_UP = 1618,
            SDL_EVENT_GAMEPAD_ADDED = 1619,
            SDL_EVENT_GAMEPAD_REMOVED = 1620,
            SDL_EVENT_GAMEPAD_REMAPPED = 1621,
            SDL_EVENT_GAMEPAD_TOUCHPAD_DOWN = 1622,
            SDL_EVENT_GAMEPAD_TOUCHPAD_MOTION = 1623,
            SDL_EVENT_GAMEPAD_TOUCHPAD_UP = 1624,
            SDL_EVENT_GAMEPAD_SENSOR_UPDATE = 1625,
            SDL_EVENT_GAMEPAD_UPDATE_COMPLETE = 1626,
            SDL_EVENT_GAMEPAD_STEAM_HANDLE_UPDATED = 1627,
            SDL_EVENT_FINGER_DOWN = 1792,
            SDL_EVENT_FINGER_UP = 1793,
            SDL_EVENT_FINGER_MOTION = 1794,
            SDL_EVENT_FINGER_CANCELED = 1795,
            SDL_EVENT_PINCH_BEGIN = 1808,
            SDL_EVENT_PINCH_UPDATE = 1809,
            SDL_EVENT_PINCH_END = 1810,
            SDL_EVENT_CLIPBOARD_UPDATE = 2304,
            SDL_EVENT_DROP_FILE = 4096,
            SDL_EVENT_DROP_TEXT = 4097,
            SDL_EVENT_DROP_BEGIN = 4098,
            SDL_EVENT_DROP_COMPLETE = 4099,
            SDL_EVENT_DROP_POSITION = 4100,
            SDL_EVENT_AUDIO_DEVICE_ADDED = 4352,
            SDL_EVENT_AUDIO_DEVICE_REMOVED = 4353,
            SDL_EVENT_AUDIO_DEVICE_FORMAT_CHANGED = 4354,
            SDL_EVENT_SENSOR_UPDATE = 4608,
            SDL_EVENT_PEN_PROXIMITY_IN = 4864,
            SDL_EVENT_PEN_PROXIMITY_OUT = 4865,
            SDL_EVENT_PEN_DOWN = 4866,
            SDL_EVENT_PEN_UP = 4867,
            SDL_EVENT_PEN_BUTTON_DOWN = 4868,
            SDL_EVENT_PEN_BUTTON_UP = 4869,
            SDL_EVENT_PEN_MOTION = 4870,
            SDL_EVENT_PEN_AXIS = 4871,
            SDL_EVENT_CAMERA_DEVICE_ADDED = 5120,
            SDL_EVENT_CAMERA_DEVICE_REMOVED = 5121,
            SDL_EVENT_CAMERA_DEVICE_APPROVED = 5122,
            SDL_EVENT_CAMERA_DEVICE_DENIED = 5123,
            SDL_EVENT_RENDER_TARGETS_RESET = 8192,
            SDL_EVENT_RENDER_DEVICE_RESET = 8193,
            SDL_EVENT_RENDER_DEVICE_LOST = 8194,
            SDL_EVENT_PRIVATE0 = 16384,
            SDL_EVENT_PRIVATE1 = 16385,
            SDL_EVENT_PRIVATE2 = 16386,
            SDL_EVENT_PRIVATE3 = 16387,
            SDL_EVENT_POLL_SENTINEL = 32512,
            SDL_EVENT_USER = 32768,
            SDL_EVENT_LAST = 65535,
            SDL_EVENT_ENUM_PADDING = int.MaxValue
        }

        public struct SDL_CommonEvent
        {
            public uint type;

            public uint reserved;

            public ulong timestamp;
        }

        public struct SDL_DisplayEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint displayID;

            public int data1;

            public int data2;
        }

        public struct SDL_WindowEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public int data1;

            public int data2;
        }

        public struct SDL_KeyboardDeviceEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;
        }

        public struct SDL_KeyboardEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public SDL_Scancode scancode;

            public uint key;

            public SDL_Keymod mod;

            public ushort raw;

            public SDLBool down;

            public SDLBool repeat;
        }

        public struct SDL_TextEditingEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public unsafe byte* text;

            public int start;

            public int length;
        }

        public struct SDL_TextEditingCandidatesEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public unsafe byte** candidates;

            public int num_candidates;

            public int selected_candidate;

            public SDLBool horizontal;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_TextInputEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public unsafe byte* text;
        }

        public struct SDL_MouseDeviceEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;
        }

        public struct SDL_MouseMotionEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public SDL_MouseButtonFlags state;

            public float x;

            public float y;

            public float xrel;

            public float yrel;
        }

        public struct SDL_MouseButtonEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public byte button;

            public SDLBool down;

            public byte clicks;

            public byte padding;

            public float x;

            public float y;
        }

        public struct SDL_MouseWheelEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public float x;

            public float y;

            public SDL_MouseWheelDirection direction;

            public float mouse_x;

            public float mouse_y;

            public int integer_x;

            public int integer_y;
        }

        public struct SDL_JoyAxisEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public byte axis;

            public byte padding1;

            public byte padding2;

            public byte padding3;

            public short value;

            public ushort padding4;
        }

        public struct SDL_JoyBallEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public byte ball;

            public byte padding1;

            public byte padding2;

            public byte padding3;

            public short xrel;

            public short yrel;
        }

        public struct SDL_JoyHatEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public byte hat;

            public byte value;

            public byte padding1;

            public byte padding2;
        }

        public struct SDL_JoyButtonEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public byte button;

            public SDLBool down;

            public byte padding1;

            public byte padding2;
        }

        public struct SDL_JoyDeviceEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;
        }

        public struct SDL_JoyBatteryEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public SDL_PowerState state;

            public int percent;
        }

        public struct SDL_GamepadAxisEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public byte axis;

            public byte padding1;

            public byte padding2;

            public byte padding3;

            public short value;

            public ushort padding4;
        }

        public struct SDL_GamepadButtonEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public byte button;

            public SDLBool down;

            public byte padding1;

            public byte padding2;
        }

        public struct SDL_GamepadDeviceEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;
        }

        public struct SDL_GamepadTouchpadEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public int touchpad;

            public int finger;

            public float x;

            public float y;

            public float pressure;
        }

        public struct SDL_GamepadSensorEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public int sensor;

            public unsafe fixed float data[3];

            public ulong sensor_timestamp;
        }

        public struct SDL_AudioDeviceEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public SDLBool recording;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_CameraDeviceEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;
        }

        public struct SDL_RenderEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;
        }

        public struct SDL_TouchFingerEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public ulong touchID;

            public ulong fingerID;

            public float x;

            public float y;

            public float dx;

            public float dy;

            public float pressure;

            public uint windowID;
        }

        public struct SDL_PinchFingerEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public float scale;

            public uint windowID;
        }

        public struct SDL_PenProximityEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;
        }

        public struct SDL_PenMotionEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public SDL_PenInputFlags pen_state;

            public float x;

            public float y;
        }

        public struct SDL_PenTouchEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public SDL_PenInputFlags pen_state;

            public float x;

            public float y;

            public SDLBool eraser;

            public SDLBool down;
        }

        public struct SDL_PenButtonEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public SDL_PenInputFlags pen_state;

            public float x;

            public float y;

            public byte button;

            public SDLBool down;
        }

        public struct SDL_PenAxisEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public uint which;

            public SDL_PenInputFlags pen_state;

            public float x;

            public float y;

            public SDL_PenAxis axis;

            public float value;
        }

        public struct SDL_DropEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public float x;

            public float y;

            public unsafe byte* source;

            public unsafe byte* data;
        }

        public struct SDL_ClipboardEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public SDLBool owner;

            public int num_mime_types;

            public unsafe byte** mime_types;
        }

        public struct SDL_SensorEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;

            public uint which;

            public unsafe fixed float data[6];

            public ulong sensor_timestamp;
        }

        public struct SDL_QuitEvent
        {
            public SDL_EventType type;

            public uint reserved;

            public ulong timestamp;
        }

        public struct SDL_UserEvent
        {
            public uint type;

            public uint reserved;

            public ulong timestamp;

            public uint windowID;

            public int code;

            public IntPtr data1;

            public IntPtr data2;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_Event
        {
            [FieldOffset(0)]
            public uint type;

            [FieldOffset(0)]
            public SDL_CommonEvent common;

            [FieldOffset(0)]
            public SDL_DisplayEvent display;

            [FieldOffset(0)]
            public SDL_WindowEvent window;

            [FieldOffset(0)]
            public SDL_KeyboardDeviceEvent kdevice;

            [FieldOffset(0)]
            public SDL_KeyboardEvent key;

            [FieldOffset(0)]
            public SDL_TextEditingEvent edit;

            [FieldOffset(0)]
            public SDL_TextEditingCandidatesEvent edit_candidates;

            [FieldOffset(0)]
            public SDL_TextInputEvent text;

            [FieldOffset(0)]
            public SDL_MouseDeviceEvent mdevice;

            [FieldOffset(0)]
            public SDL_MouseMotionEvent motion;

            [FieldOffset(0)]
            public SDL_MouseButtonEvent button;

            [FieldOffset(0)]
            public SDL_MouseWheelEvent wheel;

            [FieldOffset(0)]
            public SDL_JoyDeviceEvent jdevice;

            [FieldOffset(0)]
            public SDL_JoyAxisEvent jaxis;

            [FieldOffset(0)]
            public SDL_JoyBallEvent jball;

            [FieldOffset(0)]
            public SDL_JoyHatEvent jhat;

            [FieldOffset(0)]
            public SDL_JoyButtonEvent jbutton;

            [FieldOffset(0)]
            public SDL_JoyBatteryEvent jbattery;

            [FieldOffset(0)]
            public SDL_GamepadDeviceEvent gdevice;

            [FieldOffset(0)]
            public SDL_GamepadAxisEvent gaxis;

            [FieldOffset(0)]
            public SDL_GamepadButtonEvent gbutton;

            [FieldOffset(0)]
            public SDL_GamepadTouchpadEvent gtouchpad;

            [FieldOffset(0)]
            public SDL_GamepadSensorEvent gsensor;

            [FieldOffset(0)]
            public SDL_AudioDeviceEvent adevice;

            [FieldOffset(0)]
            public SDL_CameraDeviceEvent cdevice;

            [FieldOffset(0)]
            public SDL_SensorEvent sensor;

            [FieldOffset(0)]
            public SDL_QuitEvent quit;

            [FieldOffset(0)]
            public SDL_UserEvent user;

            [FieldOffset(0)]
            public SDL_TouchFingerEvent tfinger;

            [FieldOffset(0)]
            public SDL_PinchFingerEvent pinch;

            [FieldOffset(0)]
            public SDL_PenProximityEvent pproximity;

            [FieldOffset(0)]
            public SDL_PenTouchEvent ptouch;

            [FieldOffset(0)]
            public SDL_PenMotionEvent pmotion;

            [FieldOffset(0)]
            public SDL_PenButtonEvent pbutton;

            [FieldOffset(0)]
            public SDL_PenAxisEvent paxis;

            [FieldOffset(0)]
            public SDL_RenderEvent render;

            [FieldOffset(0)]
            public SDL_DropEvent drop;

            [FieldOffset(0)]
            public SDL_ClipboardEvent clipboard;

            [FieldOffset(0)]
            public unsafe fixed byte padding[128];
        }

        public enum SDL_EventAction
        {
            SDL_ADDEVENT,
            SDL_PEEKEVENT,
            SDL_GETEVENT
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate bool SDL_EventFilter(IntPtr userdata, SDL_Event* evt);

        public enum SDL_Folder
        {
            SDL_FOLDER_HOME,
            SDL_FOLDER_DESKTOP,
            SDL_FOLDER_DOCUMENTS,
            SDL_FOLDER_DOWNLOADS,
            SDL_FOLDER_MUSIC,
            SDL_FOLDER_PICTURES,
            SDL_FOLDER_PUBLICSHARE,
            SDL_FOLDER_SAVEDGAMES,
            SDL_FOLDER_SCREENSHOTS,
            SDL_FOLDER_TEMPLATES,
            SDL_FOLDER_VIDEOS,
            SDL_FOLDER_COUNT
        }

        public enum SDL_PathType
        {
            SDL_PATHTYPE_NONE,
            SDL_PATHTYPE_FILE,
            SDL_PATHTYPE_DIRECTORY,
            SDL_PATHTYPE_OTHER
        }

        public struct SDL_PathInfo
        {
            public SDL_PathType type;

            public ulong size;

            public long create_time;

            public long modify_time;

            public long access_time;
        }

        [Flags]
        public enum SDL_GlobFlags : uint
        {
            SDL_GLOB_CASEINSENSITIVE = 1u
        }

        public enum SDL_EnumerationResult
        {
            SDL_ENUM_CONTINUE,
            SDL_ENUM_SUCCESS,
            SDL_ENUM_FAILURE
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate SDL_EnumerationResult SDL_EnumerateDirectoryCallback(IntPtr userdata, byte* dirname, byte* fname);

        public enum SDL_GPUPrimitiveType
        {
            SDL_GPU_PRIMITIVETYPE_TRIANGLELIST,
            SDL_GPU_PRIMITIVETYPE_TRIANGLESTRIP,
            SDL_GPU_PRIMITIVETYPE_LINELIST,
            SDL_GPU_PRIMITIVETYPE_LINESTRIP,
            SDL_GPU_PRIMITIVETYPE_POINTLIST
        }

        public enum SDL_GPULoadOp
        {
            SDL_GPU_LOADOP_LOAD,
            SDL_GPU_LOADOP_CLEAR,
            SDL_GPU_LOADOP_DONT_CARE
        }

        public enum SDL_GPUStoreOp
        {
            SDL_GPU_STOREOP_STORE,
            SDL_GPU_STOREOP_DONT_CARE,
            SDL_GPU_STOREOP_RESOLVE,
            SDL_GPU_STOREOP_RESOLVE_AND_STORE
        }

        public enum SDL_GPUIndexElementSize
        {
            SDL_GPU_INDEXELEMENTSIZE_16BIT,
            SDL_GPU_INDEXELEMENTSIZE_32BIT
        }

        public enum SDL_GPUTextureFormat
        {
            SDL_GPU_TEXTUREFORMAT_INVALID,
            SDL_GPU_TEXTUREFORMAT_A8_UNORM,
            SDL_GPU_TEXTUREFORMAT_R8_UNORM,
            SDL_GPU_TEXTUREFORMAT_R8G8_UNORM,
            SDL_GPU_TEXTUREFORMAT_R8G8B8A8_UNORM,
            SDL_GPU_TEXTUREFORMAT_R16_UNORM,
            SDL_GPU_TEXTUREFORMAT_R16G16_UNORM,
            SDL_GPU_TEXTUREFORMAT_R16G16B16A16_UNORM,
            SDL_GPU_TEXTUREFORMAT_R10G10B10A2_UNORM,
            SDL_GPU_TEXTUREFORMAT_B5G6R5_UNORM,
            SDL_GPU_TEXTUREFORMAT_B5G5R5A1_UNORM,
            SDL_GPU_TEXTUREFORMAT_B4G4R4A4_UNORM,
            SDL_GPU_TEXTUREFORMAT_B8G8R8A8_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC1_RGBA_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC2_RGBA_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC3_RGBA_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC4_R_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC5_RG_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC7_RGBA_UNORM,
            SDL_GPU_TEXTUREFORMAT_BC6H_RGB_FLOAT,
            SDL_GPU_TEXTUREFORMAT_BC6H_RGB_UFLOAT,
            SDL_GPU_TEXTUREFORMAT_R8_SNORM,
            SDL_GPU_TEXTUREFORMAT_R8G8_SNORM,
            SDL_GPU_TEXTUREFORMAT_R8G8B8A8_SNORM,
            SDL_GPU_TEXTUREFORMAT_R16_SNORM,
            SDL_GPU_TEXTUREFORMAT_R16G16_SNORM,
            SDL_GPU_TEXTUREFORMAT_R16G16B16A16_SNORM,
            SDL_GPU_TEXTUREFORMAT_R16_FLOAT,
            SDL_GPU_TEXTUREFORMAT_R16G16_FLOAT,
            SDL_GPU_TEXTUREFORMAT_R16G16B16A16_FLOAT,
            SDL_GPU_TEXTUREFORMAT_R32_FLOAT,
            SDL_GPU_TEXTUREFORMAT_R32G32_FLOAT,
            SDL_GPU_TEXTUREFORMAT_R32G32B32A32_FLOAT,
            SDL_GPU_TEXTUREFORMAT_R11G11B10_UFLOAT,
            SDL_GPU_TEXTUREFORMAT_R8_UINT,
            SDL_GPU_TEXTUREFORMAT_R8G8_UINT,
            SDL_GPU_TEXTUREFORMAT_R8G8B8A8_UINT,
            SDL_GPU_TEXTUREFORMAT_R16_UINT,
            SDL_GPU_TEXTUREFORMAT_R16G16_UINT,
            SDL_GPU_TEXTUREFORMAT_R16G16B16A16_UINT,
            SDL_GPU_TEXTUREFORMAT_R32_UINT,
            SDL_GPU_TEXTUREFORMAT_R32G32_UINT,
            SDL_GPU_TEXTUREFORMAT_R32G32B32A32_UINT,
            SDL_GPU_TEXTUREFORMAT_R8_INT,
            SDL_GPU_TEXTUREFORMAT_R8G8_INT,
            SDL_GPU_TEXTUREFORMAT_R8G8B8A8_INT,
            SDL_GPU_TEXTUREFORMAT_R16_INT,
            SDL_GPU_TEXTUREFORMAT_R16G16_INT,
            SDL_GPU_TEXTUREFORMAT_R16G16B16A16_INT,
            SDL_GPU_TEXTUREFORMAT_R32_INT,
            SDL_GPU_TEXTUREFORMAT_R32G32_INT,
            SDL_GPU_TEXTUREFORMAT_R32G32B32A32_INT,
            SDL_GPU_TEXTUREFORMAT_R8G8B8A8_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_B8G8R8A8_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_BC1_RGBA_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_BC2_RGBA_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_BC3_RGBA_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_BC7_RGBA_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_D16_UNORM,
            SDL_GPU_TEXTUREFORMAT_D24_UNORM,
            SDL_GPU_TEXTUREFORMAT_D32_FLOAT,
            SDL_GPU_TEXTUREFORMAT_D24_UNORM_S8_UINT,
            SDL_GPU_TEXTUREFORMAT_D32_FLOAT_S8_UINT,
            SDL_GPU_TEXTUREFORMAT_ASTC_4x4_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_5x4_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_5x5_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_6x5_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_6x6_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x5_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x6_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x8_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x5_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x6_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x8_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x10_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_12x10_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_12x12_UNORM,
            SDL_GPU_TEXTUREFORMAT_ASTC_4x4_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_5x4_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_5x5_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_6x5_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_6x6_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x5_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x6_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x8_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x5_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x6_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x8_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x10_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_12x10_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_12x12_UNORM_SRGB,
            SDL_GPU_TEXTUREFORMAT_ASTC_4x4_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_5x4_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_5x5_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_6x5_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_6x6_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x5_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x6_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_8x8_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x5_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x6_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x8_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_10x10_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_12x10_FLOAT,
            SDL_GPU_TEXTUREFORMAT_ASTC_12x12_FLOAT
        }

        [Flags]
        public enum SDL_GPUTextureUsageFlags : uint
        {
            SDL_GPU_TEXTUREUSAGE_SAMPLER = 1u,
            SDL_GPU_TEXTUREUSAGE_COLOR_TARGET = 2u,
            SDL_GPU_TEXTUREUSAGE_DEPTH_STENCIL_TARGET = 4u,
            SDL_GPU_TEXTUREUSAGE_GRAPHICS_STORAGE_READ = 8u,
            SDL_GPU_TEXTUREUSAGE_COMPUTE_STORAGE_READ = 0x10u,
            SDL_GPU_TEXTUREUSAGE_COMPUTE_STORAGE_WRITE = 0x20u
        }

        public enum SDL_GPUTextureType
        {
            SDL_GPU_TEXTURETYPE_2D,
            SDL_GPU_TEXTURETYPE_2D_ARRAY,
            SDL_GPU_TEXTURETYPE_3D,
            SDL_GPU_TEXTURETYPE_CUBE,
            SDL_GPU_TEXTURETYPE_CUBE_ARRAY
        }

        public enum SDL_GPUSampleCount
        {
            SDL_GPU_SAMPLECOUNT_1,
            SDL_GPU_SAMPLECOUNT_2,
            SDL_GPU_SAMPLECOUNT_4,
            SDL_GPU_SAMPLECOUNT_8
        }

        public enum SDL_GPUCubeMapFace
        {
            SDL_GPU_CUBEMAPFACE_POSITIVEX,
            SDL_GPU_CUBEMAPFACE_NEGATIVEX,
            SDL_GPU_CUBEMAPFACE_POSITIVEY,
            SDL_GPU_CUBEMAPFACE_NEGATIVEY,
            SDL_GPU_CUBEMAPFACE_POSITIVEZ,
            SDL_GPU_CUBEMAPFACE_NEGATIVEZ
        }

        [Flags]
        public enum SDL_GPUBufferUsageFlags : uint
        {
            SDL_GPU_BUFFERUSAGE_VERTEX = 1u,
            SDL_GPU_BUFFERUSAGE_INDEX = 2u,
            SDL_GPU_BUFFERUSAGE_INDIRECT = 4u,
            SDL_GPU_BUFFERUSAGE_GRAPHICS_STORAGE_READ = 8u,
            SDL_GPU_BUFFERUSAGE_COMPUTE_STORAGE_READ = 0x10u,
            SDL_GPU_BUFFERUSAGE_COMPUTE_STORAGE_WRITE = 0x20u
        }

        public enum SDL_GPUTransferBufferUsage
        {
            SDL_GPU_TRANSFERBUFFERUSAGE_UPLOAD,
            SDL_GPU_TRANSFERBUFFERUSAGE_DOWNLOAD
        }

        public enum SDL_GPUShaderStage
        {
            SDL_GPU_SHADERSTAGE_VERTEX,
            SDL_GPU_SHADERSTAGE_FRAGMENT
        }

        [Flags]
        public enum SDL_GPUShaderFormat : uint
        {
            SDL_GPU_SHADERFORMAT_PRIVATE = 1u,
            SDL_GPU_SHADERFORMAT_SPIRV = 2u,
            SDL_GPU_SHADERFORMAT_DXBC = 4u,
            SDL_GPU_SHADERFORMAT_DXIL = 8u,
            SDL_GPU_SHADERFORMAT_MSL = 0x10u,
            SDL_GPU_SHADERFORMAT_METALLIB = 0x20u
        }

        public enum SDL_GPUVertexElementFormat
        {
            SDL_GPU_VERTEXELEMENTFORMAT_INVALID,
            SDL_GPU_VERTEXELEMENTFORMAT_INT,
            SDL_GPU_VERTEXELEMENTFORMAT_INT2,
            SDL_GPU_VERTEXELEMENTFORMAT_INT3,
            SDL_GPU_VERTEXELEMENTFORMAT_INT4,
            SDL_GPU_VERTEXELEMENTFORMAT_UINT,
            SDL_GPU_VERTEXELEMENTFORMAT_UINT2,
            SDL_GPU_VERTEXELEMENTFORMAT_UINT3,
            SDL_GPU_VERTEXELEMENTFORMAT_UINT4,
            SDL_GPU_VERTEXELEMENTFORMAT_FLOAT,
            SDL_GPU_VERTEXELEMENTFORMAT_FLOAT2,
            SDL_GPU_VERTEXELEMENTFORMAT_FLOAT3,
            SDL_GPU_VERTEXELEMENTFORMAT_FLOAT4,
            SDL_GPU_VERTEXELEMENTFORMAT_BYTE2,
            SDL_GPU_VERTEXELEMENTFORMAT_BYTE4,
            SDL_GPU_VERTEXELEMENTFORMAT_UBYTE2,
            SDL_GPU_VERTEXELEMENTFORMAT_UBYTE4,
            SDL_GPU_VERTEXELEMENTFORMAT_BYTE2_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_BYTE4_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_UBYTE2_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_UBYTE4_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_SHORT2,
            SDL_GPU_VERTEXELEMENTFORMAT_SHORT4,
            SDL_GPU_VERTEXELEMENTFORMAT_USHORT2,
            SDL_GPU_VERTEXELEMENTFORMAT_USHORT4,
            SDL_GPU_VERTEXELEMENTFORMAT_SHORT2_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_SHORT4_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_USHORT2_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_USHORT4_NORM,
            SDL_GPU_VERTEXELEMENTFORMAT_HALF2,
            SDL_GPU_VERTEXELEMENTFORMAT_HALF4
        }

        public enum SDL_GPUVertexInputRate
        {
            SDL_GPU_VERTEXINPUTRATE_VERTEX,
            SDL_GPU_VERTEXINPUTRATE_INSTANCE
        }

        public enum SDL_GPUFillMode
        {
            SDL_GPU_FILLMODE_FILL,
            SDL_GPU_FILLMODE_LINE
        }

        public enum SDL_GPUCullMode
        {
            SDL_GPU_CULLMODE_NONE,
            SDL_GPU_CULLMODE_FRONT,
            SDL_GPU_CULLMODE_BACK
        }

        public enum SDL_GPUFrontFace
        {
            SDL_GPU_FRONTFACE_COUNTER_CLOCKWISE,
            SDL_GPU_FRONTFACE_CLOCKWISE
        }

        public enum SDL_GPUCompareOp
        {
            SDL_GPU_COMPAREOP_INVALID,
            SDL_GPU_COMPAREOP_NEVER,
            SDL_GPU_COMPAREOP_LESS,
            SDL_GPU_COMPAREOP_EQUAL,
            SDL_GPU_COMPAREOP_LESS_OR_EQUAL,
            SDL_GPU_COMPAREOP_GREATER,
            SDL_GPU_COMPAREOP_NOT_EQUAL,
            SDL_GPU_COMPAREOP_GREATER_OR_EQUAL,
            SDL_GPU_COMPAREOP_ALWAYS
        }

        public enum SDL_GPUStencilOp
        {
            SDL_GPU_STENCILOP_INVALID,
            SDL_GPU_STENCILOP_KEEP,
            SDL_GPU_STENCILOP_ZERO,
            SDL_GPU_STENCILOP_REPLACE,
            SDL_GPU_STENCILOP_INCREMENT_AND_CLAMP,
            SDL_GPU_STENCILOP_DECREMENT_AND_CLAMP,
            SDL_GPU_STENCILOP_INVERT,
            SDL_GPU_STENCILOP_INCREMENT_AND_WRAP,
            SDL_GPU_STENCILOP_DECREMENT_AND_WRAP
        }

        public enum SDL_GPUBlendOp
        {
            SDL_GPU_BLENDOP_INVALID,
            SDL_GPU_BLENDOP_ADD,
            SDL_GPU_BLENDOP_SUBTRACT,
            SDL_GPU_BLENDOP_REVERSE_SUBTRACT,
            SDL_GPU_BLENDOP_MIN,
            SDL_GPU_BLENDOP_MAX
        }

        public enum SDL_GPUBlendFactor
        {
            SDL_GPU_BLENDFACTOR_INVALID,
            SDL_GPU_BLENDFACTOR_ZERO,
            SDL_GPU_BLENDFACTOR_ONE,
            SDL_GPU_BLENDFACTOR_SRC_COLOR,
            SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_COLOR,
            SDL_GPU_BLENDFACTOR_DST_COLOR,
            SDL_GPU_BLENDFACTOR_ONE_MINUS_DST_COLOR,
            SDL_GPU_BLENDFACTOR_SRC_ALPHA,
            SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
            SDL_GPU_BLENDFACTOR_DST_ALPHA,
            SDL_GPU_BLENDFACTOR_ONE_MINUS_DST_ALPHA,
            SDL_GPU_BLENDFACTOR_CONSTANT_COLOR,
            SDL_GPU_BLENDFACTOR_ONE_MINUS_CONSTANT_COLOR,
            SDL_GPU_BLENDFACTOR_SRC_ALPHA_SATURATE
        }

        [Flags]
        public enum SDL_GPUColorComponentFlags : byte
        {
            SDL_GPU_COLORCOMPONENT_R = 1,
            SDL_GPU_COLORCOMPONENT_G = 2,
            SDL_GPU_COLORCOMPONENT_B = 4,
            SDL_GPU_COLORCOMPONENT_A = 8
        }

        public enum SDL_GPUFilter
        {
            SDL_GPU_FILTER_NEAREST,
            SDL_GPU_FILTER_LINEAR
        }

        public enum SDL_GPUSamplerMipmapMode
        {
            SDL_GPU_SAMPLERMIPMAPMODE_NEAREST,
            SDL_GPU_SAMPLERMIPMAPMODE_LINEAR
        }

        public enum SDL_GPUSamplerAddressMode
        {
            SDL_GPU_SAMPLERADDRESSMODE_REPEAT,
            SDL_GPU_SAMPLERADDRESSMODE_MIRRORED_REPEAT,
            SDL_GPU_SAMPLERADDRESSMODE_CLAMP_TO_EDGE
        }

        public enum SDL_GPUPresentMode
        {
            SDL_GPU_PRESENTMODE_VSYNC,
            SDL_GPU_PRESENTMODE_IMMEDIATE,
            SDL_GPU_PRESENTMODE_MAILBOX
        }

        public enum SDL_GPUSwapchainComposition
        {
            SDL_GPU_SWAPCHAINCOMPOSITION_SDR,
            SDL_GPU_SWAPCHAINCOMPOSITION_SDR_LINEAR,
            SDL_GPU_SWAPCHAINCOMPOSITION_HDR_EXTENDED_LINEAR,
            SDL_GPU_SWAPCHAINCOMPOSITION_HDR10_ST2084
        }

        public struct SDL_GPUViewport
        {
            public float x;

            public float y;

            public float w;

            public float h;

            public float min_depth;

            public float max_depth;
        }

        public struct SDL_GPUTextureTransferInfo
        {
            public IntPtr transfer_buffer;

            public uint offset;

            public uint pixels_per_row;

            public uint rows_per_layer;
        }

        public struct SDL_GPUTransferBufferLocation
        {
            public IntPtr transfer_buffer;

            public uint offset;
        }

        public struct SDL_GPUTextureLocation
        {
            public IntPtr texture;

            public uint mip_level;

            public uint layer;

            public uint x;

            public uint y;

            public uint z;
        }

        public struct SDL_GPUTextureRegion
        {
            public IntPtr texture;

            public uint mip_level;

            public uint layer;

            public uint x;

            public uint y;

            public uint z;

            public uint w;

            public uint h;

            public uint d;
        }

        public struct SDL_GPUBlitRegion
        {
            public IntPtr texture;

            public uint mip_level;

            public uint layer_or_depth_plane;

            public uint x;

            public uint y;

            public uint w;

            public uint h;
        }

        public struct SDL_GPUBufferLocation
        {
            public IntPtr buffer;

            public uint offset;
        }

        public struct SDL_GPUBufferRegion
        {
            public IntPtr buffer;

            public uint offset;

            public uint size;
        }

        public struct SDL_GPUIndirectDrawCommand
        {
            public uint num_vertices;

            public uint num_instances;

            public uint first_vertex;

            public uint first_instance;
        }

        public struct SDL_GPUIndexedIndirectDrawCommand
        {
            public uint num_indices;

            public uint num_instances;

            public uint first_index;

            public int vertex_offset;

            public uint first_instance;
        }

        public struct SDL_GPUIndirectDispatchCommand
        {
            public uint groupcount_x;

            public uint groupcount_y;

            public uint groupcount_z;
        }

        public struct SDL_GPUSamplerCreateInfo
        {
            public SDL_GPUFilter min_filter;

            public SDL_GPUFilter mag_filter;

            public SDL_GPUSamplerMipmapMode mipmap_mode;

            public SDL_GPUSamplerAddressMode address_mode_u;

            public SDL_GPUSamplerAddressMode address_mode_v;

            public SDL_GPUSamplerAddressMode address_mode_w;

            public float mip_lod_bias;

            public float max_anisotropy;

            public SDL_GPUCompareOp compare_op;

            public float min_lod;

            public float max_lod;

            public SDLBool enable_anisotropy;

            public SDLBool enable_compare;

            public byte padding1;

            public byte padding2;

            public uint props;
        }

        public struct SDL_GPUVertexBufferDescription
        {
            public uint slot;

            public uint pitch;

            public SDL_GPUVertexInputRate input_rate;

            public uint instance_step_rate;
        }

        public struct SDL_GPUVertexAttribute
        {
            public uint location;

            public uint buffer_slot;

            public SDL_GPUVertexElementFormat format;

            public uint offset;
        }

        public struct SDL_GPUVertexInputState
        {
            public unsafe SDL_GPUVertexBufferDescription* vertex_buffer_descriptions;

            public uint num_vertex_buffers;

            public unsafe SDL_GPUVertexAttribute* vertex_attributes;

            public uint num_vertex_attributes;
        }

        public struct SDL_GPUStencilOpState
        {
            public SDL_GPUStencilOp fail_op;

            public SDL_GPUStencilOp pass_op;

            public SDL_GPUStencilOp depth_fail_op;

            public SDL_GPUCompareOp compare_op;
        }

        public struct SDL_GPUColorTargetBlendState
        {
            public SDL_GPUBlendFactor src_color_blendfactor;

            public SDL_GPUBlendFactor dst_color_blendfactor;

            public SDL_GPUBlendOp color_blend_op;

            public SDL_GPUBlendFactor src_alpha_blendfactor;

            public SDL_GPUBlendFactor dst_alpha_blendfactor;

            public SDL_GPUBlendOp alpha_blend_op;

            public SDL_GPUColorComponentFlags color_write_mask;

            public SDLBool enable_blend;

            public SDLBool enable_color_write_mask;

            public byte padding1;

            public byte padding2;
        }

        public struct SDL_GPUShaderCreateInfo
        {
            public UIntPtr code_size;

            public unsafe byte* code;

            public unsafe byte* entrypoint;

            public SDL_GPUShaderFormat format;

            public SDL_GPUShaderStage stage;

            public uint num_samplers;

            public uint num_storage_textures;

            public uint num_storage_buffers;

            public uint num_uniform_buffers;

            public uint props;
        }

        public struct SDL_GPUTextureCreateInfo
        {
            public SDL_GPUTextureType type;

            public SDL_GPUTextureFormat format;

            public SDL_GPUTextureUsageFlags usage;

            public uint width;

            public uint height;

            public uint layer_count_or_depth;

            public uint num_levels;

            public SDL_GPUSampleCount sample_count;

            public uint props;
        }

        public struct SDL_GPUBufferCreateInfo
        {
            public SDL_GPUBufferUsageFlags usage;

            public uint size;

            public uint props;
        }

        public struct SDL_GPUTransferBufferCreateInfo
        {
            public SDL_GPUTransferBufferUsage usage;

            public uint size;

            public uint props;
        }

        public struct SDL_GPURasterizerState
        {
            public SDL_GPUFillMode fill_mode;

            public SDL_GPUCullMode cull_mode;

            public SDL_GPUFrontFace front_face;

            public float depth_bias_constant_factor;

            public float depth_bias_clamp;

            public float depth_bias_slope_factor;

            public SDLBool enable_depth_bias;

            public SDLBool enable_depth_clip;

            public byte padding1;

            public byte padding2;
        }

        public struct SDL_GPUMultisampleState
        {
            public SDL_GPUSampleCount sample_count;

            public uint sample_mask;

            public SDLBool enable_mask;

            public SDLBool enable_alpha_to_coverage;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_GPUDepthStencilState
        {
            public SDL_GPUCompareOp compare_op;

            public SDL_GPUStencilOpState back_stencil_state;

            public SDL_GPUStencilOpState front_stencil_state;

            public byte compare_mask;

            public byte write_mask;

            public SDLBool enable_depth_test;

            public SDLBool enable_depth_write;

            public SDLBool enable_stencil_test;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_GPUColorTargetDescription
        {
            public SDL_GPUTextureFormat format;

            public SDL_GPUColorTargetBlendState blend_state;
        }

        public struct SDL_GPUGraphicsPipelineTargetInfo
        {
            public unsafe SDL_GPUColorTargetDescription* color_target_descriptions;

            public uint num_color_targets;

            public SDL_GPUTextureFormat depth_stencil_format;

            public SDLBool has_depth_stencil_target;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_GPUGraphicsPipelineCreateInfo
        {
            public IntPtr vertex_shader;

            public IntPtr fragment_shader;

            public SDL_GPUVertexInputState vertex_input_state;

            public SDL_GPUPrimitiveType primitive_type;

            public SDL_GPURasterizerState rasterizer_state;

            public SDL_GPUMultisampleState multisample_state;

            public SDL_GPUDepthStencilState depth_stencil_state;

            public SDL_GPUGraphicsPipelineTargetInfo target_info;

            public uint props;
        }

        public struct SDL_GPUComputePipelineCreateInfo
        {
            public UIntPtr code_size;

            public unsafe byte* code;

            public unsafe byte* entrypoint;

            public SDL_GPUShaderFormat format;

            public uint num_samplers;

            public uint num_readonly_storage_textures;

            public uint num_readonly_storage_buffers;

            public uint num_readwrite_storage_textures;

            public uint num_readwrite_storage_buffers;

            public uint num_uniform_buffers;

            public uint threadcount_x;

            public uint threadcount_y;

            public uint threadcount_z;

            public uint props;
        }

        public struct SDL_GPUColorTargetInfo
        {
            public IntPtr texture;

            public uint mip_level;

            public uint layer_or_depth_plane;

            public SDL_FColor clear_color;

            public SDL_GPULoadOp load_op;

            public SDL_GPUStoreOp store_op;

            public IntPtr resolve_texture;

            public uint resolve_mip_level;

            public uint resolve_layer;

            public SDLBool cycle;

            public SDLBool cycle_resolve_texture;

            public byte padding1;

            public byte padding2;
        }

        public struct SDL_GPUDepthStencilTargetInfo
        {
            public IntPtr texture;

            public float clear_depth;

            public SDL_GPULoadOp load_op;

            public SDL_GPUStoreOp store_op;

            public SDL_GPULoadOp stencil_load_op;

            public SDL_GPUStoreOp stencil_store_op;

            public SDLBool cycle;

            public byte clear_stencil;

            public byte mip_level;

            public byte layer;
        }

        public struct SDL_GPUBlitInfo
        {
            public SDL_GPUBlitRegion source;

            public SDL_GPUBlitRegion destination;

            public SDL_GPULoadOp load_op;

            public SDL_FColor clear_color;

            public SDL_FlipMode flip_mode;

            public SDL_GPUFilter filter;

            public SDLBool cycle;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_GPUBufferBinding
        {
            public IntPtr buffer;

            public uint offset;
        }

        public struct SDL_GPUTextureSamplerBinding
        {
            public IntPtr texture;

            public IntPtr sampler;
        }

        public struct SDL_GPUStorageBufferReadWriteBinding
        {
            public IntPtr buffer;

            public SDLBool cycle;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_GPUStorageTextureReadWriteBinding
        {
            public IntPtr texture;

            public uint mip_level;

            public uint layer;

            public SDLBool cycle;

            public byte padding1;

            public byte padding2;

            public byte padding3;
        }

        public struct SDL_GPUVulkanOptions
        {
            public uint vulkan_api_version;

            public IntPtr feature_list;

            public IntPtr vulkan_10_physical_device_features;

            public uint device_extension_count;

            public unsafe byte** device_extension_names;

            public uint instance_extension_count;

            public unsafe byte** instance_extension_names;
        }

        public struct SDL_HapticDirection
        {
            public byte type;

            public unsafe fixed int dir[3];
        }

        public struct SDL_HapticConstant
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public short level;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        public struct SDL_HapticPeriodic
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public ushort period;

            public short magnitude;

            public short offset;

            public ushort phase;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        public struct SDL_HapticCondition
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public unsafe fixed ushort right_sat[3];

            public unsafe fixed ushort left_sat[3];

            public unsafe fixed short right_coeff[3];

            public unsafe fixed short left_coeff[3];

            public unsafe fixed ushort deadband[3];

            public unsafe fixed short center[3];
        }

        public struct SDL_HapticRamp
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public short start;

            public short end;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        public struct SDL_HapticLeftRight
        {
            public ushort type;

            public uint length;

            public ushort large_magnitude;

            public ushort small_magnitude;
        }

        public struct SDL_HapticCustom
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public byte channels;

            public ushort period;

            public ushort samples;

            public unsafe ushort* data;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_HapticEffect
        {
            [FieldOffset(0)]
            public ushort type;

            [FieldOffset(0)]
            public SDL_HapticConstant constant;

            [FieldOffset(0)]
            public SDL_HapticPeriodic periodic;

            [FieldOffset(0)]
            public SDL_HapticCondition condition;

            [FieldOffset(0)]
            public SDL_HapticRamp ramp;

            [FieldOffset(0)]
            public SDL_HapticLeftRight leftright;

            [FieldOffset(0)]
            public SDL_HapticCustom custom;
        }

        public enum SDL_hid_bus_type
        {
            SDL_HID_API_BUS_UNKNOWN,
            SDL_HID_API_BUS_USB,
            SDL_HID_API_BUS_BLUETOOTH,
            SDL_HID_API_BUS_I2C,
            SDL_HID_API_BUS_SPI
        }

        public struct SDL_hid_device_info
        {
            public unsafe byte* path;

            public ushort vendor_id;

            public ushort product_id;

            public unsafe byte* serial_number;

            public ushort release_number;

            public unsafe byte* manufacturer_string;

            public unsafe byte* product_string;

            public ushort usage_page;

            public ushort usage;

            public int interface_number;

            public int interface_class;

            public int interface_subclass;

            public int interface_protocol;

            public SDL_hid_bus_type bus_type;

            public unsafe SDL_hid_device_info* next;
        }

        public enum SDL_HintPriority
        {
            SDL_HINT_DEFAULT,
            SDL_HINT_NORMAL,
            SDL_HINT_OVERRIDE
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void SDL_HintCallback(IntPtr userdata, byte* name, byte* oldValue, byte* newValue);

        [Flags]
        public enum SDL_InitFlags : uint
        {
            SDL_INIT_TIMER = 1u,
            SDL_INIT_AUDIO = 0x10u,
            SDL_INIT_VIDEO = 0x20u,
            SDL_INIT_JOYSTICK = 0x200u,
            SDL_INIT_HAPTIC = 0x1000u,
            SDL_INIT_GAMEPAD = 0x2000u,
            SDL_INIT_EVENTS = 0x4000u,
            SDL_INIT_SENSOR = 0x8000u,
            SDL_INIT_CAMERA = 0x10000u
        }

        public enum SDL_AppResult
        {
            SDL_APP_CONTINUE,
            SDL_APP_SUCCESS,
            SDL_APP_FAILURE
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate SDL_AppResult SDL_AppInit_func(IntPtr appstate, int argc, IntPtr argv);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate SDL_AppResult SDL_AppIterate_func(IntPtr appstate);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate SDL_AppResult SDL_AppEvent_func(IntPtr appstate, SDL_Event* evt);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_AppQuit_func(IntPtr appstate, SDL_AppResult result);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_MainThreadCallback(IntPtr userdata);

        public struct SDL_Locale
        {
            public unsafe byte* language;

            public unsafe byte* country;
        }

        public enum SDL_LogCategory
        {
            SDL_LOG_CATEGORY_APPLICATION,
            SDL_LOG_CATEGORY_ERROR,
            SDL_LOG_CATEGORY_ASSERT,
            SDL_LOG_CATEGORY_SYSTEM,
            SDL_LOG_CATEGORY_AUDIO,
            SDL_LOG_CATEGORY_VIDEO,
            SDL_LOG_CATEGORY_RENDER,
            SDL_LOG_CATEGORY_INPUT,
            SDL_LOG_CATEGORY_TEST,
            SDL_LOG_CATEGORY_GPU,
            SDL_LOG_CATEGORY_RESERVED2,
            SDL_LOG_CATEGORY_RESERVED3,
            SDL_LOG_CATEGORY_RESERVED4,
            SDL_LOG_CATEGORY_RESERVED5,
            SDL_LOG_CATEGORY_RESERVED6,
            SDL_LOG_CATEGORY_RESERVED7,
            SDL_LOG_CATEGORY_RESERVED8,
            SDL_LOG_CATEGORY_RESERVED9,
            SDL_LOG_CATEGORY_RESERVED10,
            SDL_LOG_CATEGORY_CUSTOM
        }

        public enum SDL_LogPriority
        {
            SDL_LOG_PRIORITY_INVALID,
            SDL_LOG_PRIORITY_TRACE,
            SDL_LOG_PRIORITY_VERBOSE,
            SDL_LOG_PRIORITY_DEBUG,
            SDL_LOG_PRIORITY_INFO,
            SDL_LOG_PRIORITY_WARN,
            SDL_LOG_PRIORITY_ERROR,
            SDL_LOG_PRIORITY_CRITICAL,
            SDL_LOG_PRIORITY_COUNT
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void SDL_LogOutputFunction(IntPtr userdata, int category, SDL_LogPriority priority, byte* message);

        [Flags]
        public enum SDL_MessageBoxFlags : uint
        {
            SDL_MESSAGEBOX_ERROR = 0x10u,
            SDL_MESSAGEBOX_WARNING = 0x20u,
            SDL_MESSAGEBOX_INFORMATION = 0x40u,
            SDL_MESSAGEBOX_BUTTONS_LEFT_TO_RIGHT = 0x80u,
            SDL_MESSAGEBOX_BUTTONS_RIGHT_TO_LEFT = 0x100u
        }

        [Flags]
        public enum SDL_MessageBoxButtonFlags : uint
        {
            SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT = 1u,
            SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT = 2u
        }

        public struct SDL_MessageBoxButtonData
        {
            public SDL_MessageBoxButtonFlags flags;

            public int buttonID;

            public unsafe byte* text;
        }

        public struct SDL_MessageBoxColor
        {
            public byte r;

            public byte g;

            public byte b;
        }

        public enum SDL_MessageBoxColorType
        {
            SDL_MESSAGEBOX_COLOR_BACKGROUND,
            SDL_MESSAGEBOX_COLOR_TEXT,
            SDL_MESSAGEBOX_COLOR_BUTTON_BORDER,
            SDL_MESSAGEBOX_COLOR_BUTTON_BACKGROUND,
            SDL_MESSAGEBOX_COLOR_BUTTON_SELECTED,
            SDL_MESSAGEBOX_COLOR_COUNT
        }

        public struct SDL_MessageBoxColorScheme
        {
            public SDL_MessageBoxColor colors0;

            public SDL_MessageBoxColor colors1;

            public SDL_MessageBoxColor colors2;

            public SDL_MessageBoxColor colors3;

            public SDL_MessageBoxColor colors4;
        }

        public struct SDL_MessageBoxData
        {
            public SDL_MessageBoxFlags flags;

            public IntPtr window;

            public unsafe byte* title;

            public unsafe byte* message;

            public int numbuttons;

            public unsafe SDL_MessageBoxButtonData* buttons;

            public unsafe SDL_MessageBoxColorScheme* colorScheme;
        }

        public enum SDL_ProcessIO
        {
            SDL_PROCESS_STDIO_INHERITED,
            SDL_PROCESS_STDIO_NULL,
            SDL_PROCESS_STDIO_APP,
            SDL_PROCESS_STDIO_REDIRECT
        }

        public struct SDL_Vertex
        {
            public SDL_FPoint position;

            public SDL_FColor color;

            public SDL_FPoint tex_coord;
        }

        public enum SDL_TextureAccess
        {
            SDL_TEXTUREACCESS_STATIC,
            SDL_TEXTUREACCESS_STREAMING,
            SDL_TEXTUREACCESS_TARGET
        }

        public enum SDL_TextureAddressMode
        {
            SDL_TEXTURE_ADDRESS_INVALID = -1,
            SDL_TEXTURE_ADDRESS_AUTO,
            SDL_TEXTURE_ADDRESS_CLAMP,
            SDL_TEXTURE_ADDRESS_WRAP
        }

        public enum SDL_RendererLogicalPresentation
        {
            SDL_LOGICAL_PRESENTATION_DISABLED,
            SDL_LOGICAL_PRESENTATION_STRETCH,
            SDL_LOGICAL_PRESENTATION_LETTERBOX,
            SDL_LOGICAL_PRESENTATION_OVERSCAN,
            SDL_LOGICAL_PRESENTATION_INTEGER_SCALE
        }

        public struct SDL_Texture
        {
            public SDL_PixelFormat format;

            public int w;

            public int h;

            public int refcount;
        }

        public struct SDL_GPURenderStateCreateInfo
        {
            public IntPtr fragment_shader;

            public int num_sampler_bindings;

            public unsafe SDL_GPUTextureSamplerBinding* sampler_bindings;

            public int num_storage_textures;

            public unsafe IntPtr* storage_textures;

            public int num_storage_buffers;

            public unsafe IntPtr* storage_buffers;

            public uint props;
        }

        public struct SDL_StorageInterface
        {
            public uint version;

            public IntPtr close;

            public IntPtr ready;

            public IntPtr enumerate;

            public IntPtr info;

            public IntPtr read_file;

            public IntPtr write_file;

            public IntPtr mkdir;

            public IntPtr remove;

            public IntPtr rename;

            public IntPtr copy;

            public IntPtr space_remaining;
        }

        public enum SDL_Sandbox
        {
            SDL_SANDBOX_NONE,
            SDL_SANDBOX_UNKNOWN_CONTAINER,
            SDL_SANDBOX_FLATPAK,
            SDL_SANDBOX_SNAP,
            SDL_SANDBOX_MACOS
        }

        public struct SDL_DateTime
        {
            public int year;

            public int month;

            public int day;

            public int hour;

            public int minute;

            public int second;

            public int nanosecond;

            public int day_of_week;

            public int utc_offset;
        }

        public enum SDL_DateFormat
        {
            SDL_DATE_FORMAT_YYYYMMDD,
            SDL_DATE_FORMAT_DDMMYYYY,
            SDL_DATE_FORMAT_MMDDYYYY
        }

        public enum SDL_TimeFormat
        {
            SDL_TIME_FORMAT_24HR,
            SDL_TIME_FORMAT_12HR
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint SDL_TimerCallback(IntPtr userdata, uint timerID, uint interval);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ulong SDL_NSTimerCallback(IntPtr userdata, uint timerID, ulong interval);

        [Flags]
        public enum SDL_TrayEntryFlags : uint
        {
            SDL_TRAYENTRY_BUTTON = 1u,
            SDL_TRAYENTRY_CHECKBOX = 2u,
            SDL_TRAYENTRY_SUBMENU = 4u,
            SDL_TRAYENTRY_DISABLED = 0x80000000u,
            SDL_TRAYENTRY_CHECKED = 0x40000000u
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_TrayCallback(IntPtr userdata, IntPtr entry);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDL_main_func(int argc, IntPtr argv);

        private const string nativeLibName = "SDL3";

        public const string SDL_PROP_NAME_STRING = "SDL.name";

        public const string SDL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER = "SDL.thread.create.entry_function";

        public const string SDL_PROP_THREAD_CREATE_NAME_STRING = "SDL.thread.create.name";

        public const string SDL_PROP_THREAD_CREATE_USERDATA_POINTER = "SDL.thread.create.userdata";

        public const string SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER = "SDL.thread.create.stacksize";

        public const string SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER = "SDL.iostream.windows.handle";

        public const string SDL_PROP_IOSTREAM_STDIO_FILE_POINTER = "SDL.iostream.stdio.file";

        public const string SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER = "SDL.iostream.file_descriptor";

        public const string SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER = "SDL.iostream.android.aasset";

        public const string SDL_PROP_IOSTREAM_MEMORY_POINTER = "SDL.iostream.memory.base";

        public const string SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER = "SDL.iostream.memory.size";

        public const string SDL_PROP_IOSTREAM_MEMORY_FREE_FUNC_POINTER = "SDL.iostream.memory.free";

        public const string SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER = "SDL.iostream.dynamic.memory";

        public const string SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER = "SDL.iostream.dynamic.chunksize";

        public const string SDL_PROP_AUDIOSTREAM_AUTO_CLEANUP_BOOLEAN = "SDL.audiostream.auto_cleanup";

        public const string SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT = "SDL.surface.SDR_white_point";

        public const string SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT = "SDL.surface.HDR_headroom";

        public const string SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING = "SDL.surface.tonemap";

        public const string SDL_PROP_SURFACE_HOTSPOT_X_NUMBER = "SDL.surface.hotspot.x";

        public const string SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER = "SDL.surface.hotspot.y";

        public const string SDL_PROP_SURFACE_ROTATION_FLOAT = "SDL.surface.rotation";

        public const string SDL_PROP_GLOBAL_VIDEO_WAYLAND_WL_DISPLAY_POINTER = "SDL.video.wayland.wl_display";

        public const string SDL_PROP_DISPLAY_HDR_ENABLED_BOOLEAN = "SDL.display.HDR_enabled";

        public const string SDL_PROP_DISPLAY_KMSDRM_PANEL_ORIENTATION_NUMBER = "SDL.display.KMSDRM.panel_orientation";

        public const string SDL_PROP_DISPLAY_WAYLAND_WL_OUTPUT_POINTER = "SDL.display.wayland.wl_output";

        public const string SDL_PROP_DISPLAY_WINDOWS_HMONITOR_POINTER = "SDL.display.windows.hmonitor";

        public const string SDL_PROP_WINDOW_CREATE_ALWAYS_ON_TOP_BOOLEAN = "SDL.window.create.always_on_top";

        public const string SDL_PROP_WINDOW_CREATE_BORDERLESS_BOOLEAN = "SDL.window.create.borderless";

        public const string SDL_PROP_WINDOW_CREATE_CONSTRAIN_POPUP_BOOLEAN = "SDL.window.create.constrain_popup";

        public const string SDL_PROP_WINDOW_CREATE_FOCUSABLE_BOOLEAN = "SDL.window.create.focusable";

        public const string SDL_PROP_WINDOW_CREATE_EXTERNAL_GRAPHICS_CONTEXT_BOOLEAN = "SDL.window.create.external_graphics_context";

        public const string SDL_PROP_WINDOW_CREATE_FLAGS_NUMBER = "SDL.window.create.flags";

        public const string SDL_PROP_WINDOW_CREATE_FULLSCREEN_BOOLEAN = "SDL.window.create.fullscreen";

        public const string SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER = "SDL.window.create.height";

        public const string SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN = "SDL.window.create.hidden";

        public const string SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN = "SDL.window.create.high_pixel_density";

        public const string SDL_PROP_WINDOW_CREATE_MAXIMIZED_BOOLEAN = "SDL.window.create.maximized";

        public const string SDL_PROP_WINDOW_CREATE_MENU_BOOLEAN = "SDL.window.create.menu";

        public const string SDL_PROP_WINDOW_CREATE_METAL_BOOLEAN = "SDL.window.create.metal";

        public const string SDL_PROP_WINDOW_CREATE_MINIMIZED_BOOLEAN = "SDL.window.create.minimized";

        public const string SDL_PROP_WINDOW_CREATE_MODAL_BOOLEAN = "SDL.window.create.modal";

        public const string SDL_PROP_WINDOW_CREATE_MOUSE_GRABBED_BOOLEAN = "SDL.window.create.mouse_grabbed";

        public const string SDL_PROP_WINDOW_CREATE_OPENGL_BOOLEAN = "SDL.window.create.opengl";

        public const string SDL_PROP_WINDOW_CREATE_PARENT_POINTER = "SDL.window.create.parent";

        public const string SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN = "SDL.window.create.resizable";

        public const string SDL_PROP_WINDOW_CREATE_TITLE_STRING = "SDL.window.create.title";

        public const string SDL_PROP_WINDOW_CREATE_TRANSPARENT_BOOLEAN = "SDL.window.create.transparent";

        public const string SDL_PROP_WINDOW_CREATE_TOOLTIP_BOOLEAN = "SDL.window.create.tooltip";

        public const string SDL_PROP_WINDOW_CREATE_UTILITY_BOOLEAN = "SDL.window.create.utility";

        public const string SDL_PROP_WINDOW_CREATE_VULKAN_BOOLEAN = "SDL.window.create.vulkan";

        public const string SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER = "SDL.window.create.width";

        public const string SDL_PROP_WINDOW_CREATE_X_NUMBER = "SDL.window.create.x";

        public const string SDL_PROP_WINDOW_CREATE_Y_NUMBER = "SDL.window.create.y";

        public const string SDL_PROP_WINDOW_CREATE_COCOA_WINDOW_POINTER = "SDL.window.create.cocoa.window";

        public const string SDL_PROP_WINDOW_CREATE_COCOA_VIEW_POINTER = "SDL.window.create.cocoa.view";

        public const string SDL_PROP_WINDOW_CREATE_WINDOWSCENE_POINTER = "SDL.window.create.uikit.windowscene";

        public const string SDL_PROP_WINDOW_CREATE_WAYLAND_SURFACE_ROLE_CUSTOM_BOOLEAN = "SDL.window.create.wayland.surface_role_custom";

        public const string SDL_PROP_WINDOW_CREATE_WAYLAND_CREATE_EGL_WINDOW_BOOLEAN = "SDL.window.create.wayland.create_egl_window";

        public const string SDL_PROP_WINDOW_CREATE_WAYLAND_WL_SURFACE_POINTER = "SDL.window.create.wayland.wl_surface";

        public const string SDL_PROP_WINDOW_CREATE_WIN32_HWND_POINTER = "SDL.window.create.win32.hwnd";

        public const string SDL_PROP_WINDOW_CREATE_WIN32_PIXEL_FORMAT_HWND_POINTER = "SDL.window.create.win32.pixel_format_hwnd";

        public const string SDL_PROP_WINDOW_CREATE_X11_WINDOW_NUMBER = "SDL.window.create.x11.window";

        public const string SDL_PROP_WINDOW_CREATE_EMSCRIPTEN_CANVAS_ID_STRING = "SDL.window.create.emscripten.canvas_id";

        public const string SDL_PROP_WINDOW_CREATE_EMSCRIPTEN_KEYBOARD_ELEMENT_STRING = "SDL.window.create.emscripten.keyboard_element";

        public const string SDL_PROP_WINDOW_SHAPE_POINTER = "SDL.window.shape";

        public const string SDL_PROP_WINDOW_HDR_ENABLED_BOOLEAN = "SDL.window.HDR_enabled";

        public const string SDL_PROP_WINDOW_SDR_WHITE_LEVEL_FLOAT = "SDL.window.SDR_white_level";

        public const string SDL_PROP_WINDOW_HDR_HEADROOM_FLOAT = "SDL.window.HDR_headroom";

        public const string SDL_PROP_WINDOW_ANDROID_WINDOW_POINTER = "SDL.window.android.window";

        public const string SDL_PROP_WINDOW_ANDROID_SURFACE_POINTER = "SDL.window.android.surface";

        public const string SDL_PROP_WINDOW_UIKIT_WINDOW_POINTER = "SDL.window.uikit.window";

        public const string SDL_PROP_WINDOW_UIKIT_METAL_VIEW_TAG_NUMBER = "SDL.window.uikit.metal_view_tag";

        public const string SDL_PROP_WINDOW_UIKIT_OPENGL_FRAMEBUFFER_NUMBER = "SDL.window.uikit.opengl.framebuffer";

        public const string SDL_PROP_WINDOW_UIKIT_OPENGL_RENDERBUFFER_NUMBER = "SDL.window.uikit.opengl.renderbuffer";

        public const string SDL_PROP_WINDOW_UIKIT_OPENGL_RESOLVE_FRAMEBUFFER_NUMBER = "SDL.window.uikit.opengl.resolve_framebuffer";

        public const string SDL_PROP_WINDOW_KMSDRM_DEVICE_INDEX_NUMBER = "SDL.window.kmsdrm.dev_index";

        public const string SDL_PROP_WINDOW_KMSDRM_DRM_FD_NUMBER = "SDL.window.kmsdrm.drm_fd";

        public const string SDL_PROP_WINDOW_KMSDRM_GBM_DEVICE_POINTER = "SDL.window.kmsdrm.gbm_dev";

        public const string SDL_PROP_WINDOW_COCOA_WINDOW_POINTER = "SDL.window.cocoa.window";

        public const string SDL_PROP_WINDOW_COCOA_METAL_VIEW_TAG_NUMBER = "SDL.window.cocoa.metal_view_tag";

        public const string SDL_PROP_WINDOW_OPENVR_OVERLAY_ID_NUMBER = "SDL.window.openvr.overlay_id";

        public const string SDL_PROP_WINDOW_VIVANTE_DISPLAY_POINTER = "SDL.window.vivante.display";

        public const string SDL_PROP_WINDOW_VIVANTE_WINDOW_POINTER = "SDL.window.vivante.window";

        public const string SDL_PROP_WINDOW_VIVANTE_SURFACE_POINTER = "SDL.window.vivante.surface";

        public const string SDL_PROP_WINDOW_WIN32_HWND_POINTER = "SDL.window.win32.hwnd";

        public const string SDL_PROP_WINDOW_WIN32_HDC_POINTER = "SDL.window.win32.hdc";

        public const string SDL_PROP_WINDOW_WIN32_INSTANCE_POINTER = "SDL.window.win32.instance";

        public const string SDL_PROP_WINDOW_WAYLAND_DISPLAY_POINTER = "SDL.window.wayland.display";

        public const string SDL_PROP_WINDOW_WAYLAND_SURFACE_POINTER = "SDL.window.wayland.surface";

        public const string SDL_PROP_WINDOW_WAYLAND_VIEWPORT_POINTER = "SDL.window.wayland.viewport";

        public const string SDL_PROP_WINDOW_WAYLAND_EGL_WINDOW_POINTER = "SDL.window.wayland.egl_window";

        public const string SDL_PROP_WINDOW_WAYLAND_XDG_SURFACE_POINTER = "SDL.window.wayland.xdg_surface";

        public const string SDL_PROP_WINDOW_WAYLAND_XDG_TOPLEVEL_POINTER = "SDL.window.wayland.xdg_toplevel";

        public const string SDL_PROP_WINDOW_WAYLAND_XDG_TOPLEVEL_EXPORT_HANDLE_STRING = "SDL.window.wayland.xdg_toplevel_export_handle";

        public const string SDL_PROP_WINDOW_WAYLAND_XDG_POPUP_POINTER = "SDL.window.wayland.xdg_popup";

        public const string SDL_PROP_WINDOW_WAYLAND_XDG_POSITIONER_POINTER = "SDL.window.wayland.xdg_positioner";

        public const string SDL_PROP_WINDOW_X11_DISPLAY_POINTER = "SDL.window.x11.display";

        public const string SDL_PROP_WINDOW_X11_SCREEN_NUMBER = "SDL.window.x11.screen";

        public const string SDL_PROP_WINDOW_X11_WINDOW_NUMBER = "SDL.window.x11.window";

        public const string SDL_PROP_WINDOW_EMSCRIPTEN_CANVAS_ID_STRING = "SDL.window.emscripten.canvas_id";

        public const string SDL_PROP_WINDOW_EMSCRIPTEN_KEYBOARD_ELEMENT_STRING = "SDL.window.emscripten.keyboard_element";

        public const string SDL_PROP_FILE_DIALOG_FILTERS_POINTER = "SDL.filedialog.filters";

        public const string SDL_PROP_FILE_DIALOG_NFILTERS_NUMBER = "SDL.filedialog.nfilters";

        public const string SDL_PROP_FILE_DIALOG_WINDOW_POINTER = "SDL.filedialog.window";

        public const string SDL_PROP_FILE_DIALOG_LOCATION_STRING = "SDL.filedialog.location";

        public const string SDL_PROP_FILE_DIALOG_MANY_BOOLEAN = "SDL.filedialog.many";

        public const string SDL_PROP_FILE_DIALOG_TITLE_STRING = "SDL.filedialog.title";

        public const string SDL_PROP_FILE_DIALOG_ACCEPT_STRING = "SDL.filedialog.accept";

        public const string SDL_PROP_FILE_DIALOG_CANCEL_STRING = "SDL.filedialog.cancel";

        public const string SDL_PROP_JOYSTICK_CAP_MONO_LED_BOOLEAN = "SDL.joystick.cap.mono_led";

        public const string SDL_PROP_JOYSTICK_CAP_RGB_LED_BOOLEAN = "SDL.joystick.cap.rgb_led";

        public const string SDL_PROP_JOYSTICK_CAP_PLAYER_LED_BOOLEAN = "SDL.joystick.cap.player_led";

        public const string SDL_PROP_JOYSTICK_CAP_RUMBLE_BOOLEAN = "SDL.joystick.cap.rumble";

        public const string SDL_PROP_JOYSTICK_CAP_TRIGGER_RUMBLE_BOOLEAN = "SDL.joystick.cap.trigger_rumble";

        public const string SDL_PROP_TEXTINPUT_TYPE_NUMBER = "SDL.textinput.type";

        public const string SDL_PROP_TEXTINPUT_CAPITALIZATION_NUMBER = "SDL.textinput.capitalization";

        public const string SDL_PROP_TEXTINPUT_AUTOCORRECT_BOOLEAN = "SDL.textinput.autocorrect";

        public const string SDL_PROP_TEXTINPUT_MULTILINE_BOOLEAN = "SDL.textinput.multiline";

        public const string SDL_PROP_TEXTINPUT_ANDROID_INPUTTYPE_NUMBER = "SDL.textinput.android.inputtype";

        public const string SDL_PROP_GPU_DEVICE_CREATE_DEBUGMODE_BOOLEAN = "SDL.gpu.device.create.debugmode";

        public const string SDL_PROP_GPU_DEVICE_CREATE_PREFERLOWPOWER_BOOLEAN = "SDL.gpu.device.create.preferlowpower";

        public const string SDL_PROP_GPU_DEVICE_CREATE_VERBOSE_BOOLEAN = "SDL.gpu.device.create.verbose";

        public const string SDL_PROP_GPU_DEVICE_CREATE_NAME_STRING = "SDL.gpu.device.create.name";

        public const string SDL_PROP_GPU_DEVICE_CREATE_FEATURE_CLIP_DISTANCE_BOOLEAN = "SDL.gpu.device.create.feature.clip_distance";

        public const string SDL_PROP_GPU_DEVICE_CREATE_FEATURE_DEPTH_CLAMPING_BOOLEAN = "SDL.gpu.device.create.feature.depth_clamping";

        public const string SDL_PROP_GPU_DEVICE_CREATE_FEATURE_INDIRECT_DRAW_FIRST_INSTANCE_BOOLEAN = "SDL.gpu.device.create.feature.indirect_draw_first_instance";

        public const string SDL_PROP_GPU_DEVICE_CREATE_FEATURE_ANISOTROPY_BOOLEAN = "SDL.gpu.device.create.feature.anisotropy";

        public const string SDL_PROP_GPU_DEVICE_CREATE_SHADERS_PRIVATE_BOOLEAN = "SDL.gpu.device.create.shaders.private";

        public const string SDL_PROP_GPU_DEVICE_CREATE_SHADERS_SPIRV_BOOLEAN = "SDL.gpu.device.create.shaders.spirv";

        public const string SDL_PROP_GPU_DEVICE_CREATE_SHADERS_DXBC_BOOLEAN = "SDL.gpu.device.create.shaders.dxbc";

        public const string SDL_PROP_GPU_DEVICE_CREATE_SHADERS_DXIL_BOOLEAN = "SDL.gpu.device.create.shaders.dxil";

        public const string SDL_PROP_GPU_DEVICE_CREATE_SHADERS_MSL_BOOLEAN = "SDL.gpu.device.create.shaders.msl";

        public const string SDL_PROP_GPU_DEVICE_CREATE_SHADERS_METALLIB_BOOLEAN = "SDL.gpu.device.create.shaders.metallib";

        public const string SDL_PROP_GPU_DEVICE_CREATE_D3D12_ALLOW_FEWER_RESOURCE_SLOTS_BOOLEAN = "SDL.gpu.device.create.d3d12.allowtier1resourcebinding";

        public const string SDL_PROP_GPU_DEVICE_CREATE_D3D12_SEMANTIC_NAME_STRING = "SDL.gpu.device.create.d3d12.semantic";

        public const string SDL_PROP_GPU_DEVICE_CREATE_VULKAN_REQUIRE_HARDWARE_ACCELERATION_BOOLEAN = "SDL.gpu.device.create.vulkan.requirehardwareacceleration";

        public const string SDL_PROP_GPU_DEVICE_CREATE_VULKAN_OPTIONS_POINTER = "SDL.gpu.device.create.vulkan.options";

        public const string SDL_PROP_GPU_DEVICE_NAME_STRING = "SDL.gpu.device.name";

        public const string SDL_PROP_GPU_DEVICE_DRIVER_NAME_STRING = "SDL.gpu.device.driver_name";

        public const string SDL_PROP_GPU_DEVICE_DRIVER_VERSION_STRING = "SDL.gpu.device.driver_version";

        public const string SDL_PROP_GPU_DEVICE_DRIVER_INFO_STRING = "SDL.gpu.device.driver_info";

        public const string SDL_PROP_GPU_COMPUTEPIPELINE_CREATE_NAME_STRING = "SDL.gpu.computepipeline.create.name";

        public const string SDL_PROP_GPU_GRAPHICSPIPELINE_CREATE_NAME_STRING = "SDL.gpu.graphicspipeline.create.name";

        public const string SDL_PROP_GPU_SAMPLER_CREATE_NAME_STRING = "SDL.gpu.sampler.create.name";

        public const string SDL_PROP_GPU_SHADER_CREATE_NAME_STRING = "SDL.gpu.shader.create.name";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_R_FLOAT = "SDL.gpu.texture.create.d3d12.clear.r";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_G_FLOAT = "SDL.gpu.texture.create.d3d12.clear.g";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_B_FLOAT = "SDL.gpu.texture.create.d3d12.clear.b";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_A_FLOAT = "SDL.gpu.texture.create.d3d12.clear.a";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_DEPTH_FLOAT = "SDL.gpu.texture.create.d3d12.clear.depth";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_STENCIL_NUMBER = "SDL.gpu.texture.create.d3d12.clear.stencil";

        public const string SDL_PROP_GPU_TEXTURE_CREATE_NAME_STRING = "SDL.gpu.texture.create.name";

        public const string SDL_PROP_GPU_BUFFER_CREATE_NAME_STRING = "SDL.gpu.buffer.create.name";

        public const string SDL_PROP_GPU_TRANSFERBUFFER_CREATE_NAME_STRING = "SDL.gpu.transferbuffer.create.name";

        public const string SDL_PROP_HIDAPI_LIBUSB_DEVICE_HANDLE_POINTER = "SDL.hidapi.libusb.device.handle";

        public const string SDL_HINT_ALLOW_ALT_TAB_WHILE_GRABBED = "SDL_ALLOW_ALT_TAB_WHILE_GRABBED";

        public const string SDL_HINT_ANDROID_ALLOW_RECREATE_ACTIVITY = "SDL_ANDROID_ALLOW_RECREATE_ACTIVITY";

        public const string SDL_HINT_ANDROID_BLOCK_ON_PAUSE = "SDL_ANDROID_BLOCK_ON_PAUSE";

        public const string SDL_HINT_ANDROID_LOW_LATENCY_AUDIO = "SDL_ANDROID_LOW_LATENCY_AUDIO";

        public const string SDL_HINT_ANDROID_TRAP_BACK_BUTTON = "SDL_ANDROID_TRAP_BACK_BUTTON";

        public const string SDL_HINT_APP_ID = "SDL_APP_ID";

        public const string SDL_HINT_APP_NAME = "SDL_APP_NAME";

        public const string SDL_HINT_APPLE_TV_CONTROLLER_UI_EVENTS = "SDL_APPLE_TV_CONTROLLER_UI_EVENTS";

        public const string SDL_HINT_APPLE_TV_REMOTE_ALLOW_ROTATION = "SDL_APPLE_TV_REMOTE_ALLOW_ROTATION";

        public const string SDL_HINT_AUDIO_ALSA_DEFAULT_DEVICE = "SDL_AUDIO_ALSA_DEFAULT_DEVICE";

        public const string SDL_HINT_AUDIO_ALSA_DEFAULT_PLAYBACK_DEVICE = "SDL_AUDIO_ALSA_DEFAULT_PLAYBACK_DEVICE";

        public const string SDL_HINT_AUDIO_ALSA_DEFAULT_RECORDING_DEVICE = "SDL_AUDIO_ALSA_DEFAULT_RECORDING_DEVICE";

        public const string SDL_HINT_AUDIO_CATEGORY = "SDL_AUDIO_CATEGORY";

        public const string SDL_HINT_AUDIO_CHANNELS = "SDL_AUDIO_CHANNELS";

        public const string SDL_HINT_AUDIO_DEVICE_APP_ICON_NAME = "SDL_AUDIO_DEVICE_APP_ICON_NAME";

        public const string SDL_HINT_AUDIO_DEVICE_SAMPLE_FRAMES = "SDL_AUDIO_DEVICE_SAMPLE_FRAMES";

        public const string SDL_HINT_AUDIO_DEVICE_STREAM_NAME = "SDL_AUDIO_DEVICE_STREAM_NAME";

        public const string SDL_HINT_AUDIO_DEVICE_STREAM_ROLE = "SDL_AUDIO_DEVICE_STREAM_ROLE";

        public const string SDL_HINT_AUDIO_DEVICE_RAW_STREAM = "SDL_AUDIO_DEVICE_RAW_STREAM";

        public const string SDL_HINT_AUDIO_DISK_INPUT_FILE = "SDL_AUDIO_DISK_INPUT_FILE";

        public const string SDL_HINT_AUDIO_DISK_OUTPUT_FILE = "SDL_AUDIO_DISK_OUTPUT_FILE";

        public const string SDL_HINT_AUDIO_DISK_TIMESCALE = "SDL_AUDIO_DISK_TIMESCALE";

        public const string SDL_HINT_AUDIO_DRIVER = "SDL_AUDIO_DRIVER";

        public const string SDL_HINT_AUDIO_DUMMY_TIMESCALE = "SDL_AUDIO_DUMMY_TIMESCALE";

        public const string SDL_HINT_AUDIO_FORMAT = "SDL_AUDIO_FORMAT";

        public const string SDL_HINT_AUDIO_FREQUENCY = "SDL_AUDIO_FREQUENCY";

        public const string SDL_HINT_AUDIO_INCLUDE_MONITORS = "SDL_AUDIO_INCLUDE_MONITORS";

        public const string SDL_HINT_AUTO_UPDATE_JOYSTICKS = "SDL_AUTO_UPDATE_JOYSTICKS";

        public const string SDL_HINT_AUTO_UPDATE_SENSORS = "SDL_AUTO_UPDATE_SENSORS";

        public const string SDL_HINT_BMP_SAVE_LEGACY_FORMAT = "SDL_BMP_SAVE_LEGACY_FORMAT";

        public const string SDL_HINT_CAMERA_DRIVER = "SDL_CAMERA_DRIVER";

        public const string SDL_HINT_CPU_FEATURE_MASK = "SDL_CPU_FEATURE_MASK";

        public const string SDL_HINT_JOYSTICK_DIRECTINPUT = "SDL_JOYSTICK_DIRECTINPUT";

        public const string SDL_HINT_FILE_DIALOG_DRIVER = "SDL_FILE_DIALOG_DRIVER";

        public const string SDL_HINT_DISPLAY_USABLE_BOUNDS = "SDL_DISPLAY_USABLE_BOUNDS";

        public const string SDL_HINT_INVALID_PARAM_CHECKS = "SDL_INVALID_PARAM_CHECKS";

        public const string SDL_HINT_EMSCRIPTEN_ASYNCIFY = "SDL_EMSCRIPTEN_ASYNCIFY";

        public const string SDL_HINT_EMSCRIPTEN_CANVAS_SELECTOR = "SDL_EMSCRIPTEN_CANVAS_SELECTOR";

        public const string SDL_HINT_EMSCRIPTEN_KEYBOARD_ELEMENT = "SDL_EMSCRIPTEN_KEYBOARD_ELEMENT";

        public const string SDL_HINT_ENABLE_SCREEN_KEYBOARD = "SDL_ENABLE_SCREEN_KEYBOARD";

        public const string SDL_HINT_EVDEV_DEVICES = "SDL_EVDEV_DEVICES";

        public const string SDL_HINT_EVENT_LOGGING = "SDL_EVENT_LOGGING";

        public const string SDL_HINT_FORCE_RAISEWINDOW = "SDL_FORCE_RAISEWINDOW";

        public const string SDL_HINT_FRAMEBUFFER_ACCELERATION = "SDL_FRAMEBUFFER_ACCELERATION";

        public const string SDL_HINT_GAMECONTROLLERCONFIG = "SDL_GAMECONTROLLERCONFIG";

        public const string SDL_HINT_GAMECONTROLLERCONFIG_FILE = "SDL_GAMECONTROLLERCONFIG_FILE";

        public const string SDL_HINT_GAMECONTROLLERTYPE = "SDL_GAMECONTROLLERTYPE";

        public const string SDL_HINT_GAMECONTROLLER_IGNORE_DEVICES = "SDL_GAMECONTROLLER_IGNORE_DEVICES";

        public const string SDL_HINT_GAMECONTROLLER_IGNORE_DEVICES_EXCEPT = "SDL_GAMECONTROLLER_IGNORE_DEVICES_EXCEPT";

        public const string SDL_HINT_GAMECONTROLLER_SENSOR_FUSION = "SDL_GAMECONTROLLER_SENSOR_FUSION";

        public const string SDL_HINT_GDK_TEXTINPUT_DEFAULT_TEXT = "SDL_GDK_TEXTINPUT_DEFAULT_TEXT";

        public const string SDL_HINT_GDK_TEXTINPUT_DESCRIPTION = "SDL_GDK_TEXTINPUT_DESCRIPTION";

        public const string SDL_HINT_GDK_TEXTINPUT_MAX_LENGTH = "SDL_GDK_TEXTINPUT_MAX_LENGTH";

        public const string SDL_HINT_GDK_TEXTINPUT_SCOPE = "SDL_GDK_TEXTINPUT_SCOPE";

        public const string SDL_HINT_GDK_TEXTINPUT_TITLE = "SDL_GDK_TEXTINPUT_TITLE";

        public const string SDL_HINT_HIDAPI_LIBUSB = "SDL_HIDAPI_LIBUSB";

        public const string SDL_HINT_HIDAPI_LIBUSB_GAMECUBE = "SDL_HIDAPI_LIBUSB_GAMECUBE";

        public const string SDL_HINT_HIDAPI_LIBUSB_WHITELIST = "SDL_HIDAPI_LIBUSB_WHITELIST";

        public const string SDL_HINT_HIDAPI_UDEV = "SDL_HIDAPI_UDEV";

        public const string SDL_HINT_GPU_DRIVER = "SDL_GPU_DRIVER";

        public const string SDL_HINT_HIDAPI_ENUMERATE_ONLY_CONTROLLERS = "SDL_HIDAPI_ENUMERATE_ONLY_CONTROLLERS";

        public const string SDL_HINT_HIDAPI_IGNORE_DEVICES = "SDL_HIDAPI_IGNORE_DEVICES";

        public const string SDL_HINT_IME_IMPLEMENTED_UI = "SDL_IME_IMPLEMENTED_UI";

        public const string SDL_HINT_IOS_HIDE_HOME_INDICATOR = "SDL_IOS_HIDE_HOME_INDICATOR";

        public const string SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS = "SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS";

        public const string SDL_HINT_JOYSTICK_ARCADESTICK_DEVICES = "SDL_JOYSTICK_ARCADESTICK_DEVICES";

        public const string SDL_HINT_JOYSTICK_ARCADESTICK_DEVICES_EXCLUDED = "SDL_JOYSTICK_ARCADESTICK_DEVICES_EXCLUDED";

        public const string SDL_HINT_JOYSTICK_BLACKLIST_DEVICES = "SDL_JOYSTICK_BLACKLIST_DEVICES";

        public const string SDL_HINT_JOYSTICK_BLACKLIST_DEVICES_EXCLUDED = "SDL_JOYSTICK_BLACKLIST_DEVICES_EXCLUDED";

        public const string SDL_HINT_JOYSTICK_DEVICE = "SDL_JOYSTICK_DEVICE";

        public const string SDL_HINT_JOYSTICK_ENHANCED_REPORTS = "SDL_JOYSTICK_ENHANCED_REPORTS";

        public const string SDL_HINT_JOYSTICK_FLIGHTSTICK_DEVICES = "SDL_JOYSTICK_FLIGHTSTICK_DEVICES";

        public const string SDL_HINT_JOYSTICK_FLIGHTSTICK_DEVICES_EXCLUDED = "SDL_JOYSTICK_FLIGHTSTICK_DEVICES_EXCLUDED";

        public const string SDL_HINT_JOYSTICK_GAMEINPUT = "SDL_JOYSTICK_GAMEINPUT";

        public const string SDL_HINT_JOYSTICK_GAMECUBE_DEVICES = "SDL_JOYSTICK_GAMECUBE_DEVICES";

        public const string SDL_HINT_JOYSTICK_GAMECUBE_DEVICES_EXCLUDED = "SDL_JOYSTICK_GAMECUBE_DEVICES_EXCLUDED";

        public const string SDL_HINT_JOYSTICK_HIDAPI = "SDL_JOYSTICK_HIDAPI";

        public const string SDL_HINT_JOYSTICK_HIDAPI_COMBINE_JOY_CONS = "SDL_JOYSTICK_HIDAPI_COMBINE_JOY_CONS";

        public const string SDL_HINT_JOYSTICK_HIDAPI_GAMECUBE = "SDL_JOYSTICK_HIDAPI_GAMECUBE";

        public const string SDL_HINT_JOYSTICK_HIDAPI_GAMECUBE_RUMBLE_BRAKE = "SDL_JOYSTICK_HIDAPI_GAMECUBE_RUMBLE_BRAKE";

        public const string SDL_HINT_JOYSTICK_HIDAPI_JOY_CONS = "SDL_JOYSTICK_HIDAPI_JOY_CONS";

        public const string SDL_HINT_JOYSTICK_HIDAPI_JOYCON_HOME_LED = "SDL_JOYSTICK_HIDAPI_JOYCON_HOME_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_LUNA = "SDL_JOYSTICK_HIDAPI_LUNA";

        public const string SDL_HINT_JOYSTICK_HIDAPI_NINTENDO_CLASSIC = "SDL_JOYSTICK_HIDAPI_NINTENDO_CLASSIC";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS3 = "SDL_JOYSTICK_HIDAPI_PS3";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS3_SIXAXIS_DRIVER = "SDL_JOYSTICK_HIDAPI_PS3_SIXAXIS_DRIVER";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS4 = "SDL_JOYSTICK_HIDAPI_PS4";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS4_REPORT_INTERVAL = "SDL_JOYSTICK_HIDAPI_PS4_REPORT_INTERVAL";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS5 = "SDL_JOYSTICK_HIDAPI_PS5";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS5_PLAYER_LED = "SDL_JOYSTICK_HIDAPI_PS5_PLAYER_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SHIELD = "SDL_JOYSTICK_HIDAPI_SHIELD";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STADIA = "SDL_JOYSTICK_HIDAPI_STADIA";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STEAM = "SDL_JOYSTICK_HIDAPI_STEAM";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STEAM_HOME_LED = "SDL_JOYSTICK_HIDAPI_STEAM_HOME_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STEAMDECK = "SDL_JOYSTICK_HIDAPI_STEAMDECK";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STEAM_HORI = "SDL_JOYSTICK_HIDAPI_STEAM_HORI";

        public const string SDL_HINT_JOYSTICK_HIDAPI_LG4FF = "SDL_JOYSTICK_HIDAPI_LG4FF";

        public const string SDL_HINT_JOYSTICK_HIDAPI_8BITDO = "SDL_JOYSTICK_HIDAPI_8BITDO";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SINPUT = "SDL_JOYSTICK_HIDAPI_SINPUT";

        public const string SDL_HINT_JOYSTICK_HIDAPI_ZUIKI = "SDL_JOYSTICK_HIDAPI_ZUIKI";

        public const string SDL_HINT_JOYSTICK_HIDAPI_FLYDIGI = "SDL_JOYSTICK_HIDAPI_FLYDIGI";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SWITCH = "SDL_JOYSTICK_HIDAPI_SWITCH";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SWITCH_HOME_LED = "SDL_JOYSTICK_HIDAPI_SWITCH_HOME_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SWITCH_PLAYER_LED = "SDL_JOYSTICK_HIDAPI_SWITCH_PLAYER_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SWITCH2 = "SDL_JOYSTICK_HIDAPI_SWITCH2";

        public const string SDL_HINT_JOYSTICK_HIDAPI_VERTICAL_JOY_CONS = "SDL_JOYSTICK_HIDAPI_VERTICAL_JOY_CONS";

        public const string SDL_HINT_JOYSTICK_HIDAPI_WII = "SDL_JOYSTICK_HIDAPI_WII";

        public const string SDL_HINT_JOYSTICK_HIDAPI_WII_PLAYER_LED = "SDL_JOYSTICK_HIDAPI_WII_PLAYER_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX = "SDL_JOYSTICK_HIDAPI_XBOX";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX_360 = "SDL_JOYSTICK_HIDAPI_XBOX_360";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX_360_PLAYER_LED = "SDL_JOYSTICK_HIDAPI_XBOX_360_PLAYER_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX_360_WIRELESS = "SDL_JOYSTICK_HIDAPI_XBOX_360_WIRELESS";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX_ONE = "SDL_JOYSTICK_HIDAPI_XBOX_ONE";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX_ONE_HOME_LED = "SDL_JOYSTICK_HIDAPI_XBOX_ONE_HOME_LED";

        public const string SDL_HINT_JOYSTICK_HIDAPI_GIP = "SDL_JOYSTICK_HIDAPI_GIP";

        public const string SDL_HINT_JOYSTICK_HIDAPI_GIP_RESET_FOR_METADATA = "SDL_JOYSTICK_HIDAPI_GIP_RESET_FOR_METADATA";

        public const string SDL_HINT_JOYSTICK_IOKIT = "SDL_JOYSTICK_IOKIT";

        public const string SDL_HINT_JOYSTICK_LINUX_CLASSIC = "SDL_JOYSTICK_LINUX_CLASSIC";

        public const string SDL_HINT_JOYSTICK_LINUX_DEADZONES = "SDL_JOYSTICK_LINUX_DEADZONES";

        public const string SDL_HINT_JOYSTICK_LINUX_DIGITAL_HATS = "SDL_JOYSTICK_LINUX_DIGITAL_HATS";

        public const string SDL_HINT_JOYSTICK_LINUX_HAT_DEADZONES = "SDL_JOYSTICK_LINUX_HAT_DEADZONES";

        public const string SDL_HINT_JOYSTICK_MFI = "SDL_JOYSTICK_MFI";

        public const string SDL_HINT_JOYSTICK_RAWINPUT = "SDL_JOYSTICK_RAWINPUT";

        public const string SDL_HINT_JOYSTICK_RAWINPUT_CORRELATE_XINPUT = "SDL_JOYSTICK_RAWINPUT_CORRELATE_XINPUT";

        public const string SDL_HINT_JOYSTICK_ROG_CHAKRAM = "SDL_JOYSTICK_ROG_CHAKRAM";

        public const string SDL_HINT_JOYSTICK_THREAD = "SDL_JOYSTICK_THREAD";

        public const string SDL_HINT_JOYSTICK_THROTTLE_DEVICES = "SDL_JOYSTICK_THROTTLE_DEVICES";

        public const string SDL_HINT_JOYSTICK_THROTTLE_DEVICES_EXCLUDED = "SDL_JOYSTICK_THROTTLE_DEVICES_EXCLUDED";

        public const string SDL_HINT_JOYSTICK_WGI = "SDL_JOYSTICK_WGI";

        public const string SDL_HINT_JOYSTICK_WHEEL_DEVICES = "SDL_JOYSTICK_WHEEL_DEVICES";

        public const string SDL_HINT_JOYSTICK_WHEEL_DEVICES_EXCLUDED = "SDL_JOYSTICK_WHEEL_DEVICES_EXCLUDED";

        public const string SDL_HINT_JOYSTICK_ZERO_CENTERED_DEVICES = "SDL_JOYSTICK_ZERO_CENTERED_DEVICES";

        public const string SDL_HINT_JOYSTICK_HAPTIC_AXES = "SDL_JOYSTICK_HAPTIC_AXES";

        public const string SDL_HINT_KEYCODE_OPTIONS = "SDL_KEYCODE_OPTIONS";

        public const string SDL_HINT_KMSDRM_DEVICE_INDEX = "SDL_KMSDRM_DEVICE_INDEX";

        public const string SDL_HINT_KMSDRM_REQUIRE_DRM_MASTER = "SDL_KMSDRM_REQUIRE_DRM_MASTER";

        public const string SDL_HINT_KMSDRM_ATOMIC = "SDL_KMSDRM_ATOMIC";

        public const string SDL_HINT_LOGGING = "SDL_LOGGING";

        public const string SDL_HINT_MAC_BACKGROUND_APP = "SDL_MAC_BACKGROUND_APP";

        public const string SDL_HINT_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK = "SDL_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK";

        public const string SDL_HINT_MAC_OPENGL_ASYNC_DISPATCH = "SDL_MAC_OPENGL_ASYNC_DISPATCH";

        public const string SDL_HINT_MAC_OPTION_AS_ALT = "SDL_MAC_OPTION_AS_ALT";

        public const string SDL_HINT_MAC_SCROLL_MOMENTUM = "SDL_MAC_SCROLL_MOMENTUM";

        public const string SDL_HINT_MAC_PRESS_AND_HOLD = "SDL_MAC_PRESS_AND_HOLD";

        public const string SDL_HINT_MAIN_CALLBACK_RATE = "SDL_MAIN_CALLBACK_RATE";

        public const string SDL_HINT_MOUSE_AUTO_CAPTURE = "SDL_MOUSE_AUTO_CAPTURE";

        public const string SDL_HINT_MOUSE_DOUBLE_CLICK_RADIUS = "SDL_MOUSE_DOUBLE_CLICK_RADIUS";

        public const string SDL_HINT_MOUSE_DOUBLE_CLICK_TIME = "SDL_MOUSE_DOUBLE_CLICK_TIME";

        public const string SDL_HINT_MOUSE_DEFAULT_SYSTEM_CURSOR = "SDL_MOUSE_DEFAULT_SYSTEM_CURSOR";

        public const string SDL_HINT_MOUSE_DPI_SCALE_CURSORS = "SDL_MOUSE_DPI_SCALE_CURSORS";

        public const string SDL_HINT_MOUSE_EMULATE_WARP_WITH_RELATIVE = "SDL_MOUSE_EMULATE_WARP_WITH_RELATIVE";

        public const string SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH = "SDL_MOUSE_FOCUS_CLICKTHROUGH";

        public const string SDL_HINT_MOUSE_NORMAL_SPEED_SCALE = "SDL_MOUSE_NORMAL_SPEED_SCALE";

        public const string SDL_HINT_MOUSE_RELATIVE_MODE_CENTER = "SDL_MOUSE_RELATIVE_MODE_CENTER";

        public const string SDL_HINT_MOUSE_RELATIVE_SPEED_SCALE = "SDL_MOUSE_RELATIVE_SPEED_SCALE";

        public const string SDL_HINT_MOUSE_RELATIVE_SYSTEM_SCALE = "SDL_MOUSE_RELATIVE_SYSTEM_SCALE";

        public const string SDL_HINT_MOUSE_RELATIVE_WARP_MOTION = "SDL_MOUSE_RELATIVE_WARP_MOTION";

        public const string SDL_HINT_MOUSE_RELATIVE_CURSOR_VISIBLE = "SDL_MOUSE_RELATIVE_CURSOR_VISIBLE";

        public const string SDL_HINT_MOUSE_TOUCH_EVENTS = "SDL_MOUSE_TOUCH_EVENTS";

        public const string SDL_HINT_MUTE_CONSOLE_KEYBOARD = "SDL_MUTE_CONSOLE_KEYBOARD";

        public const string SDL_HINT_NO_SIGNAL_HANDLERS = "SDL_NO_SIGNAL_HANDLERS";

        public const string SDL_HINT_OPENGL_LIBRARY = "SDL_OPENGL_LIBRARY";

        public const string SDL_HINT_EGL_LIBRARY = "SDL_EGL_LIBRARY";

        public const string SDL_HINT_OPENGL_ES_DRIVER = "SDL_OPENGL_ES_DRIVER";

        public const string SDL_HINT_OPENVR_LIBRARY = "SDL_OPENVR_LIBRARY";

        public const string SDL_HINT_ORIENTATIONS = "SDL_ORIENTATIONS";

        public const string SDL_HINT_POLL_SENTINEL = "SDL_POLL_SENTINEL";

        public const string SDL_HINT_PREFERRED_LOCALES = "SDL_PREFERRED_LOCALES";

        public const string SDL_HINT_QUIT_ON_LAST_WINDOW_CLOSE = "SDL_QUIT_ON_LAST_WINDOW_CLOSE";

        public const string SDL_HINT_RENDER_DIRECT3D_THREADSAFE = "SDL_RENDER_DIRECT3D_THREADSAFE";

        public const string SDL_HINT_RENDER_DIRECT3D11_DEBUG = "SDL_RENDER_DIRECT3D11_DEBUG";

        public const string SDL_HINT_RENDER_DIRECT3D11_WARP = "SDL_RENDER_DIRECT3D11_WARP";

        public const string SDL_HINT_RENDER_VULKAN_DEBUG = "SDL_RENDER_VULKAN_DEBUG";

        public const string SDL_HINT_RENDER_GPU_DEBUG = "SDL_RENDER_GPU_DEBUG";

        public const string SDL_HINT_RENDER_GPU_LOW_POWER = "SDL_RENDER_GPU_LOW_POWER";

        public const string SDL_HINT_RENDER_DRIVER = "SDL_RENDER_DRIVER";

        public const string SDL_HINT_RENDER_LINE_METHOD = "SDL_RENDER_LINE_METHOD";

        public const string SDL_HINT_RENDER_METAL_PREFER_LOW_POWER_DEVICE = "SDL_RENDER_METAL_PREFER_LOW_POWER_DEVICE";

        public const string SDL_HINT_RENDER_VSYNC = "SDL_RENDER_VSYNC";

        public const string SDL_HINT_RETURN_KEY_HIDES_IME = "SDL_RETURN_KEY_HIDES_IME";

        public const string SDL_HINT_ROG_GAMEPAD_MICE = "SDL_ROG_GAMEPAD_MICE";

        public const string SDL_HINT_ROG_GAMEPAD_MICE_EXCLUDED = "SDL_ROG_GAMEPAD_MICE_EXCLUDED";

        public const string SDL_HINT_PS2_GS_WIDTH = "SDL_PS2_GS_WIDTH";

        public const string SDL_HINT_PS2_GS_HEIGHT = "SDL_PS2_GS_HEIGHT";

        public const string SDL_HINT_PS2_GS_PROGRESSIVE = "SDL_PS2_GS_PROGRESSIVE";

        public const string SDL_HINT_PS2_GS_MODE = "SDL_PS2_GS_MODE";

        public const string SDL_HINT_RPI_VIDEO_LAYER = "SDL_RPI_VIDEO_LAYER";

        public const string SDL_HINT_SCREENSAVER_INHIBIT_ACTIVITY_NAME = "SDL_SCREENSAVER_INHIBIT_ACTIVITY_NAME";

        public const string SDL_HINT_SHUTDOWN_DBUS_ON_QUIT = "SDL_SHUTDOWN_DBUS_ON_QUIT";

        public const string SDL_HINT_STORAGE_TITLE_DRIVER = "SDL_STORAGE_TITLE_DRIVER";

        public const string SDL_HINT_STORAGE_USER_DRIVER = "SDL_STORAGE_USER_DRIVER";

        public const string SDL_HINT_THREAD_FORCE_REALTIME_TIME_CRITICAL = "SDL_THREAD_FORCE_REALTIME_TIME_CRITICAL";

        public const string SDL_HINT_THREAD_PRIORITY_POLICY = "SDL_THREAD_PRIORITY_POLICY";

        public const string SDL_HINT_TIMER_RESOLUTION = "SDL_TIMER_RESOLUTION";

        public const string SDL_HINT_TOUCH_MOUSE_EVENTS = "SDL_TOUCH_MOUSE_EVENTS";

        public const string SDL_HINT_TRACKPAD_IS_TOUCH_ONLY = "SDL_TRACKPAD_IS_TOUCH_ONLY";

        public const string SDL_HINT_TV_REMOTE_AS_JOYSTICK = "SDL_TV_REMOTE_AS_JOYSTICK";

        public const string SDL_HINT_VIDEO_ALLOW_SCREENSAVER = "SDL_VIDEO_ALLOW_SCREENSAVER";

        public const string SDL_HINT_VIDEO_DISPLAY_PRIORITY = "SDL_VIDEO_DISPLAY_PRIORITY";

        public const string SDL_HINT_VIDEO_DOUBLE_BUFFER = "SDL_VIDEO_DOUBLE_BUFFER";

        public const string SDL_HINT_VIDEO_DRIVER = "SDL_VIDEO_DRIVER";

        public const string SDL_HINT_VIDEO_DUMMY_SAVE_FRAMES = "SDL_VIDEO_DUMMY_SAVE_FRAMES";

        public const string SDL_HINT_VIDEO_EGL_ALLOW_GETDISPLAY_FALLBACK = "SDL_VIDEO_EGL_ALLOW_GETDISPLAY_FALLBACK";

        public const string SDL_HINT_VIDEO_FORCE_EGL = "SDL_VIDEO_FORCE_EGL";

        public const string SDL_HINT_VIDEO_MAC_FULLSCREEN_SPACES = "SDL_VIDEO_MAC_FULLSCREEN_SPACES";

        public const string SDL_HINT_VIDEO_MAC_FULLSCREEN_MENU_VISIBILITY = "SDL_VIDEO_MAC_FULLSCREEN_MENU_VISIBILITY";

        public const string SDL_HINT_VIDEO_METAL_AUTO_RESIZE_DRAWABLE = "SDL_VIDEO_METAL_AUTO_RESIZE_DRAWABLE";

        public const string SDL_HINT_VIDEO_MATCH_EXCLUSIVE_MODE_ON_MOVE = "SDL_VIDEO_MATCH_EXCLUSIVE_MODE_ON_MOVE";

        public const string SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS = "SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS";

        public const string SDL_HINT_VIDEO_OFFSCREEN_SAVE_FRAMES = "SDL_VIDEO_OFFSCREEN_SAVE_FRAMES";

        public const string SDL_HINT_VIDEO_SYNC_WINDOW_OPERATIONS = "SDL_VIDEO_SYNC_WINDOW_OPERATIONS";

        public const string SDL_HINT_VIDEO_WAYLAND_ALLOW_LIBDECOR = "SDL_VIDEO_WAYLAND_ALLOW_LIBDECOR";

        public const string SDL_HINT_VIDEO_WAYLAND_MODE_EMULATION = "SDL_VIDEO_WAYLAND_MODE_EMULATION";

        public const string SDL_HINT_VIDEO_WAYLAND_MODE_SCALING = "SDL_VIDEO_WAYLAND_MODE_SCALING";

        public const string SDL_HINT_VIDEO_WAYLAND_PREFER_LIBDECOR = "SDL_VIDEO_WAYLAND_PREFER_LIBDECOR";

        public const string SDL_HINT_VIDEO_WAYLAND_SCALE_TO_DISPLAY = "SDL_VIDEO_WAYLAND_SCALE_TO_DISPLAY";

        public const string SDL_HINT_VIDEO_WIN_D3DCOMPILER = "SDL_VIDEO_WIN_D3DCOMPILER";

        public const string SDL_HINT_VIDEO_X11_EXTERNAL_WINDOW_INPUT = "SDL_VIDEO_X11_EXTERNAL_WINDOW_INPUT";

        public const string SDL_HINT_VIDEO_X11_NET_WM_BYPASS_COMPOSITOR = "SDL_VIDEO_X11_NET_WM_BYPASS_COMPOSITOR";

        public const string SDL_HINT_VIDEO_X11_NET_WM_PING = "SDL_VIDEO_X11_NET_WM_PING";

        public const string SDL_HINT_VIDEO_X11_NODIRECTCOLOR = "SDL_VIDEO_X11_NODIRECTCOLOR";

        public const string SDL_HINT_VIDEO_X11_SCALING_FACTOR = "SDL_VIDEO_X11_SCALING_FACTOR";

        public const string SDL_HINT_VIDEO_X11_VISUALID = "SDL_VIDEO_X11_VISUALID";

        public const string SDL_HINT_VIDEO_X11_WINDOW_VISUALID = "SDL_VIDEO_X11_WINDOW_VISUALID";

        public const string SDL_HINT_VIDEO_X11_XRANDR = "SDL_VIDEO_X11_XRANDR";

        public const string SDL_HINT_VITA_ENABLE_BACK_TOUCH = "SDL_VITA_ENABLE_BACK_TOUCH";

        public const string SDL_HINT_VITA_ENABLE_FRONT_TOUCH = "SDL_VITA_ENABLE_FRONT_TOUCH";

        public const string SDL_HINT_VITA_MODULE_PATH = "SDL_VITA_MODULE_PATH";

        public const string SDL_HINT_VITA_PVR_INIT = "SDL_VITA_PVR_INIT";

        public const string SDL_HINT_VITA_RESOLUTION = "SDL_VITA_RESOLUTION";

        public const string SDL_HINT_VITA_PVR_OPENGL = "SDL_VITA_PVR_OPENGL";

        public const string SDL_HINT_VITA_TOUCH_MOUSE_DEVICE = "SDL_VITA_TOUCH_MOUSE_DEVICE";

        public const string SDL_HINT_VULKAN_DISPLAY = "SDL_VULKAN_DISPLAY";

        public const string SDL_HINT_VULKAN_LIBRARY = "SDL_VULKAN_LIBRARY";

        public const string SDL_HINT_WAVE_FACT_CHUNK = "SDL_WAVE_FACT_CHUNK";

        public const string SDL_HINT_WAVE_CHUNK_LIMIT = "SDL_WAVE_CHUNK_LIMIT";

        public const string SDL_HINT_WAVE_RIFF_CHUNK_SIZE = "SDL_WAVE_RIFF_CHUNK_SIZE";

        public const string SDL_HINT_WAVE_TRUNCATION = "SDL_WAVE_TRUNCATION";

        public const string SDL_HINT_WINDOW_ACTIVATE_WHEN_RAISED = "SDL_WINDOW_ACTIVATE_WHEN_RAISED";

        public const string SDL_HINT_WINDOW_ACTIVATE_WHEN_SHOWN = "SDL_WINDOW_ACTIVATE_WHEN_SHOWN";

        public const string SDL_HINT_WINDOW_ALLOW_TOPMOST = "SDL_WINDOW_ALLOW_TOPMOST";

        public const string SDL_HINT_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN = "SDL_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN";

        public const string SDL_HINT_WINDOWS_CLOSE_ON_ALT_F4 = "SDL_WINDOWS_CLOSE_ON_ALT_F4";

        public const string SDL_HINT_WINDOWS_ENABLE_MENU_MNEMONICS = "SDL_WINDOWS_ENABLE_MENU_MNEMONICS";

        public const string SDL_HINT_WINDOWS_ENABLE_MESSAGELOOP = "SDL_WINDOWS_ENABLE_MESSAGELOOP";

        public const string SDL_HINT_WINDOWS_GAMEINPUT = "SDL_WINDOWS_GAMEINPUT";

        public const string SDL_HINT_WINDOWS_RAW_KEYBOARD = "SDL_WINDOWS_RAW_KEYBOARD";

        public const string SDL_HINT_WINDOWS_RAW_KEYBOARD_EXCLUDE_HOTKEYS = "SDL_WINDOWS_RAW_KEYBOARD_EXCLUDE_HOTKEYS";

        public const string SDL_HINT_WINDOWS_FORCE_SEMAPHORE_KERNEL = "SDL_WINDOWS_FORCE_SEMAPHORE_KERNEL";

        public const string SDL_HINT_WINDOWS_INTRESOURCE_ICON = "SDL_WINDOWS_INTRESOURCE_ICON";

        public const string SDL_HINT_WINDOWS_INTRESOURCE_ICON_SMALL = "SDL_WINDOWS_INTRESOURCE_ICON_SMALL";

        public const string SDL_HINT_WINDOWS_USE_D3D9EX = "SDL_WINDOWS_USE_D3D9EX";

        public const string SDL_HINT_WINDOWS_ERASE_BACKGROUND_MODE = "SDL_WINDOWS_ERASE_BACKGROUND_MODE";

        public const string SDL_HINT_X11_FORCE_OVERRIDE_REDIRECT = "SDL_X11_FORCE_OVERRIDE_REDIRECT";

        public const string SDL_HINT_X11_WINDOW_TYPE = "SDL_X11_WINDOW_TYPE";

        public const string SDL_HINT_X11_XCB_LIBRARY = "SDL_X11_XCB_LIBRARY";

        public const string SDL_HINT_XINPUT_ENABLED = "SDL_XINPUT_ENABLED";

        public const string SDL_HINT_ASSERT = "SDL_ASSERT";

        public const string SDL_HINT_PEN_MOUSE_EVENTS = "SDL_PEN_MOUSE_EVENTS";

        public const string SDL_HINT_PEN_TOUCH_EVENTS = "SDL_PEN_TOUCH_EVENTS";

        public const string SDL_PROP_APP_METADATA_NAME_STRING = "SDL.app.metadata.name";

        public const string SDL_PROP_APP_METADATA_VERSION_STRING = "SDL.app.metadata.version";

        public const string SDL_PROP_APP_METADATA_IDENTIFIER_STRING = "SDL.app.metadata.identifier";

        public const string SDL_PROP_APP_METADATA_CREATOR_STRING = "SDL.app.metadata.creator";

        public const string SDL_PROP_APP_METADATA_COPYRIGHT_STRING = "SDL.app.metadata.copyright";

        public const string SDL_PROP_APP_METADATA_URL_STRING = "SDL.app.metadata.url";

        public const string SDL_PROP_APP_METADATA_TYPE_STRING = "SDL.app.metadata.type";

        public const string SDL_PROP_PROCESS_CREATE_ARGS_POINTER = "SDL.process.create.args";

        public const string SDL_PROP_PROCESS_CREATE_ENVIRONMENT_POINTER = "SDL.process.create.environment";

        public const string SDL_PROP_PROCESS_CREATE_WORKING_DIRECTORY_STRING = "SDL.process.create.working_directory";

        public const string SDL_PROP_PROCESS_CREATE_STDIN_NUMBER = "SDL.process.create.stdin_option";

        public const string SDL_PROP_PROCESS_CREATE_STDIN_POINTER = "SDL.process.create.stdin_source";

        public const string SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER = "SDL.process.create.stdout_option";

        public const string SDL_PROP_PROCESS_CREATE_STDOUT_POINTER = "SDL.process.create.stdout_source";

        public const string SDL_PROP_PROCESS_CREATE_STDERR_NUMBER = "SDL.process.create.stderr_option";

        public const string SDL_PROP_PROCESS_CREATE_STDERR_POINTER = "SDL.process.create.stderr_source";

        public const string SDL_PROP_PROCESS_CREATE_STDERR_TO_STDOUT_BOOLEAN = "SDL.process.create.stderr_to_stdout";

        public const string SDL_PROP_PROCESS_CREATE_BACKGROUND_BOOLEAN = "SDL.process.create.background";

        public const string SDL_PROP_PROCESS_CREATE_CMDLINE_STRING = "SDL.process.create.cmdline";

        public const string SDL_PROP_PROCESS_PID_NUMBER = "SDL.process.pid";

        public const string SDL_PROP_PROCESS_STDIN_POINTER = "SDL.process.stdin";

        public const string SDL_PROP_PROCESS_STDOUT_POINTER = "SDL.process.stdout";

        public const string SDL_PROP_PROCESS_STDERR_POINTER = "SDL.process.stderr";

        public const string SDL_PROP_PROCESS_BACKGROUND_BOOLEAN = "SDL.process.background";

        public const string SDL_PROP_RENDERER_CREATE_NAME_STRING = "SDL.renderer.create.name";

        public const string SDL_PROP_RENDERER_CREATE_WINDOW_POINTER = "SDL.renderer.create.window";

        public const string SDL_PROP_RENDERER_CREATE_SURFACE_POINTER = "SDL.renderer.create.surface";

        public const string SDL_PROP_RENDERER_CREATE_OUTPUT_COLORSPACE_NUMBER = "SDL.renderer.create.output_colorspace";

        public const string SDL_PROP_RENDERER_CREATE_PRESENT_VSYNC_NUMBER = "SDL.renderer.create.present_vsync";

        public const string SDL_PROP_RENDERER_CREATE_GPU_DEVICE_POINTER = "SDL.renderer.create.gpu.device";

        public const string SDL_PROP_RENDERER_CREATE_GPU_SHADERS_SPIRV_BOOLEAN = "SDL.renderer.create.gpu.shaders_spirv";

        public const string SDL_PROP_RENDERER_CREATE_GPU_SHADERS_DXIL_BOOLEAN = "SDL.renderer.create.gpu.shaders_dxil";

        public const string SDL_PROP_RENDERER_CREATE_GPU_SHADERS_MSL_BOOLEAN = "SDL.renderer.create.gpu.shaders_msl";

        public const string SDL_PROP_RENDERER_CREATE_VULKAN_INSTANCE_POINTER = "SDL.renderer.create.vulkan.instance";

        public const string SDL_PROP_RENDERER_CREATE_VULKAN_SURFACE_NUMBER = "SDL.renderer.create.vulkan.surface";

        public const string SDL_PROP_RENDERER_CREATE_VULKAN_PHYSICAL_DEVICE_POINTER = "SDL.renderer.create.vulkan.physical_device";

        public const string SDL_PROP_RENDERER_CREATE_VULKAN_DEVICE_POINTER = "SDL.renderer.create.vulkan.device";

        public const string SDL_PROP_RENDERER_CREATE_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.create.vulkan.graphics_queue_family_index";

        public const string SDL_PROP_RENDERER_CREATE_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.create.vulkan.present_queue_family_index";

        public const string SDL_PROP_RENDERER_NAME_STRING = "SDL.renderer.name";

        public const string SDL_PROP_RENDERER_WINDOW_POINTER = "SDL.renderer.window";

        public const string SDL_PROP_RENDERER_SURFACE_POINTER = "SDL.renderer.surface";

        public const string SDL_PROP_RENDERER_VSYNC_NUMBER = "SDL.renderer.vsync";

        public const string SDL_PROP_RENDERER_MAX_TEXTURE_SIZE_NUMBER = "SDL.renderer.max_texture_size";

        public const string SDL_PROP_RENDERER_TEXTURE_FORMATS_POINTER = "SDL.renderer.texture_formats";

        public const string SDL_PROP_RENDERER_TEXTURE_WRAPPING_BOOLEAN = "SDL.renderer.texture_wrapping";

        public const string SDL_PROP_RENDERER_OUTPUT_COLORSPACE_NUMBER = "SDL.renderer.output_colorspace";

        public const string SDL_PROP_RENDERER_HDR_ENABLED_BOOLEAN = "SDL.renderer.HDR_enabled";

        public const string SDL_PROP_RENDERER_SDR_WHITE_POINT_FLOAT = "SDL.renderer.SDR_white_point";

        public const string SDL_PROP_RENDERER_HDR_HEADROOM_FLOAT = "SDL.renderer.HDR_headroom";

        public const string SDL_PROP_RENDERER_D3D9_DEVICE_POINTER = "SDL.renderer.d3d9.device";

        public const string SDL_PROP_RENDERER_D3D11_DEVICE_POINTER = "SDL.renderer.d3d11.device";

        public const string SDL_PROP_RENDERER_D3D11_SWAPCHAIN_POINTER = "SDL.renderer.d3d11.swap_chain";

        public const string SDL_PROP_RENDERER_D3D12_DEVICE_POINTER = "SDL.renderer.d3d12.device";

        public const string SDL_PROP_RENDERER_D3D12_SWAPCHAIN_POINTER = "SDL.renderer.d3d12.swap_chain";

        public const string SDL_PROP_RENDERER_D3D12_COMMAND_QUEUE_POINTER = "SDL.renderer.d3d12.command_queue";

        public const string SDL_PROP_RENDERER_VULKAN_INSTANCE_POINTER = "SDL.renderer.vulkan.instance";

        public const string SDL_PROP_RENDERER_VULKAN_SURFACE_NUMBER = "SDL.renderer.vulkan.surface";

        public const string SDL_PROP_RENDERER_VULKAN_PHYSICAL_DEVICE_POINTER = "SDL.renderer.vulkan.physical_device";

        public const string SDL_PROP_RENDERER_VULKAN_DEVICE_POINTER = "SDL.renderer.vulkan.device";

        public const string SDL_PROP_RENDERER_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.vulkan.graphics_queue_family_index";

        public const string SDL_PROP_RENDERER_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.vulkan.present_queue_family_index";

        public const string SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER = "SDL.renderer.vulkan.swapchain_image_count";

        public const string SDL_PROP_RENDERER_GPU_DEVICE_POINTER = "SDL.renderer.gpu.device";

        public const string SDL_PROP_TEXTURE_CREATE_COLORSPACE_NUMBER = "SDL.texture.create.colorspace";

        public const string SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER = "SDL.texture.create.format";

        public const string SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER = "SDL.texture.create.access";

        public const string SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER = "SDL.texture.create.width";

        public const string SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER = "SDL.texture.create.height";

        public const string SDL_PROP_TEXTURE_CREATE_PALETTE_POINTER = "SDL.texture.create.palette";

        public const string SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT = "SDL.texture.create.SDR_white_point";

        public const string SDL_PROP_TEXTURE_CREATE_HDR_HEADROOM_FLOAT = "SDL.texture.create.HDR_headroom";

        public const string SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_POINTER = "SDL.texture.create.d3d11.texture";

        public const string SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_U_POINTER = "SDL.texture.create.d3d11.texture_u";

        public const string SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_V_POINTER = "SDL.texture.create.d3d11.texture_v";

        public const string SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_POINTER = "SDL.texture.create.d3d12.texture";

        public const string SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_U_POINTER = "SDL.texture.create.d3d12.texture_u";

        public const string SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_V_POINTER = "SDL.texture.create.d3d12.texture_v";

        public const string SDL_PROP_TEXTURE_CREATE_METAL_PIXELBUFFER_POINTER = "SDL.texture.create.metal.pixelbuffer";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_NUMBER = "SDL.texture.create.opengl.texture";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_UV_NUMBER = "SDL.texture.create.opengl.texture_uv";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_U_NUMBER = "SDL.texture.create.opengl.texture_u";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_V_NUMBER = "SDL.texture.create.opengl.texture_v";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_NUMBER = "SDL.texture.create.opengles2.texture";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_UV_NUMBER = "SDL.texture.create.opengles2.texture_uv";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_U_NUMBER = "SDL.texture.create.opengles2.texture_u";

        public const string SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_V_NUMBER = "SDL.texture.create.opengles2.texture_v";

        public const string SDL_PROP_TEXTURE_CREATE_VULKAN_TEXTURE_NUMBER = "SDL.texture.create.vulkan.texture";

        public const string SDL_PROP_TEXTURE_CREATE_VULKAN_LAYOUT_NUMBER = "SDL.texture.create.vulkan.layout";

        public const string SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_POINTER = "SDL.texture.create.gpu.texture";

        public const string SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_UV_POINTER = "SDL.texture.create.gpu.texture_uv";

        public const string SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_U_POINTER = "SDL.texture.create.gpu.texture_u";

        public const string SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_V_POINTER = "SDL.texture.create.gpu.texture_v";

        public const string SDL_PROP_TEXTURE_COLORSPACE_NUMBER = "SDL.texture.colorspace";

        public const string SDL_PROP_TEXTURE_FORMAT_NUMBER = "SDL.texture.format";

        public const string SDL_PROP_TEXTURE_ACCESS_NUMBER = "SDL.texture.access";

        public const string SDL_PROP_TEXTURE_WIDTH_NUMBER = "SDL.texture.width";

        public const string SDL_PROP_TEXTURE_HEIGHT_NUMBER = "SDL.texture.height";

        public const string SDL_PROP_TEXTURE_SDR_WHITE_POINT_FLOAT = "SDL.texture.SDR_white_point";

        public const string SDL_PROP_TEXTURE_HDR_HEADROOM_FLOAT = "SDL.texture.HDR_headroom";

        public const string SDL_PROP_TEXTURE_D3D11_TEXTURE_POINTER = "SDL.texture.d3d11.texture";

        public const string SDL_PROP_TEXTURE_D3D11_TEXTURE_U_POINTER = "SDL.texture.d3d11.texture_u";

        public const string SDL_PROP_TEXTURE_D3D11_TEXTURE_V_POINTER = "SDL.texture.d3d11.texture_v";

        public const string SDL_PROP_TEXTURE_D3D12_TEXTURE_POINTER = "SDL.texture.d3d12.texture";

        public const string SDL_PROP_TEXTURE_D3D12_TEXTURE_U_POINTER = "SDL.texture.d3d12.texture_u";

        public const string SDL_PROP_TEXTURE_D3D12_TEXTURE_V_POINTER = "SDL.texture.d3d12.texture_v";

        public const string SDL_PROP_TEXTURE_OPENGL_TEXTURE_NUMBER = "SDL.texture.opengl.texture";

        public const string SDL_PROP_TEXTURE_OPENGL_TEXTURE_UV_NUMBER = "SDL.texture.opengl.texture_uv";

        public const string SDL_PROP_TEXTURE_OPENGL_TEXTURE_U_NUMBER = "SDL.texture.opengl.texture_u";

        public const string SDL_PROP_TEXTURE_OPENGL_TEXTURE_V_NUMBER = "SDL.texture.opengl.texture_v";

        public const string SDL_PROP_TEXTURE_OPENGL_TEXTURE_TARGET_NUMBER = "SDL.texture.opengl.target";

        public const string SDL_PROP_TEXTURE_OPENGL_TEX_W_FLOAT = "SDL.texture.opengl.tex_w";

        public const string SDL_PROP_TEXTURE_OPENGL_TEX_H_FLOAT = "SDL.texture.opengl.tex_h";

        public const string SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_NUMBER = "SDL.texture.opengles2.texture";

        public const string SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_UV_NUMBER = "SDL.texture.opengles2.texture_uv";

        public const string SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_U_NUMBER = "SDL.texture.opengles2.texture_u";

        public const string SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_V_NUMBER = "SDL.texture.opengles2.texture_v";

        public const string SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_TARGET_NUMBER = "SDL.texture.opengles2.target";

        public const string SDL_PROP_TEXTURE_VULKAN_TEXTURE_NUMBER = "SDL.texture.vulkan.texture";

        public const string SDL_PROP_TEXTURE_GPU_TEXTURE_POINTER = "SDL.texture.gpu.texture";

        public const string SDL_PROP_TEXTURE_GPU_TEXTURE_UV_POINTER = "SDL.texture.gpu.texture_uv";

        public const string SDL_PROP_TEXTURE_GPU_TEXTURE_U_POINTER = "SDL.texture.gpu.texture_u";

        public const string SDL_PROP_TEXTURE_GPU_TEXTURE_V_POINTER = "SDL.texture.gpu.texture_v";

        private unsafe static byte* EncodeAsUTF8(string str)
        {
            if (str == null)
            {
                return null;
            }
            int num = str.Length * 4 + 1;
            byte* ptr = (byte*)(void*)SDL_malloc((UIntPtr)(ulong)num);
            fixed (char* chars = str)
            {
                Encoding.UTF8.GetBytes(chars, str.Length + 1, ptr, num);
            }
            return ptr;
        }

        private unsafe static string DecodeFromUTF8(IntPtr ptr, bool shouldFree = false)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            byte* ptr2;
            for (ptr2 = (byte*)(void*)ptr; *ptr2 != 0; ptr2++)
            {
            }
            string result = new string((sbyte*)(void*)ptr, 0, (int)(ptr2 - (byte*)(void*)ptr), Encoding.UTF8);
            if (shouldFree)
            {
                SDL_free(ptr);
            }
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr SDL_malloc(UIntPtr size);


        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_free(IntPtr mem);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ReportAssertion")]
        private unsafe static extern SDL_AssertState INTERNAL_SDL_ReportAssertion(ref SDL_AssertData data, byte* func, byte* file, int line);

        public unsafe static SDL_AssertState SDL_ReportAssertion(ref SDL_AssertData data, string func, string file, int line)
        {
            byte* ptr = EncodeAsUTF8(func);
            byte* ptr2 = EncodeAsUTF8(file);
            SDL_AssertState result = INTERNAL_SDL_ReportAssertion(ref data, ptr, ptr2, line);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetAssertionHandler(SDL_AssertionHandler handler, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetDefaultAssertionHandler();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAssertionHandler(out IntPtr puserdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAssertionReport();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ResetAssertionReport();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AsyncIOFromFile")]
        private unsafe static extern IntPtr INTERNAL_SDL_AsyncIOFromFile(byte* file, byte* mode);

        public unsafe static IntPtr SDL_AsyncIOFromFile(string file, string mode)
        {
            byte* intPtr = EncodeAsUTF8(file);
            byte* ptr = EncodeAsUTF8(mode);
            IntPtr result = INTERNAL_SDL_AsyncIOFromFile(intPtr, ptr);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_GetAsyncIOSize(IntPtr asyncio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadAsyncIO(IntPtr asyncio, IntPtr ptr, ulong offset, ulong size, IntPtr queue, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteAsyncIO(IntPtr asyncio, IntPtr ptr, ulong offset, ulong size, IntPtr queue, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CloseAsyncIO(IntPtr asyncio, SDLBool flush, IntPtr queue, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateAsyncIOQueue();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyAsyncIOQueue(IntPtr queue);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetAsyncIOResult(IntPtr queue, out SDL_AsyncIOOutcome outcome);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitAsyncIOResult(IntPtr queue, out SDL_AsyncIOOutcome outcome, int timeoutMS);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SignalAsyncIOQueue(IntPtr queue);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadFileAsync")]
        private unsafe static extern SDLBool INTERNAL_SDL_LoadFileAsync(byte* file, IntPtr queue, IntPtr userdata);

        public unsafe static SDLBool SDL_LoadFileAsync(string file, IntPtr queue, IntPtr userdata)
        {
            byte* intPtr = EncodeAsUTF8(file);
            SDLBool result = INTERNAL_SDL_LoadFileAsync(intPtr, queue, userdata);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TryLockSpinlock(IntPtr @lock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockSpinlock(IntPtr @lock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSpinlock(IntPtr @lock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MemoryBarrierReleaseFunction();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MemoryBarrierAcquireFunction();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CompareAndSwapAtomicInt(ref SDL_AtomicInt a, int oldval, int newval);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetAtomicInt(ref SDL_AtomicInt a, int v);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAtomicInt(ref SDL_AtomicInt a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AddAtomicInt(ref SDL_AtomicInt a, int v);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CompareAndSwapAtomicU32(ref SDL_AtomicU32 a, uint oldval, uint newval);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_SetAtomicU32(ref SDL_AtomicU32 a, uint v);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetAtomicU32(ref SDL_AtomicU32 a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_AddAtomicU32(ref SDL_AtomicU32 a, int v);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CompareAndSwapAtomicPointer(ref IntPtr a, IntPtr oldval, IntPtr newval);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_SetAtomicPointer(ref IntPtr a, IntPtr v);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAtomicPointer(ref IntPtr a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetError")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetError(byte* fmt);

        public unsafe static SDLBool SDL_SetError(string fmt)
        {
            byte* intPtr = EncodeAsUTF8(fmt);
            SDLBool result = INTERNAL_SDL_SetError(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_OutOfMemory();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetError")]
        private static extern IntPtr INTERNAL_SDL_GetError();

        public static string SDL_GetError()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetError());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ClearError();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGlobalProperties();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_CreateProperties();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CopyProperties(uint src, uint dst);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_LockProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetPointerPropertyWithCleanup")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetPointerPropertyWithCleanup(uint props, byte* name, IntPtr value, SDL_CleanupPropertyCallback cleanup, IntPtr userdata);

        public unsafe static SDLBool SDL_SetPointerPropertyWithCleanup(uint props, string name, IntPtr value, SDL_CleanupPropertyCallback cleanup, IntPtr userdata)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_SetPointerPropertyWithCleanup(props, ptr, value, cleanup, userdata);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetPointerProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetPointerProperty(uint props, byte* name, IntPtr value);

        public unsafe static SDLBool SDL_SetPointerProperty(uint props, string name, IntPtr value)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_SetPointerProperty(props, ptr, value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetStringProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetStringProperty(uint props, byte* name, byte* value);

        public unsafe static SDLBool SDL_SetStringProperty(uint props, string name, string value)
        {
            byte* ptr = EncodeAsUTF8(name);
            byte* ptr2 = EncodeAsUTF8(value);
            SDLBool result = INTERNAL_SDL_SetStringProperty(props, ptr, ptr2);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetNumberProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetNumberProperty(uint props, byte* name, long value);

        public unsafe static SDLBool SDL_SetNumberProperty(uint props, string name, long value)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_SetNumberProperty(props, ptr, value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetFloatProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetFloatProperty(uint props, byte* name, float value);

        public unsafe static SDLBool SDL_SetFloatProperty(uint props, string name, float value)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_SetFloatProperty(props, ptr, value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetBooleanProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetBooleanProperty(uint props, byte* name, SDLBool value);

        public unsafe static SDLBool SDL_SetBooleanProperty(uint props, string name, SDLBool value)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_SetBooleanProperty(props, ptr, value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_HasProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_HasProperty(uint props, byte* name);

        public unsafe static SDLBool SDL_HasProperty(uint props, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_HasProperty(props, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPropertyType")]
        private unsafe static extern SDL_PropertyType INTERNAL_SDL_GetPropertyType(uint props, byte* name);

        public unsafe static SDL_PropertyType SDL_GetPropertyType(uint props, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDL_PropertyType result = INTERNAL_SDL_GetPropertyType(props, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPointerProperty")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetPointerProperty(uint props, byte* name, IntPtr default_value);

        public unsafe static IntPtr SDL_GetPointerProperty(uint props, string name, IntPtr default_value)
        {
            byte* ptr = EncodeAsUTF8(name);
            IntPtr result = INTERNAL_SDL_GetPointerProperty(props, ptr, default_value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetStringProperty")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetStringProperty(uint props, byte* name, byte* default_value);

        public unsafe static string SDL_GetStringProperty(uint props, string name, string default_value)
        {
            byte* ptr = EncodeAsUTF8(name);
            byte* ptr2 = EncodeAsUTF8(default_value);
            string result = DecodeFromUTF8(INTERNAL_SDL_GetStringProperty(props, ptr, ptr2));
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetNumberProperty")]
        private unsafe static extern long INTERNAL_SDL_GetNumberProperty(uint props, byte* name, long default_value);

        public unsafe static long SDL_GetNumberProperty(uint props, string name, long default_value)
        {
            byte* ptr = EncodeAsUTF8(name);
            long result = INTERNAL_SDL_GetNumberProperty(props, ptr, default_value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetFloatProperty")]
        private unsafe static extern float INTERNAL_SDL_GetFloatProperty(uint props, byte* name, float default_value);

        public unsafe static float SDL_GetFloatProperty(uint props, string name, float default_value)
        {
            byte* ptr = EncodeAsUTF8(name);
            float result = INTERNAL_SDL_GetFloatProperty(props, ptr, default_value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetBooleanProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_GetBooleanProperty(uint props, byte* name, SDLBool default_value);

        public unsafe static SDLBool SDL_GetBooleanProperty(uint props, string name, SDLBool default_value)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_GetBooleanProperty(props, ptr, default_value);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ClearProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_ClearProperty(uint props, byte* name);

        public unsafe static SDLBool SDL_ClearProperty(uint props, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_ClearProperty(props, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_EnumerateProperties(uint props, SDL_EnumeratePropertiesCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateThreadRuntime")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateThreadRuntime(SDL_ThreadFunction fn, byte* name, IntPtr data, IntPtr pfnBeginThread, IntPtr pfnEndThread);

        public unsafe static IntPtr SDL_CreateThreadRuntime(SDL_ThreadFunction fn, string name, IntPtr data, IntPtr pfnBeginThread, IntPtr pfnEndThread)
        {
            byte* ptr = EncodeAsUTF8(name);
            IntPtr result = INTERNAL_SDL_CreateThreadRuntime(fn, ptr, data, pfnBeginThread, pfnEndThread);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateThreadWithPropertiesRuntime(uint props, IntPtr pfnBeginThread, IntPtr pfnEndThread);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetThreadName")]
        private static extern IntPtr INTERNAL_SDL_GetThreadName(IntPtr thread);

        public static string SDL_GetThreadName(IntPtr thread)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetThreadName(thread));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetCurrentThreadID();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetThreadID(IntPtr thread);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetCurrentThreadPriority(SDL_ThreadPriority priority);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_WaitThread(IntPtr thread, IntPtr status);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_ThreadState SDL_GetThreadState(IntPtr thread);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DetachThread(IntPtr thread);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTLS(IntPtr id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTLS(IntPtr id, IntPtr value, SDL_TLSDestructorCallback destructor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CleanupTLS();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateMutex();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockMutex(IntPtr mutex);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TryLockMutex(IntPtr mutex);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockMutex(IntPtr mutex);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyMutex(IntPtr mutex);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRWLock();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockRWLockForReading(IntPtr rwlock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockRWLockForWriting(IntPtr rwlock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TryLockRWLockForReading(IntPtr rwlock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TryLockRWLockForWriting(IntPtr rwlock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockRWLock(IntPtr rwlock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyRWLock(IntPtr rwlock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSemaphore(uint initial_value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroySemaphore(IntPtr sem);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_WaitSemaphore(IntPtr sem);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TryWaitSemaphore(IntPtr sem);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitSemaphoreTimeout(IntPtr sem, int timeoutMS);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SignalSemaphore(IntPtr sem);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetSemaphoreValue(IntPtr sem);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateCondition();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyCondition(IntPtr cond);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SignalCondition(IntPtr cond);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BroadcastCondition(IntPtr cond);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_WaitCondition(IntPtr cond, IntPtr mutex);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitConditionTimeout(IntPtr cond, IntPtr mutex, int timeoutMS);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ShouldInit(ref SDL_InitState state);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ShouldQuit(ref SDL_InitState state);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetInitialized(ref SDL_InitState state, SDLBool initialized);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_IOFromFile")]
        private unsafe static extern IntPtr INTERNAL_SDL_IOFromFile(byte* file, byte* mode);

        public unsafe static IntPtr SDL_IOFromFile(string file, string mode)
        {
            byte* intPtr = EncodeAsUTF8(file);
            byte* ptr = EncodeAsUTF8(mode);
            IntPtr result = INTERNAL_SDL_IOFromFile(intPtr, ptr);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_IOFromMem(IntPtr mem, UIntPtr size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_IOFromConstMem(IntPtr mem, UIntPtr size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_IOFromDynamicMem();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenIO(ref SDL_IOStreamInterface iface, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CloseIO(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetIOProperties(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_IOStatus SDL_GetIOStatus(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_GetIOSize(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_SeekIO(IntPtr context, long offset, SDL_IOWhence whence);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_TellIO(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr SDL_ReadIO(IntPtr context, IntPtr ptr, UIntPtr size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr SDL_WriteIO(IntPtr context, IntPtr ptr, UIntPtr size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_IOprintf")]
        private unsafe static extern UIntPtr INTERNAL_SDL_IOprintf(IntPtr context, byte* fmt);

        public unsafe static UIntPtr SDL_IOprintf(IntPtr context, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            UIntPtr result = INTERNAL_SDL_IOprintf(context, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FlushIO(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_LoadFile_IO(IntPtr src, out UIntPtr datasize, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadFile")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadFile(byte* file, out UIntPtr datasize);

        public unsafe static IntPtr SDL_LoadFile(string file, out UIntPtr datasize)
        {
            byte* intPtr = EncodeAsUTF8(file);
            IntPtr result = INTERNAL_SDL_LoadFile(intPtr, out datasize);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SaveFile_IO(IntPtr src, IntPtr data, UIntPtr datasize, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SaveFile")]
        private unsafe static extern SDLBool INTERNAL_SDL_SaveFile(byte* file, IntPtr data, UIntPtr datasize);

        public unsafe static SDLBool SDL_SaveFile(string file, IntPtr data, UIntPtr datasize)
        {
            byte* intPtr = EncodeAsUTF8(file);
            SDLBool result = INTERNAL_SDL_SaveFile(intPtr, data, datasize);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU8(IntPtr src, out byte value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS8(IntPtr src, out sbyte value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU16LE(IntPtr src, out ushort value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS16LE(IntPtr src, out short value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU16BE(IntPtr src, out ushort value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS16BE(IntPtr src, out short value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU32LE(IntPtr src, out uint value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS32LE(IntPtr src, out int value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU32BE(IntPtr src, out uint value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS32BE(IntPtr src, out int value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU64LE(IntPtr src, out ulong value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS64LE(IntPtr src, out long value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadU64BE(IntPtr src, out ulong value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadS64BE(IntPtr src, out long value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU8(IntPtr dst, byte value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS8(IntPtr dst, sbyte value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU16LE(IntPtr dst, ushort value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS16LE(IntPtr dst, short value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU16BE(IntPtr dst, ushort value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS16BE(IntPtr dst, short value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU32LE(IntPtr dst, uint value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS32LE(IntPtr dst, int value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU32BE(IntPtr dst, uint value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS32BE(IntPtr dst, int value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU64LE(IntPtr dst, ulong value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS64LE(IntPtr dst, long value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteU64BE(IntPtr dst, ulong value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteS64BE(IntPtr dst, long value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumAudioDrivers();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetAudioDriver")]
        private static extern IntPtr INTERNAL_SDL_GetAudioDriver(int index);

        public static string SDL_GetAudioDriver(int index)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetAudioDriver(index));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentAudioDriver")]
        private static extern IntPtr INTERNAL_SDL_GetCurrentAudioDriver();

        public static string SDL_GetCurrentAudioDriver()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetCurrentAudioDriver());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAudioPlaybackDevices(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAudioRecordingDevices(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetAudioDeviceName")]
        private static extern IntPtr INTERNAL_SDL_GetAudioDeviceName(uint devid);

        public static string SDL_GetAudioDeviceName(uint devid)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetAudioDeviceName(devid));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetAudioDeviceFormat(uint devid, out SDL_AudioSpec spec, out int sample_frames);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAudioDeviceChannelMap(uint devid, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_OpenAudioDevice(uint devid, ref SDL_AudioSpec spec);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsAudioDevicePhysical(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsAudioDevicePlayback(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PauseAudioDevice(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ResumeAudioDevice(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_AudioDevicePaused(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetAudioDeviceGain(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioDeviceGain(uint devid, float gain);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseAudioDevice(uint devid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BindAudioStreams(uint devid, IntPtr[] streams, int num_streams);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BindAudioStream(uint devid, IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnbindAudioStreams(IntPtr[] streams, int num_streams);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnbindAudioStream(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetAudioStreamDevice(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateAudioStream(ref SDL_AudioSpec src_spec, ref SDL_AudioSpec dst_spec);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetAudioStreamProperties(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetAudioStreamFormat(IntPtr stream, out SDL_AudioSpec src_spec, out SDL_AudioSpec dst_spec);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamFormat(IntPtr stream, ref SDL_AudioSpec src_spec, ref SDL_AudioSpec dst_spec);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetAudioStreamFrequencyRatio(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamFrequencyRatio(IntPtr stream, float ratio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetAudioStreamGain(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamGain(IntPtr stream, float gain);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAudioStreamInputChannelMap(IntPtr stream, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetAudioStreamOutputChannelMap(IntPtr stream, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamInputChannelMap(IntPtr stream, int[] chmap, int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamOutputChannelMap(IntPtr stream, int[] chmap, int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PutAudioStreamData(IntPtr stream, IntPtr buf, int len);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PutAudioStreamDataNoCopy(IntPtr stream, IntPtr buf, int len, SDL_AudioStreamDataCompleteCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PutAudioStreamPlanarData(IntPtr stream, IntPtr[] channel_buffers, int num_channels, int num_samples);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAudioStreamData(IntPtr stream, IntPtr buf, int len);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAudioStreamAvailable(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAudioStreamQueued(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FlushAudioStream(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ClearAudioStream(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PauseAudioStreamDevice(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ResumeAudioStreamDevice(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_AudioStreamDevicePaused(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_LockAudioStream(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UnlockAudioStream(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamGetCallback(IntPtr stream, SDL_AudioStreamCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioStreamPutCallback(IntPtr stream, SDL_AudioStreamCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyAudioStream(IntPtr stream);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenAudioDeviceStream(uint devid, ref SDL_AudioSpec spec, SDL_AudioStreamCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetAudioPostmixCallback(uint devid, SDL_AudioPostmixCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_LoadWAV_IO(IntPtr src, SDLBool closeio, out SDL_AudioSpec spec, out IntPtr audio_buf, out uint audio_len);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadWAV")]
        private unsafe static extern SDLBool INTERNAL_SDL_LoadWAV(byte* path, out SDL_AudioSpec spec, out IntPtr audio_buf, out uint audio_len);

        public unsafe static SDLBool SDL_LoadWAV(string path, out SDL_AudioSpec spec, out IntPtr audio_buf, out uint audio_len)
        {
            byte* intPtr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_LoadWAV(intPtr, out spec, out audio_buf, out audio_len);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_MixAudio(IntPtr dst, IntPtr src, SDL_AudioFormat format, uint len, float volume);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ConvertAudioSamples(ref SDL_AudioSpec src_spec, IntPtr src_data, int src_len, ref SDL_AudioSpec dst_spec, IntPtr dst_data, out int dst_len);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetAudioFormatName")]
        private static extern IntPtr INTERNAL_SDL_GetAudioFormatName(SDL_AudioFormat format);

        public static string SDL_GetAudioFormatName(SDL_AudioFormat format)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetAudioFormatName(format));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSilenceValueForFormat(SDL_AudioFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_ComposeCustomBlendMode(SDL_BlendFactor srcColorFactor, SDL_BlendFactor dstColorFactor, SDL_BlendOperation colorOperation, SDL_BlendFactor srcAlphaFactor, SDL_BlendFactor dstAlphaFactor, SDL_BlendOperation alphaOperation);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPixelFormatName")]
        private static extern IntPtr INTERNAL_SDL_GetPixelFormatName(SDL_PixelFormat format);

        public static string SDL_GetPixelFormatName(SDL_PixelFormat format)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetPixelFormatName(format));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetMasksForPixelFormat(SDL_PixelFormat format, out int bpp, out uint Rmask, out uint Gmask, out uint Bmask, out uint Amask);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PixelFormat SDL_GetPixelFormatForMasks(int bpp, uint Rmask, uint Gmask, uint Bmask, uint Amask);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetPixelFormatDetails(SDL_PixelFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreatePalette(int ncolors);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetPaletteColors(IntPtr palette, SDL_Color[] colors, int firstcolor, int ncolors);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyPalette(IntPtr palette);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MapRGB(IntPtr format, IntPtr palette, byte r, byte g, byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MapRGBA(IntPtr format, IntPtr palette, byte r, byte g, byte b, byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetRGB(uint pixelvalue, IntPtr format, IntPtr palette, out byte r, out byte g, out byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetRGBA(uint pixelvalue, IntPtr format, IntPtr palette, out byte r, out byte g, out byte b, out byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasRectIntersection(ref SDL_Rect A, ref SDL_Rect B);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectIntersection(ref SDL_Rect A, ref SDL_Rect B, out SDL_Rect result);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectUnion(ref SDL_Rect A, ref SDL_Rect B, out SDL_Rect result);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectEnclosingPoints(SDL_Point[] points, int count, ref SDL_Rect clip, out SDL_Rect result);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectAndLineIntersection(ref SDL_Rect rect, ref int X1, ref int Y1, ref int X2, ref int Y2);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasRectIntersectionFloat(ref SDL_FRect A, ref SDL_FRect B);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectIntersectionFloat(ref SDL_FRect A, ref SDL_FRect B, out SDL_FRect result);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectUnionFloat(ref SDL_FRect A, ref SDL_FRect B, out SDL_FRect result);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectEnclosingPointsFloat(SDL_FPoint[] points, int count, ref SDL_FRect clip, out SDL_FRect result);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRectAndLineIntersectionFloat(ref SDL_FRect rect, ref float X1, ref float Y1, ref float X2, ref float Y2);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSurface(int width, int height, SDL_PixelFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSurfaceFrom(int width, int height, SDL_PixelFormat format, IntPtr pixels, int pitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroySurface(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetSurfaceProperties(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceColorspace(IntPtr surface, SDL_Colorspace colorspace);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Colorspace SDL_GetSurfaceColorspace(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSurfacePalette(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfacePalette(IntPtr surface, IntPtr palette);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetSurfacePalette(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_AddSurfaceAlternateImage(IntPtr surface, IntPtr image);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SurfaceHasAlternateImages(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetSurfaceImages(IntPtr surface, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RemoveSurfaceAlternateImages(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_LockSurface(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSurface(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_LoadSurface_IO(IntPtr src, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadSurface")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadSurface(byte* file);

        public unsafe static IntPtr SDL_LoadSurface(string file)
        {
            byte* intPtr = EncodeAsUTF8(file);
            IntPtr result = INTERNAL_SDL_LoadSurface(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_LoadBMP_IO(IntPtr src, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadBMP")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadBMP(byte* file);

        public unsafe static IntPtr SDL_LoadBMP(string file)
        {
            byte* intPtr = EncodeAsUTF8(file);
            IntPtr result = INTERNAL_SDL_LoadBMP(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SaveBMP_IO(IntPtr surface, IntPtr dst, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SaveBMP")]
        private unsafe static extern SDLBool INTERNAL_SDL_SaveBMP(IntPtr surface, byte* file);

        public unsafe static SDLBool SDL_SaveBMP(IntPtr surface, string file)
        {
            byte* ptr = EncodeAsUTF8(file);
            SDLBool result = INTERNAL_SDL_SaveBMP(surface, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_LoadPNG_IO(IntPtr src, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadPNG")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadPNG(byte* file);

        public unsafe static IntPtr SDL_LoadPNG(string file)
        {
            byte* intPtr = EncodeAsUTF8(file);
            IntPtr result = INTERNAL_SDL_LoadPNG(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SavePNG_IO(IntPtr surface, IntPtr dst, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SavePNG")]
        private unsafe static extern SDLBool INTERNAL_SDL_SavePNG(IntPtr surface, byte* file);

        public unsafe static SDLBool SDL_SavePNG(IntPtr surface, string file)
        {
            byte* ptr = EncodeAsUTF8(file);
            SDLBool result = INTERNAL_SDL_SavePNG(surface, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceRLE(IntPtr surface, SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SurfaceHasRLE(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceColorKey(IntPtr surface, SDLBool enabled, uint key);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SurfaceHasColorKey(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetSurfaceColorKey(IntPtr surface, out uint key);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceColorMod(IntPtr surface, byte r, byte g, byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetSurfaceColorMod(IntPtr surface, out byte r, out byte g, out byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceAlphaMod(IntPtr surface, byte alpha);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetSurfaceAlphaMod(IntPtr surface, out byte alpha);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceBlendMode(IntPtr surface, uint blendMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetSurfaceBlendMode(IntPtr surface, IntPtr blendMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetSurfaceClipRect(IntPtr surface, ref SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetSurfaceClipRect(IntPtr surface, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FlipSurface(IntPtr surface, SDL_FlipMode flip);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RotateSurface(IntPtr surface, float angle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_DuplicateSurface(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_ScaleSurface(IntPtr surface, int width, int height, SDL_ScaleMode scaleMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_ConvertSurface(IntPtr surface, SDL_PixelFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_ConvertSurfaceAndColorspace(IntPtr surface, SDL_PixelFormat format, IntPtr palette, SDL_Colorspace colorspace, uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ConvertPixels(int width, int height, SDL_PixelFormat src_format, IntPtr src, int src_pitch, SDL_PixelFormat dst_format, IntPtr dst, int dst_pitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ConvertPixelsAndColorspace(int width, int height, SDL_PixelFormat src_format, SDL_Colorspace src_colorspace, uint src_properties, IntPtr src, int src_pitch, SDL_PixelFormat dst_format, SDL_Colorspace dst_colorspace, uint dst_properties, IntPtr dst, int dst_pitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PremultiplyAlpha(int width, int height, SDL_PixelFormat src_format, IntPtr src, int src_pitch, SDL_PixelFormat dst_format, IntPtr dst, int dst_pitch, SDLBool linear);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PremultiplySurfaceAlpha(IntPtr surface, SDLBool linear);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ClearSurface(IntPtr surface, float r, float g, float b, float a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FillSurfaceRect(IntPtr dst, IntPtr rect, uint color);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FillSurfaceRects(IntPtr dst, SDL_Rect[] rects, int count, uint color);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurfaceUnchecked(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurfaceScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect, SDL_ScaleMode scaleMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurfaceUncheckedScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect, SDL_ScaleMode scaleMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StretchSurface(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect, SDL_ScaleMode scaleMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurfaceTiled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurfaceTiledWithScale(IntPtr src, IntPtr srcrect, float scale, SDL_ScaleMode scaleMode, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_BlitSurface9Grid(IntPtr src, IntPtr srcrect, int left_width, int right_width, int top_height, int bottom_height, float scale, SDL_ScaleMode scaleMode, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MapSurfaceRGB(IntPtr surface, byte r, byte g, byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MapSurfaceRGBA(IntPtr surface, byte r, byte g, byte b, byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadSurfacePixel(IntPtr surface, int x, int y, out byte r, out byte g, out byte b, out byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReadSurfacePixelFloat(IntPtr surface, int x, int y, out float r, out float g, out float b, out float a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteSurfacePixel(IntPtr surface, int x, int y, byte r, byte g, byte b, byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WriteSurfacePixelFloat(IntPtr surface, int x, int y, float r, float g, float b, float a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumCameraDrivers();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCameraDriver")]
        private static extern IntPtr INTERNAL_SDL_GetCameraDriver(int index);

        public static string SDL_GetCameraDriver(int index)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetCameraDriver(index));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentCameraDriver")]
        private static extern IntPtr INTERNAL_SDL_GetCurrentCameraDriver();

        public static string SDL_GetCurrentCameraDriver()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetCurrentCameraDriver());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetCameras(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetCameraSupportedFormats(uint instance_id, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCameraName")]
        private static extern IntPtr INTERNAL_SDL_GetCameraName(uint instance_id);

        public static string SDL_GetCameraName(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetCameraName(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_CameraPosition SDL_GetCameraPosition(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenCamera(uint instance_id, ref SDL_CameraSpec spec);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_CameraPermissionState SDL_GetCameraPermissionState(IntPtr camera);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetCameraID(IntPtr camera);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetCameraProperties(IntPtr camera);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetCameraFormat(IntPtr camera, out SDL_CameraSpec spec);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AcquireCameraFrame(IntPtr camera, out ulong timestampNS);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseCameraFrame(IntPtr camera, IntPtr frame);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseCamera(IntPtr camera);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetClipboardText")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetClipboardText(byte* text);

        public unsafe static SDLBool SDL_SetClipboardText(string text)
        {
            byte* intPtr = EncodeAsUTF8(text);
            SDLBool result = INTERNAL_SDL_SetClipboardText(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetClipboardText")]
        private static extern IntPtr INTERNAL_SDL_GetClipboardText();

        public static string SDL_GetClipboardText()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetClipboardText(), true);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasClipboardText();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetPrimarySelectionText")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetPrimarySelectionText(byte* text);

        public unsafe static SDLBool SDL_SetPrimarySelectionText(string text)
        {
            byte* intPtr = EncodeAsUTF8(text);
            SDLBool result = INTERNAL_SDL_SetPrimarySelectionText(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPrimarySelectionText")]
        private static extern IntPtr INTERNAL_SDL_GetPrimarySelectionText();

        public static string SDL_GetPrimarySelectionText()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetPrimarySelectionText(), true);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasPrimarySelectionText();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetClipboardData(SDL_ClipboardDataCallback callback, SDL_ClipboardCleanupCallback cleanup, IntPtr userdata, IntPtr mime_types, UIntPtr num_mime_types);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ClearClipboardData();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetClipboardData")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetClipboardData(byte* mime_type, out UIntPtr size);

        public unsafe static IntPtr SDL_GetClipboardData(string mime_type, out UIntPtr size)
        {
            byte* intPtr = EncodeAsUTF8(mime_type);
            IntPtr result = INTERNAL_SDL_GetClipboardData(intPtr, out size);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_HasClipboardData")]
        private unsafe static extern SDLBool INTERNAL_SDL_HasClipboardData(byte* mime_type);

        public unsafe static SDLBool SDL_HasClipboardData(string mime_type)
        {
            byte* intPtr = EncodeAsUTF8(mime_type);
            SDLBool result = INTERNAL_SDL_HasClipboardData(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetClipboardMimeTypes(out UIntPtr num_mime_types);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumLogicalCPUCores();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetCPUCacheLineSize();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasAltiVec();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasMMX();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasSSE();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasSSE2();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasSSE3();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasSSE41();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasSSE42();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasAVX();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasAVX2();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasAVX512F();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasARMSIMD();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasNEON();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasLSX();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasLASX();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSystemRAM();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr SDL_GetSIMDAlignment();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSystemPageSize();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumVideoDrivers();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetVideoDriver")]
        private static extern IntPtr INTERNAL_SDL_GetVideoDriver(int index);

        public static string SDL_GetVideoDriver(int index)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetVideoDriver(index));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentVideoDriver")]
        private static extern IntPtr INTERNAL_SDL_GetCurrentVideoDriver();

        public static string SDL_GetCurrentVideoDriver()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetCurrentVideoDriver());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SystemTheme SDL_GetSystemTheme();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetDisplays(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetPrimaryDisplay();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetDisplayProperties(uint displayID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDisplayName")]
        private static extern IntPtr INTERNAL_SDL_GetDisplayName(uint displayID);

        public static string SDL_GetDisplayName(uint displayID)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetDisplayName(displayID));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetDisplayBounds(uint displayID, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetDisplayUsableBounds(uint displayID, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_DisplayOrientation SDL_GetNaturalDisplayOrientation(uint displayID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_DisplayOrientation SDL_GetCurrentDisplayOrientation(uint displayID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetDisplayContentScale(uint displayID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetFullscreenDisplayModes(uint displayID, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetClosestFullscreenDisplayMode(uint displayID, int w, int h, float refresh_rate, SDLBool include_high_density_modes, out SDL_DisplayMode closest);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetDesktopDisplayMode(uint displayID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetCurrentDisplayMode(uint displayID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetDisplayForPoint(ref SDL_Point point);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetDisplayForRect(ref SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetDisplayForWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetWindowPixelDensity(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetWindowDisplayScale(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowFullscreenMode(IntPtr window, ref SDL_DisplayMode mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowFullscreenMode(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowICCProfile(IntPtr window, out UIntPtr size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PixelFormat SDL_GetWindowPixelFormat(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindows(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateWindow")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateWindow(byte* title, int w, int h, SDL_WindowFlags flags);

        public unsafe static IntPtr SDL_CreateWindow(string title, int w, int h, SDL_WindowFlags flags)
        {
            byte* intPtr = EncodeAsUTF8(title);
            IntPtr result = INTERNAL_SDL_CreateWindow(intPtr, w, h, flags);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreatePopupWindow(IntPtr parent, int offset_x, int offset_y, int w, int h, SDL_WindowFlags flags);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateWindowWithProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowID(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowFromID(uint id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowParent(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowProperties(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_WindowFlags SDL_GetWindowFlags(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowTitle")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetWindowTitle(IntPtr window, byte* title);

        public unsafe static SDLBool SDL_SetWindowTitle(IntPtr window, string title)
        {
            byte* ptr = EncodeAsUTF8(title);
            SDLBool result = INTERNAL_SDL_SetWindowTitle(window, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowTitle")]
        private static extern IntPtr INTERNAL_SDL_GetWindowTitle(IntPtr window);

        public static string SDL_GetWindowTitle(IntPtr window)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetWindowTitle(window));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowIcon(IntPtr window, IntPtr icon);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowPosition(IntPtr window, int x, int y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowPosition(IntPtr window, out int x, out int y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowSize(IntPtr window, int w, int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowSize(IntPtr window, out int w, out int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowSafeArea(IntPtr window, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowAspectRatio(IntPtr window, float min_aspect, float max_aspect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowAspectRatio(IntPtr window, out float min_aspect, out float max_aspect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowBordersSize(IntPtr window, out int top, out int left, out int bottom, out int right);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowSizeInPixels(IntPtr window, out int w, out int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowMinimumSize(IntPtr window, int min_w, int min_h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowMinimumSize(IntPtr window, out int w, out int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowMaximumSize(IntPtr window, int max_w, int max_h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowMaximumSize(IntPtr window, out int w, out int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowBordered(IntPtr window, SDLBool bordered);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowResizable(IntPtr window, SDLBool resizable);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowAlwaysOnTop(IntPtr window, SDLBool on_top);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowFillDocument(IntPtr window, SDLBool fill);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ShowWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HideWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RaiseWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_MaximizeWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_MinimizeWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RestoreWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowFullscreen(IntPtr window, SDLBool fullscreen);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SyncWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WindowHasSurface(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowSurface(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowSurfaceVSync(IntPtr window, int vsync);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowSurfaceVSync(IntPtr window, out int vsync);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UpdateWindowSurface(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UpdateWindowSurfaceRects(IntPtr window, SDL_Rect[] rects, int numrects);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_DestroyWindowSurface(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowKeyboardGrab(IntPtr window, SDLBool grabbed);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowMouseGrab(IntPtr window, SDLBool grabbed);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowKeyboardGrab(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowMouseGrab(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGrabbedWindow();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowMouseRect(IntPtr window, ref SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowMouseRect(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowOpacity(IntPtr window, float opacity);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetWindowOpacity(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowParent(IntPtr window, IntPtr parent);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowModal(IntPtr window, SDLBool modal);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowFocusable(IntPtr window, SDLBool focusable);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ShowWindowSystemMenu(IntPtr window, int x, int y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowHitTest(IntPtr window, SDL_HitTest callback, IntPtr callback_data);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowShape(IntPtr window, IntPtr shape);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FlashWindow(IntPtr window, SDL_FlashOperation operation);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowProgressState(IntPtr window, SDL_ProgressState state);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_ProgressState SDL_GetWindowProgressState(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowProgressValue(IntPtr window, float value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetWindowProgressValue(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ScreenSaverEnabled();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_EnableScreenSaver();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_DisableScreenSaver();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_LoadLibrary")]
        private unsafe static extern SDLBool INTERNAL_SDL_GL_LoadLibrary(byte* path);

        public unsafe static SDLBool SDL_GL_LoadLibrary(string path)
        {
            byte* intPtr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_GL_LoadLibrary(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_GetProcAddress")]
        private unsafe static extern IntPtr INTERNAL_SDL_GL_GetProcAddress(byte* proc);

        public unsafe static IntPtr SDL_GL_GetProcAddress(string proc)
        {
            byte* intPtr = EncodeAsUTF8(proc);
            IntPtr result = INTERNAL_SDL_GL_GetProcAddress(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_EGL_GetProcAddress")]
        private unsafe static extern IntPtr INTERNAL_SDL_EGL_GetProcAddress(byte* proc);

        public unsafe static IntPtr SDL_EGL_GetProcAddress(string proc)
        {
            byte* intPtr = EncodeAsUTF8(proc);
            IntPtr result = INTERNAL_SDL_EGL_GetProcAddress(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_UnloadLibrary();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_ExtensionSupported")]
        private unsafe static extern SDLBool INTERNAL_SDL_GL_ExtensionSupported(byte* extension);

        public unsafe static SDLBool SDL_GL_ExtensionSupported(string extension)
        {
            byte* intPtr = EncodeAsUTF8(extension);
            SDLBool result = INTERNAL_SDL_GL_ExtensionSupported(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_ResetAttributes();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_SetAttribute(SDL_GLAttr attr, int value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_GetAttribute(SDL_GLAttr attr, out int value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_CreateContext(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_MakeCurrent(IntPtr window, IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetCurrentWindow();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetCurrentContext();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_EGL_GetCurrentDisplay();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_EGL_GetCurrentConfig();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_EGL_GetWindowSurface(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_EGL_SetAttributeCallbacks(SDL_EGLAttribArrayCallback platformAttribCallback, SDL_EGLIntArrayCallback surfaceAttribCallback, SDL_EGLIntArrayCallback contextAttribCallback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_SetSwapInterval(int interval);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_GetSwapInterval(out int interval);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_SwapWindow(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GL_DestroyContext(IntPtr context);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowOpenFileDialog")]
        private unsafe static extern void INTERNAL_SDL_ShowOpenFileDialog(SDL_DialogFileCallback callback, IntPtr userdata, IntPtr window, SDL_DialogFileFilter[] filters, int nfilters, byte* default_location, SDLBool allow_many);

        public unsafe static void SDL_ShowOpenFileDialog(SDL_DialogFileCallback callback, IntPtr userdata, IntPtr window, SDL_DialogFileFilter[] filters, int nfilters, string default_location, SDLBool allow_many)
        {
            byte* ptr = EncodeAsUTF8(default_location);
            INTERNAL_SDL_ShowOpenFileDialog(callback, userdata, window, filters, nfilters, ptr, allow_many);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowSaveFileDialog")]
        private unsafe static extern void INTERNAL_SDL_ShowSaveFileDialog(SDL_DialogFileCallback callback, IntPtr userdata, IntPtr window, SDL_DialogFileFilter[] filters, int nfilters, byte* default_location);

        public unsafe static void SDL_ShowSaveFileDialog(SDL_DialogFileCallback callback, IntPtr userdata, IntPtr window, SDL_DialogFileFilter[] filters, int nfilters, string default_location)
        {
            byte* ptr = EncodeAsUTF8(default_location);
            INTERNAL_SDL_ShowSaveFileDialog(callback, userdata, window, filters, nfilters, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowOpenFolderDialog")]
        private unsafe static extern void INTERNAL_SDL_ShowOpenFolderDialog(SDL_DialogFileCallback callback, IntPtr userdata, IntPtr window, byte* default_location, SDLBool allow_many);

        public unsafe static void SDL_ShowOpenFolderDialog(SDL_DialogFileCallback callback, IntPtr userdata, IntPtr window, string default_location, SDLBool allow_many)
        {
            byte* ptr = EncodeAsUTF8(default_location);
            INTERNAL_SDL_ShowOpenFolderDialog(callback, userdata, window, ptr, allow_many);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ShowFileDialogWithProperties(SDL_FileDialogType type, SDL_DialogFileCallback callback, IntPtr userdata, uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GUIDToString")]
        private unsafe static extern void INTERNAL_SDL_GUIDToString(SDL_GUID guid, byte* pszGUID, int cbGUID);

        public unsafe static void SDL_GUIDToString(SDL_GUID guid, string pszGUID, int cbGUID)
        {
            byte* ptr = EncodeAsUTF8(pszGUID);
            INTERNAL_SDL_GUIDToString(guid, ptr, cbGUID);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_StringToGUID")]
        private unsafe static extern SDL_GUID INTERNAL_SDL_StringToGUID(byte* pchGUID);

        public unsafe static SDL_GUID SDL_StringToGUID(string pchGUID)
        {
            byte* intPtr = EncodeAsUTF8(pchGUID);
            SDL_GUID result = INTERNAL_SDL_StringToGUID(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PowerState SDL_GetPowerInfo(out int seconds, out int percent);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetSensors(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetSensorNameForID")]
        private static extern IntPtr INTERNAL_SDL_GetSensorNameForID(uint instance_id);

        public static string SDL_GetSensorNameForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetSensorNameForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SensorType SDL_GetSensorTypeForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSensorNonPortableTypeForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenSensor(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetSensorFromID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetSensorProperties(IntPtr sensor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetSensorName")]
        private static extern IntPtr INTERNAL_SDL_GetSensorName(IntPtr sensor);

        public static string SDL_GetSensorName(IntPtr sensor)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetSensorName(sensor));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SensorType SDL_GetSensorType(IntPtr sensor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSensorNonPortableType(IntPtr sensor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetSensorID(IntPtr sensor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern SDLBool SDL_GetSensorData(IntPtr sensor, float* data, int num_values);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseSensor(IntPtr sensor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UpdateSensors();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockJoysticks();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockJoysticks();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasJoystick();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetJoysticks(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetJoystickNameForID")]
        private static extern IntPtr INTERNAL_SDL_GetJoystickNameForID(uint instance_id);

        public static string SDL_GetJoystickNameForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetJoystickNameForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetJoystickPathForID")]
        private static extern IntPtr INTERNAL_SDL_GetJoystickPathForID(uint instance_id);

        public static string SDL_GetJoystickPathForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetJoystickPathForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetJoystickPlayerIndexForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GUID SDL_GetJoystickGUIDForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickVendorForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickProductForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickProductVersionForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickType SDL_GetJoystickTypeForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenJoystick(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetJoystickFromID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetJoystickFromPlayerIndex(int player_index);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_AttachVirtualJoystick(ref SDL_VirtualJoystickDesc desc);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_DetachVirtualJoystick(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsJoystickVirtual(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickVirtualAxis(IntPtr joystick, int axis, short value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickVirtualBall(IntPtr joystick, int ball, short xrel, short yrel);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickVirtualButton(IntPtr joystick, int button, SDLBool down);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickVirtualHat(IntPtr joystick, int hat, byte value);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickVirtualTouchpad(IntPtr joystick, int touchpad, int finger, SDLBool down, float x, float y, float pressure);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern SDLBool SDL_SendJoystickVirtualSensorData(IntPtr joystick, SDL_SensorType type, ulong sensor_timestamp, float* data, int num_values);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetJoystickProperties(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetJoystickName")]
        private static extern IntPtr INTERNAL_SDL_GetJoystickName(IntPtr joystick);

        public static string SDL_GetJoystickName(IntPtr joystick)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetJoystickName(joystick));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetJoystickPath")]
        private static extern IntPtr INTERNAL_SDL_GetJoystickPath(IntPtr joystick);

        public static string SDL_GetJoystickPath(IntPtr joystick)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetJoystickPath(joystick));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetJoystickPlayerIndex(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickPlayerIndex(IntPtr joystick, int player_index);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GUID SDL_GetJoystickGUID(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickVendor(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickProduct(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickProductVersion(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetJoystickFirmwareVersion(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetJoystickSerial")]
        private static extern IntPtr INTERNAL_SDL_GetJoystickSerial(IntPtr joystick);

        public static string SDL_GetJoystickSerial(IntPtr joystick)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetJoystickSerial(joystick));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickType SDL_GetJoystickType(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetJoystickGUIDInfo(SDL_GUID guid, out ushort vendor, out ushort product, out ushort version, out ushort crc16);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_JoystickConnected(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetJoystickID(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumJoystickAxes(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumJoystickBalls(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumJoystickHats(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumJoystickButtons(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetJoystickEventsEnabled(SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_JoystickEventsEnabled();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UpdateJoysticks();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SDL_GetJoystickAxis(IntPtr joystick, int axis);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetJoystickAxisInitialState(IntPtr joystick, int axis, out short state);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetJoystickBall(IntPtr joystick, int ball, out int dx, out int dy);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_GetJoystickHat(IntPtr joystick, int hat);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetJoystickButton(IntPtr joystick, int button);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RumbleJoystick(IntPtr joystick, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RumbleJoystickTriggers(IntPtr joystick, ushort left_rumble, ushort right_rumble, uint duration_ms);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetJoystickLED(IntPtr joystick, byte red, byte green, byte blue);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SendJoystickEffect(IntPtr joystick, IntPtr data, int size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseJoystick(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickConnectionState SDL_GetJoystickConnectionState(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PowerState SDL_GetJoystickPowerInfo(IntPtr joystick, out int percent);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AddGamepadMapping")]
        private unsafe static extern int INTERNAL_SDL_AddGamepadMapping(byte* mapping);

        public unsafe static int SDL_AddGamepadMapping(string mapping)
        {
            byte* intPtr = EncodeAsUTF8(mapping);
            int result = INTERNAL_SDL_AddGamepadMapping(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AddGamepadMappingsFromIO(IntPtr src, SDLBool closeio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AddGamepadMappingsFromFile")]
        private unsafe static extern int INTERNAL_SDL_AddGamepadMappingsFromFile(byte* file);

        public unsafe static int SDL_AddGamepadMappingsFromFile(string file)
        {
            byte* intPtr = EncodeAsUTF8(file);
            int result = INTERNAL_SDL_AddGamepadMappingsFromFile(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ReloadGamepadMappings();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGamepadMappings(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadMappingForGUID")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadMappingForGUID(SDL_GUID guid);

        public static string SDL_GetGamepadMappingForGUID(SDL_GUID guid)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadMappingForGUID(guid), true);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadMapping")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadMapping(IntPtr gamepad);

        public static string SDL_GetGamepadMapping(IntPtr gamepad)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadMapping(gamepad), true);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetGamepadMapping")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetGamepadMapping(uint instance_id, byte* mapping);

        public unsafe static SDLBool SDL_SetGamepadMapping(uint instance_id, string mapping)
        {
            byte* ptr = EncodeAsUTF8(mapping);
            SDLBool result = INTERNAL_SDL_SetGamepadMapping(instance_id, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasGamepad();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGamepads(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsGamepad(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadNameForID")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadNameForID(uint instance_id);

        public static string SDL_GetGamepadNameForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadNameForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadPathForID")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadPathForID(uint instance_id);

        public static string SDL_GetGamepadPathForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadPathForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetGamepadPlayerIndexForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GUID SDL_GetGamepadGUIDForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadVendorForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadProductForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadProductVersionForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GamepadType SDL_GetGamepadTypeForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GamepadType SDL_GetRealGamepadTypeForID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadMappingForID")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadMappingForID(uint instance_id);

        public static string SDL_GetGamepadMappingForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadMappingForID(instance_id), true);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenGamepad(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGamepadFromID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGamepadFromPlayerIndex(int player_index);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGamepadProperties(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGamepadID(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadName")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadName(IntPtr gamepad);

        public static string SDL_GetGamepadName(IntPtr gamepad)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadName(gamepad));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadPath")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadPath(IntPtr gamepad);

        public static string SDL_GetGamepadPath(IntPtr gamepad)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadPath(gamepad));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GamepadType SDL_GetGamepadType(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GamepadType SDL_GetRealGamepadType(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetGamepadPlayerIndex(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGamepadPlayerIndex(IntPtr gamepad, int player_index);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadVendor(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadProduct(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadProductVersion(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GetGamepadFirmwareVersion(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadSerial")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadSerial(IntPtr gamepad);

        public static string SDL_GetGamepadSerial(IntPtr gamepad)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadSerial(gamepad));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetGamepadSteamHandle(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickConnectionState SDL_GetGamepadConnectionState(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PowerState SDL_GetGamepadPowerInfo(IntPtr gamepad, out int percent);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GamepadConnected(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGamepadJoystick(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetGamepadEventsEnabled(SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GamepadEventsEnabled();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGamepadBindings(IntPtr gamepad, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UpdateGamepads();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadTypeFromString")]
        private unsafe static extern SDL_GamepadType INTERNAL_SDL_GetGamepadTypeFromString(byte* str);

        public unsafe static SDL_GamepadType SDL_GetGamepadTypeFromString(string str)
        {
            byte* intPtr = EncodeAsUTF8(str);
            SDL_GamepadType result = INTERNAL_SDL_GetGamepadTypeFromString(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadStringForType")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadStringForType(SDL_GamepadType type);

        public static string SDL_GetGamepadStringForType(SDL_GamepadType type)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadStringForType(type));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadAxisFromString")]
        private unsafe static extern SDL_GamepadAxis INTERNAL_SDL_GetGamepadAxisFromString(byte* str);

        public unsafe static SDL_GamepadAxis SDL_GetGamepadAxisFromString(string str)
        {
            byte* intPtr = EncodeAsUTF8(str);
            SDL_GamepadAxis result = INTERNAL_SDL_GetGamepadAxisFromString(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadStringForAxis")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadStringForAxis(SDL_GamepadAxis axis);

        public static string SDL_GetGamepadStringForAxis(SDL_GamepadAxis axis)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadStringForAxis(axis));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GamepadHasAxis(IntPtr gamepad, SDL_GamepadAxis axis);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SDL_GetGamepadAxis(IntPtr gamepad, SDL_GamepadAxis axis);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadButtonFromString")]
        private unsafe static extern SDL_GamepadButton INTERNAL_SDL_GetGamepadButtonFromString(byte* str);

        public unsafe static SDL_GamepadButton SDL_GetGamepadButtonFromString(string str)
        {
            byte* intPtr = EncodeAsUTF8(str);
            SDL_GamepadButton result = INTERNAL_SDL_GetGamepadButtonFromString(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadStringForButton")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadStringForButton(SDL_GamepadButton button);

        public static string SDL_GetGamepadStringForButton(SDL_GamepadButton button)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadStringForButton(button));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GamepadHasButton(IntPtr gamepad, SDL_GamepadButton button);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetGamepadButton(IntPtr gamepad, SDL_GamepadButton button);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GamepadButtonLabel SDL_GetGamepadButtonLabelForType(SDL_GamepadType type, SDL_GamepadButton button);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GamepadButtonLabel SDL_GetGamepadButtonLabel(IntPtr gamepad, SDL_GamepadButton button);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumGamepadTouchpads(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumGamepadTouchpadFingers(IntPtr gamepad, int touchpad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetGamepadTouchpadFinger(IntPtr gamepad, int touchpad, int finger, out SDLBool down, out float x, out float y, out float pressure);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GamepadHasSensor(IntPtr gamepad, SDL_SensorType type);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGamepadSensorEnabled(IntPtr gamepad, SDL_SensorType type, SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GamepadSensorEnabled(IntPtr gamepad, SDL_SensorType type);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetGamepadSensorDataRate(IntPtr gamepad, SDL_SensorType type);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern SDLBool SDL_GetGamepadSensorData(IntPtr gamepad, SDL_SensorType type, float* data, int num_values);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RumbleGamepad(IntPtr gamepad, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RumbleGamepadTriggers(IntPtr gamepad, ushort left_rumble, ushort right_rumble, uint duration_ms);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGamepadLED(IntPtr gamepad, byte red, byte green, byte blue);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SendGamepadEffect(IntPtr gamepad, IntPtr data, int size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseGamepad(IntPtr gamepad);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadAppleSFSymbolsNameForButton")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadAppleSFSymbolsNameForButton(IntPtr gamepad, SDL_GamepadButton button);

        public static string SDL_GetGamepadAppleSFSymbolsNameForButton(IntPtr gamepad, SDL_GamepadButton button)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadAppleSFSymbolsNameForButton(gamepad, button));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGamepadAppleSFSymbolsNameForAxis")]
        private static extern IntPtr INTERNAL_SDL_GetGamepadAppleSFSymbolsNameForAxis(IntPtr gamepad, SDL_GamepadAxis axis);

        public static string SDL_GetGamepadAppleSFSymbolsNameForAxis(IntPtr gamepad, SDL_GamepadAxis axis)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGamepadAppleSFSymbolsNameForAxis(gamepad, axis));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasKeyboard();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboards(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetKeyboardNameForID")]
        private static extern IntPtr INTERNAL_SDL_GetKeyboardNameForID(uint instance_id);

        public static string SDL_GetKeyboardNameForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetKeyboardNameForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardFocus();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardState(out int numkeys);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ResetKeyboard();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Keymod SDL_GetModState();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetModState(SDL_Keymod modstate);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetKeyFromScancode(SDL_Scancode scancode, SDL_Keymod modstate, SDLBool key_event);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Scancode SDL_GetScancodeFromKey(uint key, IntPtr modstate);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetScancodeName")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetScancodeName(SDL_Scancode scancode, byte* name);

        public unsafe static SDLBool SDL_SetScancodeName(SDL_Scancode scancode, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_SetScancodeName(scancode, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetScancodeName")]
        private static extern IntPtr INTERNAL_SDL_GetScancodeName(SDL_Scancode scancode);

        public static string SDL_GetScancodeName(SDL_Scancode scancode)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetScancodeName(scancode));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetScancodeFromName")]
        private unsafe static extern SDL_Scancode INTERNAL_SDL_GetScancodeFromName(byte* name);

        public unsafe static SDL_Scancode SDL_GetScancodeFromName(string name)
        {
            byte* intPtr = EncodeAsUTF8(name);
            SDL_Scancode result = INTERNAL_SDL_GetScancodeFromName(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetKeyName")]
        private static extern IntPtr INTERNAL_SDL_GetKeyName(uint key);

        public static string SDL_GetKeyName(uint key)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetKeyName(key));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetKeyFromName")]
        private unsafe static extern uint INTERNAL_SDL_GetKeyFromName(byte* name);

        public unsafe static uint SDL_GetKeyFromName(string name)
        {
            byte* intPtr = EncodeAsUTF8(name);
            uint result = INTERNAL_SDL_GetKeyFromName(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StartTextInput(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StartTextInputWithProperties(IntPtr window, uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TextInputActive(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StopTextInput(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ClearComposition(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextInputArea(IntPtr window, ref SDL_Rect rect, int cursor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextInputArea(IntPtr window, out SDL_Rect rect, out int cursor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasScreenKeyboardSupport();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ScreenKeyboardShown(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasMouse();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetMice(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetMouseNameForID")]
        private static extern IntPtr INTERNAL_SDL_GetMouseNameForID(uint instance_id);

        public static string SDL_GetMouseNameForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetMouseNameForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetMouseFocus();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_MouseButtonFlags SDL_GetMouseState(out float x, out float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_MouseButtonFlags SDL_GetGlobalMouseState(out float x, out float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_MouseButtonFlags SDL_GetRelativeMouseState(out float x, out float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_WarpMouseInWindow(IntPtr window, float x, float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WarpMouseGlobal(float x, float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRelativeMouseTransform(SDL_MouseMotionTransformCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetWindowRelativeMouseMode(IntPtr window, SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetWindowRelativeMouseMode(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CaptureMouse(SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateCursor(IntPtr data, IntPtr mask, int w, int h, int hot_x, int hot_y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateColorCursor(IntPtr surface, int hot_x, int hot_y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateAnimatedCursor(SDL_CursorFrameInfo[] frames, int frame_count, int hot_x, int hot_y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSystemCursor(SDL_SystemCursor id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetCursor(IntPtr cursor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetCursor();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetDefaultCursor();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyCursor(IntPtr cursor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ShowCursor();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HideCursor();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CursorVisible();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTouchDevices(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetTouchDeviceName")]
        private static extern IntPtr INTERNAL_SDL_GetTouchDeviceName(ulong touchID);

        public static string SDL_GetTouchDeviceName(ulong touchID)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetTouchDeviceName(touchID));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_TouchDeviceType SDL_GetTouchDeviceType(ulong touchID);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTouchFingers(ulong touchID, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PenDeviceType SDL_GetPenDeviceType(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PumpEvents();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PeepEvents([Out] SDL_Event[] events, int numevents, SDL_EventAction action, uint minType, uint maxType);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasEvent(uint type);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HasEvents(uint minType, uint maxType);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FlushEvent(uint type);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FlushEvents(uint minType, uint maxType);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PollEvent(out SDL_Event @event);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitEvent(out SDL_Event @event);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitEventTimeout(out SDL_Event @event, int timeoutMS);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PushEvent(ref SDL_Event @event);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetEventFilter(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetEventFilter(out SDL_EventFilter filter, out IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_AddEventWatch(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RemoveEventWatch(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FilterEvents(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetEventEnabled(uint type, SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_EventEnabled(uint type);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_RegisterEvents(int numevents);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowFromEvent(ref SDL_Event @event);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetEventDescription")]
        private unsafe static extern int INTERNAL_SDL_GetEventDescription(ref SDL_Event @event, byte* buf, int buflen);

        public unsafe static int SDL_GetEventDescription(ref SDL_Event @event, string buf, int buflen)
        {
            byte* ptr = EncodeAsUTF8(buf);
            int result = INTERNAL_SDL_GetEventDescription(ref @event, ptr, buflen);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetBasePath")]
        private static extern IntPtr INTERNAL_SDL_GetBasePath();

        public static string SDL_GetBasePath()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetBasePath());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPrefPath")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetPrefPath(byte* org, byte* app);

        public unsafe static string SDL_GetPrefPath(string org, string app)
        {
            byte* intPtr = EncodeAsUTF8(org);
            byte* ptr = EncodeAsUTF8(app);
            string result = DecodeFromUTF8(INTERNAL_SDL_GetPrefPath(intPtr, ptr), true);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetUserFolder")]
        private static extern IntPtr INTERNAL_SDL_GetUserFolder(SDL_Folder folder);

        public static string SDL_GetUserFolder(SDL_Folder folder)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetUserFolder(folder));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateDirectory")]
        private unsafe static extern SDLBool INTERNAL_SDL_CreateDirectory(byte* path);

        public unsafe static SDLBool SDL_CreateDirectory(string path)
        {
            byte* intPtr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_CreateDirectory(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_EnumerateDirectory")]
        private unsafe static extern SDLBool INTERNAL_SDL_EnumerateDirectory(byte* path, SDL_EnumerateDirectoryCallback callback, IntPtr userdata);

        public unsafe static SDLBool SDL_EnumerateDirectory(string path, SDL_EnumerateDirectoryCallback callback, IntPtr userdata)
        {
            byte* intPtr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_EnumerateDirectory(intPtr, callback, userdata);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RemovePath")]
        private unsafe static extern SDLBool INTERNAL_SDL_RemovePath(byte* path);

        public unsafe static SDLBool SDL_RemovePath(string path)
        {
            byte* intPtr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_RemovePath(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RenamePath")]
        private unsafe static extern SDLBool INTERNAL_SDL_RenamePath(byte* oldpath, byte* newpath);

        public unsafe static SDLBool SDL_RenamePath(string oldpath, string newpath)
        {
            byte* intPtr = EncodeAsUTF8(oldpath);
            byte* ptr = EncodeAsUTF8(newpath);
            SDLBool result = INTERNAL_SDL_RenamePath(intPtr, ptr);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CopyFile")]
        private unsafe static extern SDLBool INTERNAL_SDL_CopyFile(byte* oldpath, byte* newpath);

        public unsafe static SDLBool SDL_CopyFile(string oldpath, string newpath)
        {
            byte* intPtr = EncodeAsUTF8(oldpath);
            byte* ptr = EncodeAsUTF8(newpath);
            SDLBool result = INTERNAL_SDL_CopyFile(intPtr, ptr);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPathInfo")]
        private unsafe static extern SDLBool INTERNAL_SDL_GetPathInfo(byte* path, out SDL_PathInfo info);

        public unsafe static SDLBool SDL_GetPathInfo(string path, out SDL_PathInfo info)
        {
            byte* intPtr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_GetPathInfo(intPtr, out info);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GlobDirectory")]
        private unsafe static extern IntPtr INTERNAL_SDL_GlobDirectory(byte* path, byte* pattern, SDL_GlobFlags flags, out int count);

        public unsafe static IntPtr SDL_GlobDirectory(string path, string pattern, SDL_GlobFlags flags, out int count)
        {
            byte* intPtr = EncodeAsUTF8(path);
            byte* ptr = EncodeAsUTF8(pattern);
            IntPtr result = INTERNAL_SDL_GlobDirectory(intPtr, ptr, flags, out count);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentDirectory")]
        private static extern IntPtr INTERNAL_SDL_GetCurrentDirectory();

        public static string SDL_GetCurrentDirectory()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetCurrentDirectory(), true);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GPUSupportsShaderFormats")]
        private unsafe static extern SDLBool INTERNAL_SDL_GPUSupportsShaderFormats(SDL_GPUShaderFormat format_flags, byte* name);

        public unsafe static SDLBool SDL_GPUSupportsShaderFormats(SDL_GPUShaderFormat format_flags, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_GPUSupportsShaderFormats(format_flags, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GPUSupportsProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateGPUDevice")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateGPUDevice(SDL_GPUShaderFormat format_flags, SDLBool debug_mode, byte* name);

        public unsafe static IntPtr SDL_CreateGPUDevice(SDL_GPUShaderFormat format_flags, SDLBool debug_mode, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            IntPtr result = INTERNAL_SDL_CreateGPUDevice(format_flags, debug_mode, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUDeviceWithProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyGPUDevice(IntPtr device);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumGPUDrivers();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGPUDriver")]
        private static extern IntPtr INTERNAL_SDL_GetGPUDriver(int index);

        public static string SDL_GetGPUDriver(int index)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGPUDriver(index));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetGPUDeviceDriver")]
        private static extern IntPtr INTERNAL_SDL_GetGPUDeviceDriver(IntPtr device);

        public static string SDL_GetGPUDeviceDriver(IntPtr device)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetGPUDeviceDriver(device));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GPUShaderFormat SDL_GetGPUShaderFormats(IntPtr device);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGPUDeviceProperties(IntPtr device);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUComputePipeline(IntPtr device, ref SDL_GPUComputePipelineCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUGraphicsPipeline(IntPtr device, ref SDL_GPUGraphicsPipelineCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUSampler(IntPtr device, ref SDL_GPUSamplerCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUShader(IntPtr device, ref SDL_GPUShaderCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUTexture(IntPtr device, ref SDL_GPUTextureCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUBuffer(IntPtr device, ref SDL_GPUBufferCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPUTransferBuffer(IntPtr device, ref SDL_GPUTransferBufferCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetGPUBufferName")]
        private unsafe static extern void INTERNAL_SDL_SetGPUBufferName(IntPtr device, IntPtr buffer, byte* text);

        public unsafe static void SDL_SetGPUBufferName(IntPtr device, IntPtr buffer, string text)
        {
            byte* ptr = EncodeAsUTF8(text);
            INTERNAL_SDL_SetGPUBufferName(device, buffer, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetGPUTextureName")]
        private unsafe static extern void INTERNAL_SDL_SetGPUTextureName(IntPtr device, IntPtr texture, byte* text);

        public unsafe static void SDL_SetGPUTextureName(IntPtr device, IntPtr texture, string text)
        {
            byte* ptr = EncodeAsUTF8(text);
            INTERNAL_SDL_SetGPUTextureName(device, texture, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_InsertGPUDebugLabel")]
        private unsafe static extern void INTERNAL_SDL_InsertGPUDebugLabel(IntPtr command_buffer, byte* text);

        public unsafe static void SDL_InsertGPUDebugLabel(IntPtr command_buffer, string text)
        {
            byte* ptr = EncodeAsUTF8(text);
            INTERNAL_SDL_InsertGPUDebugLabel(command_buffer, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_PushGPUDebugGroup")]
        private unsafe static extern void INTERNAL_SDL_PushGPUDebugGroup(IntPtr command_buffer, byte* name);

        public unsafe static void SDL_PushGPUDebugGroup(IntPtr command_buffer, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            INTERNAL_SDL_PushGPUDebugGroup(command_buffer, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PopGPUDebugGroup(IntPtr command_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUTexture(IntPtr device, IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUSampler(IntPtr device, IntPtr sampler);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUBuffer(IntPtr device, IntPtr buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUTransferBuffer(IntPtr device, IntPtr transfer_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUComputePipeline(IntPtr device, IntPtr compute_pipeline);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUShader(IntPtr device, IntPtr shader);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUGraphicsPipeline(IntPtr device, IntPtr graphics_pipeline);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AcquireGPUCommandBuffer(IntPtr device);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PushGPUVertexUniformData(IntPtr command_buffer, uint slot_index, IntPtr data, uint length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PushGPUFragmentUniformData(IntPtr command_buffer, uint slot_index, IntPtr data, uint length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PushGPUComputeUniformData(IntPtr command_buffer, uint slot_index, IntPtr data, uint length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_BeginGPURenderPass(IntPtr command_buffer, SDL_GPUColorTargetInfo[] color_target_infos, uint num_color_targets, ref SDL_GPUDepthStencilTargetInfo depth_stencil_target_info);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUGraphicsPipeline(IntPtr render_pass, IntPtr graphics_pipeline);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetGPUViewport(IntPtr render_pass, ref SDL_GPUViewport viewport);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetGPUScissor(IntPtr render_pass, ref SDL_Rect scissor);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetGPUBlendConstants(IntPtr render_pass, SDL_FColor blend_constants);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetGPUStencilReference(IntPtr render_pass, byte reference);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUVertexBuffers(IntPtr render_pass, uint first_slot, SDL_GPUBufferBinding[] bindings, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUIndexBuffer(IntPtr render_pass, ref SDL_GPUBufferBinding binding, SDL_GPUIndexElementSize index_element_size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUVertexSamplers(IntPtr render_pass, uint first_slot, SDL_GPUTextureSamplerBinding[] texture_sampler_bindings, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUVertexStorageTextures(IntPtr render_pass, uint first_slot, IntPtr[] storage_textures, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUVertexStorageBuffers(IntPtr render_pass, uint first_slot, IntPtr[] storage_buffers, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUFragmentSamplers(IntPtr render_pass, uint first_slot, SDL_GPUTextureSamplerBinding[] texture_sampler_bindings, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUFragmentStorageTextures(IntPtr render_pass, uint first_slot, IntPtr[] storage_textures, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUFragmentStorageBuffers(IntPtr render_pass, uint first_slot, IntPtr[] storage_buffers, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DrawGPUIndexedPrimitives(IntPtr render_pass, uint num_indices, uint num_instances, uint first_index, int vertex_offset, uint first_instance);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DrawGPUPrimitives(IntPtr render_pass, uint num_vertices, uint num_instances, uint first_vertex, uint first_instance);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DrawGPUPrimitivesIndirect(IntPtr render_pass, IntPtr buffer, uint offset, uint draw_count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DrawGPUIndexedPrimitivesIndirect(IntPtr render_pass, IntPtr buffer, uint offset, uint draw_count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_EndGPURenderPass(IntPtr render_pass);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_BeginGPUComputePass(IntPtr command_buffer, SDL_GPUStorageTextureReadWriteBinding[] storage_texture_bindings, uint num_storage_texture_bindings, SDL_GPUStorageBufferReadWriteBinding[] storage_buffer_bindings, uint num_storage_buffer_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUComputePipeline(IntPtr compute_pass, IntPtr compute_pipeline);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUComputeSamplers(IntPtr compute_pass, uint first_slot, SDL_GPUTextureSamplerBinding[] texture_sampler_bindings, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUComputeStorageTextures(IntPtr compute_pass, uint first_slot, IntPtr[] storage_textures, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BindGPUComputeStorageBuffers(IntPtr compute_pass, uint first_slot, IntPtr[] storage_buffers, uint num_bindings);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DispatchGPUCompute(IntPtr compute_pass, uint groupcount_x, uint groupcount_y, uint groupcount_z);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DispatchGPUComputeIndirect(IntPtr compute_pass, IntPtr buffer, uint offset);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_EndGPUComputePass(IntPtr compute_pass);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_MapGPUTransferBuffer(IntPtr device, IntPtr transfer_buffer, SDLBool cycle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnmapGPUTransferBuffer(IntPtr device, IntPtr transfer_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_BeginGPUCopyPass(IntPtr command_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UploadToGPUTexture(IntPtr copy_pass, ref SDL_GPUTextureTransferInfo source, ref SDL_GPUTextureRegion destination, SDLBool cycle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UploadToGPUBuffer(IntPtr copy_pass, ref SDL_GPUTransferBufferLocation source, ref SDL_GPUBufferRegion destination, SDLBool cycle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CopyGPUTextureToTexture(IntPtr copy_pass, ref SDL_GPUTextureLocation source, ref SDL_GPUTextureLocation destination, uint w, uint h, uint d, SDLBool cycle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CopyGPUBufferToBuffer(IntPtr copy_pass, ref SDL_GPUBufferLocation source, ref SDL_GPUBufferLocation destination, uint size, SDLBool cycle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DownloadFromGPUTexture(IntPtr copy_pass, ref SDL_GPUTextureRegion source, ref SDL_GPUTextureTransferInfo destination);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DownloadFromGPUBuffer(IntPtr copy_pass, ref SDL_GPUBufferRegion source, ref SDL_GPUTransferBufferLocation destination);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_EndGPUCopyPass(IntPtr copy_pass);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GenerateMipmapsForGPUTexture(IntPtr command_buffer, IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_BlitGPUTexture(IntPtr command_buffer, ref SDL_GPUBlitInfo info);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WindowSupportsGPUSwapchainComposition(IntPtr device, IntPtr window, SDL_GPUSwapchainComposition swapchain_composition);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WindowSupportsGPUPresentMode(IntPtr device, IntPtr window, SDL_GPUPresentMode present_mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ClaimWindowForGPUDevice(IntPtr device, IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseWindowFromGPUDevice(IntPtr device, IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGPUSwapchainParameters(IntPtr device, IntPtr window, SDL_GPUSwapchainComposition swapchain_composition, SDL_GPUPresentMode present_mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGPUAllowedFramesInFlight(IntPtr device, uint allowed_frames_in_flight);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GPUTextureFormat SDL_GetGPUSwapchainTextureFormat(IntPtr device, IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_AcquireGPUSwapchainTexture(IntPtr command_buffer, IntPtr window, out IntPtr swapchain_texture, out uint swapchain_texture_width, out uint swapchain_texture_height);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitForGPUSwapchain(IntPtr device, IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitAndAcquireGPUSwapchainTexture(IntPtr command_buffer, IntPtr window, out IntPtr swapchain_texture, out uint swapchain_texture_width, out uint swapchain_texture_height);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SubmitGPUCommandBuffer(IntPtr command_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_SubmitGPUCommandBufferAndAcquireFence(IntPtr command_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CancelGPUCommandBuffer(IntPtr command_buffer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitForGPUIdle(IntPtr device);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitForGPUFences(IntPtr device, SDLBool wait_all, IntPtr[] fences, uint num_fences);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_QueryGPUFence(IntPtr device, IntPtr fence);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ReleaseGPUFence(IntPtr device, IntPtr fence);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GPUTextureFormatTexelBlockSize(SDL_GPUTextureFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GPUTextureSupportsFormat(IntPtr device, SDL_GPUTextureFormat format, SDL_GPUTextureType type, SDL_GPUTextureUsageFlags usage);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GPUTextureSupportsSampleCount(IntPtr device, SDL_GPUTextureFormat format, SDL_GPUSampleCount sample_count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_CalculateGPUTextureFormatSize(SDL_GPUTextureFormat format, uint width, uint height, uint depth_or_layer_count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PixelFormat SDL_GetPixelFormatFromGPUTextureFormat(SDL_GPUTextureFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GPUTextureFormat SDL_GetGPUTextureFormatFromPixelFormat(SDL_PixelFormat format);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetHaptics(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetHapticNameForID")]
        private static extern IntPtr INTERNAL_SDL_GetHapticNameForID(uint instance_id);

        public static string SDL_GetHapticNameForID(uint instance_id)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetHapticNameForID(instance_id));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenHaptic(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetHapticFromID(uint instance_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetHapticID(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetHapticName")]
        private static extern IntPtr INTERNAL_SDL_GetHapticName(IntPtr haptic);

        public static string SDL_GetHapticName(IntPtr haptic)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetHapticName(haptic));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsMouseHaptic();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenHapticFromMouse();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsJoystickHaptic(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenHapticFromJoystick(IntPtr joystick);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseHaptic(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetMaxHapticEffects(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetMaxHapticEffectsPlaying(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetHapticFeatures(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumHapticAxes(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HapticEffectSupported(IntPtr haptic, ref SDL_HapticEffect effect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_CreateHapticEffect(IntPtr haptic, ref SDL_HapticEffect effect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UpdateHapticEffect(IntPtr haptic, int effect, ref SDL_HapticEffect data);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RunHapticEffect(IntPtr haptic, int effect, uint iterations);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StopHapticEffect(IntPtr haptic, int effect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyHapticEffect(IntPtr haptic, int effect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetHapticEffectStatus(IntPtr haptic, int effect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetHapticGain(IntPtr haptic, int gain);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetHapticAutocenter(IntPtr haptic, int autocenter);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PauseHaptic(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ResumeHaptic(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StopHapticEffects(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_HapticRumbleSupported(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_InitHapticRumble(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_PlayHapticRumble(IntPtr haptic, float strength, uint length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StopHapticRumble(IntPtr haptic);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_init();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_exit();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_hid_device_change_count();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_hid_enumerate(ushort vendor_id, ushort product_id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_hid_free_enumeration(IntPtr devs);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_hid_open")]
        private unsafe static extern IntPtr INTERNAL_SDL_hid_open(ushort vendor_id, ushort product_id, byte* serial_number);

        public unsafe static IntPtr SDL_hid_open(ushort vendor_id, ushort product_id, string serial_number)
        {
            byte* ptr = EncodeAsUTF8(serial_number);
            IntPtr result = INTERNAL_SDL_hid_open(vendor_id, product_id, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_hid_open_path")]
        private unsafe static extern IntPtr INTERNAL_SDL_hid_open_path(byte* path);

        public unsafe static IntPtr SDL_hid_open_path(string path)
        {
            byte* intPtr = EncodeAsUTF8(path);
            IntPtr result = INTERNAL_SDL_hid_open_path(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_hid_get_properties(IntPtr dev);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_write(IntPtr dev, IntPtr data, UIntPtr length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_read_timeout(IntPtr dev, IntPtr data, UIntPtr length, int milliseconds);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_read(IntPtr dev, IntPtr data, UIntPtr length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_set_nonblocking(IntPtr dev, int nonblock);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_send_feature_report(IntPtr dev, IntPtr data, UIntPtr length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_get_feature_report(IntPtr dev, IntPtr data, UIntPtr length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_get_input_report(IntPtr dev, IntPtr data, UIntPtr length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_close(IntPtr dev);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_hid_get_manufacturer_string")]
        private unsafe static extern int INTERNAL_SDL_hid_get_manufacturer_string(IntPtr dev, byte* @string, UIntPtr maxlen);

        public unsafe static int SDL_hid_get_manufacturer_string(IntPtr dev, string @string, UIntPtr maxlen)
        {
            byte* ptr = EncodeAsUTF8(@string);
            int result = INTERNAL_SDL_hid_get_manufacturer_string(dev, ptr, maxlen);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_hid_get_product_string")]
        private unsafe static extern int INTERNAL_SDL_hid_get_product_string(IntPtr dev, byte* @string, UIntPtr maxlen);

        public unsafe static int SDL_hid_get_product_string(IntPtr dev, string @string, UIntPtr maxlen)
        {
            byte* ptr = EncodeAsUTF8(@string);
            int result = INTERNAL_SDL_hid_get_product_string(dev, ptr, maxlen);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_hid_get_serial_number_string")]
        private unsafe static extern int INTERNAL_SDL_hid_get_serial_number_string(IntPtr dev, byte* @string, UIntPtr maxlen);

        public unsafe static int SDL_hid_get_serial_number_string(IntPtr dev, string @string, UIntPtr maxlen)
        {
            byte* ptr = EncodeAsUTF8(@string);
            int result = INTERNAL_SDL_hid_get_serial_number_string(dev, ptr, maxlen);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_hid_get_indexed_string")]
        private unsafe static extern int INTERNAL_SDL_hid_get_indexed_string(IntPtr dev, int string_index, byte* @string, UIntPtr maxlen);

        public unsafe static int SDL_hid_get_indexed_string(IntPtr dev, int string_index, string @string, UIntPtr maxlen)
        {
            byte* ptr = EncodeAsUTF8(@string);
            int result = INTERNAL_SDL_hid_get_indexed_string(dev, string_index, ptr, maxlen);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_hid_get_device_info(IntPtr dev);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_hid_get_report_descriptor(IntPtr dev, IntPtr buf, UIntPtr buf_size);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_hid_ble_scan(SDLBool active);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetHintWithPriority")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetHintWithPriority(byte* name, byte* value, SDL_HintPriority priority);

        public unsafe static SDLBool SDL_SetHintWithPriority(string name, string value, SDL_HintPriority priority)
        {
            byte* intPtr = EncodeAsUTF8(name);
            byte* ptr = EncodeAsUTF8(value);
            SDLBool result = INTERNAL_SDL_SetHintWithPriority(intPtr, ptr, priority);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetHint")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetHint(byte* name, byte* value);

        public unsafe static SDLBool SDL_SetHint(string name, string value)
        {
            byte* intPtr = EncodeAsUTF8(name);
            byte* ptr = EncodeAsUTF8(value);
            SDLBool result = INTERNAL_SDL_SetHint(intPtr, ptr);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ResetHint")]
        private unsafe static extern SDLBool INTERNAL_SDL_ResetHint(byte* name);

        public unsafe static SDLBool SDL_ResetHint(string name)
        {
            byte* intPtr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_ResetHint(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ResetHints();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetHint")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetHint(byte* name);

        public unsafe static string SDL_GetHint(string name)
        {
            byte* intPtr = EncodeAsUTF8(name);
            string result = DecodeFromUTF8(INTERNAL_SDL_GetHint(intPtr));
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetHintBoolean")]
        private unsafe static extern SDLBool INTERNAL_SDL_GetHintBoolean(byte* name, SDLBool default_value);

        public unsafe static SDLBool SDL_GetHintBoolean(string name, SDLBool default_value)
        {
            byte* intPtr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_GetHintBoolean(intPtr, default_value);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AddHintCallback")]
        private unsafe static extern SDLBool INTERNAL_SDL_AddHintCallback(byte* name, SDL_HintCallback callback, IntPtr userdata);

        public unsafe static SDLBool SDL_AddHintCallback(string name, SDL_HintCallback callback, IntPtr userdata)
        {
            byte* intPtr = EncodeAsUTF8(name);
            SDLBool result = INTERNAL_SDL_AddHintCallback(intPtr, callback, userdata);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RemoveHintCallback")]
        private unsafe static extern void INTERNAL_SDL_RemoveHintCallback(byte* name, SDL_HintCallback callback, IntPtr userdata);

        public unsafe static void SDL_RemoveHintCallback(string name, SDL_HintCallback callback, IntPtr userdata)
        {
            byte* intPtr = EncodeAsUTF8(name);
            INTERNAL_SDL_RemoveHintCallback(intPtr, callback, userdata);
            SDL_free((IntPtr)intPtr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_Init(SDL_InitFlags flags);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_InitSubSystem(SDL_InitFlags flags);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_QuitSubSystem(SDL_InitFlags flags);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_InitFlags SDL_WasInit(SDL_InitFlags flags);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsMainThread();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RunOnMainThread(SDL_MainThreadCallback callback, IntPtr userdata, SDLBool wait_complete);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetAppMetadata")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetAppMetadata(byte* appname, byte* appversion, byte* appidentifier);

        public unsafe static SDLBool SDL_SetAppMetadata(string appname, string appversion, string appidentifier)
        {
            byte* intPtr = EncodeAsUTF8(appname);
            byte* ptr = EncodeAsUTF8(appversion);
            byte* ptr2 = EncodeAsUTF8(appidentifier);
            SDLBool result = INTERNAL_SDL_SetAppMetadata(intPtr, ptr, ptr2);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetAppMetadataProperty")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetAppMetadataProperty(byte* name, byte* value);

        public unsafe static SDLBool SDL_SetAppMetadataProperty(string name, string value)
        {
            byte* intPtr = EncodeAsUTF8(name);
            byte* ptr = EncodeAsUTF8(value);
            SDLBool result = INTERNAL_SDL_SetAppMetadataProperty(intPtr, ptr);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetAppMetadataProperty")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetAppMetadataProperty(byte* name);

        public unsafe static string SDL_GetAppMetadataProperty(string name)
        {
            byte* intPtr = EncodeAsUTF8(name);
            string result = DecodeFromUTF8(INTERNAL_SDL_GetAppMetadataProperty(intPtr));
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadObject")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadObject(byte* sofile);

        public unsafe static IntPtr SDL_LoadObject(string sofile)
        {
            byte* intPtr = EncodeAsUTF8(sofile);
            IntPtr result = INTERNAL_SDL_LoadObject(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadFunction")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadFunction(IntPtr handle, byte* name);

        public unsafe static IntPtr SDL_LoadFunction(IntPtr handle, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            IntPtr result = INTERNAL_SDL_LoadFunction(handle, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnloadObject(IntPtr handle);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetPreferredLocales(out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetLogPriorities(SDL_LogPriority priority);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetLogPriority(int category, SDL_LogPriority priority);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_LogPriority SDL_GetLogPriority(int category);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ResetLogPriorities();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetLogPriorityPrefix")]
        private unsafe static extern SDLBool INTERNAL_SDL_SetLogPriorityPrefix(SDL_LogPriority priority, byte* prefix);

        public unsafe static SDLBool SDL_SetLogPriorityPrefix(SDL_LogPriority priority, string prefix)
        {
            byte* ptr = EncodeAsUTF8(prefix);
            SDLBool result = INTERNAL_SDL_SetLogPriorityPrefix(priority, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_Log")]
        private unsafe static extern void INTERNAL_SDL_Log(byte* fmt);

        public unsafe static void SDL_Log(string fmt)
        {
            byte* intPtr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_Log(intPtr);
            SDL_free((IntPtr)intPtr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogTrace")]
        private unsafe static extern void INTERNAL_SDL_LogTrace(int category, byte* fmt);

        public unsafe static void SDL_LogTrace(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogTrace(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogVerbose")]
        private unsafe static extern void INTERNAL_SDL_LogVerbose(int category, byte* fmt);

        public unsafe static void SDL_LogVerbose(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogVerbose(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogDebug")]
        private unsafe static extern void INTERNAL_SDL_LogDebug(int category, byte* fmt);

        public unsafe static void SDL_LogDebug(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogDebug(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogInfo")]
        private unsafe static extern void INTERNAL_SDL_LogInfo(int category, byte* fmt);

        public unsafe static void SDL_LogInfo(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogInfo(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogWarn")]
        private unsafe static extern void INTERNAL_SDL_LogWarn(int category, byte* fmt);

        public unsafe static void SDL_LogWarn(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogWarn(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogError")]
        private unsafe static extern void INTERNAL_SDL_LogError(int category, byte* fmt);

        public unsafe static void SDL_LogError(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogError(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogCritical")]
        private unsafe static extern void INTERNAL_SDL_LogCritical(int category, byte* fmt);

        public unsafe static void SDL_LogCritical(int category, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogCritical(category, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogMessage")]
        private unsafe static extern void INTERNAL_SDL_LogMessage(int category, SDL_LogPriority priority, byte* fmt);

        public unsafe static void SDL_LogMessage(int category, SDL_LogPriority priority, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            INTERNAL_SDL_LogMessage(category, priority, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetDefaultLogOutputFunction();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetLogOutputFunction(out SDL_LogOutputFunction callback, out IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetLogOutputFunction(SDL_LogOutputFunction callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ShowMessageBox(ref SDL_MessageBoxData messageboxdata, out int buttonid);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowSimpleMessageBox")]
        private unsafe static extern SDLBool INTERNAL_SDL_ShowSimpleMessageBox(SDL_MessageBoxFlags flags, byte* title, byte* message, IntPtr window);

        public unsafe static SDLBool SDL_ShowSimpleMessageBox(SDL_MessageBoxFlags flags, string title, string message, IntPtr window)
        {
            byte* ptr = EncodeAsUTF8(title);
            byte* ptr2 = EncodeAsUTF8(message);
            SDLBool result = INTERNAL_SDL_ShowSimpleMessageBox(flags, ptr, ptr2, window);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_Metal_CreateView(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Metal_DestroyView(IntPtr view);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_Metal_GetLayer(IntPtr view);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_OpenURL")]
        private unsafe static extern SDLBool INTERNAL_SDL_OpenURL(byte* url);

        public unsafe static SDLBool SDL_OpenURL(string url)
        {
            byte* intPtr = EncodeAsUTF8(url);
            SDLBool result = INTERNAL_SDL_OpenURL(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPlatform")]
        private static extern IntPtr INTERNAL_SDL_GetPlatform();

        public static string SDL_GetPlatform()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetPlatform());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateProcess(IntPtr args, SDLBool pipe_stdio);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateProcessWithProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetProcessProperties(IntPtr process);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_ReadProcess(IntPtr process, out UIntPtr datasize, out int exitcode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetProcessInput(IntPtr process);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetProcessOutput(IntPtr process);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_KillProcess(IntPtr process, SDLBool force);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_WaitProcess(IntPtr process, SDLBool block, out int exitcode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyProcess(IntPtr process);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumRenderDrivers();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetRenderDriver")]
        private static extern IntPtr INTERNAL_SDL_GetRenderDriver(int index);

        public static string SDL_GetRenderDriver(int index)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetRenderDriver(index));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateWindowAndRenderer")]
        private unsafe static extern SDLBool INTERNAL_SDL_CreateWindowAndRenderer(byte* title, int width, int height, SDL_WindowFlags window_flags, out IntPtr window, out IntPtr renderer);

        public unsafe static SDLBool SDL_CreateWindowAndRenderer(string title, int width, int height, SDL_WindowFlags window_flags, out IntPtr window, out IntPtr renderer)
        {
            byte* ptr = EncodeAsUTF8(title);
            SDLBool result = INTERNAL_SDL_CreateWindowAndRenderer(ptr, width, height, window_flags, out window, out renderer);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateRenderer")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateRenderer(IntPtr window, byte* name);

        public unsafe static IntPtr SDL_CreateRenderer(IntPtr window, string name)
        {
            byte* ptr = EncodeAsUTF8(name);
            IntPtr result = INTERNAL_SDL_CreateRenderer(window, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRendererWithProperties(uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPURenderer(IntPtr device, IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGPURendererDevice(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSoftwareRenderer(IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderer(IntPtr window);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderWindow(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetRendererName")]
        private static extern IntPtr INTERNAL_SDL_GetRendererName(IntPtr renderer);

        public static string SDL_GetRendererName(IntPtr renderer)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetRendererName(renderer));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetRendererProperties(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderOutputSize(IntPtr renderer, out int w, out int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetCurrentRenderOutputSize(IntPtr renderer, out int w, out int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTexture(IntPtr renderer, SDL_PixelFormat format, SDL_TextureAccess access, int w, int h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTextureWithProperties(IntPtr renderer, uint props);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetTextureProperties(IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRendererFromTexture(IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureSize(IntPtr texture, out float w, out float h);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTexturePalette(IntPtr texture, ref SDL_Palette palette);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTexturePalette(IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextureColorMod(IntPtr texture, byte r, byte g, byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextureColorModFloat(IntPtr texture, float r, float g, float b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureColorMod(IntPtr texture, out byte r, out byte g, out byte b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureColorModFloat(IntPtr texture, out float r, out float g, out float b);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextureAlphaMod(IntPtr texture, byte alpha);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextureAlphaModFloat(IntPtr texture, float alpha);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureAlphaMod(IntPtr texture, out byte alpha);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureAlphaModFloat(IntPtr texture, out float alpha);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextureBlendMode(IntPtr texture, uint blendMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureBlendMode(IntPtr texture, IntPtr blendMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetTextureScaleMode(IntPtr texture, SDL_ScaleMode scaleMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTextureScaleMode(IntPtr texture, out SDL_ScaleMode scaleMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UpdateTexture(IntPtr texture, ref SDL_Rect rect, IntPtr pixels, int pitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UpdateYUVTexture(IntPtr texture, ref SDL_Rect rect, IntPtr Yplane, int Ypitch, IntPtr Uplane, int Upitch, IntPtr Vplane, int Vpitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_UpdateNVTexture(IntPtr texture, ref SDL_Rect rect, IntPtr Yplane, int Ypitch, IntPtr UVplane, int UVpitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_LockTexture(IntPtr texture, ref SDL_Rect rect, out IntPtr pixels, out int pitch);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_LockTextureToSurface(IntPtr texture, ref SDL_Rect rect, out IntPtr surface);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockTexture(IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderTarget(IntPtr renderer, IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderTarget(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderLogicalPresentation(IntPtr renderer, int w, int h, SDL_RendererLogicalPresentation mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderLogicalPresentation(IntPtr renderer, out int w, out int h, out SDL_RendererLogicalPresentation mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderLogicalPresentationRect(IntPtr renderer, out SDL_FRect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderCoordinatesFromWindow(IntPtr renderer, float window_x, float window_y, out float x, out float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderCoordinatesToWindow(IntPtr renderer, float x, float y, out float window_x, out float window_y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_ConvertEventToRenderCoordinates(IntPtr renderer, ref SDL_Event @event);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderViewport(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderViewport(IntPtr renderer, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderViewportSet(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderSafeArea(IntPtr renderer, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderClipRect(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderClipRect(IntPtr renderer, out SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderClipEnabled(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderScale(IntPtr renderer, float scaleX, float scaleY);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderScale(IntPtr renderer, out float scaleX, out float scaleY);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderDrawColorFloat(IntPtr renderer, float r, float g, float b, float a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderDrawColor(IntPtr renderer, out byte r, out byte g, out byte b, out byte a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderDrawColorFloat(IntPtr renderer, out float r, out float g, out float b, out float a);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderColorScale(IntPtr renderer, float scale);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderColorScale(IntPtr renderer, out float scale);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderDrawBlendMode(IntPtr renderer, uint blendMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderDrawBlendMode(IntPtr renderer, IntPtr blendMode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderClear(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderPoint(IntPtr renderer, float x, float y);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderPoints(IntPtr renderer, SDL_FPoint[] points, int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderLine(IntPtr renderer, float x1, float y1, float x2, float y2);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderLines(IntPtr renderer, SDL_FPoint[] points, int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderRect(IntPtr renderer, ref SDL_FRect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderRects(IntPtr renderer, SDL_FRect[] rects, int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderFillRect(IntPtr renderer, ref SDL_FRect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderFillRects(IntPtr renderer, SDL_FRect[] rects, int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderTexture(IntPtr renderer, IntPtr texture, ref SDL_FRect srcrect, ref SDL_FRect dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderTextureRotated(IntPtr renderer, IntPtr texture, ref SDL_FRect srcrect, ref SDL_FRect dstrect, double angle, ref SDL_FPoint center, SDL_FlipMode flip);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderTextureAffine(IntPtr renderer, IntPtr texture, ref SDL_FRect srcrect, ref SDL_FPoint origin, ref SDL_FPoint right, ref SDL_FPoint down);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderTextureTiled(IntPtr renderer, IntPtr texture, ref SDL_FRect srcrect, float scale, ref SDL_FRect dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderTexture9Grid(IntPtr renderer, IntPtr texture, ref SDL_FRect srcrect, float left_width, float right_width, float top_height, float bottom_height, float scale, ref SDL_FRect dstrect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderTexture9GridTiled(IntPtr renderer, IntPtr texture, ref SDL_FRect srcrect, float left_width, float right_width, float top_height, float bottom_height, float scale, ref SDL_FRect dstrect, float tileScale);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderGeometry(IntPtr renderer, IntPtr texture, SDL_Vertex[] vertices, int num_vertices, int[] indices, int num_indices);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderGeometryRaw(IntPtr renderer, IntPtr texture, IntPtr xy, int xy_stride, IntPtr color, int color_stride, IntPtr uv, int uv_stride, int num_vertices, IntPtr indices, int num_indices, int size_indices);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderTextureAddressMode(IntPtr renderer, SDL_TextureAddressMode u_mode, SDL_TextureAddressMode v_mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderTextureAddressMode(IntPtr renderer, out SDL_TextureAddressMode u_mode, out SDL_TextureAddressMode v_mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderReadPixels(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RenderPresent(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyTexture(IntPtr texture);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyRenderer(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_FlushRenderer(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderMetalLayer(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderMetalCommandEncoder(IntPtr renderer);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_AddVulkanRenderSemaphores(IntPtr renderer, uint wait_stage_mask, long wait_semaphore, long signal_semaphore);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetRenderVSync(IntPtr renderer, int vsync);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetRenderVSync(IntPtr renderer, out int vsync);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RenderDebugText")]
        private unsafe static extern SDLBool INTERNAL_SDL_RenderDebugText(IntPtr renderer, float x, float y, byte* str);

        public unsafe static SDLBool SDL_RenderDebugText(IntPtr renderer, float x, float y, string str)
        {
            byte* ptr = EncodeAsUTF8(str);
            SDLBool result = INTERNAL_SDL_RenderDebugText(renderer, x, y, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RenderDebugTextFormat")]
        private unsafe static extern SDLBool INTERNAL_SDL_RenderDebugTextFormat(IntPtr renderer, float x, float y, byte* fmt);

        public unsafe static SDLBool SDL_RenderDebugTextFormat(IntPtr renderer, float x, float y, string fmt)
        {
            byte* ptr = EncodeAsUTF8(fmt);
            SDLBool result = INTERNAL_SDL_RenderDebugTextFormat(renderer, x, y, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetDefaultTextureScaleMode(IntPtr renderer, SDL_ScaleMode scale_mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetDefaultTextureScaleMode(IntPtr renderer, out SDL_ScaleMode scale_mode);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateGPURenderState(IntPtr renderer, ref SDL_GPURenderStateCreateInfo createinfo);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGPURenderStateFragmentUniforms(IntPtr state, uint slot_index, IntPtr data, uint length);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_SetGPURenderState(IntPtr renderer, IntPtr state);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyGPURenderState(IntPtr state);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_OpenTitleStorage")]
        private unsafe static extern IntPtr INTERNAL_SDL_OpenTitleStorage(byte* @override, uint props);

        public unsafe static IntPtr SDL_OpenTitleStorage(string @override, uint props)
        {
            byte* intPtr = EncodeAsUTF8(@override);
            IntPtr result = INTERNAL_SDL_OpenTitleStorage(intPtr, props);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_OpenUserStorage")]
        private unsafe static extern IntPtr INTERNAL_SDL_OpenUserStorage(byte* org, byte* app, uint props);

        public unsafe static IntPtr SDL_OpenUserStorage(string org, string app, uint props)
        {
            byte* intPtr = EncodeAsUTF8(org);
            byte* ptr = EncodeAsUTF8(app);
            IntPtr result = INTERNAL_SDL_OpenUserStorage(intPtr, ptr, props);
            SDL_free((IntPtr)intPtr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_OpenFileStorage")]
        private unsafe static extern IntPtr INTERNAL_SDL_OpenFileStorage(byte* path);

        public unsafe static IntPtr SDL_OpenFileStorage(string path)
        {
            byte* intPtr = EncodeAsUTF8(path);
            IntPtr result = INTERNAL_SDL_OpenFileStorage(intPtr);
            SDL_free((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_OpenStorage(ref SDL_StorageInterface iface, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_CloseStorage(IntPtr storage);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_StorageReady(IntPtr storage);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetStorageFileSize")]
        private unsafe static extern SDLBool INTERNAL_SDL_GetStorageFileSize(IntPtr storage, byte* path, out ulong length);

        public unsafe static SDLBool SDL_GetStorageFileSize(IntPtr storage, string path, out ulong length)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_GetStorageFileSize(storage, ptr, out length);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ReadStorageFile")]
        private unsafe static extern SDLBool INTERNAL_SDL_ReadStorageFile(IntPtr storage, byte* path, IntPtr destination, ulong length);

        public unsafe static SDLBool SDL_ReadStorageFile(IntPtr storage, string path, IntPtr destination, ulong length)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_ReadStorageFile(storage, ptr, destination, length);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_WriteStorageFile")]
        private unsafe static extern SDLBool INTERNAL_SDL_WriteStorageFile(IntPtr storage, byte* path, IntPtr source, ulong length);

        public unsafe static SDLBool SDL_WriteStorageFile(IntPtr storage, string path, IntPtr source, ulong length)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_WriteStorageFile(storage, ptr, source, length);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateStorageDirectory")]
        private unsafe static extern SDLBool INTERNAL_SDL_CreateStorageDirectory(IntPtr storage, byte* path);

        public unsafe static SDLBool SDL_CreateStorageDirectory(IntPtr storage, string path)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_CreateStorageDirectory(storage, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_EnumerateStorageDirectory")]
        private unsafe static extern SDLBool INTERNAL_SDL_EnumerateStorageDirectory(IntPtr storage, byte* path, SDL_EnumerateDirectoryCallback callback, IntPtr userdata);

        public unsafe static SDLBool SDL_EnumerateStorageDirectory(IntPtr storage, string path, SDL_EnumerateDirectoryCallback callback, IntPtr userdata)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_EnumerateStorageDirectory(storage, ptr, callback, userdata);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RemoveStoragePath")]
        private unsafe static extern SDLBool INTERNAL_SDL_RemoveStoragePath(IntPtr storage, byte* path);

        public unsafe static SDLBool SDL_RemoveStoragePath(IntPtr storage, string path)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_RemoveStoragePath(storage, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RenameStoragePath")]
        private unsafe static extern SDLBool INTERNAL_SDL_RenameStoragePath(IntPtr storage, byte* oldpath, byte* newpath);

        public unsafe static SDLBool SDL_RenameStoragePath(IntPtr storage, string oldpath, string newpath)
        {
            byte* ptr = EncodeAsUTF8(oldpath);
            byte* ptr2 = EncodeAsUTF8(newpath);
            SDLBool result = INTERNAL_SDL_RenameStoragePath(storage, ptr, ptr2);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CopyStorageFile")]
        private unsafe static extern SDLBool INTERNAL_SDL_CopyStorageFile(IntPtr storage, byte* oldpath, byte* newpath);

        public unsafe static SDLBool SDL_CopyStorageFile(IntPtr storage, string oldpath, string newpath)
        {
            byte* ptr = EncodeAsUTF8(oldpath);
            byte* ptr2 = EncodeAsUTF8(newpath);
            SDLBool result = INTERNAL_SDL_CopyStorageFile(storage, ptr, ptr2);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetStoragePathInfo")]
        private unsafe static extern SDLBool INTERNAL_SDL_GetStoragePathInfo(IntPtr storage, byte* path, out SDL_PathInfo info);

        public unsafe static SDLBool SDL_GetStoragePathInfo(IntPtr storage, string path, out SDL_PathInfo info)
        {
            byte* ptr = EncodeAsUTF8(path);
            SDLBool result = INTERNAL_SDL_GetStoragePathInfo(storage, ptr, out info);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetStorageSpaceRemaining(IntPtr storage);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GlobStorageDirectory")]
        private unsafe static extern IntPtr INTERNAL_SDL_GlobStorageDirectory(IntPtr storage, byte* path, byte* pattern, SDL_GlobFlags flags, out int count);

        public unsafe static IntPtr SDL_GlobStorageDirectory(IntPtr storage, string path, string pattern, SDL_GlobFlags flags, out int count)
        {
            byte* ptr = EncodeAsUTF8(path);
            byte* ptr2 = EncodeAsUTF8(pattern);
            IntPtr result = INTERNAL_SDL_GlobStorageDirectory(storage, ptr, ptr2, flags, out count);
            SDL_free((IntPtr)ptr);
            SDL_free((IntPtr)ptr2);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsTablet();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_IsTV();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Sandbox SDL_GetSandbox();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_OnApplicationWillTerminate();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_OnApplicationDidReceiveMemoryWarning();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_OnApplicationWillEnterBackground();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_OnApplicationDidEnterBackground();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_OnApplicationWillEnterForeground();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_OnApplicationDidEnterForeground();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetDateTimeLocalePreferences(out SDL_DateFormat dateFormat, out SDL_TimeFormat timeFormat);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetCurrentTime(IntPtr ticks);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_TimeToDateTime(long ticks, out SDL_DateTime dt, SDLBool localTime);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_DateTimeToTime(ref SDL_DateTime dt, IntPtr ticks);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_TimeToWindows(long ticks, out uint dwLowDateTime, out uint dwHighDateTime);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_TimeFromWindows(uint dwLowDateTime, uint dwHighDateTime);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDaysInMonth(int year, int month);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDayOfYear(int year, int month, int day);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDayOfWeek(int year, int month, int day);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetTicks();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetTicksNS();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetPerformanceCounter();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetPerformanceFrequency();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Delay(uint ms);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DelayNS(ulong ns);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DelayPrecise(ulong ns);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_AddTimer(uint interval, SDL_TimerCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_AddTimerNS(ulong interval, SDL_NSTimerCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_RemoveTimer(uint id);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateTray")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateTray(IntPtr icon, byte* tooltip);

        public unsafe static IntPtr SDL_CreateTray(IntPtr icon, string tooltip)
        {
            byte* ptr = EncodeAsUTF8(tooltip);
            IntPtr result = INTERNAL_SDL_CreateTray(icon, ptr);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTrayIcon(IntPtr tray, IntPtr icon);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetTrayTooltip")]
        private unsafe static extern void INTERNAL_SDL_SetTrayTooltip(IntPtr tray, byte* tooltip);

        public unsafe static void SDL_SetTrayTooltip(IntPtr tray, string tooltip)
        {
            byte* ptr = EncodeAsUTF8(tooltip);
            INTERNAL_SDL_SetTrayTooltip(tray, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTrayMenu(IntPtr tray);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTraySubmenu(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTrayMenu(IntPtr tray);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTraySubmenu(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTrayEntries(IntPtr menu, out int count);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RemoveTrayEntry(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_InsertTrayEntryAt")]
        private unsafe static extern IntPtr INTERNAL_SDL_InsertTrayEntryAt(IntPtr menu, int pos, byte* label, SDL_TrayEntryFlags flags);

        public unsafe static IntPtr SDL_InsertTrayEntryAt(IntPtr menu, int pos, string label, SDL_TrayEntryFlags flags)
        {
            byte* ptr = EncodeAsUTF8(label);
            IntPtr result = INTERNAL_SDL_InsertTrayEntryAt(menu, pos, ptr, flags);
            SDL_free((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetTrayEntryLabel")]
        private unsafe static extern void INTERNAL_SDL_SetTrayEntryLabel(IntPtr entry, byte* label);

        public unsafe static void SDL_SetTrayEntryLabel(IntPtr entry, string label)
        {
            byte* ptr = EncodeAsUTF8(label);
            INTERNAL_SDL_SetTrayEntryLabel(entry, ptr);
            SDL_free((IntPtr)ptr);
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetTrayEntryLabel")]
        private static extern IntPtr INTERNAL_SDL_GetTrayEntryLabel(IntPtr entry);

        public static string SDL_GetTrayEntryLabel(IntPtr entry)
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetTrayEntryLabel(entry));
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTrayEntryChecked(IntPtr entry, SDLBool check);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTrayEntryChecked(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTrayEntryEnabled(IntPtr entry, SDLBool enabled);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDLBool SDL_GetTrayEntryEnabled(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTrayEntryCallback(IntPtr entry, SDL_TrayCallback callback, IntPtr userdata);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClickTrayEntry(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyTray(IntPtr tray);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTrayEntryParent(IntPtr entry);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTrayMenuParentEntry(IntPtr menu);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTrayMenuParentTray(IntPtr menu);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UpdateTrays();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetVersion();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetRevision")]
        private static extern IntPtr INTERNAL_SDL_GetRevision();

        public static string SDL_GetRevision()
        {
            return DecodeFromUTF8(INTERNAL_SDL_GetRevision());
        }

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetMainReady();

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RunApp(int argc, IntPtr argv, SDL_main_func mainFunction, IntPtr reserved);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_EnterAppMainCallbacks(int argc, IntPtr argv, SDL_AppInit_func appinit, SDL_AppIterate_func appiter, SDL_AppEvent_func appevent, SDL_AppQuit_func appquit);

        [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GDKSuspendComplete();
    }
    /// <summary>
    /// This was taken from the FNA.dll. its license i think allows me to just copy there code to interact with SDL.
    /// <para>https://opensource.org/license/ms-pl-html</para>
    /// <para>https://github.com/FNA-XNA/FNA/blob/master/licenses/LICENSE</para>
    /// </summary>
    public static class FNA_SDL2
    {
        public enum SDL_bool
        {
            SDL_FALSE,
            SDL_TRUE
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSizeCallback(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSeekCallback(IntPtr context, long offset, int whence);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDLRWopsReadCallback(IntPtr context, IntPtr ptr, IntPtr size, IntPtr maxnum);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDLRWopsWriteCallback(IntPtr context, IntPtr ptr, IntPtr size, IntPtr num);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDLRWopsCloseCallback(IntPtr context);

        public struct SDL_RWops
        {
            public IntPtr size;

            public IntPtr seek;

            public IntPtr read;

            public IntPtr write;

            public IntPtr close;

            public uint type;
        }

        public delegate int SDL_main_func(int argc, IntPtr argv);

        public enum SDL_HintPriority
        {
            SDL_HINT_DEFAULT,
            SDL_HINT_NORMAL,
            SDL_HINT_OVERRIDE
        }

        public enum SDL_LogCategory
        {
            SDL_LOG_CATEGORY_APPLICATION,
            SDL_LOG_CATEGORY_ERROR,
            SDL_LOG_CATEGORY_ASSERT,
            SDL_LOG_CATEGORY_SYSTEM,
            SDL_LOG_CATEGORY_AUDIO,
            SDL_LOG_CATEGORY_VIDEO,
            SDL_LOG_CATEGORY_RENDER,
            SDL_LOG_CATEGORY_INPUT,
            SDL_LOG_CATEGORY_TEST,
            SDL_LOG_CATEGORY_RESERVED1,
            SDL_LOG_CATEGORY_RESERVED2,
            SDL_LOG_CATEGORY_RESERVED3,
            SDL_LOG_CATEGORY_RESERVED4,
            SDL_LOG_CATEGORY_RESERVED5,
            SDL_LOG_CATEGORY_RESERVED6,
            SDL_LOG_CATEGORY_RESERVED7,
            SDL_LOG_CATEGORY_RESERVED8,
            SDL_LOG_CATEGORY_RESERVED9,
            SDL_LOG_CATEGORY_RESERVED10,
            SDL_LOG_CATEGORY_CUSTOM
        }

        public enum SDL_LogPriority
        {
            SDL_LOG_PRIORITY_VERBOSE = 1,
            SDL_LOG_PRIORITY_DEBUG,
            SDL_LOG_PRIORITY_INFO,
            SDL_LOG_PRIORITY_WARN,
            SDL_LOG_PRIORITY_ERROR,
            SDL_LOG_PRIORITY_CRITICAL,
            SDL_NUM_LOG_PRIORITIES
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_LogOutputFunction(IntPtr userdata, int category, SDL_LogPriority priority, IntPtr message);

        [Flags]
        public enum SDL_MessageBoxFlags : uint
        {
            SDL_MESSAGEBOX_ERROR = 0x10u,
            SDL_MESSAGEBOX_WARNING = 0x20u,
            SDL_MESSAGEBOX_INFORMATION = 0x40u
        }

        [Flags]
        public enum SDL_MessageBoxButtonFlags : uint
        {
            SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT = 1u,
            SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT = 2u
        }

        private struct INTERNAL_SDL_MessageBoxButtonData
        {
            public SDL_MessageBoxButtonFlags flags;

            public int buttonid;

            public IntPtr text;
        }

        public struct SDL_MessageBoxButtonData
        {
            public SDL_MessageBoxButtonFlags flags;

            public int buttonid;

            public string text;
        }

        public struct SDL_MessageBoxColor
        {
            public byte r;

            public byte g;

            public byte b;
        }

        public enum SDL_MessageBoxColorType
        {
            SDL_MESSAGEBOX_COLOR_BACKGROUND,
            SDL_MESSAGEBOX_COLOR_TEXT,
            SDL_MESSAGEBOX_COLOR_BUTTON_BORDER,
            SDL_MESSAGEBOX_COLOR_BUTTON_BACKGROUND,
            SDL_MESSAGEBOX_COLOR_BUTTON_SELECTED,
            SDL_MESSAGEBOX_COLOR_MAX
        }

        public struct SDL_MessageBoxColorScheme
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
            public SDL_MessageBoxColor[] colors;
        }

        private struct INTERNAL_SDL_MessageBoxData
        {
            public SDL_MessageBoxFlags flags;

            public IntPtr window;

            public IntPtr title;

            public IntPtr message;

            public int numbuttons;

            public IntPtr buttons;

            public IntPtr colorScheme;
        }

        public struct SDL_MessageBoxData
        {
            public SDL_MessageBoxFlags flags;

            public IntPtr window;

            public string title;

            public string message;

            public int numbuttons;

            public SDL_MessageBoxButtonData[] buttons;

            public SDL_MessageBoxColorScheme? colorScheme;
        }

        public struct SDL_version
        {
            public byte major;

            public byte minor;

            public byte patch;
        }

        public enum SDL_GLattr
        {
            SDL_GL_RED_SIZE,
            SDL_GL_GREEN_SIZE,
            SDL_GL_BLUE_SIZE,
            SDL_GL_ALPHA_SIZE,
            SDL_GL_BUFFER_SIZE,
            SDL_GL_DOUBLEBUFFER,
            SDL_GL_DEPTH_SIZE,
            SDL_GL_STENCIL_SIZE,
            SDL_GL_ACCUM_RED_SIZE,
            SDL_GL_ACCUM_GREEN_SIZE,
            SDL_GL_ACCUM_BLUE_SIZE,
            SDL_GL_ACCUM_ALPHA_SIZE,
            SDL_GL_STEREO,
            SDL_GL_MULTISAMPLEBUFFERS,
            SDL_GL_MULTISAMPLESAMPLES,
            SDL_GL_ACCELERATED_VISUAL,
            SDL_GL_RETAINED_BACKING,
            SDL_GL_CONTEXT_MAJOR_VERSION,
            SDL_GL_CONTEXT_MINOR_VERSION,
            SDL_GL_CONTEXT_EGL,
            SDL_GL_CONTEXT_FLAGS,
            SDL_GL_CONTEXT_PROFILE_MASK,
            SDL_GL_SHARE_WITH_CURRENT_CONTEXT,
            SDL_GL_FRAMEBUFFER_SRGB_CAPABLE,
            SDL_GL_CONTEXT_RELEASE_BEHAVIOR,
            SDL_GL_CONTEXT_RESET_NOTIFICATION,
            SDL_GL_CONTEXT_NO_ERROR
        }

        [Flags]
        public enum SDL_GLprofile
        {
            SDL_GL_CONTEXT_PROFILE_CORE = 1,
            SDL_GL_CONTEXT_PROFILE_COMPATIBILITY = 2,
            SDL_GL_CONTEXT_PROFILE_ES = 4
        }

        [Flags]
        public enum SDL_GLcontext
        {
            SDL_GL_CONTEXT_DEBUG_FLAG = 1,
            SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG = 2,
            SDL_GL_CONTEXT_ROBUST_ACCESS_FLAG = 4,
            SDL_GL_CONTEXT_RESET_ISOLATION_FLAG = 8
        }

        public enum SDL_WindowEventID : byte
        {
            SDL_WINDOWEVENT_NONE,
            SDL_WINDOWEVENT_SHOWN,
            SDL_WINDOWEVENT_HIDDEN,
            SDL_WINDOWEVENT_EXPOSED,
            SDL_WINDOWEVENT_MOVED,
            SDL_WINDOWEVENT_RESIZED,
            SDL_WINDOWEVENT_SIZE_CHANGED,
            SDL_WINDOWEVENT_MINIMIZED,
            SDL_WINDOWEVENT_MAXIMIZED,
            SDL_WINDOWEVENT_RESTORED,
            SDL_WINDOWEVENT_ENTER,
            SDL_WINDOWEVENT_LEAVE,
            SDL_WINDOWEVENT_FOCUS_GAINED,
            SDL_WINDOWEVENT_FOCUS_LOST,
            SDL_WINDOWEVENT_CLOSE,
            SDL_WINDOWEVENT_TAKE_FOCUS,
            SDL_WINDOWEVENT_HIT_TEST,
            SDL_WINDOWEVENT_ICCPROF_CHANGED,
            SDL_WINDOWEVENT_DISPLAY_CHANGED
        }

        public enum SDL_DisplayEventID : byte
        {
            SDL_DISPLAYEVENT_NONE,
            SDL_DISPLAYEVENT_ORIENTATION,
            SDL_DISPLAYEVENT_CONNECTED,
            SDL_DISPLAYEVENT_DISCONNECTED
        }

        public enum SDL_DisplayOrientation
        {
            SDL_ORIENTATION_UNKNOWN,
            SDL_ORIENTATION_LANDSCAPE,
            SDL_ORIENTATION_LANDSCAPE_FLIPPED,
            SDL_ORIENTATION_PORTRAIT,
            SDL_ORIENTATION_PORTRAIT_FLIPPED
        }

        public enum SDL_FlashOperation
        {
            SDL_FLASH_CANCEL,
            SDL_FLASH_BRIEFLY,
            SDL_FLASH_UNTIL_FOCUSED
        }

        [Flags]
        public enum SDL_WindowFlags : uint
        {
            SDL_WINDOW_FULLSCREEN = 1u,
            SDL_WINDOW_OPENGL = 2u,
            SDL_WINDOW_SHOWN = 4u,
            SDL_WINDOW_HIDDEN = 8u,
            SDL_WINDOW_BORDERLESS = 0x10u,
            SDL_WINDOW_RESIZABLE = 0x20u,
            SDL_WINDOW_MINIMIZED = 0x40u,
            SDL_WINDOW_MAXIMIZED = 0x80u,
            SDL_WINDOW_MOUSE_GRABBED = 0x100u,
            SDL_WINDOW_INPUT_FOCUS = 0x200u,
            SDL_WINDOW_MOUSE_FOCUS = 0x400u,
            SDL_WINDOW_FULLSCREEN_DESKTOP = 0x1001u,
            SDL_WINDOW_FOREIGN = 0x800u,
            SDL_WINDOW_ALLOW_HIGHDPI = 0x2000u,
            SDL_WINDOW_MOUSE_CAPTURE = 0x4000u,
            SDL_WINDOW_ALWAYS_ON_TOP = 0x8000u,
            SDL_WINDOW_SKIP_TASKBAR = 0x10000u,
            SDL_WINDOW_UTILITY = 0x20000u,
            SDL_WINDOW_TOOLTIP = 0x40000u,
            SDL_WINDOW_POPUP_MENU = 0x80000u,
            SDL_WINDOW_KEYBOARD_GRABBED = 0x100000u,
            SDL_WINDOW_VULKAN = 0x10000000u,
            SDL_WINDOW_METAL = 0x2000000u,
            SDL_WINDOW_INPUT_GRABBED = 0x100u
        }

        public enum SDL_HitTestResult
        {
            SDL_HITTEST_NORMAL,
            SDL_HITTEST_DRAGGABLE,
            SDL_HITTEST_RESIZE_TOPLEFT,
            SDL_HITTEST_RESIZE_TOP,
            SDL_HITTEST_RESIZE_TOPRIGHT,
            SDL_HITTEST_RESIZE_RIGHT,
            SDL_HITTEST_RESIZE_BOTTOMRIGHT,
            SDL_HITTEST_RESIZE_BOTTOM,
            SDL_HITTEST_RESIZE_BOTTOMLEFT,
            SDL_HITTEST_RESIZE_LEFT
        }

        public struct SDL_DisplayMode
        {
            public uint format;

            public int w;

            public int h;

            public int refresh_rate;

            public IntPtr driverdata;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate SDL_HitTestResult SDL_HitTest(IntPtr win, IntPtr area, IntPtr data);

        [Flags]
        public enum SDL_BlendMode
        {
            SDL_BLENDMODE_NONE = 0,
            SDL_BLENDMODE_BLEND = 1,
            SDL_BLENDMODE_ADD = 2,
            SDL_BLENDMODE_MOD = 4,
            SDL_BLENDMODE_MUL = 8,
            SDL_BLENDMODE_INVALID = int.MaxValue
        }

        public enum SDL_BlendOperation
        {
            SDL_BLENDOPERATION_ADD = 1,
            SDL_BLENDOPERATION_SUBTRACT,
            SDL_BLENDOPERATION_REV_SUBTRACT,
            SDL_BLENDOPERATION_MINIMUM,
            SDL_BLENDOPERATION_MAXIMUM
        }

        public enum SDL_BlendFactor
        {
            SDL_BLENDFACTOR_ZERO = 1,
            SDL_BLENDFACTOR_ONE,
            SDL_BLENDFACTOR_SRC_COLOR,
            SDL_BLENDFACTOR_ONE_MINUS_SRC_COLOR,
            SDL_BLENDFACTOR_SRC_ALPHA,
            SDL_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
            SDL_BLENDFACTOR_DST_COLOR,
            SDL_BLENDFACTOR_ONE_MINUS_DST_COLOR,
            SDL_BLENDFACTOR_DST_ALPHA,
            SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA
        }

        [Flags]
        public enum SDL_RendererFlags : uint
        {
            SDL_RENDERER_SOFTWARE = 1u,
            SDL_RENDERER_ACCELERATED = 2u,
            SDL_RENDERER_PRESENTVSYNC = 4u,
            SDL_RENDERER_TARGETTEXTURE = 8u
        }

        [Flags]
        public enum SDL_RendererFlip
        {
            SDL_FLIP_NONE = 0,
            SDL_FLIP_HORIZONTAL = 1,
            SDL_FLIP_VERTICAL = 2
        }

        public enum SDL_TextureAccess
        {
            SDL_TEXTUREACCESS_STATIC,
            SDL_TEXTUREACCESS_STREAMING,
            SDL_TEXTUREACCESS_TARGET
        }

        [Flags]
        public enum SDL_TextureModulate
        {
            SDL_TEXTUREMODULATE_NONE = 0,
            SDL_TEXTUREMODULATE_HORIZONTAL = 1,
            SDL_TEXTUREMODULATE_VERTICAL = 2
        }

        public struct SDL_RendererInfo
        {
            public IntPtr name;

            public uint flags;

            public uint num_texture_formats;

            public unsafe fixed uint texture_formats[16];

            public int max_texture_width;

            public int max_texture_height;
        }

        public enum SDL_ScaleMode
        {
            SDL_ScaleModeNearest,
            SDL_ScaleModeLinear,
            SDL_ScaleModeBest
        }

        public struct SDL_Vertex
        {
            public SDL_FPoint position;

            public SDL_Color color;

            public SDL_FPoint tex_coord;
        }

        public enum SDL_PixelType
        {
            SDL_PIXELTYPE_UNKNOWN,
            SDL_PIXELTYPE_INDEX1,
            SDL_PIXELTYPE_INDEX4,
            SDL_PIXELTYPE_INDEX8,
            SDL_PIXELTYPE_PACKED8,
            SDL_PIXELTYPE_PACKED16,
            SDL_PIXELTYPE_PACKED32,
            SDL_PIXELTYPE_ARRAYU8,
            SDL_PIXELTYPE_ARRAYU16,
            SDL_PIXELTYPE_ARRAYU32,
            SDL_PIXELTYPE_ARRAYF16,
            SDL_PIXELTYPE_ARRAYF32
        }

        public enum SDL_BitmapOrder
        {
            SDL_BITMAPORDER_NONE,
            SDL_BITMAPORDER_4321,
            SDL_BITMAPORDER_1234
        }

        public enum SDL_PackedOrder
        {
            SDL_PACKEDORDER_NONE,
            SDL_PACKEDORDER_XRGB,
            SDL_PACKEDORDER_RGBX,
            SDL_PACKEDORDER_ARGB,
            SDL_PACKEDORDER_RGBA,
            SDL_PACKEDORDER_XBGR,
            SDL_PACKEDORDER_BGRX,
            SDL_PACKEDORDER_ABGR,
            SDL_PACKEDORDER_BGRA
        }

        public enum SDL_ArrayOrder
        {
            SDL_ARRAYORDER_NONE,
            SDL_ARRAYORDER_RGB,
            SDL_ARRAYORDER_RGBA,
            SDL_ARRAYORDER_ARGB,
            SDL_ARRAYORDER_BGR,
            SDL_ARRAYORDER_BGRA,
            SDL_ARRAYORDER_ABGR
        }

        public enum SDL_PackedLayout
        {
            SDL_PACKEDLAYOUT_NONE,
            SDL_PACKEDLAYOUT_332,
            SDL_PACKEDLAYOUT_4444,
            SDL_PACKEDLAYOUT_1555,
            SDL_PACKEDLAYOUT_5551,
            SDL_PACKEDLAYOUT_565,
            SDL_PACKEDLAYOUT_8888,
            SDL_PACKEDLAYOUT_2101010,
            SDL_PACKEDLAYOUT_1010102
        }

        public struct SDL_Color
        {
            public byte r;

            public byte g;

            public byte b;

            public byte a;
        }

        public struct SDL_Palette
        {
            public int ncolors;

            public IntPtr colors;

            public int version;

            public int refcount;
        }

        public struct SDL_PixelFormat
        {
            public uint format;

            public IntPtr palette;

            public byte BitsPerPixel;

            public byte BytesPerPixel;

            public uint Rmask;

            public uint Gmask;

            public uint Bmask;

            public uint Amask;

            public byte Rloss;

            public byte Gloss;

            public byte Bloss;

            public byte Aloss;

            public byte Rshift;

            public byte Gshift;

            public byte Bshift;

            public byte Ashift;

            public int refcount;

            public IntPtr next;
        }

        public struct SDL_Point
        {
            public int x;

            public int y;
        }

        public struct SDL_Rect
        {
            public int x;

            public int y;

            public int w;

            public int h;
        }

        public struct SDL_FPoint
        {
            public float x;

            public float y;
        }

        public struct SDL_FRect
        {
            public float x;

            public float y;

            public float w;

            public float h;
        }

        public enum WindowShapeMode
        {
            ShapeModeDefault,
            ShapeModeBinarizeAlpha,
            ShapeModeReverseBinarizeAlpha,
            ShapeModeColorKey
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_WindowShapeParams
        {
            [FieldOffset(0)]
            public byte binarizationCutoff;

            [FieldOffset(0)]
            public SDL_Color colorKey;
        }

        public struct SDL_WindowShapeMode
        {
            public WindowShapeMode mode;

            public SDL_WindowShapeParams parameters;
        }

        public struct SDL_Surface
        {
            public uint flags;

            public IntPtr format;

            public int w;

            public int h;

            public int pitch;

            public IntPtr pixels;

            public IntPtr userdata;

            public int locked;

            public IntPtr list_blitmap;

            public SDL_Rect clip_rect;

            public IntPtr map;

            public int refcount;
        }

        public enum SDL_EventType : uint
        {
            SDL_FIRSTEVENT = 0u,
            SDL_QUIT = 256u,
            SDL_APP_TERMINATING = 257u,
            SDL_APP_LOWMEMORY = 258u,
            SDL_APP_WILLENTERBACKGROUND = 259u,
            SDL_APP_DIDENTERBACKGROUND = 260u,
            SDL_APP_WILLENTERFOREGROUND = 261u,
            SDL_APP_DIDENTERFOREGROUND = 262u,
            SDL_LOCALECHANGED = 263u,
            SDL_DISPLAYEVENT = 336u,
            SDL_WINDOWEVENT = 512u,
            SDL_SYSWMEVENT = 513u,
            SDL_KEYDOWN = 768u,
            SDL_KEYUP = 769u,
            SDL_TEXTEDITING = 770u,
            SDL_TEXTINPUT = 771u,
            SDL_KEYMAPCHANGED = 772u,
            SDL_TEXTEDITING_EXT = 773u,
            SDL_MOUSEMOTION = 1024u,
            SDL_MOUSEBUTTONDOWN = 1025u,
            SDL_MOUSEBUTTONUP = 1026u,
            SDL_MOUSEWHEEL = 1027u,
            SDL_JOYAXISMOTION = 1536u,
            SDL_JOYBALLMOTION = 1537u,
            SDL_JOYHATMOTION = 1538u,
            SDL_JOYBUTTONDOWN = 1539u,
            SDL_JOYBUTTONUP = 1540u,
            SDL_JOYDEVICEADDED = 1541u,
            SDL_JOYDEVICEREMOVED = 1542u,
            SDL_CONTROLLERAXISMOTION = 1616u,
            SDL_CONTROLLERBUTTONDOWN = 1617u,
            SDL_CONTROLLERBUTTONUP = 1618u,
            SDL_CONTROLLERDEVICEADDED = 1619u,
            SDL_CONTROLLERDEVICEREMOVED = 1620u,
            SDL_CONTROLLERDEVICEREMAPPED = 1621u,
            SDL_CONTROLLERTOUCHPADDOWN = 1622u,
            SDL_CONTROLLERTOUCHPADMOTION = 1623u,
            SDL_CONTROLLERTOUCHPADUP = 1624u,
            SDL_CONTROLLERSENSORUPDATE = 1625u,
            SDL_FINGERDOWN = 1792u,
            SDL_FINGERUP = 1793u,
            SDL_FINGERMOTION = 1794u,
            SDL_DOLLARGESTURE = 2048u,
            SDL_DOLLARRECORD = 2049u,
            SDL_MULTIGESTURE = 2050u,
            SDL_CLIPBOARDUPDATE = 2304u,
            SDL_DROPFILE = 4096u,
            SDL_DROPTEXT = 4097u,
            SDL_DROPBEGIN = 4098u,
            SDL_DROPCOMPLETE = 4099u,
            SDL_AUDIODEVICEADDED = 4352u,
            SDL_AUDIODEVICEREMOVED = 4353u,
            SDL_SENSORUPDATE = 4608u,
            SDL_RENDER_TARGETS_RESET = 8192u,
            SDL_RENDER_DEVICE_RESET = 8193u,
            SDL_POLLSENTINEL = 32512u,
            SDL_USEREVENT = 32768u,
            SDL_LASTEVENT = 65535u
        }

        public enum SDL_MouseWheelDirection : uint
        {
            SDL_MOUSEWHEEL_NORMAL,
            SDL_MOUSEWHEEL_FLIPPED
        }

        public struct SDL_GenericEvent
        {
            public SDL_EventType type;

            public uint timestamp;
        }

        public struct SDL_DisplayEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint display;

            public SDL_DisplayEventID displayEvent;

            private byte padding1;

            private byte padding2;

            private byte padding3;

            public int data1;
        }

        public struct SDL_WindowEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public SDL_WindowEventID windowEvent;

            private byte padding1;

            private byte padding2;

            private byte padding3;

            public int data1;

            public int data2;
        }

        public struct SDL_KeyboardEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public byte state;

            public byte repeat;

            private byte padding2;

            private byte padding3;

            public SDL_Keysym keysym;
        }

        public struct SDL_TextEditingEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public unsafe fixed byte text[32];

            public int start;

            public int length;
        }

        public struct SDL_TextEditingExtEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public IntPtr text;

            public int start;

            public int length;
        }

        public struct SDL_TextInputEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public unsafe fixed byte text[32];
        }

        public struct SDL_MouseMotionEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public uint which;

            public byte state;

            private byte padding1;

            private byte padding2;

            private byte padding3;

            public int x;

            public int y;

            public int xrel;

            public int yrel;
        }

        public struct SDL_MouseButtonEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public uint which;

            public byte button;

            public byte state;

            public byte clicks;

            private byte padding1;

            public int x;

            public int y;
        }

        public struct SDL_MouseWheelEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public uint which;

            public int x;

            public int y;

            public uint direction;

            public float preciseX;

            public float preciseY;
        }

        public struct SDL_JoyAxisEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public byte axis;

            private byte padding1;

            private byte padding2;

            private byte padding3;

            public short axisValue;

            public ushort padding4;
        }

        public struct SDL_JoyBallEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public byte ball;

            private byte padding1;

            private byte padding2;

            private byte padding3;

            public short xrel;

            public short yrel;
        }

        public struct SDL_JoyHatEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public byte hat;

            public byte hatValue;

            private byte padding1;

            private byte padding2;
        }

        public struct SDL_JoyButtonEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public byte button;

            public byte state;

            private byte padding1;

            private byte padding2;
        }

        public struct SDL_JoyDeviceEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;
        }

        public struct SDL_ControllerAxisEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public byte axis;

            private byte padding1;

            private byte padding2;

            private byte padding3;

            public short axisValue;

            private ushort padding4;
        }

        public struct SDL_ControllerButtonEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public byte button;

            public byte state;

            private byte padding1;

            private byte padding2;
        }

        public struct SDL_ControllerDeviceEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;
        }

        public struct SDL_ControllerTouchpadEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public int touchpad;

            public int finger;

            public float x;

            public float y;

            public float pressure;
        }

        public struct SDL_ControllerSensorEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public int sensor;

            public float data1;

            public float data2;

            public float data3;
        }

        public struct SDL_AudioDeviceEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint which;

            public byte iscapture;

            private byte padding1;

            private byte padding2;

            private byte padding3;
        }

        public struct SDL_TouchFingerEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public long touchId;

            public long fingerId;

            public float x;

            public float y;

            public float dx;

            public float dy;

            public float pressure;

            public uint windowID;
        }

        public struct SDL_MultiGestureEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public long touchId;

            public float dTheta;

            public float dDist;

            public float x;

            public float y;

            public ushort numFingers;

            public ushort padding;
        }

        public struct SDL_DollarGestureEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public long touchId;

            public long gestureId;

            public uint numFingers;

            public float error;

            public float x;

            public float y;
        }

        public struct SDL_DropEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public IntPtr file;

            public uint windowID;
        }

        public struct SDL_SensorEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public int which;

            public unsafe fixed float data[6];
        }

        public struct SDL_QuitEvent
        {
            public SDL_EventType type;

            public uint timestamp;
        }

        public struct SDL_UserEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public uint windowID;

            public int code;

            public IntPtr data1;

            public IntPtr data2;
        }

        public struct SDL_SysWMEvent
        {
            public SDL_EventType type;

            public uint timestamp;

            public IntPtr msg;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_Event
        {
            [FieldOffset(0)]
            public SDL_EventType type;

            [FieldOffset(0)]
            public SDL_EventType typeFSharp;

            [FieldOffset(0)]
            public SDL_DisplayEvent display;

            [FieldOffset(0)]
            public SDL_WindowEvent window;

            [FieldOffset(0)]
            public SDL_KeyboardEvent key;

            [FieldOffset(0)]
            public SDL_TextEditingEvent edit;

            [FieldOffset(0)]
            public SDL_TextEditingExtEvent editExt;

            [FieldOffset(0)]
            public SDL_TextInputEvent text;

            [FieldOffset(0)]
            public SDL_MouseMotionEvent motion;

            [FieldOffset(0)]
            public SDL_MouseButtonEvent button;

            [FieldOffset(0)]
            public SDL_MouseWheelEvent wheel;

            [FieldOffset(0)]
            public SDL_JoyAxisEvent jaxis;

            [FieldOffset(0)]
            public SDL_JoyBallEvent jball;

            [FieldOffset(0)]
            public SDL_JoyHatEvent jhat;

            [FieldOffset(0)]
            public SDL_JoyButtonEvent jbutton;

            [FieldOffset(0)]
            public SDL_JoyDeviceEvent jdevice;

            [FieldOffset(0)]
            public SDL_ControllerAxisEvent caxis;

            [FieldOffset(0)]
            public SDL_ControllerButtonEvent cbutton;

            [FieldOffset(0)]
            public SDL_ControllerDeviceEvent cdevice;

            [FieldOffset(0)]
            public SDL_ControllerTouchpadEvent ctouchpad;

            [FieldOffset(0)]
            public SDL_ControllerSensorEvent csensor;

            [FieldOffset(0)]
            public SDL_AudioDeviceEvent adevice;

            [FieldOffset(0)]
            public SDL_SensorEvent sensor;

            [FieldOffset(0)]
            public SDL_QuitEvent quit;

            [FieldOffset(0)]
            public SDL_UserEvent user;

            [FieldOffset(0)]
            public SDL_SysWMEvent syswm;

            [FieldOffset(0)]
            public SDL_TouchFingerEvent tfinger;

            [FieldOffset(0)]
            public SDL_MultiGestureEvent mgesture;

            [FieldOffset(0)]
            public SDL_DollarGestureEvent dgesture;

            [FieldOffset(0)]
            public SDL_DropEvent drop;

            [FieldOffset(0)]
            private unsafe fixed byte padding[56];
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDL_EventFilter(IntPtr userdata, IntPtr sdlevent);

        public enum SDL_eventaction
        {
            SDL_ADDEVENT,
            SDL_PEEKEVENT,
            SDL_GETEVENT
        }

        public enum SDL_Scancode
        {
            SDL_SCANCODE_UNKNOWN = 0,
            SDL_SCANCODE_A = 4,
            SDL_SCANCODE_B = 5,
            SDL_SCANCODE_C = 6,
            SDL_SCANCODE_D = 7,
            SDL_SCANCODE_E = 8,
            SDL_SCANCODE_F = 9,
            SDL_SCANCODE_G = 10,
            SDL_SCANCODE_H = 11,
            SDL_SCANCODE_I = 12,
            SDL_SCANCODE_J = 13,
            SDL_SCANCODE_K = 14,
            SDL_SCANCODE_L = 15,
            SDL_SCANCODE_M = 16,
            SDL_SCANCODE_N = 17,
            SDL_SCANCODE_O = 18,
            SDL_SCANCODE_P = 19,
            SDL_SCANCODE_Q = 20,
            SDL_SCANCODE_R = 21,
            SDL_SCANCODE_S = 22,
            SDL_SCANCODE_T = 23,
            SDL_SCANCODE_U = 24,
            SDL_SCANCODE_V = 25,
            SDL_SCANCODE_W = 26,
            SDL_SCANCODE_X = 27,
            SDL_SCANCODE_Y = 28,
            SDL_SCANCODE_Z = 29,
            SDL_SCANCODE_1 = 30,
            SDL_SCANCODE_2 = 31,
            SDL_SCANCODE_3 = 32,
            SDL_SCANCODE_4 = 33,
            SDL_SCANCODE_5 = 34,
            SDL_SCANCODE_6 = 35,
            SDL_SCANCODE_7 = 36,
            SDL_SCANCODE_8 = 37,
            SDL_SCANCODE_9 = 38,
            SDL_SCANCODE_0 = 39,
            SDL_SCANCODE_RETURN = 40,
            SDL_SCANCODE_ESCAPE = 41,
            SDL_SCANCODE_BACKSPACE = 42,
            SDL_SCANCODE_TAB = 43,
            SDL_SCANCODE_SPACE = 44,
            SDL_SCANCODE_MINUS = 45,
            SDL_SCANCODE_EQUALS = 46,
            SDL_SCANCODE_LEFTBRACKET = 47,
            SDL_SCANCODE_RIGHTBRACKET = 48,
            SDL_SCANCODE_BACKSLASH = 49,
            SDL_SCANCODE_NONUSHASH = 50,
            SDL_SCANCODE_SEMICOLON = 51,
            SDL_SCANCODE_APOSTROPHE = 52,
            SDL_SCANCODE_GRAVE = 53,
            SDL_SCANCODE_COMMA = 54,
            SDL_SCANCODE_PERIOD = 55,
            SDL_SCANCODE_SLASH = 56,
            SDL_SCANCODE_CAPSLOCK = 57,
            SDL_SCANCODE_F1 = 58,
            SDL_SCANCODE_F2 = 59,
            SDL_SCANCODE_F3 = 60,
            SDL_SCANCODE_F4 = 61,
            SDL_SCANCODE_F5 = 62,
            SDL_SCANCODE_F6 = 63,
            SDL_SCANCODE_F7 = 64,
            SDL_SCANCODE_F8 = 65,
            SDL_SCANCODE_F9 = 66,
            SDL_SCANCODE_F10 = 67,
            SDL_SCANCODE_F11 = 68,
            SDL_SCANCODE_F12 = 69,
            SDL_SCANCODE_PRINTSCREEN = 70,
            SDL_SCANCODE_SCROLLLOCK = 71,
            SDL_SCANCODE_PAUSE = 72,
            SDL_SCANCODE_INSERT = 73,
            SDL_SCANCODE_HOME = 74,
            SDL_SCANCODE_PAGEUP = 75,
            SDL_SCANCODE_DELETE = 76,
            SDL_SCANCODE_END = 77,
            SDL_SCANCODE_PAGEDOWN = 78,
            SDL_SCANCODE_RIGHT = 79,
            SDL_SCANCODE_LEFT = 80,
            SDL_SCANCODE_DOWN = 81,
            SDL_SCANCODE_UP = 82,
            SDL_SCANCODE_NUMLOCKCLEAR = 83,
            SDL_SCANCODE_KP_DIVIDE = 84,
            SDL_SCANCODE_KP_MULTIPLY = 85,
            SDL_SCANCODE_KP_MINUS = 86,
            SDL_SCANCODE_KP_PLUS = 87,
            SDL_SCANCODE_KP_ENTER = 88,
            SDL_SCANCODE_KP_1 = 89,
            SDL_SCANCODE_KP_2 = 90,
            SDL_SCANCODE_KP_3 = 91,
            SDL_SCANCODE_KP_4 = 92,
            SDL_SCANCODE_KP_5 = 93,
            SDL_SCANCODE_KP_6 = 94,
            SDL_SCANCODE_KP_7 = 95,
            SDL_SCANCODE_KP_8 = 96,
            SDL_SCANCODE_KP_9 = 97,
            SDL_SCANCODE_KP_0 = 98,
            SDL_SCANCODE_KP_PERIOD = 99,
            SDL_SCANCODE_NONUSBACKSLASH = 100,
            SDL_SCANCODE_APPLICATION = 101,
            SDL_SCANCODE_POWER = 102,
            SDL_SCANCODE_KP_EQUALS = 103,
            SDL_SCANCODE_F13 = 104,
            SDL_SCANCODE_F14 = 105,
            SDL_SCANCODE_F15 = 106,
            SDL_SCANCODE_F16 = 107,
            SDL_SCANCODE_F17 = 108,
            SDL_SCANCODE_F18 = 109,
            SDL_SCANCODE_F19 = 110,
            SDL_SCANCODE_F20 = 111,
            SDL_SCANCODE_F21 = 112,
            SDL_SCANCODE_F22 = 113,
            SDL_SCANCODE_F23 = 114,
            SDL_SCANCODE_F24 = 115,
            SDL_SCANCODE_EXECUTE = 116,
            SDL_SCANCODE_HELP = 117,
            SDL_SCANCODE_MENU = 118,
            SDL_SCANCODE_SELECT = 119,
            SDL_SCANCODE_STOP = 120,
            SDL_SCANCODE_AGAIN = 121,
            SDL_SCANCODE_UNDO = 122,
            SDL_SCANCODE_CUT = 123,
            SDL_SCANCODE_COPY = 124,
            SDL_SCANCODE_PASTE = 125,
            SDL_SCANCODE_FIND = 126,
            SDL_SCANCODE_MUTE = 127,
            SDL_SCANCODE_VOLUMEUP = 128,
            SDL_SCANCODE_VOLUMEDOWN = 129,
            SDL_SCANCODE_KP_COMMA = 133,
            SDL_SCANCODE_KP_EQUALSAS400 = 134,
            SDL_SCANCODE_INTERNATIONAL1 = 135,
            SDL_SCANCODE_INTERNATIONAL2 = 136,
            SDL_SCANCODE_INTERNATIONAL3 = 137,
            SDL_SCANCODE_INTERNATIONAL4 = 138,
            SDL_SCANCODE_INTERNATIONAL5 = 139,
            SDL_SCANCODE_INTERNATIONAL6 = 140,
            SDL_SCANCODE_INTERNATIONAL7 = 141,
            SDL_SCANCODE_INTERNATIONAL8 = 142,
            SDL_SCANCODE_INTERNATIONAL9 = 143,
            SDL_SCANCODE_LANG1 = 144,
            SDL_SCANCODE_LANG2 = 145,
            SDL_SCANCODE_LANG3 = 146,
            SDL_SCANCODE_LANG4 = 147,
            SDL_SCANCODE_LANG5 = 148,
            SDL_SCANCODE_LANG6 = 149,
            SDL_SCANCODE_LANG7 = 150,
            SDL_SCANCODE_LANG8 = 151,
            SDL_SCANCODE_LANG9 = 152,
            SDL_SCANCODE_ALTERASE = 153,
            SDL_SCANCODE_SYSREQ = 154,
            SDL_SCANCODE_CANCEL = 155,
            SDL_SCANCODE_CLEAR = 156,
            SDL_SCANCODE_PRIOR = 157,
            SDL_SCANCODE_RETURN2 = 158,
            SDL_SCANCODE_SEPARATOR = 159,
            SDL_SCANCODE_OUT = 160,
            SDL_SCANCODE_OPER = 161,
            SDL_SCANCODE_CLEARAGAIN = 162,
            SDL_SCANCODE_CRSEL = 163,
            SDL_SCANCODE_EXSEL = 164,
            SDL_SCANCODE_KP_00 = 176,
            SDL_SCANCODE_KP_000 = 177,
            SDL_SCANCODE_THOUSANDSSEPARATOR = 178,
            SDL_SCANCODE_DECIMALSEPARATOR = 179,
            SDL_SCANCODE_CURRENCYUNIT = 180,
            SDL_SCANCODE_CURRENCYSUBUNIT = 181,
            SDL_SCANCODE_KP_LEFTPAREN = 182,
            SDL_SCANCODE_KP_RIGHTPAREN = 183,
            SDL_SCANCODE_KP_LEFTBRACE = 184,
            SDL_SCANCODE_KP_RIGHTBRACE = 185,
            SDL_SCANCODE_KP_TAB = 186,
            SDL_SCANCODE_KP_BACKSPACE = 187,
            SDL_SCANCODE_KP_A = 188,
            SDL_SCANCODE_KP_B = 189,
            SDL_SCANCODE_KP_C = 190,
            SDL_SCANCODE_KP_D = 191,
            SDL_SCANCODE_KP_E = 192,
            SDL_SCANCODE_KP_F = 193,
            SDL_SCANCODE_KP_XOR = 194,
            SDL_SCANCODE_KP_POWER = 195,
            SDL_SCANCODE_KP_PERCENT = 196,
            SDL_SCANCODE_KP_LESS = 197,
            SDL_SCANCODE_KP_GREATER = 198,
            SDL_SCANCODE_KP_AMPERSAND = 199,
            SDL_SCANCODE_KP_DBLAMPERSAND = 200,
            SDL_SCANCODE_KP_VERTICALBAR = 201,
            SDL_SCANCODE_KP_DBLVERTICALBAR = 202,
            SDL_SCANCODE_KP_COLON = 203,
            SDL_SCANCODE_KP_HASH = 204,
            SDL_SCANCODE_KP_SPACE = 205,
            SDL_SCANCODE_KP_AT = 206,
            SDL_SCANCODE_KP_EXCLAM = 207,
            SDL_SCANCODE_KP_MEMSTORE = 208,
            SDL_SCANCODE_KP_MEMRECALL = 209,
            SDL_SCANCODE_KP_MEMCLEAR = 210,
            SDL_SCANCODE_KP_MEMADD = 211,
            SDL_SCANCODE_KP_MEMSUBTRACT = 212,
            SDL_SCANCODE_KP_MEMMULTIPLY = 213,
            SDL_SCANCODE_KP_MEMDIVIDE = 214,
            SDL_SCANCODE_KP_PLUSMINUS = 215,
            SDL_SCANCODE_KP_CLEAR = 216,
            SDL_SCANCODE_KP_CLEARENTRY = 217,
            SDL_SCANCODE_KP_BINARY = 218,
            SDL_SCANCODE_KP_OCTAL = 219,
            SDL_SCANCODE_KP_DECIMAL = 220,
            SDL_SCANCODE_KP_HEXADECIMAL = 221,
            SDL_SCANCODE_LCTRL = 224,
            SDL_SCANCODE_LSHIFT = 225,
            SDL_SCANCODE_LALT = 226,
            SDL_SCANCODE_LGUI = 227,
            SDL_SCANCODE_RCTRL = 228,
            SDL_SCANCODE_RSHIFT = 229,
            SDL_SCANCODE_RALT = 230,
            SDL_SCANCODE_RGUI = 231,
            SDL_SCANCODE_MODE = 257,
            SDL_SCANCODE_AUDIONEXT = 258,
            SDL_SCANCODE_AUDIOPREV = 259,
            SDL_SCANCODE_AUDIOSTOP = 260,
            SDL_SCANCODE_AUDIOPLAY = 261,
            SDL_SCANCODE_AUDIOMUTE = 262,
            SDL_SCANCODE_MEDIASELECT = 263,
            SDL_SCANCODE_WWW = 264,
            SDL_SCANCODE_MAIL = 265,
            SDL_SCANCODE_CALCULATOR = 266,
            SDL_SCANCODE_COMPUTER = 267,
            SDL_SCANCODE_AC_SEARCH = 268,
            SDL_SCANCODE_AC_HOME = 269,
            SDL_SCANCODE_AC_BACK = 270,
            SDL_SCANCODE_AC_FORWARD = 271,
            SDL_SCANCODE_AC_STOP = 272,
            SDL_SCANCODE_AC_REFRESH = 273,
            SDL_SCANCODE_AC_BOOKMARKS = 274,
            SDL_SCANCODE_BRIGHTNESSDOWN = 275,
            SDL_SCANCODE_BRIGHTNESSUP = 276,
            SDL_SCANCODE_DISPLAYSWITCH = 277,
            SDL_SCANCODE_KBDILLUMTOGGLE = 278,
            SDL_SCANCODE_KBDILLUMDOWN = 279,
            SDL_SCANCODE_KBDILLUMUP = 280,
            SDL_SCANCODE_EJECT = 281,
            SDL_SCANCODE_SLEEP = 282,
            SDL_SCANCODE_APP1 = 283,
            SDL_SCANCODE_APP2 = 284,
            SDL_SCANCODE_AUDIOREWIND = 285,
            SDL_SCANCODE_AUDIOFASTFORWARD = 286,
            SDL_NUM_SCANCODES = 512
        }

        public enum SDL_Keycode
        {
            SDLK_UNKNOWN = 0,
            SDLK_RETURN = 13,
            SDLK_ESCAPE = 27,
            SDLK_BACKSPACE = 8,
            SDLK_TAB = 9,
            SDLK_SPACE = 32,
            SDLK_EXCLAIM = 33,
            SDLK_QUOTEDBL = 34,
            SDLK_HASH = 35,
            SDLK_PERCENT = 37,
            SDLK_DOLLAR = 36,
            SDLK_AMPERSAND = 38,
            SDLK_QUOTE = 39,
            SDLK_LEFTPAREN = 40,
            SDLK_RIGHTPAREN = 41,
            SDLK_ASTERISK = 42,
            SDLK_PLUS = 43,
            SDLK_COMMA = 44,
            SDLK_MINUS = 45,
            SDLK_PERIOD = 46,
            SDLK_SLASH = 47,
            SDLK_0 = 48,
            SDLK_1 = 49,
            SDLK_2 = 50,
            SDLK_3 = 51,
            SDLK_4 = 52,
            SDLK_5 = 53,
            SDLK_6 = 54,
            SDLK_7 = 55,
            SDLK_8 = 56,
            SDLK_9 = 57,
            SDLK_COLON = 58,
            SDLK_SEMICOLON = 59,
            SDLK_LESS = 60,
            SDLK_EQUALS = 61,
            SDLK_GREATER = 62,
            SDLK_QUESTION = 63,
            SDLK_AT = 64,
            SDLK_LEFTBRACKET = 91,
            SDLK_BACKSLASH = 92,
            SDLK_RIGHTBRACKET = 93,
            SDLK_CARET = 94,
            SDLK_UNDERSCORE = 95,
            SDLK_BACKQUOTE = 96,
            SDLK_a = 97,
            SDLK_b = 98,
            SDLK_c = 99,
            SDLK_d = 100,
            SDLK_e = 101,
            SDLK_f = 102,
            SDLK_g = 103,
            SDLK_h = 104,
            SDLK_i = 105,
            SDLK_j = 106,
            SDLK_k = 107,
            SDLK_l = 108,
            SDLK_m = 109,
            SDLK_n = 110,
            SDLK_o = 111,
            SDLK_p = 112,
            SDLK_q = 113,
            SDLK_r = 114,
            SDLK_s = 115,
            SDLK_t = 116,
            SDLK_u = 117,
            SDLK_v = 118,
            SDLK_w = 119,
            SDLK_x = 120,
            SDLK_y = 121,
            SDLK_z = 122,
            SDLK_CAPSLOCK = 1073741881,
            SDLK_F1 = 1073741882,
            SDLK_F2 = 1073741883,
            SDLK_F3 = 1073741884,
            SDLK_F4 = 1073741885,
            SDLK_F5 = 1073741886,
            SDLK_F6 = 1073741887,
            SDLK_F7 = 1073741888,
            SDLK_F8 = 1073741889,
            SDLK_F9 = 1073741890,
            SDLK_F10 = 1073741891,
            SDLK_F11 = 1073741892,
            SDLK_F12 = 1073741893,
            SDLK_PRINTSCREEN = 1073741894,
            SDLK_SCROLLLOCK = 1073741895,
            SDLK_PAUSE = 1073741896,
            SDLK_INSERT = 1073741897,
            SDLK_HOME = 1073741898,
            SDLK_PAGEUP = 1073741899,
            SDLK_DELETE = 127,
            SDLK_END = 1073741901,
            SDLK_PAGEDOWN = 1073741902,
            SDLK_RIGHT = 1073741903,
            SDLK_LEFT = 1073741904,
            SDLK_DOWN = 1073741905,
            SDLK_UP = 1073741906,
            SDLK_NUMLOCKCLEAR = 1073741907,
            SDLK_KP_DIVIDE = 1073741908,
            SDLK_KP_MULTIPLY = 1073741909,
            SDLK_KP_MINUS = 1073741910,
            SDLK_KP_PLUS = 1073741911,
            SDLK_KP_ENTER = 1073741912,
            SDLK_KP_1 = 1073741913,
            SDLK_KP_2 = 1073741914,
            SDLK_KP_3 = 1073741915,
            SDLK_KP_4 = 1073741916,
            SDLK_KP_5 = 1073741917,
            SDLK_KP_6 = 1073741918,
            SDLK_KP_7 = 1073741919,
            SDLK_KP_8 = 1073741920,
            SDLK_KP_9 = 1073741921,
            SDLK_KP_0 = 1073741922,
            SDLK_KP_PERIOD = 1073741923,
            SDLK_APPLICATION = 1073741925,
            SDLK_POWER = 1073741926,
            SDLK_KP_EQUALS = 1073741927,
            SDLK_F13 = 1073741928,
            SDLK_F14 = 1073741929,
            SDLK_F15 = 1073741930,
            SDLK_F16 = 1073741931,
            SDLK_F17 = 1073741932,
            SDLK_F18 = 1073741933,
            SDLK_F19 = 1073741934,
            SDLK_F20 = 1073741935,
            SDLK_F21 = 1073741936,
            SDLK_F22 = 1073741937,
            SDLK_F23 = 1073741938,
            SDLK_F24 = 1073741939,
            SDLK_EXECUTE = 1073741940,
            SDLK_HELP = 1073741941,
            SDLK_MENU = 1073741942,
            SDLK_SELECT = 1073741943,
            SDLK_STOP = 1073741944,
            SDLK_AGAIN = 1073741945,
            SDLK_UNDO = 1073741946,
            SDLK_CUT = 1073741947,
            SDLK_COPY = 1073741948,
            SDLK_PASTE = 1073741949,
            SDLK_FIND = 1073741950,
            SDLK_MUTE = 1073741951,
            SDLK_VOLUMEUP = 1073741952,
            SDLK_VOLUMEDOWN = 1073741953,
            SDLK_KP_COMMA = 1073741957,
            SDLK_KP_EQUALSAS400 = 1073741958,
            SDLK_ALTERASE = 1073741977,
            SDLK_SYSREQ = 1073741978,
            SDLK_CANCEL = 1073741979,
            SDLK_CLEAR = 1073741980,
            SDLK_PRIOR = 1073741981,
            SDLK_RETURN2 = 1073741982,
            SDLK_SEPARATOR = 1073741983,
            SDLK_OUT = 1073741984,
            SDLK_OPER = 1073741985,
            SDLK_CLEARAGAIN = 1073741986,
            SDLK_CRSEL = 1073741987,
            SDLK_EXSEL = 1073741988,
            SDLK_KP_00 = 1073742000,
            SDLK_KP_000 = 1073742001,
            SDLK_THOUSANDSSEPARATOR = 1073742002,
            SDLK_DECIMALSEPARATOR = 1073742003,
            SDLK_CURRENCYUNIT = 1073742004,
            SDLK_CURRENCYSUBUNIT = 1073742005,
            SDLK_KP_LEFTPAREN = 1073742006,
            SDLK_KP_RIGHTPAREN = 1073742007,
            SDLK_KP_LEFTBRACE = 1073742008,
            SDLK_KP_RIGHTBRACE = 1073742009,
            SDLK_KP_TAB = 1073742010,
            SDLK_KP_BACKSPACE = 1073742011,
            SDLK_KP_A = 1073742012,
            SDLK_KP_B = 1073742013,
            SDLK_KP_C = 1073742014,
            SDLK_KP_D = 1073742015,
            SDLK_KP_E = 1073742016,
            SDLK_KP_F = 1073742017,
            SDLK_KP_XOR = 1073742018,
            SDLK_KP_POWER = 1073742019,
            SDLK_KP_PERCENT = 1073742020,
            SDLK_KP_LESS = 1073742021,
            SDLK_KP_GREATER = 1073742022,
            SDLK_KP_AMPERSAND = 1073742023,
            SDLK_KP_DBLAMPERSAND = 1073742024,
            SDLK_KP_VERTICALBAR = 1073742025,
            SDLK_KP_DBLVERTICALBAR = 1073742026,
            SDLK_KP_COLON = 1073742027,
            SDLK_KP_HASH = 1073742028,
            SDLK_KP_SPACE = 1073742029,
            SDLK_KP_AT = 1073742030,
            SDLK_KP_EXCLAM = 1073742031,
            SDLK_KP_MEMSTORE = 1073742032,
            SDLK_KP_MEMRECALL = 1073742033,
            SDLK_KP_MEMCLEAR = 1073742034,
            SDLK_KP_MEMADD = 1073742035,
            SDLK_KP_MEMSUBTRACT = 1073742036,
            SDLK_KP_MEMMULTIPLY = 1073742037,
            SDLK_KP_MEMDIVIDE = 1073742038,
            SDLK_KP_PLUSMINUS = 1073742039,
            SDLK_KP_CLEAR = 1073742040,
            SDLK_KP_CLEARENTRY = 1073742041,
            SDLK_KP_BINARY = 1073742042,
            SDLK_KP_OCTAL = 1073742043,
            SDLK_KP_DECIMAL = 1073742044,
            SDLK_KP_HEXADECIMAL = 1073742045,
            SDLK_LCTRL = 1073742048,
            SDLK_LSHIFT = 1073742049,
            SDLK_LALT = 1073742050,
            SDLK_LGUI = 1073742051,
            SDLK_RCTRL = 1073742052,
            SDLK_RSHIFT = 1073742053,
            SDLK_RALT = 1073742054,
            SDLK_RGUI = 1073742055,
            SDLK_MODE = 1073742081,
            SDLK_AUDIONEXT = 1073742082,
            SDLK_AUDIOPREV = 1073742083,
            SDLK_AUDIOSTOP = 1073742084,
            SDLK_AUDIOPLAY = 1073742085,
            SDLK_AUDIOMUTE = 1073742086,
            SDLK_MEDIASELECT = 1073742087,
            SDLK_WWW = 1073742088,
            SDLK_MAIL = 1073742089,
            SDLK_CALCULATOR = 1073742090,
            SDLK_COMPUTER = 1073742091,
            SDLK_AC_SEARCH = 1073742092,
            SDLK_AC_HOME = 1073742093,
            SDLK_AC_BACK = 1073742094,
            SDLK_AC_FORWARD = 1073742095,
            SDLK_AC_STOP = 1073742096,
            SDLK_AC_REFRESH = 1073742097,
            SDLK_AC_BOOKMARKS = 1073742098,
            SDLK_BRIGHTNESSDOWN = 1073742099,
            SDLK_BRIGHTNESSUP = 1073742100,
            SDLK_DISPLAYSWITCH = 1073742101,
            SDLK_KBDILLUMTOGGLE = 1073742102,
            SDLK_KBDILLUMDOWN = 1073742103,
            SDLK_KBDILLUMUP = 1073742104,
            SDLK_EJECT = 1073742105,
            SDLK_SLEEP = 1073742106,
            SDLK_APP1 = 1073742107,
            SDLK_APP2 = 1073742108,
            SDLK_AUDIOREWIND = 1073742109,
            SDLK_AUDIOFASTFORWARD = 1073742110
        }

        [Flags]
        public enum SDL_Keymod : ushort
        {
            KMOD_NONE = 0,
            KMOD_LSHIFT = 1,
            KMOD_RSHIFT = 2,
            KMOD_LCTRL = 0x40,
            KMOD_RCTRL = 0x80,
            KMOD_LALT = 0x100,
            KMOD_RALT = 0x200,
            KMOD_LGUI = 0x400,
            KMOD_RGUI = 0x800,
            KMOD_NUM = 0x1000,
            KMOD_CAPS = 0x2000,
            KMOD_MODE = 0x4000,
            KMOD_SCROLL = 0x8000,
            KMOD_CTRL = 0xC0,
            KMOD_SHIFT = 3,
            KMOD_ALT = 0x300,
            KMOD_GUI = 0xC00,
            KMOD_RESERVED = 0x8000
        }

        public struct SDL_Keysym
        {
            public SDL_Scancode scancode;

            public SDL_Keycode sym;

            public SDL_Keymod mod;

            public uint unicode;
        }

        public enum SDL_SystemCursor
        {
            SDL_SYSTEM_CURSOR_ARROW,
            SDL_SYSTEM_CURSOR_IBEAM,
            SDL_SYSTEM_CURSOR_WAIT,
            SDL_SYSTEM_CURSOR_CROSSHAIR,
            SDL_SYSTEM_CURSOR_WAITARROW,
            SDL_SYSTEM_CURSOR_SIZENWSE,
            SDL_SYSTEM_CURSOR_SIZENESW,
            SDL_SYSTEM_CURSOR_SIZEWE,
            SDL_SYSTEM_CURSOR_SIZENS,
            SDL_SYSTEM_CURSOR_SIZEALL,
            SDL_SYSTEM_CURSOR_NO,
            SDL_SYSTEM_CURSOR_HAND,
            SDL_NUM_SYSTEM_CURSORS
        }

        public struct SDL_Finger
        {
            public long id;

            public float x;

            public float y;

            public float pressure;
        }

        public enum SDL_TouchDeviceType
        {
            SDL_TOUCH_DEVICE_INVALID = -1,
            SDL_TOUCH_DEVICE_DIRECT,
            SDL_TOUCH_DEVICE_INDIRECT_ABSOLUTE,
            SDL_TOUCH_DEVICE_INDIRECT_RELATIVE
        }

        public enum SDL_JoystickPowerLevel
        {
            SDL_JOYSTICK_POWER_UNKNOWN = -1,
            SDL_JOYSTICK_POWER_EMPTY,
            SDL_JOYSTICK_POWER_LOW,
            SDL_JOYSTICK_POWER_MEDIUM,
            SDL_JOYSTICK_POWER_FULL,
            SDL_JOYSTICK_POWER_WIRED,
            SDL_JOYSTICK_POWER_MAX
        }

        public enum SDL_JoystickType
        {
            SDL_JOYSTICK_TYPE_UNKNOWN,
            SDL_JOYSTICK_TYPE_GAMECONTROLLER,
            SDL_JOYSTICK_TYPE_WHEEL,
            SDL_JOYSTICK_TYPE_ARCADE_STICK,
            SDL_JOYSTICK_TYPE_FLIGHT_STICK,
            SDL_JOYSTICK_TYPE_DANCE_PAD,
            SDL_JOYSTICK_TYPE_GUITAR,
            SDL_JOYSTICK_TYPE_DRUM_KIT,
            SDL_JOYSTICK_TYPE_ARCADE_PAD
        }

        public enum SDL_GameControllerBindType
        {
            SDL_CONTROLLER_BINDTYPE_NONE,
            SDL_CONTROLLER_BINDTYPE_BUTTON,
            SDL_CONTROLLER_BINDTYPE_AXIS,
            SDL_CONTROLLER_BINDTYPE_HAT
        }

        public enum SDL_GameControllerAxis
        {
            SDL_CONTROLLER_AXIS_INVALID = -1,
            SDL_CONTROLLER_AXIS_LEFTX,
            SDL_CONTROLLER_AXIS_LEFTY,
            SDL_CONTROLLER_AXIS_RIGHTX,
            SDL_CONTROLLER_AXIS_RIGHTY,
            SDL_CONTROLLER_AXIS_TRIGGERLEFT,
            SDL_CONTROLLER_AXIS_TRIGGERRIGHT,
            SDL_CONTROLLER_AXIS_MAX
        }

        public enum SDL_GameControllerButton
        {
            SDL_CONTROLLER_BUTTON_INVALID = -1,
            SDL_CONTROLLER_BUTTON_A,
            SDL_CONTROLLER_BUTTON_B,
            SDL_CONTROLLER_BUTTON_X,
            SDL_CONTROLLER_BUTTON_Y,
            SDL_CONTROLLER_BUTTON_BACK,
            SDL_CONTROLLER_BUTTON_GUIDE,
            SDL_CONTROLLER_BUTTON_START,
            SDL_CONTROLLER_BUTTON_LEFTSTICK,
            SDL_CONTROLLER_BUTTON_RIGHTSTICK,
            SDL_CONTROLLER_BUTTON_LEFTSHOULDER,
            SDL_CONTROLLER_BUTTON_RIGHTSHOULDER,
            SDL_CONTROLLER_BUTTON_DPAD_UP,
            SDL_CONTROLLER_BUTTON_DPAD_DOWN,
            SDL_CONTROLLER_BUTTON_DPAD_LEFT,
            SDL_CONTROLLER_BUTTON_DPAD_RIGHT,
            SDL_CONTROLLER_BUTTON_MISC1,
            SDL_CONTROLLER_BUTTON_PADDLE1,
            SDL_CONTROLLER_BUTTON_PADDLE2,
            SDL_CONTROLLER_BUTTON_PADDLE3,
            SDL_CONTROLLER_BUTTON_PADDLE4,
            SDL_CONTROLLER_BUTTON_TOUCHPAD,
            SDL_CONTROLLER_BUTTON_MAX
        }

        public enum SDL_GameControllerType
        {
            SDL_CONTROLLER_TYPE_UNKNOWN,
            SDL_CONTROLLER_TYPE_XBOX360,
            SDL_CONTROLLER_TYPE_XBOXONE,
            SDL_CONTROLLER_TYPE_PS3,
            SDL_CONTROLLER_TYPE_PS4,
            SDL_CONTROLLER_TYPE_NINTENDO_SWITCH_PRO,
            SDL_CONTROLLER_TYPE_VIRTUAL,
            SDL_CONTROLLER_TYPE_PS5,
            SDL_CONTROLLER_TYPE_AMAZON_LUNA,
            SDL_CONTROLLER_TYPE_GOOGLE_STADIA
        }

        public struct INTERNAL_GameControllerButtonBind_hat
        {
            public int hat;

            public int hat_mask;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INTERNAL_GameControllerButtonBind_union
        {
            [FieldOffset(0)]
            public int button;

            [FieldOffset(0)]
            public int axis;

            [FieldOffset(0)]
            public INTERNAL_GameControllerButtonBind_hat hat;
        }

        public struct SDL_GameControllerButtonBind
        {
            public SDL_GameControllerBindType bindType;

            public INTERNAL_GameControllerButtonBind_union value;
        }

        private struct INTERNAL_SDL_GameControllerButtonBind
        {
            public int bindType;

            public int unionVal0;

            public int unionVal1;
        }

        public struct SDL_HapticDirection
        {
            public byte type;

            public unsafe fixed int dir[3];
        }

        public struct SDL_HapticConstant
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public short level;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        public struct SDL_HapticPeriodic
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public ushort period;

            public short magnitude;

            public short offset;

            public ushort phase;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        public struct SDL_HapticCondition
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public unsafe fixed ushort right_sat[3];

            public unsafe fixed ushort left_sat[3];

            public unsafe fixed short right_coeff[3];

            public unsafe fixed short left_coeff[3];

            public unsafe fixed ushort deadband[3];

            public unsafe fixed short center[3];
        }

        public struct SDL_HapticRamp
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public short start;

            public short end;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        public struct SDL_HapticLeftRight
        {
            public ushort type;

            public uint length;

            public ushort large_magnitude;

            public ushort small_magnitude;
        }

        public struct SDL_HapticCustom
        {
            public ushort type;

            public SDL_HapticDirection direction;

            public uint length;

            public ushort delay;

            public ushort button;

            public ushort interval;

            public byte channels;

            public ushort period;

            public ushort samples;

            public IntPtr data;

            public ushort attack_length;

            public ushort attack_level;

            public ushort fade_length;

            public ushort fade_level;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_HapticEffect
        {
            [FieldOffset(0)]
            public ushort type;

            [FieldOffset(0)]
            public SDL_HapticConstant constant;

            [FieldOffset(0)]
            public SDL_HapticPeriodic periodic;

            [FieldOffset(0)]
            public SDL_HapticCondition condition;

            [FieldOffset(0)]
            public SDL_HapticRamp ramp;

            [FieldOffset(0)]
            public SDL_HapticLeftRight leftright;

            [FieldOffset(0)]
            public SDL_HapticCustom custom;
        }

        public enum SDL_SensorType
        {
            SDL_SENSOR_INVALID = -1,
            SDL_SENSOR_UNKNOWN,
            SDL_SENSOR_ACCEL,
            SDL_SENSOR_GYRO
        }

        public enum SDL_AudioStatus
        {
            SDL_AUDIO_STOPPED,
            SDL_AUDIO_PLAYING,
            SDL_AUDIO_PAUSED
        }

        public struct SDL_AudioSpec
        {
            public int freq;

            public ushort format;

            public byte channels;

            public byte silence;

            public ushort samples;

            public uint size;

            public SDL_AudioCallback callback;

            public IntPtr userdata;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_AudioCallback(IntPtr userdata, IntPtr stream, int len);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint SDL_TimerCallback(uint interval, IntPtr param);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDL_WindowsMessageHook(IntPtr userdata, IntPtr hWnd, uint message, ulong wParam, long lParam);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_iPhoneAnimationCallback(IntPtr p);

        public enum SDL_WinRT_DeviceFamily
        {
            SDL_WINRT_DEVICEFAMILY_UNKNOWN,
            SDL_WINRT_DEVICEFAMILY_DESKTOP,
            SDL_WINRT_DEVICEFAMILY_MOBILE,
            SDL_WINRT_DEVICEFAMILY_XBOX
        }

        public enum SDL_SYSWM_TYPE
        {
            SDL_SYSWM_UNKNOWN,
            SDL_SYSWM_WINDOWS,
            SDL_SYSWM_X11,
            SDL_SYSWM_DIRECTFB,
            SDL_SYSWM_COCOA,
            SDL_SYSWM_UIKIT,
            SDL_SYSWM_WAYLAND,
            SDL_SYSWM_MIR,
            SDL_SYSWM_WINRT,
            SDL_SYSWM_ANDROID,
            SDL_SYSWM_VIVANTE,
            SDL_SYSWM_OS2,
            SDL_SYSWM_HAIKU,
            SDL_SYSWM_KMSDRM
        }

        public struct INTERNAL_windows_wminfo
        {
            public IntPtr window;

            public IntPtr hdc;

            public IntPtr hinstance;
        }

        public struct INTERNAL_winrt_wminfo
        {
            public IntPtr window;
        }

        public struct INTERNAL_x11_wminfo
        {
            public IntPtr display;

            public IntPtr window;
        }

        public struct INTERNAL_directfb_wminfo
        {
            public IntPtr dfb;

            public IntPtr window;

            public IntPtr surface;
        }

        public struct INTERNAL_cocoa_wminfo
        {
            public IntPtr window;
        }

        public struct INTERNAL_uikit_wminfo
        {
            public IntPtr window;

            public uint framebuffer;

            public uint colorbuffer;

            public uint resolveFramebuffer;
        }

        public struct INTERNAL_wayland_wminfo
        {
            public IntPtr display;

            public IntPtr surface;

            public IntPtr shell_surface;

            public IntPtr egl_window;

            public IntPtr xdg_surface;

            public IntPtr xdg_toplevel;

            public IntPtr xdg_popup;

            public IntPtr xdg_positioner;
        }

        public struct INTERNAL_mir_wminfo
        {
            public IntPtr connection;

            public IntPtr surface;
        }

        public struct INTERNAL_android_wminfo
        {
            public IntPtr window;

            public IntPtr surface;
        }

        public struct INTERNAL_vivante_wminfo
        {
            public IntPtr display;

            public IntPtr window;
        }

        public struct INTERNAL_os2_wminfo
        {
            public IntPtr hwnd;

            public IntPtr hwndFrame;
        }

        public struct INTERNAL_kmsdrm_wminfo
        {
            private int dev_index;

            private int drm_fd;

            private IntPtr gbm_dev;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INTERNAL_SysWMDriverUnion
        {
            [FieldOffset(0)]
            public INTERNAL_windows_wminfo win;

            [FieldOffset(0)]
            public INTERNAL_winrt_wminfo winrt;

            [FieldOffset(0)]
            public INTERNAL_x11_wminfo x11;

            [FieldOffset(0)]
            public INTERNAL_directfb_wminfo dfb;

            [FieldOffset(0)]
            public INTERNAL_cocoa_wminfo cocoa;

            [FieldOffset(0)]
            public INTERNAL_uikit_wminfo uikit;

            [FieldOffset(0)]
            public INTERNAL_wayland_wminfo wl;

            [FieldOffset(0)]
            public INTERNAL_mir_wminfo mir;

            [FieldOffset(0)]
            public INTERNAL_android_wminfo android;

            [FieldOffset(0)]
            public INTERNAL_os2_wminfo os2;

            [FieldOffset(0)]
            public INTERNAL_vivante_wminfo vivante;

            [FieldOffset(0)]
            public INTERNAL_kmsdrm_wminfo ksmdrm;
        }

        public struct SDL_SysWMinfo
        {
            public SDL_version version;

            public SDL_SYSWM_TYPE subsystem;

            public INTERNAL_SysWMDriverUnion info;
        }

        public enum SDL_PowerState
        {
            SDL_POWERSTATE_UNKNOWN,
            SDL_POWERSTATE_ON_BATTERY,
            SDL_POWERSTATE_NO_BATTERY,
            SDL_POWERSTATE_CHARGING,
            SDL_POWERSTATE_CHARGED
        }

        public struct SDL_Locale
        {
            public IntPtr language;

            public IntPtr country;
        }

        private const string nativeLibName = "SDL2";

        public const int RW_SEEK_SET = 0;

        public const int RW_SEEK_CUR = 1;

        public const int RW_SEEK_END = 2;

        public const uint SDL_RWOPS_UNKNOWN = 0u;

        public const uint SDL_RWOPS_WINFILE = 1u;

        public const uint SDL_RWOPS_STDFILE = 2u;

        public const uint SDL_RWOPS_JNIFILE = 3u;

        public const uint SDL_RWOPS_MEMORY = 4u;

        public const uint SDL_RWOPS_MEMORY_RO = 5u;

        public const uint SDL_INIT_TIMER = 1u;

        public const uint SDL_INIT_AUDIO = 16u;

        public const uint SDL_INIT_VIDEO = 32u;

        public const uint SDL_INIT_JOYSTICK = 512u;

        public const uint SDL_INIT_HAPTIC = 4096u;

        public const uint SDL_INIT_GAMECONTROLLER = 8192u;

        public const uint SDL_INIT_EVENTS = 16384u;

        public const uint SDL_INIT_SENSOR = 32768u;

        public const uint SDL_INIT_NOPARACHUTE = 1048576u;

        public const uint SDL_INIT_EVERYTHING = 62001u;

        public const string SDL_HINT_FRAMEBUFFER_ACCELERATION = "SDL_FRAMEBUFFER_ACCELERATION";

        public const string SDL_HINT_RENDER_DRIVER = "SDL_RENDER_DRIVER";

        public const string SDL_HINT_RENDER_OPENGL_SHADERS = "SDL_RENDER_OPENGL_SHADERS";

        public const string SDL_HINT_RENDER_DIRECT3D_THREADSAFE = "SDL_RENDER_DIRECT3D_THREADSAFE";

        public const string SDL_HINT_RENDER_VSYNC = "SDL_RENDER_VSYNC";

        public const string SDL_HINT_VIDEO_X11_XVIDMODE = "SDL_VIDEO_X11_XVIDMODE";

        public const string SDL_HINT_VIDEO_X11_XINERAMA = "SDL_VIDEO_X11_XINERAMA";

        public const string SDL_HINT_VIDEO_X11_XRANDR = "SDL_VIDEO_X11_XRANDR";

        public const string SDL_HINT_GRAB_KEYBOARD = "SDL_GRAB_KEYBOARD";

        public const string SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS = "SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS";

        public const string SDL_HINT_IDLE_TIMER_DISABLED = "SDL_IOS_IDLE_TIMER_DISABLED";

        public const string SDL_HINT_ORIENTATIONS = "SDL_IOS_ORIENTATIONS";

        public const string SDL_HINT_XINPUT_ENABLED = "SDL_XINPUT_ENABLED";

        public const string SDL_HINT_GAMECONTROLLERCONFIG = "SDL_GAMECONTROLLERCONFIG";

        public const string SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS = "SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS";

        public const string SDL_HINT_ALLOW_TOPMOST = "SDL_ALLOW_TOPMOST";

        public const string SDL_HINT_TIMER_RESOLUTION = "SDL_TIMER_RESOLUTION";

        public const string SDL_HINT_RENDER_SCALE_QUALITY = "SDL_RENDER_SCALE_QUALITY";

        public const string SDL_HINT_VIDEO_HIGHDPI_DISABLED = "SDL_VIDEO_HIGHDPI_DISABLED";

        public const string SDL_HINT_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK = "SDL_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK";

        public const string SDL_HINT_VIDEO_WIN_D3DCOMPILER = "SDL_VIDEO_WIN_D3DCOMPILER";

        public const string SDL_HINT_MOUSE_RELATIVE_MODE_WARP = "SDL_MOUSE_RELATIVE_MODE_WARP";

        public const string SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT = "SDL_VIDEO_WINDOW_SHARE_PIXEL_FORMAT";

        public const string SDL_HINT_VIDEO_ALLOW_SCREENSAVER = "SDL_VIDEO_ALLOW_SCREENSAVER";

        public const string SDL_HINT_ACCELEROMETER_AS_JOYSTICK = "SDL_ACCELEROMETER_AS_JOYSTICK";

        public const string SDL_HINT_VIDEO_MAC_FULLSCREEN_SPACES = "SDL_VIDEO_MAC_FULLSCREEN_SPACES";

        public const string SDL_HINT_WINRT_PRIVACY_POLICY_URL = "SDL_WINRT_PRIVACY_POLICY_URL";

        public const string SDL_HINT_WINRT_PRIVACY_POLICY_LABEL = "SDL_WINRT_PRIVACY_POLICY_LABEL";

        public const string SDL_HINT_WINRT_HANDLE_BACK_BUTTON = "SDL_WINRT_HANDLE_BACK_BUTTON";

        public const string SDL_HINT_NO_SIGNAL_HANDLERS = "SDL_NO_SIGNAL_HANDLERS";

        public const string SDL_HINT_IME_INTERNAL_EDITING = "SDL_IME_INTERNAL_EDITING";

        public const string SDL_HINT_ANDROID_SEPARATE_MOUSE_AND_TOUCH = "SDL_ANDROID_SEPARATE_MOUSE_AND_TOUCH";

        public const string SDL_HINT_EMSCRIPTEN_KEYBOARD_ELEMENT = "SDL_EMSCRIPTEN_KEYBOARD_ELEMENT";

        public const string SDL_HINT_THREAD_STACK_SIZE = "SDL_THREAD_STACK_SIZE";

        public const string SDL_HINT_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN = "SDL_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN";

        public const string SDL_HINT_WINDOWS_ENABLE_MESSAGELOOP = "SDL_WINDOWS_ENABLE_MESSAGELOOP";

        public const string SDL_HINT_WINDOWS_NO_CLOSE_ON_ALT_F4 = "SDL_WINDOWS_NO_CLOSE_ON_ALT_F4";

        public const string SDL_HINT_XINPUT_USE_OLD_JOYSTICK_MAPPING = "SDL_XINPUT_USE_OLD_JOYSTICK_MAPPING";

        public const string SDL_HINT_MAC_BACKGROUND_APP = "SDL_MAC_BACKGROUND_APP";

        public const string SDL_HINT_VIDEO_X11_NET_WM_PING = "SDL_VIDEO_X11_NET_WM_PING";

        public const string SDL_HINT_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION = "SDL_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION";

        public const string SDL_HINT_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION = "SDL_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION";

        public const string SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH = "SDL_MOUSE_FOCUS_CLICKTHROUGH";

        public const string SDL_HINT_BMP_SAVE_LEGACY_FORMAT = "SDL_BMP_SAVE_LEGACY_FORMAT";

        public const string SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING = "SDL_WINDOWS_DISABLE_THREAD_NAMING";

        public const string SDL_HINT_APPLE_TV_REMOTE_ALLOW_ROTATION = "SDL_APPLE_TV_REMOTE_ALLOW_ROTATION";

        public const string SDL_HINT_AUDIO_RESAMPLING_MODE = "SDL_AUDIO_RESAMPLING_MODE";

        public const string SDL_HINT_RENDER_LOGICAL_SIZE_MODE = "SDL_RENDER_LOGICAL_SIZE_MODE";

        public const string SDL_HINT_MOUSE_NORMAL_SPEED_SCALE = "SDL_MOUSE_NORMAL_SPEED_SCALE";

        public const string SDL_HINT_MOUSE_RELATIVE_SPEED_SCALE = "SDL_MOUSE_RELATIVE_SPEED_SCALE";

        public const string SDL_HINT_TOUCH_MOUSE_EVENTS = "SDL_TOUCH_MOUSE_EVENTS";

        public const string SDL_HINT_WINDOWS_INTRESOURCE_ICON = "SDL_WINDOWS_INTRESOURCE_ICON";

        public const string SDL_HINT_WINDOWS_INTRESOURCE_ICON_SMALL = "SDL_WINDOWS_INTRESOURCE_ICON_SMALL";

        public const string SDL_HINT_IOS_HIDE_HOME_INDICATOR = "SDL_IOS_HIDE_HOME_INDICATOR";

        public const string SDL_HINT_TV_REMOTE_AS_JOYSTICK = "SDL_TV_REMOTE_AS_JOYSTICK";

        public const string SDL_VIDEO_X11_NET_WM_BYPASS_COMPOSITOR = "SDL_VIDEO_X11_NET_WM_BYPASS_COMPOSITOR";

        public const string SDL_HINT_MOUSE_DOUBLE_CLICK_TIME = "SDL_MOUSE_DOUBLE_CLICK_TIME";

        public const string SDL_HINT_MOUSE_DOUBLE_CLICK_RADIUS = "SDL_MOUSE_DOUBLE_CLICK_RADIUS";

        public const string SDL_HINT_JOYSTICK_HIDAPI = "SDL_JOYSTICK_HIDAPI";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS4 = "SDL_JOYSTICK_HIDAPI_PS4";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS4_RUMBLE = "SDL_JOYSTICK_HIDAPI_PS4_RUMBLE";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STEAM = "SDL_JOYSTICK_HIDAPI_STEAM";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SWITCH = "SDL_JOYSTICK_HIDAPI_SWITCH";

        public const string SDL_HINT_JOYSTICK_HIDAPI_XBOX = "SDL_JOYSTICK_HIDAPI_XBOX";

        public const string SDL_HINT_ENABLE_STEAM_CONTROLLERS = "SDL_ENABLE_STEAM_CONTROLLERS";

        public const string SDL_HINT_ANDROID_TRAP_BACK_BUTTON = "SDL_ANDROID_TRAP_BACK_BUTTON";

        public const string SDL_HINT_MOUSE_TOUCH_EVENTS = "SDL_MOUSE_TOUCH_EVENTS";

        public const string SDL_HINT_GAMECONTROLLERCONFIG_FILE = "SDL_GAMECONTROLLERCONFIG_FILE";

        public const string SDL_HINT_ANDROID_BLOCK_ON_PAUSE = "SDL_ANDROID_BLOCK_ON_PAUSE";

        public const string SDL_HINT_RENDER_BATCHING = "SDL_RENDER_BATCHING";

        public const string SDL_HINT_EVENT_LOGGING = "SDL_EVENT_LOGGING";

        public const string SDL_HINT_WAVE_RIFF_CHUNK_SIZE = "SDL_WAVE_RIFF_CHUNK_SIZE";

        public const string SDL_HINT_WAVE_TRUNCATION = "SDL_WAVE_TRUNCATION";

        public const string SDL_HINT_WAVE_FACT_CHUNK = "SDL_WAVE_FACT_CHUNK";

        public const string SDL_HINT_VIDO_X11_WINDOW_VISUALID = "SDL_VIDEO_X11_WINDOW_VISUALID";

        public const string SDL_HINT_GAMECONTROLLER_USE_BUTTON_LABELS = "SDL_GAMECONTROLLER_USE_BUTTON_LABELS";

        public const string SDL_HINT_VIDEO_EXTERNAL_CONTEXT = "SDL_VIDEO_EXTERNAL_CONTEXT";

        public const string SDL_HINT_JOYSTICK_HIDAPI_GAMECUBE = "SDL_JOYSTICK_HIDAPI_GAMECUBE";

        public const string SDL_HINT_DISPLAY_USABLE_BOUNDS = "SDL_DISPLAY_USABLE_BOUNDS";

        public const string SDL_HINT_VIDEO_X11_FORCE_EGL = "SDL_VIDEO_X11_FORCE_EGL";

        public const string SDL_HINT_GAMECONTROLLERTYPE = "SDL_GAMECONTROLLERTYPE";

        public const string SDL_HINT_JOYSTICK_HIDAPI_CORRELATE_XINPUT = "SDL_JOYSTICK_HIDAPI_CORRELATE_XINPUT";

        public const string SDL_HINT_JOYSTICK_RAWINPUT = "SDL_JOYSTICK_RAWINPUT";

        public const string SDL_HINT_AUDIO_DEVICE_APP_NAME = "SDL_AUDIO_DEVICE_APP_NAME";

        public const string SDL_HINT_AUDIO_DEVICE_STREAM_NAME = "SDL_AUDIO_DEVICE_STREAM_NAME";

        public const string SDL_HINT_PREFERRED_LOCALES = "SDL_PREFERRED_LOCALES";

        public const string SDL_HINT_THREAD_PRIORITY_POLICY = "SDL_THREAD_PRIORITY_POLICY";

        public const string SDL_HINT_EMSCRIPTEN_ASYNCIFY = "SDL_EMSCRIPTEN_ASYNCIFY";

        public const string SDL_HINT_LINUX_JOYSTICK_DEADZONES = "SDL_LINUX_JOYSTICK_DEADZONES";

        public const string SDL_HINT_ANDROID_BLOCK_ON_PAUSE_PAUSEAUDIO = "SDL_ANDROID_BLOCK_ON_PAUSE_PAUSEAUDIO";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS5 = "SDL_JOYSTICK_HIDAPI_PS5";

        public const string SDL_HINT_THREAD_FORCE_REALTIME_TIME_CRITICAL = "SDL_THREAD_FORCE_REALTIME_TIME_CRITICAL";

        public const string SDL_HINT_JOYSTICK_THREAD = "SDL_JOYSTICK_THREAD";

        public const string SDL_HINT_AUTO_UPDATE_JOYSTICKS = "SDL_AUTO_UPDATE_JOYSTICKS";

        public const string SDL_HINT_AUTO_UPDATE_SENSORS = "SDL_AUTO_UPDATE_SENSORS";

        public const string SDL_HINT_MOUSE_RELATIVE_SCALING = "SDL_MOUSE_RELATIVE_SCALING";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS5_RUMBLE = "SDL_JOYSTICK_HIDAPI_PS5_RUMBLE";

        public const string SDL_HINT_WINDOWS_FORCE_MUTEX_CRITICAL_SECTIONS = "SDL_WINDOWS_FORCE_MUTEX_CRITICAL_SECTIONS";

        public const string SDL_HINT_WINDOWS_FORCE_SEMAPHORE_KERNEL = "SDL_WINDOWS_FORCE_SEMAPHORE_KERNEL";

        public const string SDL_HINT_JOYSTICK_HIDAPI_PS5_PLAYER_LED = "SDL_JOYSTICK_HIDAPI_PS5_PLAYER_LED";

        public const string SDL_HINT_WINDOWS_USE_D3D9EX = "SDL_WINDOWS_USE_D3D9EX";

        public const string SDL_HINT_JOYSTICK_HIDAPI_JOY_CONS = "SDL_JOYSTICK_HIDAPI_JOY_CONS";

        public const string SDL_HINT_JOYSTICK_HIDAPI_STADIA = "SDL_JOYSTICK_HIDAPI_STADIA";

        public const string SDL_HINT_JOYSTICK_HIDAPI_SWITCH_HOME_LED = "SDL_JOYSTICK_HIDAPI_SWITCH_HOME_LED";

        public const string SDL_HINT_ALLOW_ALT_TAB_WHILE_GRABBED = "SDL_ALLOW_ALT_TAB_WHILE_GRABBED";

        public const string SDL_HINT_KMSDRM_REQUIRE_DRM_MASTER = "SDL_KMSDRM_REQUIRE_DRM_MASTER";

        public const string SDL_HINT_AUDIO_DEVICE_STREAM_ROLE = "SDL_AUDIO_DEVICE_STREAM_ROLE";

        public const string SDL_HINT_X11_FORCE_OVERRIDE_REDIRECT = "SDL_X11_FORCE_OVERRIDE_REDIRECT";

        public const string SDL_HINT_JOYSTICK_HIDAPI_LUNA = "SDL_JOYSTICK_HIDAPI_LUNA";

        public const string SDL_HINT_JOYSTICK_RAWINPUT_CORRELATE_XINPUT = "SDL_JOYSTICK_RAWINPUT_CORRELATE_XINPUT";

        public const string SDL_HINT_AUDIO_INCLUDE_MONITORS = "SDL_AUDIO_INCLUDE_MONITORS";

        public const string SDL_HINT_VIDEO_WAYLAND_ALLOW_LIBDECOR = "SDL_VIDEO_WAYLAND_ALLOW_LIBDECOR";

        public const string SDL_HINT_VIDEO_EGL_ALLOW_TRANSPARENCY = "SDL_VIDEO_EGL_ALLOW_TRANSPARENCY";

        public const string SDL_HINT_APP_NAME = "SDL_APP_NAME";

        public const string SDL_HINT_SCREENSAVER_INHIBIT_ACTIVITY_NAME = "SDL_SCREENSAVER_INHIBIT_ACTIVITY_NAME";

        public const string SDL_HINT_IME_SHOW_UI = "SDL_IME_SHOW_UI";

        public const string SDL_HINT_WINDOW_NO_ACTIVATION_WHEN_SHOWN = "SDL_WINDOW_NO_ACTIVATION_WHEN_SHOWN";

        public const string SDL_HINT_POLL_SENTINEL = "SDL_POLL_SENTINEL";

        public const string SDL_HINT_JOYSTICK_DEVICE = "SDL_JOYSTICK_DEVICE";

        public const string SDL_HINT_LINUX_JOYSTICK_CLASSIC = "SDL_LINUX_JOYSTICK_CLASSIC";

        public const string SDL_HINT_RENDER_LINE_METHOD = "SDL_RENDER_LINE_METHOD";

        public const string SDL_HINT_FORCE_RAISEWINDOW = "SDL_HINT_FORCE_RAISEWINDOW";

        public const string SDL_HINT_IME_SUPPORT_EXTENDED_TEXT = "SDL_IME_SUPPORT_EXTENDED_TEXT";

        public const string SDL_HINT_JOYSTICK_GAMECUBE_RUMBLE_BRAKE = "SDL_JOYSTICK_GAMECUBE_RUMBLE_BRAKE";

        public const string SDL_HINT_JOYSTICK_ROG_CHAKRAM = "SDL_JOYSTICK_ROG_CHAKRAM";

        public const string SDL_HINT_MOUSE_RELATIVE_MODE_CENTER = "SDL_MOUSE_RELATIVE_MODE_CENTER";

        public const string SDL_HINT_MOUSE_AUTO_CAPTURE = "SDL_MOUSE_AUTO_CAPTURE";

        public const string SDL_HINT_VITA_TOUCH_MOUSE_DEVICE = "SDL_HINT_VITA_TOUCH_MOUSE_DEVICE";

        public const string SDL_HINT_VIDEO_WAYLAND_PREFER_LIBDECOR = "SDL_VIDEO_WAYLAND_PREFER_LIBDECOR";

        public const string SDL_HINT_VIDEO_FOREIGN_WINDOW_OPENGL = "SDL_VIDEO_FOREIGN_WINDOW_OPENGL";

        public const string SDL_HINT_VIDEO_FOREIGN_WINDOW_VULKAN = "SDL_VIDEO_FOREIGN_WINDOW_VULKAN";

        public const string SDL_HINT_X11_WINDOW_TYPE = "SDL_X11_WINDOW_TYPE";

        public const string SDL_HINT_QUIT_ON_LAST_WINDOW_CLOSE = "SDL_QUIT_ON_LAST_WINDOW_CLOSE";

        public const int SDL_MAJOR_VERSION = 2;

        public const int SDL_MINOR_VERSION = 0;

        public const int SDL_PATCHLEVEL = 22;

        public static readonly int SDL_COMPILEDVERSION = SDL_VERSIONNUM(2, 0, 22);

        public const int SDL_WINDOWPOS_UNDEFINED_MASK = 536805376;

        public const int SDL_WINDOWPOS_CENTERED_MASK = 805240832;

        public const int SDL_WINDOWPOS_UNDEFINED = 536805376;

        public const int SDL_WINDOWPOS_CENTERED = 805240832;

        public static readonly uint SDL_PIXELFORMAT_UNKNOWN = 0u;

        public static readonly uint SDL_PIXELFORMAT_INDEX1LSB = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_INDEX1, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 1, 0);

        public static readonly uint SDL_PIXELFORMAT_INDEX1MSB = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_INDEX1, 2u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 1, 0);

        public static readonly uint SDL_PIXELFORMAT_INDEX4LSB = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_INDEX4, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 4, 0);

        public static readonly uint SDL_PIXELFORMAT_INDEX4MSB = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_INDEX4, 2u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 4, 0);

        public static readonly uint SDL_PIXELFORMAT_INDEX8 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_INDEX8, 0u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 8, 1);

        public static readonly uint SDL_PIXELFORMAT_RGB332 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED8, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_332, 8, 1);

        public static readonly uint SDL_PIXELFORMAT_XRGB444 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 12, 2);

        public static readonly uint SDL_PIXELFORMAT_RGB444 = SDL_PIXELFORMAT_XRGB444;

        public static readonly uint SDL_PIXELFORMAT_XBGR444 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 5u, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 12, 2);

        public static readonly uint SDL_PIXELFORMAT_BGR444 = SDL_PIXELFORMAT_XBGR444;

        public static readonly uint SDL_PIXELFORMAT_XRGB1555 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 15, 2);

        public static readonly uint SDL_PIXELFORMAT_RGB555 = SDL_PIXELFORMAT_XRGB1555;

        public static readonly uint SDL_PIXELFORMAT_XBGR1555 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_INDEX1, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 15, 2);

        public static readonly uint SDL_PIXELFORMAT_BGR555 = SDL_PIXELFORMAT_XBGR1555;

        public static readonly uint SDL_PIXELFORMAT_ARGB4444 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 3u, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_RGBA4444 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 4u, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_ABGR4444 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 7u, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_BGRA4444 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 8u, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_ARGB1555 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 3u, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_RGBA5551 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 4u, SDL_PackedLayout.SDL_PACKEDLAYOUT_5551, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_ABGR1555 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 7u, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_BGRA5551 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 8u, SDL_PackedLayout.SDL_PACKEDLAYOUT_5551, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_RGB565 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_565, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_BGR565 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED16, 5u, SDL_PackedLayout.SDL_PACKEDLAYOUT_565, 16, 2);

        public static readonly uint SDL_PIXELFORMAT_RGB24 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_ARRAYU8, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 24, 3);

        public static readonly uint SDL_PIXELFORMAT_BGR24 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_ARRAYU8, 4u, SDL_PackedLayout.SDL_PACKEDLAYOUT_NONE, 24, 3);

        public static readonly uint SDL_PIXELFORMAT_XRGB888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 1u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4);

        public static readonly uint SDL_PIXELFORMAT_RGB888 = SDL_PIXELFORMAT_XRGB888;

        public static readonly uint SDL_PIXELFORMAT_RGBX8888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 2u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4);

        public static readonly uint SDL_PIXELFORMAT_XBGR888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 5u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4);

        public static readonly uint SDL_PIXELFORMAT_BGR888 = SDL_PIXELFORMAT_XBGR888;

        public static readonly uint SDL_PIXELFORMAT_BGRX8888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 6u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4);

        public static readonly uint SDL_PIXELFORMAT_ARGB8888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 3u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4);

        public static readonly uint SDL_PIXELFORMAT_RGBA8888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 4u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4);

        public static readonly uint SDL_PIXELFORMAT_ABGR8888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 7u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4);

        public static readonly uint SDL_PIXELFORMAT_BGRA8888 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 8u, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4);

        public static readonly uint SDL_PIXELFORMAT_ARGB2101010 = SDL_DEFINE_PIXELFORMAT(SDL_PixelType.SDL_PIXELTYPE_PACKED32, 3u, SDL_PackedLayout.SDL_PACKEDLAYOUT_2101010, 32, 4);

        public static readonly uint SDL_PIXELFORMAT_YV12 = SDL_DEFINE_PIXELFOURCC(89, 86, 49, 50);

        public static readonly uint SDL_PIXELFORMAT_IYUV = SDL_DEFINE_PIXELFOURCC(73, 89, 85, 86);

        public static readonly uint SDL_PIXELFORMAT_YUY2 = SDL_DEFINE_PIXELFOURCC(89, 85, 89, 50);

        public static readonly uint SDL_PIXELFORMAT_UYVY = SDL_DEFINE_PIXELFOURCC(85, 89, 86, 89);

        public static readonly uint SDL_PIXELFORMAT_YVYU = SDL_DEFINE_PIXELFOURCC(89, 86, 89, 85);

        public const int SDL_NONSHAPEABLE_WINDOW = -1;

        public const int SDL_INVALID_SHAPE_ARGUMENT = -2;

        public const int SDL_WINDOW_LACKS_SHAPE = -3;

        public const uint SDL_SWSURFACE = 0u;

        public const uint SDL_PREALLOC = 1u;

        public const uint SDL_RLEACCEL = 2u;

        public const uint SDL_DONTFREE = 4u;

        public const byte SDL_PRESSED = 1;

        public const byte SDL_RELEASED = 0;

        public const int SDL_TEXTEDITINGEVENT_TEXT_SIZE = 32;

        public const int SDL_TEXTINPUTEVENT_TEXT_SIZE = 32;

        public const int SDL_QUERY = -1;

        public const int SDL_IGNORE = 0;

        public const int SDL_DISABLE = 0;

        public const int SDL_ENABLE = 1;

        public const int SDLK_SCANCODE_MASK = 1073741824;

        public const uint SDL_BUTTON_LEFT = 1u;

        public const uint SDL_BUTTON_MIDDLE = 2u;

        public const uint SDL_BUTTON_RIGHT = 3u;

        public const uint SDL_BUTTON_X1 = 4u;

        public const uint SDL_BUTTON_X2 = 5u;

        public static readonly uint SDL_BUTTON_LMASK = SDL_BUTTON(1u);

        public static readonly uint SDL_BUTTON_MMASK = SDL_BUTTON(2u);

        public static readonly uint SDL_BUTTON_RMASK = SDL_BUTTON(3u);

        public static readonly uint SDL_BUTTON_X1MASK = SDL_BUTTON(4u);

        public static readonly uint SDL_BUTTON_X2MASK = SDL_BUTTON(5u);

        public const uint SDL_TOUCH_MOUSEID = uint.MaxValue;

        public const byte SDL_HAT_CENTERED = 0;

        public const byte SDL_HAT_UP = 1;

        public const byte SDL_HAT_RIGHT = 2;

        public const byte SDL_HAT_DOWN = 4;

        public const byte SDL_HAT_LEFT = 8;

        public const byte SDL_HAT_RIGHTUP = 3;

        public const byte SDL_HAT_RIGHTDOWN = 6;

        public const byte SDL_HAT_LEFTUP = 9;

        public const byte SDL_HAT_LEFTDOWN = 12;

        public const float SDL_IPHONE_MAX_GFORCE = 5f;

        public const ushort SDL_HAPTIC_CONSTANT = 1;

        public const ushort SDL_HAPTIC_SINE = 2;

        public const ushort SDL_HAPTIC_LEFTRIGHT = 4;

        public const ushort SDL_HAPTIC_TRIANGLE = 8;

        public const ushort SDL_HAPTIC_SAWTOOTHUP = 16;

        public const ushort SDL_HAPTIC_SAWTOOTHDOWN = 32;

        public const ushort SDL_HAPTIC_SPRING = 128;

        public const ushort SDL_HAPTIC_DAMPER = 256;

        public const ushort SDL_HAPTIC_INERTIA = 512;

        public const ushort SDL_HAPTIC_FRICTION = 1024;

        public const ushort SDL_HAPTIC_CUSTOM = 2048;

        public const ushort SDL_HAPTIC_GAIN = 4096;

        public const ushort SDL_HAPTIC_AUTOCENTER = 8192;

        public const ushort SDL_HAPTIC_STATUS = 16384;

        public const ushort SDL_HAPTIC_PAUSE = 32768;

        public const byte SDL_HAPTIC_POLAR = 0;

        public const byte SDL_HAPTIC_CARTESIAN = 1;

        public const byte SDL_HAPTIC_SPHERICAL = 2;

        public const byte SDL_HAPTIC_STEERING_AXIS = 3;

        public const uint SDL_HAPTIC_INFINITY = uint.MaxValue;

        public const float SDL_STANDARD_GRAVITY = 9.80665f;

        public const ushort SDL_AUDIO_MASK_BITSIZE = 255;

        public const ushort SDL_AUDIO_MASK_DATATYPE = 256;

        public const ushort SDL_AUDIO_MASK_ENDIAN = 4096;

        public const ushort SDL_AUDIO_MASK_SIGNED = 32768;

        public const ushort AUDIO_U8 = 8;

        public const ushort AUDIO_S8 = 32776;

        public const ushort AUDIO_U16LSB = 16;

        public const ushort AUDIO_S16LSB = 32784;

        public const ushort AUDIO_U16MSB = 4112;

        public const ushort AUDIO_S16MSB = 36880;

        public const ushort AUDIO_U16 = 16;

        public const ushort AUDIO_S16 = 32784;

        public const ushort AUDIO_S32LSB = 32800;

        public const ushort AUDIO_S32MSB = 36896;

        public const ushort AUDIO_S32 = 32800;

        public const ushort AUDIO_F32LSB = 33056;

        public const ushort AUDIO_F32MSB = 37152;

        public const ushort AUDIO_F32 = 33056;

        public static readonly ushort AUDIO_U16SYS = (ushort)(BitConverter.IsLittleEndian ? 16 : 4112);

        public static readonly ushort AUDIO_S16SYS = (ushort)(BitConverter.IsLittleEndian ? 32784 : 36880);

        public static readonly ushort AUDIO_S32SYS = (ushort)(BitConverter.IsLittleEndian ? 32800 : 36896);

        public static readonly ushort AUDIO_F32SYS = (ushort)(BitConverter.IsLittleEndian ? 33056 : 37152);

        public const uint SDL_AUDIO_ALLOW_FREQUENCY_CHANGE = 1u;

        public const uint SDL_AUDIO_ALLOW_FORMAT_CHANGE = 2u;

        public const uint SDL_AUDIO_ALLOW_CHANNELS_CHANGE = 4u;

        public const uint SDL_AUDIO_ALLOW_SAMPLES_CHANGE = 8u;

        public const uint SDL_AUDIO_ALLOW_ANY_CHANGE = 15u;

        public const int SDL_MIX_MAXVOLUME = 128;

        public const int SDL_ANDROID_EXTERNAL_STORAGE_READ = 1;

        public const int SDL_ANDROID_EXTERNAL_STORAGE_WRITE = 2;

        internal static T PtrToStructure<T>(IntPtr ptr)
        {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }

        internal static Delegate GetDelegateForFunctionPointer<T>(IntPtr ptr)
        {
            return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T));
        }

        internal static int SizeOf<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }

        internal static int Utf8Size(string str)
        {
            if (str == null)
            {
                return 0;
            }
            return str.Length * 4 + 1;
        }

        internal unsafe static byte* Utf8Encode(string str, byte* buffer, int bufferSize)
        {
            if (str == null)
            {
                return null;
            }
            fixed (char* chars = str)
            {
                Encoding.UTF8.GetBytes(chars, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        internal unsafe static byte* Utf8EncodeHeap(string str)
        {
            if (str == null)
            {
                return null;
            }
            int num = Utf8Size(str);
            byte* ptr = (byte*)(void*)Marshal.AllocHGlobal(num);
            fixed (char* chars = str)
            {
                Encoding.UTF8.GetBytes(chars, str.Length + 1, ptr, num);
            }
            return ptr;
        }

        public unsafe static string UTF8_ToManaged(IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero)
            {
                return null;
            }
            byte* ptr;
            for (ptr = (byte*)(void*)s; *ptr != 0; ptr++)
            {
            }
            int num = (int)(ptr - (byte*)(void*)s);
            if (num == 0)
            {
                return string.Empty;
            }
            char* ptr2 = stackalloc char[num];
            int chars = Encoding.UTF8.GetChars((byte*)(void*)s, num, ptr2, num);
            string result = new string(ptr2, 0, chars);
            if (freePtr)
            {
                SDL_free(s);
            }
            return result;
        }

        public static uint SDL_FOURCC(byte A, byte B, byte C, byte D)
        {
            return (uint)(A | (B << 8) | (C << 16) | (D << 24));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr SDL_malloc(IntPtr size);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SDL_free(IntPtr memblock);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_memcpy(IntPtr dst, IntPtr src, IntPtr len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RWFromFile")]
        private unsafe static extern IntPtr INTERNAL_SDL_RWFromFile(byte* file, byte* mode);

        public unsafe static IntPtr SDL_RWFromFile(string file, string mode)
        {
            byte* intPtr = Utf8EncodeHeap(file);
            byte* ptr = Utf8EncodeHeap(mode);
            IntPtr result = INTERNAL_SDL_RWFromFile(intPtr, ptr);
            Marshal.FreeHGlobal((IntPtr)ptr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AllocRW();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeRW(IntPtr area);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromFP(IntPtr fp, SDL_bool autoclose);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromMem(IntPtr mem, int size);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromConstMem(IntPtr mem, int size);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWsize(IntPtr context);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWseek(IntPtr context, long offset, int whence);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWtell(IntPtr context);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWread(IntPtr context, IntPtr ptr, IntPtr size, IntPtr maxnum);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWwrite(IntPtr context, IntPtr ptr, IntPtr size, IntPtr maxnum);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_ReadU8(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_ReadLE16(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_ReadBE16(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_ReadLE32(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_ReadBE32(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_ReadLE64(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_ReadBE64(IntPtr src);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteU8(IntPtr dst, byte value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE16(IntPtr dst, ushort value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE16(IntPtr dst, ushort value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE32(IntPtr dst, uint value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE32(IntPtr dst, uint value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE64(IntPtr dst, ulong value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE64(IntPtr dst, ulong value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWclose(IntPtr context);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadFile")]
        private unsafe static extern IntPtr INTERNAL_SDL_LoadFile(byte* file, out IntPtr datasize);

        public unsafe static IntPtr SDL_LoadFile(string file, out IntPtr datasize)
        {
            byte* intPtr = Utf8EncodeHeap(file);
            IntPtr result = INTERNAL_SDL_LoadFile(intPtr, out datasize);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetMainReady();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_WinRTRunApp(SDL_main_func mainFunction, IntPtr reserved);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GDKRunApp(SDL_main_func mainFunction, IntPtr reserved);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UIKitRunApp(int argc, IntPtr argv, SDL_main_func mainFunction);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_Init(uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_InitSubSystem(uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_QuitSubSystem(uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WasInit(uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPlatform")]
        private static extern IntPtr INTERNAL_SDL_GetPlatform();

        public static string SDL_GetPlatform()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetPlatform());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClearHints();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetHint")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetHint(byte* name);

        public unsafe static string SDL_GetHint(string name)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return UTF8_ToManaged(INTERNAL_SDL_GetHint(Utf8Encode(name, buffer, num)));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetHint")]
        private unsafe static extern SDL_bool INTERNAL_SDL_SetHint(byte* name, byte* value);

        public unsafe static SDL_bool SDL_SetHint(string name, string value)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            int num2 = Utf8Size(value);
            byte* buffer2 = stackalloc byte[(int)(uint)num2];
            return INTERNAL_SDL_SetHint(Utf8Encode(name, buffer, num), Utf8Encode(value, buffer2, num2));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetHintWithPriority")]
        private unsafe static extern SDL_bool INTERNAL_SDL_SetHintWithPriority(byte* name, byte* value, SDL_HintPriority priority);

        public unsafe static SDL_bool SDL_SetHintWithPriority(string name, string value, SDL_HintPriority priority)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            int num2 = Utf8Size(value);
            byte* buffer2 = stackalloc byte[(int)(uint)num2];
            return INTERNAL_SDL_SetHintWithPriority(Utf8Encode(name, buffer, num), Utf8Encode(value, buffer2, num2), priority);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetHintBoolean")]
        private unsafe static extern SDL_bool INTERNAL_SDL_GetHintBoolean(byte* name, SDL_bool default_value);

        public unsafe static SDL_bool SDL_GetHintBoolean(string name, SDL_bool default_value)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GetHintBoolean(Utf8Encode(name, buffer, num), default_value);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClearError();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetError")]
        private static extern IntPtr INTERNAL_SDL_GetError();

        public static string SDL_GetError()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetError());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetError")]
        private unsafe static extern void INTERNAL_SDL_SetError(byte* fmtAndArglist);

        public unsafe static void SDL_SetError(string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_SetError(Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetErrorMsg(IntPtr errstr, int maxlength);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_Log")]
        private unsafe static extern void INTERNAL_SDL_Log(byte* fmtAndArglist);

        public unsafe static void SDL_Log(string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_Log(Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogVerbose")]
        private unsafe static extern void INTERNAL_SDL_LogVerbose(int category, byte* fmtAndArglist);

        public unsafe static void SDL_LogVerbose(int category, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogVerbose(category, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogDebug")]
        private unsafe static extern void INTERNAL_SDL_LogDebug(int category, byte* fmtAndArglist);

        public unsafe static void SDL_LogDebug(int category, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogDebug(category, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogInfo")]
        private unsafe static extern void INTERNAL_SDL_LogInfo(int category, byte* fmtAndArglist);

        public unsafe static void SDL_LogInfo(int category, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogInfo(category, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogWarn")]
        private unsafe static extern void INTERNAL_SDL_LogWarn(int category, byte* fmtAndArglist);

        public unsafe static void SDL_LogWarn(int category, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogWarn(category, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogError")]
        private unsafe static extern void INTERNAL_SDL_LogError(int category, byte* fmtAndArglist);

        public unsafe static void SDL_LogError(int category, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogError(category, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogCritical")]
        private unsafe static extern void INTERNAL_SDL_LogCritical(int category, byte* fmtAndArglist);

        public unsafe static void SDL_LogCritical(int category, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogCritical(category, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogMessage")]
        private unsafe static extern void INTERNAL_SDL_LogMessage(int category, SDL_LogPriority priority, byte* fmtAndArglist);

        public unsafe static void SDL_LogMessage(int category, SDL_LogPriority priority, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogMessage(category, priority, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LogMessageV")]
        private unsafe static extern void INTERNAL_SDL_LogMessageV(int category, SDL_LogPriority priority, byte* fmtAndArglist);

        public unsafe static void SDL_LogMessageV(int category, SDL_LogPriority priority, string fmtAndArglist)
        {
            int num = Utf8Size(fmtAndArglist);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_LogMessageV(category, priority, Utf8Encode(fmtAndArglist, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_LogPriority SDL_LogGetPriority(int category);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogSetPriority(int category, SDL_LogPriority priority);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogSetAllPriority(SDL_LogPriority priority);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogResetPriorities();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_LogGetOutputFunction(out IntPtr callback, out IntPtr userdata);

        public static void SDL_LogGetOutputFunction(out SDL_LogOutputFunction callback, out IntPtr userdata)
        {
            IntPtr callback2 = IntPtr.Zero;
            SDL_LogGetOutputFunction(out callback2, out userdata);
            if (callback2 != IntPtr.Zero)
            {
                callback = (SDL_LogOutputFunction)GetDelegateForFunctionPointer<SDL_LogOutputFunction>(callback2);
            }
            else
            {
                callback = null;
            }
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogSetOutputFunction(SDL_LogOutputFunction callback, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowMessageBox")]
        private static extern int INTERNAL_SDL_ShowMessageBox([In] ref INTERNAL_SDL_MessageBoxData messageboxdata, out int buttonid);

        private static IntPtr INTERNAL_AllocUTF8(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return IntPtr.Zero;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(str + "\0");
            IntPtr intPtr = SDL_malloc((IntPtr)bytes.Length);
            Marshal.Copy(bytes, 0, intPtr, bytes.Length);
            return intPtr;
        }

        public unsafe static int SDL_ShowMessageBox([In] ref SDL_MessageBoxData messageboxdata, out int buttonid)
        {
            INTERNAL_SDL_MessageBoxData iNTERNAL_SDL_MessageBoxData = default(INTERNAL_SDL_MessageBoxData);
            iNTERNAL_SDL_MessageBoxData.flags = messageboxdata.flags;
            iNTERNAL_SDL_MessageBoxData.window = messageboxdata.window;
            iNTERNAL_SDL_MessageBoxData.title = INTERNAL_AllocUTF8(messageboxdata.title);
            iNTERNAL_SDL_MessageBoxData.message = INTERNAL_AllocUTF8(messageboxdata.message);
            iNTERNAL_SDL_MessageBoxData.numbuttons = messageboxdata.numbuttons;
            INTERNAL_SDL_MessageBoxData messageboxdata2 = iNTERNAL_SDL_MessageBoxData;
            INTERNAL_SDL_MessageBoxButtonData[] array = new INTERNAL_SDL_MessageBoxButtonData[messageboxdata.numbuttons];
            for (int i = 0; i < messageboxdata.numbuttons; i++)
            {
                array[i] = new INTERNAL_SDL_MessageBoxButtonData
                {
                    flags = messageboxdata.buttons[i].flags,
                    buttonid = messageboxdata.buttons[i].buttonid,
                    text = INTERNAL_AllocUTF8(messageboxdata.buttons[i].text)
                };
            }
            if (messageboxdata.colorScheme.HasValue)
            {
                messageboxdata2.colorScheme = Marshal.AllocHGlobal(SizeOf<SDL_MessageBoxColorScheme>());
                Marshal.StructureToPtr((object)messageboxdata.colorScheme.Value, messageboxdata2.colorScheme, false);
            }
            int result;
            fixed (INTERNAL_SDL_MessageBoxButtonData* ptr = &array[0])
            {
                messageboxdata2.buttons = (IntPtr)ptr;
                result = INTERNAL_SDL_ShowMessageBox(ref messageboxdata2, out buttonid);
            }
            Marshal.FreeHGlobal(messageboxdata2.colorScheme);
            for (int j = 0; j < messageboxdata.numbuttons; j++)
            {
                SDL_free(array[j].text);
            }
            SDL_free(messageboxdata2.message);
            SDL_free(messageboxdata2.title);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowSimpleMessageBox")]
        private unsafe static extern int INTERNAL_SDL_ShowSimpleMessageBox(SDL_MessageBoxFlags flags, byte* title, byte* message, IntPtr window);

        public unsafe static int SDL_ShowSimpleMessageBox(SDL_MessageBoxFlags flags, string title, string message, IntPtr window)
        {
            int num = Utf8Size(title);
            byte* buffer = stackalloc byte[(int)(uint)num];
            int num2 = Utf8Size(message);
            byte* buffer2 = stackalloc byte[(int)(uint)num2];
            return INTERNAL_SDL_ShowSimpleMessageBox(flags, Utf8Encode(title, buffer, num), Utf8Encode(message, buffer2, num2), window);
        }

        public static void SDL_VERSION(out SDL_version x)
        {
            x.major = 2;
            x.minor = 0;
            x.patch = 22;
        }

        public static int SDL_VERSIONNUM(int X, int Y, int Z)
        {
            return X * 1000 + Y * 100 + Z;
        }

        public static bool SDL_VERSION_ATLEAST(int X, int Y, int Z)
        {
            return SDL_COMPILEDVERSION >= SDL_VERSIONNUM(X, Y, Z);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetVersion(out SDL_version ver);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetRevision")]
        private static extern IntPtr INTERNAL_SDL_GetRevision();

        public static string SDL_GetRevision()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetRevision());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRevisionNumber();

        public static int SDL_WINDOWPOS_UNDEFINED_DISPLAY(int X)
        {
            return 0x1FFF0000 | X;
        }

        public static bool SDL_WINDOWPOS_ISUNDEFINED(int X)
        {
            return (X & 0xFFFF0000u) == 536805376;
        }

        public static int SDL_WINDOWPOS_CENTERED_DISPLAY(int X)
        {
            return 0x2FFF0000 | X;
        }

        public static bool SDL_WINDOWPOS_ISCENTERED(int X)
        {
            return (X & 0xFFFF0000u) == 805240832;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateWindow")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateWindow(byte* title, int x, int y, int w, int h, SDL_WindowFlags flags);

        public unsafe static IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, SDL_WindowFlags flags)
        {
            int num = Utf8Size(title);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_CreateWindow(Utf8Encode(title, buffer, num), x, y, w, h, flags);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_CreateWindowAndRenderer(int width, int height, SDL_WindowFlags window_flags, out IntPtr window, out IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateWindowFrom(IntPtr data);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DisableScreenSaver();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_EnableScreenSaver();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetClosestDisplayMode(int displayIndex, ref SDL_DisplayMode mode, out SDL_DisplayMode closest);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetCurrentDisplayMode(int displayIndex, out SDL_DisplayMode mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentVideoDriver")]
        private static extern IntPtr INTERNAL_SDL_GetCurrentVideoDriver();

        public static string SDL_GetCurrentVideoDriver()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetCurrentVideoDriver());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDesktopDisplayMode(int displayIndex, out SDL_DisplayMode mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDisplayName")]
        private static extern IntPtr INTERNAL_SDL_GetDisplayName(int index);

        public static string SDL_GetDisplayName(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetDisplayName(index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayBounds(int displayIndex, out SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayDPI(int displayIndex, out float ddpi, out float hdpi, out float vdpi);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_DisplayOrientation SDL_GetDisplayOrientation(int displayIndex);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayMode(int displayIndex, int modeIndex, out SDL_DisplayMode mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayUsableBounds(int displayIndex, out SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumDisplayModes(int displayIndex);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumVideoDisplays();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumVideoDrivers();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetVideoDriver")]
        private static extern IntPtr INTERNAL_SDL_GetVideoDriver(int index);

        public static string SDL_GetVideoDriver(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetVideoDriver(index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetWindowBrightness(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowOpacity(IntPtr window, float opacity);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowOpacity(IntPtr window, out float out_opacity);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowModalFor(IntPtr modal_window, IntPtr parent_window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowInputFocus(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowData")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetWindowData(IntPtr window, byte* name);

        public unsafe static IntPtr SDL_GetWindowData(IntPtr window, string name)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GetWindowData(window, Utf8Encode(name, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowDisplayIndex(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowDisplayMode(IntPtr window, out SDL_DisplayMode mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowICCProfile(IntPtr window, out IntPtr mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowFlags(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowFromID(uint id);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowGammaRamp(IntPtr window, [Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] red, [Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] green, [Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] blue);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowGrab(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowKeyboardGrab(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowMouseGrab(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowID(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowPixelFormat(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowMaximumSize(IntPtr window, out int max_w, out int max_h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowMinimumSize(IntPtr window, out int min_w, out int min_h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowPosition(IntPtr window, out int x, out int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowSize(IntPtr window, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowSizeInPixels(IntPtr window, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowSurface(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowTitle")]
        private static extern IntPtr INTERNAL_SDL_GetWindowTitle(IntPtr window);

        public static string SDL_GetWindowTitle(IntPtr window)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetWindowTitle(window));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_BindTexture(IntPtr texture, out float texw, out float texh);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_CreateContext(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_DeleteContext(IntPtr context);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_LoadLibrary")]
        private unsafe static extern int INTERNAL_SDL_GL_LoadLibrary(byte* path);

        public unsafe static int SDL_GL_LoadLibrary(string path)
        {
            byte* intPtr = Utf8EncodeHeap(path);
            int result = INTERNAL_SDL_GL_LoadLibrary(intPtr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetProcAddress(IntPtr proc);

        public unsafe static IntPtr SDL_GL_GetProcAddress(string proc)
        {
            int num = Utf8Size(proc);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return SDL_GL_GetProcAddress((IntPtr)Utf8Encode(proc, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_UnloadLibrary();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_ExtensionSupported")]
        private unsafe static extern SDL_bool INTERNAL_SDL_GL_ExtensionSupported(byte* extension);

        public unsafe static SDL_bool SDL_GL_ExtensionSupported(string extension)
        {
            int num = Utf8Size(extension);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GL_ExtensionSupported(Utf8Encode(extension, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_ResetAttributes();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_GetAttribute(SDL_GLattr attr, out int value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_GetSwapInterval();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_MakeCurrent(IntPtr window, IntPtr context);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetCurrentWindow();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetCurrentContext();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_GetDrawableSize(IntPtr window, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_SetAttribute(SDL_GLattr attr, int value);

        public static int SDL_GL_SetAttribute(SDL_GLattr attr, SDL_GLprofile profile)
        {
            return SDL_GL_SetAttribute(attr, (int)profile);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_SetSwapInterval(int interval);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_SwapWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_UnbindTexture(IntPtr texture);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HideWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsScreenSaverEnabled();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MaximizeWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MinimizeWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RaiseWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RestoreWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowBrightness(IntPtr window, float brightness);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowData")]
        private unsafe static extern IntPtr INTERNAL_SDL_SetWindowData(IntPtr window, byte* name, IntPtr userdata);

        public unsafe static IntPtr SDL_SetWindowData(IntPtr window, string name, IntPtr userdata)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_SetWindowData(window, Utf8Encode(name, buffer, num), userdata);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowDisplayMode(IntPtr window, ref SDL_DisplayMode mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowDisplayMode(IntPtr window, IntPtr mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowFullscreen(IntPtr window, uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowGammaRamp(IntPtr window, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] red, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] green, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] blue);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowGrab(IntPtr window, SDL_bool grabbed);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowKeyboardGrab(IntPtr window, SDL_bool grabbed);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMouseGrab(IntPtr window, SDL_bool grabbed);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowIcon(IntPtr window, IntPtr icon);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMaximumSize(IntPtr window, int max_w, int max_h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMinimumSize(IntPtr window, int min_w, int min_h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowPosition(IntPtr window, int x, int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowSize(IntPtr window, int w, int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowBordered(IntPtr window, SDL_bool bordered);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowBordersSize(IntPtr window, out int top, out int left, out int bottom, out int right);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowResizable(IntPtr window, SDL_bool resizable);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowAlwaysOnTop(IntPtr window, SDL_bool on_top);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowTitle")]
        private unsafe static extern void INTERNAL_SDL_SetWindowTitle(IntPtr window, byte* title);

        public unsafe static void SDL_SetWindowTitle(IntPtr window, string title)
        {
            int num = Utf8Size(title);
            byte* buffer = stackalloc byte[(int)(uint)num];
            INTERNAL_SDL_SetWindowTitle(window, Utf8Encode(title, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ShowWindow(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateWindowSurface(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateWindowSurfaceRects(IntPtr window, [In] SDL_Rect[] rects, int numrects);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_VideoInit")]
        private unsafe static extern int INTERNAL_SDL_VideoInit(byte* driver_name);

        public unsafe static int SDL_VideoInit(string driver_name)
        {
            int num = Utf8Size(driver_name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_VideoInit(Utf8Encode(driver_name, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_VideoQuit();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowHitTest(IntPtr window, SDL_HitTest callback, IntPtr callback_data);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetGrabbedWindow();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowMouseRect(IntPtr window, ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowMouseRect(IntPtr window, IntPtr rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetWindowMouseRect(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FlashWindow(IntPtr window, SDL_FlashOperation operation);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_BlendMode SDL_ComposeCustomBlendMode(SDL_BlendFactor srcColorFactor, SDL_BlendFactor dstColorFactor, SDL_BlendOperation colorOperation, SDL_BlendFactor srcAlphaFactor, SDL_BlendFactor dstAlphaFactor, SDL_BlendOperation alphaOperation);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_Vulkan_LoadLibrary")]
        private unsafe static extern int INTERNAL_SDL_Vulkan_LoadLibrary(byte* path);

        public unsafe static int SDL_Vulkan_LoadLibrary(string path)
        {
            byte* intPtr = Utf8EncodeHeap(path);
            int result = INTERNAL_SDL_Vulkan_LoadLibrary(intPtr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_Vulkan_GetVkGetInstanceProcAddr();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Vulkan_UnloadLibrary();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Vulkan_GetInstanceExtensions(IntPtr window, out uint pCount, IntPtr pNames);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Vulkan_GetInstanceExtensions(IntPtr window, out uint pCount, IntPtr[] pNames);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Vulkan_CreateSurface(IntPtr window, IntPtr instance, out ulong surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Vulkan_GetDrawableSize(IntPtr window, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_Metal_CreateView(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Metal_DestroyView(IntPtr view);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_Metal_GetLayer(IntPtr view);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Metal_GetDrawableSize(IntPtr window, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRenderer(IntPtr window, int index, SDL_RendererFlags flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSoftwareRenderer(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyRenderer(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyTexture(IntPtr texture);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumRenderDrivers();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRenderDrawBlendMode(IntPtr renderer, out SDL_BlendMode blendMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureScaleMode(IntPtr texture, SDL_ScaleMode scaleMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureScaleMode(IntPtr texture, out SDL_ScaleMode scaleMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureUserData(IntPtr texture, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTextureUserData(IntPtr texture);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRenderDrawColor(IntPtr renderer, out byte r, out byte g, out byte b, out byte a);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRenderDriverInfo(int index, out SDL_RendererInfo info);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderer(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRendererInfo(IntPtr renderer, out SDL_RendererInfo info);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRendererOutputSize(IntPtr renderer, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureAlphaMod(IntPtr texture, out byte alpha);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureBlendMode(IntPtr texture, out SDL_BlendMode blendMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureColorMod(IntPtr texture, out byte r, out byte g, out byte b);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTexture(IntPtr texture, ref SDL_Rect rect, out IntPtr pixels, out int pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTexture(IntPtr texture, IntPtr rect, out IntPtr pixels, out int pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTextureToSurface(IntPtr texture, ref SDL_Rect rect, out IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTextureToSurface(IntPtr texture, IntPtr rect, out IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_QueryTexture(IntPtr texture, out uint format, out int access, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderClear(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_Rect dstrect, double angle, ref SDL_Point center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref SDL_Rect dstrect, double angle, ref SDL_Point center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, IntPtr dstrect, double angle, ref SDL_Point center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_Rect dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect, double angle, ref SDL_Point center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref SDL_Rect dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, IntPtr dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLine(IntPtr renderer, int x1, int y1, int x2, int y2);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLines(IntPtr renderer, [In] SDL_Point[] points, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPoint(IntPtr renderer, int x, int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPoints(IntPtr renderer, [In] SDL_Point[] points, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRect(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRect(IntPtr renderer, IntPtr rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRects(IntPtr renderer, [In] SDL_Rect[] rects, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRect(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRect(IntPtr renderer, IntPtr rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRects(IntPtr renderer, [In] SDL_Rect[] rects, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_FRect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref SDL_FRect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_FRect dstrect, double angle, ref SDL_FPoint center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref SDL_FRect dstrect, double angle, ref SDL_FPoint center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, IntPtr dstrect, double angle, ref SDL_FPoint center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_FRect dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect, double angle, ref SDL_FPoint center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref SDL_FRect dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, IntPtr dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect, double angle, IntPtr center, SDL_RendererFlip flip);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderGeometry(IntPtr renderer, IntPtr texture, [In] SDL_Vertex[] vertices, int num_vertices, [In] int[] indices, int num_indices);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderGeometryRaw(IntPtr renderer, IntPtr texture, [In] float[] xy, int xy_stride, [In] int[] color, int color_stride, [In] float[] uv, int uv_stride, int num_vertices, IntPtr indices, int num_indices, int size_indices);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPointF(IntPtr renderer, float x, float y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPointsF(IntPtr renderer, [In] SDL_FPoint[] points, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLineF(IntPtr renderer, float x1, float y1, float x2, float y2);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLinesF(IntPtr renderer, [In] SDL_FPoint[] points, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRectF(IntPtr renderer, ref SDL_FRect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRectF(IntPtr renderer, IntPtr rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRectsF(IntPtr renderer, [In] SDL_FRect[] rects, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRectF(IntPtr renderer, ref SDL_FRect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRectF(IntPtr renderer, IntPtr rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRectsF(IntPtr renderer, [In] SDL_FRect[] rects, int count);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderGetClipRect(IntPtr renderer, out SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderGetLogicalSize(IntPtr renderer, out int w, out int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderGetScale(IntPtr renderer, out float scaleX, out float scaleY);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderWindowToLogical(IntPtr renderer, int windowX, int windowY, out float logicalX, out float logicalY);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderLogicalToWindow(IntPtr renderer, float logicalX, float logicalY, out int windowX, out int windowY);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderGetViewport(IntPtr renderer, out SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderPresent(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderReadPixels(IntPtr renderer, ref SDL_Rect rect, uint format, IntPtr pixels, int pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetClipRect(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetClipRect(IntPtr renderer, IntPtr rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetLogicalSize(IntPtr renderer, int w, int h);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetScale(IntPtr renderer, float scaleX, float scaleY);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetIntegerScale(IntPtr renderer, SDL_bool enable);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetViewport(IntPtr renderer, ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRenderDrawBlendMode(IntPtr renderer, SDL_BlendMode blendMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRenderTarget(IntPtr renderer, IntPtr texture);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureAlphaMod(IntPtr texture, byte alpha);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureBlendMode(IntPtr texture, SDL_BlendMode blendMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureColorMod(IntPtr texture, byte r, byte g, byte b);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockTexture(IntPtr texture);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateTexture(IntPtr texture, ref SDL_Rect rect, IntPtr pixels, int pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateTexture(IntPtr texture, IntPtr rect, IntPtr pixels, int pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateYUVTexture(IntPtr texture, ref SDL_Rect rect, IntPtr yPlane, int yPitch, IntPtr uPlane, int uPitch, IntPtr vPlane, int vPitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateNVTexture(IntPtr texture, ref SDL_Rect rect, IntPtr yPlane, int yPitch, IntPtr uvPlane, int uvPitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_RenderTargetSupported(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetRenderTarget(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderGetMetalLayer(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderGetMetalCommandEncoder(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetVSync(IntPtr renderer, int vsync);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_RenderIsClipEnabled(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFlush(IntPtr renderer);

        public static uint SDL_DEFINE_PIXELFOURCC(byte A, byte B, byte C, byte D)
        {
            return SDL_FOURCC(A, B, C, D);
        }

        public static uint SDL_DEFINE_PIXELFORMAT(SDL_PixelType type, uint order, SDL_PackedLayout layout, byte bits, byte bytes)
        {
            return (uint)(0x10000000 | ((byte)type << 24) | ((byte)order << 20) | ((byte)layout << 16) | (bits << 8) | bytes);
        }

        public static byte SDL_PIXELFLAG(uint X)
        {
            return (byte)((X >> 28) & 0xF);
        }

        public static byte SDL_PIXELTYPE(uint X)
        {
            return (byte)((X >> 24) & 0xF);
        }

        public static byte SDL_PIXELORDER(uint X)
        {
            return (byte)((X >> 20) & 0xF);
        }

        public static byte SDL_PIXELLAYOUT(uint X)
        {
            return (byte)((X >> 16) & 0xF);
        }

        public static byte SDL_BITSPERPIXEL(uint X)
        {
            return (byte)((X >> 8) & 0xFF);
        }

        public static byte SDL_BYTESPERPIXEL(uint X)
        {
            if (SDL_ISPIXELFORMAT_FOURCC(X))
            {
                if (X == SDL_PIXELFORMAT_YUY2 || X == SDL_PIXELFORMAT_UYVY || X == SDL_PIXELFORMAT_YVYU)
                {
                    return 2;
                }
                return 1;
            }
            return (byte)(X & 0xFF);
        }

        public static bool SDL_ISPIXELFORMAT_INDEXED(uint format)
        {
            if (SDL_ISPIXELFORMAT_FOURCC(format))
            {
                return false;
            }
            SDL_PixelType sDL_PixelType = (SDL_PixelType)SDL_PIXELTYPE(format);
            if (sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_INDEX1 && sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_INDEX4)
            {
                return sDL_PixelType == SDL_PixelType.SDL_PIXELTYPE_INDEX8;
            }
            return true;
        }

        public static bool SDL_ISPIXELFORMAT_PACKED(uint format)
        {
            if (SDL_ISPIXELFORMAT_FOURCC(format))
            {
                return false;
            }
            SDL_PixelType sDL_PixelType = (SDL_PixelType)SDL_PIXELTYPE(format);
            if (sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_PACKED8 && sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_PACKED16)
            {
                return sDL_PixelType == SDL_PixelType.SDL_PIXELTYPE_PACKED32;
            }
            return true;
        }

        public static bool SDL_ISPIXELFORMAT_ARRAY(uint format)
        {
            if (SDL_ISPIXELFORMAT_FOURCC(format))
            {
                return false;
            }
            SDL_PixelType sDL_PixelType = (SDL_PixelType)SDL_PIXELTYPE(format);
            if (sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_ARRAYU8 && sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_ARRAYU16 && sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_ARRAYU32 && sDL_PixelType != SDL_PixelType.SDL_PIXELTYPE_ARRAYF16)
            {
                return sDL_PixelType == SDL_PixelType.SDL_PIXELTYPE_ARRAYF32;
            }
            return true;
        }

        public static bool SDL_ISPIXELFORMAT_ALPHA(uint format)
        {
            if (SDL_ISPIXELFORMAT_PACKED(format))
            {
                SDL_PackedOrder sDL_PackedOrder = (SDL_PackedOrder)SDL_PIXELORDER(format);
                if (sDL_PackedOrder != SDL_PackedOrder.SDL_PACKEDORDER_ARGB && sDL_PackedOrder != SDL_PackedOrder.SDL_PACKEDORDER_RGBA && sDL_PackedOrder != SDL_PackedOrder.SDL_PACKEDORDER_ABGR)
                {
                    return sDL_PackedOrder == SDL_PackedOrder.SDL_PACKEDORDER_BGRA;
                }
                return true;
            }
            if (SDL_ISPIXELFORMAT_ARRAY(format))
            {
                SDL_ArrayOrder sDL_ArrayOrder = (SDL_ArrayOrder)SDL_PIXELORDER(format);
                if (sDL_ArrayOrder != SDL_ArrayOrder.SDL_ARRAYORDER_ARGB && sDL_ArrayOrder != SDL_ArrayOrder.SDL_ARRAYORDER_RGBA && sDL_ArrayOrder != SDL_ArrayOrder.SDL_ARRAYORDER_ABGR)
                {
                    return sDL_ArrayOrder == SDL_ArrayOrder.SDL_ARRAYORDER_BGRA;
                }
                return true;
            }
            return false;
        }

        public static bool SDL_ISPIXELFORMAT_FOURCC(uint format)
        {
            if (format == 0)
            {
                return SDL_PIXELFLAG(format) != 1;
            }
            return false;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AllocFormat(uint pixel_format);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AllocPalette(int ncolors);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CalculateGammaRamp(float gamma, [Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] ushort[] ramp);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeFormat(IntPtr format);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreePalette(IntPtr palette);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPixelFormatName")]
        private static extern IntPtr INTERNAL_SDL_GetPixelFormatName(uint format);

        public static string SDL_GetPixelFormatName(uint format)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetPixelFormatName(format));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetRGB(uint pixel, IntPtr format, out byte r, out byte g, out byte b);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetRGBA(uint pixel, IntPtr format, out byte r, out byte g, out byte b, out byte a);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MapRGB(IntPtr format, byte r, byte g, byte b);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MapRGBA(IntPtr format, byte r, byte g, byte b, byte a);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_MasksToPixelFormatEnum(int bpp, uint Rmask, uint Gmask, uint Bmask, uint Amask);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_PixelFormatEnumToMasks(uint format, out int bpp, out uint Rmask, out uint Gmask, out uint Bmask, out uint Amask);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetPaletteColors(IntPtr palette, [In] SDL_Color[] colors, int firstcolor, int ncolors);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetPixelFormatPalette(IntPtr format, IntPtr palette);

        public static SDL_bool SDL_PointInRect(ref SDL_Point p, ref SDL_Rect r)
        {
            if (p.x < r.x || p.x >= r.x + r.w || p.y < r.y || p.y >= r.y + r.h)
            {
                return SDL_bool.SDL_FALSE;
            }
            return SDL_bool.SDL_TRUE;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_EnclosePoints([In] SDL_Point[] points, int count, ref SDL_Rect clip, out SDL_Rect result);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasIntersection(ref SDL_Rect A, ref SDL_Rect B);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IntersectRect(ref SDL_Rect A, ref SDL_Rect B, out SDL_Rect result);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IntersectRectAndLine(ref SDL_Rect rect, ref int X1, ref int Y1, ref int X2, ref int Y2);

        public static SDL_bool SDL_RectEmpty(ref SDL_Rect r)
        {
            if (r.w > 0 && r.h > 0)
            {
                return SDL_bool.SDL_FALSE;
            }
            return SDL_bool.SDL_TRUE;
        }

        public static SDL_bool SDL_RectEquals(ref SDL_Rect a, ref SDL_Rect b)
        {
            if (a.x != b.x || a.y != b.y || a.w != b.w || a.h != b.h)
            {
                return SDL_bool.SDL_FALSE;
            }
            return SDL_bool.SDL_TRUE;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnionRect(ref SDL_Rect A, ref SDL_Rect B, out SDL_Rect result);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateShapedWindow")]
        private unsafe static extern IntPtr INTERNAL_SDL_CreateShapedWindow(byte* title, uint x, uint y, uint w, uint h, SDL_WindowFlags flags);

        public unsafe static IntPtr SDL_CreateShapedWindow(string title, uint x, uint y, uint w, uint h, SDL_WindowFlags flags)
        {
            byte* ptr = Utf8EncodeHeap(title);
            IntPtr result = INTERNAL_SDL_CreateShapedWindow(ptr, x, y, w, h, flags);
            Marshal.FreeHGlobal((IntPtr)ptr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsShapedWindow(IntPtr window);

        public static bool SDL_SHAPEMODEALPHA(WindowShapeMode mode)
        {
            if ((uint)mode <= 2u)
            {
                return true;
            }
            return false;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowShape(IntPtr window, IntPtr shape, ref SDL_WindowShapeMode shape_mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetShapedWindowMode(IntPtr window, out SDL_WindowShapeMode shape_mode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetShapedWindowMode(IntPtr window, IntPtr shape_mode);

        public static bool SDL_MUSTLOCK(IntPtr surface)
        {
            return (PtrToStructure<SDL_Surface>(surface).flags & 2) != 0;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlit")]
        public static extern int SDL_BlitSurface(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlit")]
        public static extern int SDL_BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlit")]
        public static extern int SDL_BlitSurface(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlit")]
        public static extern int SDL_BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlitScaled")]
        public static extern int SDL_BlitScaled(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlitScaled")]
        public static extern int SDL_BlitScaled(IntPtr src, IntPtr srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlitScaled")]
        public static extern int SDL_BlitScaled(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlitScaled")]
        public static extern int SDL_BlitScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_ConvertPixels(int width, int height, uint src_format, IntPtr src, int src_pitch, uint dst_format, IntPtr dst, int dst_pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PremultiplyAlpha(int width, int height, uint src_format, IntPtr src, int src_pitch, uint dst_format, IntPtr dst, int dst_pitch);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_ConvertSurface(IntPtr src, IntPtr fmt, uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_ConvertSurfaceFormat(IntPtr src, uint pixel_format, uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRGBSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint Rmask, uint Gmask, uint Bmask, uint Amask);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRGBSurfaceWithFormat(uint flags, int width, int height, int depth, uint format);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRGBSurfaceWithFormatFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint format);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FillRect(IntPtr dst, ref SDL_Rect rect, uint color);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FillRect(IntPtr dst, IntPtr rect, uint color);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FillRects(IntPtr dst, [In] SDL_Rect[] rects, int count, uint color);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeSurface(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetClipRect(IntPtr surface, out SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasColorKey(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetColorKey(IntPtr surface, out uint key);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSurfaceAlphaMod(IntPtr surface, out byte alpha);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSurfaceBlendMode(IntPtr surface, out SDL_BlendMode blendMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSurfaceColorMod(IntPtr surface, out byte r, out byte g, out byte b);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_LoadBMP_RW(IntPtr src, int freesrc);

        public static IntPtr SDL_LoadBMP(string file)
        {
            return SDL_LoadBMP_RW(SDL_RWFromFile(file, "rb"), 1);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockSurface(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LowerBlit(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LowerBlitScaled(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SaveBMP_RW(IntPtr surface, IntPtr src, int freesrc);

        public static int SDL_SaveBMP(IntPtr surface, string file)
        {
            IntPtr src = SDL_RWFromFile(file, "wb");
            return SDL_SaveBMP_RW(surface, src, 1);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_SetClipRect(IntPtr surface, ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetColorKey(IntPtr surface, int flag, uint key);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceAlphaMod(IntPtr surface, byte alpha);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceBlendMode(IntPtr surface, SDL_BlendMode blendMode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceColorMod(IntPtr surface, byte r, byte g, byte b);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfacePalette(IntPtr surface, IntPtr palette);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceRLE(IntPtr surface, int flag);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSurfaceRLE(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SoftStretch(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SoftStretchLinear(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSurface(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpperBlit(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpperBlitScaled(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_DuplicateSurface(IntPtr surface);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasClipboardText();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetClipboardText")]
        private static extern IntPtr INTERNAL_SDL_GetClipboardText();

        public static string SDL_GetClipboardText()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetClipboardText(), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetClipboardText")]
        private unsafe static extern int INTERNAL_SDL_SetClipboardText(byte* text);

        public unsafe static int SDL_SetClipboardText(string text)
        {
            byte* intPtr = Utf8EncodeHeap(text);
            int result = INTERNAL_SDL_SetClipboardText(intPtr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PumpEvents();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PeepEvents([Out] SDL_Event[] events, int numevents, SDL_eventaction action, SDL_EventType minType, SDL_EventType maxType);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int SDL_PeepEvents(SDL_Event* events, int numevents, SDL_eventaction action, SDL_EventType minType, SDL_EventType maxType);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasEvent(SDL_EventType type);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasEvents(SDL_EventType minType, SDL_EventType maxType);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FlushEvent(SDL_EventType type);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FlushEvents(SDL_EventType min, SDL_EventType max);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PollEvent(out SDL_Event _event);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_WaitEvent(out SDL_Event _event);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_WaitEventTimeout(out SDL_Event _event, int timeout);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PushEvent(ref SDL_Event _event);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetEventFilter(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_bool SDL_GetEventFilter(out IntPtr filter, out IntPtr userdata);

        public static SDL_bool SDL_GetEventFilter(out SDL_EventFilter filter, out IntPtr userdata)
        {
            IntPtr filter2 = IntPtr.Zero;
            SDL_bool result = SDL_GetEventFilter(out filter2, out userdata);
            if (filter2 != IntPtr.Zero)
            {
                filter = (SDL_EventFilter)GetDelegateForFunctionPointer<SDL_EventFilter>(filter2);
                return result;
            }
            filter = null;
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_AddEventWatch(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DelEventWatch(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FilterEvents(SDL_EventFilter filter, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_EventState(SDL_EventType type, int state);

        public static byte SDL_GetEventState(SDL_EventType type)
        {
            return SDL_EventState(type, -1);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_RegisterEvents(int numevents);

        public static SDL_Keycode SDL_SCANCODE_TO_KEYCODE(SDL_Scancode X)
        {
            return (SDL_Keycode)(X | (SDL_Scancode)1073741824);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardFocus();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardState(out int numkeys);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Keymod SDL_GetModState();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetModState(SDL_Keymod modstate);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Keycode SDL_GetKeyFromScancode(SDL_Scancode scancode);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode key);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetScancodeName")]
        private static extern IntPtr INTERNAL_SDL_GetScancodeName(SDL_Scancode scancode);

        public static string SDL_GetScancodeName(SDL_Scancode scancode)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetScancodeName(scancode));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetScancodeFromName")]
        private unsafe static extern SDL_Scancode INTERNAL_SDL_GetScancodeFromName(byte* name);

        public unsafe static SDL_Scancode SDL_GetScancodeFromName(string name)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GetScancodeFromName(Utf8Encode(name, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetKeyName")]
        private static extern IntPtr INTERNAL_SDL_GetKeyName(SDL_Keycode key);

        public static string SDL_GetKeyName(SDL_Keycode key)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetKeyName(key));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetKeyFromName")]
        private unsafe static extern SDL_Keycode INTERNAL_SDL_GetKeyFromName(byte* name);

        public unsafe static SDL_Keycode SDL_GetKeyFromName(string name)
        {
            int num = Utf8Size(name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GetKeyFromName(Utf8Encode(name, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_StartTextInput();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsTextInputActive();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_StopTextInput();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClearComposition();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsTextInputShown();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTextInputRect(ref SDL_Rect rect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasScreenKeyboardSupport();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsScreenKeyboardShown(IntPtr window);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetMouseFocus();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetMouseState(out int x, out int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetMouseState(IntPtr x, out int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetMouseState(out int x, IntPtr y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetMouseState(IntPtr x, IntPtr y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGlobalMouseState(out int x, out int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGlobalMouseState(IntPtr x, out int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGlobalMouseState(out int x, IntPtr y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetGlobalMouseState(IntPtr x, IntPtr y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetRelativeMouseState(out int x, out int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_WarpMouseInWindow(IntPtr window, int x, int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_WarpMouseGlobal(int x, int y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRelativeMouseMode(SDL_bool enabled);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_CaptureMouse(SDL_bool enabled);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetRelativeMouseMode();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateCursor(IntPtr data, IntPtr mask, int w, int h, int hot_x, int hot_y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateColorCursor(IntPtr surface, int hot_x, int hot_y);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSystemCursor(SDL_SystemCursor id);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetCursor(IntPtr cursor);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetCursor();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeCursor(IntPtr cursor);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_ShowCursor(int toggle);

        public static uint SDL_BUTTON(uint X)
        {
            return (uint)(1 << (int)(X - 1));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumTouchDevices();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_GetTouchDevice(int index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumTouchFingers(long touchID);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTouchFinger(long touchID, int index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_TouchDeviceType SDL_GetTouchDeviceType(long touchID);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetTouchName")]
        private static extern IntPtr INTERNAL_SDL_GetTouchName(int index);

        public static string SDL_GetTouchName(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetTouchName(index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickRumble(IntPtr joystick, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickRumbleTriggers(IntPtr joystick, ushort left_rumble, ushort right_rumble, uint duration_ms);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_JoystickClose(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickEventState(int state);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SDL_JoystickGetAxis(IntPtr joystick, int axis);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_JoystickGetAxisInitialState(IntPtr joystick, int axis, out short state);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickGetBall(IntPtr joystick, int ball, out int dx, out int dy);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_JoystickGetButton(IntPtr joystick, int button);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_JoystickGetHat(IntPtr joystick, int hat);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_JoystickName")]
        private static extern IntPtr INTERNAL_SDL_JoystickName(IntPtr joystick);

        public static string SDL_JoystickName(IntPtr joystick)
        {
            return UTF8_ToManaged(INTERNAL_SDL_JoystickName(joystick));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_JoystickNameForIndex")]
        private static extern IntPtr INTERNAL_SDL_JoystickNameForIndex(int device_index);

        public static string SDL_JoystickNameForIndex(int device_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_JoystickNameForIndex(device_index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickNumAxes(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickNumBalls(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickNumButtons(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickNumHats(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_JoystickOpen(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_JoystickUpdate();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_NumJoysticks();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Guid SDL_JoystickGetDeviceGUID(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Guid SDL_JoystickGetGUID(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_JoystickGetGUIDString(Guid guid, byte[] pszGUID, int cbGUID);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_JoystickGetGUIDFromString")]
        private unsafe static extern Guid INTERNAL_SDL_JoystickGetGUIDFromString(byte* pchGUID);

        public unsafe static Guid SDL_JoystickGetGUIDFromString(string pchGuid)
        {
            int num = Utf8Size(pchGuid);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_JoystickGetGUIDFromString(Utf8Encode(pchGuid, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_JoystickGetDeviceVendor(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_JoystickGetDeviceProduct(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_JoystickGetDeviceProductVersion(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickType SDL_JoystickGetDeviceType(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickGetDeviceInstanceID(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_JoystickGetVendor(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_JoystickGetProduct(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_JoystickGetProductVersion(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_JoystickGetSerial")]
        private static extern IntPtr INTERNAL_SDL_JoystickGetSerial(IntPtr joystick);

        public static string SDL_JoystickGetSerial(IntPtr joystick)
        {
            return UTF8_ToManaged(INTERNAL_SDL_JoystickGetSerial(joystick));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickType SDL_JoystickGetType(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_JoystickGetAttached(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickInstanceID(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickPowerLevel SDL_JoystickCurrentPowerLevel(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_JoystickFromInstanceID(int instance_id);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockJoysticks();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockJoysticks();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_JoystickFromPlayerIndex(int player_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_JoystickSetPlayerIndex(IntPtr joystick, int player_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickAttachVirtual(int type, int naxes, int nbuttons, int nhats);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickDetachVirtual(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_JoystickIsVirtual(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickSetVirtualAxis(IntPtr joystick, int axis, short value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickSetVirtualButton(IntPtr joystick, int button, byte value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickSetVirtualHat(IntPtr joystick, int hat, byte value);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_JoystickHasLED(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_JoystickHasRumble(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_JoystickHasRumbleTriggers(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickSetLED(IntPtr joystick, byte red, byte green, byte blue);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickSendEffect(IntPtr joystick, IntPtr data, int size);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerAddMapping")]
        private unsafe static extern int INTERNAL_SDL_GameControllerAddMapping(byte* mappingString);

        public unsafe static int SDL_GameControllerAddMapping(string mappingString)
        {
            byte* intPtr = Utf8EncodeHeap(mappingString);
            int result = INTERNAL_SDL_GameControllerAddMapping(intPtr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerNumMappings();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerMappingForIndex")]
        private static extern IntPtr INTERNAL_SDL_GameControllerMappingForIndex(int mapping_index);

        public static string SDL_GameControllerMappingForIndex(int mapping_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerMappingForIndex(mapping_index), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerAddMappingsFromRW")]
        private static extern int INTERNAL_SDL_GameControllerAddMappingsFromRW(IntPtr rw, int freerw);

        public static int SDL_GameControllerAddMappingsFromFile(string file)
        {
            return INTERNAL_SDL_GameControllerAddMappingsFromRW(SDL_RWFromFile(file, "rb"), 1);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerMappingForGUID")]
        private static extern IntPtr INTERNAL_SDL_GameControllerMappingForGUID(Guid guid);

        public static string SDL_GameControllerMappingForGUID(Guid guid)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerMappingForGUID(guid), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerMapping")]
        private static extern IntPtr INTERNAL_SDL_GameControllerMapping(IntPtr gamecontroller);

        public static string SDL_GameControllerMapping(IntPtr gamecontroller)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerMapping(gamecontroller), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsGameController(int joystick_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerNameForIndex")]
        private static extern IntPtr INTERNAL_SDL_GameControllerNameForIndex(int joystick_index);

        public static string SDL_GameControllerNameForIndex(int joystick_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerNameForIndex(joystick_index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerMappingForDeviceIndex")]
        private static extern IntPtr INTERNAL_SDL_GameControllerMappingForDeviceIndex(int joystick_index);

        public static string SDL_GameControllerMappingForDeviceIndex(int joystick_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerMappingForDeviceIndex(joystick_index), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerOpen(int joystick_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerName")]
        private static extern IntPtr INTERNAL_SDL_GameControllerName(IntPtr gamecontroller);

        public static string SDL_GameControllerName(IntPtr gamecontroller)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerName(gamecontroller));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GameControllerGetVendor(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GameControllerGetProduct(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort SDL_GameControllerGetProductVersion(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetSerial")]
        private static extern IntPtr INTERNAL_SDL_GameControllerGetSerial(IntPtr gamecontroller);

        public static string SDL_GameControllerGetSerial(IntPtr gamecontroller)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerGetSerial(gamecontroller));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerGetAttached(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerEventState(int state);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GameControllerUpdate();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetAxisFromString")]
        private unsafe static extern SDL_GameControllerAxis INTERNAL_SDL_GameControllerGetAxisFromString(byte* pchString);

        public unsafe static SDL_GameControllerAxis SDL_GameControllerGetAxisFromString(string pchString)
        {
            int num = Utf8Size(pchString);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GameControllerGetAxisFromString(Utf8Encode(pchString, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetStringForAxis")]
        private static extern IntPtr INTERNAL_SDL_GameControllerGetStringForAxis(SDL_GameControllerAxis axis);

        public static string SDL_GameControllerGetStringForAxis(SDL_GameControllerAxis axis)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerGetStringForAxis(axis));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetBindForAxis")]
        private static extern INTERNAL_SDL_GameControllerButtonBind INTERNAL_SDL_GameControllerGetBindForAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis);

        public static SDL_GameControllerButtonBind SDL_GameControllerGetBindForAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis)
        {
            INTERNAL_SDL_GameControllerButtonBind iNTERNAL_SDL_GameControllerButtonBind = INTERNAL_SDL_GameControllerGetBindForAxis(gamecontroller, axis);
            SDL_GameControllerButtonBind result = default(SDL_GameControllerButtonBind);
            result.bindType = (SDL_GameControllerBindType)iNTERNAL_SDL_GameControllerButtonBind.bindType;
            result.value.hat.hat = iNTERNAL_SDL_GameControllerButtonBind.unionVal0;
            result.value.hat.hat_mask = iNTERNAL_SDL_GameControllerButtonBind.unionVal1;
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SDL_GameControllerGetAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetButtonFromString")]
        private unsafe static extern SDL_GameControllerButton INTERNAL_SDL_GameControllerGetButtonFromString(byte* pchString);

        public unsafe static SDL_GameControllerButton SDL_GameControllerGetButtonFromString(string pchString)
        {
            int num = Utf8Size(pchString);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_GameControllerGetButtonFromString(Utf8Encode(pchString, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetStringForButton")]
        private static extern IntPtr INTERNAL_SDL_GameControllerGetStringForButton(SDL_GameControllerButton button);

        public static string SDL_GameControllerGetStringForButton(SDL_GameControllerButton button)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerGetStringForButton(button));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetBindForButton")]
        private static extern INTERNAL_SDL_GameControllerButtonBind INTERNAL_SDL_GameControllerGetBindForButton(IntPtr gamecontroller, SDL_GameControllerButton button);

        public static SDL_GameControllerButtonBind SDL_GameControllerGetBindForButton(IntPtr gamecontroller, SDL_GameControllerButton button)
        {
            INTERNAL_SDL_GameControllerButtonBind iNTERNAL_SDL_GameControllerButtonBind = INTERNAL_SDL_GameControllerGetBindForButton(gamecontroller, button);
            SDL_GameControllerButtonBind result = default(SDL_GameControllerButtonBind);
            result.bindType = (SDL_GameControllerBindType)iNTERNAL_SDL_GameControllerButtonBind.bindType;
            result.value.hat.hat = iNTERNAL_SDL_GameControllerButtonBind.unionVal0;
            result.value.hat.hat_mask = iNTERNAL_SDL_GameControllerButtonBind.unionVal1;
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerRumble(IntPtr gamecontroller, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerRumbleTriggers(IntPtr gamecontroller, ushort left_rumble, ushort right_rumble, uint duration_ms);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GameControllerClose(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetAppleSFSymbolsNameForButton")]
        private static extern IntPtr INTERNAL_SDL_GameControllerGetAppleSFSymbolsNameForButton(IntPtr gamecontroller, SDL_GameControllerButton button);

        public static string SDL_GameControllerGetAppleSFSymbolsNameForButton(IntPtr gamecontroller, SDL_GameControllerButton button)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerGetAppleSFSymbolsNameForButton(gamecontroller, button));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetAppleSFSymbolsNameForAxis")]
        private static extern IntPtr INTERNAL_SDL_GameControllerGetAppleSFSymbolsNameForAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis);

        public static string SDL_GameControllerGetAppleSFSymbolsNameForAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GameControllerGetAppleSFSymbolsNameForAxis(gamecontroller, axis));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerFromInstanceID(int joyid);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GameControllerType SDL_GameControllerTypeForIndex(int joystick_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_GameControllerType SDL_GameControllerGetType(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerFromPlayerIndex(int player_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GameControllerSetPlayerIndex(IntPtr gamecontroller, int player_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerHasLED(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerHasRumble(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerHasRumbleTriggers(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerSetLED(IntPtr gamecontroller, byte red, byte green, byte blue);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerHasAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerHasButton(IntPtr gamecontroller, SDL_GameControllerButton button);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerGetNumTouchpads(IntPtr gamecontroller);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerGetNumTouchpadFingers(IntPtr gamecontroller, int touchpad);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerGetTouchpadFinger(IntPtr gamecontroller, int touchpad, int finger, out byte state, out float x, out float y, out float pressure);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerHasSensor(IntPtr gamecontroller, SDL_SensorType type);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerSetSensorEnabled(IntPtr gamecontroller, SDL_SensorType type, SDL_bool enabled);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GameControllerIsSensorEnabled(IntPtr gamecontroller, SDL_SensorType type);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerGetSensorData(IntPtr gamecontroller, SDL_SensorType type, IntPtr data, int num_values);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerGetSensorData(IntPtr gamecontroller, SDL_SensorType type, [In] float[] data, int num_values);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GameControllerGetSensorDataRate(IntPtr gamecontroller, SDL_SensorType type);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerSendEffect(IntPtr gamecontroller, IntPtr data, int size);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HapticClose(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HapticDestroyEffect(IntPtr haptic, int effect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticEffectSupported(IntPtr haptic, ref SDL_HapticEffect effect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticGetEffectStatus(IntPtr haptic, int effect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticIndex(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_HapticName")]
        private static extern IntPtr INTERNAL_SDL_HapticName(int device_index);

        public static string SDL_HapticName(int device_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_HapticName(device_index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNewEffect(IntPtr haptic, ref SDL_HapticEffect effect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNumAxes(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNumEffects(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNumEffectsPlaying(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_HapticOpen(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticOpened(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_HapticOpenFromJoystick(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_HapticOpenFromMouse();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticPause(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_HapticQuery(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumbleInit(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumblePlay(IntPtr haptic, float strength, uint length);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumbleStop(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumbleSupported(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRunEffect(IntPtr haptic, int effect, uint iterations);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticSetAutocenter(IntPtr haptic, int autocenter);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticSetGain(IntPtr haptic, int gain);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticStopAll(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticStopEffect(IntPtr haptic, int effect);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticUnpause(IntPtr haptic);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticUpdateEffect(IntPtr haptic, int effect, ref SDL_HapticEffect data);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickIsHaptic(IntPtr joystick);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_MouseIsHaptic();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_NumHaptics();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_NumSensors();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SensorGetDeviceName")]
        private static extern IntPtr INTERNAL_SDL_SensorGetDeviceName(int device_index);

        public static string SDL_SensorGetDeviceName(int device_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_SensorGetDeviceName(device_index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SensorType SDL_SensorGetDeviceType(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetDeviceNonPortableType(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetDeviceInstanceID(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_SensorOpen(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_SensorFromInstanceID(int instance_id);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SensorGetName")]
        private static extern IntPtr INTERNAL_SDL_SensorGetName(IntPtr sensor);

        public static string SDL_SensorGetName(IntPtr sensor)
        {
            return UTF8_ToManaged(INTERNAL_SDL_SensorGetName(sensor));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SensorType SDL_SensorGetType(IntPtr sensor);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetNonPortableType(IntPtr sensor);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetInstanceID(IntPtr sensor);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetData(IntPtr sensor, float[] data, int num_values);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SensorClose(IntPtr sensor);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SensorUpdate();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockSensors();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSensors();

        public static ushort SDL_AUDIO_BITSIZE(ushort x)
        {
            return (ushort)(x & 0xFF);
        }

        public static bool SDL_AUDIO_ISFLOAT(ushort x)
        {
            return (x & 0x100) != 0;
        }

        public static bool SDL_AUDIO_ISBIGENDIAN(ushort x)
        {
            return (x & 0x1000) != 0;
        }

        public static bool SDL_AUDIO_ISSIGNED(ushort x)
        {
            return (x & 0x8000) != 0;
        }

        public static bool SDL_AUDIO_ISINT(ushort x)
        {
            return (x & 0x100) == 0;
        }

        public static bool SDL_AUDIO_ISLITTLEENDIAN(ushort x)
        {
            return (x & 0x1000) == 0;
        }

        public static bool SDL_AUDIO_ISUNSIGNED(ushort x)
        {
            return (x & 0x8000) == 0;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AudioInit")]
        private unsafe static extern int INTERNAL_SDL_AudioInit(byte* driver_name);

        public unsafe static int SDL_AudioInit(string driver_name)
        {
            int num = Utf8Size(driver_name);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_AudioInit(Utf8Encode(driver_name, buffer, num));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_AudioQuit();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseAudio();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseAudioDevice(uint dev);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeWAV(IntPtr audio_buf);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetAudioDeviceName")]
        private static extern IntPtr INTERNAL_SDL_GetAudioDeviceName(int index, int iscapture);

        public static string SDL_GetAudioDeviceName(int index, int iscapture)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetAudioDeviceName(index, iscapture));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_AudioStatus SDL_GetAudioDeviceStatus(uint dev);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetAudioDriver")]
        private static extern IntPtr INTERNAL_SDL_GetAudioDriver(int index);

        public static string SDL_GetAudioDriver(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetAudioDriver(index));
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_AudioStatus SDL_GetAudioStatus();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentAudioDriver")]
        private static extern IntPtr INTERNAL_SDL_GetCurrentAudioDriver();

        public static string SDL_GetCurrentAudioDriver()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetCurrentAudioDriver());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumAudioDevices(int iscapture);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumAudioDrivers();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_LoadWAV_RW(IntPtr src, int freesrc, out SDL_AudioSpec spec, out IntPtr audio_buf, out uint audio_len);

        public static IntPtr SDL_LoadWAV(string file, out SDL_AudioSpec spec, out IntPtr audio_buf, out uint audio_len)
        {
            return SDL_LoadWAV_RW(SDL_RWFromFile(file, "rb"), 1, out spec, out audio_buf, out audio_len);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockAudio();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockAudioDevice(uint dev);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MixAudio([Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] byte[] dst, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] byte[] src, uint len, int volume);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MixAudioFormat(IntPtr dst, IntPtr src, ushort format, uint len, int volume);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MixAudioFormat([Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)] byte[] dst, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)] byte[] src, ushort format, uint len, int volume);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_OpenAudio(ref SDL_AudioSpec desired, out SDL_AudioSpec obtained);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_OpenAudio(ref SDL_AudioSpec desired, IntPtr obtained);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_OpenAudioDevice(IntPtr device, int iscapture, ref SDL_AudioSpec desired, out SDL_AudioSpec obtained, int allowed_changes);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_OpenAudioDevice")]
        private unsafe static extern uint INTERNAL_SDL_OpenAudioDevice(byte* device, int iscapture, ref SDL_AudioSpec desired, out SDL_AudioSpec obtained, int allowed_changes);

        public unsafe static uint SDL_OpenAudioDevice(string device, int iscapture, ref SDL_AudioSpec desired, out SDL_AudioSpec obtained, int allowed_changes)
        {
            int num = Utf8Size(device);
            byte* buffer = stackalloc byte[(int)(uint)num];
            return INTERNAL_SDL_OpenAudioDevice(Utf8Encode(device, buffer, num), iscapture, ref desired, out obtained, allowed_changes);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PauseAudio(int pause_on);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PauseAudioDevice(uint dev, int pause_on);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockAudio();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockAudioDevice(uint dev);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_QueueAudio(uint dev, IntPtr data, uint len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_DequeueAudio(uint dev, IntPtr data, uint len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetQueuedAudioSize(uint dev);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClearQueuedAudio(uint dev);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_NewAudioStream(ushort src_format, byte src_channels, int src_rate, ushort dst_format, byte dst_channels, int dst_rate);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AudioStreamPut(IntPtr stream, IntPtr buf, int len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AudioStreamGet(IntPtr stream, IntPtr buf, int len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AudioStreamAvailable(IntPtr stream);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_AudioStreamClear(IntPtr stream);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeAudioStream(IntPtr stream);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAudioDeviceSpec(int index, int iscapture, out SDL_AudioSpec spec);

        public static bool SDL_TICKS_PASSED(uint A, uint B)
        {
            return (int)(B - A) <= 0;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Delay(uint ms);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetTicks();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetTicks64();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetPerformanceCounter();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SDL_GetPerformanceFrequency();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AddTimer(uint interval, SDL_TimerCallback callback, IntPtr param);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_RemoveTimer(int id);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowsMessageHook(SDL_WindowsMessageHook callback, IntPtr userdata);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderGetD3D9Device(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderGetD3D11Device(IntPtr renderer);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_iPhoneSetAnimationCallback(IntPtr window, int interval, SDL_iPhoneAnimationCallback callback, IntPtr callbackParam);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_iPhoneSetEventPump(SDL_bool enabled);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AndroidGetJNIEnv();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AndroidGetActivity();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsAndroidTV();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsChromebook();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsDeXMode();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_AndroidBackButton();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AndroidGetInternalStoragePath")]
        private static extern IntPtr INTERNAL_SDL_AndroidGetInternalStoragePath();

        public static string SDL_AndroidGetInternalStoragePath()
        {
            return UTF8_ToManaged(INTERNAL_SDL_AndroidGetInternalStoragePath());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AndroidGetExternalStorageState();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AndroidGetExternalStoragePath")]
        private static extern IntPtr INTERNAL_SDL_AndroidGetExternalStoragePath();

        public static string SDL_AndroidGetExternalStoragePath()
        {
            return UTF8_ToManaged(INTERNAL_SDL_AndroidGetExternalStoragePath());
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAndroidSDKVersion();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AndroidRequestPermission")]
        private unsafe static extern SDL_bool INTERNAL_SDL_AndroidRequestPermission(byte* permission);

        public unsafe static SDL_bool SDL_AndroidRequestPermission(string permission)
        {
            byte* intPtr = Utf8EncodeHeap(permission);
            SDL_bool result = INTERNAL_SDL_AndroidRequestPermission(intPtr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AndroidShowToast")]
        private unsafe static extern int INTERNAL_SDL_AndroidShowToast(byte* message, int duration, int gravity, int xOffset, int yOffset);

        public unsafe static int SDL_AndroidShowToast(string message, int duration, int gravity, int xOffset, int yOffset)
        {
            byte* intPtr = Utf8EncodeHeap(message);
            int result = INTERNAL_SDL_AndroidShowToast(intPtr, duration, gravity, xOffset, yOffset);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_WinRT_DeviceFamily SDL_WinRTGetDeviceFamily();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsTablet();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowWMInfo(IntPtr window, ref SDL_SysWMinfo info);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetBasePath")]
        private static extern IntPtr INTERNAL_SDL_GetBasePath();

        public static string SDL_GetBasePath()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetBasePath(), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPrefPath")]
        private unsafe static extern IntPtr INTERNAL_SDL_GetPrefPath(byte* org, byte* app);

        public unsafe static string SDL_GetPrefPath(string org, string app)
        {
            int num = Utf8Size(org);
            byte* buffer = stackalloc byte[(int)(uint)num];
            int num2 = Utf8Size(app);
            byte* buffer2 = stackalloc byte[(int)(uint)num2];
            return UTF8_ToManaged(INTERNAL_SDL_GetPrefPath(Utf8Encode(org, buffer, num), Utf8Encode(app, buffer2, num2)), true);
        }

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PowerState SDL_GetPowerInfo(out int secs, out int pct);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetCPUCount();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetCPUCacheLineSize();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasRDTSC();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasAltiVec();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasMMX();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Has3DNow();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSSE();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSSE2();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSSE3();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSSE41();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSSE42();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasAVX();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasAVX2();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasAVX512F();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasNEON();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSystemRAM();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_SIMDGetAlignment();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_SIMDAlloc(uint len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_SIMDRealloc(IntPtr ptr, uint len);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SIMDFree(IntPtr ptr);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasARMSIMD();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetPreferredLocales();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_OpenURL")]
        private unsafe static extern int INTERNAL_SDL_OpenURL(byte* url);

        public unsafe static int SDL_OpenURL(string url)
        {
            byte* intPtr = Utf8EncodeHeap(url);
            int result = INTERNAL_SDL_OpenURL(intPtr);
            Marshal.FreeHGlobal((IntPtr)intPtr);
            return result;
        }
    }

}
#pragma warning restore CS1591