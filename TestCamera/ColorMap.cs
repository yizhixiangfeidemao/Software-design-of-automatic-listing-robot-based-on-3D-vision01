﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRDLLDemo
{
    public enum SHOW_COLOR_MAP
    {
        GRAY = 0,
        SSZN_COLOR = 1,
        RRAINTABLE = 2,
        IRON = 3
    }

    /// <summary>
    /// 伪彩色图像构造器
    /// </summary>
    public class ColorMap
    {
        public static int SetLut(SHOW_COLOR_MAP lut)
        {

            try
            {
                switch (lut)
                {
                    case SHOW_COLOR_MAP.GRAY:
                        for (int i = 0; i < 256; i++)
                        {
                            nowTable[i, 0] = (byte)i;
                            nowTable[i, 1] = (byte)i;
                            nowTable[i, 2] = (byte)i;
                        }

                        break;
                    case SHOW_COLOR_MAP.IRON:
                        for (int i = 0; i < 256; i++)
                        {
                            nowTable[i, 0] = (byte)ironTable[i / 2, 0];
                            nowTable[i, 1] = (byte)ironTable[i / 2, 1];
                            nowTable[i, 2] = (byte)ironTable[i / 2, 2];
                        }

                        break;
                    case SHOW_COLOR_MAP.RRAINTABLE:
                        for (int i = 0; i < 256; i++)
                        {
                            nowTable[i, 0] = (byte)rainTable[i / 2, 0];
                            nowTable[i, 1] = (byte)rainTable[i / 2, 1];
                            nowTable[i, 2] = (byte)rainTable[i / 2, 2];
                        }

                        break;
                    case SHOW_COLOR_MAP.SSZN_COLOR:
                        for (int i = 0; i < 256; i++)
                        {

                            nowTable[i, 0] = (byte)(ColorMap.SSZNColor[i] & 0x000000ff);
                            nowTable[i, 1] = (byte)(byte)((ColorMap.SSZNColor[i] & 0x0000ff00) >> 8);
                            nowTable[i, 2] = (byte)((ColorMap.SSZNColor[i] & 0x00ff0000) >> 16);
                         
                        }

                        break;

                }

            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }

        #region Now

        public static byte[,] nowTable = new byte[256, 3];
        //{    { 0,0,0}, { 1,1,1}, { 2,2,2}, { 3,3,3}, { 4,4,4}, { 5,5,5}, { 6,6,6}, { 7,7,7},};

        #endregion Now

        // 铁红色带映射表
        #region iron
        public static byte[,] ironTable = new byte[128, 3] {
                {0,   0,  0},
                {0,   0,  0},
                {0,   0,  36},
                {0,   0,  51},
                {0,   0,  66},
                {0,   0,  81},
                {2,   0,  90},
                {4,   0,  99},
                {7,   0, 106},
                {11,   0, 115},
                {14,   0, 119},
                {20,   0, 123},
                {27,   0, 128},
                {33,   0, 133},
                {41,   0, 137},
                {48,   0, 140},
                {55,   0, 143},
                {61,   0, 146},
                {66,   0, 149},
                {72,   0, 150},
                {78,   0, 151},
                {84,   0, 152},
                {91,   0, 153},
                {97,   0, 155},
                {104,   0, 155},
                {110,   0, 156},
                {115,   0, 157},
                {122,   0, 157},
                {128,   0, 157},
                {134,   0, 157},
                {139,   0, 157},
                {146,   0, 156},
                {152,   0, 155},
                {157,   0, 155},
                {162,   0, 155},
                {167,   0, 154},
                {171,   0, 153},
                {175,   1, 152},
                {178,   1, 151},
                {182,   2, 149},
                {185,   4, 149},
                {188,   5, 147},
                {191,   6, 146},
                {193,   8, 144},
                {195,  11, 142},
                {198,  13, 139},
                {201,  17, 135},
                {203,  20, 132},
                {206,  23, 127},
                {208,  26, 121},
                {210,  29, 116},
                {212,  33, 111},
                {214,  37, 103},
                {217,  41,  97},
                {219,  46,  89},
                {221,  49,  78},
                {223,  53,  66},
                {224,  56,  54},
                {226,  60,  42},
                {228,  64,  30},
                {229,  68,  25},
                {231,  72,  20},
                {232,  76,  16},
                {234,  78,  12},
                {235,  82,  10},
                {236,  86,   8},
                {237,  90,   7},
                {238,  93,   5},
                {239,  96,   4},
                {240, 100,   3},
                {241, 103,   3},
                {241, 106,   2},
                {242, 109,   1},
                {243, 113,   1},
                {244, 116,   0},
                {244, 120,   0},
                {245, 125,   0},
                {246, 129,   0},
                {247, 133,   0},
                {248, 136,   0},
                {248, 139,   0},
                {249, 142,   0},
                {249, 145,   0},
                {250, 149,   0},
                {251, 154,   0},
                {252, 159,   0},
                {253, 163,   0},
                {253, 168,   0},
                {253, 172,   0},
                {254, 176,   0},
                {254, 179,   0},
                {254, 184,   0},
                {254, 187,   0},
                {254, 191,   0},
                {254, 195,   0},
                {254, 199,   0},
                {254, 202,   1},
                {254, 205,   2},
                {254, 208,   5},
                {254, 212,   9},
                {254, 216,  12},
                {255, 219,  15},
                {255, 221,  23},
                {255, 224,  32},
                {255, 227,  39},
                {255, 229,  50},
                {255, 232,  63},
                {255, 235,  75},
                {255, 238,  88},
                {255, 239, 102},
                {255, 241, 116},
                {255, 242, 134},
                {255, 244, 149},
                {255, 245, 164},
                {255, 247, 179},
                {255, 248, 192},
                {255, 249, 203},
                {255, 251, 216},
                {255, 253, 228},
                {255, 254, 239},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249} };

        #endregion iron

        // 彩虹色带映射表
        #region rain
        public static byte[,] rainTable = new byte[128, 3]
        {
            {0,   0,   0},
            {0,   0,   0},
            {15,   0,  15},
            {31,   0,  31},
            {47,   0,  47},
            {63,   0,  63},
            {79,   0,  79},
            {95,   0,  95},
            {111,   0, 111},
            {127,   0, 127},
            {143,   0, 143},
            {159,   0, 159},
            {175,   0, 175},
            {191,   0, 191},
            {207,   0, 207},
            {223,   0, 223},
            {239,   0, 239},
            {255,   0, 255},
            {239,   0, 250},
            {223,   0, 245},
            {207,   0, 240},
            {191,   0, 236},
            {175,   0, 231},
            {159,   0, 226},
            {143,   0, 222},
            {127,   0, 217},
            {111,   0, 212},
            {95,   0, 208},
            {79,   0, 203},
            {63,   0, 198},
            {47,   0, 194},
            {31,   0, 189},
            {15,   0, 184},
            {0,   0, 180},
            {0,  15, 184},
            {0,  31, 189},
            {0,  47, 194},
            {0,  63, 198},
            {0,  79, 203},
            {0,  95, 208},
            {0, 111, 212},
            {0, 127, 217},
            {0, 143, 222},
            {0, 159, 226},
            {0, 175, 231},
            {0, 191, 236},
            {0, 207, 240},
            {0, 223, 245},
            {0, 239, 250},
            {0, 255, 255},
            {0, 245, 239},
            {0, 236, 223},
            {0, 227, 207},
            {0, 218, 191},
            {0, 209, 175},
            {0, 200, 159},
            {0, 191, 143},
            {0, 182, 127},
            {0, 173, 111},
            {0, 164,  95},
            {0, 155,  79},
            {0, 146,  63},
            {0, 137,  47},
            {0, 128,  31},
            {0, 119,  15},
            {0, 110,   0},
            {15, 118,   0},
            {30, 127,   0},
            {45, 135,   0},
            {60, 144,   0},
            {75, 152,   0},
            {90, 161,   0},
            {105, 169,  0},
            {120, 178,  0},
            {135, 186,  0},
            {150, 195,  0},
            {165, 203,  0},
            {180, 212,  0},
            {195, 220,  0},
            {210, 229,  0},
            {225, 237,  0},
            {240, 246,  0},
            {255, 255,  0},
            {251, 240,  0},
            {248, 225,  0},
            {245, 210,  0},
            {242, 195,  0},
            {238, 180,  0},
            {235, 165,  0},
            {232, 150,  0},
            {229, 135,  0},
            {225, 120,  0},
            {222, 105,  0},
            {219,  90,  0},
            {216,  75,  0},
            {212,  60,  0},
            {209,  45,  0},
            {206,  30,  0},
            {203,  15,  0},
            {200,   0,  0},
            {202,  11,  11},
            {205,  23,  23},
            {207,  34,  34},
            {210,  46,  46},
            {212,  57,  57},
            {215,  69,  69},
            {217,  81,  81},
            {220,  92,  92},
            {222, 104, 104},
            {225, 115, 115},
            {227, 127, 127},
            {230, 139, 139},
            {232, 150, 150},
            {235, 162, 162},
            {237, 173, 173},
            {240, 185, 185},
            {242, 197, 197},
            {245, 208, 208},
            {247, 220, 220},
            {250, 231, 231},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243}
        };

        #endregion rain

        #region SSZNColor
        public static UInt32[] SSZNColor = new UInt32[256]{
  0x00000000, 0x000004ff, 0x000008ff, 0x00000dff, 0x000011ff, 0x000015ff, 0x000019ff, 0x00001eff,
    0x000022ff, 0x000026ff, 0x00002aff, 0x00002fff, 0x000033ff, 0x000037ff, 0x00003cff, 0x000040ff,
    0x000044ff, 0x000048ff, 0x00004dff, 0x000051ff, 0x000055ff, 0x000059ff, 0x00005eff, 0x000062ff,
    0x000066ff, 0x00006aff, 0x00006fff, 0x000073ff, 0x000077ff, 0x00007bff, 0x000080ff, 0x000084ff,
    0x000088ff, 0x00008cff, 0x000091ff, 0x000095ff, 0x000099ff, 0x00009dff, 0x0000a2ff, 0x0000a6ff,
    0x0000aaff, 0x0000aeff, 0x0000b3ff, 0x0000b7ff, 0x0000bbff, 0x0000bfff, 0x0000c4ff, 0x0000c8ff,
    0x0000ccff, 0x0000d0ff, 0x0000d5ff, 0x0000d9ff, 0x0000ddff, 0x0000e1ff, 0x0000e6ff, 0x0000eaff,
    0x0000eeff, 0x0000f2ff, 0x0000f7ff, 0x0000fbff, 0x0000ffff, 0x0000fffb, 0x0000fff6, 0x0000fff2,
    0x0000ffee, 0x0000ffea, 0x0000ffe5, 0x0000ffe1, 0x0000ffdd, 0x0000ffd9, 0x0000ffd4, 0x0000ffd0,
    0x0000ffcc, 0x0000ffc8, 0x0000ffc3, 0x0000ffbf, 0x0000ffbb, 0x0000ffb7, 0x0000ffb3, 0x0000ffae,
    0x0000ffaa, 0x0000ffa6, 0x0000ffa2, 0x0000ff9d, 0x0000ff99, 0x0000ff95, 0x0000ff91, 0x0000ff8c,
    0x0000ff88, 0x0000ff84, 0x0000ff80, 0x0000ff7b, 0x0000ff77, 0x0000ff73, 0x0000ff6f, 0x0000ff6a,
    0x0000ff66, 0x0000ff62, 0x0000ff5e, 0x0000ff59, 0x0000ff55, 0x0000ff51, 0x0000ff4d, 0x0000ff48,
    0x0000ff44, 0x0000ff40, 0x0000ff3c, 0x0000ff37, 0x0000ff33, 0x0000ff2f, 0x0000ff2b, 0x0000ff26,
    0x0000ff22, 0x0000ff1e, 0x0000ff1a, 0x0000ff15, 0x0000ff11, 0x0000ff0d, 0x0000ff09, 0x0000ff04,
    0x0000ff00, 0x0004ff00, 0x0008ff00, 0x000dff00, 0x0011ff00, 0x0015ff00, 0x001aff00, 0x001eff00,
    0x0022ff00, 0x0026ff00, 0x002aff00, 0x002fff00, 0x0033ff00, 0x0037ff00, 0x003cff00, 0x0040ff00,
    0x0044ff00, 0x0048ff00, 0x004cff00, 0x0051ff00, 0x0055ff00, 0x0059ff00, 0x005eff00, 0x0062ff00,
    0x0066ff00, 0x006aff00, 0x006eff00, 0x0073ff00, 0x0077ff00, 0x007bff00, 0x0080ff00, 0x0084ff00,
    0x0088ff00, 0x008cff00, 0x0091ff00, 0x0095ff00, 0x0099ff00, 0x009dff00, 0x00a2ff00, 0x00a6ff00,
    0x00aaff00, 0x00aeff00, 0x00b3ff00, 0x00b7ff00, 0x00bbff00, 0x00bfff00, 0x00c3ff00, 0x00c8ff00,
    0x00ccff00, 0x00d0ff00, 0x00d5ff00, 0x00d9ff00, 0x00ddff00, 0x00e1ff00, 0x00e5ff00, 0x00eaff00,
    0x00eeff00, 0x00f2ff00, 0x00f7ff00, 0x00fbff00, 0x00ffff00, 0x00fffb00, 0x00fff700, 0x00fff200,
    0x00ffee00, 0x00ffea00, 0x00ffe500, 0x00ffe100, 0x00ffdd00, 0x00ffd900, 0x00ffd500, 0x00ffd000,
    0x00ffcc00, 0x00ffc800, 0x00ffc300, 0x00ffbf00, 0x00ffbb00, 0x00ffb700, 0x00ffb300, 0x00ffae00,
    0x00ffaa00, 0x00ffa600, 0x00ffa200, 0x00ff9d00, 0x00ff9900, 0x00ff9500, 0x00ff9100, 0x00ff8c00,
    0x00ff8800, 0x00ff8400, 0x00ff8000, 0x00ff7b00, 0x00ff7700, 0x00ff7300, 0x00ff6e00, 0x00ff6a00,
    0x00ff6600, 0x00ff6200, 0x00ff5e00, 0x00ff5900, 0x00ff5500, 0x00ff5100, 0x00ff4c00, 0x00ff4800,
    0x00ff4400, 0x00ff4000, 0x00ff3c00, 0x00ff3700, 0x00ff3300, 0x00ff2f00, 0x00ff2a00, 0x00ff2600,
    0x00ff2200, 0x00ff1e00, 0x00ff1a00, 0x00ff1500, 0x00ff1100, 0x00ff0d00, 0x00ff0800, 0x00ff0400,
    0x00ff0000, 0x00ff0004, 0x00ff0008, 0x00ff000d, 0x00ff0011, 0x00ff0015, 0x00ff0019, 0x00ff001e,
    0x00ff0022, 0x00ff0026, 0x00ff002b, 0x00ff002f, 0x00ff0033, 0x00ff0037, 0x00ff003c, 0x00ff0040 };

        #endregion SSZNColor

    }

}