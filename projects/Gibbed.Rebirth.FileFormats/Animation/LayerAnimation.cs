﻿/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Rebirth.FileFormats.Animation
{
    public struct LayerAnimation
    {
        public uint LayerId;
        public bool Visible;
        public LayerFrame[] Frames;

        internal static LayerAnimation Read(Stream input, Endian endian)
        {
            var layerId = input.ReadValueU32(endian);
            var visible = input.ReadValueB8();

            var frameCount = input.ReadValueU32(endian);
            if (frameCount >= 5000)
            {
                throw new FormatException("too many layer frames");
            }
            var frames = new LayerFrame[frameCount];
            for (uint i = 0; i < frameCount; i++)
            {
                frames[i] = LayerFrame.Read(input, endian);
            }

            LayerAnimation instance;
            instance.LayerId = layerId;
            instance.Visible = visible;
            instance.Frames = frames;
            return instance;
        }

        internal void Write(Stream output, Endian endian)
        {
            Write(output, this, endian);
        }

        internal static void Write(Stream output, LayerAnimation instance, Endian endian)
        {
            output.WriteValueU32(instance.LayerId, endian);
            output.WriteValueB8(instance.Visible);

            var frameCount = instance.Frames == null ? 0 : instance.Frames.Length;
            output.WriteValueS32(frameCount, endian);
            if (instance.Frames != null)
            {
                foreach (var frame in instance.Frames)
                {
                    frame.Write(output, endian);
                }
            }
        }
    }
}
